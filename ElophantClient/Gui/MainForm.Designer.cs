namespace ElophantClient.Gui
{
    partial class MainForm
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
            if (disposing && (Connection != null))
            {
                Connection.Dispose();
            }
            //if (disposing && (Database != null))
            //{
            //    Database.Dispose();
            //}
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.PlayerEditStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CallEditStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ModuleGroupBox = new System.Windows.Forms.GroupBox();
            this.MirrorRadio = new System.Windows.Forms.RadioButton();
            this.ToolHelpRadio = new System.Windows.Forms.RadioButton();
            this.ProcessRadio = new System.Windows.Forms.RadioButton();
            this.RegionList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IncludeBans = new System.Windows.Forms.CheckBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SummonerNamesLabel = new System.Windows.Forms.Label();
            this.BansLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GenRecLabel = new System.Windows.Forms.Label();
            this.CopyURLButton = new System.Windows.Forms.Button();
            this.OpenURLinBrowser = new System.Windows.Forms.Button();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ViewOwnedSkins = new System.Windows.Forms.Button();
            this.PlayerEditStrip.SuspendLayout();
            this.CallEditStrip.SuspendLayout();
            this.ModuleGroupBox.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlayerEditStrip
            // 
            this.PlayerEditStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.PlayerEditStrip.Name = "PlayerEditStrip";
            this.PlayerEditStrip.Size = new System.Drawing.Size(102, 48);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            // 
            // CallEditStrip
            // 
            this.CallEditStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpToolStripMenuItem,
            this.clearToolStripMenuItem1});
            this.CallEditStrip.Name = "CallEditStrip";
            this.CallEditStrip.Size = new System.Drawing.Size(108, 48);
            // 
            // dumpToolStripMenuItem
            // 
            this.dumpToolStripMenuItem.Name = "dumpToolStripMenuItem";
            this.dumpToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.dumpToolStripMenuItem.Text = "Dump";
            // 
            // clearToolStripMenuItem1
            // 
            this.clearToolStripMenuItem1.Name = "clearToolStripMenuItem1";
            this.clearToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.clearToolStripMenuItem1.Text = "Clear";
            // 
            // ModuleGroupBox
            // 
            this.ModuleGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.ModuleGroupBox.Controls.Add(this.MirrorRadio);
            this.ModuleGroupBox.Controls.Add(this.ToolHelpRadio);
            this.ModuleGroupBox.Controls.Add(this.ProcessRadio);
            this.ModuleGroupBox.Location = new System.Drawing.Point(254, 12);
            this.ModuleGroupBox.Name = "ModuleGroupBox";
            this.ModuleGroupBox.Size = new System.Drawing.Size(125, 95);
            this.ModuleGroupBox.TabIndex = 11;
            this.ModuleGroupBox.TabStop = false;
            this.ModuleGroupBox.Text = "Module Resolver";
            this.ModuleGroupBox.Visible = false;
            // 
            // MirrorRadio
            // 
            this.MirrorRadio.AutoSize = true;
            this.MirrorRadio.Location = new System.Drawing.Point(6, 65);
            this.MirrorRadio.Name = "MirrorRadio";
            this.MirrorRadio.Size = new System.Drawing.Size(51, 17);
            this.MirrorRadio.TabIndex = 2;
            this.MirrorRadio.Text = "Mirror";
            this.MirrorRadio.UseVisualStyleBackColor = true;
            // 
            // ToolHelpRadio
            // 
            this.ToolHelpRadio.AutoSize = true;
            this.ToolHelpRadio.Location = new System.Drawing.Point(6, 42);
            this.ToolHelpRadio.Name = "ToolHelpRadio";
            this.ToolHelpRadio.Size = new System.Drawing.Size(78, 17);
            this.ToolHelpRadio.TabIndex = 1;
            this.ToolHelpRadio.Text = "Toolhelp32";
            this.ToolHelpRadio.UseVisualStyleBackColor = true;
            // 
            // ProcessRadio
            // 
            this.ProcessRadio.AutoSize = true;
            this.ProcessRadio.Checked = true;
            this.ProcessRadio.Location = new System.Drawing.Point(6, 19);
            this.ProcessRadio.Name = "ProcessRadio";
            this.ProcessRadio.Size = new System.Drawing.Size(88, 17);
            this.ProcessRadio.TabIndex = 0;
            this.ProcessRadio.TabStop = true;
            this.ProcessRadio.Text = "ProcessClass";
            this.ProcessRadio.UseVisualStyleBackColor = true;
            // 
            // RegionList
            // 
            this.RegionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RegionList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegionList.FormattingEnabled = true;
            this.RegionList.Location = new System.Drawing.Point(65, 13);
            this.RegionList.Name = "RegionList";
            this.RegionList.Size = new System.Drawing.Size(156, 25);
            this.RegionList.TabIndex = 10;
            this.RegionList.SelectedIndexChanged += new System.EventHandler(this.RegionList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Region:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Include Bans:";
            // 
            // IncludeBans
            // 
            this.IncludeBans.AutoSize = true;
            this.IncludeBans.BackColor = System.Drawing.Color.Transparent;
            this.IncludeBans.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncludeBans.Location = new System.Drawing.Point(101, 57);
            this.IncludeBans.Name = "IncludeBans";
            this.IncludeBans.Size = new System.Drawing.Size(15, 14);
            this.IncludeBans.TabIndex = 15;
            this.IncludeBans.UseVisualStyleBackColor = false;
            this.IncludeBans.CheckStateChanged += new System.EventHandler(this.IncludeBans_CheckStateChanged);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 250);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(274, 22);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 16;
            this.StatusStrip.Text = "Searching for the LoL Client...";
            // 
            // StatusLabel
            // 
            this.StatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(162, 17);
            this.StatusLabel.Text = "Searching for the LoL Client...";
            // 
            // SummonerNamesLabel
            // 
            this.SummonerNamesLabel.AutoSize = true;
            this.SummonerNamesLabel.BackColor = System.Drawing.Color.Transparent;
            this.SummonerNamesLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SummonerNamesLabel.Location = new System.Drawing.Point(12, 101);
            this.SummonerNamesLabel.Name = "SummonerNamesLabel";
            this.SummonerNamesLabel.Size = new System.Drawing.Size(171, 17);
            this.SummonerNamesLabel.TabIndex = 18;
            this.SummonerNamesLabel.Text = "Summoner names acquired.";
            this.SummonerNamesLabel.Visible = false;
            // 
            // BansLabel
            // 
            this.BansLabel.AutoSize = true;
            this.BansLabel.BackColor = System.Drawing.Color.Transparent;
            this.BansLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BansLabel.Location = new System.Drawing.Point(12, 128);
            this.BansLabel.Name = "BansLabel";
            this.BansLabel.Size = new System.Drawing.Size(93, 17);
            this.BansLabel.TabIndex = 19;
            this.BansLabel.Text = "Bans acquired.";
            this.BansLabel.Visible = false;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(4, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(265, 2);
            this.label5.TabIndex = 20;
            // 
            // GenRecLabel
            // 
            this.GenRecLabel.AutoSize = true;
            this.GenRecLabel.BackColor = System.Drawing.Color.Transparent;
            this.GenRecLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenRecLabel.Location = new System.Drawing.Point(10, 174);
            this.GenRecLabel.Name = "GenRecLabel";
            this.GenRecLabel.Size = new System.Drawing.Size(190, 17);
            this.GenRecLabel.TabIndex = 21;
            this.GenRecLabel.Text = "Generating recommendations...";
            this.GenRecLabel.Visible = false;
            // 
            // CopyURLButton
            // 
            this.CopyURLButton.AutoSize = true;
            this.CopyURLButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyURLButton.Location = new System.Drawing.Point(179, 205);
            this.CopyURLButton.Name = "CopyURLButton";
            this.CopyURLButton.Size = new System.Drawing.Size(75, 27);
            this.CopyURLButton.TabIndex = 22;
            this.CopyURLButton.Text = "Copy URL";
            this.CopyURLButton.UseVisualStyleBackColor = true;
            this.CopyURLButton.Visible = false;
            this.CopyURLButton.Click += new System.EventHandler(this.CopyURLButton_Click);
            // 
            // OpenURLinBrowser
            // 
            this.OpenURLinBrowser.AutoSize = true;
            this.OpenURLinBrowser.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenURLinBrowser.Location = new System.Drawing.Point(15, 205);
            this.OpenURLinBrowser.Name = "OpenURLinBrowser";
            this.OpenURLinBrowser.Size = new System.Drawing.Size(145, 27);
            this.OpenURLinBrowser.TabIndex = 23;
            this.OpenURLinBrowser.Text = "Open URL in Browser";
            this.OpenURLinBrowser.UseVisualStyleBackColor = true;
            this.OpenURLinBrowser.Visible = false;
            this.OpenURLinBrowser.Click += new System.EventHandler(this.OpenInBrowser_Click);
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.BackColor = System.Drawing.SystemColors.MenuBar;
            this.VersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(233, 255);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(0, 15);
            this.VersionLabel.TabIndex = 24;
            // 
            // ViewOwnedSkins
            // 
            this.ViewOwnedSkins.AutoSize = true;
            this.ViewOwnedSkins.Enabled = false;
            this.ViewOwnedSkins.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.ViewOwnedSkins.Location = new System.Drawing.Point(131, 49);
            this.ViewOwnedSkins.Name = "ViewOwnedSkins";
            this.ViewOwnedSkins.Size = new System.Drawing.Size(123, 27);
            this.ViewOwnedSkins.TabIndex = 25;
            this.ViewOwnedSkins.Text = "View Owned Skins";
            this.ViewOwnedSkins.UseVisualStyleBackColor = true;
            this.ViewOwnedSkins.Click += new System.EventHandler(this.ViewOwnedSkins_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = global::ElophantClient.Properties.Resources.noise;
            this.ClientSize = new System.Drawing.Size(274, 272);
            this.Controls.Add(this.ViewOwnedSkins);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.OpenURLinBrowser);
            this.Controls.Add(this.CopyURLButton);
            this.Controls.Add(this.GenRecLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BansLabel);
            this.Controls.Add(this.SummonerNamesLabel);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.IncludeBans);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ModuleGroupBox);
            this.Controls.Add(this.RegionList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "MainForm";
            this.Text = "Elophant Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.PlayerEditStrip.ResumeLayout(false);
            this.CallEditStrip.ResumeLayout(false);
            this.ModuleGroupBox.ResumeLayout(false);
            this.ModuleGroupBox.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.ContextMenuStrip PlayerEditStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip CallEditStrip;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dumpToolStripMenuItem;
        private System.Windows.Forms.GroupBox ModuleGroupBox;
        private System.Windows.Forms.RadioButton MirrorRadio;
        private System.Windows.Forms.RadioButton ToolHelpRadio;
        private System.Windows.Forms.RadioButton ProcessRadio;
        private System.Windows.Forms.ComboBox RegionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox IncludeBans;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Label SummonerNamesLabel;
        private System.Windows.Forms.Label BansLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label GenRecLabel;
        private System.Windows.Forms.Button CopyURLButton;
        private System.Windows.Forms.Button OpenURLinBrowser;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Button ViewOwnedSkins;

    }
}

