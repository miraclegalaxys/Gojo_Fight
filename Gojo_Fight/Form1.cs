using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;

namespace Gojo_Fight
{
    public partial class Form1 : Form
    {
        // ประกาศตัวแปรสำหรับรูปภาพของตัวละคร ภาพพื้นหลัง และภาพสกิล
        //------------------------------------------------------------

        Image gojo;
        Image sukuna;
        Image bg;
        Image kai;
        Image shotmurasaki;
        Image shotao;
        Image shotarka;

        //------------------------------------------------------------

        // กำหนดตำแหน่งเริ่มต้นของตัวละคร Gojo Sukuna skill บนแกน X และ Y ขนาดตัวละคร และ ขนาดของ skill
        //------------------------------------------------------------

        int gojoX = 0;
        int gojoY = 250;

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
        int sukunaY = 400;

        int gojoWidth = 500;
        int gojoHeight = 800;

        int gojoCurrentHp = 100;
        int sukunaCurrentHp = 1000;

        int sukunaWidth = 480;
        int sukunaHeight = 600;

        int bgPosition = 0;

        //---------------------------------------------------------

        //ประกาศตัวแปรสำหรับควบคุมการเคลื่อนที่
        //---------------------------------------------------------

        bool Ao = false;
        bool Arka = false;
        bool Mugen = false;
        bool Murasaki = false;
        bool gojoReverse = false;

        bool isKeyEHeld = false;
        bool isMurasakiMoving = false;
        bool isAoMoving = false;
        bool isArkaMoving = false;

        bool isShotaoVisible = true;
        bool isShotarkaVisible = true;

        bool showImage = true;

        //-------------------------------------------------------------

        //สร้างและตั้งค่า ProgressBar Label Font ของตัวละคร Gojo และ Sukuna
        //-------------------------------------------------------------

        ProgressBar gojoHP;
        ProgressBar sukunaHP;

        Label gojoName;
        Label sukunaName;

        Font gojoFont;
        Font sukunaFont;

        //---------------------------------------------------------------

        //ประกาศ Timer สำหรับควบคุมการเคลื่อนที่
        //---------------------------------------------------------------

        System.Windows.Forms.Timer murasakiMoveTimer;
        System.Windows.Forms.Timer aoMoveTimer;
        System.Windows.Forms.Timer arkaMoveTimer;
        System.Windows.Forms.Timer skillQCooldownTimer;
        System.Windows.Forms.Timer skillWCooldownTimer;
        System.Windows.Forms.Timer skillECooldownTimer;
        System.Windows.Forms.Timer skillRCooldownTimer;
        System.Windows.Forms.Timer blinkTimer;
        //System.Windows.Forms.Timer shotaoVisibilityTimer;
        //System.Windows.Forms.Timer shotarkaVisibilityTimer;

        //-----------------------------------------------------------------

        public Form1()
        {
            //เรัยก Methods ต่างๆ
            //----------------------------------------------------------------

            InitializeComponent();
            SetUpForm();
            HPbar();
            MurasakiShotMove();
            AoShotMove();
            ArkaShotMove();
            Skillcooldown();
            Blink();
            //SetUpTimers();

            //-------------------------------------------------------------------

            //เรียกรูปภาพและกำหนดความกว้างและยาวสำหรับรูปภาพตัวแปร kai
            //-------------------------------------------------------------------

            kai = Image.FromFile("sukuna\\2.png");
            kaiWidth = 400; 
            kaiHeight = 400;

            //-------------------------------------------------------------------

        }


        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            //สร้างภาพพื้นหลังและของตัวละคร
            //---------------------------------------------------------------------------------------------------------

            e.Graphics.DrawImage(bg, new Rectangle(bgPosition, 0, this.ClientSize.Width, this.ClientSize.Height));
            e.Graphics.DrawImage(gojo, new Rectangle(gojoX, gojoY, gojoWidth, gojoHeight));
            e.Graphics.DrawImage(sukuna, new Rectangle(sukunaX, sukunaY, sukunaWidth, sukunaHeight));

            //-----------------------------------------------------------------------------------------------------------

