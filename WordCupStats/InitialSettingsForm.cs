using WordCupStats.WinForms;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;
using WorldCupStats.data.Services;

namespace WordCupStats
{
    public partial class InitialSettingsForm : Form
    {
        private AppSettings currentSettings;
        private bool hasChanges = false;
        private string _selectedTeamCode = string.Empty;
        public InitialSettingsForm(string selectedTeamCode)
        {
            InitializeComponent();
            LoadCurrentSettings();
            SetupEventHandlers();
            UpdateLanguage();

            _selectedTeamCode = selectedTeamCode;

            this.AcceptButton = btnSave; // Enter key triggers save
            this.CancelButton = btnCancel; // Escape key triggers Cancel
            this.KeyPreview = true; 
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.PerformClick(); // Trigger save
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            };
            _selectedTeamCode = selectedTeamCode;
        }

        private void UpdateLanguage()
        {
            // Form title
            this.Text = LocalizationManager.GetString("SettingsFormTitle");

            // Group boxes
            gbChampionship.Text = LocalizationManager.GetString("GroupBoxChampionship");
            gbLanguage.Text = LocalizationManager.GetString("GroupBoxLanguage");
            gbDataSource.Text = LocalizationManager.GetString("GroupBoxDataSource");

            // Radio buttons
            rbMen.Text = LocalizationManager.GetString("RadioButtonMen");
            rbWomen.Text = LocalizationManager.GetString("RadioButtonWomen");
            rbEnglish.Text = LocalizationManager.GetString("RadioButtonEnglish");
            rbHrvatski.Text = LocalizationManager.GetString("RadioButtonCroatian");
            rbApi.Text = LocalizationManager.GetString("RadioButtonAPI");
            rbJsonFiles.Text = LocalizationManager.GetString("RadioButtonJsonFiles");

            // Check boxes
            chkShowImages.Text = LocalizationManager.GetString("CheckBoxShowImages");
            chkAutoSave.Text = LocalizationManager.GetString("CheckBoxAutoSave");
            chkUseCache.Text = LocalizationManager.GetString("CheckBoxUseCache");

            // Buttons
            btnSave.Text = LocalizationManager.GetString("ButtonSave");
            btnCancel.Text = LocalizationManager.GetString("ButtonCancel");
            btnReset.Text = LocalizationManager.GetString("ButtonReset");
            btnExport.Text = LocalizationManager.GetString("ButtonExport");
            btnImport.Text = LocalizationManager.GetString("ButtonImport");
        }

        private void LoadCurrentSettings()
        {
            currentSettings = SettingsManager.LoadSettings();

            // Championship
            if (currentSettings.Championship == "women")
                rbWomen.Checked = true;
            else
                rbMen.Checked = true;

            // Language
            if (currentSettings.Language == "hr")
                rbHrvatski.Checked = true;
            else
                rbEnglish.Checked = true;

            // Data Source
            if (currentSettings.UseApi)
                rbApi.Checked = true;
            else
                rbJsonFiles.Checked = true;

            // Checkboxes
            chkShowImages.Checked = currentSettings.ShowPlayerImages;
            chkAutoSave.Checked = currentSettings.AutoSave;
            chkUseCache.Checked = currentSettings.UseCache;
        }

        private void SetupEventHandlers()
        {
            // Track changes
            rbMen.CheckedChanged += (s, e) => hasChanges = true;
            rbWomen.CheckedChanged += (s, e) => hasChanges = true;
            rbEnglish.CheckedChanged += (s, e) => hasChanges = true;
            rbHrvatski.CheckedChanged += (s, e) => hasChanges = true;
            rbJsonFiles.CheckedChanged += (s, e) => hasChanges = true;
            rbApi.CheckedChanged += (s, e) => hasChanges = true;
            chkShowImages.CheckedChanged += (s, e) => hasChanges = true;
            chkAutoSave.CheckedChanged += (s, e) => hasChanges = true;
            chkUseCache.CheckedChanged += (s, e) => hasChanges = true;
        }

        private void SaveSettings()
        {
            try
            {
                // Ask for confirmation before saving
                var confirmResult = MessageBox.Show(
                    LocalizationManager.GetString("ConfirmSaveSettings") ??
                    "Are you sure you want to save these settings?",
                    LocalizationManager.GetString("ConfirmTitle") ?? "Confirm Changes",
                    MessageBoxButtons.YesNo
                );

                if (confirmResult != DialogResult.Yes)
                    return;

                // Update settings object
                currentSettings.Championship = rbMen.Checked ? "men" : "women";
                currentSettings.Language = rbEnglish.Checked ? "en" : "hr";
                currentSettings.UseApi = rbApi.Checked;
                currentSettings.ShowPlayerImages = chkShowImages.Checked;
                currentSettings.AutoSave = chkAutoSave.Checked;
                currentSettings.UseCache = chkUseCache.Checked;

                // Save to file
                SettingsManager.SaveSettings(currentSettings);
                hasChanges = false;

                // Show success message
                MessageBox.Show(
                    LocalizationManager.GetString("SettingsSaved") ?? "Settings saved successfully!",
                    LocalizationManager.GetString("Success") ?? "Success",
                    MessageBoxButtons.OK
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (hasChanges)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you want to save them?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    SaveSettings();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to reset all settings to defaults?\n" +
                "This action cannot be undone.",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                SettingsManager.ResetSettings();
                LoadCurrentSettings();
                hasChanges = false;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog
            {
                Title = "Import Settings",
                Filter = "JSON Files (*.json)|*.json"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                SettingsManager.ImportSettings(openDialog.FileName);

                LoadCurrentSettings();
                hasChanges = false;

            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Title = "Export Settings",
                Filter = "JSON Files (*.json)|*.json",
                FileName = $"worldcup_settings_{DateTime.Now:yyyyMMdd}.json"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (SettingsManager.ExportSettings(saveDialog.FileName))
                {
                    MessageBox.Show("Settings exported successfully", "Export Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to export settings", "Export Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        
    }
}
