using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The place number e.g 1, 2, 3 ...
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// alternate name for place, eg. First, second, Winner, runner up.
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// The amount of money to be awarded to the person who finished in this place.
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// The percentage of the prize pool to be given to the person who finished in this place.
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        /// <summary>
        /// Convert inputs and create PrizeModel 
        /// </summary>
        /// <param name="placeName"></param>
        /// <param name="placeNumber"></param>
        /// <param name="prizeAmount"></param>
        /// <param name="prizePercentage"></param>
        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int.TryParse(placeNumber, out int placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal.TryParse(prizeAmount, out decimal prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double.TryParse(prizePercentage, out double prizePercentageValue);
            PrizePercentage = prizePercentageValue;



        }

    }
}
