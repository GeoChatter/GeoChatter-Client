using CefSharp;
using GeoChatter.Forms;
using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace GeoChatter.Handlers
{
    /// <summary>
    /// Keyboard hit handler
    /// </summary>
    [SupportedOSPlatform("windows7.0")]
    public class KeyboardHandler : IKeyboardHandler
    {
        private readonly MainForm myForm;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public KeyboardHandler(MainForm form)
        {
            myForm = form;
        }
        /// <inheritdoc/>>
        public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int WM_SYSKEYUP = 0x105;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int VK_TAB = 0x9;
            const int VK_LEFT = 0x25;
            const int VK_UP = 0x26;
            const int VK_RIGHT = 0x27;
            const int VK_DOWN = 0x28;

            isKeyboardShortcut = false;

            // Don't deal with TABs by default:
            // TODO: Are there any additional ones we need to be careful of?
            // i.e. Escape, Return, etc...?
            if (windowsKeyCode is VK_TAB or VK_LEFT or VK_UP or VK_DOWN or VK_RIGHT)
            {
                return false;
            }

            bool result = false;

            Control control = chromiumWebBrowser as Control;
            int msgType = 0;
            switch (type)
            {
                case KeyType.RawKeyDown:
                    msgType = isSystemKey ? WM_SYSKEYDOWN : WM_KEYDOWN;
                    break;
                case KeyType.KeyUp:
                    msgType = isSystemKey ? WM_SYSKEYUP : WM_KEYUP;
                    break;
                case KeyType.Char:
                    msgType = isSystemKey ? WM_SYSCHAR : WM_CHAR;
                    break;
                default:
                    Trace.Assert(false);
                    break;
            }
            // We have to adapt from CEF's UI thread message loop to our fronting WinForm control here.
            // So, we have to make some calls that Application.Run usually ends up handling for us:
            PreProcessControlState state = PreProcessControlState.MessageNotNeeded;
            // We can't use BeginInvoke here, because we need the results for the return value
            // and isKeyboardShortcut. In theory this shouldn't deadlock, because
            // atm this is the only synchronous operation between the two threads.
            control?.Invoke(new Action(() =>
            {
                Message msg = new()
                {
                    HWnd = control.Handle,
                    Msg = msgType,
                    WParam = new IntPtr(windowsKeyCode),
                    LParam = new IntPtr(nativeKeyCode)
                };

                // First comes Application.AddMessageFilter related processing:
                // 99.9% of the time in WinForms this doesn't do anything interesting.
                bool processed = Application.FilterMessage(ref msg);
                if (processed)
                {
                    state = PreProcessControlState.MessageProcessed;
                }
                else
                {
                    // Next we see if our control (or one of its parents)
                    // wants first crack at the message via several possible Control methods.
                    // This includes things like Mnemonics/Accelerators/Menu Shortcuts/etc...
                    state = control.PreProcessControlMessage(ref msg);
                }
            }));

            switch (state)
            {
                case PreProcessControlState.MessageNeeded:
                    // TODO: Determine how to track MessageNeeded for OnKeyEvent.
                    isKeyboardShortcut = true;
                    break;
                case PreProcessControlState.MessageProcessed:
                    // Most of the interesting cases get processed by PreProcessControlMessage.
                    result = true;
                    break;
            }
#if DEBUG
            Debug.WriteLine("OnPreKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers);
            Debug.WriteLine("OnPreKeyEvent PreProcessControlState: {0}", state);
#endif
            return result;
        }

        /// <inheritdoc/>>
        public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            // ONLY USE ON KEY DOWN
            if (type != KeyType.RawKeyDown)
            {
                return false;
            }

            bool result = false;
#if DEBUG
            Debug.WriteLine("OnKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers);
#endif
            if (modifiers == CefEventFlags.None && windowsKeyCode == 116)
            {
                // TODO: #294
                //myForm.RefreshBrowser();
            }

            // TODO: Handle MessageNeeded cases here somehow.
            Keys menuModifier = Properties.Settings.Default.ShortcutsMenuModifiers;
            bool mCtrl = menuModifier.HasFlag(Keys.Control);
            bool mAlt = menuModifier.HasFlag(Keys.Alt);
            bool mShift = menuModifier.HasFlag(Keys.Shift);
            int menuKey = Properties.Settings.Default.ShortcutsMenuKey;
            if ((!mCtrl || modifiers.HasFlag(CefEventFlags.ControlDown))
                && (!mAlt || modifiers.HasFlag(CefEventFlags.AltDown))
                && (!mShift || modifiers.HasFlag(CefEventFlags.ShiftDown))
                && windowsKeyCode == menuKey)
            {
                myForm.ShowMenu();
            }
            Keys fullscreenModifier = Properties.Settings.Default.ShortcutsFullscreenModifiers;
            bool fCtrl = fullscreenModifier.HasFlag(Keys.Control);
            bool fAlt = fullscreenModifier.HasFlag(Keys.Alt);
            bool fShift = fullscreenModifier.HasFlag(Keys.Shift);
            int fullscreenKey = Properties.Settings.Default.ShortcutsFullscreenKey;
            if (((!fCtrl || modifiers.HasFlag(CefEventFlags.ControlDown)) 
                    && (!fAlt || modifiers.HasFlag(CefEventFlags.AltDown)) 
                    && (!fShift || modifiers.HasFlag(CefEventFlags.ShiftDown))
                    && windowsKeyCode == fullscreenKey) 
                || (windowsKeyCode == 27 && myForm.IsFullscreen))
            {
                myForm.ToggleFullscreen();
            }
            Keys settingsModifier = Properties.Settings.Default.ShortcutsSettingsModifiers;
            bool sCtrl = settingsModifier.HasFlag(Keys.Control);
            bool sAlt = settingsModifier.HasFlag(Keys.Alt);
            bool sShift = settingsModifier.HasFlag(Keys.Shift);
            int settingsKey = Properties.Settings.Default.ShortcutsSettingsKeycode;
            if ((!sCtrl || modifiers.HasFlag(CefEventFlags.ControlDown)) 
                && (!sAlt || modifiers.HasFlag(CefEventFlags.AltDown)) 
                && (!sShift || modifiers.HasFlag(CefEventFlags.ShiftDown)) 
                && windowsKeyCode == settingsKey)
            {
                myForm.ShowSettingsDialog();
            }

            return result;
        }
    }
}
