﻿using System;
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
        int indexToReplace;

        #endregion Fields

        public FormEditAttack(FormAttackCompiler f, CCDCAttack a, int i)
        {
            InitializeComponent();
            mainForm = f;
            attackToEdit = a;
            indexToReplace = i;

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
                teamsTextBox.Text += t.ToString() + System.Environment.NewLine;
            }

            // Fill-in the nodes textBox
            foreach (int n in attackToEdit.NodesAffected)
            {
                nodesTextBox.Text += n.ToString() + System.Environment.NewLine;
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

        /// <summary>
        /// Compiles this form's data and converts it to a CCDCAttack, then sends it back to the main form.
        /// </summary>
        /// <param name="sender">The object calling this method.</param>
        /// <param name="e">The event being sent in.</param>
        private void submitAttackButton_Click(object sender, EventArgs e)
        {
            // Convert the team IDs into a list of integers.
            String teamBox = teamsTextBox.Text;
            String[] teams = teamBox.Split("\n");

            for (int i = 0; i < teams.Length; i++)
            {
                teams[i] = teams[i].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }

            List<int> teamIDs = new List<int>();

            foreach (String s in teams)
            {
                Int32.TryParse(s, out int result);
                teamIDs.Add(result);
            }

            // Convert the node IDs into a list of integers as-well.
            String nodeBox = nodesTextBox.Text;
            String[] nodes = nodeBox.Split("\n");

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = nodes[i].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }

            List<int> nodeIDs = new List<int>();

            foreach (String s in nodes)
            {
                Int32.TryParse(s, out int result);
                nodeIDs.Add(result);
            }

            DateTime currentTime = DateTime.Now;

            CCDCAttack newAttack = new CCDCAttack(attackTypeComboBox.Text, nodeIDs, teamIDs, attackDateTimerPicker.Value.ToShortTimeString());
            mainForm.Attacks[indexToReplace] = newAttack;

            mainForm.ListBoxAttacks.Items[indexToReplace] = newAttack.ToListBoxString();

            this.Close();
        }
    }
}
