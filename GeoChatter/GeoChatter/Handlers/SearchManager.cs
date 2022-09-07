using ScintillaNET;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace GeoChatter.Handlers
{
    internal static class SearchManager
    {
        internal static Scintilla TextArea;
        internal static TextBox SearchBox;

        internal static string LastSearch = "";

        internal static int LastSearchIndex;

        [SupportedOSPlatform("windows7.0")]
        public static void Find(bool next, bool incremental)
        {
            LastSearch = SearchBox.Text;
            if (LastSearch.Length > 0)
            {

                if (next)
                {

                    // SEARCH FOR THE NEXT OCCURANCE

                    // Search the document at the last search index
                    TextArea.TargetStart = LastSearchIndex - 1;
                    TextArea.TargetEnd = LastSearchIndex + LastSearch.Length + 1;
                    TextArea.SearchFlags = SearchFlags.None;

                    // Search, and if not found..
                    if (!incremental || TextArea.SearchInTarget(LastSearch) == -1)
                    {

                        // Search the document from the caret onwards
                        TextArea.TargetStart = TextArea.CurrentPosition;
                        TextArea.TargetEnd = TextArea.TextLength;
                        TextArea.SearchFlags = SearchFlags.None;

                        // Search, and if not found..
                        if (TextArea.SearchInTarget(LastSearch) == -1)
                        {

                            // Search again from top
                            TextArea.TargetStart = 0;
                            TextArea.TargetEnd = TextArea.TextLength;

                            // Search, and if not found..
                            if (TextArea.SearchInTarget(LastSearch) == -1)
                            {

                                // clear selection and exit
                                TextArea.ClearSelections();
                                return;
                            }
                        }

                    }

                }
                else
                {

                    // SEARCH FOR THE PREVIOUS OCCURANCE

                    // Search the document from the beginning to the caret
                    TextArea.TargetStart = 0;
                    TextArea.TargetEnd = TextArea.CurrentPosition;
                    TextArea.SearchFlags = SearchFlags.None;

                    // Search, and if not found..
                    if (TextArea.SearchInTarget(LastSearch) == -1)
                    {

                        // Search again from the caret onwards
                        TextArea.TargetStart = TextArea.CurrentPosition;
                        TextArea.TargetEnd = TextArea.TextLength;

                        // Search, and if not found..
                        if (TextArea.SearchInTarget(LastSearch) == -1)
                        {

                            // clear selection and exit
                            TextArea.ClearSelections();
                            return;
                        }
                    }

                }

                // Select the occurance
                LastSearchIndex = TextArea.TargetStart;
                TextArea.SetSelection(TextArea.TargetEnd, TextArea.TargetStart);
                TextArea.ScrollCaret();

            }

            SearchBox.Focus();
        }


    }
}
