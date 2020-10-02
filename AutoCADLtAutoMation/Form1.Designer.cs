namespace AutoCADLtAutoMation
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.kFWHSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图框填写ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打印成PdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.页码填写ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accoreConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kFWHSDToolStripMenuItem,
            this.accoreConsoleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(619, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // kFWHSDToolStripMenuItem
            // 
            this.kFWHSDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.图框填写ToolStripMenuItem,
            this.打印成PdfToolStripMenuItem,
            this.页码填写ToolStripMenuItem});
            this.kFWHSDToolStripMenuItem.Name = "kFWHSDToolStripMenuItem";
            this.kFWHSDToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.kFWHSDToolStripMenuItem.Text = "KFWH_SD";
            // 
            // 图框填写ToolStripMenuItem
            // 
            this.图框填写ToolStripMenuItem.Name = "图框填写ToolStripMenuItem";
            this.图框填写ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.图框填写ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.图框填写ToolStripMenuItem.Text = "图框填写";
            this.图框填写ToolStripMenuItem.Click += new System.EventHandler(this.图框填写ToolStripMenuItem_Click);
            // 
            // 打印成PdfToolStripMenuItem
            // 
            this.打印成PdfToolStripMenuItem.Name = "打印成PdfToolStripMenuItem";
            this.打印成PdfToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.打印成PdfToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.打印成PdfToolStripMenuItem.Text = "打印成Pdf";
            this.打印成PdfToolStripMenuItem.Click += new System.EventHandler(this.打印成PdfToolStripMenuItem_Click);
            // 
            // 页码填写ToolStripMenuItem
            // 
            this.页码填写ToolStripMenuItem.Name = "页码填写ToolStripMenuItem";
            this.页码填写ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.页码填写ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.页码填写ToolStripMenuItem.Text = "页码填写";
            this.页码填写ToolStripMenuItem.Click += new System.EventHandler(this.页码填写ToolStripMenuItem_Click);
            // 
            // accoreConsoleToolStripMenuItem
            // 
            this.accoreConsoleToolStripMenuItem.Name = "accoreConsoleToolStripMenuItem";
            this.accoreConsoleToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.accoreConsoleToolStripMenuItem.Text = "AccoreConsole";
            this.accoreConsoleToolStripMenuItem.Click += new System.EventHandler(this.accoreConsoleToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 207);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MianForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem kFWHSDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图框填写ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印成PdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 页码填写ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accoreConsoleToolStripMenuItem;
    }
}

