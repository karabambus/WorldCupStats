using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;
using WorldCupStats.data.Services;

namespace WorldCupStats.WPF
{
    public partial class TeamInfoWindow : Window
    {
        private readonly string teamFifaCode;
        private readonly Teams team;
        private IStatisticsService _statisticsService;
        public TeamInfoWindow(Teams team)
        {
            InitializeComponent();
            this.team = team;
            this.teamFifaCode = team.FifaCode;
            txtTeamName.Text = team.Country;
            _statisticsService = new StatisticsService(new FileDataRepository());
            LoadTeamStatisticsAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start animation
            Storyboard sb = (Storyboard)this.Resources["WindowAnimation"];
            sb.Begin();
        }

        private async void LoadTeamStatisticsAsync()
        {
            try
            {
                var stats = await _statisticsService.GetTeamStatisticsCalculatedAsync(teamFifaCode);

                UpdateUI(stats);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading team statistics: {ex.Message}");
            }
        }

        private void UpdateUI(TeamStatisticsCalculated stats)
        {
            txtTeamName.Text = stats.Country;
            txtFifaCode.Text = $"FIFA Code: {stats.FifaCode}";
            txtGamesPlayed.Text = stats.GamesPlayed.ToString();
            txtWins.Text = stats.Wins.ToString();
            txtLosses.Text = stats.Losses.ToString();
            txtDraws.Text = stats.Draws.ToString();
            txtGoalsFor.Text = stats.GoalsFor.ToString();
            txtGoalsAgainst.Text = stats.GoalsAgainst.ToString();

            string differential = stats.GoalDifferential >= 0
                ? $"+{stats.GoalDifferential}"
                : stats.GoalDifferential.ToString();
            txtGoalDifferential.Text = differential;

            txtGoalDifferential.Foreground = stats.GoalDifferential >= 0
                ? System.Windows.Media.Brushes.Green
                : System.Windows.Media.Brushes.Red;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
