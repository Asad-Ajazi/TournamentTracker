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
        public PersonModel CreatePerson(PersonModel model)
        {
            // using statement destroys the connection at the end of the closing curly brace }
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("Tournaments")))
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
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("Tournaments")))
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
    }
}
