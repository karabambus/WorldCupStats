namespace WordCupStats.WinForms
{
    partial class PlayerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            pbImage = new PictureBox();
            lblName = new Label();
            lblNumber = new Label();
            lblPosition = new Label();
            pbStar = new PictureBox();
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
            // lblNumber
            // 
            lblNumber.AutoSize = true;
            lblNumber.Location = new Point(65, 25);
            lblNumber.Name = "lblNumber";
            lblNumber.Size = new Size(31, 15);
            lblNumber.TabIndex = 2;
            lblNumber.Text = "Broj:";
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(65, 40);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(50, 15);
            lblPosition.TabIndex = 3;
            lblPosition.Text = "Pozicija:";
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
            // PlayerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pbStar);
            Controls.Add(lblPosition);
            Controls.Add(lblNumber);
            Controls.Add(lblName);
            Controls.Add(pbImage);
            Name = "PlayerControl";
            Size = new Size(320, 60);
            ((System.ComponentModel.ISupportInitialize)pbImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbImage;
        private Label lblName;
        private Label lblNumber;
        private Label lblPosition;
        private PictureBox pbStar;
    }
}
