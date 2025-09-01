using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorldCupStats.data.Models;

namespace WorldCupStats.WPF.Helpers
{
    class PlayerPositioningHelper
    {
        private double canvasWidth;
        private double canvasHeight;
        private const double PLAYER_WIDTH = 60;
        private const double PLAYER_HEIGHT = 70;


        public PlayerPositioningHelper(double canvasWidth, double canvasHeight)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
        }

        public Point GetPlayerPosition(Position position, int playerIndex, int totalPlayersInPosition, bool isHomeTeam)
        {
            // Get X coordinate (which column)
            double x = GetColumnPosition(position, isHomeTeam);

            // Get Y coordinate (distribute players vertically)
            double y = GetRowPosition(playerIndex, totalPlayersInPosition);

            return new Point(x, y);
        }

        private double GetColumnPosition(Position position, bool isHomeTeam)
        {
            double pitchPadding = 40; // Distance from pitch edge
            double columnSpacing = (canvasWidth - 2 * pitchPadding - PLAYER_WIDTH) / 7; // 8 columns total

            double baseX = 0;

            // Home team positions (left to right)
            switch (position)
            {
                case Position.Goalie:
                    baseX = pitchPadding; // Column 0
                    break;
                case Position.Defender:
                    baseX = pitchPadding + columnSpacing * 1; // Column 1.5
                    break;
                case Position.Midfield:
                    baseX = pitchPadding + columnSpacing * 2; // Column 3
                    break;
                case Position.Forward:
                    baseX = pitchPadding + columnSpacing * 3; // Column 4.5
                    break;
            }

            // Mirror for away team
            if (!isHomeTeam)
            {
                baseX = canvasWidth - baseX - PLAYER_WIDTH;
            }

            return baseX;
        }
    

        private double GetRowPosition(int playerIndex, int totalPlayers)
        {
            double topPadding = 40;
            double bottomPadding = 40;
            double availableHeight = canvasHeight - topPadding - bottomPadding - PLAYER_HEIGHT;

            if (totalPlayers == 1)
            {
                // Center single player
                return canvasHeight / 2 - PLAYER_HEIGHT / 2;
            }

            // Distribute multiple players evenly
            double spacing = availableHeight / (totalPlayers - 1);
            return topPadding + (spacing * playerIndex);
        }
    }
}
