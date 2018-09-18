﻿using System;
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
                if (matchups.First().MatchRound > currentRound)
                {
                    currentRound = matchups.First().MatchRound;
                    rounds.Add(currentRound);
                }
            }
            PopulateList();
        }

        private void PopulateList()
        {
            roundDropDown.DataSource = null;
            roundDropDown.DataSource = rounds;
        }
    }
}
