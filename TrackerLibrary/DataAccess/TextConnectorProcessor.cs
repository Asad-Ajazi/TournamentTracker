﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

// *Load the text file.
// *Convert text to List<PrizeModel>.
// Find last id ID.
// Add new record with new ID (last + 1)
// Convert prizers to list<string>.
// Save list<string> to text file.


// Namespace to .TextConnector to limit the classes that can directly use it.
namespace TrackerLibrary.DataAccess.TextHelpers
{
    // Discontinued, replaced with sql server database. Implementation still works.
    #region static helper methods
    public static class TextConnectorProcessor
    {
        /// <summary>
        /// Takes in a file name and appends it to the folder path in AppSettings, (Change file storage location in app settings for own system)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        /// <summary>
        /// Takes in the full file path and returns back a list of string.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        /// <summary>
        /// Take in a list of string from the LoadFile method and return a list of PrizeModel.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] col = line.Split(',');

                //convert the text to a list of PrizeModel.
                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(col[0]);
                p.PlaceNumber = int.Parse(col[1]);
                p.PlaceName = col[2];
                p.PrizeAmount = decimal.Parse(col[3]);
                p.PrizePercentage = double.Parse(col[4]);
                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> person = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            //This took a while to figure out. Can be improved.

            foreach (string line in lines)
            {
                string[] col = line.Split(',');

                TeamModel t = new TeamModel();
                t.id = int.Parse(col[0]);
                t.TeamName = col[1];

                //splits list of people id on | character.
                string[] peopleIds = col[2].Split('|');

                foreach (string id in peopleIds)
                {
                    //filter list of people in text file where id matches.
                    t.TeamMembers.Add(person.Where(x => x.Id == int.Parse(id)).First());
                }
                output.Add(t);
            }
            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines,
            string teamFileName,
            string personFileName,
            string prizeFileName)
        {
            // id,TournamentName, EntryFee,(id|id|id - EnteredTeams), (id|id|id - Prizes), (Rounds - id^id^id|id^id^id|id^id^id)
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(personFileName);
            List<PrizeModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                //split on ,
                string[] col = line.Split(',');

                TournamentModel tm = new TournamentModel();
                // id
                tm.id = int.Parse(col[0]);
                // Tournament name
                tm.TournamentName = col[1];
                // Entry fee
                tm.Entryfee = decimal.Parse(col[2]);

                // entered teams
                string[] teamIds = col[3].Split('|');
                foreach (string id in teamIds)
                {
                    // add first id where matches into tm.entered teams.
                    tm.EnteredTeams.Add(teams.Where(x => x.id == int.Parse(id)).First());
                }

                // Prizes
                string[] prizeIds = col[4].Split('|');
                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }

                // Rounds (the hard part)
                // TODO - Complete rounds information. (Discontinued the text connection)


                output.Add(tm);
            }
            return output;

        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] col = line.Split(',');

                //convert the text to a list of PeopleModel.
                PersonModel p = new PersonModel();
                p.Id = int.Parse(col[0]);
                p.FirstName = col[1];
                p.LastName = col[2];
                p.EmailAddress = col[3];
                p.MobileNumber = col[4];
                output.Add(p);
            }

            return output;
        }



        /// <summary>
        /// Converts list of PrizeModel to List of string and writes contents to file.
        /// </summary>
        /// <param name="models"></param>
        /// <param name="fileName"></param>
        public static void SaveToPrizesFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPersonFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.MobileNumber}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{t.id},{t.TeamName},{ConvertPersonListToString(t.TeamMembers)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTournametFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($@"{tm.id},
                    {tm.TournamentName},
                    {tm.Entryfee},
                    {ConvertTeamListToString(tm.EnteredTeams)},
                    {ConvertPrizeListToString(tm.Prizes)},
                    {ConvertRoundListToString(tm.Rounds)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> round)
        {
            // (Rounds - id^id^id|id^id^id|id^id^id)
            string output = "";

            if (round.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in round)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }
            //removes the last | character from the end of the return string.
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{ m.Id }|^";
            }
            //removes the last | character from the end of the return string.
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> prize)
        {
            string output = "";

            if (prize.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prize)
            {
                output += $"{ p.Id }|";
            }
            //removes the last | character from the end of the return string.
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel tm in teams)
            {
                output += $"{ tm.id }|";
            }
            //removes the last | character from the end of the return string.
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        /// <summary>
        /// Takes a list of person model and returns a string with values separated by '|'.
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        private static string ConvertPersonListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count==0)
            {
                return "";
            }

            foreach (PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }
            //removes the last | character from the end of the return string.
            output = output.Substring(0, output.Length - 1);

            return output;
        }



    }
    #endregion
}

