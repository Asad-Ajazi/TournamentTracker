using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Randomize teams.
        // Check if list is big enough (or add byes)
        // Create first round, then following rounds. 8 - 4 - 2 - 1 - winner

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomziedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int round = FindNumOfRound(randomziedTeams.Count);

        }

        /// <summary>
        /// Returns an int of the number of rounds in the tournament.
        /// </summary>
        /// <param name="teamcount"></param>
        /// <returns></returns>
        private static int FindNumOfRound(int teamcount)
        {
            int output = 1;
            int val = 2;

            while (val < teamcount)
            {
                output += 1;
                val *= 2;
            }
            return output;
        }

        /// <summary>
        /// Randomizes a list of teammodel that is provided.
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // Randomizes the team model that is passed in an returns it.
            return teams.OrderBy(p => Guid.NewGuid()).ToList();
        }

    }
}
