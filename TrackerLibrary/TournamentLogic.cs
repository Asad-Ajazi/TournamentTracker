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
            int round = FindNumOfRounds(randomziedTeams.Count);
            int byes = NumberOfByes(round, randomziedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomziedTeams));
            CreateOtherRounds(model, round);

        }

        /// <summary>
        /// Creates the remaining rounds for the tournament.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="round"></param>
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            // First round is already taken care of so we start at round 2.
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currentRound = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            // populates the model with every set of rounds.
            while (round <= rounds)
            {
                foreach (MatchupModel m in previousRound)
                {
                    currentMatchup.Entry.Add(new MatchupEntryModel { ParentMatchup = m });

                    if (currentMatchup.Entry.Count > 1)
                    {
                        currentMatchup.MatchupRound = round;
                        currentRound.Add(currentMatchup);
                        currentMatchup = new MatchupModel();
                    }
                }
                model.Rounds.Add(currentRound);
                previousRound = currentRound;
                currentRound = new List<MatchupModel>();
                round += 1;
            }

        }

        /// <summary>
        /// Creates the matchups for the first round.
        /// </summary>
        /// <param name="byes"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel current = new MatchupModel();
            
            // a single team is only half of a matchup, find other half before adding to 'current'.
            foreach (TeamModel team in teams)
            {
                current.Entry.Add(new MatchupEntryModel { TeamCompeting = team });

                // byes will be added to the first teams in the list.
                if (byes > 0 || current.Entry.Count > 1)
                {
                    // first round will always be hard coded to 1.
                    current.MatchupRound = 1;
                    output.Add(current);
                    current = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }                
            }
            return output;
        }

        /// <summary>
        /// Calculates the number of byes needed (if any) for the tournament.
        /// </summary>
        /// <param name="rounds"></param>
        /// <param name="teamcount"></param>
        /// <returns></returns>
        private static int NumberOfByes(int rounds, int teamcount)
        {
            int output = 0;
            int totalTeams = 1;

            // starting on a 1 based counting systems to *2.
            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }
            output = totalTeams - teamcount;
            return output;
        }

        /// <summary>
        /// Returns an int of the number of rounds in the tournament.
        /// </summary>
        /// <param name="teamcount"></param>
        /// <returns></returns>
        private static int FindNumOfRounds(int teamcount)
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
