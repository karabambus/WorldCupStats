using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.data.Repository;
using WorldCupStats.data.Models;
using Newtonsoft.Json.Linq;
using WorldCupStats.data.Services;
using WorldCupStats.data.Helpers;
using Newtonsoft.Json;
using System.IO;
using Accessibility;
using WordCupStats.WinForms.Helpers;


namespace WordCupStats.WinForms
{
    public partial class MainForm : Form
    {
        private IDataRepository _repository;
        private IStatisticsService _statisticsService;
        private string _selectedTeamCode;
        private DragDropManager _dragDropManager;
        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            InitializeForm();
            // Initialize drag & drop manager
            _dragDropManager = new DragDropManager(this, pnlFavorites, pnlOthers);
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            // Set language based on settings
            LocalizationManager.SetLanguage(SettingsManager.GetLanguage());
            // Menu items
            datotekeToolStripMenuItem.Text = LocalizationManager.GetString("MenuFiles");
            postavkeToolStripMenuItem.Text = LocalizationManager.GetString("MenuSettings");
            izlazToolStripMenuItem.Text = LocalizationManager.GetString("MenuExit");
            pomoćToolStripMenuItem.Text = LocalizationManager.GetString("MenuHelp");
            oProgramuToolStripMenuItem.Text = LocalizationManager.GetString("MenuAbout");

            // Main form controls
            lblSelectTeam.Text = LocalizationManager.GetString("LabelSelectTeam");
            btnLoadPlayers.Text = LocalizationManager.GetString("ButtonLoadPlayers");
            grpFavorites.Text = LocalizationManager.GetString("GroupFavorites");
            grpOthers.Text = LocalizationManager.GetString("GroupOthers");
            btRanking.Text = LocalizationManager.GetString("ButtonRankings");
            btnRankingsAttendece.Text = LocalizationManager.GetString("ButtonRankingsAttendance");

            // Status bar
            lblStatus.Text = LocalizationManager.GetString("StatusReady");
        }

        private void InitializeServices()
        {
            try
            {
                //load settubgs
                var settings = SettingsManager.LoadSettings();

                //initilize repository with AP/File mode
                _repository = new FileDataRepository(settings.UseApi);

                //initilize service with repository and championship
                _statisticsService = new StatisticsService(_repository, settings.Championship);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing: {ex.Message}");
                // Fallback to defaults
                _repository = new FileDataRepository(false);
                _statisticsService = new StatisticsService(_repository, "men");
            }
        }

        private void InitializeForm()
        {
            LoadSettings();
            SetupEventHandlers();
            //SetupDragDrop();
            LoadTeamsAsync();
        }

        private void SetupEventHandlers()
        {
            // Enable button only when a team is selected
            cmbTeams.SelectedIndexChanged += (s, e) =>
            {
                btnLoadPlayers.Enabled = cmbTeams.SelectedIndex >= 0;
            };

            btnLoadPlayers.Click += btnLoadPlayers_Click;
        }

    

