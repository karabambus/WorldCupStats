namespace WordCupStats.WinForms
{
    partial class PlayerRankingControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerRankingControl));
            pbImage = new PictureBox();
            lblName = new Label();
            lblCountry = new Label();
            lblInfo = new Label();
            pbStar = new PictureBox();
            lblAttendence = new Label();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStar).BeginInit();
            SuspendLayout();
            // 
            // pbImage
            // 
            pbImage.Location = new Point(5, 5);
            pbImage.Name = "pbImage";
            pbImage.Size = new Size(50, 50);
            pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            pbImage.TabIndex = 0;
            pbImage.TabStop = false;
            pbImage.Click += pictureBox1_Click;
            // 
            // lblName
            // 
            lblName.Font = new Font("Microsoft Sans Serif", 10F);
            lblName.Location = new Point(65, 5);
            lblName.Name = "lblName";
            lblName.Size = new Size(200, 20);
            lblName.TabIndex = 1;
            lblName.Text = "Ime";
            // 
            // lblCountry
            // 
            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(65, 25);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(43, 15);
            lblCountry.TabIndex = 2;
            lblCountry.Text = "Zemlja";
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(65, 40);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(28, 15);
            lblInfo.TabIndex = 3;
            lblInfo.Text = "Info";
            // 
            // pbStar
            // 
            pbStar.Image = (Image)resources.GetObject("pbStar.Image");
            pbStar.Location = new Point(290, 5);
            pbStar.Name = "pbStar";
            pbStar.Size = new Size(25, 25);
            pbStar.TabIndex = 4;
            pbStar.TabStop = false;
            pbStar.Visible = false;
            // 
            // lblAttendence
            // 
            lblAttendence.AutoSize = true;
            lblAttendence.Location = new Point(185, 40);
            lblAttendence.Name = "lblAttendence";
            lblAttendence.Size = new Size(68, 15);
            lblAttendence.TabIndex = 5;
            lblAttendence.Text = "Attendence";
            // 
            // PlayerRankingControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblAttendence);
            Controls.Add(pbStar);
            Controls.Add(lblInfo);
            Controls.Add(lblCountry);
            Controls.Add(lblName);
            Controls.Add(pbImage);
            Name = "PlayerRankingControl";
            Size = new Size(320, 60);
            ((System.ComponentModel.ISupportInitialize)pbImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbImage;
        private Label lblName;
        private Label lblCountry;
        private Label lblInfo;
        private PictureBox pbStar;
        private Label lblAttendence;
    }
}
