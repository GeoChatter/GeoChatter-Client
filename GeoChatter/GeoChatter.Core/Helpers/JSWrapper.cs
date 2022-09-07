using CefSharp;
using GeoChatter.Core.Common.Extensions;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Wrapper type: Local, URL, Raw
    /// </summary>
    public enum JSWrapperType
    {
        /// <summary>
        /// For a JS file relative path to "/<see cref="JSWrapper.ScriptsDirectory"/>" directory
        /// </summary>
        Local,
        /// <summary>
        /// For a JS script URL
        /// </summary>
        URL,
        /// <summary>
        /// Raw JS code
        /// </summary>
        Raw,
        /// <summary>
        /// Test script relative path to "/<see cref="JSWrapper.ScriptsDirectory"/>"
        /// <para>These scripts are only executed for debug builds</para>
        /// </summary>
        Test
    }

    /// <summary>
    /// <see cref="JSWrapper"/> execution times
    /// </summary>
    public enum JSWrapperTarget
    {
        /// <summary>
        /// Execute the script once on launch
        /// </summary>
        Launch,

        /// <summary>
        /// Execute the script everytime on adress-change
        /// </summary>
        All
    }

    /// <summary>
    /// Wrapper for tracking JS script files
    /// <para>See <see cref="DependencyPattern"/> patterns for including external libraries in scripts</para>
    /// </summary>
    public sealed class JSWrapper : IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JSWrapper));

        #region Fields / Properties
        /// <summary>
        /// Type of the wrapper for treating <see cref="Source"/> 
        /// </summary>
        public JSWrapperType Type { get; } = JSWrapperType.Raw;

        /// <summary>
        /// When to execute the script
        /// </summary>
        public JSWrapperTarget Target { get; } = JSWrapperTarget.Launch;

        /// <summary>
        /// Unique script id
        /// </summary>
        public Guid ID { get; } = Guid.NewGuid();

        /// <summary>
        /// Source code absolute path or URL
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Path for devtools
        /// </summary>
        public string RealPath { get; set; }

        /// <summary>
        /// Targeted frame for code execution
        /// </summary>
        public IFrame ExecutionFrame { get; set; }

        /// <summary>
        /// WebClient instance for getting external scripts
        /// </summary>
        private static HttpClient client { get; } = new HttpClient();

        /// <summary>
        /// JS scripts directory name
        /// </summary>
        public const string ScriptsDirectory = "Scripts";

        /// <summary>
        /// Dependency scripts to execute before the script
        /// </summary>
        public string DependencyScopes { get; set; } = string.Empty;

        /// <summary>
        /// Raw script without dependecies injected
        /// </summary>
        public string StoredRawScript { get; set; } = string.Empty;

        /// <summary>
        /// Full executable script with dependency scripts on top
        /// </summary>
        public string FullScript
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DependencyScopes))
                {
                    RegisterDependencies(StoredRawScript);
                }
                return DependencyScopes + StoredRawScript;
            }
        }

        private bool disposedValue;
        #endregion

        #region Events
        /// <summary>
        /// Fired when <see cref="Source"/> path is invalidated
        /// </summary>
        public event EventHandler OnSourceInvalid;

        /// <summary>
        /// Fired at the beginning of <see cref="Execute(IFrame)"/> call
        /// </summary>
        public event EventHandler OnExecuteStart;

        /// <summary>
        /// Fired at the end of <see cref="Execute(IFrame)"/> call before it returns
        /// </summary>
        public event EventHandler OnExecuteEnd;
        #endregion

        #region Constructors
        /// <summary>
        /// Wrap a javascript provider URL without a frame
        /// </summary>
        /// <param name="url">Javascript file URL, must accept GET requests</param>
        /// <param name="target">When to execute the script</param>
        public JSWrapper(string url, JSWrapperTarget target = JSWrapperTarget.Launch) : this(null, url, JSWrapperType.URL, target) { }

        /// <summary>
        /// Wrap a javascript file or code without a frame
        /// </summary>
        /// <param name="source">JS file name or raw JS code</param>
        /// <param name="type">Wrapper type to use with <paramref name="source"/></param>
        /// <param name="target">When to execute the script</param>
        public JSWrapper(string source, JSWrapperType type, JSWrapperTarget target = JSWrapperTarget.Launch) : this(null, source, type, target) { }

        /// <summary>
        /// Wrap a javascript source with given wrapper <paramref name="type"/> to be executed on the <paramref name="frame"/>
        /// </summary>
        /// <param name="frame">Frame to use for script execution</param>
        /// <param name="source">Javascript code provider source</param>
        /// <param name="type">Wrapper type to use with <paramref name="source"/></param>
        /// <param name="target">When to execute the script</param>
        public JSWrapper(IFrame frame, string source, JSWrapperType type, JSWrapperTarget target = JSWrapperTarget.Launch)
        {
            Type = type;
            if (source != null && (type == JSWrapperType.Local || type == JSWrapperType.Test))
            {
                RealPath = Path.Combine(Directory.GetCurrentDirectory(), ScriptsDirectory, Path.ChangeExtension(source, ".js"));
                source = RealPath;
            }

            Source = source;
            ExecutionFrame = frame;
            Target = target;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Add UserScript meta data to <see cref="StoredRawScript"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="version"></param>
        /// <param name="desc"></param>
        /// <param name="updateurl"></param>
        public void AddMeta(string source, string version, string desc, string updateurl)
        {
            if (!StoredRawScript.StartsWithDefault("// ==UserScript=="))
            {
                StoredRawScript = @"// ==UserScript==
// @source " + source + @"
// @version " + version + @"
// @description " + desc + @"
// @update " + updateurl + @"
// ==/UserScript==
" + StoredRawScript;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JSWrapper Copy()
        {
            return new JSWrapper(ExecutionFrame, Source, Type, Target)
            {
                DependencyScopes = DependencyScopes,
                StoredRawScript = StoredRawScript
            };
        }

        private bool IsValidLocalPath()
        {
            return !string.IsNullOrWhiteSpace(Source)
                && Source.EndsWithDefault(".js")
                && File.Exists(Source);
        }

        private string RequestTextFromUrl(string url, out bool success)
        {
            success = false;
            if (client == null)
            {
                return string.Empty;
            }

            try
            {
                logger.Debug(@"Retrieving url: " + url);
                string script = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                script = RegisterDependencies(script);
                success = true;
                return script;
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// Dependency patterns. Patterns are matched over given script and the text from {URL} is inserted
        /// above of the original script
        /// <para>// @require {URL}</para>
        /// <para>// @dependency {URL}</para>
        /// </summary>
        private static Regex DependencyPattern { get; } = new Regex(@"//\s*@(?:[Rr]equire|[Dd]ependency)\s+(.*)");

        /// <summary>
        /// Insert dependencies
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private string RegisterDependencies(string script)
        {
            MatchCollection matches = DependencyPattern.Matches(script);
            string depsPart = string.Empty;
            if (matches.Count > 0)
            {
                string deps = string.Empty;
                foreach (Match match in matches)
                {
                    string url = match.Groups[1].Value.TrimEnd('\r', '\n', '\t');
                    string depscript = MakeScoped(RequestTextFromUrl(url, out bool success));

                    if (success && !string.IsNullOrWhiteSpace(depscript))
                    {
                        deps += depscript + "\n";
                    }
                }
                depsPart += MakeScoped(deps);
            }
            DependencyScopes = depsPart;
            return script;
        }

        /// <summary>
        /// Wrap the script with a new scope 
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private static string MakeScoped(string script)
        {
            return string.IsNullOrWhiteSpace(script) ? string.Empty : $"(() => {{\n{script}\n}})();";
        }

        /// <summary>
        /// Prepare <see cref="StoredRawScript"/> for the instance. Returns the success state.
        /// </summary>
        /// <returns></returns>
        public bool PrepareForExecution()
        {
            if (!string.IsNullOrWhiteSpace(StoredRawScript))
            {
                return true;
            }

            switch (Type)
            {
#if DEBUG
                case JSWrapperType.Test:
#endif
                case JSWrapperType.Local:
                    {
                        if (IsValidLocalPath())
                        {
                            try
                            {
                                StoredRawScript = File.ReadAllText(Source);
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                        {
                            FireSourceInvalid();
                            return false;
                        }
                    }
                case JSWrapperType.Raw:
                    {
                        StoredRawScript = RegisterDependencies(Source);
                        return true;
                    }
                case JSWrapperType.URL:
                    {
                        try
                        {
                            StoredRawScript = RequestTextFromUrl(Source, out bool success);
                            return success;
                        }
                        catch
                        {
                            return false;
                        }
                    }
            }

            return false;
        }

        private string GetPrepearedScript(Func<string> fallback)
        {
            return string.IsNullOrWhiteSpace(FullScript) ? fallback() : FullScript;
        }

        private static void Execute(IFrame frame, string script, string path)
        {
            frame?.ExecuteJavaScriptAsync(script, path);
        }

        /// <summary>
        /// Execute <see cref="Source"/> script in <see cref="ExecutionFrame"/>
        /// </summary>
        public void Execute(IFrame frame = null)
        {
            if (frame == null)
            {
                frame = ExecutionFrame;
            }

            if (ExecutionFrame == null)
            {
                ExecutionFrame = frame;
            }

            FireExecuteStart();
            bool shouldExec = false;
            try
            {
                switch (Type)
                {
#if DEBUG
                    case JSWrapperType.Test:
#endif
                    case JSWrapperType.Local:
                        {
                            if (IsValidLocalPath())
                            {
                                string script = GetPrepearedScript(() => { return File.ReadAllText(Source); });
                                StoredRawScript = RegisterDependencies(script);
                                shouldExec = true;
                            }
                            else
                            {
                                FireSourceInvalid();
                            }
                            break;
                        }
                    case JSWrapperType.Raw:
                        {
                            StoredRawScript = RegisterDependencies(GetPrepearedScript(() => { return Source; }));
                            shouldExec = true;
                            break;
                        }
                    case JSWrapperType.URL:
                        {
                            GetPrepearedScript(() => { return RequestTextFromUrl(Source, out bool success); });
                            shouldExec = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
            if (shouldExec)
            {
                const string wrap = @"// --- Wrapper begin
console.log('Script execution...', performance.now());

if(!window.unsafeWindow) window.unsafeWindow = window;

// Source script below
{0}
// --- Wrapper end
";
                Execute(frame, string.IsNullOrWhiteSpace(RealPath) ? wrap.FormatDefault(FullScript) : FullScript, RealPath);
            }
            FireExecuteEnd();
        }

        #region Event Fire Methods
        private void FireExecuteStart(EventArgs args = null)
        {
            OnExecuteStart?.Invoke(this, args);
        }
        private void FireExecuteEnd(EventArgs args = null)
        {
            OnExecuteEnd?.Invoke(this, args);
        }
        private void FireSourceInvalid(EventArgs args = null)
        {
            OnSourceInvalid?.Invoke(this, args);
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StoredRawScript = null;
                    DependencyScopes = null;
                    ExecutionFrame = null;
                    Source = null;
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
