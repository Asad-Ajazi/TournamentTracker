using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        /// <summary>
        /// Stores a new prize to the database
        /// </summary>
        /// <param name="model">Prize information</param>
        /// <returns>Prize information including id</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // TODO - implement the CreatePrize method (save to the database.)
            model.Id = 1;
            return model;
        }
    }
}
