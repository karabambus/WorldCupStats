
namespace WordCupStats.WinForms
{
    partial class MainForm
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
            menuStrip1 = new MenuStrip();
            datotekeToolStripMenuItem = new ToolStripMenuItem();
            postavkeToolStripMenuItem = new ToolStripMenuItem();
            izlazToolStripMenuItem = new ToolStripMenuItem();
            pomoćToolStripMenuItem = new ToolStripMenuItem();
            oProgramuToolStripMenuItem = new ToolStripMenuItem();
            lblSelectTeam = new Label();
            cmbTeams = new ComboBox();
            btnLoadPlayers = new Button();
            grpFavorites = new GroupBox();
            pnlFavorites = new Panel();
            grpOthers = new GroupBox();
            pnlOthers = new Panel();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            btRanking = new Button();
            btnRankingsAttendece = new Button();
            menuStrip1.SuspendLayout();
            grpFavorites.SuspendLayout();
            grpOthers.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { datotekeToolStripMenuItem, pomoćToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(834, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // datotekeToolStripMenuItem
            // 
            datotekeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { postavkeToolStripMenuItem, izlazToolStripMenuItem });
            datotekeToolStripMenuItem.Name = "datotekeToolStripMenuItem";
            datotekeToolStripMenuItem.Size = new Size(66, 20);
            datotekeToolStripMenuItem.Text = "Datoteke";
            // 
            // postavkeToolStripMenuItem
            // 
            postavkeToolStripMenuItem.Name = "postavkeToolStripMenuItem";
            postavkeToolStripMenuItem.Size = new Size(121, 22);
            postavkeToolStripMenuItem.Text = "Postavke";
            postavkeToolStripMenuItem.Click += postavkeToolStripMenuItem_Click;
            // 
            // izlazToolStripMenuItem
            // 
            izlazToolStripMenuItem.Name = "izlazToolStripMenuItem";
            izlazToolStripMenuItem.Size = new Size(121, 22);
            izlazToolStripMenuItem.Text = "Izlaz";
            // 
            // pomoćToolStripMenuItem
            // 
            pomoćToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { oProgramuToolStripMenuItem });
            pomoćToolStripMenuItem.Name = "pomoćToolStripMenuItem";
            pomoćToolStripMenuItem.Size = new Size(57, 20);
            pomoćToolStripMenuItem.Text = "Pomoć";
            // 
            // oProgramuToolStripMenuItem
            // 
            oProgramuToolStripMenuItem.Name = "oProgramuToolStripMenuItem";
            oProgramuToolStripMenuItem.Size = new Size(139, 22);
            oProgramuToolStripMenuItem.Text = "O programu";
            // 
            // lblSelectTeam
            // 
            lblSelectTeam.AutoSize = true;
            lblSelectTeam.Font = new Font("Microsoft Sans Serif", 10F);
            lblSelectTeam.Location = new Point(20, 40);
            lblSelectTeam.Name = "lblSelectTeam";
            lblSelectTeam.Size = new Size(221, 17);
            lblSelectTeam.TabIndex = 1;
            lblSelectTeam.Text = "Odaberite omiljenu reprezentaciju";
            // 
            // cmbTeams
            // 
            cmbTeams.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTeams.FormattingEnabled = true;
            cmbTeams.Location = new Point(20, 70);
            cmbTeams.Name = "cmbTeams";
            cmbTeams.Size = new Size(300, 23);
            cmbTeams.TabIndex = 2;
            // 
            // btnLoadPlayers
            // 
            btnLoadPlayers.Enabled = false;
            btnLoadPlayers.Location = new Point(340, 70);
            btnLoadPlayers.Name = "btnLoadPlayers";
            btnLoadPlayers.Size = new Size(75, 23);
            btnLoadPlayers.TabIndex = 3;
            btnLoadPlayers.Text = "Učitaj igrače";
            btnLoadPlayers.UseVisualStyleBackColor = true;
            btnLoadPlayers.Click += btnLoadPlayers_Click;
            // 
            // grpFavorites
            // 
            grpFavorites.Controls.Add(pnlFavorites);
            grpFavorites.Location = new Point(20, 110);
            grpFavorites.Name = "grpFavorites";
            grpFavorites.Size = new Size(380, 400);
            grpFavorites.TabIndex = 4;
            grpFavorites.TabStop = false;
            grpFavorites.Text = "Omiljeni igrači (max 3)";
            // 
            // pnlFavorites
            // 
            pnlFavorites.AutoScroll = true;
            pnlFavorites.BackColor = Color.White;
            pnlFavorites.BorderStyle = BorderStyle.FixedSingle;
            pnlFavorites.Location = new Point(10, 20);
            pnlFavorites.Name = "pnlFavorites";
            pnlFavorites.Size = new Size(360, 370);
            pnlFavorites.TabIndex = 0;
            // 
            // grpOthers
            // 
            grpOthers.Controls.Add(pnlOthers);
            grpOthers.Location = new Point(442, 110);
            grpOthers.Name = "grpOthers";
            grpOthers.Size = new Size(380, 400);
            grpOthers.TabIndex = 5;
            grpOthers.TabStop = false;
            grpOthers.Text = "Ostali igrači";
            // 
            // pnlOthers
            // 
            pnlOthers.AutoScroll = true;
            pnlOthers.BackColor = Color.White;
            pnlOthers.BorderStyle = BorderStyle.FixedSingle;
            pnlOthers.Location = new Point(10, 20);
            pnlOthers.Name = "pnlOthers";
            pnlOthers.Size = new Size(360, 370);
            pnlOthers.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 539);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(834, 22);
            statusStrip1.TabIndex = 6;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(54, 17);
            lblStatus.Text = "Spreman";
            lblStatus.Click += toolStripStatusLabel1_Click;
            // 
            // btRanking
            // 
            btRanking.Location = new Point(452, 63);
            btRanking.Name = "btRanking";
            btRanking.Size = new Size(170, 30);
            btRanking.TabIndex = 7;
            btRanking.Text = "Open Rankings";
            btRanking.UseVisualStyleBackColor = true;
            btRanking.Click += btRanking_Click;
            // 
            // btnRankingsAttendece
            // 
            btnRankingsAttendece.Location = new Point(642, 63);
            btnRankingsAttendece.Name = "btnRankingsAttendece";
            btnRankingsAttendece.Size = new Size(170, 30);
            btnRankingsAttendece.TabIndex = 8;
            btnRankingsAttendece.Text = "Open Attendence Rankings";
            btnRankingsAttendece.UseVisualStyleBackColor = true;
            btnRankingsAttendece.Click += btnRankingsAttendece_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 561);
            Controls.Add(btnRankingsAttendece);
            Controls.Add(btRanking);
            Controls.Add(statusStrip1);
            Controls.Add(grpOthers);
            Controls.Add(grpFavorites);
            Controls.Add(btnLoadPlayers);
            Controls.Add(cmbTeams);
            Controls.Add(lblSelectTeam);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "World Cup Stats";
            FormClosing += MainForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            grpFavorites.ResumeLayout(false);
            grpOthers.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem datotekeToolStripMenuItem;
        private ToolStripMenuItem postavkeToolStripMenuItem;
        private ToolStripMenuItem izlazToolStripMenuItem;
        private ToolStripMenuItem pomoćToolStripMenuItem;
        private ToolStripMenuItem oProgramuToolStripMenuItem;
        private Label lblSelectTeam;
        private ComboBox cmbTeams;
        private Button btnLoadPlayers;
        private GroupBox grpFavorites;
        private Panel pnlFavorites;
        private GroupBox grpOthers;
        private Panel pnlOthers;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private Button btRanking;
        private Button btnRankingsAttendece;
    }
}