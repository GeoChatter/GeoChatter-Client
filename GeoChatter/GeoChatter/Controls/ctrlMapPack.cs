using GeoChatter.Core.Model;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GeoChatter.Forms.FlagManager
{
    internal partial class ctrlMapPack : UserControl
    {
        private FlagPack pack;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pack"></param>
        public ctrlMapPack(FlagPack pack)
        {
            this.pack = pack;
            InitializeComponent();
            Fill();

        }

        private void Fill()
        {
            if (pack == null)
            {
                return;
            }

            System.ComponentModel.ComponentResourceManager resources = new(typeof(FlagManagerDialog));
            using ImageList coll1 = new();
            coll1.ColorDepth = ColorDepth.Depth8Bit;
            coll1.ImageStream = (ImageListStreamer)resources.GetObject("coll1.ImageStream", CultureInfo.InvariantCulture);
            coll1.TransparentColor = Color.Transparent;

            lblName.Text = pack.Name;
            lblDesc.Text = pack.Description;
        }
    }
}
