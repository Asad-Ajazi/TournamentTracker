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
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament;
        BindingList<int> rounds = new BindingList<int>();
        BindingList<MatchupModel> selectedMatchups = new BindingList<MatchupModel>();

        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();
            // Stores the passed in tournament model to the form level, so anything on the form can access the object.
            tournament = tournamentModel;

            PopulateList();

            LoadFormData();
            LoadRounds();

        }

        /// <summary>
        /// loads the form data for the selected tournament.
        /// </summary>
        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }

        /// <summary>
        /// populates the dropdown and listbox.
        /// </summary>
        private void PopulateList()
        {
            roundDropDown.DataSource = rounds;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        /// <summary>
        /// loads the rounds for the dropdown
        /// </summary>
        private void LoadRounds()
        {
            rounds.Clear();

            rounds.Add(1);
            int currentRound = 1;

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {                
                if (matchups.First().MatchupRound > currentRound)
                {
                    currentRound = matchups.First().MatchupRound;
                    rounds.Add(currentRound);
                }
            }

            LoadMatchups(1);
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }

        /// <summary>
        /// loads list of matchups for the rounds
        /// </summary>
        private void LoadMatchups(int round)
        {
            //int round = (int)roundDropDown.SelectedItem;

            // get all the matchups for the round needed and displayed them.
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    selectedMatchups.Clear();
                    foreach (MatchupModel m in matchups)
                    {
                        // add if there is no winner, or is checkbox is not checked.
                        if (m.Winner == null || !unplayedOnlyCheckBox.Checked)
                        {
                            selectedMatchups.Add(m);
                        }
                    }
                }
            }
            if (selectedMatchups.Count > 0)
            {
                LoadMatchup(selectedMatchups.First()); 
            }
            DisplayMatchupInfo();
        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = (selectedMatchups.Count > 0);
            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;

            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;

            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;

        }

        /// <summary>
        /// loads a singular matchup for the selected matchup.
        /// </summary>
        private void LoadMatchup(MatchupModel m)
        {
            // additional check to make sure lists do not reset m to null. 
            if (matchupListBox.SelectedItem != null)
            {
                for (int i = 0; i < m.Entry.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entry[0].TeamCompeting != null)
                        {
                            teamOneName.Text = m.Entry[0].TeamCompeting.TeamName;
                            teamOneScoreValue.Text = m.Entry[0].Score.ToString();

                            // setting default team two to BYE for first round, will be overwritten in later if.
                            teamTwoName.Text = "<BYE>";
                            teamTwoScoreValue.Text = "0";
                        }
                        else
                        {
                            teamOneName.Text = "Not Yet Set";
                            teamOneScoreValue.Text = "";
                        }
                    }

                    if (i == 1)
                    {
                        if (m.Entry[1].TeamCompeting != null)
                        {
                            teamTwoName.Text = m.Entry[1].TeamCompeting.TeamName;
                            teamTwoScoreValue.Text = m.Entry[1].Score.ToString();
                        }
                        else
                        {
                            teamTwoName.Text = "Not Yet Set";
                            teamTwoScoreValue.Text = "";
                        }
                    }
                }
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }

        private void unplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;

            double teamOneScore = 0;
            double teamTwoScore = 0;

            for (int i = 0; i < m.Entry.Count; i++)
            {
                // loop through team one.
                if (i == 0)
                {
                    if (m.Entry[0].TeamCompeting != null)
                    {
                        bool scoreValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);

                        if (scoreValid)
                        {
                            m.Entry[0].Score = teamOneScore;
                        }
                        else
                        {
                            // return is incorrect information is supplied.
                            MessageBox.Show("Please enter a valid score for team 1");
                            return;
                        }
                    }
                }

                // loop through team two.
                if (i == 1)
                {
                    if (m.Entry[1].TeamCompeting != null)
                    {
                        bool scoreValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);

                        if (scoreValid)
                        {
                            m.Entry[1].Score = teamTwoScore;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid score for team 2");
                            return;
                        }
                    }
                }
            }

            if (teamOneScore > teamTwoScore)
            {
                // team one wins.
                m.Winner = m.Entry[0].TeamCompeting;
            }
            else if (teamTwoScore > teamOneScore)
            {
                // team two wins
                m.Winner = m.Entry[1].TeamCompeting;
            }
            else
            {
                MessageBox.Show("We don't have ties around here, play a sudden death round!");
            }

            foreach (List<MatchupModel> round in tournament.Rounds)
            {
                foreach (MatchupModel rm in round)
                {
                    foreach (MatchupEntryModel me in rm.Entry)
                    {
                        if (me.ParentMatchup != null)
                        {
                            if (me.ParentMatchup.Id == m.Id)
                            {
                                me.TeamCompeting = m.Winner;
                                GlobalConfig.Connection.UpdateMatchup(rm);
                            } 
                        }
                    }
                }
            }

            // refrehes list when score button is clicked and checkbox is checked to auto refresh and remove it from the list.
            LoadMatchups((int)roundDropDown.SelectedItem);

            GlobalConfig.Connection.UpdateMatchup(m);
        }
    }
}
