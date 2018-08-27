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
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        public CreateTeamForm()
        {
            InitializeComponent();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (validateForm())
            {
                PersonModel p = new PersonModel();
                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.EmailAddress = emailValue.Text;
                p.MobileNumber = phoneValue.Text;

                GlobalConfig.Connection.CreatePerson(p);

                MessageBox.Show($"{p.FirstName} {p.LastName} was added to the list.");
                ClearPrizeFormFields();

            }
            else
            {
                MessageBox.Show("Not all the fields to create a member we correctly filled in.");
            }
        }


        #region helper methods

        /// <summary>
        /// Validates the create member fields.
        /// </summary>
        /// <returns></returns>
        private bool validateForm()
        {
            //basic validation checks, implement robust checks in the future.
            if (firstNameValue.Text.Length == 0) { return false; }

            if (lastNameValue.Text.Length == 0) { return false; }
            
            if (emailValue.Text.Length == 0) { return false; }
                       
            if (phoneValue.Text.Length == 0) { return false; }
            
            return true;
        }

        private void ClearPrizeFormFields()
        {
            firstNameValue.Text = string.Empty;
            lastNameValue.Text = string.Empty;
            emailValue.Text = string.Empty;
            phoneValue.Text = string.Empty;
        }
        #endregion
    }
}
