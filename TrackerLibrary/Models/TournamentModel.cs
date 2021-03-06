﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        /// <summary>
        /// The unique identifier for the tournament.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the given name for this tournament.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// The required entry fee for this tournament.
        /// </summary>
        public decimal Entryfee { get; set; }
        
        //initialize all the lists

        /// <summary>
        /// A list of the teams entering this tournament.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// A list of the available prizes for this tournament.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// The current round of the tournament, a list of list of matchupmodels.
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

    }
}
