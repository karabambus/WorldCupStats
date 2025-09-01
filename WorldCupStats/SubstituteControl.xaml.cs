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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldCupStats.data.Models;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for SubstituteControl.xaml
    /// </summary>
    public partial class SubstituteControl : UserControl
    {
        private StartingEleven substitute;

        public SubstituteControl(StartingEleven player, bool isHomeTeam)
        {
            InitializeComponent();
            substitute = player;
            SetupSubstitute(player, isHomeTeam);
            AddInteractivity();
        }

        private void SetupSubstitute(StartingEleven player, bool isHomeTeam)
        {
            txtNumber.Text = player.ShirtNumber.ToString();
            txtName.Text = player.Name;
            txtPosition.Text = GetPositionAbbreviation(player.Position);

            // Set team colors for border
            if (isHomeTeam)
            {
                subBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(231, 76, 60));
            }
            else
            {
                subBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(52, 152, 219));
            }
        }

        private string GetPositionAbbreviation(Position position)
        {
            switch (position)
            {
                case Position.Goalie: return "GK";
                case Position.Defender: return "DEF";
                case Position.Midfield: return "MID";
                case Position.Forward: return "FWD";
                default: return "SUB";
            }
        }

        private void AddInteractivity()
        {
            // Hover effect
            this.MouseEnter += (s, e) =>
            {
                subBorder.Background = new LinearGradientBrush
                {
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop(Color.FromRgb(74, 90, 108), 0),
                        new GradientStop(Color.FromRgb(58, 74, 92), 1)
                    }
                };
            };

            this.MouseLeave += (s, e) =>
            {
                subBorder.Background = new LinearGradientBrush
                {
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop(Color.FromRgb(58, 74, 92), 0),
                        new GradientStop(Color.FromRgb(42, 58, 76), 1)
                    }
                };
            };

            // Click for details
            this.MouseLeftButtonUp += (s, e) =>
            {
                MessageBox.Show(
                    $"Substitute Player\n\n" +
                    $"Name: {substitute.Name}\n" +
                    $"Number: {substitute.ShirtNumber}\n" +
                    $"Position: {substitute.Position}",
                    "Player Details",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            };

            this.Cursor = Cursors.Hand;
        }
    }
}

