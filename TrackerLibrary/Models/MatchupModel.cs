using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// The unique identifier for the matchup.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A list of matchups for the tournament.
        /// </summary>
        public List<MatchupEntryModel> Entry { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// The team that won the current round.
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// The round number.
        /// </summary>
        public int MatchRound { get; set; }
    }
}
