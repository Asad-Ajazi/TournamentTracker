using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// The unique identifier for the matchup Entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Idendifier for the team
        /// </summary>
        public int TeamCompeting_ID { get; set; }

        /// <summary>
        /// Represents a single team in the matchup.
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represents the score for this team.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// The unique identifier for the parent matchup (the team)
        /// </summary>
        public int ParentMatchup_ID { get; set; }

        /// <summary>
        /// Represents the previous matchup that this team won.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }


    }
}
