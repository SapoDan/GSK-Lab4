using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSC_Lr4
{
    public partial class Form1 : Form
    {
        Bitmap myBitmap;
        Graphics g;
        Pen DrawPen = new Pen(Color.Black, 1);
        List<Point> VertexList = new List<Point>();
        Pgn NewPgn = new Pgn();
        int operation = 1; // Рисование
        int transformType = 0; // Перемещение
        bool checkPgn = false;
        Point pictureBox1MousePos = new Point();
        Brush Br = new SolidBrush(Color.Blue);

        public Form1()
        {
            InitializeComponent();
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(myBitmap);

            comboBox2.SelectedIndex = 0;
        }

        // заполнение списка вершин
        private void InputPgn(MouseEventArgs e)
        {
            Point NewP = new Point() { X = e.X, Y = e.Y };
            VertexList.Add(NewP);  //
            NewPgn.Add(NewP);
            int k = VertexList.Count();
            if (k > 1) g.DrawLine(DrawPen, VertexList[k - 2], VertexList[k - 1]);
            else g.DrawRectangle(DrawPen, e.X, e.Y, 1, 1);

            if (e.Button == MouseButtons.Right) // Конец ввода
            {
                g.DrawLine(new Pen(Color.Blue), VertexList[k - 1], VertexList[0]);
                VertexList.Clear();
                NewPgn.Fill(g, DrawPen);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            operation = 2;
        }

        // Обработчик события щелчка мыши
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1MousePos = e.Location;

            switch (operation)
            {
                case 1:   // Ввод вершин
                    {
                        InputPgn(e);
                        if (e.Button == MouseButtons.Right) operation = 0;
                    }
                    break;
                case 2:   // Выбор
                    if (NewPgn.ThisPgn(e.X, e.Y))
                    {
                        g.DrawEllipse(new Pen(Color.Blue), e.X - 2, e.Y - 2, 5, 5); ;
                        checkPgn = true;
                    }
                    else checkPgn = false;
                    break;
            }
            pictureBox1.Image = myBitmap;
        }

        // Обработчик события движения мыши
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && checkPgn)
            {
                // Calculate mouse movement delta
                int dx = e.X - pictureBox1MousePos.X;
                int dy = e.Y - pictureBox1MousePos.Y;

                switch (transformType)
                {
                    case 0: // Move (перемещение)
                        NewPgn.Move(dx, dy);
                        break;

                    case 1: // Rotate (вращение)
                            // Convert horizontal movement to rotation angle (1 pixel = 1 degree)
                        int rotationAngle = dx;
                        NewPgn.Rotate(rotationAngle, 0); // dy ignored for rotation
                        break;

                    case 2: // Scale (масштабирование)
                            // Convert movement to scaling factors (1 pixel = 1% scaling)
                        int scaleX = dx;
                        int scaleY = dy;
                        NewPgn.Scale(scaleX, scaleY);
                        break;
                }

                // Redraw the scene
                g.Clear(pictureBox1.BackColor);
                NewPgn.Fill(g, DrawPen);
                pictureBox1.Image = myBitmap;

                // Update mouse position
                pictureBox1MousePos = e.Location;
            }
        }


        // Обработчик события выбора цвета в элементе ComboBox cbLineColor
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)  // выбор цвета  
            {
                case 0: DrawPen.Color = Color.Black;
                    break;
                case 1: DrawPen.Color = Color.Red;
                    break;
                case 2: DrawPen.Color = Color.Green;
                    break;
                case 3: DrawPen.Color = Color.Blue;
                    break;
            }
        }

        // очистка
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = myBitmap;
            g.Clear(pictureBox1.BackColor);
            NewPgn.Clear();
            operation = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Выбор типа преобразования
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            transformType = comboBox2.SelectedIndex;
            // 0 - перемещение, 1 - вращение, 2 - масштабирование
        }
    }
}

