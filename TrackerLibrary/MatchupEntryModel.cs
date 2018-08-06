using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Represents a single team in the matchup.
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represents the score for this team.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Represents the previous matchup that this team won.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }


    }
}
