using OpenBots.Core.Common;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region Variable/Argument Events
        private void dgvArguments_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells["Direction"].Value = ScriptArgumentDirection.In;
            }
            catch (Exception)
            {
                //datagridview event failure
            }
        }

        private void dgvVariablesArguments_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {         
                DataGridView dgv = (DataGridView)sender;

                _preEditVarArgName = dgv.Rows[e.RowIndex].Cells[0].Value?.ToString();

                _existingVarArgSearchList = new List<string>();
                _existingVarArgSearchList.AddRange(_scriptArguments.Select(arg => arg.ArgumentName).ToList());
                _existingVarArgSearchList.AddRange(_scriptVariables.Select(var => var.VariableName).ToList());
                _existingVarArgSearchList.AddRange(Common.GenerateSystemVariables().Select(var => var.VariableName).ToList());
            }
            catch (Exception)
            {
                //datagridview event failure
            }
        }

        private void dgvVariablesArguments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {           
                DataGridView dgv = (DataGridView)sender;

                if (e.ColumnIndex == 0)
                {
                    if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        dgv.Rows.RemoveAt(e.RowIndex);
                        return;
                    }

                    string variableName = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();

                    if (_existingVarArgSearchList.Contains(variableName) && variableName != _preEditVarArgName)
                    {
                        Notify($"A variable or argument with the name '{variableName}' already exists", Color.Red);
                        //MessageBox.Show($"A variable or argument with the name '{variableName}' already exists", "Error");
                        dgv.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                        {
                            cell.ReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //datagridview event failure
            }
}

        private void dgvVariablesArguments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {           
                DataGridView dgv = (DataGridView)sender;

                if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != 0)
                            cell.ReadOnly = true;
                    }
                }
                else if (dgv.Rows[e.RowIndex].Cells[0].Value.ToString() == "ProjectPath")
                {
                    dgv.Rows[e.RowIndex].ReadOnly = true;
                }
            }
            catch (Exception)
            {
                //datagridview event failure
            }
        }
        private void dgvVariables_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row.Cells[0].Value?.ToString() == "ProjectPath")
                {
                    e.Cancel = true;
                }
            }
            catch (Exception)
            {
                //datagridview event failure
            }
        }
        #endregion             
    }
}
