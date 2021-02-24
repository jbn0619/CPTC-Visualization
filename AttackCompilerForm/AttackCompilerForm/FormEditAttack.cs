using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace AttackCompilerForm
{
    public partial class FormEditAttack : Form
    {
        #region Fields

        CCDCAttack attackToEdit;
        FormAttackCompiler mainForm;

        #endregion Fields

        public FormEditAttack(FormAttackCompiler f, CCDCAttack a)
        {
            InitializeComponent();
            mainForm = f;
            attackToEdit = a;

            RebuildAttack();
        }

        /// <summary>
        /// Populates the form's fields with the input-attack's information.
        /// </summary>
        private void RebuildAttack()
        {
            // FIll-in the teams textBox
            foreach (int t in attackToEdit.TeamsAffected)
            {
                teamsTextBox.Text += t.ToString() + "\n";
            }

            // Fill-in the nodes textBox
            foreach (int n in attackToEdit.NodesAffected)
            {
                nodesTextBox.Text += n.ToString() + "\n";
            }

            // Fill-in the attack dropdown box.
            attackTypeComboBox.SelectedText = attackToEdit.AttackType;

            // Fill-in the time box.
            CultureInfo cInfo = new CultureInfo("en-US");
            attackDateTimerPicker.Value = DateTime.ParseExact(attackToEdit.StartTime, "t", cInfo);
        }

        /// <summary>
        /// Checks to make-sure the input is only a number before updating the text.
        /// </summary>
        /// <param name="sender">The object calling this method.</param>
        /// <param name="e">The event being sent in.</param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the new digit is a number or new-line.
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
