using Forms.StreamerBotActionControl;
using GeoChatter.Integrations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoChatter.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class StreamerBotActionControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public StreamerBotActionControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        public string ActionName { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActionGuid { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="actionName"></param>
        /// <param name="actionGuid"></param>
        public void SetAction(StreamerbotClient client, string actionName, string actionGuid)
        {
            if(!string.IsNullOrEmpty(actionGuid))
                this.ActionGuid = actionGuid;
            if (!string.IsNullOrEmpty(actionName))
            {
                this.ActionName = actionName;
                btnSelectAction.Text = actionName;
            }
            this.client = client;
        }

        private StreamerbotClient client;

        private void btnSelectAction_Click(object sender, EventArgs e)
        {
            using(StreamerBotActionForm form = new StreamerBotActionForm(client, ActionGuid))
            {
                form.TopMost = true;
                
                DialogResult dr = form.ShowDialog(this.Parent);
                if(dr == DialogResult.Abort)
                {
                    this.ActionGuid = this.ActionName =String.Empty;
                    btnSelectAction.Text = "Select action";
                }
                else if (dr == DialogResult.OK)
                {
                    this.ActionGuid = form.SelectedActionGuid;
                    this.ActionName = btnSelectAction.Text = form.SelectedActionName;
                }
            }
        }
    }
}
