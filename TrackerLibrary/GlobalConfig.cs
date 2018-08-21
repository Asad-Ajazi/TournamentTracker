using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        //List to hold one or more data sources the data will be saved to/ pulled from.
        public static List<IDataConnection> Connections { get; private set; }

        public static void InitializeConnections(bool database, bool textfile)
        {
            if (database)
            {
                // TODO - create connection to database
            }
            if (textfile)
            {
                // TODO - create connection to text file.
            }
        }
    }
}
