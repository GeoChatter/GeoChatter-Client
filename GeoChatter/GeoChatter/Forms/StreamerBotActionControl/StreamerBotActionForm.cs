using GeoChatter.Integrations;
using GeoChatter.Integrations.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Forms.StreamerBotActionControl
{
    public partial class StreamerBotActionForm : Form
    {
        private StreamerbotClient client;
        public StreamerBotActionForm(StreamerbotClient client, string actionGuid)
        {
            if(client == null)
            {
                throw new ArgumentNullException("client");
            }
            InitializeComponent();
            this.client = client;
            client.ActionsReceived += Client_ActionsReceived;
            client.GetActions();
            if(!string.IsNullOrEmpty(actionGuid))
                SelectedActionGuid = actionGuid;
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
        }
        
        private void Client_ActionsReceived(object sender, GeoChatter.Integrations.Classes.ActionsReceivedEventArgs e)
        {
            listBox1.DataSource = new List<StreamerbotAction>(e.Actions).OrderBy(a => a.name).ToList();
            listBox1.ValueMember = "id";
            listBox1.DisplayMember = "name";
            if(!string.IsNullOrEmpty(SelectedActionGuid))
                listBox1.SelectedValue = SelectedActionGuid;
        }

        public string SelectedActionName { get; internal set; }
        public string SelectedActionGuid { get; internal set; }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SelectedActionGuid = SelectedActionName = string.Empty;
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            else
            {
                StreamerbotAction action = listBox1.SelectedItem as StreamerbotAction;
                if(action != null)
                {
                    SelectedActionGuid = action.id;
                    SelectedActionName = action.name;
                }
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            btnSave_Click(this, null);
        }
    }
}
