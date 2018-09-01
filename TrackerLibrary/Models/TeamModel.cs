using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// The unique identifier for the team
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///The name assigned to this team 
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Represents all of the members in a team
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>(); //initialize the list
        

    }
}
