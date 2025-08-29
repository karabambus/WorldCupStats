using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.data.Models;

namespace WorldCupStats.data.Services
{
    public interface IStatisticsService
    {
        // Team and Player methods
        Task<List<Teams>> GetAllTeamsAsync();
        Task<TeamStatisticsCalculated> GetTeamStatisticsCalculatedAsync(string fifaCode);
        Task<List<Match>> GetTeamMatchesAsync(string fifaCode);
        Task<List<PlayerStatistics>> GetPlayerStatisticsByTeamAsync(string fifaCode);

        // Rankings
        Task<List<PlayerStatistics>> GetTopScorersAsync(int count = 10);
        Task<List<PlayerStatistics>> GetMostYellowCardsAsync(int count = 10);
        Task<List<Match>> GetMatchesByAttendanceAsync(int count = 10);
    }
}
