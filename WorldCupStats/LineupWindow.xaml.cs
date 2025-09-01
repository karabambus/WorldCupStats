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
using WorldCupStats.WPF.Helpers;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for LineupWindow.xaml
    /// </summary>
    public partial class LineupWindow : Window
    {
        private Match match;
        private IStatisticsService statisticsService;
        private PlayerPositioningHelper positioningHelper;
        ///* y position offset */
        ////field values
        //int width = 700;
        //int height = 500;

        //// calculate width/8 and height/8
        //int width_8 = 700 / 8;

        ////calculate middle of 1/8
        //int middle_width_8 = (700 / 8) / 2;

        ////offset is middle width of width/8 + width/8
        //int x_offset = (700 / 8) + ((700 / 8) / 2); //*row

        ///* x position offset */
        //int y_center = 500 / 2;
        //int height_5 = (700 / 5) - 20;

        //int middle_height_5 = (700 / 5) / 2;
        //int y_offset = (700 / 5) + ((700 / 5) / 2); //*row

        //// m

        public LineupWindow(Match match)
        {
            InitializeComponent();
            InitializePositionHelper();
            this.match = match;
            statisticsService = new StatisticsService(new FileDataRepository());
            DisplayMatchInfo();
            DisplayLineups();

        }

        private void InitializePositionHelper()
        {
            // Use fixed canvas size for Viewbox approach
            if (soccerPitch.Width > 0 && soccerPitch.Height > 0)
            {
                // If canvas has explicit size
                positioningHelper = new PlayerPositioningHelper(soccerPitch.Width, soccerPitch.Height);
            }
            else if (soccerPitch.ActualWidth > 0 && soccerPitch.ActualHeight > 0)
            {
                // If canvas has actual size
                positioningHelper = new PlayerPositioningHelper(soccerPitch.ActualWidth, soccerPitch.ActualHeight);
            }
            else
            {
                // Fallback to default size
                positioningHelper = new PlayerPositioningHelper(800, 600);
            }
        }

        private void DisplayMatchInfo()
        {
            txtMatchTitle.Text = $"{match.HomeTeam.Country} vs {match.AwayTeam.Country}";
            txtMatchInfo.Text = $"{match.Location} | {match.Datetime?.ToString("dd MMM yyyy")}";

            txtHomeTeam.Text = match.HomeTeam.Country;
            txtAwayTeam.Text = match.AwayTeam.Country;

            // Display formations
            if (match.HomeTeamStatistics?.Tactics != null)
            {
                txtHomeFormation.Text = $"({match.HomeTeamStatistics.Tactics})";
            }
            if (match.AwayTeamStatistics?.Tactics != null)
            {
                txtAwayFormation.Text = $"({match.AwayTeamStatistics.Tactics})";
            }
        }

        private void DisplayLineups()
        {
            // Display home team (left side)
            if (match.HomeTeamStatistics?.StartingEleven != null)
            {
                PlacePlayersOnPitch(match.HomeTeamStatistics.StartingEleven, true);

                // Display home substitutes
                if (match.HomeTeamStatistics.Substitutes != null)
                {
                    DisplaySubstitutes(match.HomeTeamStatistics.Substitutes, true);
                }
            }

            // Display away team (right side)
            if (match.AwayTeamStatistics?.StartingEleven != null)
            {
                PlacePlayersOnPitch(match.AwayTeamStatistics.StartingEleven, false);

                // Display away substitutes
                if (match.AwayTeamStatistics.Substitutes != null)
                {
                    DisplaySubstitutes(match.AwayTeamStatistics.Substitutes, false);
                }
            }
        }

        private void DisplaySubstitutes(List<StartingEleven> substitutes, bool isHomeTeam)
        {
            var panel = isHomeTeam ? pnlHomeSubstitutes : pnlAwaySubstitutes;

            // Clear previous substitutes (if any)
            panel.Children.Clear();

            // Add each substitute
            foreach (var sub in substitutes.OrderBy(s => s.ShirtNumber))
            {
                var subControl = new SubstituteControl(sub, isHomeTeam);
                panel.Children.Add(subControl);
            }

            // If no substitutes, show message
            if (substitutes.Count == 0)
            {
                var noSubsText = new TextBlock
                {
                    Text = "No substitutes",
                    Foreground = Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                panel.Children.Add(noSubsText);
            }
        }

        private void PlacePlayersOnPitch(List<StartingEleven> players, bool isHomeTeam)
        {
            // Add null check
            if (positioningHelper == null)
            {
                MessageBox.Show("Position helper not initialized!");
                return;
            }

            // Get team code for image loading
            string teamCode = isHomeTeam ? match.HomeTeam.Code : match.AwayTeam.Code;

            // Group players by position
            var groupedByPosition = players.GroupBy(p => p.Position);

            foreach (var positionGroup in groupedByPosition)
            {
                var playersList = positionGroup.OrderBy(p => p.ShirtNumber).ToList();

                for (int i = 0; i < playersList.Count; i++)
                {
                    var player = playersList[i];

                    var position = positioningHelper.GetPlayerPosition(
                        player.Position,
                        i,
                        playersList.Count,
                        isHomeTeam
                    );

                    var playerControl = new PlayerControl(player, teamCode, isHomeTeam);
                    Canvas.SetLeft(playerControl, position.X);
                    Canvas.SetTop(playerControl, position.Y);
                    soccerPitch.Children.Add(playerControl);
                }
            }
        
        }


        //private Point CalculatePlayerPosition(Position position, bool isHomeTeam)
        //{
        //    double x = 0;
        //    double y = 0;

        //    // Base positions for home team (left side)
        //    switch (position)
        //    {
        //        case Position.Goalie:
        //            x = middle_width_8;
        //            y = y_center;
        //            break;

        //        case Position.Defender:
        //            x = middle_width_8 + width_8;
        //            y = GetDefenderYPosition();
        //            break;

        //        case Position.Midfield:
        //            x = middle_width_8 + width_8*2;
        //            y = GetMidfieldYPosition();
        //            break;

        //        case Position.Forward:
        //            x = middle_width_8 + width_8*3;
        //            y = GetForwardYPosition();
        //            break;
        //    }

        //    // Mirror for away team (right side)
        //    if (!isHomeTeam)
        //    {
        //        x = 700 - x - 60; // 60 is player control width
        //    }

        //    return new Point(x, y);
        //}

        //private int defenderCount = 0;
        //private int midfieldCount = 0;
        //private int forwardCount = 0;

        //private double GetDefenderYPosition()
        //{
        //    double[] positions = { middle_height_5 - 25, height_5 + middle_height_5 - 25, height_5*2 + middle_height_5 - 25, height_5*3 + middle_height_5 - 25 }; // 4 defender positions
        //    return positions[Math.Min(defenderCount++, positions.Length - 1)];
        //}

        //private double GetMidfieldYPosition()
        //{
        //    double[] positions = { middle_height_5 - 25, height_5 + middle_height_5 - 25, (height_5 * 2) + middle_height_5 - 25, height_5 * 3 + middle_height_5 - 25 }; // 4 midfield positions
        //    return positions[Math.Min(midfieldCount++, positions.Length - 1)];
        //}

        //private double GetForwardYPosition()
        //{
        //    double[] positions = { 175, 275 }; // 2 forward positions
        //    return positions[Math.Min(forwardCount++, positions.Length - 1)];
        //}

        //private void AnimatePlayerAppearance(PlayerControl player, int index)
        //{
        //    player.Opacity = 0;

        //    var fadeIn = new DoubleAnimation
        //    {
        //        From = 0,
        //        To = 1,
        //        Duration = TimeSpan.FromSeconds(0.5),
        //        BeginTime = TimeSpan.FromMilliseconds(index * 100) // Staggered appearance
        //    };

        //    player.BeginAnimation(OpacityProperty, fadeIn);
        //}

        //public void ResetCounters()
        //{
        //    defenderCount = 0;
        //    midfieldCount = 0;
        //    forwardCount = 0;
        //}
    }
}
