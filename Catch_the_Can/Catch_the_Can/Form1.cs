using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Catch_the_Can
{
    public partial class Form1 : Form
    {
        private PictureBox player;
        private PictureBox can;
        private Timer timer;
        private int score;
        private int missedCans;

        private string playerDirection = "straight";
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            this.FormClosing += Form1_FormClosing;
        }


        private void InitializeGame()
        {
            
            this.Text = "Catch the Can";
            this.Size = new Size(500, 500);
            this.KeyDown += Form1_KeyDown;

            
            player = new PictureBox();
            player.SizeMode = PictureBoxSizeMode.Zoom;
            player.Size = new Size(50, 50);
            player.Location = new Point(this.Width / 2 - player.Width / 2, this.Height / 2 - player.Height / 2);
            UpdatePlayerImage();
            this.Controls.Add(player);

            
            can = new PictureBox();
            can.Image = Properties.Resources.can;
            can.SizeMode = PictureBoxSizeMode.Zoom;
            can.Size = new Size(50, 50);
            RandomizeCanLocation();
            this.Controls.Add(can);

            
            timer = new Timer();
            timer.Interval = 3000; 
            timer.Tick += Timer_Tick;
            timer.Start();

            
            score = 0;
            missedCans = 0;
            UpdateScoreLabel();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            missedCans++;
            
            RandomizeCanLocation();
        }

        private void RandomizeCanLocation()
        {
            Random random = new Random();
            can.Location = new Point(random.Next(this.ClientSize.Width - can.Width), random.Next(this.ClientSize.Height - can.Height));

            if (timer != null)
            {
                timer.Stop();
                timer.Start();
            }
        }

        private void UpdateScoreLabel()
        {
            lblScore.Text = "Score: " + score.ToString();
        }

        private void CheckCollision()
        {
            // Check for collision between player and can
            if (player.Bounds.IntersectsWith(can.Bounds))
            {
                score++; 
                UpdateScoreLabel();
                RandomizeCanLocation();
                if (score == 5)
                {
                    
                    timer.Stop();
                    MessageBox.Show("Game Over! You caught 5 cans.\nTotal Score: " + score + "\nMissed Cans: " + missedCans, "Game Over");

                    this.Close();
                }
            }
        }

        private void UpdatePlayerImage()
        {
            
            switch (playerDirection)
            {
                case "straight":
                    player.Image = Properties.Resources.player1walkingstraight;
                    break;
                case "left":
                    player.Image = Properties.Resources.player1walkingleft;
                    break;
                case "right":
                    player.Image = Properties.Resources.player1walkingRight;
                    break;
                case "backward":
                    player.Image = Properties.Resources.player1walkingbackward;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (player.Top > 0)
                        player.Top -= 10;
                    playerDirection = "backward";
                    break;
                case Keys.Down:
                    if (player.Bottom < this.ClientSize.Height)
                        player.Top += 10;
                    playerDirection = "straight";
                    break;
                case Keys.Left:
                    if (player.Left > 0)
                        player.Left -= 10;
                    playerDirection = "left";
                    break;
                case Keys.Right:
                    if (player.Right < this.ClientSize.Width)
                        player.Left += 10;
                    playerDirection = "right";
                    break;
            }
            UpdatePlayerImage(); 
            CheckCollision();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
