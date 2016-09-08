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
    public partial class optionsForm : Form
    {
        mainForm mainForm = null;
        List<MakroKeyItem> MakroKeyList;
        Dictionary<Rectangle, MakroKeyItem> MakroKeyPosition;
        MakroKeyItem currentMakroKey = null;
        bool changesUnsaved = false;
        bool languageSet = false;

        MakroKeyItem SelectedMakroKey
        {
            get
            {
                return currentMakroKey;
            }
            set
            {
                currentMakroKey = value;
                if (currentMakroKey != null)
                {
                    buttonLöschen.Enabled = true;
                    buttonRename.Enabled = true;
                    buttonAuf.Enabled = false;
                    buttonAb.Enabled = false;
                    if (MakroKeyList.Count > 1)
                    {
                        if (MakroKeyList.IndexOf(currentMakroKey) > 0)
                            buttonAuf.Enabled = true;
                        if (MakroKeyList.IndexOf(currentMakroKey) < MakroKeyList.Count - 1)
                            buttonAb.Enabled = true;
                    }
                    txtKeyName.Text = currentMakroKey.KeyName;
                    txtKeyName.SelectionStart = txtKeyName.TextLength;
                    keyOptions.Enabled = true;
                    buttonApply.Enabled = false;
                    txtKeyTarget.Text = currentMakroKey.TargetFile;
                    comboTargetTextBox.Text = currentMakroKey.TargetLocation;
                }
                else
                {
                    buttonLöschen.Enabled = false;
                    buttonRename.Enabled = false;
                    buttonAuf.Enabled = false;
                    buttonAb.Enabled = false;
                    txtKeyName.Text = "";
                    keyOptions.Enabled = false;
                    txtKeyTarget.Text = "";
                    comboTargetTextBox.Text = "";
                }
            }
        }

        public optionsForm(mainForm mainForm)
        {
            InitializeComponent();
            
            bool languageMayBeSet = true;
            this.MakroKeyList = new List<MakroKeyItem>();
            this.MakroKeyPosition = new Dictionary<Rectangle, MakroKeyItem>();
            panelBasic.Location = new Point(129, 2);
            panelMakroKeys.Location = new Point(129, 2);
            panelBrowseDialogs.Location = new Point(129, 2);
            this.mainForm = mainForm;

            this.comboTargetTextBox.Items.AddRange(new object[] {
                mainForm.programmStrings.GetString("makroTargetDefine"), //"Wie in Makrodatei definiert",
                mainForm.programmStrings.GetString("makroTargetTable"), //"Tabelle",
                mainForm.programmStrings.GetString("makroTargetSimple"), //"Einfach (Header, Footer)",
                mainForm.programmStrings.GetString("makroTargetRepeated")}); //"Wiederholt (Body)"});

            //foreach (KeyValuePair<string, string> thisPair in mainForm.programmStrings.Cultures)
            //    this.cboLanguage.Items.Add(thisPair.Value);


            if (this.mainForm.programOptions.GetOptions("Language") != null)
                cboLanguage.SelectedItem = this.mainForm.programOptions.GetOptions("Language").Value;
            else
                cboLanguage.SelectedItem = "Deutsch";
            if (this.mainForm.programOptions.GetOptions("VariableMarker") != null)
                txtVariableMarker.Text = this.mainForm.programOptions.GetOptions("VariableMarker").Value;
            else
                txtVariableMarker.Text = "%";
            if (this.mainForm.programOptions.GetOptions("DokuPath") != null)
                txtDokuPath.Text = this.mainForm.programOptions.GetOptions("DokuPath").Value;
            else
                txtDokuPath.Text = "";
            if (this.mainForm.programOptions.GetOptions("StartPathType") != null)
                switch (this.mainForm.programOptions.GetOptions("StartPathType").Value)
                {
                    case "GroupsLast":
                        radGroupsLast.Checked = true;
                        break;
                    case "AlwaysSame":
                        radAlwaysSame.Checked = true;
                        break;
                    default:
                        radAnybodysLast.Checked = true;
                        break;
                }
            else
                radAnybodysLast.Checked = true;
            if (this.mainForm.programOptions.GetOptions("StartPath") != null)
                txtStartPath.Text = this.mainForm.programOptions.GetOptions("StartPath").Value;
            else
                txtStartPath.Text = Application.StartupPath;
            if (this.mainForm.programOptions.GetOptions("WriteStartPathOnEnd") != null)
            {
                switch (this.mainForm.programOptions.GetOptions("WriteStartPathOnEnd").Value)
                {
                    case "True":
                        chkSaveStartPath.CheckState = CheckState.Checked;
                        break;
                    case "Intermediate":
                        chkSaveStartPath.CheckState = CheckState.Indeterminate;
                        break;
                    default:
                        chkSaveStartPath.CheckState = CheckState.Unchecked;
                        break;
                }
            }
            foreach (makroKeys thisKey in this.mainForm.programOptions.orderKeyList(this.mainForm.programOptions.makroList))
            {
                MakroKeyItem newItem = new MakroKeyItem(thisKey.KeyName,thisKey.KeyTarget,"");
                switch (thisKey.TargetTextBox.ToUpper())
                {
                    case "TABLE":
                        newItem.TargetLocation = mainForm.programmStrings.GetString("makroTargetTable"); //"Tabelle";
                        break;
                    case "SIMPLE":
                        newItem.TargetLocation = mainForm.programmStrings.GetString("makroTargetSimple"); //"Einfach (Header, Footer)";
                        break;
                    case "REPEATED":
                        newItem.TargetLocation = mainForm.programmStrings.GetString("makroTargetRepeated"); //"Wiederholt (Body)";
                        break;
                    default:
                        newItem.TargetLocation = mainForm.programmStrings.GetString("makroTargetDefine"); //"Wie in Makrodatei definiert";
                        break;
                }
                MakroKeyList.Add(newItem);
            }
            if (this.mainForm.programOptions.GetOptions("Debugging") != null)
            {
                if(this.mainForm.programOptions.GetOptions("Debugging").Value == "On")
                    chkDebugging.Checked = true;
                else
                    chkDebugging.Checked = false;
            }
            else
                chkDebugging.Checked = false;
            changesUnsaved = false;
            languageSet = false;
            buttonOK.Enabled = false;
            if (languageMayBeSet)
                cboLanguage.Enabled = true;
            else
                cboLanguage.Enabled = false;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            panelBasic.Visible = false;
            panelMakroKeys.Visible = false;
            panelBrowseDialogs.Visible = false;
            switch (e.Node.Name)
            {
                case "knotBasic":
                    panelBasic.Visible = true;
                    break;
                case "knotMakroKeys":
                    panelMakroKeys.Visible = true;
                    break;
                case "knotBrowseDialogs":
                    panelBrowseDialogs.Visible = true;
                    break;
            }
        }

        private void txtVariableMarker_TextChanged(object sender, EventArgs e)
        {
            if (txtVariableMarker.TextLength > 1)
                txtVariableMarker.Text = txtVariableMarker.Text.Remove(1);
            buttonOK.Enabled = true;
            changesUnsaved = true;
        }

        private void txtKeyName_TextChanged(object sender, EventArgs e)
        {
            buttonHinzu.Enabled = false;
            buttonRename.Enabled = false;
            if (txtKeyName.Text.Length > 0)
                buttonHinzu.Enabled = true;
            if (SelectedMakroKey != null && txtKeyName.Text != SelectedMakroKey.KeyName)
                buttonRename.Enabled = true;
        }

        private void buttonBrowseDoku_Click(object sender, EventArgs e)
        {
            BrowseFile(mainForm.programmStrings.GetString("filterPDF") + " (*.pdf)|*.pdf", "pdf", txtDokuPath, mainForm.programmStrings.GetString("buttonBrowseDokuCaption"));
        }

        private void BrowseFile(string filter, string defaultExt, TextBox targetTextBox, string dialogTitle)
        {
            string startPath;
            OpenFileDialog openImportFile = new OpenFileDialog();
            switch (mainForm.programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathOptions";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            string selectedPath = targetTextBox.Text;
            if (selectedPath != "")
            {
                if (Directory.Exists(selectedPath))
                    selectedPath += @"\";
                openImportFile.InitialDirectory = Path.GetDirectoryName(selectedPath);
            }
            else
            {
                openImportFile.InitialDirectory = mainForm.programOptions.GetOptions(startPath).Value;
            }
            openImportFile.AddExtension = true;
            openImportFile.CheckFileExists = true;
            openImportFile.CheckPathExists = true;
            openImportFile.DefaultExt = defaultExt;
            openImportFile.InitialDirectory = targetTextBox.Text;
            openImportFile.Multiselect = false;
            openImportFile.ShowReadOnly = false;
            openImportFile.SupportMultiDottedExtensions = true;
            openImportFile.ValidateNames = true;
            openImportFile.Filter = filter;
            openImportFile.FilterIndex = 1;
            openImportFile.Title = dialogTitle;
            if (openImportFile.ShowDialog() == DialogResult.Cancel) return;
            targetTextBox.Text = openImportFile.FileName.ToString();
            mainForm.CurrentPath = Path.GetDirectoryName(openImportFile.FileName.ToString());
            if(mainForm.programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                mainForm.programOptions.SetOptions(startPath, mainForm.CurrentPath);
        }

        private void buttonKeyBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(mainForm.programmStrings.GetString("filterText") + " (*.txt)|*.txt|" + mainForm.programmStrings.GetString("filterCSV") + " (*.csv)|*.csv|" + mainForm.programmStrings.GetString("filterKnownFormats") + " (*.*)|*.*", "txt", txtKeyTarget, mainForm.programmStrings.GetString("buttonKeyBrowseCaption"));
            if (Path.GetExtension(txtKeyTarget.Text) == ".csv")
                comboTargetTextBox.Text = mainForm.programmStrings.GetString("makroTargetTable");
        }

        private void buttonHinzu_Click(object sender, EventArgs e)
        {
            foreach (MakroKeyItem thisItem in MakroKeyList)
            {
                if (thisItem.KeyName == txtKeyName.Text)
                {
                    MessageBox.Show(mainForm.programmStrings.GetString("buttonHinzuExistsMsg"), mainForm.programmStrings.GetString("buttonHinzuExistsCaption"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            MakroKeyItem newItem = new MakroKeyItem(txtKeyName.Text, "", mainForm.programmStrings.GetString("makroTargetRepeated"));
            MakroKeyList.Add(newItem);
            SelectedMakroKey = newItem;
            keyBox.Invalidate();
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (SelectedMakroKey == null) return;
            foreach (MakroKeyItem thisItem in MakroKeyList)
            {
                if (thisItem.KeyName == txtKeyName.Text && thisItem != SelectedMakroKey)
                {
                    MessageBox.Show(mainForm.programmStrings.GetString("buttonHinzuExistsMsg"), mainForm.programmStrings.GetString("buttonRenameExistsCaption"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (SelectedMakroKey.KeyName != txtKeyName.Text)
            {
                SelectedMakroKey.KeyName = txtKeyName.Text;
                buttonOK.Enabled = true;
                changesUnsaved = true;
                keyBox.Invalidate();
            }
        }

        private void buttonAufAb_Click(object sender, EventArgs e)
        {
            if (SelectedMakroKey == null) return;
            int oldIndex = MakroKeyList.IndexOf(SelectedMakroKey);
            int newIndex;
            if (sender == buttonAuf)
                newIndex = oldIndex - 1;
            else if (sender == buttonAb)
                newIndex = oldIndex + 1;
            else
                return;
            int thisIndex = 0;
            MakroKeyItem[] newItemList = new MakroKeyItem[MakroKeyList.Count];
            foreach (MakroKeyItem thisItem in MakroKeyList)
            {
                if (thisIndex == newIndex)
                    newItemList[oldIndex] = thisItem;
                else if (thisIndex == oldIndex)
                    newItemList[newIndex] = thisItem;
                else
                    newItemList[thisIndex] = thisItem;
                thisIndex++;
            }
            MakroKeyList.Clear();
            MakroKeyList.AddRange(newItemList);
            buttonAuf.Enabled = false;
            buttonAb.Enabled = false;
            if (MakroKeyList.Count > 1)
            {
                if (MakroKeyList.IndexOf(SelectedMakroKey) > 0)
                    buttonAuf.Enabled = true;
                if (MakroKeyList.IndexOf(SelectedMakroKey) < MakroKeyList.Count - 1)
                    buttonAb.Enabled = true;
            }
            buttonOK.Enabled = true;
            changesUnsaved = true;
            keyBox.Invalidate();
        }

        private void buttonLöschen_Click(object sender, EventArgs e)
        {
            if (SelectedMakroKey == null) return;
            MakroKeyList.Remove(SelectedMakroKey);
            SelectedMakroKey = null;
            txtKeyName.Focus();
            buttonOK.Enabled = true;
            changesUnsaved = true;
            keyBox.Invalidate();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            string temp1 = SelectedMakroKey.TargetFile;
            string temp2 = SelectedMakroKey.TargetLocation;
            SelectedMakroKey.TargetFile = txtKeyTarget.Text;
            SelectedMakroKey.TargetLocation = comboTargetTextBox.Text;
            buttonApply.Enabled = false;
            if (temp1 != SelectedMakroKey.TargetFile || temp2 != SelectedMakroKey.TargetLocation)
            {
                changesUnsaved = true;
                buttonOK.Enabled = true;
            }
        }

        private void txtKeyTarget_TextChanged(object sender, EventArgs e)
        {
            if (SelectedMakroKey == null || txtKeyTarget.Text == SelectedMakroKey.TargetFile)
            {
                buttonApply.Enabled = false;
            }
            else
            {
                buttonApply.Enabled = true;
                changesUnsaved = true;
            }
        }

        private void comboTargetTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SelectedMakroKey == null || comboTargetTextBox.Text == SelectedMakroKey.TargetLocation)
            {
                buttonApply.Enabled = false;
            }
            else
            {
                buttonApply.Enabled = true;
                changesUnsaved = true;
            }
        }

        private void txtDokuPath_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
            changesUnsaved = true;
            if (sender == cboLanguage)
                languageSet = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(changesUnsaved)
                saveOptions();
            if (languageSet)
                if(cboLanguage.SelectedItem.ToString() == "Deutsch")
                    MessageBox.Show("Änderungen der Sprache werden erst nach einem Neustart des Programms wirksam.","Programmoptionen ändern",MessageBoxButtons.OK,MessageBoxIcon.Information);
                else
                    MessageBox.Show("You need to restart the Application in order the change of language to become effective.", "Change Application Preferences", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveOptions()
        {
            this.mainForm.programOptions.SetOptions("VariableMarker", txtVariableMarker.Text);
            this.mainForm.programOptions.SetOptions("DokuPath", txtDokuPath.Text);
            this.mainForm.programOptions.SetOptions("Language", cboLanguage.SelectedItem.ToString());
            if (chkDebugging.Checked)
                this.mainForm.programOptions.SetOptions("Debugging", "On");
            else
                this.mainForm.programOptions.SetOptions("Debugging", "Off");
            if (radAnybodysLast.Checked)
            {
                this.mainForm.programOptions.SetOptions("StartPathType", "AnybodysLast");
                this.mainForm.programOptions.SetOptions("StartPath", mainForm.CurrentPath);
            }
            else if (radGroupsLast.Checked)
            {
                this.mainForm.programOptions.SetOptions("StartPathType", "GroupsLast");
                this.mainForm.programOptions.SetOptions("StartPathOptions", mainForm.CurrentPath);
                this.mainForm.programOptions.SetOptions("StartPathOutput", mainForm.CurrentPath);
                this.mainForm.programOptions.SetOptions("StartPathTemplate", mainForm.CurrentPath);
                this.mainForm.programOptions.SetOptions("StartPathTable", mainForm.CurrentPath);
            }
            else if (radAlwaysSame.Checked)
            {
                this.mainForm.programOptions.SetOptions("StartPathType", "AlwaysSame");
                this.mainForm.programOptions.SetOptions("StartPath", txtStartPath.Text);
            }
            switch (chkSaveStartPath.CheckState.ToString())
            {
                case "Checked":
                    this.mainForm.programOptions.SetOptions("WriteStartPathOnEnd", "True");
                    break;
                case "Intermediate":
                    this.mainForm.programOptions.SetOptions("WriteStartPathOnEnd", "Intermediate");
                    break;
                default:
                    this.mainForm.programOptions.SetOptions("WriteStartPathOnEnd", "False");
                    break;
            }
            this.mainForm.programOptions.makroList.Clear();
            
            foreach (MakroKeyItem thisItem in MakroKeyList)
            {
                string tempMakroTarget;
                if(thisItem.TargetLocation == mainForm.programmStrings.GetString("makroTargetTable"))
                    tempMakroTarget = "Table";
                else if(thisItem.TargetLocation == mainForm.programmStrings.GetString("makroTargetSimple"))
                    tempMakroTarget = "Simple";
                else if(thisItem.TargetLocation == mainForm.programmStrings.GetString("makroTargetRepeated"))
                    tempMakroTarget = "Repeated";
                else
                    tempMakroTarget = "Define";

                this.mainForm.programOptions.makroList.Add(MakroKeyList.IndexOf(thisItem), new makroKeys(thisItem.KeyName, thisItem.TargetFile, tempMakroTarget));
            }
            changesUnsaved = false;
        }

        private void buttonBrowseStartPath_Click(object sender, EventArgs e)
        {
            string startPath;
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = mainForm.programmStrings.GetString("textChooseRootDirectory");
            folderBrowser.ShowNewFolderButton = true;
            switch (mainForm.programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathOptions";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            if (txtStartPath.Text != "")
                folderBrowser.SelectedPath = Path.GetDirectoryName(txtStartPath.Text + "\\");
            else
                folderBrowser.SelectedPath = mainForm.programOptions.GetOptions(startPath).Value + "\\";
            if (folderBrowser.ShowDialog() == DialogResult.Cancel) return;
            txtStartPath.Text = folderBrowser.SelectedPath.ToString();
            mainForm.CurrentPath = folderBrowser.SelectedPath.ToString();
            if (mainForm.programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                mainForm.programOptions.SetOptions(startPath, mainForm.CurrentPath);
        }

        private void radStartPath_CheckedChanged(object sender, EventArgs e)
        {
            if (radAlwaysSame.Checked)
            {
                txtStartPath.Enabled = true;
                buttonBrowseStartPath.Enabled = true;
                if (chkSaveStartPath.CheckState == CheckState.Unchecked)
                    chkSaveStartPath.CheckState = CheckState.Indeterminate;
                chkSaveStartPath.Enabled = false;
            }
            else
            {
                txtStartPath.Enabled = false;
                buttonBrowseStartPath.Enabled = false;
                if (chkSaveStartPath.CheckState == CheckState.Indeterminate)
                    chkSaveStartPath.CheckState = CheckState.Unchecked;
                chkSaveStartPath.Enabled = true;
            }
            buttonOK.Enabled = true;
            changesUnsaved = true;
        }

        private void chkSaveStartPath_CheckedChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
            changesUnsaved = true;
        }

        private void txtStartPath_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
            changesUnsaved = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (changesUnsaved == true)
            {
                DialogResult Result = MessageBox.Show(mainForm.programmStrings.GetString("buttonCancelChangesMsg"), mainForm.programmStrings.GetString("buttonCancelChangesCaption"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Cancel)
                {
                    return;
                }
                else if (Result == DialogResult.No)
                {
                    saveOptions();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkDebugging_CheckedChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
            changesUnsaved = true;
        }

        private void keyBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics image = e.Graphics;
            Font font = new Font("Arial", 8.25F);
            Point highPos = new Point(2, 2);
            SizeF temp = image.MeasureString("Testing", font);
            int FeldHöhe = (int)temp.Height + 3;
            int FelderHöhe = MakroKeyList.Count * (FeldHöhe + 1);
            int MomentaneHöhe = keyScroll.Value;
            if (FelderHöhe > keyBox.ClientRectangle.Height)
            {
                keyScroll.Enabled = true;
                keyScroll.Maximum = FelderHöhe + FeldHöhe - keyBox.ClientRectangle.Height;
                keyScroll.SmallChange = FeldHöhe;
                keyScroll.LargeChange = FeldHöhe * 2;
            }
            else
            {
                keyScroll.Enabled = false;
                keyScroll.Maximum = keyBox.ClientRectangle.Height;
                keyScroll.SmallChange = 1;
                keyScroll.LargeChange = 2;
                keyScroll.Value = 0;
            }
            if (MomentaneHöhe <= keyScroll.Maximum)
                keyScroll.Value = MomentaneHöhe;
            else
                keyScroll.Value = keyScroll.Maximum;
            MomentaneHöhe = keyScroll.Value;
            Rectangle AnzeigeFeld = new Rectangle(0, keyBox.ClientRectangle.Location.Y + MomentaneHöhe, keyBox.ClientRectangle.Size.Width, keyBox.ClientRectangle.Height);
            MakroKeyPosition.Clear();
            foreach(MakroKeyItem thisItem in MakroKeyList)
            {
                SizeF textSize = image.MeasureString(thisItem.KeyName, font);
                Rectangle Feld = new Rectangle(highPos, new Size(keyBox.ClientRectangle.Width - 5, FeldHöhe));
                Rectangle SichtbaresFeld = Feld;
                SichtbaresFeld.Location = new Point(Feld.X, Feld.Y - MomentaneHöhe);
                if (Feld.Bottom > AnzeigeFeld.Top && Feld.Top < AnzeigeFeld.Bottom)
                {
                    Brush textColor = Brushes.Black;
                    if(thisItem == SelectedMakroKey)
                    {
                        Brush highlight = new SolidBrush(SystemColors.Highlight);
                        textColor = new SolidBrush(SystemColors.HighlightText);
                        image.FillRectangle(highlight, SichtbaresFeld);
                    }
                    image.DrawString(thisItem.KeyName, font, textColor, new PointF(SichtbaresFeld.Left + 2, SichtbaresFeld.Top + 1));
                    MakroKeyPosition.Add(SichtbaresFeld, thisItem);
                }
                highPos.Y += FeldHöhe + 3;
            }
        }

        private void keyBox_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedMakroKey = null;
            foreach(KeyValuePair<Rectangle,MakroKeyItem> thisItem in MakroKeyPosition)
                if (thisItem.Key.Contains(e.Location))
                {
                    SelectedMakroKey = thisItem.Value;

                    keyBox.Invalidate();
                    return;
                }
            keyBox.Invalidate();
        }

        private void keyScroll_Scroll(object sender, ScrollEventArgs e)
        {
            keyBox.Invalidate();
        }

        private void keyBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 38)
                keyBoxKeyUp(true);
            else if (e.KeyValue == 40)
                keyBoxKeyUp(false);
        }

        private void keyBoxKeyUp(bool flip)
        {
            if (MakroKeyList.Count == 0)
                return;
            if (SelectedMakroKey == null)
                if(flip)
                    SelectedMakroKey = MakroKeyList[0];
                else
                    SelectedMakroKey = MakroKeyList[MakroKeyList.Count - 1];
            else
                for(int i = 0; i < MakroKeyList.Count; i++)
                    if(SelectedMakroKey == MakroKeyList[i])
                    {
                        if (flip && i > 0)
                            SelectedMakroKey = MakroKeyList[i - 1];
                        else if (!flip && i < MakroKeyList.Count - 1 )
                            SelectedMakroKey = MakroKeyList[i + 1];
                        break;
                    }
            keyBox.Invalidate();
        }
    }

    public class MakroKeyItem
    {
        public string KeyName;
        public string TargetFile;
        public string TargetLocation;

        public MakroKeyItem(string KeyName, string TargetFile, string TargetLocation)
        {
            this.KeyName = KeyName;
            this.TargetFile = TargetFile;
            this.TargetLocation = TargetLocation;
        }

        public MakroKeyItem(string KeyName)
        {
            this.KeyName = KeyName;
            this.TargetFile = "";
            this.TargetLocation = "";
        }
    }
}
