using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class TeamModel
    {
        /// <summary>
        /// Represents all of the members in a team
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>(); //initialize the list
        
        /// <summary>
        ///The name assigned to this team 
        /// </summary>
        public string TeamName { get; set; }
    }
}