        public void SaveFavorites()
        {
            try
            {
                var favoritePlayerNumbers = new List<int>();

                // Get all favorite players from the favorites panel
                foreach (Control control in pnlFavorites.Controls)
                {
                    if (control is PlayerControl pc && pc.Player != null)
                    {
                        favoritePlayerNumbers.Add(pc.Player.ShirtNumber);
                    }
                }

                // Check limit
                if (favoritePlayerNumbers.Count > 3)
                {
                    MessageBox.Show("Maximum 3 favorite players allowed!", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save using SettingsManager 
                SettingsManager.SetFavoritePlayers(_selectedTeamCode, favoritePlayerNumbers);

                lblStatus.Text = $"Favoriti spremljeni: {favoritePlayerNumbers.Count} igrača";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving favorites: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupPlayerControlEvents(PlayerControl playerControl)
        {

            // Drag start
            playerControl.MouseDown += PlayerControl_MouseDown;
            // Init cleanup
            playerControl.ContextMenuStrip = null;

            var contextMenu = new ContextMenuStrip();

            //if (playerControl.IsFavorite)
            //{
            //    // if favorite, offer removal option
            //    var removeItem = new ToolStripMenuItem("Ukloni iz favorita");
            //    removeItem.Click += (s, e) => MovePlayerToOthers(playerControl);
            //    contextMenu.Items.Add(removeItem);
            //}
            //else
            //{
            //    // If not favorite, offer add option
            //    var addItem = new ToolStripMenuItem("Dodaj u favorite");
            //    addItem.Click += (s, e) => MovePlayerToFavorites(playerControl);
            //    contextMenu.Items.Add(addItem);
            //}

            contextMenu.Items.Add(new ToolStripSeparator());

            // add picture option
            var imageItem = new ToolStripMenuItem("Postavi sliku...");
            imageItem.Click += (s, e) => SetPlayerImage(playerControl);
            contextMenu.Items.Add(imageItem);

            // remove picture option
            var removeImageItem = new ToolStripMenuItem("Ukloni sliku");
            removeImageItem.Click += (s, e) => RemovePlayerImage(playerControl);
            contextMenu.Items.Add(removeImageItem);

            playerControl.ContextMenuStrip = contextMenu;

        }

        private void PlayerControl_MouseDown(object sender, MouseEventArgs e)
        {
            var playerControl = sender as PlayerControl;

            if (e.Button == MouseButtons.Left)
            {
                // Handle selection first
                _dragDropManager.HandlePlayerClick(playerControl, e);

                // Get selected players for dragging
                var selectedPlayers = GetSelectedPlayersForDrag(playerControl);

                if (selectedPlayers.Count > 1)
                {
                    // Multi-selection drag
                    playerControl.DoDragDrop(selectedPlayers, DragDropEffects.Move);
                }
                else
                {
                    // Single player drag (your existing logic)
                    playerControl.DoDragDrop(playerControl, DragDropEffects.Move);
                }
            }
        }

        private List<PlayerControl> GetSelectedPlayersForDrag(PlayerControl clickedPlayer)
        {
            var allPlayerControls = pnlFavorites.Controls.OfType<PlayerControl>()
                                   .Concat(pnlOthers.Controls.OfType<PlayerControl>())
                                   .ToList();

            var selectedPlayers = allPlayerControls.Where(p => p.IsSelected).ToList();

            // If clicked player is not selected, just drag that player
            if (!selectedPlayers.Contains(clickedPlayer))
            {
                return new List<PlayerControl> { clickedPlayer };
            }

            return selectedPlayers;
        }

        private void SetPlayerImage(PlayerControl playerControl)
        {
            using OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Odaberite sliku igrača",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the player info
                    var player = playerControl.Player;
                    string teamCode = _selectedTeamCode; // Assuming you have this from team selection

                    // Use image service to save the image
                    string savedPath = PlayerImageService.SavePlayerImage(
                        teamCode,
                        player.Name,  // Using name instead of shirt number
                        player.ShirtNumber,
                        dialog.FileName
                    );

                    // Update the PlayerControl with the new image
                    playerControl.SetPlayerImage(savedPath);

                    lblStatus.Text = $"Slika postavljena za {player.Name}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri postavljanju slike: {ex.Message}", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RemovePlayerImage(PlayerControl playerControl)
        {
            var player = playerControl.Player;
            string teamCode = _selectedTeamCode;

            // Set default image in control
            playerControl.SetDefaultImage();

            // Remove the physical file if it exists
            PlayerImageService.RemovePlayerImage(teamCode, player.Name, player.ShirtNumber);

            lblStatus.Text = $"Slika uklonjena za {player.Name}";
        }

        //context menu event handler
        private void MovePlayerToFavorites(PlayerControl playerControl)
        {
            // favorite check
            if (playerControl.IsFavorite)
            {
                return;
            }

            // remove from others panel if exists
            if (pnlOthers.Controls.Contains(playerControl))
            {
                pnlOthers.Controls.Remove(playerControl);
            }

            // set as favorite
            playerControl.IsFavorite = true;
            playerControl.BackColor = Color.LightYellow;

            // add to favorites panel
            pnlFavorites.Controls.Add(playerControl);

            // reset panels
            ResetPanelControls(pnlFavorites);
            ResetPanelControls(pnlOthers);

            // reset context menu and events
            SetupPlayerControlEvents(playerControl);

            // save to favorites
            SaveFavorites();

            // Status if successful
            if (playerControl.Player != null)
            {
                lblStatus.Text = $"Igrač {playerControl.Player.Name} dodan u favorite!";
            }
        }

        //sets cascading locations for player panels
        private void ResetPanelControls(Panel panel)
        {
            panel.AutoScroll = false;
            panel.SuspendLayout();

            int y = 5;
            foreach (PlayerControl control in panel.Controls)
            {
                control.Location = new Point(5, y);
                y += control.Height + 5;
            }

            panel.ResumeLayout();
            panel.AutoScroll = true;
        }



        private void LoadSettings()
        {
           SettingsManager.LoadSettings();
            this.Text = $"World Cup Stats - {SettingsManager.GetChampionship().ToUpper()}, " +
                $"{SettingsManager.GetFavoriteTeam}, " +
                $"{SettingsManager.GetLanguage}";
        }

        private async void LoadTeamsAsync()
        {
            try
            {
                lblStatus.Text = "Učitavanje timova...";
                cmbTeams.Enabled = false;

                // Get teams from repository
                var teams = await _statisticsService.GetAllTeamsAsync();

                // Sort teams by name
                var sortedTeams = teams.OrderBy(t => t.Country).ToList();

                // Bind directly to ComboBox
                cmbTeams.DataSource = sortedTeams;
                cmbTeams.DisplayMember = "Country";
                cmbTeams.ValueMember = "FifaCode";

                // Select favorite team if exists
                if (!string.IsNullOrEmpty(SettingsManager.GetFavoriteTeam()))
                {
                    var favorite = sortedTeams.FirstOrDefault(t => t.FifaCode == SettingsManager.GetFavoriteTeam());
                    if (favorite != null)
                    {
                        cmbTeams.SelectedItem = favorite;
                    }
                }

                cmbTeams.Enabled = true;
                lblStatus.Text = $"Učitano {sortedTeams.Count} timova";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Greška pri učitavanju";
            }
        }

        private async void btnLoadPlayers_Click(object sender, EventArgs e)
        {
            btnLoadPlayers.Enabled = false;   // disable during async work
            try
            {
                var fifaCode = cmbTeams.SelectedValue as string;

                // Fallback if SelectedValue is null 
                if (string.IsNullOrWhiteSpace(fifaCode) && cmbTeams.SelectedItem is Team teamFromItem)
                    fifaCode = teamFromItem.Code;

                if (string.IsNullOrWhiteSpace(fifaCode))
                    throw new InvalidOperationException("Nije odabran tim ili nema FIFA koda.");

                var selectedTeam = cmbTeams.SelectedItem as Team;

                // Save preferred team
                SettingsManager.SetFavoriteTeam(fifaCode.Trim());

                // Status
                lblStatus.Text = $"Učitavanje igrača za {(selectedTeam?.Country ?? fifaCode)}...";

                // Load players for the selected team code
                await LoadPlayersAsync(fifaCode.Trim());

                _selectedTeamCode = fifaCode.Trim(); // Store selected team code for image paths

                lblStatus.Text = $"Igrači učitani za {(selectedTeam?.Country ?? fifaCode)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Greška pri učitavanju igrača";
            }
            finally
            {
                btnLoadPlayers.Enabled = true; // re-enable after work
                btnRankingsAttendece.Enabled = true; // re-enable attendance rankings button
                btRanking.Enabled = true;
            }
        }


        private async Task LoadPlayersAsync(string fifaCode)
        {
            try
            {
                // get all matches for the team
                var matches = await _statisticsService.GetTeamMatchesAsync(fifaCode);

                if (matches == null || !matches.Any())
                {
                    MessageBox.Show("Nema utakmica za odabrani tim!", "Info");
                    return;
                }

                // first match
                var firstMatch = matches.First();

                // check if is home or away team
                bool isHome = firstMatch.HomeTeam?.Code == fifaCode;

                // get all players from the first match statistics
                List<StartingEleven> allPlayers = new List<StartingEleven>();
                TeamStatistics teamStats = null;

                if (isHome)
                {
                    teamStats = firstMatch.HomeTeamStatistics;
                }
                else
                {
                    teamStats = firstMatch.AwayTeamStatistics;
                }

                // check if teamStats is null
                if (teamStats == null)
                {
                    MessageBox.Show($"Nema statistike za tim {fifaCode} u prvoj utakmici!", "Upozorenje");
                    return;
                }

                // add starting eleven
                if (teamStats.StartingEleven != null && teamStats.StartingEleven.Any())
                {
                    allPlayers.AddRange(teamStats.StartingEleven);
                }

                // add substitutes
                if (teamStats.Substitutes != null && teamStats.Substitutes.Any())
                {
                    allPlayers.AddRange(teamStats.Substitutes);
                }

                // check for players
                if (!allPlayers.Any())
                {
                    MessageBox.Show($"Nema podataka o igračima za tim {fifaCode}!\n" +
                                   $"Utakmica: {firstMatch.HomeTeamCountry} vs {firstMatch.AwayTeamCountry}\n" +
                                   $"Tim je {(isHome ? "domaćin" : "gost")}",
                                   "Info");
                    return;
                }

                // load favorites from settings
                var favoritePlayers = SettingsManager.GetFavoritePlayers(fifaCode) ?? new List<int>();

                // show players in panels
                DisplayPlayers(allPlayers, favoritePlayers);

                lblStatus.Text = $"Učitano {allPlayers.Count} igrača ({teamStats.StartingEleven?.Count ?? 0} u prvoj postavi, {teamStats.Substitutes?.Count ?? 0} rezerve)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju igrača: {ex.Message}", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Greška pri učitavanju igrača";
            }
        }

        private void DisplayPlayers(List<StartingEleven> players, List<int> favoritePlayers)
        {
            // turnoff autoscroll so we can turn it on later after we add controls
            pnlFavorites.AutoScroll = false;
            pnlOthers.AutoScroll = false;

            // reset panels
            pnlFavorites.Controls.Clear();
            pnlOthers.Controls.Clear();

            // suspend layout before we add controls
            pnlFavorites.SuspendLayout();
            pnlOthers.SuspendLayout();

            //debug stuff
            int favoriteY = 5;
            int otherY = 5;
            int cardHeight = 65;
            int spacing = 5;
            int cardWidth = 330;

            foreach (var player in players.OrderBy(p => p.ShirtNumber))
            {
                var playerControl = new PlayerControl();

                //check if its favorite
                bool isFavorite = favoritePlayers.Contains(player.ShirtNumber);

                playerControl.SetPlayer(player, isFavorite);
                //set fixed size
                playerControl.Size = new Size(cardWidth, cardHeight);
                playerControl.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                playerControl.SetPlayerImage(
                    PlayerImageService.GetPlayerImagePath(_selectedTeamCode, player.Name, player.ShirtNumber)
                );

                if (isFavorite && pnlFavorites.Controls.Count < 3)
                {
                    playerControl.Location = new Point(5, favoriteY);
                    playerControl.IsFavorite = true;
                    pnlFavorites.Controls.Add(playerControl);
                    favoriteY += cardHeight + spacing;
                }
                else
                {
                    playerControl.Location = new Point(5, otherY);
                    playerControl.IsFavorite = false;
                    pnlOthers.Controls.Add(playerControl);
                    otherY += cardHeight + spacing;
                }

                //add event handlers for context menu
                SetupPlayerControlEvents(playerControl);
            }

            pnlFavorites.ResumeLayout();
            pnlOthers.ResumeLayout();

            pnlFavorites.AutoScroll = true;
            pnlOthers.AutoScroll = true;

            // Debug info
            lblStatus.Text = $"Učitano: {players.Count} igrača | Favoriti: {pnlFavorites.Controls.Count} | Ostali: {pnlOthers.Controls.Count}";
        }

        private void RefreshPanelLayout()
        {
            // Same approach as DisplayPlayers
            pnlFavorites.AutoScroll = false;
            pnlOthers.AutoScroll = false;

            pnlFavorites.SuspendLayout();
            pnlOthers.SuspendLayout();

            int favoriteY = 5;
            int otherY = 5;
            int cardHeight = 65;
            int spacing = 5;

            // Get all player controls and reposition favorites
            var favoriteControls = pnlFavorites.Controls.OfType<PlayerControl>()
                                               .OrderBy(p => p.Player.ShirtNumber)
                                               .ToList();

            foreach (var playerControl in favoriteControls)
            {
                playerControl.Location = new Point(5, favoriteY);
                favoriteY += cardHeight + spacing;
            }

            // Reposition others
            var otherControls = pnlOthers.Controls.OfType<PlayerControl>()
                                         .OrderBy(p => p.Player.ShirtNumber)
                                         .ToList();

            foreach (var playerControl in otherControls)
            {
                playerControl.Location = new Point(5, otherY);
                otherY += cardHeight + spacing;
            }

            pnlFavorites.ResumeLayout();
            pnlOthers.ResumeLayout();

            pnlFavorites.AutoScroll = true;
            pnlOthers.AutoScroll = true;
        }

        // Call this from DragDropManager
        public void OnDragDropCompleted()
        {
            RefreshPanelLayout();
        }

        private void postavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //otvori formu za postavke
            using (var settingsForm = new InitialSettingsForm(_selectedTeamCode))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    LoadSettings(); // Reload settings after confirmation
                    LoadTeamsAsync(); // Reload teams after settings change
                    UpdateLanguage(); // Update language if changed
                }
            }
        }

        private void btRanking_Click(object sender, EventArgs e)
        {
            //open ranking form
            using (var rankingsForm = new PlayerRankingsForm(SettingsManager.GetChampionship()))
            {
                rankingsForm.ShowDialog();
            }
        }

        private void btnRankingsAttendece_Click(object sender, EventArgs e)
        {
            using (var rankingsForm = new AttendanceRankingsForm(SettingsManager.GetChampionship()))
            {
                rankingsForm.ShowDialog();
            }
        }
    }
}