            //เรัยกใช้คำสั่งสร้างข้อความ
            //------------------------------------------------------------------------------------------------------------

            using (Font font = new Font("Arial", 50, FontStyle.Bold))
            {
         
                using (SolidBrush redBrush = new SolidBrush(Color.Red))
                {
                    // ตรวจสอบ HP Gojo ถ้า gojoCurrentHp <= 0 ให้แสดงข้อความว่า GAMEOVER โดยเรียกใช้คำสั่งการสร้างข้อความ
                    //-----------------------------------------------------------------------------------------------------------------------------------
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
                    // ตรวจสอบ HP Sukuna ถ้า SukunaCurrentHp <= 0 ให้แสดงข้อความว่า WIN โดยเรียกใช้คำสั่งการสร้างข้อความ
                    //-----------------------------------------------------------------------------------------------------------------------------------
                    if (sukunaCurrentHp == 0)
                    {
                        string winText = "WIN";
                        SizeF textSize = e.Graphics.MeasureString(winText, font);
                        PointF textPosition = new PointF((this.ClientSize.Width - textSize.Width) / 2, (this.ClientSize.Height - textSize.Height) / 2);
                        e.Graphics.DrawString(winText, font, goldBrush, textPosition);
                    }
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------------------------

            //สร้างภาพขึ้นมาหากหากตัวแปรในวงเล็บของ if นั้นเป็นจริง
            //------------------------------------------------------------------------------------------------------------

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

            if (isAoMoving && isShotaoVisible)
            {
                e.Graphics.DrawImage(shotao, new Rectangle(shotaoX, shotaoY, shotaoWidth, shotaoHeight));
            }

            if (isArkaMoving && isShotarkaVisible)
            {
                e.Graphics.DrawImage(shotarka, new Rectangle(shotarkaX, shotarkaY, shotarkaWidth, shotarkaHeight));
            }

            //----------------------------------------------------------------------------------------------------------------
        }


        /*private void SetUpTimers()
        {
            shotaoVisibilityTimer = new System.Windows.Forms.Timer();
            shotaoVisibilityTimer.Interval = 3000; // Example: 3 seconds
            shotaoVisibilityTimer.Tick += ShotaoVisibilityTimer_Tick;

            shotarkaVisibilityTimer = new System.Windows.Forms.Timer();
            shotarkaVisibilityTimer.Interval = 3000; // Example: 3 seconds
            shotarkaVisibilityTimer.Tick += ShotarkaVisibilityTimer_Tick;
        }

        private void ShotaoVisibilityTimer_Tick(object sender, EventArgs e)
        {
            isShotaoVisible = true;
            shotaoVisibilityTimer.Stop();
            Invalidate();
        }

        private void ShotarkaVisibilityTimer_Tick(object sender, EventArgs e)
        {
            isShotarkaVisible = true;
            shotarkaVisibilityTimer.Stop();
            Invalidate();
        }*/

       //-------------------------------------------------------------------------------------------------------

       //ตรวจสอบว่า Murasaki Ao Arka กำลังเคลื่อนที่หรือไม่ เพิ่มตำแหน่ง X ของ Shotmurasaki
       //ตรวจสอบว่าเกินขอบเขตของ Form หรือไม่ หยุดการเคลื่อนที่ถ้าเกินขอบเขตและเรียก CheckMurasakiHit() ถ้ายังไม่เกิน
       //--------------------------------------------------------------------------------------------------------

        private void MurasakiMoveTimer_Tick(object sender, EventArgs e)
        {

            if (isMurasakiMoving)
            {
                shotmurasakiX += 30;  // สกิลไปทางขวาที 30
                if (shotmurasakiX > this.Width) // ตรวจสอบถ้ากระสุนออกนอกหน้าจอ
                {
                    isMurasakiMoving = false;
                    murasakiMoveTimer.Stop();
                }
                else
                {
                    CheckMurasakiHit(); //ตรวจสอบการชนของกระสุน
                }
                Invalidate(); // อัปเดตหน้าจอ
            }
        }

        //------------------------------------------------------------------------------------------------------------

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

        //------------------------------------------------------------------------------------------------------------

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

        //------------------------------------------------------------------------------------------------------------

        //หยุดทุกท่าเคลื่อนที่ของทุกสกิลทันทีที่เริ่มต้น Timer
        //--------------------------------------------------------------------

        private void SkillCooldownTimer_Tick(object sender, EventArgs e)
        {
            skillRCooldownTimer.Stop();
            skillECooldownTimer.Stop();
            skillQCooldownTimer.Stop();
            skillWCooldownTimer.Stop();
        }

        //-----------------------------------------------------------------------

        //ตรวจสอบว่า HP ของ Gojo หรือ Sukuna มีค่าน้อยกว่าหรือเท่ากับ 0 หรือไม่
        //หยุด Timer ถ้ามีหนึ่งในตัวละครนั้น HP หมด
        //เปลี่ยนค่า showImage เพื่อทำให้ภาพเคลื่อนที่แสดงหรือซ่อนตามที่กำหนด
        //--------------------------------------------------------------------------

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }
         
