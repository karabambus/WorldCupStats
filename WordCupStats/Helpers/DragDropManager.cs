using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCupStats.WinForms.Helpers.WordCupStats.WinForms.Helpers;
using WorldCupStats.data.Helpers;

namespace WordCupStats.WinForms.Helpers
{
    //multi selection not working
    public class DragDropManager
    {
        private readonly MainForm _mainForm;
        private readonly Panel _favoritesPanel;
        private readonly Panel _othersPanel;
        private const int MAX_FAVORITES = 3;
        private readonly MultiSelectionManager _selectionManager;

        public DragDropManager(MainForm mainForm, Panel favoritesPanel, Panel othersPanel)
        {
            _mainForm = mainForm;
            _favoritesPanel = favoritesPanel;
            _othersPanel = othersPanel;
            _selectionManager = new MultiSelectionManager(favoritesPanel, othersPanel);

            SetupDragDropEvents();
        }

        // method to handle player clicks
        public void HandlePlayerClick(PlayerControl playerControl, MouseEventArgs e)
        {
            _selectionManager.HandlePlayerClick(playerControl, e);
        }


        private void SetupDragDropEvents()
        {
            // Setup for Favorites panel
            _favoritesPanel.AllowDrop = true;
            _favoritesPanel.DragEnter += Panel_DragEnter;
            _favoritesPanel.DragDrop += FavoritesPanel_DragDrop;
            _favoritesPanel.DragLeave += Panel_DragLeave;

            // Setup for Others panel
            _othersPanel.AllowDrop = true;
            _othersPanel.DragEnter += Panel_DragEnter;
            _othersPanel.DragDrop += OthersPanel_DragDrop;
            _othersPanel.DragLeave += Panel_DragLeave;
        }

        private void Panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PlayerControl)) || e.Data.GetDataPresent(typeof(List<PlayerControl>)))
            {
                Panel panel = sender as Panel;
                panel.BackColor = Color.LightBlue; // Highlight on drag over
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Panel_DragLeave(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.White; // Reset color
        }

        private void FavoritesPanel_DragDrop(object sender, DragEventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.White;

            List<PlayerControl> draggedPlayers = new List<PlayerControl>();

            // Handle both single and multiple player drops
            if (e.Data.GetDataPresent(typeof(List<PlayerControl>)))
            {
                draggedPlayers = (List<PlayerControl>)e.Data.GetData(typeof(List<PlayerControl>));
            }
            else if (e.Data.GetDataPresent(typeof(PlayerControl)))
            {
                draggedPlayers.Add((PlayerControl)e.Data.GetData(typeof(PlayerControl)));
            }
            else return;

            // Filter players that can actually move to favorites
            var playersToMove = draggedPlayers.Where(p => !p.IsFavorite).ToList();

            // Check if we can add all selected players
            if (GetFavoritesCount() + playersToMove.Count > MAX_FAVORITES)
            {
                ShowMessage("MessageMaxFavoritesReached");
                return;
            }

            // Move all valid players
            foreach (var player in playersToMove)
            {
                MovePlayerToFavorites(player);
            }

            _selectionManager.ClearSelection();
        }


        private void OthersPanel_DragDrop(object sender, DragEventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.White;

            List<PlayerControl> draggedPlayers = new List<PlayerControl>();

            // Handle both single and multiple player drops
            if (e.Data.GetDataPresent(typeof(List<PlayerControl>)))
            {
                draggedPlayers = (List<PlayerControl>)e.Data.GetData(typeof(List<PlayerControl>));
            }
            else if (e.Data.GetDataPresent(typeof(PlayerControl)))
            {
                draggedPlayers.Add((PlayerControl)e.Data.GetData(typeof(PlayerControl)));
            }
            else return;

            // Filter players that can actually move to others
            var playersToMove = draggedPlayers.Where(p => p.IsFavorite).ToList();

            // Move all valid players
            foreach (var player in playersToMove)
            {
                MovePlayerToOthers(player);
            }

            _selectionManager.ClearSelection();
        }

        private void MovePlayerToFavorites(PlayerControl player)
        {
            player.Parent?.Controls.Remove(player);
            _favoritesPanel.Controls.Add(player);
            player.IsFavorite = true;

            _mainForm.SaveFavorites(); // Delegate to MainForm
            _mainForm.OnDragDropCompleted(); 
        }

        private void MovePlayerToOthers(PlayerControl player)
        {
            player.Parent?.Controls.Remove(player);
            _othersPanel.Controls.Add(player);
            player.IsFavorite = false;

            _favoritesPanel.Controls.Remove(player); // Delegate to PlayerControl
            _mainForm.SaveFavorites(); // Delegate to MainForm
            _mainForm.OnDragDropCompleted(); 
        }

        private int GetFavoritesCount()
        {
            return _favoritesPanel.Controls.OfType<PlayerControl>().Count();
        }

        private void ShowMessage(string messageKey)
        {
            string message = LocalizationManager.GetString(messageKey);
            MessageBox.Show(message, LocalizationManager.GetString("MessageInfoTitle"),
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
