using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Script_Builder
{
    public partial class serialWizard : Form
    {
        public DataTable TblSerialTable;
        private mainForm mainForm;
        public string LinkTableA { get; private set; }
        public string LinkTableB { get; private set; }

        public serialWizard(mainForm mainForm, string[] SerialInputTable, DataTable TblSerialTable, string LinkTableA, string LinkTableB)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            cboInputLinkB.Items.AddRange(SerialInputTable);
            this.TblSerialTable = TblSerialTable;
            updateTable();
            if (LinkTableA != "")
                cboInputLinkA.Text = LinkTableA;
            if (LinkTableB != "")
                cboInputLinkB.Text = LinkTableB;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            LinkTableA = cboInputLinkA.SelectedItem != null ? cboInputLinkA.SelectedItem.ToString() : "";
            LinkTableB = cboInputLinkB.SelectedItem != null ? cboInputLinkB.SelectedItem.ToString() : "";
            dgvSerialInputTable.Enabled = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void spalteHinzuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TblSerialTable.Columns.Add();
            updateTable();

        }

        private void updateTable()
        {
            dgvSerialInputTable.DataSource = TblSerialTable.DefaultView;
            cboInputLinkA.Items.Clear();
            for (int iSpalte = 0; iSpalte < TblSerialTable.Columns.Count; iSpalte++)
                cboInputLinkA.Items.Add(TblSerialTable.Columns[iSpalte].Caption);
        }

        private void spalteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TblSerialTable.Clear();
            TblSerialTable.Columns.Clear();
            updateTable();
        }

        private void überschriftInErsterZeileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainForm.headerInFirstRow(TblSerialTable);
            updateTable();
        }

        private void excelDatenAusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainForm.PasteExcelData(TblSerialTable);
            updateTable();
        }

        private void tabelleExportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainForm.exportTableToFile(mainForm, TblSerialTable);
        }

        private void tabelleInportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainForm.importTableFromFile(mainForm, TblSerialTable);
            updateTable();
        }

        private void dgvSerialInputTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mainForm.changeTableHeader(mainForm, TblSerialTable, e.ColumnIndex);
            }
        }

        private void dgvSerialInputTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 22)
            {
                mainForm.PasteExcelData(TblSerialTable);
                e.Handled = true;
            }
        }

        private void dgvSerialInputTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            statusLabel.Text = mainForm.programmStrings.GetString("textEntities") + TblSerialTable.Rows.Count.ToString();
        }

        private void verknüpfungLösenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cboInputLinkA.SelectedItem = null;
            cboInputLinkB.SelectedItem = null;
        }
    }
}