            showImage = !showImage;// สลับสถานะการแสดงภาพ
            Invalidate(); // อัปเดตหน้าจอ
        }

        private void Blink()
        {
            blinkTimer = new System.Windows.Forms.Timer();
            blinkTimer.Interval = 2000; 
            blinkTimer.Tick += BlinkTimer_Tick;
            blinkTimer.Start();
        }

        //----------------------------------------------------------------------------------

        //ตั้งค่าตัวจับเวลาสำหรับการเคลื่อนที่ Ao Arka Murasaki Skillcooldown 
        //----------------------------------------------------------------------------------

        private void AoShotMove()
        {
            aoMoveTimer = new System.Windows.Forms.Timer();
            aoMoveTimer.Interval = 50; // ตั้งเวลานับถอยหลังเป็น 50 มิลลิวินาที
            aoMoveTimer.Tick += AoMoveTimer_Tick;
        }

        private void ArkaShotMove()
        {
            arkaMoveTimer = new System.Windows.Forms.Timer();
            arkaMoveTimer.Interval = 50; // ตั้งเวลานับถอยหลังเป็น 50 มิลลิวินาที
            arkaMoveTimer.Tick += ArkaMoveTimer_Tick;
        }


        private void MurasakiShotMove()
        {
            murasakiMoveTimer = new System.Windows.Forms.Timer();
            murasakiMoveTimer.Interval = 50; // ตั้งเวลานับถอยหลังเป็น 50 มิลลิวินาที
            murasakiMoveTimer.Tick += MurasakiMoveTimer_Tick;

        }

        private void Skillcooldown()
        {
            skillRCooldownTimer = new System.Windows.Forms.Timer();
            skillRCooldownTimer.Interval = 60000; // ตั้งเวลานับถอยหลังเป็น 60,000 มิลลิวินาที (60 วินาที)
            skillRCooldownTimer.Tick += SkillCooldownTimer_Tick;

            skillECooldownTimer = new System.Windows.Forms.Timer();
            skillECooldownTimer.Interval = 20000; // ตั้งเวลานับถอยหลังเป็น 20,000 มิลลิวินาที (20 วินาที)
            skillECooldownTimer.Tick += SkillCooldownTimer_Tick;

            skillQCooldownTimer = new System.Windows.Forms.Timer();
            skillQCooldownTimer.Interval = 3000; // ตั้งเวลานับถอยหลังเป็น 3,000 มิลลิวินาที (3 วินาที)
            skillQCooldownTimer.Tick += SkillCooldownTimer_Tick;

            skillWCooldownTimer = new System.Windows.Forms.Timer();
            skillWCooldownTimer.Interval = 5000; // ตั้งเวลานับถอยหลังเป็น 5,000 มิลลิวินาที (5 วินาที)
            skillWCooldownTimer.Tick += SkillCooldownTimer_Tick;
        }

        //----------------------------------------------------------------------------------

