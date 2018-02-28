using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Knights_Tour
{
    public partial class Form1 : Form
    {
        private List<RectangleF> _rectangles;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DrawBoard();
        }
        //Bulunan tüm adımları çiz
        private void DrawMoves(List<Moves> moves)
        {
            int count = moves.Count;
            for (int i = 0; i < count; i++)
            {
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    //Çizim ayarları
                    Pen pen;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    StringFormat format = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    //Kutucugun üstüne numara yazdırma
                    g.DrawString(moves[i].Order.ToString(), new Font("Tahoma", 15), Brushes.Black, (moves[i].Y * 100) + 5, (moves[i].X * 100) + 5);
                    //Yönleri çizme
                    if (i < count - 1)
                    {
                        if (i % 2 == 0) pen = new Pen(Color.Red, 8); else pen = new Pen(Color.Blue, 8);
                        pen.StartCap = LineCap.RoundAnchor;
                        pen.EndCap = LineCap.ArrowAnchor;
                        g.DrawLine(pen, (moves[i].Y * 100) + 50, (moves[i].X * 100) + 50, (moves[i + 1].Y * 100) + 50, (moves[i + 1].X * 100) + 50);
                    }

                }
            }
        }

        //Santraç Tahtası çiziliyor
        private void DrawBoard()
        {
            Bitmap bm = new Bitmap(800, 800);
            _rectangles = new List<RectangleF>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle r = new Rectangle { X = i * 100, Y = j * 100, Width = 100, Height = 100 };
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        Brush selPen = Brushes.White;
                        if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                        {
                            selPen = Brushes.White;
                        }
                        else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                        {
                            selPen = Brushes.Bisque;
                        }

                        g.FillRectangle(selPen, r);

                    }
                    _rectangles.Add(r);
                }
            }
            pictureBox1.Image = bm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox1.Text = string.Empty;
            int x = int.Parse(textBox2.Text);
            int y = int.Parse(textBox3.Text);
            if (x < 0 || y < 0 || x > 7 || y > 7)
            {
                MessageBox.Show("0-7 arasıdaki sayılar girilmeli");
                button1.Enabled = true;
                return;
            }
            KnightTour t = new KnightTour(x, y);
            //Bulunan tüm adımları textboxa yaz
            int[,] moveses = t.GetBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    textBox1.Text += $"{moveses[i, j]}\t";
                }
                textBox1.Text += Environment.NewLine;
            }
            //Bulunan tüm adımları çiz
            DrawBoard();
           DrawMoves(t.GetMoves());
            button1.Enabled=true;
        }
    }
}
