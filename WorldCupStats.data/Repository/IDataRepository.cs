using System.Collections.Generic;
using System.Threading.Tasks;
using WorldCupStats.data.Models;

namespace WorldCupStats.data.Repository  
{
    public interface IDataRepository : IDisposable
    {
        Task<List<Teams>> GetTeamsAsync(string championship);
        Task<List<Match>> GetMatchesAsync(string championship);
        Task<List<Match>> GetMatchesByTeamAsync(string championship, string fifaCode);
    }
}