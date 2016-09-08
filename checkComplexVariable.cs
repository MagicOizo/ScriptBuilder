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
    public partial class checkComplexVariable : Form
    {
        string variableMarker;

        public checkComplexVariable(string variableMarker)
        {
            this.variableMarker = variableMarker;
            InitializeComponent();
            labelVariable.Text = "";
        }

        private void textBoxes_TextChanged(object sender, EventArgs e)
        {
            txtOutput.Text = "";
            labelVariable.Text = "";
            Regex regex = new Regex(variableMarker + @"(.+)" + variableMarker);
            if (regex.IsMatch(txtInput.Text))
            {
                Match thisMatch = regex.Match(txtInput.Text);
                labelVariable.Text = thisMatch.Groups[1].ToString();
                string output = txtValue.Text;
                regex = new Regex(variableMarker + @"(.+)\[(?<from>[0-9$]+[-+]?[0-9$]*),(?<to>[0-9$]+[-+]?[0-9$]*)\]" + variableMarker);
                if (regex.IsMatch(txtInput.Text))
                {
                    thisMatch = regex.Match(txtInput.Text);
                    labelVariable.Text = thisMatch.Groups[1].ToString();
                    string posFrom = thisMatch.Groups["from"].ToString();
                    string posTo = thisMatch.Groups["to"].ToString();
                    posFrom = vorlage.calcString(posFrom.Replace("$", (txtValue.Text.Length - 1).ToString()));
                    posTo = vorlage.calcString(posTo.Replace("$", (txtValue.Text.Length - 1).ToString()));
                    output = "";
                    try
                    {
                        int intPosFrom = Convert.ToInt32(posFrom);
                        int intPosTo = Convert.ToInt32(posTo);
                        int intLength = intPosTo - intPosFrom + 1;
                        if (intLength + intPosFrom > txtValue.Text.Length)
                            intLength = txtValue.Text.Length - intPosFrom;
                        if (intLength > 0)
                            output = txtValue.Text.Substring(intPosFrom, intLength);
                    }
                    catch
                    {
                        output = txtValue.Text;
                    }
                }
                txtOutput.Text = output;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtInput.Text, true);
        }
    }
}
