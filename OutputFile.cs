using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace Script_Builder
{
    class OutputFile
    {
        public string FileName;
        public string FilePath;
        public bool SerialFile { get; private set; }
        public DataTable SerialInputTable;
        public string SerialFieldLinkA;
        public string SerialFieldLinkB;
        private int fileType;
        public int FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                if (value < 0 || value > 3)
                    fileType = 3;
                else
                    fileType = value;
            }
        }
        public bool Overwrite;

        public OutputFile(string FileName, string FilePath, int FileType)
        {
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.fileType = FileType;
            this.SerialFile = false;
            this.Overwrite = false;
        }

        public OutputFile(string FileName, string FilePath, int FileType, bool SerialFile)
        {
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.fileType = FileType;
            this.SerialFile = SerialFile;
            this.Overwrite = false;
            if (SerialFile)
            {
                this.SerialInputTable = new DataTable();
                this.SerialFieldLinkA = "";
                this.SerialFieldLinkB = "";
            }
        }

        public bool Exists
        {
            get
            {
                return File.Exists(FullFileName);
            }
        }

        public string FullFileName
        {
            get
            {
                return Path.Combine(FilePath,FileName);
            }
        }

        public Encoding Encoding
        {
            get
            {
                switch (fileType)
                {
                    case 0:
                        return Encoding.UTF8;
                    case 1:
                        return Encoding.Unicode;
                    case 2:
                        return Encoding.ASCII;
                    default:
                        return Encoding.Default;
                }
            }
        }

        public int SerialFieldLinkID()
        {
            return serialFieldLinkID(SerialInputTable, SerialFieldLinkA);
        }

        public int SerialFieldLinkID(DataTable inputTable)
        {
            return serialFieldLinkID(inputTable, SerialFieldLinkB);
        }

        private int serialFieldLinkID(DataTable inputTable,string matchColumnName)
        {
            if (inputTable != null)
                for (int output = 0; output < inputTable.Columns.Count; output++)
                    if (inputTable.Columns[output].ColumnName == matchColumnName)
                        return output;
            return -1;
        }

    }
}
