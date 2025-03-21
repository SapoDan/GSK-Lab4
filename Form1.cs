using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_2_Polygons
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen DrawPen = new Pen(Color.Black, 1);
        List<Point> VertexList = new List<Point>(); // Список точек для фигуры
        bool isDrawing = false;

        public Form1()
        {
            InitializeComponent();
            g = pictureBox.CreateGraphics(); // Инициализация графики
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            this.pictureBox.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            clearButton.Click += new EventHandler(clearButton_Click);
            fillStyleComboBox.SelectedIndex = 0;
            colorPickButton.BackColor = Color.Black;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            g = pictureBox.CreateGraphics(); //инициализация графики 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void colorPickButton_click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем выбранный цвет
                Color selectedColor = colorDialog1.Color;

                // Устанавливаем цвет кнопки выбора цвета
                colorPickButton.BackColor = selectedColor;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Draw(e);
            }
            else if (e.Button == MouseButtons.Right && isDrawing)
            {
                if (VertexList.Count == 1)
                {
                    Draw(e);
                }
                else if (VertexList.Count == 2)
                {
                    // Добавляем третью точку
                    VertexList.Add(new Point(e.X, e.Y));

                    // Рисуем третью точку
                    g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 2, 2);

                    // Рисуем линию между второй и третьей точками
                    g.DrawLine(DrawPen, VertexList[1], VertexList[2]);

                    // Замыкаем фигуру, соединяя третью точку с первой
                    g.DrawLine(DrawPen, VertexList[2], VertexList[0]);

                    if (fillStyleComboBox.SelectedIndex == 0)
                    {
                        FillPolygonStandard();
                    }
                    else if (fillStyleComboBox.SelectedIndex == 1)
                    {
                        FillPolygonOriented();
                    }
                    VertexList.Clear();
                    isDrawing = false;
                }
                else if (VertexList.Count > 2)
                {
                    // Замыкаем фигуру, соединяя последнюю точку с первой
                    g.DrawLine(DrawPen, VertexList[VertexList.Count - 1], VertexList[0]);

                    if (fillStyleComboBox.SelectedIndex == 0)
                    {
                        FillPolygonStandard();
                    }
                    else if (fillStyleComboBox.SelectedIndex == 1)
                    {
                        FillPolygonOriented();
                    }

                    // Очищаем список вершин и завершаем процесс рисования
                    VertexList.Clear();
                    isDrawing = false;
                }
            }
        }

        private void Draw(MouseEventArgs e)
        {
            // Добавляем новую вершину в список
            VertexList.Add(new Point(e.X, e.Y));

            // Рисуем вершину
            g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 2, 2);

            // Если вершин больше одной, рисуем линию между последней и предпоследней вершинами
            if (VertexList.Count > 1)
            {
                g.DrawLine(DrawPen, VertexList[VertexList.Count - 2], VertexList[VertexList.Count - 1]);
            }

            // Устанавливаем флаг, что идет процесс рисования
            isDrawing = true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Очищаем PictureBox
            g.Clear(Color.White);

            // Очищаем список вершин
            VertexList.Clear();

            // Сбрасываем флаг рисования
            isDrawing = false;
        }

        // Стандартный алгоритм закрашивания
        private void FillPolygonStandard()
        {
            int Ymin = VertexList.Min(p => p.Y);
            int Ymax = VertexList.Max(p => p.Y);

            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                List<int> Xb = new List<int>();

                for (int i = 0; i < VertexList.Count; i++)
                {
                    int k = (i < VertexList.Count - 1) ? i + 1 : 0;

                    int yi = VertexList[i].Y;
                    int yk = VertexList[k].Y;

                    if ((yi < Y && yk >= Y) || (yi >= Y && yk < Y))
                    {
                        double x = VertexList[i].X + (double)(Y - yi) / (yk - yi) * (VertexList[k].X - VertexList[i].X);
                        Xb.Add((int)Math.Round(x));
                    }
                }

                Xb.Sort();

                for (int j = 0; j < Xb.Count; j += 2)
                {
                    int xl = Xb[j];
                    int xr = Xb[j + 1];
                    g.DrawLine(new Pen(colorPickButton.BackColor), xl, Y, xr, Y);
                }
            }
        }

        // Ориентированный алгоритм закрашивания
        private void FillPolygonOriented()
        {
            // Проверяем, что в списке достаточно вершин
            if (VertexList.Count < 3)
            {
                MessageBox.Show("Для закрашивания нужно как минимум 3 вершины.");
                return;
            }

            // Определяем направление обхода (по часовой стрелке или против)
            bool isClockwise = IsPolygonClockwise();

            // Находим минимальную и максимальную координаты Y
            int yMin = VertexList.Min(p => p.Y);
            int yMax = VertexList.Max(p => p.Y);
            

            // Если обход по часовой стрелке, закрашиваем внешнюю область
            if (isClockwise)
            {
                FillExternalRegion(0, pictureBox.Height);
            }
            
            else // Иначе закрашиваем внутреннюю область
            {
                FillInternalRegion(yMin, yMax);
            }
        }

        // Закрашивание внешней области
        private void FillExternalRegion(int yMin, int yMax)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                List<int> xLeft = new List<int>(); // Левые границы
                List<int> xRight = new List<int>(); // Правые границы

                // Находим пересечения сторон с текущей строкой Y
                for (int i = 0; i < VertexList.Count; i++)
                {
                    int nextIndex = (i + 1) % VertexList.Count;

                    int yi = VertexList[i].Y;
                    int yk = VertexList[nextIndex].Y;

                    if ((yi < y && yk >= y) || (yi >= y && yk < y))
                    {
                        double x = CalculateIntersectionX(VertexList[i], VertexList[nextIndex], y);
                        if (yk - yi > 0) // Сторона направлена вверх
                        {
                            xRight.Add((int)Math.Round(x));
                        }
                        else // Сторона направлена вниз
                        {
                            xLeft.Add((int)Math.Round(x));
                        }
                    }
                }

                // Добавляем границы области вывода, если нужно
                if (xLeft.Count == 0 && xRight.Count == 0)
                {
                    // Если пересечений нет, закрашиваем всю строку
                    g.DrawLine(new Pen(colorPickButton.BackColor), 0, y, pictureBox.Width, y);
                }
                else
                {
                    // Сортируем списки границ
                    xLeft.Sort();
                    xRight.Sort();

                    // Добавляем левую и правую границы области вывода
                    xLeft.Insert(0, 0); // Левая граница
                    xRight.Add(pictureBox.Width); // Правая граница

                    // Закрашиваем сегменты
                    for (int j = 0; j < xLeft.Count; j++)
                    {
                        int xl = xLeft[j];
                        int xr = xRight[j];
                        if (xl < xr) // Проверка на корректность сегмента
                        {
                            g.DrawLine(new Pen(colorPickButton.BackColor), xl, y, xr, y);
                        }
                    }
                }
            }
        }

        // Закрашивание внутренней области
        private void FillInternalRegion(int yMin, int yMax)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                List<int> xIntersections = new List<int>();

                // Находим пересечения сторон с текущей строкой Y
                for (int i = 0; i < VertexList.Count; i++)
                {
                    int nextIndex = (i + 1) % VertexList.Count;

                    int yi = VertexList[i].Y;
                    int yk = VertexList[nextIndex].Y;

                    if ((yi < y && yk >= y) || (yi >= y && yk < y))
                    {
                        double x = CalculateIntersectionX(VertexList[i], VertexList[nextIndex], y);
                        xIntersections.Add((int)Math.Round(x));
                    }
                }

                // Сортируем пересечения
                xIntersections.Sort();

                // Закрашиваем сегменты между парами пересечений
                for (int j = 0; j < xIntersections.Count; j += 2)
                {
                    if (j + 1 < xIntersections.Count)
                    {
                        int xl = xIntersections[j];
                        int xr = xIntersections[j + 1];
                        g.DrawLine(new Pen(colorPickButton.BackColor), xl, y, xr, y);
                    }
                }
            }
        }

        // Определяет, обходятся ли вершины по часовой стрелке
        private bool IsPolygonClockwise()
        {
            double area = 0;
            for (int i = 0; i < VertexList.Count; i++)
            {
                int nextIndex = (i + 1) % VertexList.Count;
                area += (VertexList[nextIndex].X - VertexList[i].X) * (VertexList[nextIndex].Y + VertexList[i].Y);
            }
            return area > 0; // Если площадь положительная, обход по часовой стрелке
        }

        // Вычисляет координату X пересечения стороны с горизонтальной линией Y
        private int CalculateIntersectionX(Point p1, Point p2, int y)
        {
            return p1.X + (p2.X - p1.X) * (y - p1.Y) / (p2.Y - p1.Y);
        }

        // Закрашивает горизонтальную область между yStart и yEnd
        private void FillHorizontalRegion(int yStart, int yEnd)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                g.DrawLine(new Pen(colorPickButton.BackColor), 0, y, pictureBox.Width, y);
            }
        }
    }
}