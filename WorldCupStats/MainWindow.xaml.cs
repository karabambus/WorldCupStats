using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;
using WorldCupStats.data.Services;
using WorldCupStats.WPF;

namespace WorldCupStats
{
    public partial class MainWindow : Window
    {
        private Match currentMatch; // Add this field
        private Teams selectedFavoriteTeam;
        private Teams selectedOpponentTeam;
        private List<Match> teamMatches;
        private IStatisticsService _statisticsService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
            LoadSettings();

            // Add keyboard handling
            this.KeyDown += MainWindow_KeyDown;
            this.Focusable = true;

        }
        // settings methods

        private async void LoadSettings()
        {
            try
            {
                // Initialize window mode ComboBox FIRST
                InitializeWindowModeComboBox();
                // Load current settings from setting manage
                var championship = SettingsManager.GetChampionship() ?? "men";
                var language = SettingsManager.GetLanguage() ?? "en";
                var windowMode = SettingsManager.GetWindowMode() ?? "windowed";

                // Set championship selection
                SetComboBoxSelection(cmbChampionship, championship);

                // Set language selection
                SetComboBoxSelection(cmbLanguage, language);

                // Set window mode
                SetComboBoxSelection(cmbWindowMode, windowMode);

                // Apply language immediately
                LocalizationManager.SetLanguage(language);
                UpdateUILanguage();

                // Apply window mode
                ApplyWindowMode(windowMode);

                //update statzs
                txtStatus.Text = LocalizationManager.GetString("StatusReady");
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error loading setting: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeWindowModeComboBox()
        {
            cmbWindowMode.Items.Clear();
            cmbWindowMode.Items.Add(new ComboBoxItem { Content = "Normal (800x600)", Tag = "normal" });
            cmbWindowMode.Items.Add(new ComboBoxItem { Content = "Large (1200x800)", Tag = "large" });
            cmbWindowMode.Items.Add(new ComboBoxItem { Content = "Extra Large (1600x1000)", Tag = "xlarge" });
        }

        private void ApplyWindowMode(string windowMode)
        {
            switch (windowMode?.ToLower())
            {
                case "large":
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.Width = 1200;
                    this.Height = 800;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
                case "xlarge":
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.Width = 1600;
                    this.Height = 1000;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
                default: // "normal"
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.Width = 800;
                    this.Height = 600;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
            }
        }

        private void SetComboBoxSelection(ComboBox comboBox, string tagValue)
        {
            // Handle window mode combo box initialization
            if (comboBox == cmbWindowMode && comboBox.Items.Count == 0)
            {
                cmbWindowMode.Items.Clear();
                cmbWindowMode.Items.Add(new ComboBoxItem { Content = LocalizationManager.GetString("WindowModeNormal"), Tag = "normal" });
                cmbWindowMode.Items.Add(new ComboBoxItem { Content = LocalizationManager.GetString("WindowModeMaximized"), Tag = "large" });
                cmbWindowMode.Items.Add(new ComboBoxItem { Content = LocalizationManager.GetString("WindowModeFullScreen"), Tag = "xlarge" });
            }

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Tag?.ToString() == tagValue)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveCurrentSettings()
        {
            try
            {
                // Save championship
                if (cmbChampionship.SelectedItem is ComboBoxItem champItem)
                {
                    SettingsManager.SetChampionship(champItem.Tag.ToString());
                }

                // Save language (already saved in SelectionChanged)

                // Save window mode
                if (cmbWindowMode.SelectedItem is ComboBoxItem windowItem)
                {
                    SettingsManager.SetWindowMode(windowItem.Tag.ToString());
                }

                // Save selected team
                if (cmbTeams.SelectedValue != null)
                {
                    SettingsManager.SetFavoriteTeam(cmbTeams.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                // Don't show error on closing, just log it
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
        // Save settings when window closes
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveCurrentSettings();
            base.OnClosing(e);
        }

        private void InitializeServices()
        {
            _statisticsService = new StatisticsService(new FileDataRepository(SettingsManager.IsApiMode()));
        }

        /* Methods for loading stats */
        private async Task LoadTeamsAsync()
        {
            try
            {
                btnLoadTeams.IsEnabled = false;
                txtStatus.Text = LocalizationManager.GetString("StatusLoadingTeams");

                var result = await _statisticsService.GetAllTeamsAsync();
                var teams = result.OrderBy(t => t.Country).ToList();

                PopulateTeamsComboBox(teams);
                SetFavoriteTeamSelection(teams);

                txtStatus.Text = $"{LocalizationManager.GetString("StatusTeamsLoaded")} ({teams.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = LocalizationManager.GetString("StatusError");
                HideFavoriteTeam();
            }
            finally
            {
                btnLoadTeams.IsEnabled = true;
            }
        }

        private async void LoadOpponentsAsync(string teamCode)
        {
            try
            {
                txtStatus.Text = "Loading opponents...";

                // Get all matches for the selected team
                teamMatches = await _statisticsService.GetTeamMatchesAsync(teamCode);

                // Get unique opponents with proper formatting
                var opponents = new List<Teams>();
                var processedCodes = new HashSet<string>();

                foreach (var match in teamMatches)
                {
                    // Determine opponent
                    var opponentTeam = match.HomeTeam.Code == teamCode
                        ? match.AwayTeam
                        : match.HomeTeam;

                    if (!processedCodes.Contains(opponentTeam.Code))
                    {
                        processedCodes.Add(opponentTeam.Code);
                        // Create Teams object for display
                        opponents.Add(new Teams
                        {
                            Country = opponentTeam.Country,
                            FifaCode = opponentTeam.Code,
                            // Format for display: "COUNTRY (FIFA_CODE)"
                        });
                    }
                }

                // Custom class for ComboBox display
                var displayOpponents = opponents.Select(o => new OpponentDisplay
                {
                    Team = o,
                    DisplayText = $"{o.Country} ({o.FifaCode})"
                }).ToList();

                cmbOpponents.ItemsSource = displayOpponents;
                cmbOpponents.DisplayMemberPath = "DisplayText";

                txtStatus.Text = $"Loaded {opponents.Count} opponents";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading opponents: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Error loading opponents";
            }
        }


        //
        // Display & hide result 
        //
        private void DisplayMatchResult()
        {
            pnlFavoriteTeam.Visibility = Visibility.Collapsed;
            if (selectedFavoriteTeam == null || selectedOpponentTeam == null) return;

            // Find the match between these two teams
            currentMatch = teamMatches.FirstOrDefault(m =>
                (m.HomeTeam.Code == selectedFavoriteTeam.FifaCode &&
                 m.AwayTeam.Code == selectedOpponentTeam.FifaCode) ||
                (m.AwayTeam.Code == selectedFavoriteTeam.FifaCode &&
                 m.HomeTeam.Code == selectedOpponentTeam.FifaCode));

            if (currentMatch != null)
            {
                pnlMatchResult.Visibility = Visibility.Visible;

                // Display as "HOME_GOALS : AWAY_GOALS"
                txtMatchScore.Text = $"{currentMatch.HomeTeam.Goals} : {currentMatch.AwayTeam.Goals}";

                // Update button content with team names
                btnFavoriteTeamInfo.Content = $"{selectedFavoriteTeam.Country} Info";
                btnOpponentTeamInfo.Content = $"{selectedOpponentTeam.Country} Info";

                // Show lineup button if statistics are available
                if (currentMatch.HomeTeamStatistics?.StartingEleven != null &&
                    currentMatch.AwayTeamStatistics?.StartingEleven != null)
                {
                    btnViewLineup.Visibility = Visibility.Visible;
                }
            }
        }
        private void DisplayFavoriteTeam(Teams team)
        {


            txtFavoriteTeamName.Text = team.Country;
            txtFavoriteTeamCode.Text = $"FIFA Code: {team.FifaCode}";

            // Show opponent selection and favorite team display
            grpOpponentSelection.Visibility = Visibility.Visible;
            pnlFavoriteTeam.Visibility = Visibility.Visible;
            txtNoTeamSelected.Visibility = Visibility.Collapsed;

            LoadOpponentsAsync(team.FifaCode);

        }
        private void HideFavoriteTeam()
        {
            // Hide favorite team panel, show no selection message
            pnlFavoriteTeam.Visibility = Visibility.Collapsed;
            txtNoTeamSelected.Visibility = Visibility.Visible;
        }

        private void HideResult()
        {
            if (pnlMatchResult.Visibility == Visibility.Visible)
            {
                pnlMatchResult.Visibility = Visibility.Collapsed;
            }
        }
        //
        // Combo box helpers
        //
        private void PopulateTeamsComboBox(List<Teams> teams)
        {
            cmbTeams.ItemsSource = teams;
            cmbTeams.DisplayMemberPath = "Country";
            cmbTeams.SelectedValuePath = "FifaCode";
        }
        private class OpponentDisplay
        {
            public Teams Team { get; set; }
            public string DisplayText { get; set; }
        }

        private void SetFavoriteTeamSelection(List<Teams> teams)
        {
            if (!string.IsNullOrEmpty(SettingsManager.GetFavoriteTeam()))
            {
                var favorite = teams.FirstOrDefault(t => t.FifaCode == SettingsManager.GetFavoriteTeam());
                if (favorite != null)
                {
                    cmbTeams.SelectedItem = favorite;
                    selectedFavoriteTeam = favorite;
                    return;
                }
            }

            HideFavoriteTeam();
        }
        //
        // Language helpers
        //
        private void UpdateUILanguage()
        {
            // Update window title
            this.Title = LocalizationManager.GetString("MainWindowTitle");

            // Update GroupBox headers
            grpSettings.Header = LocalizationManager.GetString("SettingsTitle");
            grpTeamSelection.Header = LocalizationManager.GetString("TeamSelectionTitle");

            // Update buttons
            btnLoadTeams.Content = LocalizationManager.GetString("ButtonLoadTeams");
            //btnSelectOpponent.Content = LocalizationManager.GetString("ButtonSelectOpponent");
            //btnTeamInfo.Content = LocalizationManager.GetString("ButtonTeamInfo");

            // Update favorite team section
            txtFavoriteTeamTitle.Text = LocalizationManager.GetString("FavoriteTeamTitle");
            txtNoTeamSelected.Text = LocalizationManager.GetString("SelectTeamMessage");

            lblChampionship.Text = LocalizationManager.GetString("ChampionshipLabel");
            lblLanguage.Text = LocalizationManager.GetString("LanguageLabel");
            lblWindowMode.Text = LocalizationManager.GetString("WindowModeLabel");
            lblSelectTeam.Text = LocalizationManager.GetString("LabelSelectTeam");

            grpOpponentSelection.Header = LocalizationManager.GetString("OpponentSelection");
            lblSelectOpponent.Text = LocalizationManager.GetString("SelectOpponent");
            txtFavoriteTeamTitle.Text = LocalizationManager.GetString("YourFavoriteTeam");
            btnOpponentTeamInfo.Content = LocalizationManager.GetString("SelectOpponentButton");
            btnFavoriteTeamInfo.Content = LocalizationManager.GetString("TeamInfo");


            // Update ComboBox items
            UpdateComboBoxItems();

            // Update status
            txtStatus.Text = LocalizationManager.GetString("StatusReady");
        }


        private void UpdateComboBoxItems()
        {
            // Championship ComboBox
            if (cmbChampionship.Items.Count >= 2)
            {
                ((ComboBoxItem)cmbChampionship.Items[0]).Content = LocalizationManager.GetString("RadioButtonMen");
                ((ComboBoxItem)cmbChampionship.Items[1]).Content = LocalizationManager.GetString("RadioButtonWomen");
            }

            // Language ComboBox 

            // Window Mode ComboBox - update with localized strings
            if (cmbWindowMode.Items.Count >= 3)
            {
                ((ComboBoxItem)cmbWindowMode.Items[0]).Content = LocalizationManager.GetString("WindowModeNormal");
                ((ComboBoxItem)cmbWindowMode.Items[1]).Content = LocalizationManager.GetString("WindowModeMaximized");
                ((ComboBoxItem)cmbWindowMode.Items[2]).Content = LocalizationManager.GetString("WindowModeFullScreen");
            }
        }
        //
        // Event handlers (click) for buttons
        //
        private async void btnLoadTeams_Click(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
        }

        private void BtnViewLineup_Click(object sender, RoutedEventArgs e)
        {
            if (currentMatch != null)
            {
                var lineupWindow = new LineupWindow(currentMatch);
                lineupWindow.Owner = this;
                lineupWindow.ShowDialog();
            }
        }

        private void BtnFavoriteTeamInfo_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new TeamInfoWindow(selectedFavoriteTeam);
            infoWindow.Owner = this;
            infoWindow.ShowDialog();
        }

        private void BtnOpponentTeamInfo_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new TeamInfoWindow(selectedOpponentTeam);
            infoWindow.Owner = this;
            infoWindow.ShowDialog();
        }

        //
        // Event handlers (selection change) for ComboBoxes        
        //
        private void cmbOpponent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOpponents.SelectedItem is OpponentDisplay selected)
            {
                selectedOpponentTeam = selected.Team;
                selectedFavoriteTeam = cmbTeams.SelectedItem as Teams;
                DisplayMatchResult();
            }
        }

        private void cmbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTeams.SelectedItem is Teams selectedTeam) // Change to Teams type
            {
                SettingsManager.SetFavoriteTeam(selectedTeam.FifaCode); // Use FifaCode
                DisplayFavoriteTeam(selectedTeam);
                txtStatus.Text = $"{LocalizationManager.GetString("StatusFavoriteTeamSet")}: {selectedTeam.Country}";
            }

