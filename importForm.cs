using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Script_Builder
{
    public enum ImportType
    {
        Clipboard,
        File
    }

    public partial class importForm : Form
    {
        private mainForm mainForm;

        public importForm(mainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            string startPath;
            switch (mainForm.programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathTable";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            importFile.Text = mainForm.programOptions.GetOptions(startPath).Value;
        }

        public string importFilePath
        {
            get
            {
                return importFile.Text;
            }
        }

        public bool firstRowHeader
        {
            get
            {
                return firstRowHeaders.Checked;
            }
        }

        public Encoding ImportEncoding
        {
            get
            {
                if (radANSI.Checked)
                    return Encoding.Default;
                else if (radUnicode.Checked)
                    return Encoding.Unicode;
                else //(radUTF8.Checked)
                    return Encoding.UTF8;
            }
        }

        public ImportType ImportType
        {
            get
            {
                if (radClipboard.Checked == true)
                    return ImportType.Clipboard;
                else
                    return ImportType.File;
            }
        }

        public char[] SeparatorChar
        {
            get
            {
                if (radKomma.Checked)
                    return new char[] { ',' };
                else if (radSemikolon.Checked)
                    return new char[] { ';' };
                else if (radTab.Checked)
                    return new char[] { '\t' };
                else if (radPipe.Checked)
                    return new char[] { '|' };
                else if (radSpace.Checked)
                    return new char[] { ' ' };
                else //radCustom.Checked
                    return txtCustom.Text.ToCharArray();
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            string startPath;
            OpenFileDialog openImportFile = new OpenFileDialog();
            switch (mainForm.programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathTable";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            if (importFile.Text != "")
                openImportFile.InitialDirectory = Path.GetDirectoryName(importFile.Text + "\\");
            else
                openImportFile.InitialDirectory = mainForm.programOptions.GetOptions(startPath).Value;
            openImportFile.AddExtension = true;
            openImportFile.CheckFileExists = true;
            openImportFile.CheckPathExists = true;
            openImportFile.DefaultExt = "csv";
            openImportFile.Multiselect = false;
            openImportFile.ShowReadOnly = false;
            openImportFile.SupportMultiDottedExtensions = true;
            openImportFile.ValidateNames = true;
            openImportFile.Filter = mainForm.programmStrings.GetString("filterCSV") + " (*.csv)|*.csv";
            openImportFile.Filter += "|" + mainForm.programmStrings.GetString("filterText") + " (*.txt)|*.txt";
            openImportFile.Filter += "|" + mainForm.programmStrings.GetString("filterKnownFormats") + " (*.*)|*.*";
            openImportFile.FilterIndex = 1;
            openImportFile.Title = mainForm.programmStrings.GetString("textImportTableFrom");
            DialogResult result = openImportFile.ShowDialog();
            if (result == DialogResult.Cancel) return;
            importFile.Text = openImportFile.FileName.ToString();
            mainForm.CurrentPath = Path.GetDirectoryName(openImportFile.FileName);
            if (mainForm.programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                mainForm.programOptions.SetOptions(startPath, mainForm.CurrentPath);
        }

        private void txtCustom_TextChanged(object sender, EventArgs e)
        {
            if (txtCustom.TextLength > 1)
                txtCustom.Text = txtCustom.Text.Remove(1);
        }

        private void radCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (radCustom.Checked == true)
                txtCustom.Enabled = true;
            else
                txtCustom.Enabled = false;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            if (radCustom.Checked && txtCustom.TextLength != 1)
            {
                MessageBox.Show(mainForm.programmStrings.GetString("textCustomSeperator"), mainForm.programmStrings.GetString("textImportTable"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (radFile.Checked == true && !File.Exists(importFile.Text))
            {
                MessageBox.Show(mainForm.programmStrings.GetString("textFileDontExits"), mainForm.programmStrings.GetString("textImportTable"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void importFile_TextChanged(object sender, EventArgs e)
        {
            mainForm.CurrentPath = Path.GetFullPath(importFile.Text);
        }

        private void radFile_CheckedChanged(object sender, EventArgs e)
        {
            importFile.Enabled = false;
            browseButton.Enabled = false;
            if (radFile.Checked)
            {
                importFile.Enabled = true;
                browseButton.Enabled = true;
            }
        }

        private void radClipboard_CheckedChanged(object sender, EventArgs e)
        {
            labelDLS.Visible = false;
            labelExcel.Visible = false;
            if (radClipboard.Checked)
            {
                labelDLS.Visible = true;
                labelExcel.Visible = true;
            }
        }
    }
}
