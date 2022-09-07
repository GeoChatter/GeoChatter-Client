using System.Windows.Forms;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Splash screen form
    /// </summary>
    public partial class SplashForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public SplashForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set progress bar to given <paramref name="percentage"/>
        /// </summary>
        /// <param name="percentage"></param>
        /// <param name="text"></param>
        public void SetPercentage(int percentage, string text)
        {
            if (percentage is >= 0 and <= 100)
            {
                progressBar1.Value = percentage;
                label1.Text = text;
            }
        }
    }
}
