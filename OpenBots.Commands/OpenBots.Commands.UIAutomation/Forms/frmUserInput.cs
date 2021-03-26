using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.Commands.UIAutomation.Forms
{
    public partial class frmUserInput : UIForm
    {
        public List<Control> InputControls;
        private string _header;
        private string _directions;
        private DataTable _inputTable;

        public frmUserInput(string header, string directions, DataTable inputTable)
        {
            InitializeComponent();
            _header = header;
            _directions = directions;
            _inputTable = inputTable;

        }

        private void frmUserInput_Load(object sender, EventArgs e)
        {
            InputControls = new List<Control>();

            //get presentation data from command
            lblHeader.Text = _header;
            lblDirections.Text = _directions;

            //get input table
            var inputTable = _inputTable;

            //loop each data collection point
            foreach (DataRow rw in inputTable.Rows)
            {
                //get properties to render controls with
                var fieldType = rw["Type"] as string;
                var fieldLabel = rw["Label"] as string;
                var fieldSize = rw["Size"] as string;

                //attempt to parse custom width/height
                int fieldWidth, fieldHeight;
                try
                {
                    //format should be X,Y
                    var fieldSizeData = fieldSize.Split(',');
                    fieldWidth = int.Parse(fieldSizeData[0].Trim());
                    fieldHeight = int.Parse(fieldSizeData[1].Trim());
                }
                catch (Exception)
                {
                    //if something goes wrong just use defaults
                    fieldWidth = 500;
                    fieldHeight = 100;
                }

                //get default value
                var defaultFieldValue = rw["DefaultValue"] as string;

                var labelingFont = new Font("Segoe UI Bold", 12);

                var label = new Label();

                switch (fieldType)
                {
                    //add more cases here
                    case "ComboBox":

                        //add label
                        label.AutoSize = true;
                        label.ForeColor = Color.SteelBlue;
                        label.Font = labelingFont;
                        label.Text = fieldLabel;
                        flwInputControls.Controls.Add(label);

                        var combobox = new ComboBox();

                        try
                        {
                            var items = defaultFieldValue.Split(',');
                            foreach (var comboItem in items)
                                combobox.Items.Add(comboItem.Trim());

                            if (combobox.Items.Count > 0)
                                combobox.SelectedIndex = 0;
                            else
                                combobox.SelectedIndex = -1;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Generating Control: " + ex.ToString());
                            return;
                        }

              
                        combobox.Width = fieldWidth;
                        combobox.Height = fieldHeight;
                        combobox.Margin = new Padding(10, 5, 0, 0);
                        combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                        combobox.Font = labelingFont;
                        combobox.ForeColor = Color.SteelBlue;
                        combobox.KeyDown += Combobox_KeyDown;
                        InputControls.Add(combobox);
                        flwInputControls.Controls.Add(combobox);

                        break;
                    case "CheckBox":
                        var checkBox = new CheckBox();

                        try
                        {
                            checkBox.Checked = bool.Parse(defaultFieldValue);
                        }
                        catch (Exception)
                        {
                            checkBox.Checked = false;
                        }

                        checkBox.Width = fieldWidth;
                        checkBox.Height = fieldHeight;
                        checkBox.Margin = new Padding(10, 5, 0, 0);
                        checkBox.Text = fieldLabel;
                        checkBox.Font = labelingFont;
                        checkBox.ForeColor = Color.SteelBlue;
                        checkBox.AutoSize = true;
                        checkBox.KeyDown += CheckBox_KeyDown;

                        InputControls.Add(checkBox);
                        flwInputControls.Controls.Add(checkBox);

                        break;
                    default:

                        //add label 
                        label.AutoSize = true;
                        label.ForeColor = Color.SteelBlue;
                        label.Font = labelingFont;
                        label.Text = fieldLabel;
                        flwInputControls.Controls.Add(label);

                        //add textbox
                        var textBox = new TextBox();
                        textBox.Multiline = true;
                        textBox.Width = fieldWidth;
                        textBox.Height = fieldHeight;
                        textBox.Margin = new Padding(10,5,0,0);
                        textBox.Text = defaultFieldValue;
                        textBox.Font = labelingFont;
                        textBox.ForeColor = Color.SteelBlue;
                        textBox.KeyDown += TextBox_KeyDown;
                        InputControls.Add(textBox);
                        flwInputControls.Controls.Add(textBox);
                        break;
                }
            }
        }

        private void CheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                uiBtnOk_Click(null, null);
        }

        private void Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                uiBtnOk_Click(null, null);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Enter)
                return;
            else if (e.KeyCode == Keys.Enter)
                uiBtnOk_Click(null, null);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
