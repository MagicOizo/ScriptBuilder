using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Reflection;

namespace Script_Builder
{
    public partial class mainForm : Form
    {
        DataTable tblExcel2WinData;
        public string CurrentPath = Application.StartupPath;
        public options programOptions { get; private set; }
        public Strings programmStrings { get; private set; }
        private TreeNode selectedNode;
        private bool importTemplateMode;
        private Dictionary<TreeNode, VorlagenType> nodeTypen;
        private Dictionary<TreeNode, RichTextBox> nodeTextBoxes;
        private Dictionary<TreeNode, OutputFile> nodeOutputFile;
        private bool checkingText = false;
        private int checkingStartSelection;
        private String debuggingFileName = "C:\\ScriptBuilder_Debug_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
        private String currentTemplatePath;
        List<string> outputLog;

        public mainForm(string[] args)
        {
            programOptions = new options(Path.Combine(Application.StartupPath, "options.xml"), this);
            programOptions.ReadOptions();
            Thread.CurrentThread.CurrentCulture = programOptions.GetCulture();
            Thread.CurrentThread.CurrentUICulture = programOptions.GetCulture();
            programmStrings = new Strings(programOptions.GetCulture(), Application.StartupPath);
            InitializeComponent();
            importTemplateMode = false;
            programOptions.CreateMakroMenu();
            this.splitContainer1.Panel2MinSize = 427;
            tblExcel2WinData = new DataTable();
            dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
            if (programOptions.GetOptions("DokuPath") != null && File.Exists(programOptions.GetOptions("DokuPath").Value))
                this.openScriptingDoku.Enabled = true;
            else
                this.openScriptingDoku.Enabled = false;
            this.cboNodeFileFormat.Items.AddRange(new object[] {
                programmStrings.GetString("NodeFileFormatUTF"),
                programmStrings.GetString("NodeFileFormatUnicode"),
                programmStrings.GetString("NodeFileFormatASCII"),
                programmStrings.GetString("NodeFileFormatANSI")});

            if (args.Length > 0)
                loadVorlage(args[0]);
            else
            {
                newDocument();
                addTemplate(null, VorlagenType.File);
                addTemplate(trvVorlageSelect.Nodes[0], VorlagenType.Header);
                addTemplate(trvVorlageSelect.Nodes[0], VorlagenType.Body);
            }

            showTextBox(null);
            selectedNode = null;
            if (nodeSelectionChanged(trvVorlageSelect.Nodes[0])) {
                trvVorlageSelect.SelectedNode = selectedNode;
            }
            #region Shortcuts Localisiert setzen
            beenden.ShortcutKeys = Keys.Control | Keys.End;
            createOutputFile.ShortcutKeys = Keys.Control | Keys.S;
            options.ShortcutKeys = Keys.Control | Keys.O;
            clearAll.ShortcutKeys = Keys.Control | Keys.N;
            checkTemplate.ShortcutKeys = Keys.Control | Keys.P;
            spalteHinzu.ShortcutKeys = Keys.Control | Keys.Insert;
            clearTable.ShortcutKeys = Keys.Control | Keys.L;
            openScriptingDoku.ShortcutKeys = Keys.Control | Keys.D;
            infoPageShow.ShortcutKeys = Keys.Control | Keys.I;

            beenden.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlEnd");
            createOutputFile.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlS");
            options.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlO");
            clearAll.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlN");
            checkTemplate.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlP");
            spalteHinzu.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlIns");
            clearTable.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlL");
            openScriptingDoku.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlD");
            infoPageShow.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutCtrlI");
            tsmOrderUp.ShortcutKeyDisplayString = "+";
            tsmOrderDown.ShortcutKeyDisplayString = "-";
            tsmEditName.ShortcutKeyDisplayString = "F2";
            tsmRemove.ShortcutKeyDisplayString = programmStrings.GetString("ShortcutDel"); ;
            #endregion
            /*mnuChangeNodeName.Enabled = false;
            mnuRemoveNode.Enabled = false;
            mnuAddFile.Enabled = true;
            mnuAddBlock.Enabled = false;
            mnuChangeOrder.Enabled = false;*/
            //cxmTextBox = new ContextMenuStrip(this.components);
            outputLog = new List<string>();
            trvVorlageSelect.Select();
        }

