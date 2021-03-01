
namespace AttackCompilerForm
{
    partial class FormAttackCompiler
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.attackTypeComboBox = new System.Windows.Forms.ComboBox();
            this.attackTypeLabel = new System.Windows.Forms.Label();
            this.attackBuilderGroupBox = new System.Windows.Forms.GroupBox();
            this.nodesLabel = new System.Windows.Forms.Label();
            this.nodesTextBox = new System.Windows.Forms.TextBox();
            this.submitAttackButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compiledAttacksGroupBox = new System.Windows.Forms.GroupBox();
            this.compiledAttacksListBox = new System.Windows.Forms.ListBox();
            this.attacksContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editAttackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAttackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateAttackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attackBuilderGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.compiledAttacksGroupBox.SuspendLayout();
            this.attacksContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // attackTypeComboBox
            // 
            this.attackTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.attackTypeComboBox.FormattingEnabled = true;
            this.attackTypeComboBox.Items.AddRange(new object[] {
            "Recon",
            "Intrusion",
            "Execute Code",
            "Cry"});
            this.attackTypeComboBox.Location = new System.Drawing.Point(121, 79);
            this.attackTypeComboBox.Name = "attackTypeComboBox";
            this.attackTypeComboBox.Size = new System.Drawing.Size(121, 23);
            this.attackTypeComboBox.TabIndex = 0;
            // 
            // attackTypeLabel
            // 
            this.attackTypeLabel.AutoSize = true;
            this.attackTypeLabel.Location = new System.Drawing.Point(137, 61);
            this.attackTypeLabel.Name = "attackTypeLabel";
            this.attackTypeLabel.Size = new System.Drawing.Size(70, 15);
            this.attackTypeLabel.TabIndex = 1;
            this.attackTypeLabel.Text = "Attack type:";
            // 
            // attackBuilderGroupBox
            // 
            this.attackBuilderGroupBox.Controls.Add(this.nodesLabel);
            this.attackBuilderGroupBox.Controls.Add(this.nodesTextBox);
            this.attackBuilderGroupBox.Controls.Add(this.submitAttackButton);
            this.attackBuilderGroupBox.Controls.Add(this.attackTypeComboBox);
            this.attackBuilderGroupBox.Controls.Add(this.attackTypeLabel);
            this.attackBuilderGroupBox.Location = new System.Drawing.Point(12, 39);
            this.attackBuilderGroupBox.Name = "attackBuilderGroupBox";
            this.attackBuilderGroupBox.Size = new System.Drawing.Size(263, 241);
            this.attackBuilderGroupBox.TabIndex = 2;
            this.attackBuilderGroupBox.TabStop = false;
            this.attackBuilderGroupBox.Text = "Attack Builder";
            // 
            // nodesLabel
            // 
            this.nodesLabel.AutoSize = true;
            this.nodesLabel.Location = new System.Drawing.Point(6, 22);
            this.nodesLabel.Name = "nodesLabel";
            this.nodesLabel.Size = new System.Drawing.Size(92, 15);
            this.nodesLabel.TabIndex = 6;
            this.nodesLabel.Text = "Affected Nodes:";
            // 
            // nodesTextBox
            // 
            this.nodesTextBox.Location = new System.Drawing.Point(6, 40);
            this.nodesTextBox.Multiline = true;
            this.nodesTextBox.Name = "nodesTextBox";
            this.nodesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.nodesTextBox.Size = new System.Drawing.Size(100, 166);
            this.nodesTextBox.TabIndex = 4;
            this.nodesTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // submitAttackButton
            // 
            this.submitAttackButton.Location = new System.Drawing.Point(137, 183);
            this.submitAttackButton.Name = "submitAttackButton";
            this.submitAttackButton.Size = new System.Drawing.Size(105, 23);
            this.submitAttackButton.TabIndex = 2;
            this.submitAttackButton.Text = "Submit Attack";
            this.submitAttackButton.UseVisualStyleBackColor = true;
            this.submitAttackButton.Click += new System.EventHandler(this.submitAttackButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(641, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileMenuItem,
            this.openFileMenuItem,
            this.closeFileMenuItem,
            this.saveFileMenuItem,
            this.saveAsFileMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "File";
            // 
            // newFileMenuItem
            // 
            this.newFileMenuItem.Name = "newFileMenuItem";
            this.newFileMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newFileMenuItem.Text = "New";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openFileMenuItem.Text = "Open";
            // 
            // closeFileMenuItem
            // 
            this.closeFileMenuItem.Name = "closeFileMenuItem";
            this.closeFileMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeFileMenuItem.Text = "Close";
            // 
            // saveFileMenuItem
            // 
            this.saveFileMenuItem.Name = "saveFileMenuItem";
            this.saveFileMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveFileMenuItem.Text = "Save";
            this.saveFileMenuItem.Click += new System.EventHandler(this.saveFileMenuItem_Click);
            // 
            // saveAsFileMenuItem
            // 
            this.saveAsFileMenuItem.Name = "saveAsFileMenuItem";
            this.saveAsFileMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsFileMenuItem.Text = "Save As";
            this.saveAsFileMenuItem.Click += new System.EventHandler(this.saveAsFileMenuItem_Click);
            // 
            // compiledAttacksGroupBox
            // 
            this.compiledAttacksGroupBox.Controls.Add(this.compiledAttacksListBox);
            this.compiledAttacksGroupBox.Location = new System.Drawing.Point(281, 39);
            this.compiledAttacksGroupBox.Name = "compiledAttacksGroupBox";
            this.compiledAttacksGroupBox.Size = new System.Drawing.Size(344, 399);
            this.compiledAttacksGroupBox.TabIndex = 4;
            this.compiledAttacksGroupBox.TabStop = false;
            this.compiledAttacksGroupBox.Text = "Compiled Attacks";
            // 
            // compiledAttacksListBox
            // 
            this.compiledAttacksListBox.FormattingEnabled = true;
            this.compiledAttacksListBox.ItemHeight = 15;
            this.compiledAttacksListBox.Location = new System.Drawing.Point(6, 22);
            this.compiledAttacksListBox.Name = "compiledAttacksListBox";
            this.compiledAttacksListBox.Size = new System.Drawing.Size(332, 364);
            this.compiledAttacksListBox.TabIndex = 0;
            this.compiledAttacksListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.compiledAttacksListBox_MouseDown);
            // 
            // attacksContextMenuStrip
            // 
            this.attacksContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editAttackToolStripMenuItem,
            this.deleteAttackToolStripMenuItem,
            this.duplicateAttackToolStripMenuItem});
            this.attacksContextMenuStrip.Name = "attacksContextMenuStrip";
            this.attacksContextMenuStrip.Size = new System.Drawing.Size(125, 70);
            // 
            // editAttackToolStripMenuItem
            // 
            this.editAttackToolStripMenuItem.Name = "editAttackToolStripMenuItem";
            this.editAttackToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.editAttackToolStripMenuItem.Text = "Edit";
            this.editAttackToolStripMenuItem.Click += new System.EventHandler(this.editAttackToolStripMenuItem_Click);
            // 
            // deleteAttackToolStripMenuItem
            // 
            this.deleteAttackToolStripMenuItem.Name = "deleteAttackToolStripMenuItem";
            this.deleteAttackToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteAttackToolStripMenuItem.Text = "Delete";
            this.deleteAttackToolStripMenuItem.Click += new System.EventHandler(this.deleteAttackToolStripMenuItem_Click);
            // 
            // duplicateAttackToolStripMenuItem
            // 
            this.duplicateAttackToolStripMenuItem.Name = "duplicateAttackToolStripMenuItem";
            this.duplicateAttackToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.duplicateAttackToolStripMenuItem.Text = "Duplicate";
            this.duplicateAttackToolStripMenuItem.Click += new System.EventHandler(this.duplicateAttackToolStripMenuItem_Click);
            // 
            // FormAttackCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 450);
            this.Controls.Add(this.compiledAttacksGroupBox);
            this.Controls.Add(this.attackBuilderGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormAttackCompiler";
            this.RightToLeftLayout = true;
            this.Text = "Attack Compiler";
            this.attackBuilderGroupBox.ResumeLayout(false);
            this.attackBuilderGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.compiledAttacksGroupBox.ResumeLayout(false);
            this.attacksContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox attackTypeComboBox;
        private System.Windows.Forms.Label attackTypeLabel;
        private System.Windows.Forms.GroupBox attackBuilderGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button submitAttackButton;
        private System.Windows.Forms.TextBox nodesTextBox;
        private System.Windows.Forms.Label nodesLabel;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsFileMenuItem;
        private System.Windows.Forms.GroupBox compiledAttacksGroupBox;
        private System.Windows.Forms.ListBox compiledAttacksListBox;
        private System.Windows.Forms.ContextMenuStrip attacksContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editAttackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAttackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateAttackToolStripMenuItem;
    }
}

