using CefSharp;
using CefSharp.Callback;
using GeoChatter.Helpers;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Interfaces;
using log4net;
using GeoChatter.Core.Handlers;
using System.Diagnostics;

namespace GeoChatter.Core.Handlers
{
    /// <summary>
    /// Resource data to be server over the GC scheme to requests
    /// </summary>
    public struct SchemeResource : IEquatable<SchemeResource>
    {
        /// <summary>
        /// Resource file path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Resource value
        /// </summary>
        public string Value { get; set; }

        internal static string GetPathFromSchemePath(string schemepath)
        {
            return schemepath.StartsWithDefault(GCSchemeHandler.SchemePrefix) ? schemepath[GCSchemeHandler.SchemePrefix.Length..] : schemepath;
        }

        /// <summary>
        /// Create a new resource unsafely 
        /// </summary>
        /// <param name="path">Scheme or file path</param>
        /// <returns><see cref="SchemeResource"/></returns>
        /// <exception cref="ArgumentNullException">If path is whitespace, empty or null</exception>
        public static SchemeResource Create(string path)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(path, nameof(path), "Invalid path given for resource.");

            path = GetPathFromSchemePath(path);
            string val = File.ReadAllText(path);
            return new SchemeResource() { Path = path, Value = val };
        }

        /// <summary>
        /// Try creating a new resource safely 
        /// </summary>
        /// <param name="path">Scheme or file path</param>
        /// <returns><see cref="SchemeResource"/> on success, otherwise <see langword="null"/></returns>
        public static SchemeResource? TryCreate(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return null;
                }

