
namespace AttackCompilerForm
{
    partial class FormEditAttack
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
            this.attackBuilderGroupBox = new System.Windows.Forms.GroupBox();
            this.attackDateTimerPicker = new System.Windows.Forms.DateTimePicker();
            this.nodesLabel = new System.Windows.Forms.Label();
            this.teamsLable = new System.Windows.Forms.Label();
            this.nodesTextBox = new System.Windows.Forms.TextBox();
            this.teamsTextBox = new System.Windows.Forms.TextBox();
            this.submitAttackButton = new System.Windows.Forms.Button();
            this.attackTypeComboBox = new System.Windows.Forms.ComboBox();
            this.attackTypeLabel = new System.Windows.Forms.Label();
            this.attackTimeLabel = new System.Windows.Forms.Label();
            this.attackBuilderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // attackBuilderGroupBox
            // 
            this.attackBuilderGroupBox.Controls.Add(this.attackTimeLabel);
            this.attackBuilderGroupBox.Controls.Add(this.attackDateTimerPicker);
            this.attackBuilderGroupBox.Controls.Add(this.nodesLabel);
            this.attackBuilderGroupBox.Controls.Add(this.teamsLable);
            this.attackBuilderGroupBox.Controls.Add(this.nodesTextBox);
            this.attackBuilderGroupBox.Controls.Add(this.teamsTextBox);
            this.attackBuilderGroupBox.Controls.Add(this.submitAttackButton);
            this.attackBuilderGroupBox.Controls.Add(this.attackTypeComboBox);
            this.attackBuilderGroupBox.Controls.Add(this.attackTypeLabel);
            this.attackBuilderGroupBox.Location = new System.Drawing.Point(12, 11);
            this.attackBuilderGroupBox.Name = "attackBuilderGroupBox";
            this.attackBuilderGroupBox.Size = new System.Drawing.Size(445, 241);
            this.attackBuilderGroupBox.TabIndex = 3;
            this.attackBuilderGroupBox.TabStop = false;
            this.attackBuilderGroupBox.Text = "Attack Builder";
            // 
            // attackDateTimerPicker
            // 
            this.attackDateTimerPicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.attackDateTimerPicker.Location = new System.Drawing.Point(297, 159);
            this.attackDateTimerPicker.Name = "attackDateTimerPicker";
            this.attackDateTimerPicker.Size = new System.Drawing.Size(86, 23);
            this.attackDateTimerPicker.TabIndex = 7;
            // 
            // nodesLabel
            // 
            this.nodesLabel.AutoSize = true;
            this.nodesLabel.Location = new System.Drawing.Point(159, 43);
            this.nodesLabel.Name = "nodesLabel";
            this.nodesLabel.Size = new System.Drawing.Size(92, 15);
            this.nodesLabel.TabIndex = 6;
            this.nodesLabel.Text = "Affected Nodes:";
            // 
            // teamsLable
            // 
            this.teamsLable.AutoSize = true;
            this.teamsLable.Location = new System.Drawing.Point(17, 43);
            this.teamsLable.Name = "teamsLable";
            this.teamsLable.Size = new System.Drawing.Size(91, 15);
            this.teamsLable.TabIndex = 5;
            this.teamsLable.Text = "Affected Teams:";
            // 
            // nodesTextBox
            // 
            this.nodesTextBox.Location = new System.Drawing.Point(159, 61);
            this.nodesTextBox.Multiline = true;
            this.nodesTextBox.Name = "nodesTextBox";
            this.nodesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.nodesTextBox.Size = new System.Drawing.Size(100, 166);
            this.nodesTextBox.TabIndex = 4;
            // 
            // teamsTextBox
            // 
            this.teamsTextBox.Location = new System.Drawing.Point(17, 61);
            this.teamsTextBox.Multiline = true;
            this.teamsTextBox.Name = "teamsTextBox";
            this.teamsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.teamsTextBox.Size = new System.Drawing.Size(100, 166);
            this.teamsTextBox.TabIndex = 3;
            // 
            // submitAttackButton
            // 
            this.submitAttackButton.Location = new System.Drawing.Point(297, 204);
            this.submitAttackButton.Name = "submitAttackButton";
            this.submitAttackButton.Size = new System.Drawing.Size(105, 23);
            this.submitAttackButton.TabIndex = 2;
            this.submitAttackButton.Text = "Edit Attack";
            this.submitAttackButton.UseVisualStyleBackColor = true;
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
            this.attackTypeComboBox.Location = new System.Drawing.Point(297, 91);
            this.attackTypeComboBox.Name = "attackTypeComboBox";
            this.attackTypeComboBox.Size = new System.Drawing.Size(121, 23);
            this.attackTypeComboBox.TabIndex = 0;
            // 
            // attackTypeLabel
            // 
            this.attackTypeLabel.AutoSize = true;
            this.attackTypeLabel.Location = new System.Drawing.Point(297, 61);
            this.attackTypeLabel.Name = "attackTypeLabel";
            this.attackTypeLabel.Size = new System.Drawing.Size(70, 15);
            this.attackTypeLabel.TabIndex = 1;
            this.attackTypeLabel.Text = "Attack type:";
            // 
            // attackTimeLabel
            // 
            this.attackTimeLabel.AutoSize = true;
            this.attackTimeLabel.Location = new System.Drawing.Point(297, 132);
            this.attackTimeLabel.Name = "attackTimeLabel";
            this.attackTimeLabel.Size = new System.Drawing.Size(116, 15);
            this.attackTimeLabel.TabIndex = 8;
            this.attackTimeLabel.Text = "Attack starting-time:";
            // 
            // FormEditAttack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 264);
            this.Controls.Add(this.attackBuilderGroupBox);
            this.Name = "FormEditAttack";
            this.Text = "Edit Attack";
            this.attackBuilderGroupBox.ResumeLayout(false);
            this.attackBuilderGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox attackBuilderGroupBox;
        private System.Windows.Forms.Label nodesLabel;
        private System.Windows.Forms.Label teamsLable;
        private System.Windows.Forms.TextBox nodesTextBox;
        private System.Windows.Forms.TextBox teamsTextBox;
        private System.Windows.Forms.Button submitAttackButton;
        private System.Windows.Forms.ComboBox attackTypeComboBox;
        private System.Windows.Forms.Label attackTypeLabel;
        private System.Windows.Forms.DateTimePicker attackDateTimerPicker;
        private System.Windows.Forms.Label attackTimeLabel;
    }
}