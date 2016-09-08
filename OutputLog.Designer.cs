namespace Script_Builder
{
    partial class OutputLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputLog));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveOutputLog = new System.Windows.Forms.ToolStripButton();
            this.clearOutputLog = new System.Windows.Forms.ToolStripButton();
            this.closeOutputLog = new System.Windows.Forms.ToolStripButton();
            this.txtOutputLog = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveOutputLog,
            this.clearOutputLog,
            this.closeOutputLog});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Name = "toolStrip1";
            // 
            // saveOutputLog
            // 
            resources.ApplyResources(this.saveOutputLog, "saveOutputLog");
            this.saveOutputLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveOutputLog.Image = global::Script_Builder.Resource1.disk;
            this.saveOutputLog.Name = "saveOutputLog";
            this.saveOutputLog.Click += new System.EventHandler(this.saveOutputLog_Click);
            // 
            // clearOutputLog
            // 
            resources.ApplyResources(this.clearOutputLog, "clearOutputLog");
            this.clearOutputLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.clearOutputLog.Image = global::Script_Builder.Resource1.page;
            this.clearOutputLog.Name = "clearOutputLog";
            this.clearOutputLog.Click += new System.EventHandler(this.clearOutputLog_Click);
            // 
            // closeOutputLog
            // 
            resources.ApplyResources(this.closeOutputLog, "closeOutputLog");
            this.closeOutputLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.closeOutputLog.Image = global::Script_Builder.Resource1.door_in;
            this.closeOutputLog.Name = "closeOutputLog";
            this.closeOutputLog.Click += new System.EventHandler(this.closeOutputLog_Click);
            // 
            // txtOutputLog
            // 
            resources.ApplyResources(this.txtOutputLog, "txtOutputLog");
            this.txtOutputLog.BackColor = System.Drawing.Color.White;
            this.txtOutputLog.Name = "txtOutputLog";
            this.txtOutputLog.ReadOnly = true;
            // 
            // OutputLog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtOutputLog);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "OutputLog";
            this.ShowInTaskbar = false;
            this.Shown += new System.EventHandler(this.OutputLog_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveOutputLog;
        private System.Windows.Forms.ToolStripButton clearOutputLog;
        private System.Windows.Forms.ToolStripButton closeOutputLog;
        private System.Windows.Forms.TextBox txtOutputLog;
    }
}