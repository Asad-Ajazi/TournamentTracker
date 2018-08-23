using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // TODO - implement the CreatePrize method (save to the text file.)
            model.Id = 1;
            return model;
        }
    }
}
