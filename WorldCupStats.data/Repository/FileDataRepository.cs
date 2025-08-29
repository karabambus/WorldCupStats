using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using WorldCupStats.data.Exceptions;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;

namespace WorldCupStats.data.Repository  
{
    public class FileDataRepository : IDataRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _dataPath;
        private readonly bool _useApi;
        // API endpoints
        private const string API_BASE_URL = "https://worldcup-vua.nullbit.hr";
        private const string JSON_BASE_PATH = "Data";

        public FileDataRepository(bool useApi = true)
        {
            _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JSON_BASE_PATH);
            _httpClient = new HttpClient();
            _useApi = useApi;
        }

        // Alternative constructor that reads from SettingsManager
        public FileDataRepository() : this(SettingsManager.IsApiMode())
        {
        }

        public async Task<List<Teams>> GetTeamsAsync(string championship)
        {
            try
            {
                string jsonData;

                if (_useApi)
                {
                    //fetch from API
                    string url = $"{API_BASE_URL}/{championship}/teams";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"API Error: {response.StatusCode}");

                    jsonData = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //fetch from local file
                    string filePath = Path.Combine(_dataPath, championship, "teams.json");

                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException($"Cannot find teams.json at: {filePath}");
                    }

                    jsonData = await File.ReadAllTextAsync(filePath);


                }

                var teamResult = JsonConvert.DeserializeObject<List<Teams>>(jsonData, Converter.Settings);

                return teamResult ?? new List<Teams>();
            }
            catch(Exception ex)
            {
                throw new RepositoryException($"Error fetching teams for championship {championship}: {ex.Message}", ex);
            }
        }

        public async Task<List<Match>> GetMatchesAsync(string championship)
        {
            try
            {
                string jsonData;

                if (_useApi)
                {
                    //fetch from API
                    string url = $"{API_BASE_URL}/{championship}/matches";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"API Error: {response.StatusCode}");

                    jsonData = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //fetch from local file
                    string filePath = Path.Combine(_dataPath, championship, "matches.json");

                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException($"Cannot find teams.json at: {filePath}");
                    }

                    jsonData = await File.ReadAllTextAsync(filePath);
                    Console.WriteLine(jsonData);


                }

                var matches = JsonConvert.DeserializeObject<List<Match>>(jsonData, Converter.Settings);

                return matches ?? new List<Match>();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching matches for championship {championship}: {ex.Message}", ex);
            }
        }

        public async Task<List<Match>> GetMatchesByTeamAsync(string championship, string fifaCode)
        {
            try
            {
                string jsonData;

                if (_useApi)
                {
                    // API endpoint for team matches
                    var url = $"{API_BASE_URL}/{championship}/matches/country?fifa_code={fifaCode}";
                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"API Error: {response.StatusCode}");

                    jsonData = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // For JSON mode, filter matches locally
                    var allMatches = await GetMatchesAsync(championship);
                    var teamMatches = allMatches.Where(m =>
                        m.HomeTeam.Code == fifaCode || m.AwayTeam.Code == fifaCode
                    ).ToList();

                    return teamMatches;
                }

                var matches = JsonConvert.DeserializeObject<List<Match>>(jsonData, Converter.Settings);
                return matches ?? new List<Match>();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching matches for team {fifaCode} from API: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}