            HideResult();
        }

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLanguage.SelectedItem is ComboBoxItem selectedItem && e.AddedItems.Count > 0)
            {
                if (ConfirmSettingsChange("language"))
                {
                    string language = selectedItem.Tag.ToString();
                    LocalizationManager.SetLanguage(language);
                    SettingsManager.SetLanguage(language);
                    UpdateUILanguage();
                    txtStatus.Text = LocalizationManager.GetString("StatusLanguageChanged");
                }
                else
                {
                    // Revert selection without triggering event
                    cmbLanguage.SelectionChanged -= cmbLanguage_SelectionChanged;
                    var currentLanguage = SettingsManager.GetLanguage();
                    SetComboBoxSelection(cmbLanguage, currentLanguage);
                    cmbLanguage.SelectionChanged += cmbLanguage_SelectionChanged;
                }
            }
        }

        private void cmbWindowMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbWindowMode.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null && e.AddedItems.Count > 0)
            {
                if (ConfirmSettingsChange("window mode"))
                {
                    string windowMode = selectedItem.Tag.ToString();
                    SettingsManager.SetWindowMode(windowMode);
                    ApplyWindowMode(windowMode);
                    txtStatus.Text = $"Window mode changed to: {selectedItem.Content}";
                }
                else
                {
                    // Revert selection
                    cmbWindowMode.SelectionChanged -= cmbWindowMode_SelectionChanged;
                    var currentMode = SettingsManager.GetWindowMode();
                    SetComboBoxSelection(cmbWindowMode, currentMode);
                    cmbWindowMode.SelectionChanged += cmbWindowMode_SelectionChanged;
                }
            }
        }

        private void cmbChampionship_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChampionship.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null && e.AddedItems.Count > 0)
            {
                if (ConfirmSettingsChange("championship"))
                {
                    string championship = selectedItem.Tag.ToString();
                    SettingsManager.SetChampionship(championship);

                    // Recreate service
                    _statisticsService = new StatisticsService(new FileDataRepository(SettingsManager.IsApiMode()));

                    // Clear teams when championship changes
                    cmbTeams.ItemsSource = null;
                    HideFavoriteTeam();
                }
                else
                {
                    // Revert selection
                    cmbChampionship.SelectionChanged -= cmbChampionship_SelectionChanged;
                    var currentChampionship = SettingsManager.GetChampionship();
                    SetComboBoxSelection(cmbChampionship, currentChampionship);
                    cmbChampionship.SelectionChanged += cmbChampionship_SelectionChanged;
                }
            }
        }

        private bool ConfirmSettingsChange(string settingType)
        {
            var result = MessageBox.Show(
                LocalizationManager.GetString("ConfirmSaveSettings") ??
                "Are you sure you want to save these settings?",
                LocalizationManager.GetString("ConfirmTitle") ?? "Confirm Changes",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No // Default to No
            );

            return result == MessageBoxResult.Yes;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // Handle ESC key for canceling operations or closing
                if (ConfirmExit())
                {
                    this.Close();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                // Handle Enter key - could be used for confirming current selection
                e.Handled = true;
            }
        }

        private bool ConfirmExit()
        {
            var result = MessageBox.Show(
                LocalizationManager.GetString("ConfirmExit") ??
                "Are you sure you want to exit the application?",
                LocalizationManager.GetString("ConfirmExitTitle") ?? "Exit Application",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No
            );

            return result == MessageBoxResult.Yes;
        }
    }
}