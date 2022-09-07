using System;
using System.Windows.Forms;

namespace GeoChatter.Controls
{
    internal partial class ShortcutRecorderControl : UserControl
    {
        public ShortcutRecorderControl()
        {
            InitializeComponent();
        }

        private bool editable;
        private void btnEditSave_Click(object sender, EventArgs e)
        {
            if (!editable)
            {
                // txtShortcut.ReadOnly = false;
                txtShortcut.Enabled = true;
                txtShortcut.PreviewKeyDown += TextBox1_PreviewKeyDown;
                txtShortcut.Focus();
                editable = true;
                txtShortcut.Text = "";
                btnEditSave.Text = "Save";
                btnCancel.Visible = true;
            }
            else
            {
                Disable();
                Modifiers = modifiers;
                KeyCode = keyCode;

            }
        }

        private void Disable()
        {
            //  txtShortcut.ReadOnly = true;
            txtShortcut.Enabled = false;
            txtShortcut.PreviewKeyDown -= TextBox1_PreviewKeyDown;
            btnEditSave.Text = "Edit";
            editable = false;
            btnCancel.Visible = false;
        }

        public Keys Modifiers { get; set; }
        private Keys modifiers;
        public int KeyCode { get; set; }
        private int keyCode;
        private void TextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode is Keys.Escape or Keys.Back or Keys.Enter or Keys.Return or Keys.Space or Keys.Apps)
            {
                return;
            }

            keyCode = e.KeyValue;
            modifiers = e.Modifiers;
            bool isCommandKey = e.KeyCode is Keys.ControlKey or Keys.Menu or Keys.ShiftKey;
            txtShortcut.Text = (e.Control ? "Ctrl + " : "") + (e.Shift ? "Shift + " : "") + (e.Alt ? "Alt + " : "") + (isCommandKey ? "" : e.KeyCode.ToString());
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // e.Handled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Disable();
            Keys k = Modifiers;
            bool ctrl = k.HasFlag(Keys.Control);
            bool alt = k.HasFlag(Keys.Alt);
            bool shift = k.HasFlag(Keys.Shift);
            txtShortcut.Text = (ctrl ? "Ctrl + " : "") + (shift ? "Shift + " : "") + (alt ? "Alt + " : "") + ((Keys)KeyCode).ToString();
        }

        public void SetShortcut(int keyCode, Keys modifiers)
        {
            Modifiers = modifiers;
            KeyCode = keyCode;
            Keys k = Modifiers;
            bool ctrl = k.HasFlag(Keys.Control);
            bool alt = k.HasFlag(Keys.Alt);
            bool shift = k.HasFlag(Keys.Shift);
            txtShortcut.Text = (ctrl ? "Ctrl + " : "") + (shift ? "Shift + " : "") + (alt ? "Alt + " : "") + ((Keys)KeyCode).ToString();
        }
    }
}
