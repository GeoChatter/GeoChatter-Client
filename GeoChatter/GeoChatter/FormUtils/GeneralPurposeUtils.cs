using GeoChatter.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GeoChatter.FormUtils
{
    class SetClipboardHelper : StaHelper
    {
        readonly string _format;
        readonly object _data;

        public SetClipboardHelper(string format, object data)
        {
            _format = format;
            _data = data;
        }

        protected override void Work()
        {
            var obj = new System.Windows.Forms.DataObject(
                _format,
                _data
            );

            Clipboard.SetDataObject(obj, true);
        }
    }
    abstract class StaHelper
    {
        readonly ManualResetEvent _complete = new ManualResetEvent(false);

        public void Go()
        {
            var thread = new Thread(new ThreadStart(DoWork))
            {
                IsBackground = true,
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        // Thread entry method
        private void DoWork()
        {
            try
            {
                _complete.Reset();
                Work();
            }
            catch (Exception ex)
            {
                if (DontRetryWorkOnFailed)
                    throw;
                else
                {
                    try
                    {
                        Thread.Sleep(1000);
                        Work();
                    }
                    catch
                    {
                        // ex from first exception
                       // LogAndShowMessage(ex);
                    }
                }
            }
            finally
            {
                _complete.Set();
            }
        }

        public bool DontRetryWorkOnFailed { get; set; }

        // Implemented in base class to do actual work.
        protected abstract void Work();
    }
    internal static class GeneralPurposeUtils
    {
        public static void OpenURL(string url)
        {
            try
            {
                GCUtils.OpenURL(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public static string GetFriendlyName([NotNull] this Type type)
        {
            if (type == typeof(int))
            {
                return "integer (32bit)";
            }
            else if (type == typeof(short))
            {
                return "integer (16bit)";
            }
            else if (type == typeof(byte))
            {
                return "integer (8bit)";
            }
            else
            {
                return type == typeof(bool)
                    ? "true/false"
                    : type == typeof(long)
                                    ? "integer (64bit)"
                                    : type == typeof(float)
                                                    ? "decimal (32bit)"
                                                    : type == typeof(double)
                                                                    ? "decimal (64bit)"
                                                                    : type == typeof(decimal)
                                                                                    ? "decimal (128bit)"
                                                                                    : type == typeof(string)
                                                                                                    ? "text"
                                                                                                    : type.IsGenericType
                                                                                                                    ? type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName).ToArray()) + ">"
                                                                                                                    : type.Name;
            }
        }

        public static void AddPlaceHolder([NotNull] this Control control, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                control.Text = placeholder;
            }

            control.GotFocus += (object sender, EventArgs e) =>
            {
                if (control.Text == placeholder)
                {
                    control.Text = string.Empty;
                }
            };
            control.LostFocus += (object sender, EventArgs e) =>
            {
                if (string.IsNullOrWhiteSpace(control.Text))
                {
                    control.Text = placeholder;
                }
            };
        }

        public static string MakeValidFileName(IEnumerable<string> names, string original)
        {
            string copy = original;
            int i = 1;

            while (names.FirstOrDefault(e => e == copy) != null)
            {
                copy = $"{original}_{i++}";
            }

            return copy;
        }
    }
}
