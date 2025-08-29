using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;
using WorldCupStats.data.Services;

namespace WordCupStats.WinForms
{
    public partial class AttendanceRankingsForm : Form
    {
        private string championship;
        private readonly IStatisticsService statisticsService;

        public AttendanceRankingsForm(string championship)
        {
            InitializeComponent();
            statisticsService = new StatisticsService(new FileDataRepository());
            this.championship = championship;

            statisticsService.GetMatchesByAttendanceAsync(10)
              .ContinueWith(task =>
              {
                  if (task.IsFaulted)
                  {
                      MessageBox.Show("Error loading matches: " + task.Exception?.Message);
                  }
                  else
                  {
                      cmbMatches.DataSource = task.Result;
                      cmbMatches.DisplayMember = "DisplayName" ;
                      cmbMatches.ValueMember = "FifaId"; 
                  }
              }, TaskScheduler.FromCurrentSynchronizationContext());
        }
            
        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMatches.SelectedItem is Match selectedMatch)
                {
                    tbAttendence.Clear();
                    tbLocation.Clear();
                    tbHomeTeam.Clear();
                    tbAwayTeam.Clear();
                    tbAttendence.Text = selectedMatch.Attendance.ToString();
                    tbLocation.Text = selectedMatch.Location.ToString();
                    tbHomeTeam.Text = selectedMatch.HomeTeam.Country;
                    tbAwayTeam.Text = selectedMatch.AwayTeam.Country;
                }
                else
                {
                    MessageBox.Show("Please select a match from the list.");
                }
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show(
                    "An error occurred while loading match details: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
           
        }
    }
}
