using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint_Application
{
    public partial class Form1 : Form
    {
        Bitmap bm;//圖形物件bm , 虛擬畫布
        Graphics g;// 畫布物件g, 關係: pic 跟 bmp拿資料, g跟 bmp拿資料(g draw)
        bool isPainting = false;
        Point px, py;
        Pen pen = new Pen(Color.Black, 2);
        Pen eraser = new Pen(Color.White, 10);

        int sX, sY, cX, cY; //  sX, sY是圓形半徑
        int x, y; //畫直線會用到的變數

        int index; // 用來選不同工具

        ColorDialog cd = new ColorDialog();//調色盤用變數
        Color new_Color;

        public Form1()
        {
            InitializeComponent();

            this.Width = 900;
            this.Height = 700;
            bm = new Bitmap(pic.Width, pic.Height); // pic = picturebox, 初始化bitmap size
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);// 畫布初始化成白色
            pic.Image = bm; // 把bitmap這塊畫布 放到picturebox的image裡面
        }


        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            isPainting = true;
            px = e.Location;

            cX = e.X;//畫圓時的起點座標
            cY = e.Y;
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            isPainting = false;

            if(index == 3)
            {
                g.DrawEllipse(pen, cX, cY, sX, sY);
            }

            if (index == 4)
            {
                g.DrawLine(pen, cX, cY, x, y);
            }

            if (index == 5)
            {
                g.DrawRectangle(pen, cX, cY, sX, sY);
            }
        }

      

        private void pic_MouseMove(object sender, MouseEventArgs e)
        { // 要畫出線條要滿足兩個條件: select pencil tool & mouse down picture box
            if (isPainting)
            {
                if (index == 1)
                {
                    py = e.Location;
                    g.DrawLine(pen, px, py);
                    px = py;
                    
                }

                if (index == 2)
                {
                    py = e.Location;
                    g.DrawLine(eraser, px, py);
                    px = py;
                }

            }

            pic.Refresh();

            x = e.X;
            y = e.Y;
            // sX, sY是圓終點
            //cX, cY是圓起點
            
            sX = e.X - cX;// 用當下滑鼠座標 - 起點座標 = 圓半徑
            sY = e.Y - cY;


        }

        // 這個method是在畫圓 長方形 直線時可以看到軌跡
        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (isPainting)
            {
                if (index == 3)
                {
                    g.DrawEllipse(pen, cX, cY, sX, sY);
                }

                if (index == 4)
                {
                    g.DrawLine(pen, cX, cY, x, y);
                }

                if (index == 5)
                {
                    g.DrawRectangle(pen, cX, cY, sX, sY);
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pic.Image = bm;
            index = 0;
        }

        private void btn_color_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();// 開啟調色盤(color dialog)
            new_Color = cd.Color;
            pic_color.BackColor = cd.Color;// 把選擇的顏色顯示在空白的pic_color上(BackColor是背景色)
            pen.Color = cd.Color;
        }

        private void btn_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }

 
        private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }
        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog(); // 新增存檔視窗物件
            saveFileDialog.Filter = "Jpeg Image|* .jpg|Bitmap Image *.bmp|"; //在視窗介面顯示"檔案類型"
            saveFileDialog.Title = "Save an Image File";// 存檔視窗的title文字
            saveFileDialog.ShowDialog();// 加上這行視窗才會彈出來

            if (saveFileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        this.pic.Image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.pic.Image.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }


                fs.Close();// 要加這行完成存檔動作
            }


        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            index = 4;
        }
        private void btn_rect_Click(object sender, EventArgs e)
        {
            index = 5;

        }

        static Point set_point(PictureBox pb, Point pt)//定位點選color picker(PictureBox)的位置
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void color_picker_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(color_picker, e.Location);
            pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            new_Color = pic_color.BackColor;
            pen.Color = pic_color.BackColor;
        }

 
    }
}
