namespace WordCupStats.WinForms
{
    partial class AttendanceRankingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbMatches = new ComboBox();
            lblLocation = new Label();
            lblAttendence = new Label();
            lblHomeTeam = new Label();
            lblAwayTeam = new Label();
            tbLocation = new TextBox();
            tbAttendence = new TextBox();
            tbHomeTeam = new TextBox();
            tbAwayTeam = new TextBox();
            btnLoad = new Button();
            SuspendLayout();
            // 
            // cmbMatches
            // 
            cmbMatches.FormattingEnabled = true;
            cmbMatches.Location = new Point(30, 30);
            cmbMatches.Name = "cmbMatches";
            cmbMatches.Size = new Size(334, 23);
            cmbMatches.TabIndex = 0;
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(30, 105);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(56, 15);
            lblLocation.TabIndex = 1;
            lblLocation.Text = "Location:";
            // 
            // lblAttendence
            // 
            lblAttendence.AutoSize = true;
            lblAttendence.Location = new Point(30, 145);
            lblAttendence.Name = "lblAttendence";
            lblAttendence.Size = new Size(71, 15);
            lblAttendence.TabIndex = 2;
            lblAttendence.Text = "Attendence:";
            lblAttendence.Click += label2_Click;
            // 
            // lblHomeTeam
            // 
            lblHomeTeam.AutoSize = true;
            lblHomeTeam.Location = new Point(30, 185);
            lblHomeTeam.Name = "lblHomeTeam";
            lblHomeTeam.Size = new Size(74, 15);
            lblHomeTeam.TabIndex = 3;
            lblHomeTeam.Text = "Home Team:";
            // 
            // lblAwayTeam
            // 
            lblAwayTeam.AutoSize = true;
            lblAwayTeam.Location = new Point(30, 225);
            lblAwayTeam.Name = "lblAwayTeam";
            lblAwayTeam.Size = new Size(70, 15);
            lblAwayTeam.TabIndex = 4;
            lblAwayTeam.Text = "Away Team:";
            // 
            // tbLocation
            // 
            tbLocation.Location = new Point(130, 105);
            tbLocation.Name = "tbLocation";
            tbLocation.Size = new Size(150, 23);
            tbLocation.TabIndex = 5;
            // 
            // tbAttendence
            // 
            tbAttendence.Location = new Point(130, 145);
            tbAttendence.Name = "tbAttendence";
            tbAttendence.Size = new Size(150, 23);
            tbAttendence.TabIndex = 6;
            // 
            // tbHomeTeam
            // 
            tbHomeTeam.Location = new Point(130, 185);
            tbHomeTeam.Name = "tbHomeTeam";
            tbHomeTeam.Size = new Size(150, 23);
            tbHomeTeam.TabIndex = 7;
            // 
            // tbAwayTeam
            // 
            tbAwayTeam.Location = new Point(130, 225);
            tbAwayTeam.Name = "tbAwayTeam";
            tbAwayTeam.Size = new Size(150, 23);
            tbAwayTeam.TabIndex = 8;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(381, 30);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 9;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // AttendanceRankingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(btnLoad);
            Controls.Add(tbAwayTeam);
            Controls.Add(tbHomeTeam);
            Controls.Add(tbAttendence);
            Controls.Add(tbLocation);
            Controls.Add(lblAwayTeam);
            Controls.Add(lblHomeTeam);
            Controls.Add(lblAttendence);
            Controls.Add(lblLocation);
            Controls.Add(cmbMatches);
            Name = "AttendanceRankingsForm";
            Text = "Attendance Rankings Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbMatches;
        private Label lblLocation;
        private Label lblAttendence;
        private Label lblHomeTeam;
        private Label lblAwayTeam;
        private TextBox tbLocation;
        private TextBox tbAttendence;
        private TextBox tbHomeTeam;
        private TextBox tbAwayTeam;
        private Button btnLoad;
    }
}