using System;
using System.Windows.Forms;
using WordCupStats;
using WordCupStats.WinForms;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Repository;

namespace WorldCupStats.WinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var repository = new FileDataRepository();
            var settings = SettingsManager.GetChampionship();

            // Provjeri postavke
            if (string.IsNullOrEmpty(settings))
            {
                using (var settingsForm = new InitialSettingsForm(SettingsManager.GetFavoriteTeam()))
                {
                    if (settingsForm.ShowDialog() != DialogResult.OK)
                    {
                        return; // Izaði ako odustane
                    }
                }
            }

            Application.Run(new MainForm());
        }
    }
}