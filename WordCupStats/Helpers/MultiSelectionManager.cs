using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCupStats.WinForms.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    namespace WordCupStats.WinForms.Helpers
    {
        //to do fix
        public class MultiSelectionManager
        {
            private readonly List<PlayerControl> _selectedPlayers = new();
            private readonly Panel _favoritesPanel;
            private readonly Panel _othersPanel;
            private PlayerControl _lastClickedPlayer;

            public MultiSelectionManager(Panel favoritesPanel, Panel othersPanel)
            {
                _favoritesPanel = favoritesPanel;
                _othersPanel = othersPanel;
            }

            public void HandlePlayerClick(PlayerControl playerControl, MouseEventArgs e)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    // Ctrl+Click: Toggle selection
                    TogglePlayerSelection(playerControl);
                }
                else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift && _lastClickedPlayer != null)
                {
                    // Shift+Click: Range selection
                    SelectRange(_lastClickedPlayer, playerControl);
                }
                else
                {
                    // Normal click: Single selection
                    ClearSelection();
                    SelectPlayer(playerControl);
                }

                _lastClickedPlayer = playerControl;
            }

            private void TogglePlayerSelection(PlayerControl player)
            {
                if (_selectedPlayers.Contains(player))
                {
                    DeselectPlayer(player);
                }
                else
                {
                    SelectPlayer(player);
                }
            }

            private void SelectPlayer(PlayerControl player)
            {
                if (!_selectedPlayers.Contains(player))
                {
                    _selectedPlayers.Add(player);
                    player.SetSelected(true);
                }
            }

            private void DeselectPlayer(PlayerControl player)
            {
                _selectedPlayers.Remove(player);
                player.SetSelected(false);
            }

            private void SelectRange(PlayerControl start, PlayerControl end)
            {
                ClearSelection();

                var startPanel = start.Parent as Panel;
                var endPanel = end.Parent as Panel;

                // Only allow range selection within the same panel
                if (startPanel != endPanel)
                {
                    SelectPlayer(end);
                    return;
                }

                var controls = startPanel.Controls.OfType<PlayerControl>()
                                        .OrderBy(p => p.Location.Y)
                                        .ToList();

                int startIndex = controls.IndexOf(start);
                int endIndex = controls.IndexOf(end);

                if (startIndex == -1 || endIndex == -1) return;

                // Ensure correct order
                if (startIndex > endIndex)
                {
                    (startIndex, endIndex) = (endIndex, startIndex);
                }

                // Select range
                for (int i = startIndex; i <= endIndex; i++)
                {
                    SelectPlayer(controls[i]);
                }
            }

            public void ClearSelection()
            {
                foreach (var player in _selectedPlayers.ToList())
                {
                    player.SetSelected(false);
                }
                _selectedPlayers.Clear();
            }

            public List<PlayerControl> GetSelectedPlayers()
            {
                return _selectedPlayers.ToList();
            }

            public bool HasSelection => _selectedPlayers.Count > 0;

            public int SelectionCount => _selectedPlayers.Count;
        }
    }
}
