using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Gojo_Fight
{
    public partial class Form1 : Form
    {
        Image gojo;
        Image sukuna;
        Image bg;

        int gojoX = 0;
        int gojoY = 350;

        int sukunaX = 1450;
        int sukunaY = 500;

        int gojoWidth = 500;
        int gojoHeight = 800;

        int gojoCurrentHp = 100;
        int sukunaCurrentHp = 1000;

        int sukunaWidth = 480;
        int sukunaHeight = 600;

        int bgPosition = 0;

        bool Ao = false;
        bool Arka = false;
        bool Mugen = false;
        bool Murasaki = false;
        bool gojoReverse = false;

        bool isKeyEHeld = false;

        bool kai = false;
        bool hashi = false;
        bool sukunaReverse = false;

        ProgressBar gojoHP;
        ProgressBar sukunaHP;

        Label gojoName;
        Label sukunaName;

        Font gojoFont;
        Font sukunaFont;

        //System.Windows.Forms.Timer gojoHPTimer;
        Stopwatch timerStopwatch;
        System.Windows.Forms.Timer autoIncreaseGojoTimer;
        //System.Windows.Forms.Timer eKeyTimer;

        public Form1()
        {
            InitializeComponent();
            SetUpForm();
            HPbar();

            /*gojoHPTimer = new System.Windows.Forms.Timer();
            gojoHPTimer.Interval = 1000; // 1 second
            gojoHPTimer.Tick += GojoHPTimer_Tick;*/

            /*eKeyTimer = new System.Windows.Forms.Timer();
            eKeyTimer.Interval = 5000; // Set the interval to 5000 milliseconds (5 seconds)
            eKeyTimer.Tick += EKeyTimer_Tick;*/

          /*  autoIncreaseGojoTimer = new System.Windows.Forms.Timer();
            autoIncreaseGojoTimer.Interval = 5000; // Set the interval to 5000 milliseconds (5 seconds)
            autoIncreaseGojoTimer.Tick += AutoIncreaseTimer_Tick; // Add event handler
            autoIncreaseGojoTimer.Start(); // Start the timer for auto HP increase */

           // timerStopwatch = new Stopwatch();
           // timerStopwatch.Start();
        }

        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bg, new Rectangle(bgPosition, 0, this.ClientSize.Width, this.ClientSize.Height));
            e.Graphics.DrawImage(gojo, new Rectangle(gojoX, gojoY, gojoWidth, gojoHeight));
            e.Graphics.DrawImage(sukuna, new Rectangle(sukunaX, sukunaY, sukunaWidth, sukunaHeight));
        }

        private void HPbar()
        {
            gojoHP = new ProgressBar();
            gojoName = new Label();
            gojoFont = new Font("Arial", 12, FontStyle.Regular);
            gojoHP.Minimum = 0;
            gojoHP.Maximum = 100;
            gojoHP.Value = 10;
            gojoName.Text = "Gojo Satoru";
            gojoName.Font = gojoFont;

            autoIncreaseGojoTimer = new System.Windows.Forms.Timer();
            autoIncreaseGojoTimer.Interval = 5000; // Set the interval to 5000 milliseconds (5 seconds)
            autoIncreaseGojoTimer.Tick += AutoIncreaseTimer_Tick; // Add event handler
            autoIncreaseGojoTimer.Start(); // Start the timer for auto HP increase

            gojoHP.Location = new Point(70, 50);
            gojoName.Location = new Point(70, 100);
            gojoHP.Size = new Size(850, 30);
            gojoName.Size = new Size(190, 40);

            sukunaHP = new ProgressBar();
            sukunaName = new Label();
            sukunaFont = new Font("Arial", 12, FontStyle.Regular);
            sukunaHP.Minimum = 0;
            sukunaHP.Maximum = 1000;
            sukunaHP.Value = 1000;
            sukunaName.Text = "Ryomen Sukuna (Yuji Form)";
            sukunaName.Font = sukunaFont;

            sukunaHP.Location = new Point(1080, 50);
            sukunaName.Location = new Point(1680, 100);
            sukunaHP.Size = new Size(850, 30);
            sukunaName.Size = new Size(250, 40);

            Controls.Add(gojoHP);
            Controls.Add(gojoName);
            Controls.Add(sukunaHP);
            Controls.Add(sukunaName);
        }

        private void SetUpForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            gojo = Image.FromFile("gojo\\1.png");
            sukuna = Image.FromFile("sukuna\\1.png");
            bg = Image.FromFile("background.jpg");

        }
        private void EKeyTimer_Tick(object sender, EventArgs e)
        {
            /*if (isKeyEHeld)
            {
                GojoReverse(25); // Increment HP by 25
            }*/
        }
        private void AutoIncreaseTimer_Tick(object sender, EventArgs e)
        {
            GojoReverse(1); // Increment HP by 25
        }

        private void GojoHPTimer_Tick(object sender, EventArgs e)
        {
            /*if (gojoReverse && isKeyEHeld && timerStopwatch.ElapsedMilliseconds % 5000 == 0)  // Every 5 seconds
            {
                GojoReverse(25); // Increment HP by 25
            }*/
        }

        private void GojoReverse(int increment)
        {
            gojoCurrentHp += increment;

            if (gojoCurrentHp < gojoHP.Minimum)
                gojoCurrentHp = gojoHP.Minimum;

            if (gojoCurrentHp > gojoHP.Maximum)
                gojoCurrentHp = gojoHP.Maximum;

            gojoHP.Value = gojoCurrentHp;
        }

        private void gojoSkills()
        {
            if (Ao)
            {
                gojo = Image.FromFile("gojo\\2.png");
                gojoWidth = 600;
                gojoHeight = 800;
            }
            else if (Arka)
            {
                gojo = Image.FromFile("gojo\\3.png");
                gojoWidth = 550;
                gojoHeight = 800;
            }
            else if (Mugen)
            {
                gojo = Image.FromFile("gojo\\4.png");
                gojoWidth = 550;
                gojoHeight = 800;
            }
            else if (Murasaki)
            {
                gojo = Image.FromFile("gojo\\5.png");
                gojoWidth = 550;
                gojoHeight = 800;
            }
            else
            {
                gojo = Image.FromFile("gojo\\1.png");
                gojoWidth = 500;
                gojoHeight = 800;
            }

            if (gojoReverse && isKeyEHeld)
            {
                GojoReverse(1); // Increment HP by 1 every second
            }

            Invalidate();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Ao = true;
                gojoSkills();
            }
            else if (e.KeyCode == Keys.W)
            {
                Arka = true;
                gojoSkills();
            }
            else if (e.KeyCode == Keys.E)
            {
                Mugen = true;
                gojoReverse = true;
                //isKeyEHeld = true;
                autoIncreaseGojoTimer.Start();
                //gojoHPTimer.Start();
                gojoSkills();
            }
            else if (e.KeyCode == Keys.R)
            {
                Murasaki = true;
                gojoSkills();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Ao = false;
                gojoSkills();
            }
            else if (e.KeyCode == Keys.W)
            {
                Arka = false;
                gojoSkills();
            }
            else if (e.KeyCode == Keys.E)
            {
                Mugen = false;
                gojoReverse = false;
                //isKeyEHeld = false;
                autoIncreaseGojoTimer.Start();
                //eKeyTimer.Stop();
                //gojoHPTimer.Stop();
                gojoSkills();
            }
            else if (e.KeyCode == Keys.R)
            {
                Murasaki = false;
                gojoSkills();
            }
        }
    }
}
