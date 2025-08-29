namespace WordCupStats.WinForms
{
    partial class PlayerRankingsForm
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
            pnlRankingYellow = new Panel();
            cmbRankingType = new ComboBox();
            gbPlayerRankings = new GroupBox();
            btnLoadPlayers = new Button();
            gbPlayerRankings.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRankingYellow
            // 
            pnlRankingYellow.BackColor = SystemColors.ControlLightLight;
            pnlRankingYellow.ForeColor = SystemColors.ControlText;
            pnlRankingYellow.Location = new Point(20, 22);
            pnlRankingYellow.Name = "pnlRankingYellow";
            pnlRankingYellow.Size = new Size(359, 399);
            pnlRankingYellow.TabIndex = 0;
            // 
            // cmbRankingType
            // 
            cmbRankingType.FormattingEnabled = true;
            cmbRankingType.Items.AddRange(new object[] { "Goals", "Cards" });
            cmbRankingType.Location = new Point(251, 12);
            cmbRankingType.Name = "cmbRankingType";
            cmbRankingType.Size = new Size(250, 23);
            cmbRankingType.TabIndex = 1;
            // 
            // gbPlayerRankings
            // 
            gbPlayerRankings.Controls.Add(pnlRankingYellow);
            gbPlayerRankings.Location = new Point(231, 65);
            gbPlayerRankings.Name = "gbPlayerRankings";
            gbPlayerRankings.Size = new Size(394, 436);
            gbPlayerRankings.TabIndex = 2;
            gbPlayerRankings.TabStop = false;
            gbPlayerRankings.Text = "Player Rankings (Top 10)";
            // 
            // btnLoadPlayers
            // 
            btnLoadPlayers.Location = new Point(535, 12);
            btnLoadPlayers.Name = "btnLoadPlayers";
            btnLoadPlayers.Size = new Size(75, 23);
            btnLoadPlayers.TabIndex = 3;
            btnLoadPlayers.Text = "Load";
            btnLoadPlayers.UseVisualStyleBackColor = true;
            btnLoadPlayers.Click += btnLoadPlayers_Click;
            // 
            // PlayerRankingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(btnLoadPlayers);
            Controls.Add(gbPlayerRankings);
            Controls.Add(cmbRankingType);
            Name = "PlayerRankingsForm";
            Text = "Rankings";
            gbPlayerRankings.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlRankingYellow;
        private ComboBox cmbRankingType;
        private GroupBox gbPlayerRankings;
        private Button btnLoadPlayers;
    }
}