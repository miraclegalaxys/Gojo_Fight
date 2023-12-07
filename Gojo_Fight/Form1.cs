using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;

namespace Gojo_Fight
{
    public partial class Form1 : Form
    {
        Image gojo;
        Image sukuna;
        Image bg;
        Image kai;
        Image shotmurasaki;
        Image shotao;
        Image shotarka;


        int gojoX = 0;
        int gojoY = 350;

        int shotmurasakiX;
        int shotmurasakiY;

        int shotmurasakiWidth;
        int shotmurasakiHeight;


        int shotaoX;
        int shotaoY;

        int shotaoWidth;
        int shotaoHeight;

        int shotarkaX;
        int shotarkaY;

        int shotarkaWidth;
        int shotarkaHeight;

        int kaiWidth = 100;
        int kaiHeight = 100;


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
        bool isMurasakiMoving = false;
        bool isAoMoving = false;
        bool isArkaMoving = false;

        //bool kai = false;
        //bool hashi = false;

        bool showImage = true;

        ProgressBar gojoHP;
        ProgressBar sukunaHP;

        Label gojoName;
        Label sukunaName;

        Font gojoFont;
        Font sukunaFont;


        System.Windows.Forms.Timer murasakiMoveTimer;
        System.Windows.Forms.Timer aoMoveTimer;
        System.Windows.Forms.Timer arkaMoveTimer;
        System.Windows.Forms.Timer skillQCooldownTimer;
        System.Windows.Forms.Timer skillWCooldownTimer;
        System.Windows.Forms.Timer skillECooldownTimer;
        System.Windows.Forms.Timer skillRCooldownTimer;
        System.Windows.Forms.Timer blinkTimer;



        public Form1()
        {
            InitializeComponent();
            SetUpForm();
            HPbar();
            MurasakiShotMove();
            AoShotMove();
            ArkaShotMove();
            Skillcooldown();
            Blink();

            kai = Image.FromFile("sukuna\\2.png");
            kaiWidth = 400; 
            kaiHeight = 400;

        }


        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bg, new Rectangle(bgPosition, 0, this.ClientSize.Width, this.ClientSize.Height));
            e.Graphics.DrawImage(gojo, new Rectangle(gojoX, gojoY, gojoWidth, gojoHeight));
            e.Graphics.DrawImage(sukuna, new Rectangle(sukunaX, sukunaY, sukunaWidth, sukunaHeight));


            using (Font font = new Font("Arial", 50, FontStyle.Bold))
            {
         
                using (SolidBrush redBrush = new SolidBrush(Color.Red))
                {
                    // Check Gojo's HP
                    if (gojoCurrentHp <= 0)
                    {
                        string gameOverText = "GAME OVER";
                        SizeF textSize = e.Graphics.MeasureString(gameOverText, font);
                        PointF textPosition = new PointF((this.ClientSize.Width - textSize.Width) / 2, (this.ClientSize.Height - textSize.Height) / 2);
                        e.Graphics.DrawString(gameOverText, font, redBrush, textPosition);
                    }
                }

                
                using (SolidBrush goldBrush = new SolidBrush(Color.Gold))
                {
                    // Check Sukuna's HP
                    if (sukunaCurrentHp <= 0)
                    {
                        string winText = "WIN";
                        SizeF textSize = e.Graphics.MeasureString(winText, font);
                        PointF textPosition = new PointF((this.ClientSize.Width - textSize.Width) / 2, (this.ClientSize.Height - textSize.Height) / 2);
                        e.Graphics.DrawString(winText, font, goldBrush, textPosition);
                    }
                }
            }

            if (showImage)
            {
                int kaiPositionX = gojoX + (gojoWidth / 2) - (kaiWidth / 2); 
                int kaiPositionY = gojoY +120; 
                e.Graphics.DrawImage(kai, new Rectangle(kaiPositionX, kaiPositionY, kaiWidth, kaiHeight));
            }

            if (isMurasakiMoving)
            {
                e.Graphics.DrawImage(shotmurasaki, new Rectangle(shotmurasakiX, shotmurasakiY, shotmurasakiWidth, shotmurasakiHeight));
            }

            if (isAoMoving)
            {
                e.Graphics.DrawImage(shotao, new Rectangle(shotaoX, shotaoY, shotaoWidth, shotaoHeight));
            }

