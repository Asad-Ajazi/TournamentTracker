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
        private ITeamRequester callingForm;
        //Lists that will populate the dropdown and list box on the create team form from the datasource.
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            callingForm = caller;
            //SampleData();
            WireUpLists();
        }


        /// <summary>
        /// sample data to test if the list populates correctly
        /// </summary>
        private void SampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Bob", LastName = "Jones" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Forest", LastName = "Mike" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Harry", LastName = "Kane" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Kate", LastName = "Julian" });

        }

        /// <summary>
        /// Wires up the datasources to the dropdown and listbox.
        /// </summary>
        public void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";


            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
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

                selectedTeamMembers.Add(p);
                WireUpLists();

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

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            // Remove a person from the available list to the selected list.
            if (person != null)
            {
                availableTeamMembers.Remove(person);
                selectedTeamMembers.Add(person);

                WireUpLists(); 
            }

        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)teamMembersListBox.SelectedItem;
            // Remove a person from the selected list to the available list.
            if (person != null)
            {
                selectedTeamMembers.Remove(person);
                availableTeamMembers.Add(person);

                WireUpLists(); 
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();

            t.TeamName = teamNameValue.Text;
            t.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);
            MessageBox.Show("Team created");
            this.Close();
            
        }
    }
}
