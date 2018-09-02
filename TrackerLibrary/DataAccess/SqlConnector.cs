using Dapper;
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
    }
}
