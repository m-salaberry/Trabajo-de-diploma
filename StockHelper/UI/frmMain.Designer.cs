namespace UI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            tsmUserAndPerms = new ToolStripMenuItem();
            tsmUsers = new ToolStripMenuItem();
            tsmPerms = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { tsmUserAndPerms });
            menuStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Size = new Size(838, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // tsmUserAndPerms
            // 
            tsmUserAndPerms.DropDownItems.AddRange(new ToolStripItem[] { tsmUsers, tsmPerms });
            tsmUserAndPerms.Name = "tsmUserAndPerms";
            tsmUserAndPerms.Size = new Size(136, 20);
            tsmUserAndPerms.Text = "Users and Permissions";
            // 
            // tsmUsers
            // 
            tsmUsers.Name = "tsmUsers";
            tsmUsers.Size = new Size(205, 22);
            tsmUsers.Text = "Users Managment";
            // 
            // tsmPerms
            // 
            tsmPerms.Name = "tsmPerms";
            tsmPerms.Size = new Size(205, 22);
            tsmPerms.Text = "Permissions Managment";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(838, 441);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "frmMain";
            Text = "frmMain";
            Load += frmMain_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem tsmUserAndPerms;
        private ToolStripMenuItem tsmUsers;
        private ToolStripMenuItem tsmPerms;
    }
}