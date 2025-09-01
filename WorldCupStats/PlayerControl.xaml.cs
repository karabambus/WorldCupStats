using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;
using WorldCupStats.data.Services;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        private StartingEleven player;
        private string teamCode;

        public PlayerControl(StartingEleven player, string teamCode, bool isHomeTeam = true)
        {
            InitializeComponent();
            this.player = player;
            SetupPlayer(player, isHomeTeam);
            LoadPlayerImage();
            AddAnimations();
        }

        private void SetupPlayer(StartingEleven player, bool isHomeTeam)
        {
            // Set player info
            txtNumber.Text = player.ShirtNumber.ToString();
            txtPlayerName.Text = player.Name;

            // set team code
            this.teamCode = teamCode;

            // Show captain badge
            if (player.Captain)
            {
                txtCaptain.Visibility = Visibility.Visible;
            }

            // Set team colors
            if (isHomeTeam)
            {
                playerBorder.Background = new RadialGradientBrush(
                    Color.FromRgb(231, 76, 60),   // Red for home
                    Color.FromRgb(192, 57, 43));
                playerBorder.BorderBrush = Brushes.DarkRed;
            }
            else
            {
                playerBorder.Background = new RadialGradientBrush(
                    Color.FromRgb(52, 152, 219),  // Blue for away
                    Color.FromRgb(41, 128, 185));
                playerBorder.BorderBrush = Brushes.DarkBlue;
            }
        }

        private void LoadPlayerImage()
        {
            try
            {
                // Try to get player image using ImageHelper
                var imagePath = PlayerImageService.GetPlayerImagePath(teamCode, player.Name, player.ShirtNumber);
                

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    // Load custom player image
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                    bitmap.EndInit();

                    playerImageBrush.ImageSource = bitmap;
                }
            }
            catch (Exception ex)
            {
                // If error loading image, use default
                System.Diagnostics.Debug.WriteLine($"Error loading player image: {ex.Message}");
            }
        }

        private void AddAnimations()
        {
            // Mouse enter animation
            this.MouseEnter += (s, e) =>
            {
                var scaleTransform = new ScaleTransform(1, 1);
                playerBorder.RenderTransform = scaleTransform;
                playerBorder.RenderTransformOrigin = new Point(0.5, 0.5);

                var animation = new DoubleAnimation
                {
                    From = 1,
                    To = 1.15,
                    Duration = TimeSpan.FromSeconds(0.2)
                };

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);

                // Show larger name on hover
                txtPlayerName.FontSize = 10;
            };

            // Mouse leave animation
            this.MouseLeave += (s, e) =>
            {
                var scaleTransform = playerBorder.RenderTransform as ScaleTransform;
                if (scaleTransform != null)
                {
                    var animation = new DoubleAnimation
                    {
                        From = 1.15,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.2)
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
                }

                txtPlayerName.FontSize = 9;
            };

            // Click for details with image option
            this.MouseLeftButtonUp += (s, e) =>
            {
                ShowPlayerDetails();
            };

            this.Cursor = Cursors.Hand;
        }

        public static void Show(string playerName, string position, string number)
        {
            string message = $"Player: {playerName}\nPosition: {position}\nShirt Number: {number}";

            var msgBox = new Window
            {
                Title = "Player Details",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new System.Windows.Controls.TextBlock
                {
                    Text = message,
                    Margin = new Thickness(20),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                Opacity = 0
            };

            // 0.3s fade in
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));

            msgBox.Loaded += (s, e) => msgBox.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            msgBox.ShowDialog();
        }

        private void ShowPlayerDetails()
        {
            Show(player.Name, player.Position.ToString(), player.ShirtNumber.ToString());
            var result = MessageBox.Show(
                $"\nWould you like to change the player's image?",
                "Player Details",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                // Open file dialog to select new image
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                    Title = "Select Player Image"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Save new image using ImageHelper
                        var newImagePath = PlayerImageService.SavePlayerImage(
                            teamCode,
                            player.Name,
                            player.ShirtNumber,
                            openFileDialog.FileName);

                        // Reload the image
                        LoadPlayerImage();

                        MessageBox.Show("Player image updated successfully!", "Success",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating image: {ex.Message}", "Error",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
