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
    public partial class OutputLog : Form
    {
        private mainForm mainForm;
        public OutputLog(string[] LogText,mainForm mainForm)
        {
            InitializeComponent();
            txtOutputLog.Lines = LogText;
            this.mainForm = mainForm;
        }

        private void closeOutputLog_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearOutputLog_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(mainForm.programmStrings.GetString("textShallDeleteLog"), mainForm.programmStrings.GetString("textDeleteLog"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            mainForm.ClearOutputLog();
            txtOutputLog.Text = "";
        }

        private void saveOutputLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dialog = new SaveFileDialog();
            string startPath;
            switch (mainForm.programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathOutput";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            Dialog.InitialDirectory = mainForm.programOptions.GetOptions(startPath).Value;
            Dialog.AddExtension = true;
            Dialog.CheckPathExists = true;
            Dialog.DefaultExt = "txt";
            Dialog.Filter = mainForm.programmStrings.GetString("filterText") + " (*.txt)|*.txt";
            Dialog.OverwritePrompt = true;
            Dialog.Title = mainForm.programmStrings.GetString("textSaveLog");
            Dialog.ValidateNames = true;
            DialogResult result = Dialog.ShowDialog();
            if (result == DialogResult.Cancel || result == DialogResult.Abort)
                return;
            using (FileStream sw = new FileStream(Dialog.FileName, FileMode.Create))
            {
                foreach (string line in txtOutputLog.Lines)
                {
                    mainForm.writeLines(line, sw, Encoding.Default, true);
                }
            }
            mainForm.CurrentPath = Path.GetDirectoryName(Dialog.FileName);
            if (mainForm.programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                mainForm.programOptions.SetOptions(startPath, mainForm.CurrentPath);
        }

        private void OutputLog_Shown(object sender, EventArgs e)
        {
            txtOutputLog.Select(txtOutputLog.Text.Length, 0);
        }
    }
}
