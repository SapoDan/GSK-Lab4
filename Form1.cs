using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Lab_3
{
    public partial class Form1 : Form
    {
        // Класс для хранения данных о полигоне
        public class PolygonShape
        {
            public List<Point> Vertices { get; set; } // Список вершин
            public Color FillColor { get; set; }       // Цвет заливки
            public Color ContourColor { get; set; }     // Цвет контура
            public int FillType { get; set; }           // Тип заливки (в данной версии не используется)
            public Bitmap RenderedImage { get; set; }   // Растеризованное изображение полигона

            public PolygonShape(List<Point> vertices, Color fill, Color contour, int type)
            {
                Vertices = vertices ?? new List<Point>();
                FillColor = fill;
                ContourColor = contour;
                FillType = type; // В данной реализации всегда 0 (четный-нечетный)
                RenderedImage = null;
            }
        }

        // Основные переменные приложения
        private List<PolygonShape> polygons = new List<PolygonShape>(); // Список созданных полигонов
        private List<Point> currentPolygon = new List<Point>(); // Текущий рисуемый полигон
        private int selectedTMO = 1; // Выбранная топологическая операция
        private Bitmap resultBmp;     // Результирующее изображение операции
        private bool operationApplied = false; // Флаг применения операции

        public Form1()
        {
            InitializeComponent();
            comboBoxTMO.SelectedIndex = 0; // Инициализация выпадающего списка операций
        }

        // Обработчик отрисовки PictureBox
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Отрисовка всех сохраненных полигонов
            foreach (var polygon in polygons)
            {
                if (polygon.RenderedImage != null)
                {
                    e.Graphics.DrawImage(polygon.RenderedImage, 0, 0);
                }
            }

            // Отрисовка результата операции (если применена)
            if (resultBmp != null && operationApplied)
            {
                e.Graphics.DrawImage(resultBmp, 0, 0);
            }

            // Отрисовка текущего полигона (в процессе рисования)
            if (currentPolygon.Count > 0)
            {
                Color contourColor = polygons.Count == 0 ? Color.Blue : Color.Red;
                DrawCurrentPolygon(e.Graphics, contourColor);
            }
        }

        // Обработчик кликов мыши
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Добавление вершины
            {
                currentPolygon.Add(e.Location);
                pictureBox1.Invalidate();
            }
            else if (e.Button == MouseButtons.Right && currentPolygon.Count > 2) // Завершение полигона
            {
                CompleteCurrentPolygon();
                operationApplied = false;
            }
        }

        // Завершение создания полигона
        private void CompleteCurrentPolygon()
        {
            if (currentPolygon.Count < 3) return; // Нужно минимум 3 точки

            // Определение цветов для первого и последующих полигонов
            Color fillColor = polygons.Count == 0 ? Color.Blue : Color.Red;
            Color contourColor = polygons.Count == 0 ? Color.Blue : Color.Red;

            // Создание Bitmap для растеризации полигона
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                
                DrawAndFillPolygon(gBmp, currentPolygon, fillColor, contourColor);
            }

            // Сохранение полигона
            polygons.Add(new PolygonShape(
                new List<Point>(currentPolygon),
                fillColor,
                contourColor,
                0 
            )
            { RenderedImage = bmp });

            currentPolygon.Clear();
            pictureBox1.Invalidate();
        }

        // Применение топологической операции
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (polygons.Count < 2) return; // Нужно два полигона
            
            var polygonA = polygons[0];
            var polygonB = polygons[1];


            // Создание результирующего изображения
            resultBmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics gResult = Graphics.FromImage(resultBmp))
            {
                gResult.Clear(Color.White);

                // Определение рабочей области Y
                int ymin = Math.Max(0, Math.Min(
                    polygonA.Vertices.Min(p => p.Y),
                    polygonB.Vertices.Min(p => p.Y)));

                int ymax = Math.Min(pictureBox1.Height - 1, Math.Max(
                    polygonA.Vertices.Max(p => p.Y),
                    polygonB.Vertices.Max(p => p.Y)));

                // Обработка каждой строки сканирования
                using (Pen resultPen = new Pen(Color.Black))
                {
                    for (int y = ymin; y <= ymax; y++)
                    {
                        // Получение пересечений для обоих полигонов
                        List<int> Xa = GetIntersections(polygonA.Vertices, y);
                        List<int> Xb = GetIntersections(polygonB.Vertices, y);

                        // Применение топологической операции
                        List<int> Xr = ApplyTMOToSections(Xa, Xb, selectedTMO);

                        // Отрисовка результирующих отрезков
                        for (int i = 0; i < Xr.Count; i += 2)
                        {
                            if (i + 1 < Xr.Count)
                            {
                                int xStart = Math.Max(0, Xr[i]);
                                int xEnd = Math.Min(pictureBox1.Width, Xr[i + 1]);

                                if (xEnd > xStart)
                                {
                                    gResult.DrawLine(resultPen, xStart, y, xEnd - 1, y);
                                }
                            }
                        }
                    }
                }
            }

            operationApplied = true;
            pictureBox1.Invalidate();
        }

        // Получение пересечений полигона с горизонтальной линией Y
        private List<int> GetIntersections(List<Point> vertices, int y)
        {
            List<int> intersections = new List<int>();
            if (vertices.Count < 3) return intersections;

            for (int i = 0; i < vertices.Count; i++)
            {
                int j = (i + 1) % vertices.Count;
                Point p1 = vertices[i];
                Point p2 = vertices[j];

                // Проверка пересечения ребра с линией Y
                if ((p1.Y <= y && p2.Y > y) || (p1.Y > y && p2.Y <= y))
                {
                    if (p2.Y != p1.Y) // Исключение горизонтальных ребер
                    {
                        // Линейная интерполяция
                        float t = (y - p1.Y) / (float)(p2.Y - p1.Y);
                        int x = (int)(p1.X + t * (p2.X - p1.X));
                        intersections.Add(x);
                    }
                }
            }

            intersections.Sort(); // Сортировка для парной обработки
            return intersections;
        }

        // Применение топологической операции к отрезкам
        private List<int> ApplyTMOToSections(List<int> Xa, List<int> Xb, int tmo)
        {
            // Определение правил для операций
            HashSet<int> SetQ = new HashSet<int>();
            switch (tmo)
            {
                case 1: SetQ.UnionWith(new[] { 1, 2, 3 }); break; // Объединение
                case 2: SetQ.Add(3); break;                      // Пересечение
                case 3: SetQ.UnionWith(new[] { 1, 2 }); break;  // Симметрическая разность
                case 4: SetQ.Add(2); break;                      // Разность A \ B
                case 5: SetQ.Add(1); break;                       // Разность B \ A
            }

            // Создание объединенного списка событий
            List<(int X, int DQ)> M = new List<(int, int)>();

            // Добавление пересечений полигона A (вес 2/-2)
            for (int i = 0; i < Xa.Count; i++)
                M.Add((Xa[i], (i % 2 == 0) ? 2 : -2));

            // Добавление пересечений полигона B (вес 1/-1)
            for (int i = 0; i < Xb.Count; i++)
                M.Add((Xb[i], (i % 2 == 0) ? 1 : -1));

            M.Sort((a, b) => a.X.CompareTo(b.X)); // Сортировка по X

            int Q = 0; // Текущее состояние
            List<int> Xr = new List<int>(); // Результирующие точки

            // Обработка событий
            for (int i = 0; i < M.Count;)
            {
                int currentX = M[i].X;
                int totalDQ = 0;

                // Суммирование всех событий в текущей X позиции
                while (i < M.Count && M[i].X == currentX)
                {
                    totalDQ += M[i].DQ;
                    i++;
                }

                int Qnew = Q + totalDQ;

                // Проверка изменения состояния
                bool stateChanged = SetQ.Contains(Q) != SetQ.Contains(Qnew);

                if (stateChanged)
                {
                    Xr.Add(currentX);
                }

                Q = Qnew;
            }

            return Xr;
        }

        // Отрисовка и заливка полигона
        private void DrawAndFillPolygon(Graphics g, List<Point> vertices, Color fillColor, Color contourColor)
        {
            if (vertices.Count < 3) return;

            // Определение диапазона Y
            int ymin = vertices.Min(p => p.Y);
            int ymax = vertices.Max(p => p.Y);

            // Заливка полигона
            using (Pen fillPen = new Pen(fillColor))
            {
                for (int y = ymin; y <= ymax; y++)
                {
                    List<int> intersections = GetIntersections(vertices, y);

                    // Отрисовка горизонтальных отрезков между парами точек
                    for (int i = 0; i < intersections.Count; i += 2)
                    {
                        if (i + 1 >= intersections.Count) break;

                        int x1 = Math.Max(0, intersections[i]);
                        int x2 = Math.Min(pictureBox1.Width, intersections[i + 1]);

                        if (x2 > x1)
                        {
                            g.DrawLine(fillPen, x1, y, x2 - 1, y);
                        }
                    }
                }
            }

            // Отрисовка контура
            }

        // Отрисовка текущего полигона (в процессе создания)
        private void DrawCurrentPolygon(Graphics g, Color color)
        {
            if (currentPolygon.Count < 2) return;

            using (Pen contourPen = new Pen(color, 2))
            {
                // Линии между вершинами
                g.DrawLines(contourPen, currentPolygon.ToArray());

                // Маркеры вершин
                using (Brush markerBrush = new SolidBrush(color))
                {
                    foreach (Point p in currentPolygon)
                    {
                        g.FillEllipse(markerBrush, p.X - 3, p.Y - 3, 7, 7);
                    }
                } 
            }
            
        }

        // Обработчик изменения выбора операции
        private void ComboBoxTMO_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTMO = comboBoxTMO.SelectedIndex + 1; // Индексы 0-4 -> операции 1-5
            operationApplied = false;
            pictureBox1.Invalidate();
        }

        // Очистка всех данных
        private void buttonClear_Click(object sender, EventArgs e)
        {
            // Освобождение ресурсов
            foreach (var p in polygons)
                p.RenderedImage?.Dispose();

            polygons.Clear();
            currentPolygon.Clear();
            resultBmp?.Dispose();
            resultBmp = null;
            operationApplied = false;
            pictureBox1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}