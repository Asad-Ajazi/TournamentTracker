using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PersonModel
    {

        /// <summary>
        /// The unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The users first name.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// The users Last Name.
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// The Email Address of this person.
        /// </summary>
        public string EmailAddress { get; set; }
        
        /// <summary>
        /// The mobile phone number of this person.
        /// </summary>
        public string MobileNumber { get; set; }

    }
}
