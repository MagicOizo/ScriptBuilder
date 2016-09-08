using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Script_Builder
{
    public class Strings
    {
        private Dictionary<String, String> standard;
        private Dictionary<String, String> aktive;
        private Dictionary<String, String> cultures;

        public Strings(CultureInfo currentCulture, String StartupPath)
        {
            standard = new Dictionary<string,string>();
            aktive = new Dictionary<string,string>();
            cultures = new Dictionary<string, string>();
            if (File.Exists(Path.Combine(StartupPath, "languagepack.xml")))
            {
                XmlDocument strings = new XmlDocument();
                string stringsChildName;
                try
                {
                    strings.Load(Path.Combine(StartupPath, "languagepack.xml"));
                    XmlElement stringsRoot = strings.DocumentElement;
                    if (stringsRoot.Name != "Languages")
                        throw new XmlException("Root tag is not \"Languages\"");
                    foreach (XmlNode stringsKnoten in stringsRoot.ChildNodes)
                    {
                        stringsChildName = stringsKnoten.Name.ToString();
                        string tempLanguageName = stringsChildName;
                        if (stringsKnoten.Attributes != null && stringsKnoten.Attributes.Count > 0)
                        {
                            foreach(XmlAttribute thisAttribute in stringsKnoten.Attributes)
                            {
                                if(thisAttribute.Name == "name")
                                {
                                    tempLanguageName = thisAttribute.Value;
                                    break;
                                }
                            }
                        }
                        cultures.Add(stringsChildName, tempLanguageName);
                        if (stringsChildName == currentCulture.ToString())
                        {
                            if (stringsKnoten.HasChildNodes)
                            {
                                foreach (XmlNode stringsDaten in stringsKnoten.ChildNodes)
                                {
                                    aktive.Add(stringsDaten.Name.ToString(), stringsDaten.InnerText.ToString());
                                }
                            }
                        }
                    }
                }
                catch (XmlException e)
                {
                    System.Windows.Forms.MessageBox.Show("The languagepack file \"" + Path.Combine(StartupPath, "languagepack.xml").ToString() + "\" has an error:\n\r" + e.Message + "\n\r\n\rSelected language can not be loaded, default german will be used.", "Loading Languagepack", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }

            #region Standardsprache definieren
            standard.Add("makroTargetDefine", "Wie in Makrodatei definiert");
            standard.Add("makroTargetTable", "Tabelle");
            standard.Add("makroTargetSimple", "Einfach (Header, Footer)");
            standard.Add("makroTargetRepeated", "Wiederholt (Body)");
            standard.Add("buttonBrowseDokuCaption", "Dokumentationsdatei wählen...");
            standard.Add("buttonKeyBrowseCaption", "Makrodatei wählen...");
            standard.Add("buttonHinzuExistsMsg", "Ein Makroplatz mit diesem Namen ist bereits vorhanden!");
            standard.Add("buttonHinzuExistsCaption", "Makro hinzufügen");
            standard.Add("buttonRenameExistsCaption", "Makro umbenennen");
            standard.Add("buttonCancelChangesMsg", "Es wurden Änderungen an den Optionen vorgenommen. Sollen die Änderungen verworfen werden?");
            standard.Add("buttonCancelChangesCaption", "Änderungen nicht gesichert");
            standard.Add("textChooseRootDirectory", "Startverzeichniss auswählen...");
            standard.Add("textShallDeleteLog", "Log wirklich löschen?");
            standard.Add("textDeleteLog", "Log löschen");
            standard.Add("textSaveLog", "Output Log speichern");
            standard.Add("filterText", "Text Datei");
            standard.Add("filterCSV", "Komma-Separierte Datei");
            standard.Add("filterKnownFormats", "Alle unterstützten Formate");
            standard.Add("filterPDF", "PDF Datei");
            standard.Add("filterTable", "Tabellen Datei");
            standard.Add("filterTemplate", "Vorlagen Datei (*.txt)|*.txt");
            standard.Add("filterTemplateXml", "Script Builder Vorlage (*.stx)|*.stx");
            standard.Add("textImportTableFrom", "Tabelle importieren aus...");
            standard.Add("textImportTable", "Tabelle importieren");
            standard.Add("textFileDontExits", "Diese Datei existiert nicht.");
            standard.Add("textCustomSeperator", "Kein Custom Trenner angegeben.");
            standard.Add("textInsertIFTemplate", "Bedingte Vorlagenzeilen hier einfügen");
            standard.Add("textInsertELSETemplate", "Alternative Vorlagenzeile hier einfügen");
            standard.Add("textOptionsXMLNotFound", "Es wurde keine \"options.xml\"-Datei gefunden.\r\nNeue Options-Datei mit Standardwerten erstellt.");
            standard.Add("textReadOptions", "Lese Optionen");
            standard.Add("textErrorReadOptions", "Fehler beim öffnen der Options-Datei");
            standard.Add("textErrorWriteOptions", "Fehler beim schreiben der Options-Datei");
            standard.Add("textChangeHeader", "Spaltenüberschrift ändern");
            standard.Add("textNewHeader1", "Neue Spaltenüberschrift:");
            standard.Add("textNewHeader2", "Neuer Spaltenname");
            standard.Add("textErrorHeaderUsed1", "Der Name");
            standard.Add("textErrorHeaderUsed2", "wurde bereits für eine andere Spalte verwendet.");
            standard.Add("textErrorNoOutputFile", "Sie haben keine Outputdatei ausgewählt. Soll eine neue Outputdatei angelegt werden und der Header darin neu angelegt werden?");
            standard.Add("textImportText", "Text importieren");
            standard.Add("textErrorPathIsDir1", "Die Outputdatei");
            standard.Add("textErrorPathIsDir2", "ist ein Verzeichnis.");
            standard.Add("textErrorPathIsDir3", "Ungültiger Dateiname");
            standard.Add("textCreateOutputFile", "Outputdatei erzeugen");
            standard.Add("textErrorPathDoentExist1", "Das Verzeichnis");
            standard.Add("textErrorPathDoentExist2", "existiert nicht.");
            standard.Add("textErrorPathDoentExist3", "Ungültiges Verzeichnis");
            standard.Add("textErrorReplaceOutputFile1", "Die Outputdatei");
            standard.Add("textErrorReplaceOutputFile2", "existiert bereits in dem ausgewählten Verzeichnis.\r\nSoll sie überschrieben werden?");
            standard.Add("logCreateScript1", "Script-Erstellung");
            standard.Add("logCreateScript2", "Script Erstellung...");
            standard.Add("logStart", "Start: ");
            standard.Add("logTimeCode", "dd.MM.yyyy HH:mm:ss");
            standard.Add("logOutputFile", "Output Datei:");
            standard.Add("logBlock", "Block");
            standard.Add("logSingleBlockWith1", "Einzelblock mit ");
            standard.Add("logMultiBlockWith1", "Wiederholungsblock mit ");
            standard.Add("logMultiBlockWith2", " für ");
            standard.Add("logMultiBlockWith3", " Datensätze");
            standard.Add("logMultiBlockWith4", "einen Datensatz");
            standard.Add("logBlockWith2", " Zeilen.");
            standard.Add("logBlockWith3", "einer Zeile.");
            standard.Add("logEnde", "Ende: ");
            standard.Add("logFinishedScriptCreation1", "In ");
            standard.Add("logFinishedScriptCreation2", " wurden ");
            standard.Add("logFinishedScriptCreation3", " Datensätze erstellt");
            standard.Add("logFinishedScriptCreation4", "Erstellung des Scripts abgeschlossen.");
            standard.Add("logFinishWarning1", "ACHTUNG: Bei einigen Output Dateien wurde File Format Basic-ASCII gewählt, Sonderzeichen (>127) wurden durch \"?\" ersetzt.");
            standard.Add("logFinishWarning2", "Bitte Output-Log beachten.");
            standard.Add("logFinishWarning3", "Folgende Output Dateien wurden mit File Format Basic-ASCII erzeugt:");
            standard.Add("textErrorOpenDokument1", "Die PDF-Datei wird mit dem Standardprogramm in einem externen Fenster geöffnet.\r\nBitte haben sie einen Augenblick Geduld");
            standard.Add("textErrorOpenDokument2", "Beim öffnen der Dokumentation ist ein Fehler aufgetreten, bitte überprüfen sie ob die Datei vorhanden ist.");
            standard.Add("textErrorOpenDokument3", "Open Scape Voice Scripting Dokumentation öffnen");
            standard.Add("textImportTable1", "Wollen sie wirklich die aktuelle Tabelle schließen\nund eine neue importieren?");
            standard.Add("textImportTable2", "Tabelle importieren");
            standard.Add("textErrorUnexpected", "Es ist ein unerwarteter Fehler aufgetreten!");
            standard.Add("textEntities", "Datensätze: ");
            standard.Add("textImportTemplate1", "Wollen sie wirklich die aktuelle Vorlage schließen\nund eine neue importieren?");
            standard.Add("textImportTemplate2", "Vorlage importieren");
            standard.Add("textImportTemplate3", "Vorlage importieren aus ...");
            standard.Add("textImportTemplate4", "Datei existiert nicht.");
            standard.Add("textImportTemplateEmpty", "Die importierte Datei enthielt kein verwertbares Script Builder Template.");
            standard.Add("textErrorNoOutputMakro1", "Sie haben keine Outputdatei ausgewählt. Soll eine neue Outputdatei angelegt werden und das Makro darin ausgeführt werden?");
            standard.Add("textErrorNoOutputMakro2", "Text importieren");
            standard.Add("textExportTemplate1", "Vorlage exportieren nach ...");
            standard.Add("textExportTemplate2", "Vorlage exportieren");
            standard.Add("textBasicNodeNameSimple", "Einfach");
            standard.Add("textBasicNodeNameMulti", "Wiederholt");
            standard.Add("textBasicNodeNameOutput", "Output");
            standard.Add("textExportTable1", "Tabelle exportieren");
            standard.Add("textExportTable2", "Tabelle exportieren nach ...");
            standard.Add("textRemoveTemplate1", "Soll die Vorlage");
            standard.Add("textRemoveTemplate2", "wirklich entfernt werden?");
            standard.Add("textRemoveTemplate3", "Vorlage entfernen");
            standard.Add("textRemoveFile1", "Soll die Datei");
            standard.Add("textRemoveFile2", "und alle dazugehörigen Vorlagen wirklich entfernt werden?");
            standard.Add("textRemoveUnknown1", "Soll die unbekannte Vorlage");
            standard.Add("textRemoveUnknown2", "wirklich entfernt werden?");
            standard.Add("textSelectPath", "Ausgabeordner für diese Datei festlegen");
            standard.Add("textErrorFileInTemplate1", "Der Dateiname");
            standard.Add("textErrorFileInTemplate2", "existiert bereits in der Vorlage!\r\nBitte einen anderen Dateinamen wählen.");
            standard.Add("textErrorFileContains1", "Der Dateiname");
            standard.Add("textErrorFileContains2", "enthält ungültige Zeichen.\r\nEin Windows Dateiname darf folgende Zeichen nicht enthalten ? \" / \\ < > * | : und muss zwischen 1 und 255 Zeichen lang sein!");
            standard.Add("textUnsavedChanges1", "There are changes made to the current attribute page, that were not saved.\r\nReject changes?");
            standard.Add("textUnsavedChanges2", "Unsaved Changes");
            standard.Add("textBasicNodeNameSerialBlock", "Serienfeld");
            standard.Add("textInfoSerialFileName", "Der ausgewählte Dateiname bezieht sich nicht auf die Serien-Quelltabelle. Die Dateinamen werden durchnummeriert.\r\n\r\nBeispiel:\r\n");
            standard.Add("textInfoSerialFileNameCaption", "Seriendateiname");
            standard.Add("InfoBoxHeader", "Info über Script Builder");
            standard.Add("InfoBoxDescription", "Tool zum Erstellen von Scripts für Massendaten mit sich häufig wiederholenden Kommandos.\r\n\r\n\r\nIcons by: http://led24.de/");
            standard.Add("ShortcutCtrlEnd", "Strg+Ende");
            standard.Add("ShortcutCtrlS", "Strg+S");
            standard.Add("ShortcutCtrlO", "Strg+O");
            standard.Add("ShortcutCtrlN", "Strg+N");
            standard.Add("ShortcutCtrlP", "Strg+P");
            standard.Add("ShortcutCtrlIns", "Strg+Einfg");
            standard.Add("ShortcutCtrlL", "Strg+L");
            standard.Add("ShortcutCtrlD", "Strg+D");
            standard.Add("ShortcutCtrlI", "Strg+I");
            standard.Add("NodeFileFormatUTF", "Output Dateien UTF8");
            standard.Add("NodeFileFormatUnicode", "Output Dateien Unicode");
            standard.Add("NodeFileFormatASCII", "Output Dateien Basic ASCII");
            standard.Add("NodeFileFormatANSI", "Output Dateien ANSI");
            standard.Add("textStripColumn1", "Soll die Spalte \"");
            standard.Add("textStripColumn2", "\" wirklich entfernt werden?");
            standard.Add("textStripColumn3", "Spalte entfernen");
            standard.Add("templateImportXml", "Import Script Builder Template XML Datei");
            standard.Add("templateImportErrorXml1", "Die XML Datei hat nicht die richtige Stuktur eines Script Builder Templates.");
            standard.Add("templateImportErrorXml2", "Das Template wurde mit einer neueren Version des Script Builder erzeugt und kann daher nicht geladen werden.");
            standard.Add("templateImportErrorXml3", "Beim Laden der XML Datei ist ein Fehler aufgetreten:");
            standard.Add("textNewTemplate", "[Neues Template]");
            #endregion
            
            if (currentCulture.ToString() == "")
                aktive = standard;
        }

        public Dictionary<string, string> Cultures
        {
            get { return cultures; }
        }

        public string CultureName(string CultureCode)
        {
            if(cultures.ContainsKey(CultureCode))
                return cultures[CultureCode];
            else
                return "";
        }

        public string CultureCode(string CultureName)
        {
            if (cultures.ContainsValue(CultureName))
            {
                foreach (KeyValuePair<string, string> thisPair in cultures)
                    if (thisPair.Value == CultureName)
                        return thisPair.Key;
                return "";
            }
            else
                return "";
        }       

        public string GetString(string StringName)
        {
            if (aktive.ContainsKey(StringName))
                return aktive[StringName];
            else if (standard.ContainsKey(StringName))
                return standard[StringName];
            else
                throw new Exception("Requested string name is neither defined in active nor in standard!");
        }
    }
}
