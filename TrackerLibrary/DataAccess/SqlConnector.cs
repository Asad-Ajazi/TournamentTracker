﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string dbName = "Tournaments";

        public PersonModel CreatePerson(PersonModel model)
        {
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString(dbName)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                //phone number and mobile number are the same.
                p.Add("@PhoneNumber", model.MobileNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPerson_insert", p, commandType: CommandType.StoredProcedure);

                //sets the model.id to the value of the id that was inserted.
                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// <summary>
        /// Stores a new prize to the database
        /// </summary>
        /// <param name="model">Prize information</param>
        /// <returns>Prize information including id</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString(dbName)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrize_insert", p, commandType: CommandType.StoredProcedure);

                //sets the model.id to the value of the id that was inserted.
                model.Id = p.Get<int>("@id");

                return model;
            }

        }

        public TeamModel CreateTeam(TeamModel model)
        {
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString(dbName)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeam_Insert", p, commandType: CommandType.StoredProcedure);

                //sets the model.id to the value of the id that was inserted.
                model.id = p.Get<int>("@id");

                //loops through and inserts rows into the TeamMember table.
                foreach (PersonModel teamMember in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@Team_ID", model.id);
                    p.Add("@People_ID", teamMember.Id);

                    connection.Execute("dbo.spTeamMember_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return model;
            }
        }

        public void CreateTournament(TournamentModel model)
        {
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString(dbName)))
            {
                SaveTournament(connection, model);
                SaveTournamentPrizes(connection, model);
                SaveTournamentEntries(connection, model);
                SaveTournamentRounds(connection, model);
                //return model;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString(dbName)))
            {
                //executes the stored procedure to a list of PersonModel into output.
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString(dbName)))
            {
                //executes the stored procedure to a list of PersonModel into output.
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();


                //query team members list for each team in list of teams.
                foreach (TeamModel t in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@Team_ID", t.id);
                    t.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMember_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }

        #region CreateTournament helpers
        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.Entryfee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);

            //sets the model.id to the value of the id that was inserted.
            model.id = p.Get<int>("@id");
        }
        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            //loops through and inserts rows into tournamentprize.
            foreach (PrizeModel prize in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@Tournament_ID", model.id);
                p.Add("@Prize_ID", prize.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentPrize_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel tm in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@Tournament_ID", model.id);
                p.Add("@Team_ID", tm.id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            // tournament model - List<List<MatchupModel>> Rounds
            // matchupmodel - public List<MatchupEntryModel> Entry

            // 1. loop through rounds. list<list<matchupmodel>>
            // 2. loop through the matchups.
            // 3. Save the matchup
            // 4. loop through the entries of the saved matchup + save them.

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@Tournament_ID", model.id);
                    p.Add("@MatchupRound", matchup.MatchRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchup_Insert", p, commandType: CommandType.StoredProcedure);

                    //sets the model.id to the value of the id that was inserted.
                    matchup.Id = p.Get<int>("@id");

                    foreach (MatchupEntryModel entry in matchup.Entry)
                    {
                        p = new DynamicParameters();
                        // matchup id from previous foreach.
                        p.Add("@Matchup_ID", matchup.Id);     
                        
                        if (entry.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchup_ID", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchup_ID", entry.ParentMatchup.Id);
                        }

                        if (entry.TeamCompeting == null)
                        {
                            p.Add("@TeamCompeting_ID", null);
                        }
                        else
                        {
                            p.Add("@TeamCompeting_ID", entry.TeamCompeting.id);
                        }
                        
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                    }
                }
            }


        }
        #endregion
    }
}
