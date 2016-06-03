using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;


namespace Pingpong
{
    
    public partial class SettingsForm : Form
    {
        private Bitmap originalBitmap = null;
        private Bitmap previewBitmap = null;
        private Bitmap resultBitmap = null;

       
        
        public SettingsForm()
        {
            InitializeComponent();
           
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var Btn = (NumericUpDown)sender;
            if (Btn.Value > 0)
            {
                switch (Btn.Name)
                {
                    case "numericUpDown1":
                        Properties.Settings.Default.BallSpeed = (int)numericUpDown1.Value;
                        break;
                    case "numericUpDown2":
                        Properties.Settings.Default.Timer_Enemy = (int)numericUpDown2.Value;
                        break;
                    case "numericUpDown3":
                        Properties.Settings.Default.Timer_Movement = (int)numericUpDown3.Value;
                        break;
                    case "numericUpDown4":
                        Properties.Settings.Default.EnemySpeed = (int)numericUpDown4.Value;
                        break;
                    case "numericUpDown5":
                        Properties.Settings.Default.Speed_Player = (int)numericUpDown5.Value;
                        break;

                }
            }
            else
            {
                Btn.Value = 1;
            }
            Properties.Settings.Default.Save();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                pictureBox4.BackColor = CD.Color;
                Properties.Settings.Default.Color_Player = CD.Color;
            }
            Properties.Settings.Default.Save();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.BackColor = CD.Color;
                Properties.Settings.Default.Color_Enemy = CD.Color;
            }
            Properties.Settings.Default.Save();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.BackColor = CD.Color;
                Properties.Settings.Default.Color_Ball = CD.Color;
            }
            Properties.Settings.Default.Save();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = CD.Color;
                Properties.Settings.Default.Color_Frame = CD.Color;
            }
            Properties.Settings.Default.Save();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["Form1"].Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            pictureBox2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Red;
            pictureBox4.BackColor = Color.SkyBlue;

            numericUpDown1.Value = 3;
            numericUpDown2.Value = 10;
            numericUpDown3.Value = 1;
            numericUpDown4.Value = 1;
            numericUpDown5.Value = 1;

            Properties.Settings.Default.Timer_Enemy = 10;
            Properties.Settings.Default.Timer_Movement = 1;
            Properties.Settings.Default.BallSpeed = 3;
            Properties.Settings.Default.EnemySpeed = 1;
            Properties.Settings.Default.Speed_Player = 1;
            Properties.Settings.Default.Color_Ball = Color.Black;
            Properties.Settings.Default.Color_Frame = Color.White;
            Properties.Settings.Default.Color_Enemy = Color.Red;
            Properties.Settings.Default.Color_Player = Color.SkyBlue;
           

            Properties.Settings.Default.Save();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Properties.Settings.Default.Color_Frame;
            pictureBox2.BackColor = Properties.Settings.Default.Color_Ball;
            pictureBox3.BackColor = Properties.Settings.Default.Color_Enemy;
            pictureBox4.BackColor = Properties.Settings.Default.Color_Player;
            numericUpDown1.Value = Properties.Settings.Default.BallSpeed;
            numericUpDown2.Value = Properties.Settings.Default.Timer_Enemy;
            numericUpDown3.Value = Properties.Settings.Default.Timer_Movement;
            numericUpDown4.Value = Properties.Settings.Default.EnemySpeed;
            numericUpDown5.Value = Properties.Settings.Default.Speed_Player;

            
        }
        private void ApplyFilter(bool preview)
        {
            if (previewBitmap == null)
            {
                return;
            }

            if (preview == true)
            {
                picPreview.Image = previewBitmap.Contrast(trcThreshold.Value);

            }
            else
            {
                resultBitmap = originalBitmap.Contrast(trcThreshold.Value);
            }
        }
        private void trcThreshold_Scroll(object sender, EventArgs e)
        {

        }

        private void lblContrast_Click(object sender, EventArgs e)
        {

        }

        private void ThresholdValueChangedEventHandler(object sender, EventArgs e)
        {
            lblContrastValue.Text = trcThreshold.Value.ToString();

            ApplyFilter(true);
        }

        private void btnOpenOriginal_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
            ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();

                previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
                picPreview.Image = previewBitmap;

                ApplyFilter(true);
            }
        }

        private void btnSaveNewImage_Click(object sender, EventArgs e)
        {
            ApplyFilter(false);

            if (resultBitmap != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Specify a file name and file path";
                sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                    ImageFormat imgFormat = ImageFormat.Png;

                    if (fileExtension == "BMP")
                    {
                        imgFormat = ImageFormat.Bmp;
                    }
                    else if (fileExtension == "JPG")
                    {
                        imgFormat = ImageFormat.Jpeg;
                    }

                    StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                    resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                    streamWriter.Flush();
                    streamWriter.Close();

                    resultBitmap = null;
                }
            }
        }
       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            

        }
    }
}
