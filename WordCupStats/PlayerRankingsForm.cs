using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;
using WorldCupStats.data.Services;

namespace WordCupStats.WinForms
{
    public partial class PlayerRankingsForm : Form
    {
        private string championship;
        private readonly IStatisticsService statisticsService;

        public PlayerRankingsForm(string championship)
        {
            InitializeComponent();
            this.championship = championship;
            statisticsService = new StatisticsService(new FileDataRepository());

            LoadPlayersAsync();
        }

        private void btnLoadPlayers_Click(object sender, EventArgs e)
        {
            LoadPlayersAsync(cmbRankingType.SelectedItem.ToString());
        }

        private async Task LoadPlayersAsync(string type = "Cards")
        {
            try
            {
                IEnumerable<PlayerStatistics> players = Enumerable.Empty<PlayerStatistics>();

                switch (type)
                {
                    case "Goals":
                        players = await statisticsService.GetTopScorersAsync(10);
                        break;

                    case "Cards":
                        players = await statisticsService.GetMostYellowCardsAsync(10);
                        break;

                    case "Attendance":
                        // TODO: implement later
                        break;

                }

                if (players == null || !players.Any())
                {
                    MessageBox.Show(
                        "No players found for the selected championship.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                DisplayPlayers(players.ToList(), type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load players: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        private void DisplayPlayers(List<PlayerStatistics> players, string type)
        {

            pnlRankingYellow.AutoScroll = false; // enable scrolling if needed
            pnlRankingYellow.Controls.Clear(); // clear old ones
            pnlRankingYellow.SuspendLayout();

            int y = 5;
            int cardHeight = 65;
            int spacing = 5;
            int cardWidth = 330;

            foreach (var player in players)
            {
                var playerControl = new PlayerRankingControl();
                playerControl.SetPlayerImage(
                    PlayerImageService.GetPlayerImagePath(player.FifaCode, player.Name, player.ShirtNumber)
                );
                
                if (type == "Cards")
                    playerControl.SetPlayer(player, false);
                else if (type == "Goals")
                    playerControl.SetPlayer(player, true);

                playerControl.Size = new Size(cardWidth, cardHeight);
                playerControl.Location = new Point(5, y); // <-- place below previous one

                pnlRankingYellow.Controls.Add(playerControl);

                y += cardHeight + spacing; // update y position
            }


            pnlRankingYellow.AutoScroll = true;
            pnlRankingYellow.ResumeLayout();
        }

        
    }
}
