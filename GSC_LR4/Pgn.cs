using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using csMatrix;

namespace GSC_Lr4
{
    class Pgn
    {
        List<PointF> VertexList;

        float[,] I = { {1, 0, 0},
                       {0, 1, 0},
                       {0, 0, 1} }; // единичная матрица 

        public Pgn()
        {
            VertexList = new List<PointF>();
        }

        // метод Добавление вершины
        public void Add(Point NewVertex)
        {
            VertexList.Add(NewVertex);
        }

        // метод Вывод закрашенного многоугольника с помощью g.FillPolygon
        // Вместо него здесь должен быть свой метод закрашивания из л.р. № 2 !
        public void Fill(Graphics g, Pen DrawPen)
        {
            Brush DrawBrush = new SolidBrush(DrawPen.Color);

            int n = VertexList.Count() - 1;
            Point[] PgVertex = new Point[VertexList.Count()]; // массив вершин
            for (int i = 0; i <= n; i++)
            {
                PgVertex[i].X = (int)Math.Round(VertexList[i].X);
                PgVertex[i].Y = (int)Math.Round(VertexList[i].Y);
            }
            g.FillPolygon(DrawBrush, PgVertex);
        }

        // выделение многоугольника
        public bool ThisPgn(int mX, int mY)
        {
            int n = VertexList.Count() - 1, k = 0, m = 0;
            PointF Pi, Pk; 
            bool check = false;
            for (int i = 0; i <= n; i++)
            {
                if (i < n) k = i + 1; else k = 0;
                Pi = VertexList[i]; Pk = VertexList[k];
                if ((Pi.Y < mY) & (Pk.Y >= mY) | (Pi.Y >= mY) & (Pk.Y < mY))
                    if ((mY - Pi.Y) * (Pk.X - Pi.X) / (Pk.Y - Pi.Y) + Pi.X < mX) m++;
            }
            if (m % 2 == 1) check = true;
            return check;
        }

        // формирует матрицу-строку координат точки Vertex
        public float[] PointM(PointF Vertex)
        {
            float[] m3 = new float[3];
            m3[0] = Vertex.X; m3[1] = Vertex.Y; m3[2] = 1;

            return m3;
        }

        // формирует матрицу 3х3 масштабирования
        public float[,] ScaleM(float Sx, float Sy)
        {
            float[,] m33 = (float[,])I.Clone();
            m33[0, 0] = Sx; m33[1, 1] = Sy; 

            return m33;
        }

        // формирует матрицу 3х3 перемещения
        public float[,] MoveM(float Dx, float Dy)
        {
            float[,] m33 = (float[,]) I.Clone();
            m33[2, 0] = Dx; m33[2, 1] = Dy;

            return m33;
        }

        // умножение матрицы координат A на матрицу B преобразования
        public float[] Mul13(float[] A, float[,] B)
        {
            float[] m3 = new float[3];

            for (int i = 0; i < 3; i++) 
            {
                m3[i] = 0;
                for (int j = 0; j < 3; j++)
                    m3[i] += A[j] * B[j, i];
            }
            return m3;
        }

        // умножение матриц 3х3
        public float[,] Mul33(float[,] A, float[,] B)
        {
            float[,] m33 = new float[3, 3];

            for (int i = 0; i < 3; i++)     // строки А
                for (int j = 0; j < 3; j++) // столбцы В
                {
                    m33[i, j] = 0;
                    for (int k = 0; k < 3; k++) // строки В в столбце
                        m33[i, j] += A[i, k] * B[k, j];
                }
            return m33;
        }

        // плоско-параллельное перемещение в матричной форме
        public void Move(int dx, int dy)
        {
            int n = VertexList.Count() - 1;
            PointF fP = new PointF();
            for (int i = 0; i <= n; i++)
            {
                float[] C = PointM(VertexList[i]);

                float[,] M = MoveM(dx, dy);
                float[,] m33 = Mul33(I, M);

                float[] m3 = Mul13(C, m33);

                fP.X = m3[0];   fP.Y = m3[1];
                VertexList[i] = fP;
            }
        }

        public void Rotate(int dx, int dy)
        {
            int n = VertexList.Count() - 1;
            PointF fP = new PointF();

            // нахождение центра
            float centerX = 0, centerY = 0;
            foreach (PointF p in VertexList)
            {
                centerX += p.X;
                centerY += p.Y;
            }
            centerX /= (n + 1);
            centerY /= (n + 1);

            // обработка угла
            float angle = (float)(-dx * Math.PI / 180.0);
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            // создание матриц
            float[,] moveToOrigin = MoveM(-centerX, -centerY);
            float[,] rotate = new float[,] { { cos, -sin, 0 }, { sin, cos, 0 }, { 0, 0, 1 } };
            float[,] moveBack = MoveM(centerX, centerY);

            // комбинирование преобразований в центр, повернуть, обратно
            float[,] m33 = Mul33(moveToOrigin, rotate);
            m33 = Mul33(m33, moveBack);


            for (int i = 0; i <= n; i++)
            {
                float[] C = PointM(VertexList[i]);
                float[] m3 = Mul13(C, m33);
                fP.X = m3[0]; fP.Y = m3[1];
                VertexList[i] = fP;
            }
        }

        public void Scale(int dx, int dy)
        {
            int n = VertexList.Count() - 1;
            PointF fP = new PointF();

            // нахождение центра
            float centerX = 0, centerY = 0;
            foreach (PointF p in VertexList)
            {
                centerX += p.X;
                centerY += p.Y;
            }
            centerX /= (n + 1);
            centerY /= (n + 1);

            // нахождение факторов масштаба
            float sx = 1 + dx / 100f;
            float sy = 1 - dy / 100f;

            // создание матриц
            float[,] moveToOrigin = MoveM(-centerX, -centerY);
            float[,] scale = ScaleM(sx, sy);
            float[,] moveBack = MoveM(centerX, centerY);

            // комбинирование преобразований в центр, масштабировать, обратно
            float[,] m33 = Mul33(moveToOrigin, scale);
            m33 = Mul33(m33, moveBack);

            for (int i = 0; i <= n; i++)
            {
                float[] C = PointM(VertexList[i]);
                float[] m3 = Mul13(C, m33);
                fP.X = m3[0]; fP.Y = m3[1];
                VertexList[i] = fP;
            }
        }

        public void Clear()
        {
            VertexList.Clear();
        }

    }
}
