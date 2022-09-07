namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Class for setting up JS binds
    /// </summary>
    public class JSBind
    {
        /// <summary>
        /// Name to use while referencing from JS
        /// </summary>
        public string NameInJS { get; set; }

        /// <summary>
        /// An instance of the object to bind
        /// </summary>
        public object ObjectToBind { get; set; }

        /// <summary>
        /// Extra binding options
        /// </summary>
        public CefSharp.BindingOptions Options { get; set; }

        /// <summary>
        /// Create a bind ready to be registered to a given repo later
        /// </summary>
        /// <param name="name">Name to use while referencing from JS</param>
        /// <param name="objectToBind">An instance of the object to bind</param>
        /// <param name="options">Extra binding options</param>
        public JSBind(string name, object objectToBind, CefSharp.BindingOptions options = null)
        {
            NameInJS = name;
            ObjectToBind = objectToBind;

            Options = options ?? CefSharp.BindingOptions.DefaultBinder;
        }

        /// <summary>
        /// Bind self to given object repo
        /// </summary>
        /// <param name="repo"></param>
        public void Register(CefSharp.IJavascriptObjectRepository repo)
        {
            repo?.Register(NameInJS, ObjectToBind, Options);
        }
    }
}
