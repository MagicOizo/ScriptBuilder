namespace Script_Builder
{
    partial class serialWizard
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(serialWizard));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAccept = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.quellTabelleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelDatenAusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabelleExportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabelleInportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.spalteHinzuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spalteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.überschriftInErsterZeileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verknüpfungToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboInputLinkA = new System.Windows.Forms.ToolStripComboBox();
            this.cboInputLinkB = new System.Windows.Forms.ToolStripComboBox();
            this.verknüpfungLösenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvSerialInputTable = new System.Windows.Forms.DataGridView();
            this.menuMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSerialInputTable)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.quellTabelleToolStripMenuItem,
            this.verknüpfungToolStripMenuItem});
            resources.ApplyResources(this.menuMain, "menuMain");
            this.menuMain.Name = "menuMain";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAccept,
            this.btnCancel});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // btnAccept
            // 
            this.btnAccept.Image = global::Script_Builder.Resource1.disk;
            this.btnAccept.Name = "btnAccept";
            resources.ApplyResources(this.btnAccept, "btnAccept");
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Script_Builder.Resource1.door_in;
            this.btnCancel.Name = "btnCancel";
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // quellTabelleToolStripMenuItem
            // 
            this.quellTabelleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelDatenAusToolStripMenuItem,
            this.tabelleExportierenToolStripMenuItem,
            this.tabelleInportierenToolStripMenuItem,
            this.toolStripSeparator1,
            this.spalteHinzuToolStripMenuItem,
            this.spalteToolStripMenuItem,
            this.toolStripSeparator2,
            this.überschriftInErsterZeileToolStripMenuItem});
            this.quellTabelleToolStripMenuItem.Name = "quellTabelleToolStripMenuItem";
            resources.ApplyResources(this.quellTabelleToolStripMenuItem, "quellTabelleToolStripMenuItem");
            // 
            // excelDatenAusToolStripMenuItem
            // 
            this.excelDatenAusToolStripMenuItem.Image = global::Script_Builder.Resource1.doc_excel_table;
            this.excelDatenAusToolStripMenuItem.Name = "excelDatenAusToolStripMenuItem";
            resources.ApplyResources(this.excelDatenAusToolStripMenuItem, "excelDatenAusToolStripMenuItem");
            this.excelDatenAusToolStripMenuItem.Click += new System.EventHandler(this.excelDatenAusToolStripMenuItem_Click);
            // 
            // tabelleExportierenToolStripMenuItem
            // 
            this.tabelleExportierenToolStripMenuItem.Image = global::Script_Builder.Resource1.table_export;
            this.tabelleExportierenToolStripMenuItem.Name = "tabelleExportierenToolStripMenuItem";
            resources.ApplyResources(this.tabelleExportierenToolStripMenuItem, "tabelleExportierenToolStripMenuItem");
            this.tabelleExportierenToolStripMenuItem.Click += new System.EventHandler(this.tabelleExportierenToolStripMenuItem_Click);
            // 
            // tabelleInportierenToolStripMenuItem
            // 
            this.tabelleInportierenToolStripMenuItem.Image = global::Script_Builder.Resource1.table_import;
            this.tabelleInportierenToolStripMenuItem.Name = "tabelleInportierenToolStripMenuItem";
            resources.ApplyResources(this.tabelleInportierenToolStripMenuItem, "tabelleInportierenToolStripMenuItem");
            this.tabelleInportierenToolStripMenuItem.Click += new System.EventHandler(this.tabelleInportierenToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // spalteHinzuToolStripMenuItem
            // 
            this.spalteHinzuToolStripMenuItem.Image = global::Script_Builder.Resource1.add;
            this.spalteHinzuToolStripMenuItem.Name = "spalteHinzuToolStripMenuItem";
            resources.ApplyResources(this.spalteHinzuToolStripMenuItem, "spalteHinzuToolStripMenuItem");
            this.spalteHinzuToolStripMenuItem.Click += new System.EventHandler(this.spalteHinzuToolStripMenuItem_Click);
            // 
            // spalteToolStripMenuItem
            // 
            this.spalteToolStripMenuItem.Image = global::Script_Builder.Resource1.cross;
            this.spalteToolStripMenuItem.Name = "spalteToolStripMenuItem";
            resources.ApplyResources(this.spalteToolStripMenuItem, "spalteToolStripMenuItem");
            this.spalteToolStripMenuItem.Click += new System.EventHandler(this.spalteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // überschriftInErsterZeileToolStripMenuItem
            // 
            this.überschriftInErsterZeileToolStripMenuItem.Image = global::Script_Builder.Resource1.useFirstRow;
            this.überschriftInErsterZeileToolStripMenuItem.Name = "überschriftInErsterZeileToolStripMenuItem";
            resources.ApplyResources(this.überschriftInErsterZeileToolStripMenuItem, "überschriftInErsterZeileToolStripMenuItem");
            this.überschriftInErsterZeileToolStripMenuItem.Click += new System.EventHandler(this.überschriftInErsterZeileToolStripMenuItem_Click);
            // 
            // verknüpfungToolStripMenuItem
            // 
            this.verknüpfungToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cboInputLinkA,
            this.cboInputLinkB,
            this.verknüpfungLösenToolStripMenuItem});
            this.verknüpfungToolStripMenuItem.Name = "verknüpfungToolStripMenuItem";
            resources.ApplyResources(this.verknüpfungToolStripMenuItem, "verknüpfungToolStripMenuItem");
            // 
            // cboInputLinkA
            // 
            this.cboInputLinkA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInputLinkA.Name = "cboInputLinkA";
            resources.ApplyResources(this.cboInputLinkA, "cboInputLinkA");
            this.cboInputLinkA.Sorted = true;
            // 
            // cboInputLinkB
            // 
            this.cboInputLinkB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInputLinkB.Name = "cboInputLinkB";
            resources.ApplyResources(this.cboInputLinkB, "cboInputLinkB");
            this.cboInputLinkB.Sorted = true;
            // 
            // verknüpfungLösenToolStripMenuItem
            // 
            this.verknüpfungLösenToolStripMenuItem.Name = "verknüpfungLösenToolStripMenuItem";
            resources.ApplyResources(this.verknüpfungLösenToolStripMenuItem, "verknüpfungLösenToolStripMenuItem");
            this.verknüpfungLösenToolStripMenuItem.Click += new System.EventHandler(this.verknüpfungLösenToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            resources.ApplyResources(this.statusLabel, "statusLabel");
            // 
            // dgvSerialInputTable
            // 
            this.dgvSerialInputTable.AllowUserToOrderColumns = true;
            this.dgvSerialInputTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvSerialInputTable, "dgvSerialInputTable");
            this.dgvSerialInputTable.Name = "dgvSerialInputTable";
            this.dgvSerialInputTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSerialInputTable_ColumnHeaderMouseClick);
            this.dgvSerialInputTable.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvSerialInputTable_RowPostPaint);
            this.dgvSerialInputTable.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSerialInputTable_KeyPress);
            // 
            // serialWizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvSerialInputTable);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuMain);
            this.DoubleBuffered = true;
            this.Name = "serialWizard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSerialInputTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btnAccept;
        private System.Windows.Forms.ToolStripMenuItem btnCancel;
        private System.Windows.Forms.ToolStripMenuItem quellTabelleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelDatenAusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tabelleInportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tabelleExportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem spalteHinzuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spalteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem überschriftInErsterZeileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verknüpfungToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox cboInputLinkA;
        private System.Windows.Forms.ToolStripComboBox cboInputLinkB;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.DataGridView dgvSerialInputTable;
        private System.Windows.Forms.ToolStripMenuItem verknüpfungLösenToolStripMenuItem;

    }
}