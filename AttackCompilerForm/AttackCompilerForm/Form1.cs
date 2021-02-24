using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttackCompilerForm
{
    public partial class attackCompilerForm : Form
    {
        #region Fields

        List<CCDCAttack> attacks;

        string fileName;

        #endregion Fields

        #region Properties



        #endregion Properties

        public attackCompilerForm()
        {
            InitializeComponent();

            attacks = new List<CCDCAttack>();
            fileName = "attacks.json";
        }

        /// <summary>
        /// Checks to make-sure the input is only a number before updating the text.
        /// </summary>
        /// <param name="sender">The object calling this method.</param>
        /// <param name="e">The event being sent in.</param>
        private void teamsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the new digit is a number or new-line.
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Compiles the information from the attackBuilderGroupBox into a CCDCAttack object.
        /// </summary>
        /// <param name="sender">The button that invoked this method.</param>
        /// <param name="e">The eventArgs sent with this call.</param>
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
            
            CCDCAttack newAttack = new CCDCAttack(attackTypeComboBox.Text, nodeIDs, teamIDs, currentTime.ToShortTimeString());
            attacks.Add(newAttack);

            compiledAttacksListBox.Items.Add(newAttack.ToListBoxString());
        }

        #region Menu Strip Methods

        /// <summary>
        /// Takes all the CCDCAttack objects and writes them to a JSON file.
        /// </summary>
        /// <param name="sender">The menu item that invoked this method.</param>
        /// <param name="e">This event handler.</param>
        private void saveFileMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(fileName, string.Empty);
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (CCDCAttack a in attacks)
                {
                    string attack = JsonSerializer.Serialize(attacks[0]);
                    sw.Write(attack);
                }
            }
        }

        /// <summary>
        /// Takes all the CCDCAttack objects and writes them to a new JSON file at a user-determined location.
        /// </summary>
        /// <param name="sender">The menu item that invoked this method.</param>
        /// <param name="e">This event handler.</param>
        private void saveAsFileMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, string.Empty);
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (CCDCAttack a in attacks)
                    {
                        string attack = JsonSerializer.Serialize(attacks[0]);
                        sw.Write(attack);
                    }
                }
            }
        }

        #endregion Menu Strip Methods

        #region ListBox-Polish Methods

        /// <summary>
        /// Checks if a right-click has occured on an item in this list box before making a context menu strip appear.
        /// </summary>
        /// <param name="sender">The list box that was clicked.</param>
        /// <param name="e">Information about the mouse click.</param>
        private void compiledAttacksListBox_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                int index = lb.IndexFromPoint(e.Location);
                if (index != System.Windows.Forms.ListBox.NoMatches)
                {
                    lb.SelectedIndex = index;

                    // Spawn-in the context menu strip.
                    attacksContextMenuStrip.Show(lb, e.Location);
                }
            }
        }

        /// <summary>
        /// Deletes the selected-attack when this method is invoked.
        /// </summary>
        /// <param name="sender">The menu item that invoked this method.</param>
        /// <param name="e">The eventArgs sent with this call.</param>
        private void deleteAttackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this attack?", "Delete Attack", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                attacks.RemoveAt(this.compiledAttacksListBox.SelectedIndex);
                this.compiledAttacksListBox.Items.RemoveAt(this.compiledAttacksListBox.SelectedIndex);
                this.compiledAttacksListBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Duplicates the selected-attack when this method is invoked.
        /// </summary>
        /// <param name="sender">The menu item that invoked this method.</param>
        /// <param name="e">The eventArgs sent with this call.</param>
        private void duplicateAttackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = this.compiledAttacksListBox.SelectedIndex;

            CCDCAttack duplicateAttack = attacks[index];
            attacks.Add(duplicateAttack);
            compiledAttacksListBox.Items.Add(duplicateAttack.ToListBoxString());
        }

        /// <summary>
        /// Edits the selected-attack when this method is invoked.
        /// </summary>
        /// <param name="sender">The menu item that invoked this method.</param>
        /// <param name="e">The eventArgs sent with this call.</param>
        private void editAttackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Edit works!");
        }

        #endregion ListBox-Polish Methods
    }
}