        private void dgrExcelContents_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                changeTableHeader(this, tblExcel2WinData, e.ColumnIndex);
            }
        }

        public static void changeTableHeader(mainForm mainForm, DataTable InputTable, int ColumnIndex)
        {
            string value = InputTable.Columns[ColumnIndex].ColumnName.ToString();
            if (InputBox.InputBox2(mainForm.programmStrings.GetString("textChangeHeader"), mainForm.programmStrings.GetString("textNewHeader1"), ref value) == DialogResult.OK && value.Length > 0)
            {
                try
                {
                    InputTable.Columns[ColumnIndex].ColumnName = value;
                }
                catch (DuplicateNameException)
                {
                    MessageBox.Show(mainForm.programmStrings.GetString("textErrorHeaderUsed1") + " \"" + value + "\" " + mainForm.programmStrings.GetString("textErrorHeaderUsed2"), mainForm.programmStrings.GetString("textNewHeader2"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dgrExcelContents_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 22)
            {
                PasteExcelData(tblExcel2WinData);
                e.Handled = true;
            }
            /*
            else
            {
                MessageBox.Show(((int)e.KeyChar).ToString());
            }
            */
        }

        public static void PasteExcelData(DataTable InputTable)
        {
            IDataObject objPresumablyExcel = Clipboard.GetDataObject();
            if (objPresumablyExcel.GetDataPresent(DataFormats.CommaSeparatedValue))
            {
                using (StreamReader srReadExcel = new StreamReader(objPresumablyExcel.GetData(DataFormats.CommaSeparatedValue) as Stream,Encoding.Default))
                {
                    readClipboardInput(srReadExcel, InputTable, new Char[] { ';' });
                }
            }
            else if (objPresumablyExcel.GetDataPresent(DataFormats.Text))
            {
                using(StringReader srReadText = new StringReader((string)objPresumablyExcel.GetData(DataFormats.Text)))
                {
                    readClipboardInput(srReadText, InputTable, new Char[] { '\t' });
                }
            }
        }

        private void loadHeaderFormat_Click(object sender, EventArgs e)
        {
            TreeNode fileNode;
            if (selectedNode == null)
            {
                DialogResult result = MessageBox.Show(this.programmStrings.GetString("textErrorNoOutputFile"), this.programmStrings.GetString("textImportText"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
            }
            if (selectedNode != null && nodeTypen.ContainsKey(selectedNode))
            {
                if (nodeTypen[selectedNode] != VorlagenType.File)
                    fileNode = selectedNode.Parent;
                else
                    fileNode = selectedNode;
            }
            else
                fileNode = addTemplate(null, VorlagenType.File);
            TreeNode newNode = addTemplate(fileNode, VorlagenType.Header);
            if(nodeTextBoxes.ContainsKey(newNode))
                if(sender == headerNewFormat)
                {
                    nodeTextBoxes[newNode].Text = "FILE VERSION:11.00.01:MP2\r\n";
                }
                else if (sender == headerOldFormat)
                {
                    nodeTextBoxes[newNode].Text = "FILE VERSION:10.01.01\r\n";
                }
            fileNode.Expand();
            selectedNode = newNode;
            trvVorlageSelect.SelectedNode = selectedNode;
        }

        private void activateControlls(bool activate)
        {
            if (activate)
                progressBar.Maximum = 100;
            progressBar.Visible = !activate;
            mainMenuStrip.Enabled = activate;
            dgrExcelContents.Enabled = activate;
            trvVorlageSelect.Enabled = activate;
            pnlFileProperties.Enabled = activate;
            foreach (KeyValuePair<TreeNode, RichTextBox> thisTextBoxPair in nodeTextBoxes)
                thisTextBoxPair.Value.ReadOnly = !activate;
        }

        private void createOutputFile_Click(object sender, EventArgs e)
        {
            activateControlls(false);
            if (selectedNode != null && nodeTypen.ContainsKey(selectedNode) && (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial) && nodeChanged())
            {
                DialogResult declineChanges = MessageBox.Show(this.programmStrings.GetString("textUnsavedChanges1"), this.programmStrings.GetString("textUnsavedChanges2"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (declineChanges == DialogResult.Yes)
                {
                    declineNodeChanges();
                }
                else if (declineChanges == DialogResult.No)
                {
                    acceptNodeChanges();
                }
                else
                {
                    activateControlls(true);
                    return;
                }
            }
            bool Debugging = false;
            if (programOptions.GetOptions("Debugging") != null && programOptions.GetOptions("Debugging").Value == "On")
            {
                Debugging = true;
                writeDebugging("Start OutputFile Creation");
            }
            #region Überprüfen ob Dateinamen und Speicherpfad korrekt und existiert
            foreach (KeyValuePair<TreeNode, OutputFile> thisOutputFilePair in nodeOutputFile)
            {
                if (Directory.Exists(thisOutputFilePair.Value.FullFileName))
                {
                    MessageBox.Show(this.programmStrings.GetString("textErrorPathIsDir1") + " \"" + thisOutputFilePair.Value.FullFileName + "\" " + this.programmStrings.GetString("textErrorPathIsDir2"), this.programmStrings.GetString("textCreateOutputFile"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Debugging)
                        writeDebugging("Die Outputdatei \"" + thisOutputFilePair.Value.FullFileName + "\" ist ein Verzeichnis.");
                    activateControlls(true);
                    return;
                }
                if (!Directory.Exists(thisOutputFilePair.Value.FilePath))
                {
                    MessageBox.Show(this.programmStrings.GetString("textErrorPathDoentExist1") + " \"" + thisOutputFilePair.Value.FilePath + "\" " + this.programmStrings.GetString("textErrorPathDoentExist2"), this.programmStrings.GetString("textCreateOutputFile"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Debugging)
                        writeDebugging("Das Verzeichnis \"" + thisOutputFilePair.Value.FilePath + "\" existiert nicht.");
                    activateControlls(true);
                    return;
                }
                if (thisOutputFilePair.Value.Exists)
                {
                    DialogResult result = MessageBox.Show(this.programmStrings.GetString("textErrorReplaceOutputFile1") + " \"" + thisOutputFilePair.Value.FileName + "\" " + this.programmStrings.GetString("textErrorReplaceOutputFile2"), this.programmStrings.GetString("textCreateOutputFile"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (Debugging)
                        writeDebugging("Die Outputdatei \"" + thisOutputFilePair.Value.FileName + "\" existiert bereits in dem ausgewählten Verzeichnis.\r\nSoll sie überschrieben werden?");
                    if (result == DialogResult.Cancel)
                    {
                        activateControlls(true);
                        return;
                    }
                    else if (result == DialogResult.No)
                    {
                        thisOutputFilePair.Value.Overwrite = false;
                    }
                    else
                        thisOutputFilePair.Value.Overwrite = true;
                }
            }
            #endregion
            dgrExcelContents.EndEdit();
            dgrExcelContents.Refresh();
            #region ProgressBar Maximum setzen
            int barMaximum = 0;
            int multiplikator;
            foreach(TreeNode thisNode in trvVorlageSelect.Nodes)
            {
                if(nodeOutputFile.ContainsKey(thisNode) && (!nodeOutputFile[thisNode].Exists || nodeOutputFile[thisNode].Overwrite))
                {
                    foreach(TreeNode thisChildNode in thisNode.Nodes)
                    {
                        if(nodeTypen.ContainsKey(thisChildNode) && nodeTypen.ContainsKey(thisNode))
                        {
                            if (nodeTypen[thisNode] == VorlagenType.Serial && nodeOutputFile.ContainsKey(thisNode) && nodeOutputFile[thisNode].SerialInputTable != null)
                                multiplikator = nodeOutputFile[thisNode].SerialInputTable.Rows.Count;
                            else
                                multiplikator = 1;
                            if (nodeTypen[thisChildNode] == VorlagenType.Body)
                                barMaximum += tblExcel2WinData.Rows.Count * multiplikator;
                            else if (nodeTypen[thisChildNode] == VorlagenType.Header || nodeTypen[thisChildNode] == VorlagenType.SerialHeader)
                                barMaximum += multiplikator;
                        }
                    }
                }
            }
            progressBar.Maximum = barMaximum;
            progressBar.Value = 0;
            if (Debugging)
            {
                writeDebugging("progressBar.Maximum = " + progressBar.Maximum);
                writeDebugging("progressBar.Value = " + progressBar.Value);
                writeDebugging("tblExcel2WinData.Rows = " + tblExcel2WinData.Rows.Count);
            }
            #endregion

            int iSpalten;
            int CounterOK = 0;
            int matchFieldA = -1;
            int matchFieldB = -1;
                        
            DateTime startTime = DateTime.Now;;
            DateTime endTime;
            outputLog.Add(this.programmStrings.GetString("logCreateScript1"));
            outputLog.Add(this.programmStrings.GetString("logStart") + startTime.ToString(this.programmStrings.GetString("logTimeCode")));
            outputLog.Add("");
            List<OutputFile> warnFileFormat = new List<OutputFile>();
            string variabelMarker;
            if (programOptions.GetOptions("VariableMarker") != null)
                variabelMarker = programOptions.GetOptions("VariableMarker").Value;
            else
                variabelMarker = "%";
            string[] columns = new string[tblExcel2WinData.Columns.Count];
            for (iSpalten = 0; iSpalten < tblExcel2WinData.Columns.Count; iSpalten++)
            {
                columns[iSpalten] = tblExcel2WinData.Columns[iSpalten].ColumnName;
            }
            Dictionary<TreeNode,vorlage> nodeVorlagen = new Dictionary<TreeNode,vorlage>();
            foreach(KeyValuePair<TreeNode,VorlagenType> thisNodePair in nodeTypen)
            {
                if(thisNodePair.Value == VorlagenType.Body && nodeTextBoxes.ContainsKey(thisNodePair.Key))
                    nodeVorlagen.Add(thisNodePair.Key, new vorlage(nodeTextBoxes[thisNodePair.Key].Lines,columns,variabelMarker));
                else if (thisNodePair.Value == VorlagenType.SerialHeader && nodeTextBoxes.ContainsKey(thisNodePair.Key) && thisNodePair.Key.Parent != null && nodeOutputFile.ContainsKey(thisNodePair.Key.Parent))
                {
                    string[] tempColumns = new string[nodeOutputFile[thisNodePair.Key.Parent].SerialInputTable.Columns.Count];
                    for (iSpalten = 0; iSpalten < nodeOutputFile[thisNodePair.Key.Parent].SerialInputTable.Columns.Count; iSpalten++)
                        tempColumns[iSpalten] = nodeOutputFile[thisNodePair.Key.Parent].SerialInputTable.Columns[iSpalten].ColumnName;
                    nodeVorlagen.Add(thisNodePair.Key, new vorlage(nodeTextBoxes[thisNodePair.Key].Lines, tempColumns, variabelMarker));
                }
            }
            foreach(TreeNode thisNode in trvVorlageSelect.Nodes)
            {
                if(nodeOutputFile.ContainsKey(thisNode))
                {
                    if (nodeOutputFile[thisNode].SerialFile)
                    {
                        if (nodeOutputFile[thisNode].SerialInputTable != null)
                        {
                            string OutputFileName;
                            int iZeilen;

                            matchFieldA = nodeOutputFile[thisNode].SerialFieldLinkID();
                            matchFieldB = nodeOutputFile[thisNode].SerialFieldLinkID(tblExcel2WinData);

                            for (iZeilen = 0; iZeilen < nodeOutputFile[thisNode].SerialInputTable.Rows.Count; iZeilen++)
                            {
                                string[] thisRow = new string[nodeOutputFile[thisNode].SerialInputTable.Columns.Count];
                                for (iSpalten = 0; iSpalten < nodeOutputFile[thisNode].SerialInputTable.Columns.Count; iSpalten++)
                                {
                                    if (nodeOutputFile[thisNode].SerialInputTable.Rows[iZeilen].Field<string>(iSpalten) != null)
                                        thisRow[iSpalten] = nodeOutputFile[thisNode].SerialInputTable.Rows[iZeilen].Field<string>(iSpalten).Trim();
                                    else
                                        thisRow[iSpalten] = "";
                                }
                                OutputFileName = nodeOutputFile[thisNode].FullFileName;
                                string TempFileName = OutputFileName;
                                for (iSpalten = 0; iSpalten < thisRow.Length; iSpalten++)
                                    OutputFileName = vorlage.patternSearch(OutputFileName, variabelMarker, nodeOutputFile[thisNode].SerialInputTable.Columns[iSpalten].Caption, thisRow[iSpalten]);
                                if (TempFileName == OutputFileName)
                                    OutputFileName = numberingFileName(OutputFileName, (iZeilen + 1).ToString());
                                if(matchFieldA != -1)
                                    CounterOK += createNodeOutput(thisNode, nodeVorlagen, warnFileFormat, Debugging, OutputFileName, thisRow, matchFieldB, thisRow[matchFieldA]);
                                else
                                    CounterOK += createNodeOutput(thisNode, nodeVorlagen, warnFileFormat, Debugging, OutputFileName, thisRow, matchFieldB, null);
                            }
                        }
                    }
                    else
                    {
                        CounterOK += createNodeOutput(thisNode, nodeVorlagen, warnFileFormat, Debugging);
                    }
                }
            }

            endTime = DateTime.Now;
            outputLog.Add(this.programmStrings.GetString("logEnde") + endTime.ToString(this.programmStrings.GetString("logTimeCode")));
            if (Debugging)
                writeDebugging("End of creation");
            TimeSpan createDuration = endTime.Subtract(startTime);
            outputLog.Add(this.programmStrings.GetString("logFinishedScriptCreation1") + (createDuration.Days > 0 ? createDuration.Days + "t" : "") + (createDuration.Hours > 0 ? createDuration.Hours + "h" : "") + createDuration.Minutes + "m" + createDuration.Seconds + "s" + createDuration.Milliseconds + "ms" + this.programmStrings.GetString("logFinishedScriptCreation2") + CounterOK + this.programmStrings.GetString("logFinishedScriptCreation3"));
            string resultText = this.programmStrings.GetString("logFinishedScriptCreation4") + "\r\n" + this.programmStrings.GetString("logFinishedScriptCreation1") + (createDuration.Days > 0 ? createDuration.Days + "t" : "") + (createDuration.Hours > 0 ? createDuration.Hours + "h" : "") + createDuration.Minutes + "m" + createDuration.Seconds + "s" + createDuration.Milliseconds + "ms " + this.programmStrings.GetString("logFinishedScriptCreation2") + CounterOK + this.programmStrings.GetString("logFinishedScriptCreation3");
            if (warnFileFormat.Count > 0)
            {
                outputLog.Add(this.programmStrings.GetString("logFinishWarning1"));
                resultText += "\r\n\r\n" + this.programmStrings.GetString("logFinishWarning1") + "\r\n" + this.programmStrings.GetString("logFinishWarning2");
                outputLog.Add(this.programmStrings.GetString("logFinishWarning3"));
                foreach(OutputFile thisOutputFile in warnFileFormat)
                    outputLog.Add("  - " + thisOutputFile.FileName);
            }
            outputLog.Add(this.programmStrings.GetString("logFinishedScriptCreation4"));
            outputLog.Add("");
            MessageBox.Show(resultText, this.programmStrings.GetString("logCreateScript2"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            activateControlls(true);
        }

        private int createNodeOutput(TreeNode thisNode, Dictionary<TreeNode, vorlage> nodeVorlagen, List<OutputFile> warnFileFormat, Boolean Debugging)
        {
            return createNodeOutput(thisNode, nodeVorlagen, warnFileFormat, Debugging, null , null, -1, null);
        }

        private int createNodeOutput(TreeNode thisNode, Dictionary<TreeNode,vorlage> nodeVorlagen, List<OutputFile> warnFileFormat, Boolean Debugging, String FullFileName, String[] SerialHeaders, int matchField, string matchWith)
        {
            int iZeilen;
            int iSpalten;
            int ZeilenCounterPart = 0;
            int CounterOK = 0;
            string CreateFileName;
            if(!nodeTypen.ContainsKey(thisNode) || !nodeOutputFile.ContainsKey(thisNode))
                return 0;
            if (nodeTypen[thisNode] == VorlagenType.File && nodeOutputFile[thisNode].Exists && !nodeOutputFile[thisNode].Overwrite)
                return 0; //Standard File existiert bereits und overwrite ist nicht erlaubt
            else if (nodeTypen[thisNode] == VorlagenType.Serial && (FullFileName == null || SerialHeaders == null))
                return 0; //Vorlage ist Serien-Datei aber die Werte für den FileName und die Serien-Tabellen Felder sind nicht übergeben.
            else if (nodeTypen[thisNode] == VorlagenType.Serial && File.Exists(FullFileName) && nodeOutputFile[thisNode].Overwrite)
                return 0; //Vorlage ist Serien-Datei aber der aktuelle Filename existiert bereits oder overwrite ist nicht erlaubt
            else if (matchField == -1 ^ matchWith == null)
                return 0; //Es müssen entweder beide null sein, oder beide gesetzt sein

            if (nodeTypen[thisNode] == VorlagenType.File)
                CreateFileName = nodeOutputFile[thisNode].FullFileName;
            else
                CreateFileName = FullFileName;

            outputLog.Add(this.programmStrings.GetString("logOutputFile") + " \"" + CreateFileName + "\"");
            if (Debugging)
                writeDebugging("Output Datei: \"" + CreateFileName + "\"");
            if(nodeOutputFile[thisNode].Encoding is ASCIIEncoding)
                warnFileFormat.Add(nodeOutputFile[thisNode]);
            using (FileStream sw = new FileStream(CreateFileName, FileMode.Create))
            {
                if (nodeOutputFile[thisNode].Encoding is UnicodeEncoding)
                {
                    sw.WriteByte((byte)255);
                    sw.WriteByte((byte)254);
                }
                foreach(TreeNode thisChildNode in thisNode.Nodes)
                {
                    if(nodeTypen.ContainsKey(thisChildNode) && nodeTextBoxes.ContainsKey(thisChildNode))
                    {
                        outputLog.Add(this.programmStrings.GetString("logBlock") + " \"" + thisChildNode.Text + "\" (" + DateTime.Now.ToString("HH:mm:ss") + ")");
                        if (Debugging)
                            writeDebugging("Block \"" + thisChildNode.Text);
                        if (nodeTypen[thisChildNode] == VorlagenType.Header)
                        {
                            ZeilenCounterPart = 0;
                            foreach (string lineText in nodeTextBoxes[thisChildNode].Lines)
                            {
                                writeLines(lineText, sw, nodeOutputFile[thisNode].Encoding);
                                ZeilenCounterPart++;
                            }
                            outputLog.Add(this.programmStrings.GetString("logSingleBlockWith1") + (ZeilenCounterPart > 1 ? ZeilenCounterPart + this.programmStrings.GetString("logBlockWith2") : this.programmStrings.GetString("logBlockWith3")));
                            if (progressBar.Value < progressBar.Maximum)
                                progressBar.Value++;
                            if (Debugging)
                            {
                                writeDebugging("Einzelblock mit " + (ZeilenCounterPart > 1 ? ZeilenCounterPart + " Zeilen." : "einer Zeile."));
                                writeDebugging("progressBar.Value = " + progressBar.Value);
                            }
                            Application.DoEvents();
                        }
                        else if (nodeTypen[thisChildNode] == VorlagenType.Body && nodeVorlagen.ContainsKey(thisChildNode))
                        {
                            ZeilenCounterPart = 0;
                            for (iZeilen = 0; iZeilen < tblExcel2WinData.Rows.Count; iZeilen++)
                            {
                                string[] thisRow = new string[tblExcel2WinData.Columns.Count];
                                for (iSpalten = 0; iSpalten < tblExcel2WinData.Columns.Count; iSpalten++)
                                {
                                    if (tblExcel2WinData.Rows[iZeilen].Field<string>(iSpalten) != null)
                                                thisRow[iSpalten] = tblExcel2WinData.Rows[iZeilen].Field<string>(iSpalten).Trim();
                                    else
                                                thisRow[iSpalten] = "";
                                }
                                /*string[] tempOutput = nodeVorlagen[thisChildNode].CreateOutput(thisRow);
                                foreach (string lineOutput in tempOutput)
                                {
                                    writeLines(lineOutput, sw, nodeOutputFile[thisNode].Encoding);
                                    ZeilenCounterPart++;
                                }
                                CounterOK++;
                                */
                                if ((matchField == -1 && matchWith == null) || (matchField != -1 && matchWith != null && thisRow[matchField] == matchWith))
                                {
                                    ZeilenCounterPart += createVorlageOutput(nodeVorlagen[thisChildNode], thisRow, sw, nodeOutputFile[thisNode].Encoding, Debugging);
                                    CounterOK++;
                                }
                                if (progressBar.Value < progressBar.Maximum)
                                    progressBar.Value++;
                                if (Debugging)
                                    writeDebugging("progressBar.Value = " + progressBar.Value);
                                Application.DoEvents();
                            }
                            outputLog.Add(this.programmStrings.GetString("logMultiBlockWith1") + (ZeilenCounterPart > 1 ? ZeilenCounterPart + this.programmStrings.GetString("logBlockWith2") : this.programmStrings.GetString("logBlockWith3")) + this.programmStrings.GetString("logMultiBlockWith2") + (tblExcel2WinData.Rows.Count > 1 ? tblExcel2WinData.Rows.Count + this.programmStrings.GetString("logMultiBlockWith3") : this.programmStrings.GetString("logMultiBlockWith4")));
                            if (Debugging)
                                writeDebugging("Wiederholungsblock mit " + (ZeilenCounterPart > 1 ? ZeilenCounterPart + " Zeilen" : "einer Zeile") + " für " + (tblExcel2WinData.Rows.Count > 1 ? tblExcel2WinData.Rows.Count + " Datensätze" : "einen Datensatz"));
                        }
                        else if (nodeTypen[thisChildNode] == VorlagenType.SerialHeader && nodeVorlagen.ContainsKey(thisChildNode))
                        {
                            ZeilenCounterPart = 0;
                            ZeilenCounterPart = createVorlageOutput(nodeVorlagen[thisChildNode], SerialHeaders, sw, nodeOutputFile[thisNode].Encoding, Debugging);
                            if (progressBar.Value < progressBar.Maximum)
                                progressBar.Value++;
                            if (Debugging)
                            {
                                writeDebugging("Serienfeldblock mit " + (ZeilenCounterPart > 1 ? ZeilenCounterPart + " Zeilen." : "einer Zeile."));
                                writeDebugging("progressBar.Value = " + progressBar.Value);
                            }
                            Application.DoEvents();
                        }
                        outputLog.Add("");
                    }
                }
            }
            return CounterOK;
        }

        private int createVorlageOutput(vorlage nodeVorlage, string[] datenReihe, FileStream sw, Encoding fileEncoding, Boolean Debugging)
        {
            int ZeilenCounterPart = 0;
            string[] tempOutput = nodeVorlage.CreateOutput(datenReihe);
            foreach (string lineOutput in tempOutput)
            {
                writeLines(lineOutput, sw, fileEncoding);
                ZeilenCounterPart++;
            }
            return ZeilenCounterPart;
        }

        private void excelTableImport_Click(object sender, EventArgs e)
        {
            PasteExcelData(tblExcel2WinData);
            dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
        }

        private void clearTable_Click(object sender, EventArgs e)
        {
            tblExcel2WinData.Clear();
            tblExcel2WinData.Columns.Clear();
            dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
        }

        private void openScriptingDoku_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(programOptions.GetOptions("DokuPath").Value);
                MessageBox.Show(this.programmStrings.GetString("textErrorOpenDokument1"), this.programmStrings.GetString("textErrorOpenDokument3"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this.programmStrings.GetString("textErrorOpenDokument2"), this.programmStrings.GetString("textErrorOpenDokument3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void beenden_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void spalteHinzuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tblExcel2WinData.Columns.Add();
            dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
        }

        private void infoPageShow_Click(object sender, EventArgs e)
        {
            AboutBox1 AboutBox = new AboutBox1(programOptions.GetCulture(), programmStrings.GetString("InfoBoxHeader"),programmStrings.GetString("InfoBoxDescription"));
            AboutBox.ShowDialog();
        }

        private static void writeLines(string inputText, FileStream fs, Encoding encoder)
        {
            writeLines(inputText, fs, encoder, false);
        }

        public static void writeLines(string inputText, FileStream fs, Encoding encoder, bool windowsFormat)
        {
            byte[] encodedBytes = encoder.GetBytes(inputText);
            foreach (Byte b in encodedBytes)
            {
                fs.WriteByte(b);
            }
            if (windowsFormat)
            {
                fs.WriteByte((byte)13);
                if (encoder is UnicodeEncoding)
                    fs.WriteByte((byte)0);
            }
            fs.WriteByte((byte)10);
            if (encoder is UnicodeEncoding)
                fs.WriteByte((byte)0);
        }

        public static void readClipboardInput(TextReader text, DataTable table, Char[] splitChar)
        {
            string sFormattedData;
            int iLoopCounter;
            DataRow rowNew;
            string[] arrSplitData;
            string lastRow;
            while (text.Peek() > 0)
            {
                sFormattedData = text.ReadLine();
                lastRow = sFormattedData;
                if (!sFormattedData.StartsWith("#") && sFormattedData.Length > 0)
                {
                    arrSplitData = sFormattedData.Split(splitChar);
                    if (table.Columns.Count < arrSplitData.Count())
                    {
                        int moreColumns = arrSplitData.Count() - table.Columns.Count;
                        for (iLoopCounter = 0; iLoopCounter < moreColumns; iLoopCounter++)
                        {
                            table.Columns.Add();
                        }
                    }
                    rowNew = table.NewRow();
                    for (iLoopCounter = 0; iLoopCounter <= arrSplitData.GetUpperBound(0); iLoopCounter++)
                    {
                        rowNew[iLoopCounter] = arrSplitData.GetValue(iLoopCounter);
                    }
                    table.Rows.Add(rowNew);
                }
            }
        }

        private void fileTableImport_Click(object sender, EventArgs e)
        {
            importTableFromFile(this, tblExcel2WinData);
            dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
        }

        public static void importTableFromFile(mainForm mainForm, DataTable InputTable)
        {
            importForm importForm1 = new importForm(mainForm);
            DialogResult result = importForm1.ShowDialog();
            if (result == DialogResult.Cancel) return;
            result = MessageBox.Show(mainForm.programmStrings.GetString("textImportTable1"), mainForm.programmStrings.GetString("textImportTable2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            InputTable.Clear();
            InputTable.Columns.Clear();
            try
            {
                if(importForm1.ImportType == ImportType.File) {
                    using (StreamReader srImportFile = new StreamReader(importForm1.importFilePath,importForm1.ImportEncoding))
                    {
                        readClipboardInput(srImportFile, InputTable, importForm1.SeparatorChar);
                    }
                }
                else
                {
                    IDataObject objClipboard = Clipboard.GetDataObject();
                    if (objClipboard.GetDataPresent(DataFormats.Text))
                    {
                        using (StreamReader srImportData = new StreamReader(objClipboard.GetData(DataFormats.CommaSeparatedValue) as Stream, importForm1.ImportEncoding))
                        {
                            readClipboardInput(srImportData, InputTable, importForm1.SeparatorChar);
                        }   
                    }
                }
                if (importForm1.firstRowHeader)
                    headerInFirstRow(InputTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(mainForm.programmStrings.GetString("textErrorUnexpected") + "\r\n" + ex.Message, mainForm.programmStrings.GetString("textImportTable2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void createMakroMenu(List<makroKeys> makroKeys)
        {
            if(makrosMenu == null) {
                return;
            }
            makrosMenu.DropDownItems.Clear();
            makrosMenu.DropDownItems.AddRange(new ToolStripItem[] {
                this.groupDefaultHeader,
                this.toolStripSeparator6 });
            foreach (makroKeys thisKey in makroKeys)
            {
                ToolStripItem newKey = new ToolStripMenuItem();
                newKey.Name = thisKey.KeyName.Replace(" ","");
                newKey.Size = new System.Drawing.Size(237, 22);
                newKey.Text = thisKey.KeyName;
                newKey.Click += new EventHandler(this.MakroKey_Click);
                makrosMenu.DropDownItems.Add(newKey);
            }
        }

        private void dgrExcelContents_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            statusLabel.Text = this.programmStrings.GetString("textEntities") + tblExcel2WinData.Rows.Count.ToString();
        }

        private void MakroKey_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem)) return;
            ToolStripMenuItem thisItem = sender as ToolStripMenuItem;
            makroKeys thisKey = programOptions.GetMakroKey(thisItem.Text);
            if (thisKey == null)
                return;
            if (thisKey.TargetTextBox == "Define")
            {
                DialogResult result = MessageBox.Show(this.programmStrings.GetString("textImportTemplate1"), this.programmStrings.GetString("textImportTemplate2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
                loadVorlage(thisKey.KeyTarget, "DEFINE");
            }
            else if (thisKey.TargetTextBox == "Table")
            {
                DialogResult result = MessageBox.Show(this.programmStrings.GetString("textImportTable1"), this.programmStrings.GetString("textImportTable2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
                loadVorlage(thisKey.KeyTarget, "TABLE");
            }
            else
            {
                if (selectedNode == null)
                {
                    DialogResult result = MessageBox.Show(this.programmStrings.GetString("textErrorNoOutputMakro1"), this.programmStrings.GetString("textErrorNoOutputMakro2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                        return;
                }
                loadVorlage(thisKey.KeyTarget, thisKey.TargetTextBox.ToUpper());
            }
        }

        private void options_Click(object sender, EventArgs e)
        {
            optionsForm optionsForm = new optionsForm(this);
            if (optionsForm.ShowDialog() != DialogResult.OK) return;
            this.programOptions.WriteOptions();
        }

        private void clearAll_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this.programmStrings.GetString("textImportTemplate1"), this.programmStrings.GetString("textImportTemplate2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            newDocument();
        }

        private void newDocument() {
            tblExcel2WinData = new DataTable();
            dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
            trvVorlageSelect.Nodes.Clear();
            nodeTypen = new Dictionary<TreeNode, VorlagenType>();
            if (nodeTextBoxes != null)
            {
                foreach (KeyValuePair<TreeNode, RichTextBox> thisPair in nodeTextBoxes)
                {
                    splitContainer1.Panel2.Controls.Remove(thisPair.Value);
                }
            }
            nodeTextBoxes = new Dictionary<TreeNode, RichTextBox>();
            nodeOutputFile = new Dictionary<TreeNode, OutputFile>();
            pnlFileProperties.Visible = false;
            this.Text = "Script Builder - " + this.programmStrings.GetString("textNewTemplate");
            currentTemplatePath = "";
            Application.DoEvents();
        }

        private void docConvert_Click(object sender, EventArgs e)
        {
            string convertedText = "";
            if (activeTextBox == null)
                return;
            foreach (string textLine in activeTextBox.Lines)
            {
                if (convertedText != "")
                    convertedText += "\r\n";
                convertedText += textLine;
            }
            activeTextBox.Text = convertedText;
        }

        private RichTextBox activeTextBox
        {
            get {
                if(selectedNode != null && nodeTextBoxes.ContainsKey(selectedNode))
                    return nodeTextBoxes[selectedNode];
                else
                    return null;
                
            }
        }

        private void wrongDotNetVersion()
        {
            this.Dispose();
            Application.Exit();
        }

        public static void headerInFirstRow(DataTable InputTable)
        {
            if (InputTable.Rows.Count > 0 && InputTable.Columns.Count > 0)
            {
                for (int i = 0; i < InputTable.Columns.Count; i++)
                {
                    if (InputTable.Rows[0].Field<string>(i) != null && InputTable.Rows[0].Field<string>(i).Trim() != "" && !InputTable.Columns.Contains(InputTable.Rows[0].Field<string>(i).Trim().Replace(" ", "_")))
                        InputTable.Columns[i].ColumnName = InputTable.Rows[0].Field<string>(i).Trim().Replace(" ", "_");
                }
                InputTable.Rows[0].Delete();
            }
        }

        private void firstRowHeader_Click(object sender, EventArgs e)
        {
            headerInFirstRow(tblExcel2WinData);
        }

        private void vorlagenExport_Click(object sender, EventArgs e)
        {
            activateControlls(false);
            string Ausgabedateiname = "";
            string workLineText;
            string startPath;
            string caption;
            DialogResult result;
            SaveFileDialog saveOutput = new SaveFileDialog();
            switch (programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathTemplate";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            saveOutput.InitialDirectory = programOptions.GetOptions(startPath).Value;
            saveOutput.OverwritePrompt = true;
            saveOutput.ValidateNames = true;
            saveOutput.SupportMultiDottedExtensions = true;
            saveOutput.AddExtension = true;
            saveOutput.Filter = this.programmStrings.GetString("filterTemplateXml");
            //saveOutput.Filter += "|" + this.programmStrings.GetString("filterTemplate");
            saveOutput.DefaultExt = "stx";
            saveOutput.FilterIndex = 1;
            saveOutput.Title = this.programmStrings.GetString("textExportTemplate1");
            if (currentTemplatePath != "")
                saveOutput.FileName = Path.GetFileNameWithoutExtension(currentTemplatePath) + ".stx";
            result = saveOutput.ShowDialog();
            if (result == DialogResult.Cancel || result == DialogResult.Abort)
            {
                activateControlls(true);
                return;
            }
            Ausgabedateiname = saveOutput.FileName.ToString();
            CurrentPath = Path.GetDirectoryName(Ausgabedateiname);
            if (programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                programOptions.SetOptions(startPath, CurrentPath);
            if (Path.GetExtension(saveOutput.FileName) == ".txt")
            {
                Encoding encoderOutput = Encoding.Default;
                try
                {
                    using (FileStream sw = new FileStream(Ausgabedateiname, FileMode.Create))
                    {
                        if (tblExcel2WinData.Columns.Count > 0)
                        {
                            writeLines("# Table", sw, encoderOutput, true);
                            createCSVValues(sw, encoderOutput, tblExcel2WinData);
                        }
                        foreach (TreeNode thisFile in trvVorlageSelect.Nodes)
                        {
                            OutputFile thisOutputFile;
                            if (nodeOutputFile.ContainsKey(thisFile))
                                thisOutputFile = nodeOutputFile[thisFile];
                            else
                                thisOutputFile = new OutputFile(thisFile.Name, startPath, 3);
                            if (!thisOutputFile.SerialFile)
                                writeLines("# File " + Path.Combine(thisOutputFile.FilePath, thisOutputFile.FileName).ToString(), sw, encoderOutput, true);
                            else
                                writeLines("# Serial " + Path.Combine(thisOutputFile.FilePath, thisOutputFile.FileName).ToString(), sw, encoderOutput, true);
                            writeLines("FileType=" + thisOutputFile.FileType.ToString(), sw, encoderOutput, true);
                            if (thisOutputFile.SerialFile)
                            {
                                if (thisOutputFile.SerialFieldLinkA != null && thisOutputFile.SerialFieldLinkA != "")
                                    writeLines("LinkTableA=" + thisOutputFile.SerialFieldLinkA, sw, encoderOutput, true);
                                if (thisOutputFile.SerialFieldLinkB != null && thisOutputFile.SerialFieldLinkB != "")
                                    writeLines("LinkTableB=" + thisOutputFile.SerialFieldLinkB, sw, encoderOutput, true);
                                if (thisOutputFile.SerialInputTable.Columns.Count > 0)
                                {
                                    writeLines("SerialSourceTable", sw, encoderOutput, true);
                                    createCSVValues(sw, encoderOutput, thisOutputFile.SerialInputTable);
                                }
                            }
                            foreach (TreeNode thisTemplate in thisFile.Nodes)
                            {
                                if (nodeTypen.ContainsKey(thisTemplate) && nodeTextBoxes.ContainsKey(thisTemplate))
                                {
                                    if (nodeTypen[thisTemplate] == VorlagenType.Header)
                                        caption = "# Simple " + thisTemplate.Text;
                                    else if (nodeTypen[thisTemplate] == VorlagenType.Body)
                                        //caption = "# Multi " + thisTemplate.Text;
                                        caption = "# Repeated " + thisTemplate.Text;
                                    else if (nodeTypen[thisTemplate] == VorlagenType.SerialHeader)
                                        caption = "# SerialHeader " + thisTemplate.Text;
                                    else
                                        caption = "# Unknown " + thisTemplate.Text;
                                    writeLines(caption, sw, encoderOutput, true);

                                    foreach (string lineText in nodeTextBoxes[thisTemplate].Lines)
                                    {
                                        workLineText = lineText;
                                        if (workLineText.StartsWith("#") || workLineText.StartsWith("\\"))
                                            workLineText = "\\" + lineText;
                                        writeLines(workLineText, sw, encoderOutput, true);
                                    }
                                }
                            }
                        }
                    }
                    currentTemplatePath = Ausgabedateiname;
                    this.Text = "Script Builder - [" + Path.GetFileName(currentTemplatePath) + "]";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.programmStrings.GetString("textErrorUnexpected") + "\r\n" + ex.Message, this.programmStrings.GetString("textExportTemplate2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Path.GetExtension(saveOutput.FileName) == ".stx")
            {
                try
                {
                    using (XmlTextWriter stxWriter = new XmlTextWriter(saveOutput.FileName , System.Text.Encoding.UTF8))
                    {
                        stxWriter.WriteStartDocument();
                        stxWriter.Formatting = Formatting.Indented;
                        stxWriter.WriteComment("Script Builder Template File");
                        //Begin Template Element
                        stxWriter.WriteStartElement("Template");
                        stxWriter.WriteAttributeString("Build", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                        //Begin Table Element
                        stxWriter.WriteStartElement("Table");
                        if (tblExcel2WinData.Columns.Count > 0)
                            createXMLTable(stxWriter, tblExcel2WinData); //Create Table in XML Format
                        //End Table Element
                        stxWriter.WriteEndElement();

                        foreach (TreeNode thisFile in trvVorlageSelect.Nodes)
                        {
                            OutputFile thisOutputFile;
                            if (nodeOutputFile.ContainsKey(thisFile))
                                thisOutputFile = nodeOutputFile[thisFile];
                            else
                                thisOutputFile = new OutputFile(thisFile.Name, startPath, 3);
                            if (!thisOutputFile.SerialFile)
                                stxWriter.WriteStartElement("File"); //Write File Element
                            else
                                stxWriter.WriteStartElement("Serial"); //Write Serial File Element
                            //Write Attribute FileName
                            stxWriter.WriteAttributeString("FileName", Path.Combine(thisOutputFile.FilePath, thisOutputFile.FileName).ToString());
                            //Write Attribute FileType
                            stxWriter.WriteAttributeString("FileType",thisOutputFile.FileType.ToString());
                            
                            //Write Serial Table when Serial File
                            if (thisOutputFile.SerialFile)
                            {
                                //Begin SerialSourceTable Element
                                stxWriter.WriteStartElement("SerialSourceTable");
                                //When LinkTableA is set write Attribute LinkTableA
                                if (thisOutputFile.SerialFieldLinkA != null && thisOutputFile.SerialFieldLinkA != "")
                                    stxWriter.WriteAttributeString("LinkTableA", thisOutputFile.SerialFieldLinkA);
                                //When LinkTableA is set write Attribute LinkTableA
                                if (thisOutputFile.SerialFieldLinkB != null && thisOutputFile.SerialFieldLinkB != "")
                                    stxWriter.WriteAttributeString("LinkTableB", thisOutputFile.SerialFieldLinkB);
                                if (thisOutputFile.SerialInputTable.Columns.Count > 0)
                                    createXMLTable(stxWriter, thisOutputFile.SerialInputTable); //Create Table in XML Format
                                //End SerialSourceTable Element
                                stxWriter.WriteEndElement();
                            }

                            //Write Contents
                            foreach (TreeNode thisTemplate in thisFile.Nodes)
                            {
                                if (nodeTypen.ContainsKey(thisTemplate) && nodeTextBoxes.ContainsKey(thisTemplate))
                                {
                                    if (nodeTypen[thisTemplate] == VorlagenType.Header)
                                        stxWriter.WriteStartElement("Simple"); //Begin Simple Content Element
                                    else if (nodeTypen[thisTemplate] == VorlagenType.Body)
                                        stxWriter.WriteStartElement("Repeated"); //Begin Repeated Content Element
                                    else if (nodeTypen[thisTemplate] == VorlagenType.SerialHeader)
                                        stxWriter.WriteStartElement("SerialHeader"); //Begin SerialHeader Content Element
                                    else
                                        stxWriter.WriteStartElement("Unknown"); //Begin Unknown Content Element
                                    stxWriter.WriteAttributeString("Name", thisTemplate.Text); //Write Name Attribute

                                    stxWriter.WriteString(nodeTextBoxes[thisTemplate].Text); //Write Text of Contents

                                    //End Content Element
                                    stxWriter.WriteEndElement();
                                }
                            }
                            //End File Element
                            stxWriter.WriteEndElement();
                        }

                        //End Template Element
                        stxWriter.WriteEndElement();
                        stxWriter.WriteEndDocument();
                    }
                    currentTemplatePath = Ausgabedateiname;
                    this.Text = "Script Builder - [" + Path.GetFileName(currentTemplatePath) + "]";
                }
                catch (XmlException expt)
                {
                    MessageBox.Show(this.programmStrings.GetString("textErrorUnexpected") + "\r\n" + expt.Message, this.programmStrings.GetString("textExportTemplate2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            activateControlls(true);
        }

        private void vorlagenImport_Click(object sender, EventArgs e)
        {
            string startPath;
            DialogResult result = MessageBox.Show(this.programmStrings.GetString("textImportTemplate1"), this.programmStrings.GetString("textImportTemplate2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            OpenFileDialog openImportFile = new OpenFileDialog();
            switch (programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathTemplate";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            openImportFile.InitialDirectory = programOptions.GetOptions(startPath).Value;
            openImportFile.AddExtension = true;
            openImportFile.CheckFileExists = true;
            openImportFile.CheckPathExists = true;
            openImportFile.DefaultExt = "txt";
            openImportFile.Multiselect = false;
            openImportFile.ShowReadOnly = false;
            openImportFile.SupportMultiDottedExtensions = true;
            openImportFile.ValidateNames = true;
            openImportFile.Filter = this.programmStrings.GetString("filterTemplateXml");
            openImportFile.Filter += "|" + this.programmStrings.GetString("filterTemplate");
            openImportFile.Filter += "|" + this.programmStrings.GetString("filterKnownFormats") + " (*.*)|*.*";
            openImportFile.FilterIndex = 1;
            openImportFile.Title = this.programmStrings.GetString("textImportTemplate3");
            result = openImportFile.ShowDialog();
            if (result == DialogResult.Cancel) return;
            CurrentPath = Path.GetDirectoryName(openImportFile.FileName);
            if (programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                programOptions.SetOptions(startPath, CurrentPath);
            activateControlls(false);
            importTemplateMode = true;
            //if(openImportFile.FilterIndex == 1)
            //    loadVorlageXml(openImportFile.FileName);
            //else
            loadVorlage(openImportFile.FileName);
            selectedNode = null;
            trvVorlageSelect.SelectedNode = null;
            importTemplateMode = false;
            activateControlls(true);
        }

        private void loadVorlage(string importFileName)
        {
            if(!File.Exists(importFileName))
            {
                MessageBox.Show(this.programmStrings.GetString("textImportTemplate4"),this.programmStrings.GetString("textImportTemplate2"),MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                XmlDocument thisXmlTemplate = new XmlDocument();
                thisXmlTemplate.Load(importFileName);
                loadVorlage(thisXmlTemplate, importFileName);
            }
            catch
            {
                loadVorlage(importFileName, "");
            }
        }

        private void loadVorlage(string importFileName, string fileFormat)
        {
            int fileFormatID; //0=OPEN/DEFINE; 1=TABLE; 2=HEADER/ONCE/SIMPE; 3=BODY/MULTI; 4=FOOTER
            switch (fileFormat)
            {
                case "TABLE":
                    fileFormatID = 1;
                    tblExcel2WinData = new DataTable();
                    break;
                case "ONCE":
                case "SIMPLE":
                case "HEADER":
                    fileFormatID = 2;
                    break;
                case "MULTI":
                case "REPEATED":
                case "BODY":
                    fileFormatID = 3;
                    break;
                case "FOOTER":
                    fileFormatID = 4;
                    break;
                default:
                    fileFormatID = 0;
                    break;
            }
            try
            {
                if (fileFormatID == 0)
                {
                    newDocument();
                }
                using (StreamReader sr = new StreamReader(importFileName,Encoding.Default))
                {
                    string readData;
                    int dataType = fileFormatID; //0=OPEN/DEFINE/NONE; 1=TABLE; 2=HEADER/ONCE; 3=BODY/MULTI; 4=FOOTER; 5=FILE; 6=SERIAL; 7=SERIALTABLE; 8=SERIALHEADER
                    bool table = false;
                    TreeNode newNode = null;
                    TreeNode fileNode = null;
                    if (fileFormatID > 1)
                    {
                        VorlagenType vorlagenType;
                        if (fileFormat == "BODY" || fileFormat == "MULTI" || fileFormat == "REPEATED")
                            vorlagenType = VorlagenType.Body;
                        else
                            vorlagenType = VorlagenType.Header;
                        if (selectedNode != null && nodeTypen.ContainsKey(selectedNode))
                        {
                            if (nodeTypen[selectedNode] != VorlagenType.File)
                                fileNode = selectedNode.Parent;
                            else
                                fileNode = selectedNode;
                        }
                        else
                            fileNode = addTemplate(null, VorlagenType.File);
                        newNode = addTemplate(fileNode, vorlagenType);
                    }
                    while (sr.Peek() > 0)
                    {
                        readData = sr.ReadLine();
                        if (readData.StartsWith("#") && fileFormatID == 0)
                        {
                            if (dataType == 7)
                            {
                                headerInFirstRow(nodeOutputFile[fileNode].SerialInputTable);
                            }
                            if (readData.ToUpper() == "# TABLE")
                                dataType = 1;
                            else if (readData.ToUpper().StartsWith("# HEADER") || readData.ToUpper().StartsWith("# BODY") || readData.ToUpper().StartsWith("# FOOTER") || readData.ToUpper().StartsWith("# ONCE") || readData.ToUpper().StartsWith("# MULTI") || readData.ToUpper().StartsWith("# REPEATED") || readData.ToUpper().StartsWith("# SIMPLE") || readData.ToUpper().StartsWith("# SERIALHEADER"))
                            {
                                VorlagenType vorlagenType ;
                                string vorlagenName = "";
                                int nodeIcon = 0;
                                string basicNodeName = "";
                                if (fileNode == null)
                                {
                                    fileNode = addTemplate(null, VorlagenType.File);
                                }
                                if (readData.ToUpper().StartsWith("# HEADER"))
                                {
                                    vorlagenType = VorlagenType.Header;
                                    basicNodeName = this.programmStrings.GetString("textBasicNodeNameSimple");
                                    dataType = 2;
                                }
                                else if (readData.ToUpper().StartsWith("# ONCE") || readData.ToUpper().StartsWith("# SIMPLE"))
                                {
                                    vorlagenType = VorlagenType.Header;
                                    if (readData.ToUpper().StartsWith("# ONCE") && readData.Length > 7)
                                        vorlagenName = readData.Substring(7);
                                    else if (readData.ToUpper().StartsWith("# SIMPLE") && readData.Length > 9)
                                        vorlagenName = readData.Substring(9);
                                    basicNodeName = this.programmStrings.GetString("textBasicNodeNameSimple");
                                    dataType = 2;
                                }
                                else if (readData.ToUpper().StartsWith("# BODY"))
                                {
                                    vorlagenType = VorlagenType.Body;
                                    nodeIcon = 1;
                                    basicNodeName = this.programmStrings.GetString("textBasicNodeNameMulti");
                                    dataType = 3;
                                }
                                else if (readData.ToUpper().StartsWith("# MULTI") || readData.ToUpper().StartsWith("# REPEATED"))
                                {
                                    vorlagenType = VorlagenType.Body;
                                    if (readData.ToUpper().StartsWith("# MULTI") && readData.Length > 8)
                                        vorlagenName = readData.Substring(8);
                                    else if (readData.ToUpper().StartsWith("# REPEATED") && readData.Length > 11)
                                        vorlagenName = readData.Substring(11);
                                    nodeIcon = 1;
                                    basicNodeName = this.programmStrings.GetString("textBasicNodeNameMulti");
                                    dataType = 3;
                                }
                                else if (readData.ToUpper().StartsWith("# SERIALHEADER"))
                                {
                                    vorlagenType = VorlagenType.SerialHeader;
                                    if (readData.Length > 15)
                                        vorlagenName = readData.Substring(15);
                                    nodeIcon = 5;
                                    basicNodeName = this.programmStrings.GetString("textBasicNodeNameMulti");
                                    dataType = 8;
                                }
                                else
                                {
                                    vorlagenType = VorlagenType.Header;
                                    if (readData.Length > 8)
                                        vorlagenName = readData.Substring(8);
                                    basicNodeName = this.programmStrings.GetString("textBasicNodeNameSimple");
                                    dataType = 4;
                                }
                                if (vorlagenName.Trim() != "")
                                {
                                    newNode = addTemplate(fileNode.Nodes, fileNode, vorlagenType, nodeIcon, vorlagenName.Trim(), basicNodeName, null);
                                }
                                else
                                {
                                    newNode = addTemplate(fileNode, vorlagenType);
                                }
                            }
                            else if (readData.ToUpper().StartsWith("# FILE ") && readData.Length > 7)
                            {
                                OutputFile file = new OutputFile(Path.GetFileName(readData.Substring(7)), Path.GetDirectoryName(readData.Substring(7)), 3);
                                fileNode = addTemplate(trvVorlageSelect.Nodes,null, VorlagenType.File, 3, file.FileName, this.programmStrings.GetString("textBasicNodeNameOutput"), file);
                                dataType = 5;
                            }
                            else if (readData.ToUpper().StartsWith("# SERIAL ") && readData.Length > 9)
                            {
                                OutputFile file = new OutputFile(Path.GetFileName(readData.Substring(9)), Path.GetDirectoryName(readData.Substring(9)), 3, true);
                                fileNode = addTemplate(trvVorlageSelect.Nodes, null,VorlagenType.Serial, 4, file.FileName, this.programmStrings.GetString("textBasicNodeNameOutput"), file);
                                dataType = 6;
                            }
                            else
                                dataType = 0;
                        }
                        else
                        {
                            if (dataType == 5 && fileNode != null && readData.ToUpper().StartsWith("FILETYPE=") && readData.Length > 9)
                            {
                                string fileType = readData.Substring(9).Trim();
                                if (fileType == "0" || fileType == "1" || fileType == "2" || fileType == "3")
                                    nodeOutputFile[fileNode].FileType = Convert.ToInt32(fileType);
                            }
                            else if (dataType == 6 && fileNode != null)
                            {
                                if (readData.ToUpper().StartsWith("FILETYPE=") && readData.Length > 9)
                                {
                                    string fileType = readData.Substring(9).Trim();
                                    if (fileType == "0" || fileType == "1" || fileType == "2" || fileType == "3")
                                        nodeOutputFile[fileNode].FileType = Convert.ToInt32(fileType);
                                }
                                else if (readData.ToUpper().StartsWith("LINKTABLEA=") && readData.Length > 11)
                                {
                                    nodeOutputFile[fileNode].SerialFieldLinkA = readData.Substring(11).Trim();
                                }
                                else if (readData.ToUpper().StartsWith("LINKTABLEB=") && readData.Length > 11)
                                {
                                    nodeOutputFile[fileNode].SerialFieldLinkB = readData.Substring(11).Trim();
                                }
                                else if (readData.ToUpper().StartsWith("SERIALSOURCETABLE") && readData.Length == 17)
                                {
                                    dataType = 7;
                                }
                            }
                            else if (((dataType > 1 && dataType < 5) || dataType == 8) && newNode != null && nodeTextBoxes.ContainsKey(newNode))
                            {
                                if (readData.StartsWith("\\") && fileFormatID == 0)
                                    readData = readData.Substring(1);
                                if (nodeTextBoxes[newNode].Lines.Count() > 0)
                                    nodeTextBoxes[newNode].AppendText(Environment.NewLine);
                                nodeTextBoxes[newNode].AppendText(readData);
                            }
                            else if (dataType == 1)
                            {
                                using (StringReader srReadText = new StringReader(readData))
                                {
                                    readClipboardInput(srReadText, tblExcel2WinData, new Char[] { ';' });
                                    table = true;
                                }
                            }
                            else if (dataType == 7)
                            {
                                using (StringReader srReadText = new StringReader(readData))
                                {
                                    readClipboardInput(srReadText, nodeOutputFile[fileNode].SerialInputTable, new Char[] { ';' });
                                }
                            }
                        }
                        Application.DoEvents();
                    }
                    if (table)
                    {
                        headerInFirstRow(tblExcel2WinData);
                        dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
                    }
                    selectedNode = newNode;
                    trvVorlageSelect.SelectedNode = selectedNode;
                }
                if (fileFormatID == 0 && trvVorlageSelect.Nodes.Count == 0 && tblExcel2WinData.Columns.Count == 0 && tblExcel2WinData.Rows.Count == 0)
                {
                    MessageBox.Show(this.programmStrings.GetString("textImportTemplateEmpty"), this.programmStrings.GetString("textImportTemplate2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                trvVorlageSelect.ExpandAll();
                checkTemplate_CheckedChanged(checkTemplate, new EventArgs());
                if (fileFormatID == 0)
                {
                    currentTemplatePath = importFileName;
                    this.Text = "Script Builder - [" + Path.GetFileName(currentTemplatePath) + "]";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.programmStrings.GetString("textErrorUnexpected") + "\r\n" + ex.Message, this.programmStrings.GetString("textImportTemplate2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool containsXmlAttribute(XmlAttributeCollection attributeCollection, string name)
        {
            foreach (XmlAttribute thisAttribute in attributeCollection)
                if(thisAttribute.Name == name)
                    return true;
            return false;
        }

        private bool templateVersionCompatible(string version)
        {
            Version currentVerison = Assembly.GetExecutingAssembly().GetName().Version;
            Version compareVersion = new Version(version);

            if (currentVerison.CompareTo(compareVersion) >= 0)
                return true;
            return false;
        }

        private DataTable loadXmlTable(string xmlTable)
        {
            DataTable tempTable = new DataTable();
            XmlDocument thisTable = new XmlDocument();
            try
            {
                thisTable.LoadXml(xmlTable);
                XmlElement tableRoot = thisTable.DocumentElement;
                foreach (XmlNode thisRow in tableRoot.ChildNodes)
                {
                    int cellCount = 0;
                    List<String> thisDataRow = new List<string>();
                    foreach (XmlNode thisCell in thisRow.ChildNodes)
                    {
                        if (thisCell.Name == "Cell")
                        {
                            cellCount++;
                            if (cellCount > tempTable.Columns.Count)
                                tempTable.Columns.Add();
                            if (thisRow.Name == "Header")
                                tempTable.Columns[cellCount - 1].ColumnName = thisCell.InnerText.Replace(" ", "_");
                            else if (thisRow.Name == "Row")
                                thisDataRow.Add(thisCell.InnerText);
                        }
                    }
                    if (thisDataRow.Count() > 0)
                        tempTable.Rows.Add(thisDataRow.ToArray());
                }
            }
            catch (XmlException expt)
            {
                MessageBox.Show(expt.Message + "\n\r(loadXmlTable)", this.programmStrings.GetString("templateImportXml"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tempTable;
        }

        private void loadXmlFileNode(string xmlNodeText)
        {
            TreeNode fileNode = null;
            VorlagenType thisVorlagenType = VorlagenType.File;
            XmlDocument thisFile = new XmlDocument();
            try
            {
                thisFile.LoadXml(xmlNodeText);
                XmlElement fileRoot = thisFile.DocumentElement;
                if (fileRoot.Attributes.Count == 2 && containsXmlAttribute(fileRoot.Attributes, "FileName") && containsXmlAttribute(fileRoot.Attributes, "FileType"))
                {
                    int nodeIcon = 3;
                    if (fileRoot.Name == "Serial")
                    {
                        thisVorlagenType = VorlagenType.Serial;
                        nodeIcon = 4;
                    }
                    OutputFile file = new OutputFile(Path.GetFileName(fileRoot.Attributes["FileName"].Value), Path.GetDirectoryName(fileRoot.Attributes["FileName"].Value), Convert.ToInt32(fileRoot.Attributes["FileType"].Value), fileRoot.Name == "Serial");
                    fileNode = addTemplate(trvVorlageSelect.Nodes, null, thisVorlagenType, nodeIcon, file.FileName, this.programmStrings.GetString("textBasicNodeNameOutput"), file);

                    foreach (XmlNode thisChildNode in fileRoot.ChildNodes)
                    {
                        loadXmlContentNode(thisChildNode.OuterXml, fileNode);
                    }
                }
            }
            catch (XmlException expt)
            {
                MessageBox.Show(expt.Message + "\n\r(loadXmlFileNode)", this.programmStrings.GetString("templateImportXml"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadXmlContentNode(string xmlContent, TreeNode fileNode)
        {
            XmlDocument thisContent = new XmlDocument();
            try
            {
                thisContent.LoadXml(xmlContent);
                XmlElement contentRoot = thisContent.DocumentElement;
                string contentName = "";
                int nodeIcon = 0;
                string basicNodeName = "";
                TreeNode newNode;
                VorlagenType contentType;
                if (containsXmlAttribute(contentRoot.Attributes, "Name"))
                    contentName = contentRoot.Attributes["Name"].Value;
                switch (contentRoot.Name)
                {
                    case "SerialSourceTable":
                        if (nodeOutputFile.ContainsKey(fileNode))
                        {
                            OutputFile tempFile = nodeOutputFile[fileNode];
                            tempFile.SerialInputTable = loadXmlTable(contentRoot.OuterXml);
                            if (containsXmlAttribute(contentRoot.Attributes, "LinkTableA"))
                                tempFile.SerialFieldLinkA = contentRoot.Attributes["LinkTableA"].Value;
                            if (containsXmlAttribute(contentRoot.Attributes, "LinkTableB"))
                                tempFile.SerialFieldLinkB = contentRoot.Attributes["LinkTableB"].Value;

                        }
                        return;
                    case "Repeated":
                        contentType = VorlagenType.Body;
                        nodeIcon = 1;
                        basicNodeName = this.programmStrings.GetString("textBasicNodeNameMulti");
                        break;
                    case "SerialHeader":
                        contentType = VorlagenType.SerialHeader;
                        nodeIcon = 5;
                        basicNodeName = this.programmStrings.GetString("textBasicNodeNameMulti");
                        break;
                    default: //Simple
                        contentType = VorlagenType.Header;
                        basicNodeName = this.programmStrings.GetString("textBasicNodeNameSimple");
                        break;
                }
                if (contentName != "")
                    newNode = addTemplate(fileNode.Nodes, fileNode, contentType, nodeIcon, contentName, basicNodeName, null);
                else
                    newNode = addTemplate(fileNode, contentType);
                if (contentRoot.InnerText.Length > 0 && newNode != null && nodeTextBoxes.ContainsKey(newNode))
                    nodeTextBoxes[newNode].Text = contentRoot.InnerText;
            }
            catch (XmlException expt)
            {
                MessageBox.Show(expt.Message + "\n\r(loadXmlContent)", this.programmStrings.GetString("templateImportXml"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadVorlage(XmlDocument thisXmlTemplate, string importFileName)
        {
            newDocument();
            try
            {
                //XmlDocument thisXmlTemplate = new XmlDocument();
                //thisXmlTemplate.Load(importFileName);
                XmlElement templateRoot = thisXmlTemplate.DocumentElement;
                if (templateRoot.Name != "Template")
                {
                    MessageBox.Show(this.programmStrings.GetString("templateImportErrorXml1"), this.programmStrings.GetString("templateImportXml"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (templateRoot.Attributes.Count == 0 || !containsXmlAttribute(templateRoot.Attributes, "Build") || !templateVersionCompatible(templateRoot.Attributes["Build"].Value))
                {
                    MessageBox.Show(this.programmStrings.GetString("templateImportErrorXml2"), this.programmStrings.GetString("templateImportXml"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (XmlNode thisChildNode in templateRoot.ChildNodes)
                {
                    if (thisChildNode.Name == "Table")
                    {
                        tblExcel2WinData = loadXmlTable(thisChildNode.OuterXml);
                        dgrExcelContents.DataSource = tblExcel2WinData.DefaultView;
                    }
                    else if (thisChildNode.Name == "Serial" || thisChildNode.Name == "File")
                        loadXmlFileNode(thisChildNode.OuterXml);
                }
                trvVorlageSelect.ExpandAll();
                currentTemplatePath = importFileName;
                this.Text = "Script Builder - [" + Path.GetFileName(currentTemplatePath) + "]";
            }
            catch (XmlException expt)
            {
                MessageBox.Show(expt.Message + "\n\r(loadVorlageXml)", this.programmStrings.GetString("templateImportXml"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fileTableExport_Click(object sender, EventArgs e)
        {
            activateControlls(false);
            exportTableToFile(this, tblExcel2WinData);
            activateControlls(true);
        }
        
        public static void exportTableToFile(mainForm mainForm, DataTable InputTable)
        {
            string ausgabeDateiname = "";
            string startPath;
            DialogResult result;
            SaveFileDialog saveOutput = new SaveFileDialog();
            switch (mainForm.programOptions.GetOptions("StartPathType").Value)
            {
                case "GroupsLast":
                    startPath = "StartPathTable";
                    break;
                default:
                    startPath = "StartPath";
                    break;
            }
            saveOutput.InitialDirectory = mainForm.programOptions.GetOptions(startPath).Value;
            saveOutput.OverwritePrompt = true;
            saveOutput.ValidateNames = true;
            saveOutput.SupportMultiDottedExtensions = true;
            saveOutput.AddExtension = true;
            saveOutput.Filter = mainForm.programmStrings.GetString("filterTable") + " (*.csv)|*.csv";
            saveOutput.DefaultExt = "csv";
            saveOutput.FilterIndex = 1;
            saveOutput.Title = mainForm.programmStrings.GetString("textExportTable2");
            result = saveOutput.ShowDialog();
            if (result == DialogResult.Cancel || result == DialogResult.Abort)
                return;
            ausgabeDateiname = saveOutput.FileName.ToString();
            mainForm.CurrentPath = Path.GetDirectoryName(ausgabeDateiname);
            if (mainForm.programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                mainForm.programOptions.SetOptions(startPath, mainForm.CurrentPath);
            Encoding encoderOutput = Encoding.Default;
            try
            {
                using (FileStream sw = new FileStream(ausgabeDateiname, FileMode.Create))
                {
                    createCSVValues(sw, encoderOutput, InputTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(mainForm.programmStrings.GetString("textErrorUnexpected") + "\r\n" + ex.Message, mainForm.programmStrings.GetString("textExportTable1"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void createCSVValues(FileStream sw, Encoding encoderOutput, DataTable InputTable)
        {
            string textLine = "";
            if (InputTable.Columns.Count > 0)
            {
                for (int iSpalten = 0; iSpalten < InputTable.Columns.Count; iSpalten++)
                {
                    if (iSpalten > 0)
                    {
                        textLine += ";";
                    }
                    textLine += InputTable.Columns[iSpalten].ColumnName;
                }
                writeLines(textLine, sw, encoderOutput,true);
                if (InputTable.Rows.Count > 0)
                {
                    for (int iZeilen = 0; iZeilen < InputTable.Rows.Count; iZeilen++)
                    {
                        textLine = "";
                        for (int iSpalten = 0; iSpalten < InputTable.Columns.Count; iSpalten++)
                        {
                            if (iSpalten > 0)
                            {
                                textLine += ";";
                            }
                            textLine += InputTable.Rows[iZeilen].Field<string>(iSpalten);
                        }

                        writeLines(textLine, sw, encoderOutput, true);
                    }
                }
            }
        }

        public static void createXMLTable(XmlTextWriter thisXMLTextWriter, DataTable InputTable)
        {
            if (InputTable.Columns.Count > 0)
            {
                //Write Header Element
                thisXMLTextWriter.WriteStartElement("Header");
                for (int iSpalten = 0; iSpalten < InputTable.Columns.Count; iSpalten++)
                {
                    //Write Cell Element
                    thisXMLTextWriter.WriteStartElement("Cell");
                    //Write Cell Column Name
                    thisXMLTextWriter.WriteString(InputTable.Columns[iSpalten].ColumnName);
                    //End Cell Element
                    thisXMLTextWriter.WriteEndElement();
                }
                //End Header Element
                thisXMLTextWriter.WriteEndElement();
                if (InputTable.Rows.Count > 0)
                {
                    for (int iZeilen = 0; iZeilen < InputTable.Rows.Count; iZeilen++)
                    {
                        //Write Row Element
                        thisXMLTextWriter.WriteStartElement("Row");
                        for (int iSpalten = 0; iSpalten < InputTable.Columns.Count; iSpalten++)
                        {
                            //Write Cell Element
                            thisXMLTextWriter.WriteStartElement("Cell");
                            //Write Cell Column Name
                            thisXMLTextWriter.WriteString(InputTable.Rows[iZeilen].Field<string>(iSpalten));
                            //End Cell Element
                            thisXMLTextWriter.WriteEndElement();
                        }
                        //End Row Element
                        thisXMLTextWriter.WriteEndElement();
                    }
                }
            }
        }

        private void checkTemplate_CheckedChanged(object sender, EventArgs e)
        {
            foreach(KeyValuePair<TreeNode,RichTextBox> thisTextBoxPair in nodeTextBoxes)
            {
                if (nodeTypen.ContainsKey(thisTextBoxPair.Key) && (nodeTypen[thisTextBoxPair.Key] == VorlagenType.Body || nodeTypen[thisTextBoxPair.Key] == VorlagenType.SerialHeader))
                {

                    formatTextbox(thisTextBoxPair.Value, checkTemplate.Checked);
                }
            }
            if (checkTemplate.Checked)
            {
                checkTemplate.Image = Resource1.check_template_on;
            }
            else
            {
                checkTemplate.Image = Resource1.check_template_off;
            }
        }

        private void formatSelection(RichTextBox textBox, int startSelection, int lengthSelection)
        {
            textBox.Select(startSelection, lengthSelection);
            Font boldFont = new Font(textBox.Font, FontStyle.Bold);
            Font normalFont = textBox.Font;
            string checkText = textBox.SelectedText;
            Regex regex;
            string pattern;
            string variableMarker = programOptions.GetOptions("VariableMarker").Value;
            //Kommentare markieren
            pattern = @"(^//.*)|( //.*)";
            regex = new Regex(pattern);
            if (regex.IsMatch(checkText))
            {
                foreach (Match thisMatch in regex.Matches(checkText))
                {
                    textBox.Select(thisMatch.Index + startSelection, thisMatch.Length);
                    textBox.SelectionColor = Color.Green;
                    textBox.SelectionFont = boldFont;
                    //txtVorlage.SelectionBackColor = Color.LightGreen;
                }
            }
            //IF Blocks Markieren
            pattern = @"^/IF.*";
            regex = new Regex(pattern, RegexOptions.Multiline);
            if (regex.IsMatch(checkText))
            {
                foreach (Match thisMatch in regex.Matches(checkText))
                {
                    textBox.Select(thisMatch.Index + startSelection, thisMatch.Length);
                    textBox.SelectionBackColor = Color.Yellow;
                }
            }
            pattern = @"^/ELSE";
            regex = new Regex(pattern, RegexOptions.Multiline);
            if (regex.IsMatch(checkText))
            {
                foreach (Match thisMatch in regex.Matches(checkText))
                {
                    textBox.Select(thisMatch.Index + startSelection, thisMatch.Length);
                    textBox.SelectionBackColor = Color.Yellow;
                }
            }
            pattern = @"^/FI";
            regex = new Regex(pattern, RegexOptions.Multiline);
            if (regex.IsMatch(checkText))
            {
                foreach (Match thisMatch in regex.Matches(checkText))
                {
                    textBox.Select(thisMatch.Index + startSelection, thisMatch.Length);
                    textBox.SelectionBackColor = Color.Yellow;
                }
            }
            //Variablen markieren
            DataTable tempTable = findDataTable(textBox);
            if (tempTable == null)
                return;
            for (int iSpalten = 0; iSpalten < tempTable.Columns.Count; iSpalten++)
            {
                pattern = variableMarker + tempTable.Columns[iSpalten].Caption + @"(\[[0-9$]+[-+]?[0-9$]*,[0-9$]+[-+]?[0-9$]*\])?" + variableMarker;
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch(checkText))
                {
                    foreach (Match thisMatch in regex.Matches(checkText))
                    {
                        textBox.Select(thisMatch.Index + startSelection, thisMatch.Length);
                        textBox.SelectionColor = Color.Blue;
                        textBox.SelectionFont = boldFont;
                        //txtVorlage.SelectionBackColor = Color.LightCyan;
                    }
                }
            }
        }

        private void formatTextbox(RichTextBox textBox, bool checkColors)
        {
            checkingText = true;
            Font boldFont = new Font(textBox.Font, FontStyle.Bold);
            Font normalFont = textBox.Font;
            if (checkColors)
            {
                int StartIndex = textBox.SelectionStart;
                textBox.SelectAll();
                textBox.SelectionBackColor = Color.Transparent;
                textBox.SelectionColor = textBox.ForeColor;
                textBox.SelectionFont = textBox.Font;
                formatSelection(textBox, 0, textBox.Text.Length);
                textBox.Select(StartIndex, 0);
                textBox.SelectionBackColor = Color.Transparent;
                textBox.SelectionColor = textBox.ForeColor;
                textBox.SelectionFont = textBox.Font;
                checkingText = false;
            }
            else
            {
                int StartIndex = textBox.SelectionStart;
                textBox.SelectAll();
                textBox.SelectionBackColor = Color.Transparent;
                textBox.SelectionFont = normalFont;
                textBox.SelectionColor = Color.Black;
                textBox.Select(StartIndex, 0);
            }
        }

        private void vorlageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is RichTextBox))
                return;
            RichTextBox thisTextBox = sender as RichTextBox;
            checkingStartSelection = thisTextBox.SelectionStart;
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsText())
                    thisTextBox.SelectedText = Clipboard.GetText();
            }
            else if (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.X))
            {
                copyVorlageTextToClipboard(thisTextBox, e.KeyCode == Keys.X);
            }
            else
            {
                if (thisTextBox.SelectionLength == 0)
                {
                    thisTextBox.SelectionBackColor = Color.Transparent;
                    thisTextBox.SelectionColor = thisTextBox.ForeColor;
                    thisTextBox.SelectionFont = thisTextBox.Font;
                }
                return;
            }
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void vorlageText_TextChanged(object sender, EventArgs e)
        { 
            if (checkingText || !checkTemplate.Checked || !(sender is RichTextBox) || !nodeTypen.ContainsKey(selectedNode) || (nodeTypen[selectedNode] != VorlagenType.Body && nodeTypen[selectedNode] != VorlagenType.SerialHeader))
                return;
            RichTextBox thisTextBox = sender as RichTextBox;
            int curCaret = thisTextBox.SelectionStart;
            int curCaretLenght = thisTextBox.SelectionLength;
            int curLineA;
            int curLineB;
            if (thisTextBox.SelectionStart != checkingStartSelection)
            {
                if (thisTextBox.SelectionStart > checkingStartSelection)
                {
                    curLineA = thisTextBox.GetLineFromCharIndex(checkingStartSelection);
                    curLineB = thisTextBox.GetLineFromCharIndex(thisTextBox.SelectionStart);
                }
                else
                {
                    curLineA = thisTextBox.GetLineFromCharIndex(thisTextBox.SelectionStart);
                    curLineB = thisTextBox.GetLineFromCharIndex(checkingStartSelection);
                }
            }
            else
            {
                curLineA = thisTextBox.GetLineFromCharIndex(curCaret);
                curLineB = thisTextBox.GetLineFromCharIndex(curCaret);
            }
            int lineSelectionStart = thisTextBox.GetFirstCharIndexFromLine(curLineA);
            int lineSelectionLength = 0;
            thisTextBox.SelectionStart = lineSelectionStart;
            if (thisTextBox.Lines.Length > 0 && thisTextBox.Lines.Length >= curLineA && thisTextBox.Lines.Length >= curLineB)
            {
                for(int i = curLineA;i<=curLineB;i++)
                    lineSelectionLength += thisTextBox.Lines[i].Length + 2;
                lineSelectionLength -= 2;
            }
            else
                lineSelectionLength = 0;
            thisTextBox.SelectionLength = lineSelectionLength;
            thisTextBox.SelectionBackColor = Color.Transparent;
            thisTextBox.SelectionColor = thisTextBox.ForeColor;
            thisTextBox.SelectionFont = thisTextBox.Font;
            formatSelection(thisTextBox, lineSelectionStart, lineSelectionLength);
            thisTextBox.SelectionStart = curCaret;
            thisTextBox.SelectionLength = curCaretLenght;
            thisTextBox.SelectionBackColor = Color.Transparent;
            thisTextBox.SelectionColor = thisTextBox.ForeColor;
            thisTextBox.SelectionFont = thisTextBox.Font;

        }

        private void createIfBlock_Click(object sender, EventArgs e)
        {
            string[] variablen = new string[tblExcel2WinData.Columns.Count];
            for (int iSpalte = 0; iSpalte < tblExcel2WinData.Columns.Count; iSpalte++)
                variablen[iSpalte] = tblExcel2WinData.Columns[iSpalte].Caption;
            createIfForm dialog = new createIfForm(variablen, programOptions.GetOptions("VariableMarker").Value, this);
            dialog.ShowDialog();
        }

        private void checkComplexVariable_Click(object sender, EventArgs e)
        {
            checkComplexVariable dialog = new checkComplexVariable(programOptions.GetOptions("VariableMarker").Value);
            dialog.ShowDialog();
        }

        private void cxmVorlageSelect_Opening(object sender, CancelEventArgs e)
        {
            if (selectedNode == null)
            {
                tsmEditName.Enabled = false;
                tsmRemove.Enabled = false;
                tsmAddFile.Enabled = true;
                tsmAddContent.Enabled = false;
                tsmChangeOrder.Enabled = false;
            }
            else
            {
                tsmEditName.Enabled = true;
                tsmRemove.Enabled = true;
                tsmAddFile.Enabled = true;
                tsmChangeOrder.Enabled = true;
                tsmAddSerialBlock.Enabled = false;
                if (selectedNode.PrevNode != null)
                    tsmOrderUp.Enabled = true;
                else
                    tsmOrderUp.Enabled = false;
                if (selectedNode.NextNode != null)
                    tsmOrderDown.Enabled = true;
                else
                    tsmOrderDown.Enabled = false;
                if (nodeTypen.ContainsKey(selectedNode) && (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial))
                {
                    tsmAddContent.Enabled = true;
                    tsmAddSingle.Enabled = true;
                    tsmAddMultiple.Enabled = true;
                    if (nodeTypen[selectedNode] == VorlagenType.Serial)
                        tsmAddSerialBlock.Enabled = true;
                }
                else
                {
                    tsmAddContent.Enabled = false;
                    tsmAddSingle.Enabled = false;
                    tsmAddMultiple.Enabled = false;
                    tsmAddSerialBlock.Enabled = false;
                }
            }
        }

        private void trvVorlageSelect_MouseDown(object sender, MouseEventArgs e)
        {
            if (trvVorlageSelect.GetNodeAt(e.Location) == null)
            {
                if (!changeNodePossible(null))
                {
                    return;
                }
                selectedNode = null;
                showTextBox(null);
                pnlFileProperties.Visible = false;
            }
            else
                if (!nodeSelectionChanged(trvVorlageSelect.GetNodeAt(e.Location)))
                    return;

            trvVorlageSelect.SelectedNode = selectedNode;
            /*if (selectedNode == null)
            {
                mnuChangeNodeName.Enabled = false;
                mnuRemoveNode.Enabled = false;
                mnuAddFile.Enabled = true;
                mnuAddSerial.Enabled = true;
                mnuAddBlock.Enabled = false;
                mnuChangeOrder.Enabled = false;
            }
            else
            {
                mnuChangeNodeName.Enabled = true;
                mnuRemoveNode.Enabled = true;
                mnuAddFile.Enabled = true;
                mnuAddSerial.Enabled = true;
                mnuChangeOrder.Enabled = true;
                if (selectedNode.PrevNode != null)
                    mnuOrderUp.Enabled = true;
                else
                    mnuOrderUp.Enabled = false;
                if (selectedNode.NextNode != null)
                    mnuOrderDown.Enabled = true;
                else
                    mnuOrderDown.Enabled = false;
                if (nodeTypen.ContainsKey(selectedNode) && (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial))
                {
                    mnuAddBlock.Enabled = true;
                    mnuAddOnce.Enabled = true;
                    mnuAddMulti.Enabled = true;
                }
                else
                {
                    mnuAddBlock.Enabled = false;
                    mnuAddOnce.Enabled = false;
                    mnuAddMulti.Enabled = false;
                }
            }*/
            trvVorlageSelect.Invalidate();
            if(selectedNode != null && e.Button == MouseButtons.Right)
            {
                cxmVorlageSelect.Show(trvVorlageSelect, e.Location);
            }
        } 

        private bool nodeSelectionChanged(TreeNode changeSelectedNode)
        {
            if (!changeNodePossible(changeSelectedNode))
            {
                return false;
            }
            selectedNode = changeSelectedNode;
            if (nodeTextBoxes.ContainsKey(selectedNode))
                showTextBox(nodeTextBoxes[selectedNode]);
            else
            {
                showTextBox(null);
                txtNodeName.Text = nodeOutputFile[selectedNode].FileName;
                txtNodePath.Text = nodeOutputFile[selectedNode].FilePath;
                cboNodeFileFormat.SelectedIndex = nodeOutputFile[selectedNode].FileType;
                pnlFileProperties.Visible = true;
                if (nodeOutputFile[selectedNode].SerialFile)
                {
                    btnSerialWizard.Enabled = true;
                    btnSerialWizard.Visible = true;
                }
                else
                {
                    btnSerialWizard.Enabled = false;
                    btnSerialWizard.Visible = false;
                }
                checkValidSerialFileName();
            }
            return true;
        }

        private void tsmEditName_Click(object sender, EventArgs e)
        {
            if (selectedNode != null)
            {
                trvVorlageSelect.LabelEdit = true;
                selectedNode.BeginEdit();
            }
        }

        private void trvVorlageSelect_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null || !checkNodeName(e.Node, e.Label.Trim()))
                e.CancelEdit = true;
            if (!e.CancelEdit && nodeTypen.ContainsKey(e.Node) && (nodeTypen[e.Node] == VorlagenType.File || nodeTypen[e.Node] == VorlagenType.Serial))
            {
                if (nodeOutputFile.ContainsKey(e.Node))
                {
                    if (Directory.Exists(Path.Combine(nodeOutputFile[e.Node].FilePath, e.Label)))
                    {
                        e.CancelEdit = true;
                    }
                    else
                    {
                        nodeOutputFile[e.Node].FileName = e.Label.Trim();
                        txtNodeName.Text = nodeOutputFile[e.Node].FileName;
                    }
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
            trvVorlageSelect.LabelEdit = false;
        }

        private void tsmRemove_Click(object sender, EventArgs e)
        {
            deleteSelectedNode();
        }

        private void deleteSelectedNode()
        {
            if (selectedNode != null)
            {
                string Frage;
                if (nodeTypen.ContainsKey(selectedNode))
                {
                    switch (nodeTypen[selectedNode])
                    {
                        case VorlagenType.Header:
                        case VorlagenType.Footer:
                        case VorlagenType.Body:
                            Frage = this.programmStrings.GetString("textRemoveTemplate1") + " \"" + selectedNode.Text + "\" " + this.programmStrings.GetString("textRemoveTemplate2");
                            break;
                        case VorlagenType.File:
                            Frage = this.programmStrings.GetString("textRemoveFile1") + " \"" + selectedNode.Text + "\" " + this.programmStrings.GetString("textRemoveFile2");
                            break;
                        default:
                            Frage = this.programmStrings.GetString("textRemoveUnknown1") + " \"" + selectedNode.Text + "\" " + this.programmStrings.GetString("textRemoveUnknown2");
                            break;
                    }
                    if (MessageBox.Show(Frage, this.programmStrings.GetString("textRemoveTemplate3"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial)
                        {
                            List<TreeNode> removeNodeTypen = new List<TreeNode>();
                            List<TreeNode> removeNodeTextBoxes = new List<TreeNode>();
                            foreach (TreeNode thisTreeNode in selectedNode.Nodes)
                            {
                                if (nodeTypen.ContainsKey(thisTreeNode))
                                    removeNodeTypen.Add(thisTreeNode);
                                if (nodeTextBoxes.ContainsKey(thisTreeNode))
                                    removeNodeTextBoxes.Add(thisTreeNode);
                            }
                            foreach (TreeNode thisTreeNode in removeNodeTypen)
                                nodeTypen.Remove(thisTreeNode);
                            foreach (TreeNode thisTreeNode in removeNodeTextBoxes)
                            {
                                splitContainer1.Panel2.Controls.Remove(nodeTextBoxes[thisTreeNode]);
                                nodeTextBoxes.Remove(thisTreeNode);
                            }
                            if (nodeOutputFile.ContainsKey(selectedNode))
                                nodeOutputFile.Remove(selectedNode);
                            trvVorlageSelect.Nodes.Remove(selectedNode);
                        }
                        else
                        {
                            if (nodeTypen.ContainsKey(selectedNode))
                                nodeTypen.Remove(selectedNode);
                            if (nodeTextBoxes.ContainsKey(selectedNode))
                            {
                                splitContainer1.Panel2.Controls.Remove(nodeTextBoxes[selectedNode]);
                                nodeTextBoxes.Remove(selectedNode);
                            }
                            selectedNode.Parent.Nodes.Remove(selectedNode);
                        }
                        selectedNode = null;
                        pnlFileProperties.Visible = false;
                    }
                }
                else
                {
                    trvVorlageSelect.Nodes.Remove(selectedNode);
                    selectedNode = null;
                    pnlFileProperties.Visible = false;
                }
            }
        }

        public enum VorlagenType
        {
            Unknown,
            Header,
            Body,
            Footer,
            File,
            Serial,
            SerialHeader
        }

        private void tsmAddFile_Click(object sender, EventArgs e)
        {
            addTemplate(null, VorlagenType.File);
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (programOptions.GetOptions("WriteStartPathOnEnd") != null && (programOptions.GetOptions("WriteStartPathOnEnd").Value == "True" || programOptions.GetOptions("WriteStartPathOnEnd").Value == "Intermediate"))
            {
                programOptions.WriteOptions();
            }
        }

        private void tsmAddSingle_Click(object sender, EventArgs e)
        {
            if (selectedNode != null && nodeTypen.ContainsKey(selectedNode))
            {
                if (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial)
                    addTemplate(selectedNode, VorlagenType.Header);
                else if (nodeTypen[selectedNode] == VorlagenType.Header || nodeTypen[selectedNode] == VorlagenType.Body)
                    addTemplate(selectedNode.Parent, VorlagenType.Header);
            }
        }


        private TreeNode addTemplate(TreeNode basicNode, VorlagenType vorlagenType)
        {
            string newNodeName;
            string basicNodeName;
            int nodeIcon = 0;
            TreeNodeCollection basicTreeNodes;
            if (vorlagenType == VorlagenType.Header && basicNode != null)
            {
                basicNodeName = this.programmStrings.GetString("textBasicNodeNameSimple");
                basicTreeNodes = basicNode.Nodes;
            }
            else if (vorlagenType == VorlagenType.SerialHeader && basicNode != null && nodeTypen.ContainsKey(basicNode) && nodeTypen[basicNode] == VorlagenType.Serial)
            {
                basicNodeName = this.programmStrings.GetString("textBasicNodeNameSerialBlock");
                basicTreeNodes = basicNode.Nodes;
                nodeIcon = 5;
            }
            else if (vorlagenType == VorlagenType.Body && basicNode != null)
            {
                basicNodeName = this.programmStrings.GetString("textBasicNodeNameMulti");
                basicTreeNodes = basicNode.Nodes;
                nodeIcon = 1;
            }
            else if (vorlagenType == VorlagenType.File && basicNode == null)
            {
                basicNodeName = this.programmStrings.GetString("textBasicNodeNameOutput");
                basicTreeNodes = trvVorlageSelect.Nodes;
                nodeIcon = 3;
            }
            else if (vorlagenType == VorlagenType.Serial && basicNode == null)
            {
                basicNodeName = this.programmStrings.GetString("textBasicNodeNameOutput");
                basicTreeNodes = trvVorlageSelect.Nodes;
                nodeIcon = 4;
            }
            else
                return null;
            newNodeName = (vorlagenType == VorlagenType.File || vorlagenType == VorlagenType.Serial) ? basicNodeName + ".txt" : basicNodeName;
            int counter = 0;
            bool found = false;
            do
            {
                found = false;
                foreach (TreeNode treeNode in basicTreeNodes)
                    if (treeNode.Text == newNodeName)
                        found = true;
                if (found)
                    newNodeName = basicNodeName + ++counter + ((vorlagenType == VorlagenType.File || vorlagenType == VorlagenType.Serial) ? ".txt" : "");
            } while (found);
            return addTemplate(basicTreeNodes, basicNode, vorlagenType, nodeIcon, newNodeName, basicNodeName, null);
        }

        private TreeNode addTemplate(TreeNodeCollection basicTreeNodes, TreeNode basicNode, VorlagenType vorlagenType, int nodeIcon, string newNodeName, string basicNodeName, OutputFile outputFile)
        {
            if (basicTreeNodes != null && basicTreeNodes.Count > 0)
                foreach (TreeNode treeNode in basicTreeNodes)
                    if (treeNode.Text == newNodeName)
                        return null;
            TreeNode newNode = new TreeNode(newNodeName, nodeIcon, nodeIcon);
            if (basicNode != null && (vorlagenType == VorlagenType.Header || vorlagenType == VorlagenType.Body || (vorlagenType == VorlagenType.SerialHeader && nodeTypen.ContainsKey(basicNode) && nodeTypen[basicNode] == VorlagenType.Serial)))
            {
                basicTreeNodes.Add(newNode);
                nodeTypen.Add(newNode, vorlagenType);
                RichTextBox newTextBox = new RichTextBox();
                newTextBox.Name = newNodeName + "_TextBox";
                splitContainer1.Panel2.Controls.Add(newTextBox);
                newTextBox.AcceptsTab = true;
                newTextBox.DetectUrls = false;
                newTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
                newTextBox.Location = new System.Drawing.Point(0, 0);
                newTextBox.Size = new System.Drawing.Size(245, 61);
                newTextBox.Text = "";
                newTextBox.KeyDown += new KeyEventHandler(vorlageText_KeyDown);
                newTextBox.TextChanged += new EventHandler(vorlageText_TextChanged);
                newTextBox.ContextMenuStrip = cxmTextBox;
                newTextBox.WordWrap = false;
                nodeTextBoxes.Add(newNode, newTextBox);
                showTextBox(newTextBox);
            }
            else if (vorlagenType == VorlagenType.File)
            {
                string startPath;
                basicTreeNodes.Add(newNode);
                nodeTypen.Add(newNode, vorlagenType);
                switch (programOptions.GetOptions("StartPathType").Value)
                {
                    case "GroupsLast":
                        startPath = "StartPathOutput";
                        break;
                    default:
                        startPath = "StartPath";
                        break;
                }
                if (outputFile == null)
                    nodeOutputFile.Add(newNode, new OutputFile(newNode.Text, programOptions.GetOptions(startPath).Value, 3));
                else
                    nodeOutputFile.Add(newNode, outputFile);
            }
            else if (vorlagenType == VorlagenType.Serial)
            {
                string startPath;
                basicTreeNodes.Add(newNode);
                nodeTypen.Add(newNode, vorlagenType);
                switch (programOptions.GetOptions("StartPathType").Value)
                {
                    case "GroupsLast":
                        startPath = "StartPathOutput";
                        break;
                    default:
                        startPath = "StartPath";
                        break;
                }
                if (outputFile == null)
                {
                    OutputFile tempFile = new OutputFile(newNode.Text, programOptions.GetOptions(startPath).Value, 3, true);
                    nodeOutputFile.Add(newNode, tempFile);
                }
                else
                    nodeOutputFile.Add(newNode, outputFile);
            }
            else
                return null;
            return newNode;
        }

        private void showTextBox(RichTextBox TextBox)
        {
            pnlFileProperties.Visible = false;
            foreach (RichTextBox thisTextBox in nodeTextBoxes.Values)
                thisTextBox.Visible = false;
            if (TextBox != null && !importTemplateMode)
                TextBox.Visible = true;
        }

        private void tsmAddMultiple_Click(object sender, EventArgs e)
        {
            if (selectedNode != null && nodeTypen.ContainsKey(selectedNode))
            {
                if (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial)
                    addTemplate(selectedNode, VorlagenType.Body);
                else if (nodeTypen[selectedNode] == VorlagenType.Header || nodeTypen[selectedNode] == VorlagenType.Body)
                    addTemplate(selectedNode.Parent, VorlagenType.Body);
            }
        }

        private void btnNodeBrowseFilePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = this.programmStrings.GetString("textSelectPath");
            string selectedPath = txtNodePath.Text;
            if (Directory.Exists(selectedPath))
                selectedPath += @"\";            

            folderBrowser.SelectedPath = Path.GetDirectoryName(selectedPath);
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtNodePath.Text = folderBrowser.SelectedPath;
                CurrentPath = folderBrowser.SelectedPath;
                string startPath;
                if (programOptions.GetOptions("StartPathType").Value != "AlwaysSame")
                {
                    switch (programOptions.GetOptions("StartPathType").Value)
                    {
                        case "GroupsLast":
                            startPath = "StartPathTemplate";
                            break;
                        default:
                            startPath = "StartPath";
                            break;
                    }
                    programOptions.SetOptions(startPath, CurrentPath);
                }
            }
        }

        private void checkValidSerialFileName()
        {
            picErrorName.Visible = false;
            if (selectedNode != null && nodeOutputFile.ContainsKey(selectedNode) && nodeOutputFile[selectedNode].SerialFile)
            {
                string[] thisRow = new string[nodeOutputFile[selectedNode].SerialInputTable.Columns.Count];
                string OutputFileName = txtNodeName.Text;
                string variabelMarker;
                if (programOptions.GetOptions("VariableMarker") != null)
                    variabelMarker = programOptions.GetOptions("VariableMarker").Value;
                else
                    variabelMarker = "%";
                for (int iSpalten = 0; iSpalten < thisRow.Length; iSpalten++)
                    OutputFileName = vorlage.patternSearch(OutputFileName, variabelMarker, nodeOutputFile[selectedNode].SerialInputTable.Columns[iSpalten].Caption, "V" + iSpalten.ToString());
                if (OutputFileName == txtNodeName.Text)
                    picErrorName.Visible = true;
            }
        }

        private void fileProperties_Changed(object sender, EventArgs e)
        {
            if (sender == txtNodeName)
            {
                checkValidSerialFileName();
            }

            if (nodeChanged() == true)
            {
                btnNodeChange.Enabled = true;
                btnNodeCancel.Enabled = true;
            }
            else
            {
                btnNodeChange.Enabled = false;
                btnNodeCancel.Enabled = false;
            }
        }

        private bool nodeChanged()
        {
            if (selectedNode == null)
                return false;
            if (nodeOutputFile.ContainsKey(selectedNode))
            {
                if (nodeOutputFile[selectedNode].FileName != txtNodeName.Text ||
                    nodeOutputFile[selectedNode].FilePath != txtNodePath.Text ||
                    nodeOutputFile[selectedNode].FileType != cboNodeFileFormat.SelectedIndex)
                    return true;
                else
                    return false;
            }
            return true;
        }

        private void btnNodeCancel_Click(object sender, EventArgs e)
        {
            declineNodeChanges();
        } 

        private void btnNodeChange_Click(object sender, EventArgs e)
        {
            acceptNodeChanges();
        }

        private void acceptNodeChanges()
        {
            if (selectedNode == null || !checkNodeName(selectedNode, txtNodeName.Text))
                return;
            if (!Directory.Exists(txtNodePath.Text))
            {
                MessageBox.Show(this.programmStrings.GetString("textErrorPathDoentExist1") + " \"" + txtNodePath.Text + "\" " + this.programmStrings.GetString("textErrorPathDoentExist2"), this.programmStrings.GetString("textErrorPathDoentExist3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Directory.Exists(Path.Combine(txtNodePath.Text, txtNodeName.Text)))
            {
                MessageBox.Show(this.programmStrings.GetString("textErrorPathIsDir1") + " \"" + Path.Combine(txtNodePath.Text, txtNodeName.Text) + "\" " + this.programmStrings.GetString("textErrorPathIsDir2"), this.programmStrings.GetString("textErrorPathIsDir3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (nodeOutputFile.ContainsKey(selectedNode))
            {
                nodeOutputFile[selectedNode].FileName = txtNodeName.Text;
                selectedNode.Text = nodeOutputFile[selectedNode].FileName;
                nodeOutputFile[selectedNode].FilePath = txtNodePath.Text;
                nodeOutputFile[selectedNode].FileType = cboNodeFileFormat.SelectedIndex;
            }
            else
            {
                nodeOutputFile.Add(selectedNode, new OutputFile(txtNodeName.Text, txtNodePath.Text, cboNodeFileFormat.SelectedIndex));
            }
            btnNodeCancel.Enabled = false;
            btnNodeChange.Enabled = false;
        }

        private void declineNodeChanges()
        {
            if (selectedNode != null && nodeOutputFile.ContainsKey(selectedNode))
            {
                txtNodeName.Text = nodeOutputFile[selectedNode].FileName;
                txtNodePath.Text = nodeOutputFile[selectedNode].FilePath;
                cboNodeFileFormat.SelectedIndex = nodeOutputFile[selectedNode].FileType;
            }
        }

        private bool checkNodeName(TreeNode TreeNode, string NewName)
        {
            Regex validName = new Regex("^[^?\"/\\\\<>*|:]{1,255}$");
            if (validName.IsMatch(NewName) && !NewName.Contains("\n") && !NewName.Contains("\r"))
            {
                TreeNodeCollection basicNodes;
                if (TreeNode.Parent != null)
                    basicNodes = TreeNode.Parent.Nodes;
                else
                    basicNodes = trvVorlageSelect.Nodes;
                foreach (TreeNode thisTreeNode in basicNodes)
                    if (TreeNode != thisTreeNode && thisTreeNode.Text == NewName)
                    {
                        MessageBox.Show(this.programmStrings.GetString("textErrorFileInTemplate1") + " \"" + NewName + "\" " + this.programmStrings.GetString("textErrorFileInTemplate2"), this.programmStrings.GetString("textErrorPathIsDir3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
            }
            else
            {
                MessageBox.Show(this.programmStrings.GetString("textErrorFileContains1") + " \"" + NewName + "\" " + this.programmStrings.GetString("textErrorFileContains2"), this.programmStrings.GetString("textErrorPathIsDir3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool changeNodePossible(TreeNode NewNode)
        {
            if (NewNode == selectedNode || selectedNode == null)
                return true;
            if (nodeTypen.ContainsKey(selectedNode) && (nodeTypen[selectedNode] == VorlagenType.File || nodeTypen[selectedNode] == VorlagenType.Serial) && nodeChanged())
            {
                if (MessageBox.Show(this.programmStrings.GetString("textUnsavedChanges1"), this.programmStrings.GetString("textUnsavedChanges2"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }
            return true;
        }

        private void tsmOrderUpDown_Click(object sender, EventArgs e)
        {
            changeNodeOrder(sender == tsmOrderUp);
        }

        private void changeNodeOrder(bool changeUp)
        {
            if (selectedNode == null)
                return;
            TreeNodeCollection basicNode;
            TreeNode changeNode;
            TreeNode thisSelectedNode = selectedNode;
            if (changeUp && selectedNode.PrevNode != null)
                changeNode = selectedNode.PrevNode;
            else if (!changeUp && selectedNode.NextNode != null)
                changeNode = selectedNode.NextNode;
            else
                return;
            if (selectedNode.Parent == null)
                basicNode = trvVorlageSelect.Nodes;
            else
                basicNode = selectedNode.Parent.Nodes;
            List<TreeNode> tempTreeNodes = new List<TreeNode>();

            foreach (TreeNode thisNode in basicNode)
            {
                if (thisNode == changeNode)
                    tempTreeNodes.Add(thisSelectedNode);
                else if (thisNode == thisSelectedNode)
                    tempTreeNodes.Add(changeNode);
                else
                    tempTreeNodes.Add(thisNode);
            }
            basicNode.Clear();
            basicNode.AddRange(tempTreeNodes.ToArray());
            trvVorlageSelect.SelectedNode = thisSelectedNode;
            trvVorlageSelect.Invalidate();
        }

        private void showOutputLog_Click(object sender, EventArgs e)
        {
            OutputLog LogWindow = new OutputLog(outputLog.ToArray(),this);
            LogWindow.ShowDialog();
        }

        public void ClearOutputLog()
        {
            outputLog = new List<string>();
        }

        private void cxmTextBox_Opening(object sender, CancelEventArgs e)
        {
            if (selectedNode == null || !nodeTextBoxes.ContainsKey(selectedNode) || !nodeTypen.ContainsKey(selectedNode))
            {
                e.Cancel = true;
                return;
            }
            RichTextBox selectedTextBox = nodeTextBoxes[selectedNode];
            if (selectedTextBox.CanPaste(DataFormats.GetFormat(DataFormats.Text)))
                tsmTextPaste.Enabled = true;
            else
                tsmTextPaste.Enabled = false;
            if (selectedTextBox.SelectedText.Length > 0)
            {
                tsmTextCopy.Enabled = true;
                tsmTextCut.Enabled = true;
            }
            else
            {
                tsmTextCopy.Enabled = false;
                tsmTextCut.Enabled = false;
            }
            DataTable tempTable = selectedDataTable();
            if (nodeTypen.ContainsKey(selectedNode) && (nodeTypen[selectedNode] == VorlagenType.Body || nodeTypen[selectedNode] == VorlagenType.SerialHeader) && tempTable != null && tempTable.Columns.Count > 0)
            {
                tsmTextMenuSeperator.Visible = true;
                tsmAddVariable.Visible = true;
                tsmAddVariable.DropDownItems.Clear();
                ToolStripMenuItem tsmTempVariable;
                for (int iSpalten = 0; iSpalten < tempTable.Columns.Count; iSpalten++)
                {
                    tsmTempVariable = new ToolStripMenuItem(tempTable.Columns[iSpalten].ColumnName);
                    tsmTempVariable.Name = "tsmTextVariable" + tsmTempVariable.Text;
                    tsmTempVariable.Size = new System.Drawing.Size(118, 22);
                    tsmTempVariable.Click += new EventHandler(tsmAddVariable_Click);
                    tsmAddVariable.DropDownItems.Add(tsmTempVariable);
                }
            }
            else
            {
                tsmTextMenuSeperator.Visible = false;
                tsmAddVariable.Visible = false;
                tsmAddVariable.DropDownItems.Clear();
                ToolStripMenuItem tsmTempVariable = new ToolStripMenuItem();
            }
        }

        private DataTable selectedDataTable()
        {
            if (selectedNode.Parent != null && nodeOutputFile.ContainsKey(selectedNode.Parent))
                if (nodeOutputFile[selectedNode.Parent].SerialFile && nodeTypen[selectedNode] == VorlagenType.SerialHeader)
                    return nodeOutputFile[selectedNode.Parent].SerialInputTable;
                else
                    return tblExcel2WinData;
            return null;
        }

        private TreeNode findNodeByTextBox(Dictionary<TreeNode, RichTextBox> dictionary, RichTextBox textbox)
        {
            foreach (KeyValuePair<TreeNode, RichTextBox> thisPair in dictionary)
                if (thisPair.Value == textbox)
                    return thisPair.Key;
            return null;
        }

        private DataTable findDataTable(RichTextBox TextBox)
        {
            TreeNode tempNode = findNodeByTextBox(nodeTextBoxes,TextBox);
            if(tempNode == null)
                return null;
            if (tempNode.Parent != null && nodeOutputFile.ContainsKey(tempNode.Parent))
                if (nodeOutputFile[tempNode.Parent].SerialFile && nodeTypen[tempNode] == VorlagenType.SerialHeader)
                    return nodeOutputFile[tempNode.Parent].SerialInputTable;
                else
                    return tblExcel2WinData;
            return null;
        }

        private void tsmAddVariable_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || !nodeTextBoxes.ContainsKey(selectedNode) || !(sender is ToolStripMenuItem))
                return;
            ToolStripMenuItem thisVariableItem = sender as ToolStripMenuItem;
            RichTextBox selectedTextBox = nodeTextBoxes[selectedNode];
            string variableMarker;
            if (programOptions.GetOptions("VariableMarker") != null)
                variableMarker = programOptions.GetOptions("VariableMarker").Value;
            else
                variableMarker = "%";
            selectedTextBox.SelectedText = variableMarker + thisVariableItem.Text + variableMarker;
        }

        private void tsmTextPaste_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || !nodeTextBoxes.ContainsKey(selectedNode))
                return;
            RichTextBox selectedTextBox = nodeTextBoxes[selectedNode];
            if (selectedTextBox != null && selectedTextBox.CanPaste(DataFormats.GetFormat(DataFormats.Text)))
                selectedTextBox.Paste(DataFormats.GetFormat(DataFormats.Text));
        }

        private void tsmTextCutCopy_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || !nodeTextBoxes.ContainsKey(selectedNode))
                return;
            RichTextBox selectedTextBox = nodeTextBoxes[selectedNode];
            copyVorlageTextToClipboard(selectedTextBox, sender == tsmTextCut);
        }

        private void copyVorlageTextToClipboard(RichTextBox VorlageTextBox, bool cut)
        {
            if (VorlageTextBox.SelectedText.Length > 0)
            {
                Clipboard.SetText(VorlageTextBox.SelectedText);
                if (cut)
                    VorlageTextBox.SelectedText = "";
            }
        }

        private void writeDebugging(string Line)
        {
            using(StreamWriter sw = new StreamWriter(debuggingFileName,true,Encoding.Default))
            {
                sw.WriteLine(DateTime.Now.ToString("dd.mm.yyyy HH:mm:ss") + " " + Line);
            }
        }

        private void tsmAddSerial_Click(object sender, EventArgs e)
        {
            addTemplate(null, VorlagenType.Serial);
        }

        private void btnSerialWizard_Click(object sender, EventArgs e)
        {
            showSerialWizard();
        }

        private void showSerialWizard()
        {
            string[] header = new string[tblExcel2WinData.Columns.Count];
            for (int iSpalte = 0; iSpalte < tblExcel2WinData.Columns.Count; iSpalte++)
                header[iSpalte] = tblExcel2WinData.Columns[iSpalte].Caption;
            if (selectedNode == null || !nodeOutputFile.ContainsKey(selectedNode) || !nodeOutputFile[selectedNode].SerialFile)
                return;            
            serialWizard serialWizard = new serialWizard(this, header, nodeOutputFile[selectedNode].SerialInputTable.Copy(), nodeOutputFile[selectedNode].SerialFieldLinkA, nodeOutputFile[selectedNode].SerialFieldLinkB);
            serialWizard.ShowDialog();
            if (serialWizard.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;
            nodeOutputFile[selectedNode].SerialInputTable = serialWizard.TblSerialTable;
            nodeOutputFile[selectedNode].SerialFieldLinkA = serialWizard.LinkTableA;
            nodeOutputFile[selectedNode].SerialFieldLinkB = serialWizard.LinkTableB;

        }

        private void tsmAddSerialBlock_Click(object sender, EventArgs e)
        {
            if (selectedNode != null && nodeTypen.ContainsKey(selectedNode))
            {
                if (nodeTypen[selectedNode] == VorlagenType.Serial)
                    addTemplate(selectedNode, VorlagenType.SerialHeader);
                else if (selectedNode.Parent != null && nodeTypen.ContainsKey(selectedNode.Parent) && nodeTypen[selectedNode.Parent] == VorlagenType.Serial)
                    addTemplate(selectedNode.Parent, VorlagenType.SerialHeader);
            }
        }

        private void picErrorName_Click(object sender, EventArgs e)
        {
            string tempName1 = numberingFileName(txtNodeName.Text, "1");
            string tempName2 = numberingFileName(txtNodeName.Text, "2");
            string tempNameN = numberingFileName(txtNodeName.Text, "n");
            MessageBox.Show(this.programmStrings.GetString("textInfoSerialFileName") + tempName1 + "\r\n" + tempName2 + "\r\n.\r\n.\r\n.\r\n" + tempNameN, this.programmStrings.GetString("textInfoSerialFileNameCaption"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string numberingFileName(string filename, string number)
        {
            string basicName = "";
            string extName = "";
            for (int charNo = filename.Length - 1; charNo >= 0; charNo--)
                if (filename.Substring(charNo, 1) == ".")
                {
                    basicName = filename.Substring(0, charNo);
                    extName = filename.Substring(charNo);
                }
            if (basicName != "")
                return basicName + number + extName;
            else
                return filename + number;
        }

        private void trvVorlageSelect_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!nodeSelectionChanged(e.Node))
                e.Cancel = true;
        }

        private void stripColumn_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            ToolStripMenuItem tsmTempColumn = sender as ToolStripMenuItem;
            if (tsmTempColumn.Text == "" || !tblExcel2WinData.Columns.Contains(tsmTempColumn.Text) || !tblExcel2WinData.Columns.CanRemove(tblExcel2WinData.Columns[tsmTempColumn.Text]))
                return;
            if (MessageBox.Show(programmStrings.GetString("textStripColumn1") + tsmTempColumn.Text + programmStrings.GetString("textStripColumn2"), programmStrings.GetString("textStripColumn3"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                return;
            tblExcel2WinData.Columns.Remove(tsmTempColumn.Text);
        }

        private void tabelleMenuStrip_DropDownOpening(object sender, EventArgs e)
        {
            spalteEntfernenToolStripMenuItem.Enabled = false;
            if(tblExcel2WinData.Columns.Count > 0)
            {
                ToolStripMenuItem tsmTempColumn;
                spalteEntfernenToolStripMenuItem.DropDownItems.Clear();
                for (int iSpalten = 0; iSpalten < tblExcel2WinData.Columns.Count; iSpalten++)
                {
                    tsmTempColumn = new ToolStripMenuItem(tblExcel2WinData.Columns[iSpalten].ColumnName);
                    tsmTempColumn.Name = "tsmTextColumn" + tsmTempColumn.Text;
                    tsmTempColumn.Size = new System.Drawing.Size(118, 22);
                    tsmTempColumn.Click += new EventHandler(stripColumn_Click);
                    spalteEntfernenToolStripMenuItem.DropDownItems.Add(tsmTempColumn);
                }
                spalteEntfernenToolStripMenuItem.Enabled = true;
            }
        }

        private void trvVorlageSelect_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) // .KeyValue == (char)43) //plus (+)
            {
                changeNodeOrder(true);
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract) //e.KeyValue == (char)45) //minus (-)
            {
                changeNodeOrder(false);
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                deleteSelectedNode();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F2)
            {
                if (selectedNode == null)
                    return;
                trvVorlageSelect.LabelEdit = true;
                selectedNode.BeginEdit();
                e.Handled = true;
                return;
            }
        }
    }
}
