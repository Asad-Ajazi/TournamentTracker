using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(placeNameValue.Text, placeNumberValue.Text, prizeAmountValue.Text, prizePercentageValue.Text);

                foreach (var db in GlobalConfig.Connections)
                {
                    db.CreatePrize(model);
                }
                ClearPrizeFormFields();
                MessageBox.Show("New prize was added");

            }
            else
            {
                MessageBox.Show("The form contains invalid information, please check and try again");
            }
        }

        #region helper methods

        /// <summary>
        /// Validates all the fields in the prize form.
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            //using an output variable to check through the whole form instead of returning true or false on first validation.
            bool output = true;
            int placeNumber = 0;
            //checks to see if the given number can be parsed to an int, returns bool. also the number.
            bool placeNumberVaildateNumber = int.TryParse(placeNumberValue.Text,out placeNumber);

            //placeNumber = the valid number given by the user.
            
            if (!placeNumberVaildateNumber)
            {
                // Could add error messages, but leaving them out for now.
                output = false;
            }
            if (placeNumber < 1)
            {
                output = false;
            }

            if (placeNameValue.Text.Length == 0)
            {
                output = false;
            }


            decimal prizeAmount = 0;
            double prizePercentage = 0;
            //Parses the text fields to decimal/int and returns a bool/ the number.
            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);
            //both fields must contain a value.
            if (!prizeAmountValid || !prizePercentageValid)
            {
                output = false;
            }
            //atleast one of the fields must be greater than 0.
            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false;
            }
            // The percentage must be between 1-100.
            if (prizePercentage < 0 || prizePercentage >100)
            {
                output = false;
            }
            
            return output;
        }

        private void ClearPrizeFormFields()
        {
            placeNameValue.Text = string.Empty;
            placeNumberValue.Text = string.Empty;
            prizeAmountValue.Text = "0";
            prizePercentageValue.Text = "0";
        }

        #endregion

    }
}
