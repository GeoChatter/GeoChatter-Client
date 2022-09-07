using System;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Progress bar form
    /// </summary>
    public partial class ProgressWindow : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public ProgressWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lblText"></param>
        /// <param name="progressText"></param>
        public ProgressWindow(string lblText, string progressText)
        {
            InitializeComponent();
            Text = lblText;
            label2.Text = progressText;
        }

        private void ProgressWindow_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Set progress bar current state
        /// </summary>
        /// <param name="progressText"></param>
        /// <param name="value"></param>
        public void SetProgress(string progressText, int value)
        {
            progressBar1.Value = value;
            label2.Text = progressText;
        }
    }
}
