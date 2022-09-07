using GeoChatter.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Meta data model for custom userscripts
    /// </summary>
    public sealed class UserScriptMeta : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// UserScript name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Script source name (editor or a URL)
        /// </summary>
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// Auto update URL
        /// </summary>
        public string UpdateURL { get; set; } = string.Empty;
        /// <summary>
        /// Wheter userscript is enabled
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>
        /// Wheter marked for auto updates
        /// </summary>
        public bool AutoUpdateEnabled { get; set; } = true;
        /// <summary>
        /// Creation time
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
        /// <summary>
        /// Last version update time
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Last edit date
        /// </summary>
        public DateTime LastEdited { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserScriptMeta()
        {
            LastUpdated = Created;
            LastEdited = Created;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Name = null;
                    Description = null;
                    Source = null;
                    UpdateURL = null;
                    AutoUpdateEnabled = false;
                    IsEnabled = false;
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
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class UserScriptUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// UserScript of which had a property updated
        /// </summary>
        public JSUserScript UserScript { get; }

        /// <summary>
        /// Old property value
        /// </summary>
        public object OldValue { get; }

        /// <summary>
        /// New property value
        /// </summary>
        public object NewValue { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="us">UserScript that was updated</param>
        /// <param name="propName">Name of property that was updated</param>
        /// <param name="oldval">Old value of <paramref name="propName"/></param>
        /// <param name="newval">New value of <paramref name="propName"/></param>
        public UserScriptUpdateEventArgs(JSUserScript us, string propName, object oldval, object newval)
        {
            UserScript = us;
            OldValue = oldval;
            NewValue = newval;
        }
    }

    /// <summary>
    /// JS UserScript model
    /// </summary>
    public sealed class JSUserScript : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Wrapper for executable script
        /// </summary>
        public JSWrapper Wrapper { get; set; }

        /// <summary>
        /// Name update event
        /// </summary>
        public event EventHandler<UserScriptUpdateEventArgs> NameChanged;

        /// <summary>
        /// Description update event
        /// </summary>
        public event EventHandler<UserScriptUpdateEventArgs> DescriptionChanged;

        /// <summary>
        /// Auto update URL change event
        /// </summary>
        public event EventHandler<UserScriptUpdateEventArgs> UpdateURLChanged;

        private string name = string.Empty;

        /// <summary>
        /// Name of the userscript
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                {
                    return;
                }

                NameChanged?.Invoke(this, new(this, nameof(Name), name, value));

                name = value;
            }
        }

        /// <summary>
        /// Set userscript <see cref="Name"/> but bypass <see cref="NameChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void SetNameDirect(string name)
        {
            this.name = name;
        }

        private string description = string.Empty;

        /// <summary>
        /// UserScript description
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                if (value == description)
                {
                    return;
                }

                DescriptionChanged?.Invoke(this, new(this, nameof(Description), description, value));

                description = value;
            }
        }

        /// <summary>
        /// Source of userscript (editor or URL)
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// UserScript version
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Non-null auto update url
        /// </summary>
        public string AutoUpdateURLString => AutoUpdateURL ?? string.Empty;

        /// <summary>
        /// Update auto update URL
        /// </summary>
        /// <param name="uri"></param>
        public void SetAutoUpdateURL(string uri)
        {
            if (uri == AutoUpdateURL)
            {
                return;
            }

            UpdateURLChanged?.Invoke(this, new(this, nameof(AutoUpdateURL), AutoUpdateURL, uri));

            AutoUpdateURL = uri;
        }

        /// <summary>
        /// Time userscript was created
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Last time userscript was updated to a newer version
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Last time script was edited
        /// </summary>
        public DateTime LastEdited { get; set; }

        /// <summary>
        /// Get auto update URL
        /// </summary>
        public string AutoUpdateURL { get; set; }

        /// <summary>
        /// Wheter userscript is enabled
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Wheter marked for auto updates
        /// </summary>
        public bool IsAutoUpdateEnabled { get; set; } = true;

        /// <summary>
        /// Wheter userscript is ready for execution
        /// </summary>
        public bool IsPrepared => Wrapper != null && !string.IsNullOrWhiteSpace(Wrapper.FullScript);

        /// <summary>
        /// Base pattern to find the container to search for other meta data
        /// </summary>
        public static Regex UserScriptBlock { get; } = new(@"\s*//\s*==UserScript==(.+)[\s\r\n]+//\s*==/UserScript==", RegexOptions.Singleline);

        /// <summary>
        /// Pattern of <see cref="Source"/>
        /// </summary>
        public static Regex SourceRegex { get; } = new(@"//\s*@[Ss]ource\s+([^\s\r\n]+)[\s\r\n]*");

        /// <summary>
        /// Pattern of <see cref="Version"/>
        /// </summary>
        public static Regex VersionRegex { get; } = new(@"//\s*@[Vv]ersion\s+([^\s\r\n]+)[\s\r\n]*");

        /// <summary>
        /// Pattern of <see cref="Description"/>
        /// </summary>
        public static Regex DescriptionRegex { get; } = new(@"//\s*@[Dd]escription\s+([^\r\n]+)[\s\r\n]*");

        /// <summary>
        /// Pattern of <see cref="AutoUpdateURL"/>
        /// </summary>
        public static Regex UpdateURLRegex { get; } = new(@"//\s*(?:@[Uu]pdate[Uu][Rr][Ll]|@[Uu]pdate)\s+([^\s\r\n]+)[\s\r\n]*");

        /// <summary>
        /// Pattern for extra run information
        /// </summary>
        public static Regex RunAtRegex { get; } = new(@"//\s*@[Rr]un-at\s+([^\s\r\n]+)[\s\r\n]*");

        /// <summary>
        /// Pattern of a greasyfork script main page
        /// </summary>
        public static Regex GreasyFork { get; } = new(@"https://greasyfork.org/\w+/scripts/([^/]+)(?:/code/user\.js|/code(?:/?))?$");

        /// <summary>
        /// Pattern of a openuser script main page
        /// </summary>
        public static Regex OpenUser { get; } = new(@"https://openuserjs.org/(?:src/)?scripts/([^/]+)/([^/\.]+)(?:/source(?:/?)|\.user\.js)?$");

        /// <summary>
        /// Source code url patterns and cleaners
        /// </summary>
        public static List<Func<string, string>> SourcePatterns { get; } = new()
        {
            (string url) =>
            {
                Match matchmain = GreasyFork.Match(url.Trim().Trim('/'));
                return matchmain == null
                || matchmain.Groups.Count != 2
                    ? null
                    : $"https://greasyfork.org/en/scripts/{matchmain.Groups[1].Value}/code/user.js"; },

            (string url) =>
            {
                Match matchmain = OpenUser.Match(url.Trim().Trim('/'));
                return matchmain == null
                || matchmain.Groups.Count != 3
                    ? null
                    : $"https://openuserjs.org/src/scripts/{matchmain.Groups[1].Value}/{matchmain.Groups[2].Value}.user.js"; }
        };

        /// <summary>
        /// Wheter to run the userscript before any other built-in wrapper
        /// </summary>
        public bool RunAtDocumentStart => Discover(RunAtRegex) == "document-start";

        /// <summary>
        /// 
        /// </summary>
        public JSUserScript()
        {
            LastUpdated = Created;
            LastEdited = Created;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jSWrapper"></param>
        public JSUserScript(JSWrapper jSWrapper) : this()
        {
            Wrapper = jSWrapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="type"></param>
        public JSUserScript(string name, string source, JSWrapperType type) : this(new JSWrapper(source, type))
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JSUserScript Copy()
        {
            if (Wrapper == null)
            {
                return new();
            }
            JSUserScript e = new(Wrapper.Copy())
            {
                IsEnabled = IsEnabled,
                Source = Source,
                Version = Version,
                IsAutoUpdateEnabled = IsAutoUpdateEnabled,
                Name = Name,
                Description = Description
            };
            e.SetAutoUpdateURL(AutoUpdateURLString);

            return e;
        }

        /// <summary>
        /// Discover information from raw script and set <see cref="Version"/>, <see cref="Source"/>, <see cref="AutoUpdateURL"/> and <see cref="Description"/>
        /// </summary>
        public void DiscoverAndSetAll()
        {
            Version = DiscoverVersion();
            Source = DiscoverSource();
            string a = DiscoverUpdateURL();
            if (!string.IsNullOrWhiteSpace(a))
            {
                SetAutoUpdateURL(a);
            }
            Description = DiscoverDescription();
        }

        /// <summary>
        /// Fix <paramref name="url"/> to be GET requestable raw JS script (.js) link
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFixedSourceURL(string url)
        {
            Func<string, string> func = SourcePatterns.FirstOrDefault(p => !string.IsNullOrEmpty(p(url)));
            return func != null
                ? func(url)
                : !string.IsNullOrWhiteSpace(url) && url.EndsWithDefault(".js")
                    ? url
                    : string.Empty;
        }

        /// <summary>
        /// Check for updates from <paramref name="updateUrl"/>, if new version is available update and return <see langword="true"/>, otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="updateUrl"></param>
        /// <returns></returns>
        public bool CheckForUpdates(string updateUrl)
        {
            updateUrl = GetFixedSourceURL(updateUrl);

            if (!updateUrl.EndsWithDefault(".js"))
            {
                return false;
            }

            using JSUserScript ext = new("_temp_", updateUrl, JSWrapperType.URL);
            bool valid = ext.PrepareForExecution();
            ext.DiscoverAndSetAll();

            if (!valid || string.IsNullOrWhiteSpace(ext.Version) || ext.Version == Version)
            {
                return false;
            }

            Wrapper = ext.Wrapper;
            Version = ext.Version;

            ext.Wrapper = null;

            LastUpdated = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Prepare <see cref="Wrapper"/> to be executable on demand (in case it is a not yet fetched URL)
        /// </summary>
        /// <returns></returns>
        public bool PrepareForExecution()
        {
            return Wrapper != null && Wrapper.PrepareForExecution();
        }

        /// <summary>
        /// Match pattern <paramref name="regex"/> in <see cref="UserScriptBlock"/> container of <see cref="Wrapper"/> and return it if anything matches
        /// </summary>
        /// <param name="regex">Pattern to match</param>
        /// <param name="block">Found <see cref="UserScriptBlock"/> container if any</param>
        /// <returns>Matched <paramref name="regex"/> in <paramref name="block"/> or <see langword="null"/></returns>
        public Match FindInUserScriptMeta(Regex regex, out Match block)
        {
            block = null;
            if (!IsPrepared || regex == null)
            {
                return null;
            }

            block = UserScriptBlock.Match(Wrapper.StoredRawScript);
            if (!block.Success)
            {
                return null;
            }

            Match match = regex.Match(block.Groups[1].Value.ToString());

            return match.Success ? match : null;
        }

        /// <summary>
        /// Get first match of <paramref name="regex"/> pattern found in <see cref="UserScriptBlock"/> container or <see cref="string.Empty"/>
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public string Discover(Regex regex)
        {
            if (regex == null || !IsPrepared)
            {
                return string.Empty;
            }

            Match block = UserScriptBlock.Match(Wrapper.StoredRawScript);
            if (!block.Success)
            {
                return string.Empty;
            }

            Match match = regex.Match(block.Groups[1].Value.ToString());

            return !match.Success ? string.Empty : match.Groups[1].Value;
        }

        /// <summary>
        /// Get <see cref="Source"/> by matching <see cref="SourceRegex"/> in <see cref="UserScriptBlock"/> container
        /// </summary>
        /// <returns></returns>
        public string DiscoverSource()
        {
            string res = Discover(SourceRegex);
            return string.IsNullOrWhiteSpace(res) ? Source : res;
        }

        /// <summary>
        /// Get <see cref="Version"/> by matching <see cref="VersionRegex"/> in <see cref="UserScriptBlock"/> container
        /// </summary>
        /// <returns></returns>
        public string DiscoverVersion()
        {
            string res = Discover(VersionRegex);
            return string.IsNullOrWhiteSpace(res) ? Version : res;
        }

        /// <summary>
        /// Get <see cref="Description"/> by matching <see cref="DescriptionRegex"/> in <see cref="UserScriptBlock"/> container
        /// </summary>
        /// <returns></returns>
        public string DiscoverDescription()
        {
            string res = Discover(DescriptionRegex);
            return string.IsNullOrWhiteSpace(res) ? Description : res;
        }

        /// <summary>
        /// Get <see cref="AutoUpdateURL"/> by matching <see cref="UpdateURLRegex"/> in <see cref="UserScriptBlock"/> container
        /// </summary>
        /// <returns></returns>
        public string DiscoverUpdateURL()
        {
            string res = Discover(UpdateURLRegex);
            return res;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Wrapper?.Dispose();
                    Name = null;
                    SetAutoUpdateURL(uri: null);
                    Version = null;
                    Description = null;
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
    }
}
