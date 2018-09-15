using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {

        //storing items in induvidual text files(csv), per model.
        private const string PrizesFile = "PrizeModels.csv";
        private const string PersonFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";
        private const string TournamentFile = "TournamentModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            //read all the people in the text file.
            List<PersonModel> person = PersonFile.FullFilePath().LoadFile().ConvertToPersonModels();

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
            person.SaveToPersonFile(PersonFile);

            return model;

        }

        public PrizeModel CreatePrize(PrizeModel model)
        {           
            //Loads the text file and converts it to a List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

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
            prizes.SaveToPrizesFile(PrizesFile);
            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonFile);

            // If there is no file, the default id of 1 will be used to create it.
            int currentId = 1;
            if (teams.Count > 0)
            {
                // gets the max id currently in the list and adds 1.
                currentId = teams.OrderByDescending(p => p.id).First().id + 1;
            }
            model.id = currentId;
            teams.Add(model);

            teams.SaveToTeamFile(TeamFile);
            return model;
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournament = TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels(TeamFile, PersonFile, PrizesFile);
            // If there is no file, the default id of 1 will be used to create it.
            int currentId = 1;
            if (tournament.Count > 0)
            {
                // gets the max id currently in the list and adds 1.
                currentId = tournament.OrderByDescending(p => p.id).First().id + 1;
            }

            model.id = currentId;
            tournament.Add(model);
            tournament.SaveToTournametFile(TournamentFile);

        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonFile);
        }

        public List<TournamentModel> GetTournament_All()
        {
            // Discontinued text implementation, Possibly add in the future.
            throw new NotImplementedException();
        }
    }
}
