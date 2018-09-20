using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                        selectedMatchups.Add(m);
                    }
                }
            }
            LoadMatchup(selectedMatchups.First());
        }

        /// <summary>
        /// loads a singular matchup for the selected matchup.
        /// </summary>
        private void LoadMatchup(MatchupModel m)
        {
            // additional check to make sure lists do not reset m to null. 
            if (matchupListBox.SelectedItem != null)
            {


                // MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;

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
    }
}
