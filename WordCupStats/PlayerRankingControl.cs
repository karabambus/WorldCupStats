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
using WorldCupStats.data.Services;

namespace WordCupStats.WinForms
{
    public partial class PlayerRankingControl : UserControl
    {
        public PlayerStatistics Player { get; set; }

        public PlayerRankingControl()
        {
            InitializeComponent();
        }

        public void SetPlayer(PlayerStatistics player, bool goals)
        {
            Player = player;

            lblName.Text = player.Name;
            lblCountry.Text = player.Country;
            lblInfo.Text = $"Goals/Yellow cards: {(goals ? player.Goals.ToString() : player.YellowCards.ToString())}";
            lblAttendence.Text = $"Apperances: {player.Appearances.ToString()}";

            // Postavi default sliku ako nema
            // pbImage.Image = Image.FromFile("default-player.png");
        }

        public void SetPlayerImage(String imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    // Dispose old image if exists
                    pbImage.Image?.Dispose();

                    // Load new image without locking the file
                    using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        pbImage.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    SetDefaultImage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                SetDefaultImage();
            }
        }

        public void SetDefaultImage()
        {
            // Postavi default sliku ako nema
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
