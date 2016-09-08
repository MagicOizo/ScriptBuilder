using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Script_Builder
{
    public partial class createIfForm : Form
    {
        string variableMarker;
        string[] variablen;
        mainForm mainForm;

        public createIfForm(string[] variablen, string variableMarker, mainForm mainForm)
        {
            InitializeComponent();
            this.variableMarker = variableMarker;
            this.variablen = variablen;
            this.mainForm = mainForm;
            cboCompareOperator.SelectedIndex = 0;
            warningA.Visible = false;
            warningB.Visible = false;
            foreach (string thisVariable in variablen)
            {
                cboCompareeA.Items.Add(variableMarker + thisVariable + variableMarker);
                cboCompareeB.Items.Add(variableMarker + thisVariable + variableMarker);
            }
        }

        private void comboBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCompareeA.Text != "" && cboCompareeB.Text != "" && cboCompareOperator.Text != "")
            {
                cboCompareeA.BackColor = Color.White;
                cboCompareeB.BackColor = Color.White;
                warningA.Visible = false;
                warningB.Visible = false;
                string txtA = cboCompareeA.Text;
                string txtB = cboCompareeB.Text;
                Regex regex = new Regex(@"^-?[0-9]+$");
                if (!regex.IsMatch(txtA))
                {
                    bool isVariable = false;
                    foreach (string thisVariable in variablen)
                    {
                        regex = new Regex(variableMarker + thisVariable + @"(\[[0-9$]+[-+]?[0-9$]*,[0-9$]+[-+]?[0-9$]*\])?" + variableMarker);
                        if (regex.IsMatch(txtA))
                        {
                            isVariable = true;
                            break;
                        }
                    }
                    if (!isVariable)
                    {
                        if (cboCompareOperator.SelectedIndex > 1)
                        {
                            cboCompareeA.BackColor = Color.Red;
                            warningA.Visible = true;
                            txtPreview.Text = "";
                            return;
                        }
                        txtA = "\"" + txtA + "\"";
                    }
                }
                regex = new Regex(@"^-?[0-9]+$");
                if (!regex.IsMatch(txtB))
                {
                    bool isVariable = false;
                    foreach (string thisVariable in variablen)
                    {
                        regex = new Regex(variableMarker + thisVariable + @"(\[[0-9$]+[-+]?[0-9$]*,[0-9$]+[-+]?[0-9$]*\])?" + variableMarker);
                        if (regex.IsMatch(txtB))
                        {
                            isVariable = true;
                            break;
                        }
                    }
                    if (!isVariable)
                    {
                        if (cboCompareOperator.SelectedIndex > 1)
                        {
                            cboCompareeB.BackColor = Color.Red;
                            warningB.Visible = true;
                            txtPreview.Text = "";
                            return;
                        }
                        txtB = "\"" + txtB + "\"";
                    }
                }
                txtPreview.Text = "/IF " + txtA + " " + cboCompareOperator.Text + " " + txtB + "\r\n//" + mainForm.programmStrings.GetString("textInsertIFTemplate") + "\r\n";
                if (chkElse.Checked)
                    txtPreview.Text += "/ELSE\r\n//" + mainForm.programmStrings.GetString("textInsertELSETemplate") + "\r\n";
                txtPreview.Text += "/FI";
            }
            else
            {
                txtPreview.Text = "";
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtPreview.Text, true);
        }
    }
}
