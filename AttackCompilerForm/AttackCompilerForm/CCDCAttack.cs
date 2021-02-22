using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AttackCompilerForm
{
    class CCDCAttack
    {
        #region Fields

        private List<int> teamsAffected;
        private List<int> nodesAffected;

        private string attackType;
        private string startTime;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets what type of attack has just been sent.
        /// </summary>
        public string AttackType
        {
            get
            {
                return attackType;
            }
            set
            {
                attackType = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting-time of this attack.
        /// </summary>
        public String StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }

        /// <summary>
        /// Gets a list of IDs of teams affected by this attack.
        /// </summary>
        public List<int> TeamsAffected
        {
            get
            {
                return teamsAffected;
            }
        }

        /// <summary>
        /// Gets a list of IDs of nodes affected by this attack.
        /// </summary>
        public List<int> NodesAffected
        {
            get
            {
                return nodesAffected;
            }
        }

        #endregion Properties

        /// <summary>
        /// Constructor for making a serializable CCDCAttack that a JSON could parse-through.
        /// </summary>
        /// <param name="a">This attack's type.</param>
        /// <param name="n">The nodes affected by this attack.</param>
        /// <param name="t">The teams affected by this attack.</param>
        public CCDCAttack(string a, List<int> n, List<int> t, string s)
        {
            attackType = a;
            nodesAffected = n;
            teamsAffected = t;
            startTime = s;
        }

        /// <summary>
        /// Converts this object's data into a single-line string for the list box.
        /// </summary>
        /// <returns>A string of data in a single line.</returns>
        public string ToListBoxString()
        {
            string result = "";

            // Add the attack type
            result += attackType + ", ";

            // Add the affected team IDs
            result += "{ ";
            foreach (int i in teamsAffected)
            {
                result += i + ", ";
            }
            result = result.Remove(result.Length - 2, 2);
            result += " }, ";

            // Add the affected node IDs
            result += "{ ";
            foreach (int i in nodesAffected)
            {
                result += i + ", ";
            }
            result = result.Remove(result.Length - 2, 2);
            result += " }";

            // Add the starting time
            result += startTime;

            return result;
        }
    }
}
