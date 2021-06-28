using System.Drawing;
using System.Windows.Forms;

namespace readboard
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            if (!Program.isChn)
                this.Text = "Magnifier";
        }



        public void setPic(int x,int y)
        {

            System.Drawing.Bitmap bitmap = new Bitmap(20, 20);
            using (System.Drawing.Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(x-10, y-10, 0, 0, new System.Drawing.Size(20, 20));
                
            }
            System.Drawing.Bitmap bitmap2 = ZoomImage(bitmap, 120, 120);
            
            pictureBox1.Image = bitmap2;
          

        }


        private Bitmap ZoomImage(Bitmap bitmap, int destHeight, int destWidth)
        {
            try
            {
                System.Drawing.Image sourImage = bitmap;
                int width = 0, height = 0;
                //按比例缩放             
                int sourWidth = sourImage.Width;
                int sourHeight = sourImage.Height;
                if (sourHeight > destHeight || sourWidth > destWidth)
                {
                    if ((sourWidth * destHeight) > (sourHeight * destWidth))
                    {
                        width = destWidth;
                        height = (destWidth * sourHeight) / sourWidth;
                    }
                    else
                    {
                        height = destHeight;
                        width = (sourWidth * destHeight) / sourHeight;
                    }
                }
                else
                {
                    width = sourWidth;
                    height = sourHeight;
                }
                Bitmap destBitmap = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(destBitmap);
                g.Clear(Color.Transparent);
                //设置画布的描绘质量           
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(sourImage, new Rectangle(0,0 , destWidth, destHeight), 0, 0, sourImage.Width, sourImage.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(Color.Red, 5), 45, 60, 75, 60);
                g.DrawLine(new Pen(Color.Red, 5), 60, 45, 60, 75);
                g.Dispose();
                
                //设置压缩质量       
                //System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
                //long[] quality = new long[1];
                //quality[0] = 100;
                //System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                //encoderParams.Param[0] = encoderParam;
                sourImage.Dispose();
                return destBitmap;
            }
            catch
            {
                return bitmap;
            }
        }
    }
}