            if (isArkaMoving)
            {
                e.Graphics.DrawImage(shotarka, new Rectangle(shotarkaX, shotarkaY, shotarkaWidth, shotarkaHeight));
            }

        }

        private void MurasakiMoveTimer_Tick(object sender, EventArgs e)
        {

            if (isMurasakiMoving)
            {
                shotmurasakiX += 30; 
                if (shotmurasakiX > this.Width)
                {
                    isMurasakiMoving = false;
                    murasakiMoveTimer.Stop();
                }
                else
                {
                    CheckMurasakiHit(); 
                }
                Invalidate();
            }
        }


        private void AoMoveTimer_Tick(object sender, EventArgs e)
        {
           if (isAoMoving)
            {
                shotaoX += 30;
                if(shotaoX > this.Width)
                {
                    isAoMoving = false;
                    aoMoveTimer.Stop();
                }
                else
                {
                    CheckAoHit();
                }
                Invalidate();
            }
               
        }

        private void ArkaMoveTimer_Tick(object sender, EventArgs e)
        {
            if (isArkaMoving)
            {
                shotarkaX += 30;
                if(shotarkaX > this.Width)
                {
                    isArkaMoving = false;
                    arkaMoveTimer.Stop();
                }
                else
                {
                    CheckArkaHit();
                }
                Invalidate();
            }
        }

        private void SkillCooldownTimer_Tick(object sender, EventArgs e)
        {
            skillRCooldownTimer.Stop();
            skillECooldownTimer.Stop();
            skillQCooldownTimer.Stop();
            skillWCooldownTimer.Stop();
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }
         
            showImage = !showImage;
            Invalidate(); 
        }

        private void Blink()
        {
            blinkTimer = new System.Windows.Forms.Timer();
            blinkTimer.Interval = 2000; 
            blinkTimer.Tick += BlinkTimer_Tick;
            blinkTimer.Start();
        }

        private void AoShotMove()
        {
            aoMoveTimer = new System.Windows.Forms.Timer();
            aoMoveTimer.Interval = 50;
            aoMoveTimer.Tick += AoMoveTimer_Tick;
        }

        private void ArkaShotMove()
        {
            arkaMoveTimer = new System.Windows.Forms.Timer();
            arkaMoveTimer.Interval = 50;
            arkaMoveTimer.Tick += ArkaMoveTimer_Tick;
        }


        private void MurasakiShotMove()
        {
            murasakiMoveTimer = new System.Windows.Forms.Timer();
            murasakiMoveTimer.Interval = 50; 
            murasakiMoveTimer.Tick += MurasakiMoveTimer_Tick;

        }

        private void Skillcooldown()
        {
            skillRCooldownTimer = new System.Windows.Forms.Timer();
            skillRCooldownTimer.Interval = 60000; 
            skillRCooldownTimer.Tick += SkillCooldownTimer_Tick;

            skillECooldownTimer = new System.Windows.Forms.Timer();
            skillECooldownTimer.Interval = 20000;
            skillECooldownTimer.Tick += SkillCooldownTimer_Tick;

            skillQCooldownTimer = new System.Windows.Forms.Timer();
            skillQCooldownTimer.Interval = 3000;
            skillQCooldownTimer.Tick += SkillCooldownTimer_Tick;

            skillWCooldownTimer = new System.Windows.Forms.Timer();
            skillWCooldownTimer.Interval = 5000;
            skillWCooldownTimer.Tick += SkillCooldownTimer_Tick;
        }

        private void HPbar()
        {
            gojoHP = new ProgressBar();
            gojoName = new Label();
            gojoFont = new Font("Arial", 12, FontStyle.Regular);
            gojoHP.Minimum = 0;
            gojoHP.Maximum = 100;
            gojoHP.Value = 100;
            gojoName.Text = "Gojo Satoru";
            gojoName.Font = gojoFont;

            System.Windows.Forms.Timer gojoHPDecrementTimer = new System.Windows.Forms.Timer();
            gojoHPDecrementTimer.Interval = 2000; 
            gojoHPDecrementTimer.Tick += GojoHPDecrementTimer_Tick;
            gojoHPDecrementTimer.Start();

            
            System.Windows.Forms.Timer autoIncreaseGojoTimer;
            autoIncreaseGojoTimer = new System.Windows.Forms.Timer();
            autoIncreaseGojoTimer.Interval = 1000; 
            autoIncreaseGojoTimer.Tick += AutoIncreaseTimer_Tick;
            autoIncreaseGojoTimer.Start();

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


            // Timer for increasing HP
            System.Windows.Forms.Timer autoIncreaseSukunaTimer;
            autoIncreaseSukunaTimer = new System.Windows.Forms.Timer();
            autoIncreaseSukunaTimer.Interval = 2000; 
            autoIncreaseSukunaTimer.Tick += AutoIncreaseTimer_Tick;
            autoIncreaseSukunaTimer.Start();

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

        private void AutoIncreaseTimer_Tick(object sender, EventArgs e)
        {
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }

            SukunaReverse(20);
            IncrementGojoHealth(2);
        }


        private void GojoHPDecrementTimer_Tick(object sender, EventArgs e)
        {
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }
            DecrementGojoHealth(10);
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

        private void IncrementGojoHealth(int increment)
        {

            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                return;
            }

            gojoCurrentHp += increment;
            if (gojoCurrentHp > gojoHP.Maximum)
            {
                gojoCurrentHp = gojoHP.Maximum;
            }

            gojoHP.Value = gojoCurrentHp;
        }

        private void SukunaReverse(int increment2)
        {

            if (sukunaCurrentHp <= 0 || gojoCurrentHp <= 0)
            {
                return;
            }

            sukunaCurrentHp += increment2;
            if (sukunaCurrentHp > sukunaHP.Maximum)
            {
                sukunaCurrentHp = sukunaHP.Maximum;
            }

            sukunaHP.Value = sukunaCurrentHp;
        }


        private void DecrementGojoHealth(int decrement)
        {
            gojoCurrentHp -= decrement;

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
                gojoWidth = 550;
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
                GojoReverse(50); 
            }

            Invalidate();
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                if (skillQCooldownTimer !=  null && !skillQCooldownTimer.Enabled)
                {
                    Ao = true;
                    ShootAo();
                    gojoSkills();
                    skillQCooldownTimer.Start();
                } 

            }
            else if (e.KeyCode == Keys.W)
            {
                if (skillWCooldownTimer != null && !skillWCooldownTimer.Enabled)
                {
                    Arka = true;
                    gojoSkills();
                    ShootArka();
                    skillWCooldownTimer.Start();
                }
            }
            else if (e.KeyCode == Keys.E)
            {
                if (skillECooldownTimer != null && !skillECooldownTimer.Enabled)
                {
                    Mugen = true;
                    gojoReverse = true;
                    isKeyEHeld = true;
                    gojoSkills();
                    skillECooldownTimer.Start();
                }

            }
            else if (e.KeyCode == Keys.R)
            {
                if (skillRCooldownTimer != null && !skillRCooldownTimer.Enabled)
                {
                    Murasaki = true;
                    gojoSkills();
                    ShootMurasaki();
                    skillRCooldownTimer.Start();
                }
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
                isKeyEHeld = false;
                gojoSkills();
            }
            else if (e.KeyCode == Keys.R)
            {
                Murasaki = false;
                gojoSkills();
            }
        }


        private void ShootMurasaki()
        {
            shotmurasaki = Image.FromFile("gojo\\shotmurasaki.png");
            shotmurasakiX = gojoX + gojoWidth;
            shotmurasakiY = 200;
            shotmurasakiWidth = 1000; 
            shotmurasakiHeight = 1000; 
            isMurasakiMoving = true;
            murasakiMoveTimer.Start(); 
        }

        private void ShootAo()
        {
            shotao = Image.FromFile("gojo\\shotao.png");
            shotaoX = gojoX + 350;
            shotaoY = gojoY;
            shotaoWidth = 250;
            shotaoHeight = 270;
            isAoMoving = true;
            aoMoveTimer.Start();
        }
        private void ShootArka()
        {
            shotarka = Image.FromFile("gojo\\shotarka.png");
            shotarkaX = gojoX + 350;
            shotarkaY = gojoY;
            shotarkaWidth = 250;
            shotarkaHeight = 270;
            isArkaMoving = true;
            arkaMoveTimer.Start();
        }

        private void CheckMurasakiHit()
        {
            bool collision = DetectCollision(shotmurasakiX, shotmurasakiY, shotmurasakiWidth, shotmurasakiHeight, sukunaX, sukunaY, sukunaWidth, sukunaHeight);

            if (collision)
            {
                sukunaCurrentHp -= 15;
                if (sukunaCurrentHp < 0) sukunaCurrentHp = 0; 
                sukunaHP.Value = sukunaCurrentHp; 
            }
        }

        private void CheckAoHit()
        {
            bool collision = DetectCollision(shotaoX, shotaoY, shotaoWidth, shotaoHeight, sukunaX, sukunaY, sukunaWidth, sukunaHeight);

            if (collision)
            {
                sukunaCurrentHp -= 5; 
                if (sukunaCurrentHp < 0) sukunaCurrentHp = 0; 
                sukunaHP.Value = sukunaCurrentHp;
            }
        }

        private void CheckArkaHit()
        {
            bool collision = DetectCollision(shotarkaX, shotarkaY, shotarkaWidth, shotarkaHeight, sukunaX, sukunaY, sukunaWidth, sukunaHeight);

            if (collision)
            {
                sukunaCurrentHp -= 8; 
                if (sukunaCurrentHp < 0) sukunaCurrentHp = 0; 
                sukunaHP.Value = sukunaCurrentHp; 
            }
        }

        private bool DetectCollision(int object1X, int object1Y, int object1Width, int object1Height, int object2X, int object2Y, int object2Width, int object2Height)
        {
            if (object1X + object1Width <= object2X || object1X >= object2X + object2Width || object1Y + object1Height <= object2Y || object1Y >= object2Y + object2Height)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {


        }
    }
}
