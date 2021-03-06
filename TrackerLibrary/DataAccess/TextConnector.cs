﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    // saving to text file was discontuined, implemented functions still work.
    public class TextConnector : IDataConnection
    {

        //storing items in induvidual text files(csv), per model.
        //private const string PrizesFile = "PrizeModels.csv";
        //private const string PersonFile = "PersonModels.csv";
        //private const string TeamFile = "TeamModels.csv";
        //private const string TournamentFile = "TournamentModels.csv";

        public void CreatePerson(PersonModel model)
        {
            //read all the people in the text file.
            List<PersonModel> person = GlobalConfig.PersonFile.FullFilePath().LoadFile().ConvertToPersonModels();

            // If there is no file, the default id of 1 will be used to create it.
            int currentId = 1;
            if (person.Count > 0)
            {
                // gets the max id currently in the list and adds 1.
                currentId = person.OrderByDescending(p => p.Id).First().Id + 1;
            }

            model.Id = currentId;

            // Add new record with new ID (max + 1)
            person.Add(model);

            // Convert prizes to list<string>.
            // Save list<string> to text file.
            person.SaveToPersonFile(GlobalConfig.PersonFile);
        }

        public void CreatePrize(PrizeModel model)
        {           
            //Loads the text file and converts it to a List<PrizeModel>
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // If there is no file, the default id of 1 will be used to create it.
            int currentId = 1;
            if (prizes.Count > 0)
            {
                // gets the max id currently in the list and adds 1.
                currentId = prizes.OrderByDescending(p => p.Id).First().Id + 1;
            }

            model.Id = currentId;

            // Add new record with new ID (max + 1)
            prizes.Add(model);

            // Convert prizes to list<string>.
            // Save list<string> to text file.
            prizes.SaveToPrizesFile(GlobalConfig.PrizesFile);
        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PersonFile);

            // If there is no file, the default id of 1 will be used to create it.
            int currentId = 1;
            if (teams.Count > 0)
            {
                // gets the max id currently in the list and adds 1.
                currentId = teams.OrderByDescending(p => p.id).First().id + 1;
            }
            model.id = currentId;
            teams.Add(model);

            teams.SaveToTeamFile(GlobalConfig.TeamFile);
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournament = GlobalConfig.TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels(GlobalConfig.TeamFile, GlobalConfig.PersonFile, GlobalConfig.PrizesFile);
            // If there is no file, the default id of 1 will be used to create it.
            int currentId = 1;
            if (tournament.Count > 0)
            {
                // gets the max id currently in the list and adds 1.
                currentId = tournament.OrderByDescending(p => p.id).First().id + 1;
            }

            model.id = currentId;
            tournament.Add(model);
            tournament.SaveToTournametFile(GlobalConfig.TournamentFile);

        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PersonFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PersonFile);
        }

        public List<TournamentModel> GetTournament_All()
        {
            // Discontinued text implementation, Possibly add in the future.
            throw new NotImplementedException();
        }

        public void UpdateMatchup(MatchupModel model)
        {
            throw new NotImplementedException();
        }
    }
}
