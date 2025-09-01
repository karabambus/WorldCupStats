using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;

namespace WordCupStats.WinForms
{
    public partial class PlayerControl : UserControl
    {
        public StartingEleven Player { get; set; }
        public bool IsFavorite { get; set; }
        private bool isDragging = false;
        private Point dragStartPoint;
        private bool _isSelected = false;

        public PlayerControl()
        {
            InitializeComponent();
            SetupDragDrop();
            
            // FIKSNA veličina - ne mijenjaj!
            this.Size = new Size(330, 65);
            this.MinimumSize = new Size(330, 65);
            this.MaximumSize = new Size(330, 65);

            // Spriječi auto-resize
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Stiliziranje
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Cursor = Cursors.Hand;
            this.Padding = new Padding(2);

            // Hover efekt
            this.MouseEnter += (s, e) =>
            {
                this.BackColor = Color.FromArgb(240, 248, 255);
            };
            this.MouseLeave += (s, e) =>
            {
                this.BackColor = this.IsFavorite ? Color.LightYellow : Color.White;
            };
        }

        private void SetupDragDrop()
        {
            // Omogući drag na svim child kontrolama
            this.MouseDown += PlayerControl_MouseDown;
            this.MouseMove += PlayerControl_MouseMove;
            this.MouseUp += PlayerControl_MouseUp;

            // Za sve child kontrole također
            foreach (Control control in this.Controls)
            {
                control.MouseDown += PlayerControl_MouseDown;
                control.MouseMove += PlayerControl_MouseMove;
                control.MouseUp += PlayerControl_MouseUp;
            }
        }

        private void PlayerControl_MouseUp(object? sender, MouseEventArgs e)
        {
            isDragging = false;
            this.BackColor = IsFavorite ? Color.LightYellow : Color.White;
            this.Cursor = Cursors.Hand;
        }

        private void PlayerControl_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // set movement threshold
                if (Math.Abs(e.X - dragStartPoint.X) > 5 ||
                    Math.Abs(e.Y - dragStartPoint.Y) > 5)
                {
                    // Započni drag & drop
                    this.DoDragDrop(this, DragDropEffects.Move);
                    isDragging = false;
                }
            }

        }

        private void PlayerControl_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = e.Location;
                
                // feedback
                this.BackColor = Color.LightGray;
                this.Cursor = Cursors.Hand;
            }
        }

        public void SetPlayer(StartingEleven player, bool isFavorite)
        {
            Player = player;
            IsFavorite = isFavorite;

            lblName.Text = player.Name;
            lblNumber.Text = $"Broj: {player.ShirtNumber}";
            lblPosition.Text = $"Pozicija: {player.Position}";

            pbStar.Visible = isFavorite;

            // Postavi default sliku ako nema
            // pbImage.Image = Image.FromFile("default-player.png");
        }

        public void UpdateFavorite(bool isFavorite)
        {
            IsFavorite = isFavorite;
            pbStar.Visible = isFavorite;
            UpdateVisualState();
        }

        public void SetPlayerImage(String imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    // Dispose old image if exists
                    pbImage.Image?.Dispose();

                    // Load new image without locking the file
                    using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        pbImage.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    SetDefaultImage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                SetDefaultImage();
            }
        }

        public void SetDefaultImage()
        {
            pbImage.Image?.Dispose();

            var defaultPath = ImageHelper.GetDefaultPlayerImage();
            if (!string.IsNullOrEmpty(defaultPath) && File.Exists(defaultPath))
            {
                using (var stream = new FileStream(defaultPath, FileMode.Open, FileAccess.Read))
                {
                    pbImage.Image = Image.FromStream(stream);
                }
            }
            else
            {
                // Set a color as placeholder
                pbImage.Image = null;
                pbImage.BackColor = Color.LightGray;
            }
        }

        //private void UpdateDisplay(string teamCode)
        //{
        //    if (Player == null) return;

        //    lblName.Text = Player.Name;
        //    lblNumber.Text = $"#{Player.ShirtNumber}";

        //    // Set player image
        //    if (!string.IsNullOrEmpty(teamCode))
        //    {
        //        var imagePath = ImageHelper.GetPlayerImagePath(teamCode, Player.Name, Player.ShirtNumber);
        //        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        //        {
        //            SetPlayerImage(imagePath);
        //        }
        //        else
        //        {
        //            SetDefaultImage();
        //        }
        //    }
        //    else
        //    {
        //        SetDefaultImage();
        //    }
        //}

        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            UpdateVisualState();
        }

        public bool IsSelected => _isSelected;


        private void UpdateVisualState()
        {
            if (_isSelected)
            {
                // Selected appearance - bright blue border
                this.BackColor = Color.LightSkyBlue;
                this.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (IsFavorite)
            {
                // Favorite appearance - light green background
                this.BackColor = Color.LightGreen;
                this.BorderStyle = BorderStyle.None;
            }
            else
            {
                // Normal appearance
                this.BackColor = Color.White;
                this.BorderStyle = BorderStyle.None;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
