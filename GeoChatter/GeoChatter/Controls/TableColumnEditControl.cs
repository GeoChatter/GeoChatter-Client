using GeoChatter.Core.Model;
using GeoChatter.Model;
using System;
using System.Windows.Forms;

namespace GeoChatter.Controls
{
    // TODO: Column position control
    internal partial class TableColumnEditControl : UserControl
    {
        public event EventHandler<TableColumnChangedEventArgs> ColumnChanged;
        public TableColumn EditColumn { get; }
        public TableColumnEditControl(TableColumn col)
        {
            InitializeComponent();
            if (col != null)
            {
                EditColumn = col;
                fieldTextBox.Text = col.DataField;
                textBoxName.Text = col.Name;
                widthNumericUD.Value = Convert.ToDecimal(col.Width);
                checkBoxVisible.Checked = col.Visible;
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text != EditColumn.Name)
            {
                buttonReset.Enabled = buttonSave.Enabled = true;
            }
        }

        private void checkBoxVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVisible.Checked != EditColumn.Visible)
            {
                buttonReset.Enabled = buttonSave.Enabled = true;
            }
        }

        private void propertyGridDefaults_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            EditColumn.DataField = fieldTextBox.Text;
            EditColumn.Name = textBoxName.Text;
            EditColumn.Width = Convert.ToDouble(widthNumericUD.Value);
            EditColumn.Visible = checkBoxVisible.Checked;

            OnColumnChanged();
            buttonReset.Enabled = buttonSave.Enabled = false;
        }

        protected virtual void OnColumnChanged()
        {
            ColumnChanged?.Invoke(this, new(EditColumn));
        }


        private void buttonReset_Click(object sender, EventArgs e)
        {
            fieldTextBox.Text = EditColumn.DataField;
            textBoxName.Text = EditColumn.Name;
            widthNumericUD.Value = Convert.ToDecimal(EditColumn.Width);
            checkBoxVisible.Checked = EditColumn.Visible;
        }

        private void widthNumericUD_ValueChanged(object sender, EventArgs e)
        {
            if (widthNumericUD.Value != Convert.ToDecimal(EditColumn.Width))
            {
                buttonReset.Enabled = buttonSave.Enabled = true;
            }
        }
    }

}
