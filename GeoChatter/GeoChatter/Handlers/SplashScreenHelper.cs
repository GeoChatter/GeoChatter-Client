/* ------------------------------------------------------------------------
 *
 * Datei:       SplashScreenHelper.cs
 *
 * Projekt:     RG-Info
 *
 * Zweck:       Zeigt das Splashfenster des RG-Navigators an.
 *
 * Autor:       $Author$
 * 
 * Revision:    $Revision$
 *
 * Datum:       $Date$
 * 
 * -------------------------------------------------------------------------
 */

using GeoChatter.Forms;
using log4net;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GeoChatter.Handlers
{
    internal static class SplashScreenHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SplashScreenHelper));
        private static Thread splashThread;

        public static bool Visible => splashThread != null;

        private static void CreateAndShowSplash()
        {
            try
            {
                Form = new SplashForm();
                Form.ShowDialog();
            }
            catch (ThreadAbortException)
            {
                //Thread.ResetAbort();
            }
        }
        public static SplashForm Form { get; private set; }
        public static void Show()
        {
            if (splashThread != null)
            {
                return;
            }

            try
            {
                splashThread = new Thread(CreateAndShowSplash)
                {
                    IsBackground = true
                };
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
                splashThread.SetApartmentState(ApartmentState.STA);
#pragma warning restore CA1416 // Validate platform compatibility
#endif
                splashThread.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);

                Form = null;
                splashThread = null;
            }
        }

        public static void SetPercentage(int value, string text)
        {
            try
            {
                if (Form != null)
                {
                    if (Form.InvokeRequired)
                    {
                        Form.Invoke(() => Form.SetPercentage(value, text));
                        return;
                    }
                    Form.SetPercentage(value, text);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public static void Close()
        {
            try
            {
                if (Form != null)
                {
                    if (Form.InvokeRequired)
                    {
                        Form.Invoke(new MethodInvoker(Form.Close));
                        return;
                    }
                    Form.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                Form = null;
                splashThread = null;
            }
        }
    }
}
