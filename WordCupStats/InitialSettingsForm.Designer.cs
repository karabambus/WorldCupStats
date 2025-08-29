namespace WordCupStats
{
    partial class InitialSettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rbMen = new RadioButton();
            rbWomen = new RadioButton();
            btnSave = new Button();
            gbChampionship = new GroupBox();
            gbLanguage = new GroupBox();
            rbEnglish = new RadioButton();
            rbHrvatski = new RadioButton();
            gbDataSource = new GroupBox();
            rbApi = new RadioButton();
            rbJsonFiles = new RadioButton();
            chkShowImages = new CheckBox();
            chkAutoSave = new CheckBox();
            chkUseCache = new CheckBox();
            btnCancel = new Button();
            btnReset = new Button();
            btnExport = new Button();
            btnImport = new Button();
            gbChampionship.SuspendLayout();
            gbLanguage.SuspendLayout();
            gbDataSource.SuspendLayout();
            SuspendLayout();
            // 
            // rbMen
            // 
            rbMen.AutoSize = true;
            rbMen.Checked = true;
            rbMen.Location = new Point(20, 30);
            rbMen.Name = "rbMen";
            rbMen.Size = new Size(116, 19);
            rbMen.TabIndex = 1;
            rbMen.TabStop = true;
            rbMen.Text = "Muško prvenstvo";
            rbMen.UseVisualStyleBackColor = true;
            // 
            // rbWomen
            // 
            rbWomen.AutoSize = true;
            rbWomen.Location = new Point(200, 30);
            rbWomen.Name = "rbWomen";
            rbWomen.Size = new Size(118, 19);
            rbWomen.TabIndex = 2;
            rbWomen.Text = "Žensko prvenstvo";
            rbWomen.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(140, 401);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 35);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // gbChampionship
            // 
            gbChampionship.Controls.Add(rbMen);
            gbChampionship.Controls.Add(rbWomen);
            gbChampionship.Location = new Point(12, 12);
            gbChampionship.Name = "gbChampionship";
            gbChampionship.Size = new Size(460, 80);
            gbChampionship.TabIndex = 3;
            gbChampionship.TabStop = false;
            gbChampionship.Text = "Prvenstvo";
            // 
            // gbLanguage
            // 
            gbLanguage.Controls.Add(rbEnglish);
            gbLanguage.Controls.Add(rbHrvatski);
            gbLanguage.Location = new Point(12, 98);
            gbLanguage.Name = "gbLanguage";
            gbLanguage.Size = new Size(460, 80);
            gbLanguage.TabIndex = 4;
            gbLanguage.TabStop = false;
            gbLanguage.Text = "Jezik";
            gbLanguage.Enter += groupBox1_Enter;
            // 
            // rbEnglish
            // 
            rbEnglish.AutoSize = true;
            rbEnglish.Checked = true;
            rbEnglish.Location = new Point(20, 30);
            rbEnglish.Name = "rbEnglish";
            rbEnglish.Size = new Size(63, 19);
            rbEnglish.TabIndex = 1;
            rbEnglish.TabStop = true;
            rbEnglish.Text = "English";
            rbEnglish.UseVisualStyleBackColor = true;
            // 
            // rbHrvatski
            // 
            rbHrvatski.AutoSize = true;
            rbHrvatski.Location = new Point(200, 30);
            rbHrvatski.Name = "rbHrvatski";
            rbHrvatski.Size = new Size(68, 19);
            rbHrvatski.TabIndex = 2;
            rbHrvatski.Text = "Hrvatski";
            rbHrvatski.UseVisualStyleBackColor = true;
            // 
            // gbDataSource
            // 
            gbDataSource.Controls.Add(rbApi);
            gbDataSource.Controls.Add(rbJsonFiles);
            gbDataSource.Location = new Point(12, 184);
            gbDataSource.Name = "gbDataSource";
            gbDataSource.Size = new Size(460, 80);
            gbDataSource.TabIndex = 5;
            gbDataSource.TabStop = false;
            gbDataSource.Text = "Izvor podataka";
            // 
            // rbApi
            // 
            rbApi.AutoSize = true;
            rbApi.Location = new Point(20, 30);
            rbApi.Name = "rbApi";
            rbApi.Size = new Size(116, 19);
            rbApi.TabIndex = 1;
            rbApi.Text = "Web API (Online)";
            rbApi.UseVisualStyleBackColor = true;
            // 
            // rbJsonFiles
            // 
            rbJsonFiles.AutoSize = true;
            rbJsonFiles.Checked = true;
            rbJsonFiles.Location = new Point(200, 30);
            rbJsonFiles.Name = "rbJsonFiles";
            rbJsonFiles.Size = new Size(157, 19);
            rbJsonFiles.TabIndex = 2;
            rbJsonFiles.TabStop = true;
            rbJsonFiles.Text = "Local JSON Files (Offline)";
            rbJsonFiles.UseVisualStyleBackColor = true;
            // 
            // chkShowImages
            // 
            chkShowImages.AutoSize = true;
            chkShowImages.Location = new Point(32, 285);
            chkShowImages.Name = "chkShowImages";
            chkShowImages.Size = new Size(131, 19);
            chkShowImages.TabIndex = 6;
            chkShowImages.Text = "Show Player Images";
            chkShowImages.UseVisualStyleBackColor = true;
            // 
            // chkAutoSave
            // 
            chkAutoSave.AutoSize = true;
            chkAutoSave.Location = new Point(32, 310);
            chkAutoSave.Name = "chkAutoSave";
            chkAutoSave.Size = new Size(125, 19);
            chkAutoSave.TabIndex = 7;
            chkAutoSave.Text = "Auto-save Settings";
            chkAutoSave.UseVisualStyleBackColor = true;
            // 
            // chkUseCache
            // 
            chkUseCache.AutoSize = true;
            chkUseCache.Location = new Point(32, 335);
            chkUseCache.Name = "chkUseCache";
            chkUseCache.Size = new Size(81, 19);
            chkUseCache.TabIndex = 8;
            chkUseCache.Text = "Use Cache";
            chkUseCache.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(246, 401);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(352, 401);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(100, 35);
            btnReset.TabIndex = 10;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(352, 358);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 35);
            btnExport.TabIndex = 11;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btnImport
            // 
            btnImport.Location = new Point(246, 358);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(100, 35);
            btnImport.TabIndex = 12;
            btnImport.Text = "Import";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // InitialSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 461);
            Controls.Add(btnImport);
            Controls.Add(btnExport);
            Controls.Add(btnReset);
            Controls.Add(btnCancel);
            Controls.Add(chkUseCache);
            Controls.Add(chkAutoSave);
            Controls.Add(chkShowImages);
            Controls.Add(gbDataSource);
            Controls.Add(gbLanguage);
            Controls.Add(gbChampionship);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InitialSettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Početne postavke";
            gbChampionship.ResumeLayout(false);
            gbChampionship.PerformLayout();
            gbLanguage.ResumeLayout(false);
            gbLanguage.PerformLayout();
            gbDataSource.ResumeLayout(false);
            gbDataSource.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RadioButton rbMen;
        private RadioButton rbWomen;
        private Button btnSave;
        private Panel panel2;
        private Label lblDataSource;
        private RadioButton rbAPI;
        private RadioButton rbLocal;
        private GroupBox gbChampionship;
        private GroupBox gbLanguage;
        private RadioButton rbEnglish;
        private RadioButton radioButton1;
        private RadioButton rbHrvatski;
        private GroupBox gbDataSource;
        private RadioButton rbApi;
        private RadioButton rbJsonFiles;
        private CheckBox chkShowImages;
        private CheckBox chkAutoSave;
        private CheckBox chkUseCache;
        private Button btnCancel;
        private Button btnReset;
        private Button btnExport;
        private Button btnImport;
    }
}