        //
        private void HPbar()
        {
            // สร้าง ProgressBar, Label, และ Font สำหรับ Gojo
            // ---------------------------------------------------------------------------

            gojoHP = new ProgressBar();
            gojoName = new Label();
            gojoFont = new Font("Arial", 20, FontStyle.Regular);
            gojoHP.Minimum = 0;
            gojoHP.Maximum = 100;
            gojoHP.Value = 100;
            gojoName.Text = "Gojo Satoru";
            gojoName.Font = gojoFont;

            //--------------------------------------------------------------------------------

            // สร้างและตั้งค่า Timer สำหรับลด GojoHP ทุก 2 วินาที
            //---------------------------------------------------------------------------------

            System.Windows.Forms.Timer gojoHPDecrementTimer = new System.Windows.Forms.Timer();
            gojoHPDecrementTimer.Interval = 2000; // ตั้งเวลานับถอยหลังเป็น 2000 มิลลิวินาที (2 วินาที)
            gojoHPDecrementTimer.Tick += GojoHPDecrementTimer_Tick; // เชื่อมต่อกับเหตุการณ์ Tick กับฟังก์ชัน GojoHPDecrementTimer_Tick
            gojoHPDecrementTimer.Start(); // เริ่มการทำงานของ Timer

            //---------------------------------------------------------------------------------

            // สร้างและตั้งค่า Timer สำหรับเพิ่ม GojoHP ทุก 2 วินาที
            //----------------------------------------------------------------------------------

            System.Windows.Forms.Timer autoIncreaseGojoTimer;
            autoIncreaseGojoTimer = new System.Windows.Forms.Timer();
            autoIncreaseGojoTimer.Interval = 1000; 
            autoIncreaseGojoTimer.Tick += AutoIncreaseTimer_Tick;
            autoIncreaseGojoTimer.Start();

            //-----------------------------------------------------------------------------------

            // กำหนดตำแหน่งและขนาดของ GojoHP และ GojoName
            //-----------------------------------------------------------------------------------

            gojoHP.Location = new Point(70, 50);
            gojoName.Location = new Point(70, 100);
            gojoHP.Size = new Size(800, 30);
            gojoName.Size = new Size(170, 32);

            //-----------------------------------------------------------------------------------

            // สร้าง ProgressBar, Label, และ Font สำหรับ Sukuna
            //-----------------------------------------------------------------------------------

            sukunaHP = new ProgressBar();
            sukunaName = new Label();
            sukunaFont = new Font("Arial", 20, FontStyle.Regular);
            sukunaHP.Minimum = 0;
            sukunaHP.Maximum = 1000;
            sukunaHP.Value = 1000;
            sukunaName.Text = "Ryomen Sukuna (Yuji Form)";
            sukunaName.Font = sukunaFont;

            //------------------------------------------------------------------------------------

            // สร้างและตั้งค่า Timer สำหรับเพิ่ม SukunaHP ทุก 2 วินาที
            //------------------------------------------------------------------------------------

            System.Windows.Forms.Timer autoIncreaseSukunaTimer;
            autoIncreaseSukunaTimer = new System.Windows.Forms.Timer();
            autoIncreaseSukunaTimer.Interval = 2000; 
            autoIncreaseSukunaTimer.Tick += AutoIncreaseTimer_Tick;
            autoIncreaseSukunaTimer.Start();

            //-------------------------------------------------------------------------------------

            // กำหนดตำแหน่งและขนาดของ SukunaHP และ SukunaName
            //-------------------------------------------------------------------------------------

            sukunaHP.Location = new Point(1000, 50);
            sukunaName.Location = new Point(1430, 100);
            sukunaHP.Size = new Size(800, 30);
            sukunaName.Size = new Size(370, 32);

            //-------------------------------------------------------------------------------------

            //เพิ่ม GojoHP, GojoName, SukunaHP, และ SukunaName ลงใน Controls
            //-------------------------------------------------------------------------------------

            Controls.Add(gojoHP);
            Controls.Add(gojoName);
            Controls.Add(sukunaHP);
            Controls.Add(sukunaName);

            //--------------------------------------------------------------------------------------
        }
        // โหลดรูปภาพสำหรับตัวละครและพื้นหลัง
        //---------------------------------------------------------------
        private void SetUpForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            gojo = Image.FromFile("gojo\\1.png");
            sukuna = Image.FromFile("sukuna\\1.png");
            bg = Image.FromFile("background.jpg");
        }

        //---------------------------------------------------------------

        //ตัวจับเวลาสำหรับเพิ่ม HP อัตโนมัติ
        //--------------------------------------------------------------------

        private void AutoIncreaseTimer_Tick(object sender, EventArgs e)
        {
            // หยุดจับเวลาถ้า HP ของ Gojo หรือ Sukuna เป็น 0 หรือน้อยกว่า
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }
            // เพิ่ม HP ของ Sukuna และ Gojo ตามเวลา
            SukunaReverse(10); // เพิ่ม HP ของ Sukuna 10 หน่วย
            IncrementGojoHealth(1); // เพิ่ม HP ของ Gojo 1 หน่วย
        }

        //---------------------------------------------------------------------

        //ตัวจับเวลาสำหรับลด HP ของ Gojo ตามเวลา
        //---------------------------------------------------------------------

        private void GojoHPDecrementTimer_Tick(object sender, EventArgs e)
        {
            // หยุดจับเวลาถ้า HP ของ Gojo หรือ Sukuna เป็น 0 หรือน้อยกว่า
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }
            // ลด HP ของ Gojo ถ้าปุ่ม E ไม่ถูกกดค้าง
            if (!isKeyEHeld)  
            {
                if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
                {
                    ((System.Windows.Forms.Timer)sender).Stop();
                    return;
                }
                DecrementGojoHealth(10);
            }
        }

        //-----------------------------------------------------------------------

        //เพิ่ม HP ของทั้ง Gojo และ Sukuna 
        //-------------------------------------------------------------------------

        private void GojoReverse(int increment)
        {
            gojoCurrentHp += increment;
            // ตรวจสอบและจำกัด HP ให้อยู่ในช่วงที่กำหนด
            if (gojoCurrentHp < gojoHP.Minimum)
                gojoCurrentHp = gojoHP.Minimum;

            if (gojoCurrentHp > gojoHP.Maximum)
                gojoCurrentHp = gojoHP.Maximum;

            gojoHP.Value = gojoCurrentHp;
        }

        private void IncrementGojoHealth(int increment)
        {
            // ตรวจสอบก่อนเพิ่ม HP
            if (gojoCurrentHp <= 0 || sukunaCurrentHp <= 0)
            {
                return;
            }

            gojoCurrentHp += increment;
            // จำกัด HP ให้อยู่ในช่วงที่กำหนด
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

        //--------------------------------------------------------------------

        //ลด HP ของ Gojo และจัดการลด HP
        //-------------------------------------------------------------------

        private void DecrementGojoHealth(int decrement)
        {
            gojoCurrentHp -= decrement;

            if (gojoCurrentHp < gojoHP.Minimum)
            {
                gojoCurrentHp = gojoHP.Minimum;
                this.Invalidate(); 
            }

            if (gojoCurrentHp > gojoHP.Maximum)
                gojoCurrentHp = gojoHP.Maximum;

            gojoHP.Value = gojoCurrentHp;
        }

        //-----------------------------------------------------------------------

        // ตรวจสอบสถานะของสกิลต่างๆ และกำหนดภาพและขนาดของตัวละคร Gojo ตามสกิลที่ถูกเลือก
        //-----------------------------------------------------------------

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

            // ถ้า gojoReverse และ isKeyEHeld เป็น true, จะเรียกใช้ GojoReverse
            if (gojoReverse && isKeyEHeld)
            {
                GojoReverse(50); 
            }
            // อัปเดตหน้าจอเพื่อแสดงการเปลี่ยนแปลง
            Invalidate();
        }

        //------------------------------------------------------------------------------

        // ตรวจสอบเมื่อมีการกดปุ่มบนแป้นพิมพ์และทำการเปิดใช้สกิลตามปุ่มที่ถูกกด
        //------------------------------------------------------------------------------

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

        //----------------------------------------------------------------------------------

        // ตรวจสอบเมื่อปล่อยปุ่มบนแป้นพิมพ์และหยุดการใช้สกิลที่เกี่ยวข้อง
        //--------------------------------------------------------------------------------------
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

        //----------------------------------------------------------------------

        // ฟังก์ชันที่จัดการกับการใช้สกิล Murasaki Ao Arka
        //----------------------------------------------------------------------

        private void ShootMurasaki()
        {
            shotmurasaki = Image.FromFile("gojo\\shotmurasaki.png");
            shotmurasakiX = gojoX + gojoWidth;
            shotmurasakiY = 200;
            shotmurasakiWidth = 750; 
            shotmurasakiHeight = 750; 
            isMurasakiMoving = true;
            murasakiMoveTimer.Start(); 
        }

        private void ShootAo()
        {
            shotao = Image.FromFile("gojo\\shotao.png");
            shotaoX = gojoX + 350;
            shotaoY = 400;
            shotaoWidth = 250;
            shotaoHeight = 270;
            isAoMoving = true;
            aoMoveTimer.Start();
        }
        private void ShootArka()
        {
            shotarka = Image.FromFile("gojo\\shotarka.png");
            shotarkaX = gojoX + 350;
            shotarkaY = 400;
            shotarkaWidth = 250;
            shotarkaHeight = 270;
            isArkaMoving = true;
            arkaMoveTimer.Start();
        }

        //----------------------------------------------------------------------

        //ฟังก์ชันที่ตรวจสอบว่าสกิล Murasaki Ao Arka ถูกตัวละคร Sukuna หรือไม่
        //----------------------------------------------------------------------------------------------------------------------------------------------

        private void CheckMurasakiHit()
        {
            // ตรวจสอบสกิล Murasaki โดนตัวละคร Sukuna
            bool collision = DetectCollision(shotmurasakiX, shotmurasakiY, shotmurasakiWidth, shotmurasakiHeight, sukunaX, sukunaY, sukunaWidth, sukunaHeight);

            if (collision)
            {
                // ลดค่า HP ของ Sukuna ถ้ามีการชน
                sukunaCurrentHp -= 20;
                // ตรวจสอบไม่ให้ HP ต่ำกว่า 0
                if (sukunaCurrentHp < 0) sukunaCurrentHp = 0;
                // อัปเดตแถบ HP ของ Sukuna
                sukunaHP.Value = sukunaCurrentHp; 
            }
        }

        private void CheckAoHit()
        {
            // ตรวจสอบสกิล Ao โดนตัวละคร Sukuna
            bool collision = DetectCollision(shotaoX, shotaoY, shotaoWidth, shotaoHeight, sukunaX, sukunaY, sukunaWidth, sukunaHeight);

            if (collision)
            {
                sukunaCurrentHp -= 8; 
                if (sukunaCurrentHp < 0) sukunaCurrentHp = 0; 
                sukunaHP.Value = sukunaCurrentHp;
              //  isShotaoVisible = false;
               // shotaoVisibilityTimer.Start();
            }
        }

        private void CheckArkaHit()
        {
            // ตรวจสอบสกิล Arka โดนตัวละคร Sukuna
            bool collision = DetectCollision(shotarkaX, shotarkaY, shotarkaWidth, shotarkaHeight, sukunaX, sukunaY, sukunaWidth, sukunaHeight);

            if (collision)
            {
                sukunaCurrentHp -= 10; 
                if (sukunaCurrentHp < 0) sukunaCurrentHp = 0; 
                sukunaHP.Value = sukunaCurrentHp; 
               // isShotarkaVisible = false;
               // shotarkaVisibilityTimer.Start();

            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------

        //ฟังก์ชันที่ตรวจสอบการชนระหว่างสองวัตถุ
        //------------------------------------------------------------------------------------------------------------------------------------------------------

        private bool DetectCollision(int object1X, int object1Y, int object1Width, int object1Height, int object2X, int object2Y, int object2Width, int object2Height)
        {
            // ตรวจสอบว่ามีการชนระหว่าง object1 และ object2 หรือไม่
            if (object1X + object1Width <= object2X || object1X >= object2X + object2Width || object1Y + object1Height <= object2Y || object1Y >= object2Y + object2Height)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

    }
}