                path = GetPathFromSchemePath(path);
                string val = File.ReadAllText(path);
                return new SchemeResource() { Path = path, Value = val };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Check if <paramref name="obj"/> has the same <see cref="Value"/>
        /// <para>Does NOT compare <see cref="Path"/></para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is SchemeResource sc)
            {
                return sc.Value == Value;
            }
            else if (obj is string s)
            {
                return s == Value;
            }
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SchemeResource left, SchemeResource right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SchemeResource left, SchemeResource right)
        {
            return !(left == right);
        }

        /// <summary>
        /// See <see cref="Equals(object)"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SchemeResource other)
        {
            return Equals(obj: other);
        }
    }

    /// <summary>
    /// Scheme handler used for request resource handling
    /// </summary>
    public sealed class GCSchemeHandler : IResourceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(GCSchemeHandler));

        /// <summary>
        /// If <see langword="true"/>, local files are searched in case of a missing resource paths. Set to <see langword="false"/> to fail upon unknown resource path.
        /// </summary>
        public static bool EnableDynamicResources { get; set; } = true;

        /// <summary>
        /// Parameter to look for in the request url for refreshing the resource
        /// </summary>
        public static string RefreshParam { get; } = "refresh";

        /// <summary>
        /// Prefix used for requesting resources
        /// </summary>
        public const string SchemePrefix = GCSchemeHandlerFactory.SchemeName + "://";

        private static string CleanPath(string path)
        {
            return path.ReplaceDefault("\\", "/");
        }

        /// <summary>
        /// Get <paramref name="path"/> written in a scheme template
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MakeSchemePath([NotNull] string path)
        {
            path = path.TrimStart('.', '/', '\\');
            path = path.StartsWithDefault(SchemePrefix) ? path.ToLowerInvariant() : Path.Combine(SchemePrefix, path).ToLowerInvariant();
            return CleanPath(path);
        }

        private static KeyValuePair<string, SchemeResource> MakeResourcePair(string path, Func<string, string> schemePathFunc = null)
        {
            return schemePathFunc == null
                ? new KeyValuePair<string, SchemeResource>(MakeSchemePath(path), SchemeResource.Create(path))
                : new KeyValuePair<string, SchemeResource>(MakeSchemePath(schemePathFunc(path)), SchemeResource.Create(path));
        }

        private static IList<string> Paths { get; } = new List<string>()
        {
            Settings.Default.FlagStylesCustom,
            Settings.Default.FlagStyles,
            Settings.Default.FlagBase
        };

        /// <summary>
        /// Base CSS classes for flag icons
        /// </summary>
        public static string FlagBaseCSS => ResourceDictionary[MakeSchemePath(Settings.Default.FlagBase)].Value;

        /// <summary>
        /// Resource data
        /// </summary>
        public static IDictionary<string, SchemeResource> ResourceDictionary { get; } = Paths.Map(path => MakeResourcePair(path)).ToDictionary(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// MimeType to be used if none provided
        /// </summary>
        public const string DefaultMimeType = "text/html";

        /// <summary>
        /// We reuse a temp buffer where possible for copying the data from the stream
        /// into the output stream
        /// </summary>
        private byte[] tempBuffer;

        /// <summary>
        /// Gets or sets the Charset
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the resource stream.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the http status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets ResponseLength, when you know the size of your
        /// Stream (Response) set this property. This is optional.
        /// If you use a MemoryStream and don't provide a value
        /// here then it will be cast and its size used
        /// </summary>
        public long? ResponseLength { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers { get; private set; }

        /// <summary>
        /// When true the Stream will be Disposed when
        /// this instance is Disposed. The default value for
        /// this property is false.
        /// </summary>
        public bool AutoDisposeStream { get; set; }

        /// <summary>
        /// If the ErrorCode is set then the response will be ignored and
        /// the errorCode returned.
        /// </summary>
        public CefErrorCode? ErrorCode { get; set; }

        private static bool InitializedDict { get; set; }

        /// <summary>
        /// Separator comment for flag-icon from flag-icon-xx classes
        /// </summary>
        public const string FlagBaseCSSComment = "\r\n/*Flags*/\r\n";

        /// <summary>
        /// Flag css classes name prefix
        /// </summary>
        public const string FlagCodeCssClassPrefix = "flag-icon";

        /// <summary>
        /// Flag related css classes stylesheet
        /// </summary>
        public static string FlagCache => ResourceDictionary[MakeSchemePath(Settings.Default.FlagStyles)].Value.Split(FlagBaseCSSComment)[1];

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandler"/> class.
        /// </summary>
        /// <param name="mimeType">Optional mimeType defaults to <see cref="DefaultMimeType"/></param>
        /// <param name="stream">Optional Stream - must be set at some point to provide a valid response</param>
        /// <param name="autoDisposeStream">When true the Stream will be disposed when this instance is Disposed, you will
        /// be unable to use this ResourceHandler after the Stream has been disposed</param>
        /// <param name="charset">response charset</param>
        public GCSchemeHandler(string mimeType = DefaultMimeType, Stream stream = null, bool autoDisposeStream = false, string charset = null)
        {
            // Read SVG resources
            if (!InitializedDict)
            {
                InitializedDict = true;

                InitializeHandler();
            }

            GCUtils.ThrowIfNullEmptyOrWhiteSpace(mimeType, nameof(mimeType), "Please provide a valid mimeType");

            StatusCode = 200;
            StatusText = "OK";
            MimeType = mimeType;
            Headers = new NameValueCollection();
            Stream = stream;
            AutoDisposeStream = autoDisposeStream;
            Charset = charset;

            //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin
            //Potential workaround for requests coming from different scheme
            //e.g. request from https made to myScheme
            Headers.Add("Access-Control-Allow-Origin", "*");
        }

        private static void InitializeHandler()
        {
            string[] flags = Directory.GetFiles(CountryHelper.FlagMeta.TargetDirectory);
            flags.ForEach(path => ResourceDictionary.Add(MakeResourcePair(path, p => Path.Combine(CountryHelper.FlagMeta.TargetDirectory, Path.GetFileName(p)))));

            string pt = MakeSchemePath(Settings.Default.FlagStyles);
            string fcss = CreateFlagCSSClasses(flags);
            ResourceDictionary[pt] = new SchemeResource() { Path = pt, Value = FlagBaseCSS + FlagBaseCSSComment + fcss };
        }

        private static string FlagCodeToCSSClassName(string flagCode)
        {
            return $".{FlagCodeCssClassPrefix}-{flagCode.ToLowerInvariant()}";
        }

        private static string SvgToCSSBackground(string flagResourcePath, string rawsvg)
        {
            string flagCode = flagResourcePath.Split('/')[^1];
            flagCode = flagCode[..(flagCode.Length - 4)];

            return $"{FlagCodeToCSSClassName(flagCode)}{{background-image: url(\"data:image/svg+xml,{Uri.EscapeDataString(rawsvg)}\");}}";
        }

        /// <summary>
        /// Get css classes from given flag svg paths
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private static string CreateFlagCSSClasses([NotNull] string[] paths)
        {
            return string.Join(" ",
                ResourceDictionary
                    .Where(pair => pair.Key.StartsWithDefault(MakeSchemePath(CountryHelper.FlagMeta.TargetDirectory)) && pair.Key.EndsWithDefault(".svg"))
                    .Select(pair => SvgToCSSBackground(pair.Key, pair.Value.Value))
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="handleRequest"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool Open(IRequest request, out bool handleRequest, ICallback callback)
        {
            CefReturnValue processRequest = ProcessRequestAsync(request, callback);

            //Process the request in an async fashion
            if (processRequest == CefReturnValue.ContinueAsync)
            {
                handleRequest = false;

                return true;
            }
            else if (processRequest == CefReturnValue.Continue)
            {
                handleRequest = true;

                return true;
            }

            //Cancel Request
            handleRequest = true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesToSkip"></param>
        /// <param name="bytesSkipped"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
        {
            //No Stream or Stream cannot seek then we indicate failure
            if (Stream == null || !Stream.CanSeek)
            {
                //Indicate failure
                bytesSkipped = -2;

                return false;
            }

            bytesSkipped = bytesToSkip;

            Stream.Seek(bytesToSkip, SeekOrigin.Current);

            //If data is available immediately set bytesSkipped to the number of of bytes skipped and return true.
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataOut"></param>
        /// <param name="bytesRead"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
        {

            bytesRead = 0;

            //We don't need the callback, as it's an unmanaged resource we should dispose it (could wrap it in a using statement).
            callback?.Dispose();

            if (dataOut == null || Stream == null)
            {
                return false;
            }

            //Data out represents an underlying unmanaged buffer (typically 64kb in size).
            //We reuse a temp buffer where possible
            if (tempBuffer == null || tempBuffer.Length < dataOut.Length)
            {
                tempBuffer = new byte[dataOut.Length];
            }

            //Only read the number of bytes that can be written to dataOut
            bytesRead = Stream.Read(tempBuffer, 0, (int)dataOut.Length);

            // To indicate response completion set bytesRead to 0 and return false
            if (bytesRead == 0)
            {
                return false;
            }

            //We need to use bytesRead instead of tempbuffer.Length otherwise
            //garbage from the previous copy would be written to dataOut
            dataOut.Write(tempBuffer, 0, bytesRead);

            return bytesRead > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseLength"></param>
        /// <param name="redirectUrl"></param>
        public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            redirectUrl = null;
            responseLength = -1;

            if (response == null)
            {
                return;
            }

            response.MimeType = MimeType;
            response.StatusCode = StatusCode;
            response.StatusText = StatusText;
            response.Headers = Headers;

            if (!string.IsNullOrEmpty(Charset))
            {
                response.Charset = Charset;
            }

            if (ResponseLength.HasValue)
            {
                responseLength = ResponseLength.Value;
            }

            if (Stream != null && Stream.CanSeek)
            {
                //ResponseLength property has higher precedence over Stream.Length
                if (ResponseLength == null || responseLength == 0)
                {
                    //If no ResponseLength provided then attempt to infer the length
                    responseLength = Stream.Length;
                }

                Stream.Position = 0;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public void Cancel()
        {
            // Prior to Prior to https://bitbucket.org/chromiumembedded/cef/commits/90301bdb7fd0b32137c221f38e8785b3a8ad8aa4
            // This method was unexpectedly being called during Read (from a different thread),
            // changes to the threading model were made and I haven't confirmed if this is still
            // the case.
            // 
            // The documentation for Cancel is vague and there aren't any examples that
            // illustrate its intended use case so for now we'll just keep things
            // simple and free our resources in Dispose
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ProcessRequest(IRequest request, ICallback callback)
        {
            throw new NotImplementedException("This method was deprecated and is no longer used.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataOut"></param>
        /// <param name="bytesRead"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            throw new NotImplementedException("This method was deprecated and is no longer used.");
        }

        /// <summary>
        /// Begin processing the request. If you have the data in memory you can execute the callback
        /// immediately and return true. For Async processing you would typically spawn a Task to perform processing,
        /// then return true. When the processing is complete execute callback.Continue(); In your processing Task, simply set
        /// the StatusCode, StatusText, MimeType, ResponseLength and Stream
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>To handle the request return true and call
        /// <see cref="ICallback.Continue"/> once the response header information is available
        /// <see cref="ICallback.Continue"/> can also be called from inside this method if
        /// header information is available immediately).
        /// To cancel the request return false.</returns>
        public CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            if (request == null)
            {
                return CefReturnValue.Continue;
            }

            string fileName = request.Url.ToLowerInvariant();
            string[] parameters = fileName.Split('?');
            fileName = CleanPath(parameters[0]);

            bool refresh = parameters.Length > 1
                && parameters[1].StartsWithDefault(RefreshParam);

            bool valid = ResourceDictionary.TryGetValue(fileName, out SchemeResource resource);

            if (refresh)
            {
                if (valid)
                {
                    resource = SchemeResource.Create(resource.Path);
                    ResourceDictionary[fileName] = resource;
                }
                else if (EnableDynamicResources)
                {
                    SchemeResource? res = SchemeResource.TryCreate(fileName);
                    valid = res != null;
                    if (valid)
                    {
                        resource = (SchemeResource)res;
                        ResourceDictionary.Add(fileName, resource);
                    }
                }
            }
            else if (!valid && EnableDynamicResources)
            {
                SchemeResource? res = SchemeResource.TryCreate(fileName);
                valid = res != null;
                if (valid)
                {
                    resource = (SchemeResource)res;
                    ResourceDictionary.Add(fileName, resource);
                }
            }

            if (valid)
            {
                using (callback)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(resource.Value);
                    Stream = new MemoryStream(bytes);

                    string fileExtension = Path.GetExtension(fileName);
                    MimeType = ResourceHandler.GetMimeType(fileExtension);

                    callback?.Continue();
                }

                return CefReturnValue.Continue;
            }
            else
            {
                Debug.WriteLine("ERROR: Failed to handle scheme request = " + fileName);
                callback?.Dispose();
            }

            return CefReturnValue.Cancel;
        }

        /// <summary>
        /// Gets the resource from the file path specified. Use the Cef.GetMimeType()
        /// helper method to lookup the mimeType if required. Uses CefStreamResourceHandler for reading the data
        /// </summary>
        /// <param name="filePath">Location of the file.</param>
        /// <param name="mimeType">The mimeType if null then text/html is used.</param>
        /// <param name="autoDisposeStream">Dispose of the stream when finished with (you will only be able to serve one
        /// request).</param>
        /// <returns>IResourceHandler.</returns>
        public static IResourceHandler FromFilePath(string filePath, string mimeType = null, bool autoDisposeStream = false)
        {
            FileStream stream = File.OpenRead(filePath);

            return FromStream(stream, mimeType ?? DefaultMimeType, autoDisposeStream);
        }

        /// <summary>
        /// Creates a IResourceHandler that represents a Byte[], uses CefStreamResourceHandler for reading the data
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="mimeType">mimeType</param>
        /// <param name="charSet">response charset</param>
        /// <returns>IResourceHandler</returns>
        public static IResourceHandler FromByteArray(byte[] data, string mimeType = null, string charSet = null)
        {
            return new ByteArrayResourceHandler(mimeType ?? DefaultMimeType, data);
        }

        /// <summary>
        /// Gets the resource from the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>ResourceHandler.</returns>
        [Obsolete("Use ResourceHandler.FromString(resource, mimeType: Cef.GetMimeType(fileExtension)); instead, this method will be removed")]
        public static IResourceHandler FromString(string text, string fileExtension)
        {
            string mimeType = GetMimeType(fileExtension);
            return FromString(text, Encoding.UTF8, false, mimeType);
        }

        /// <summary>
        /// Gets a <see cref="ResourceHandler"/> that represents a string.
        /// Without a Preamble, Cef will use BrowserSettings.DefaultEncoding to load the html.
        /// </summary>
        /// <param name="text">The html string</param>
        /// <param name="encoding">Character Encoding</param>
        /// <param name="includePreamble">Include encoding preamble</param>
        /// <param name="mimeType">Mime Type</param>
        /// <returns>ResourceHandler</returns>
        public static IResourceHandler FromString(string text, Encoding encoding = null, bool includePreamble = true, string mimeType = DefaultMimeType)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return new ByteArrayResourceHandler(mimeType, GetByteArray(text, encoding, includePreamble));
        }

        /// <summary>
        /// Generates a ResourceHandler that has it's StatusCode set
        /// </summary>
        /// <param name="errorMessage">Body the response to be displayed</param>
        /// <param name="statusCode">StatusCode</param>
        /// <returns>ResourceHandler</returns>
        public static IResourceHandler ForErrorMessage(string errorMessage, HttpStatusCode statusCode)
        {
            MemoryStream stream = GetMemoryStream(errorMessage, Encoding.UTF8);

            ResourceHandler resourceHandler = FromStream(stream);
            resourceHandler.StatusCode = (int)statusCode;

            return resourceHandler;
        }

        /// <summary>
        /// Gets the resource from a stream.
        /// </summary>
        /// <param name="stream">A stream of the resource.</param>
        /// <param name="mimeType">Type of MIME.</param>
        /// <param name="autoDisposeStream">Dispose of the stream when finished with (you will only be able to serve one
        /// request).</param>
        /// <param name="charSet">response charset</param>
        /// <returns>ResourceHandler.</returns>
        public static ResourceHandler FromStream(Stream stream, string mimeType = DefaultMimeType, bool autoDisposeStream = false, string charSet = null)
        {
            return new ResourceHandler(mimeType, stream, autoDisposeStream, charSet);
        }

        /// <summary>
        /// Gets a MemoryStream from the given string using the provided encoding
        /// </summary>
        /// <param name="text">string to be converted to a stream</param>
        /// <param name="encoding">encoding</param>
        /// <param name="includePreamble">if true a BOM will be written to the beginning of the stream</param>
        /// <returns>A memory stream from the given string</returns>
        public static MemoryStream GetMemoryStream(string text, Encoding encoding, bool includePreamble = true)
        {
            encoding ??= Encoding.Default;

            if (includePreamble)
            {
                byte[] preamble = encoding.GetPreamble();
                byte[] bytes = encoding.GetBytes(text);

                MemoryStream memoryStream = new(preamble.Length + bytes.Length);

                memoryStream.Write(preamble, 0, preamble.Length);
                memoryStream.Write(bytes, 0, bytes.Length);

                memoryStream.Position = 0;

                return memoryStream;
            }

            return new MemoryStream(encoding.GetBytes(text));
        }

        /// <summary>
        /// Gets a byteArray from the given string using the provided encoding
        /// </summary>
        /// <param name="text">string to be converted to a stream</param>
        /// <param name="encoding">encoding. If <see langword="null"/>, uses <see cref="Encoding.Default"/></param>
        /// <param name="includePreamble">if true a BOM will be written to the beginning of the stream</param>
        /// <returns>A memory stream from the given string</returns>
        public static byte[] GetByteArray(string text, Encoding encoding, bool includePreamble = true)
        {
            encoding ??= Encoding.Default;

            if (includePreamble)
            {
                byte[] preamble = encoding.GetPreamble();
                byte[] bytes = encoding.GetBytes(text);

                using MemoryStream memoryStream = new(preamble.Length + bytes.Length);

                memoryStream.Write(preamble, 0, preamble.Length);
                memoryStream.Write(bytes, 0, bytes.Length);

                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }

            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Gets the MIME type of the content.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">extension</exception>
        /// <remarks>In most cases it's preferable to use Cef.GetMimeType(extension); instead. See https://github.com/cefsharp/CefSharp/issues/3041 for details.</remarks>
        public static string GetMimeType(string extension)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(extension, nameof(extension));

            if (!extension.StartsWithDefault("."))
            {
                extension = "." + extension;
            }

            return ResourceDictionary.TryGetValue(extension, out SchemeResource mime) ? mime.Value : "application/octet-stream";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (AutoDisposeStream && Stream != null)
                {
                    Stream.Dispose();
                }
                Stream = null;
                tempBuffer = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseLength"></param>
        /// <param name="redirectUrl"></param>
        public void GetResponseHeaders(IResponse response, out long responseLength, out Uri redirectUrl)
        {
            GetResponseHeaders(response, out responseLength, out string url);
            redirectUrl = new(url ?? string.Empty);
        }
    }
}
