using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Globalization;

namespace Script_Builder
{
    public class option
    {
        private string name;
        private string value;

        public option(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    public class makroKeys
    {
        private string keyName;
        private string keyTarget;
        private string targetTextBox;

        public makroKeys(string keyName, string keyTarget, string targetTextBox)
        {
            this.keyName = keyName;
            this.keyTarget = keyTarget;
            this.targetTextBox = targetTextBox;
            /*
            switch (targetTextBox)
            {
                case "Tabelle":
                    this.targetTextBox = "Tabelle";
                    break;
                case "Einfach (Header, Footer)":
                    this.targetTextBox = "Once";
                    break;
                case "Wiederholt (Body)":
                    this.targetTextBox = "Multi";
                    break;
                case "Wie in Makrodatei definiert":
                    this.targetTextBox = "None";
                    break;
                default:
                    this.targetTextBox = targetTextBox;
                    break;
            }*/
        }

        public string KeyName
        {
            get
            {
                return this.keyName;
            }
        }

        public string KeyTarget
        {
            get
            {
                return this.keyTarget;
            }
        }

        public string TargetTextBox
        {
            get
            {
                return this.targetTextBox;
            }
        }
    }

    public class options
    {
        private string optionsFile;
        private Dictionary<string,option> optionList = new Dictionary<string,option>();
        public Dictionary<int, makroKeys> makroList = new Dictionary<int, makroKeys>();
        private mainForm mainForm;

        public options(string optionsFile, mainForm mainForm)
        {
            this.optionsFile = optionsFile;
            this.mainForm = mainForm;
        }

        public option GetOptions(string optionName)
        {
            if (optionList.ContainsKey(optionName))
                return optionList[optionName];
            else
                return null;
        }

        public void SetOptions(string optionName, string optionValue)
        {
            if (optionList.ContainsKey(optionName))
                optionList[optionName].Value = optionValue;
            else
                optionList.Add(optionName, new option(optionName,optionValue));
        }

        public bool ReadOptions()
        {
            if (!File.Exists(optionsFile))
            {
                if(File.Exists(Path.Combine(Application.StartupPath, "OSV-Scripting-Help_V10.pdf")))
                    SetOptions("DokuPath", Path.Combine(Application.StartupPath, "OSV-Scripting-Help_V10.pdf"));
                int tempMakroIndex = 0;
                if (File.Exists(Path.Combine(Application.StartupPath, "example - DLS - change E164 DisplayID Remark.txt")))
                    SetMakros(tempMakroIndex++.ToString(), "Example - DLS - Change E164", Path.Combine(Application.StartupPath, "example - DLS - change E164 DisplayID Remark.txt").ToString(), "Repeated");
                if (File.Exists(Path.Combine(Application.StartupPath, "example - OSV - Add Subscribers.txt")))
                    SetMakros(tempMakroIndex++.ToString(), "Example - OSV - New Subscriber", Path.Combine(Application.StartupPath, "example - OSV - Add Subscribers.txt").ToString(), "Repeated");
                //SetOptions("Language", "English");
                SetOptions("Language", "Deutsch");
                WriteOptions();
                if(mainForm.programmStrings != null)
                    MessageBox.Show(mainForm.programmStrings.GetString("textOptionsXMLNotFound"), mainForm.programmStrings.GetString("textReadOptions"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("There was no \"options.xml\" file found.\r\nNew preference file was created with default values.", "Read Preferences", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return false;
            }
            try
            {
                XmlDocument options = new XmlDocument();
                string optionsChildName;
                options.Load(optionsFile);
                XmlElement optionsRoot = options.DocumentElement;
                foreach (XmlNode optionsKnoten in optionsRoot.ChildNodes)
                {
                    optionsChildName = optionsKnoten.Name.ToString();
                    if (optionsKnoten.HasChildNodes)
                    {
                        foreach (XmlNode optionsDaten in optionsKnoten.ChildNodes)
                        {
                            switch (optionsChildName)
                            {
                                case "Basic":
                                    SetOptions(optionsDaten.Name.ToString(), optionsDaten.InnerText.ToString());
                                    break;
                                case "MakroKeys":
                                    if (optionsDaten.Name == "MakroKey" && optionsDaten.HasChildNodes && optionsDaten.ChildNodes.Count == 4)
                                    {
                                        string index = "";
                                        string keyName = "";
                                        string keyTarget = "";
                                        string targetTextBox = "";
                                        for (int feldIndex = 0; feldIndex < 4; feldIndex++)
                                        {
                                            switch (optionsDaten.ChildNodes[feldIndex].Name)
                                            {
                                                case "Index":
                                                    index = optionsDaten.ChildNodes[feldIndex].InnerText;
                                                    break;
                                                case "KeyName":
                                                    keyName = optionsDaten.ChildNodes[feldIndex].InnerText;
                                                    break;
                                                case "KeyTarget":
                                                    keyTarget = optionsDaten.ChildNodes[feldIndex].InnerText;
                                                    break;
                                                case "TargetTextBox":
                                                    switch (optionsDaten.ChildNodes[feldIndex].InnerText)
                                                    {
                                                        case "Body":
                                                        case "Multi":
                                                            targetTextBox = "Repeated";
                                                            break;
                                                        case "Header":
                                                        case "Footer":
                                                        case "Once":
                                                            targetTextBox = "Simple";
                                                            break;
                                                        case "Tabelle":
                                                            targetTextBox = "Table";
                                                            break;
                                                        case "None":
                                                            targetTextBox = "Define";
                                                            break;
                                                        default:
                                                            targetTextBox = optionsDaten.ChildNodes[feldIndex].InnerText;
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                        SetMakros(index, keyName, keyTarget, targetTextBox);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (XmlException expt)
            {
                MessageBox.Show(expt.Message, mainForm.programmStrings.GetString("textErrorReadOptions"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bool needWrite = false;
            if (GetOptions("VariableMarker") == null)
            {
                SetOptions("VariableMarker", "%");
                needWrite = true;
            }
            if (GetOptions("StartPathType") == null)
            {
                SetOptions("StartPathType", "AnybodysLast");
                needWrite = true;
            }
            if (GetOptions("Language") == null)
            {
                SetOptions("Language", "Deutsch");
                needWrite = true;
            }
            if ((GetOptions("StartPathType").Value == "AnybodysLast" || GetOptions("StartPathType").Value == "AlwaysSame") && GetOptions("StartPath") == null)
            {
                SetOptions("StartPath", Application.StartupPath.ToString());
            }
            else if (GetOptions("StartPathType").Value == "GroupssLast")
            {
                if (GetOptions("StartPathOptions") == null)
                {
                    SetOptions("StartPathOptions", Application.StartupPath.ToString());
                }
                if (GetOptions("StartPathOutput") == null)
                {
                    SetOptions("StartPathOutput", Application.StartupPath.ToString());
                }
                if (GetOptions("StartPathTemplate") == null)
                {
                    SetOptions("StartPathTemplate", Application.StartupPath.ToString());
                }
                if (GetOptions("StartPathTable") == null)
                {
                    SetOptions("StartPathTable", Application.StartupPath.ToString());
                }
            }
            if(needWrite)
                WriteOptions();
            return true;                
        }

        public void CreateMakroMenu()
        {
            List<makroKeys> tempList = orderKeyList(makroList);
            mainForm.createMakroMenu(tempList);
        }

        private void SetMakros(string index, string keyName, string keyTarget, string targetTextBox)
        {
            if (index == "" || keyName == "" || keyTarget == "" || targetTextBox == "") return;
            int intIndex;
            try
            {
                intIndex = Convert.ToInt16(index);
            }
            catch
            {
                return;
            }
            if (makroList.ContainsKey(intIndex))
            {
                makroList[intIndex] = new makroKeys(keyName, keyTarget, targetTextBox);
            }
            else
            {
                makroKeys testTempKey = new makroKeys(keyName, keyTarget, targetTextBox);
                makroList.Add(intIndex, testTempKey);
            }
        }

        public bool WriteOptions()
        {
            try
            {
                using (XmlTextWriter myXmlTextWriter = new XmlTextWriter(optionsFile, System.Text.Encoding.UTF8))
                {
                    myXmlTextWriter.WriteStartDocument(false);
                    myXmlTextWriter.Formatting = Formatting.Indented;
                    myXmlTextWriter.WriteComment("Options File for Script Builder");
                    myXmlTextWriter.WriteStartElement("Options");
                    myXmlTextWriter.WriteStartElement("Basic");
                    if (GetOptions("VariableMarker") != null)
                        myXmlTextWriter.WriteElementString("VariableMarker", GetOptions("VariableMarker").Value);
                    if (GetOptions("DokuPath") != null)
                        myXmlTextWriter.WriteElementString("DokuPath", GetOptions("DokuPath").Value);
                    if (GetOptions("Debugging") != null)
                        myXmlTextWriter.WriteElementString("Debugging", GetOptions("Debugging").Value);
                    if (GetOptions("Language") != null)
                        myXmlTextWriter.WriteElementString("Language", GetOptions("Language").Value);
                    if (GetOptions("StartPathType") != null)
                    {
                        myXmlTextWriter.WriteElementString("StartPathType", GetOptions("StartPathType").Value);
                        if (GetOptions("StartPathType").Value == "AlwaysSame")
                            myXmlTextWriter.WriteElementString("StartPath", GetOptions("StartPath").Value);
                    }
                    if (GetOptions("WriteStartPathOnEnd") != null)
                    {
                        myXmlTextWriter.WriteElementString("WriteStartPathOnEnd", GetOptions("WriteStartPathOnEnd").Value);
                        if (GetOptions("WriteStartPathOnEnd").Value == "True")
                        {
                            if (GetOptions("StartPathType").Value == "AnybodysLast")
                            {
                                myXmlTextWriter.WriteElementString("StartPath", GetOptions("StartPath").Value);
                            }
                            else if (GetOptions("StartPathType").Value == "GroupsLast")
                            {
                                myXmlTextWriter.WriteElementString("StartPathOptions", GetOptions("StartPathOptions").Value);
                                myXmlTextWriter.WriteElementString("StartPathOutput", GetOptions("StartPathOutput").Value);
                                myXmlTextWriter.WriteElementString("StartPathTemplate", GetOptions("StartPathTemplate").Value);
                                myXmlTextWriter.WriteElementString("StartPathTable", GetOptions("StartPathTable").Value);
                            }
                        }
                    }
                    myXmlTextWriter.WriteEndElement();
                    if (makroList.Count > 0)
                    {
                        myXmlTextWriter.WriteStartElement("MakroKeys");
                        foreach (KeyValuePair<int, makroKeys> thisKey in makroList)
                        {
                            myXmlTextWriter.WriteStartElement("MakroKey");
                            myXmlTextWriter.WriteElementString("Index", thisKey.Key.ToString());
                            myXmlTextWriter.WriteElementString("KeyName", thisKey.Value.KeyName);
                            myXmlTextWriter.WriteElementString("KeyTarget", thisKey.Value.KeyTarget);
                            myXmlTextWriter.WriteElementString("TargetTextBox", thisKey.Value.TargetTextBox);
                            myXmlTextWriter.WriteEndElement();
                        }
                        myXmlTextWriter.WriteEndElement();
                    }
                    myXmlTextWriter.WriteEndElement();
                }
                this.mainForm.createMakroMenu(orderKeyList(makroList));
                return true;
            }
            catch (XmlException expt)
            {
                MessageBox.Show(expt.Message, mainForm.programmStrings.GetString("textErrorWriteOptions"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<makroKeys> orderKeyList(Dictionary<int, makroKeys> inputDictionary)
        {
            List<KeyValuePair<int, makroKeys>> TempKeyList = new List<KeyValuePair<int, makroKeys>>();
            List<makroKeys> outputKeys = new List<makroKeys>();
            foreach (KeyValuePair<int,makroKeys> thisKeyPair in inputDictionary)
            {
                if (TempKeyList.Count > 0)
                {
                    for (int I = 0; I <= TempKeyList.Count; I++)
                    {
                        if (I == TempKeyList.Count)
                        {
                            TempKeyList.Add(thisKeyPair);
                            break;
                        }
                        else if (thisKeyPair.Key < TempKeyList[I].Key)
                        {
                            TempKeyList.Insert(I, thisKeyPair);
                            break;
                        }
                    }
                }
                else
                {
                    TempKeyList.Add(thisKeyPair);
                }
            }
            foreach (KeyValuePair<int, makroKeys> thisKeyPair in TempKeyList)
            {
                outputKeys.Add(thisKeyPair.Value);
            }
            return outputKeys;
        }

        public makroKeys GetMakroKey(string KeyName)
        {
            foreach (KeyValuePair<int, makroKeys> thisKeyPair in makroList)
            {
                if (thisKeyPair.Value.KeyName == KeyName) return thisKeyPair.Value;
            }
            return null;
        }

        public CultureInfo GetCulture()
        {
            if (GetOptions("Language") != null && GetOptions("Language").Value == "English")
                return new CultureInfo("en-US");
            else
                //return new CultureInfo("de-DE");
                return new CultureInfo("");
        }
    }
}
