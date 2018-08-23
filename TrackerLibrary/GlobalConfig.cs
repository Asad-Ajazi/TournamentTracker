using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        //List to hold one or more data sources the data will be saved to/ pulled from.
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool database, bool textfile)
        {
            if (database)
            {
                // TODO - Setup sql connection
                SqlConnector sql = new SqlConnector();
                Connections.Add(sql);
            }
            if (textfile)
            {
                // TODO - setup text connection
                TextConnector textConnection = new TextConnector();
                Connections.Add(textConnection);
            }
        }
        public static string ConString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
