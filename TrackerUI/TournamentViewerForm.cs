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
        List<int> rounds = new List<int>();
        List<MatchupModel> selectedMatchups = new List<MatchupModel>();

        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();
            // Stores the passed in tournament model to the form level, so anything on the form can access the object.
            tournament = tournamentModel;

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

        private void PopulateRoundsList()
        {
            roundDropDown.DataSource = null;
            roundDropDown.DataSource = rounds;
        }

        private void PopulateMatchupsList()
        {
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        /// <summary>
        /// loads the rounds for the dropdown
        /// </summary>
        private void LoadRounds()
        {
            rounds = new List<int>();

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
            PopulateRoundsList();
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        /// <summary>
        /// loads list of matchups for the rounds
        /// </summary>
        private void LoadMatchups()
        {
            int round = (int)roundDropDown.SelectedItem;

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    selectedMatchups = matchups;
                }
            }
            PopulateMatchupsList();
        }

        /// <summary>
        /// loads a singular matchup for the selected matchup.
        /// </summary>
        private void LoadMatchup()
        {
            // additional check to make sure lists do not reset m to null. 
            if (matchupListBox.SelectedItem != null)
            {


                MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;

                for (int i = 0; i < m.Entry.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entry[0].TeamCompeting != null)
                        {
                            teamOneName.Text = m.Entry[0].TeamCompeting.TeamName;
                            teamOneScoreValue.Text = m.Entry[0].Score.ToString();

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
            LoadMatchup();
        }
    }
}
