using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeomComputTema3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Draws the minimum area circle enclosing a set of points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
            Graphics g = e.Graphics;
            Pen redPen = new Pen(Color.Red, 3);
            Pen bluePen = new Pen(Color.Blue, 2);
            
            List<Point> pointsList = new List<Point>();
            Random rnd = new Random();
            int pointsNr = rnd.Next(2, 80);
           float minCircleDiam = panel1.Height;
            float maxCircleDiam = panel1.Height;
            Point MD1 = new Point();   //MD1 and MD2 are two opposite points on the edge of the biggest circle
            MD1.X = 0;
            MD1.Y = panel1.Height / 2;
            Point MD2 = new Point();
            MD2.X = panel1.Height;
            MD2.Y = panel1.Height / 2;
            Point maxCircleCenter = Middle(MD1, MD2); //center of the biggest circle that fits in the panel
            Point minCircleCenter = Middle(MD1, MD2);  
            while (pointsNr > 0)
            {
                float x = rnd.Next(panel1.Height);
                float y = rnd.Next(panel1.Height);
                Point P = new Point();
                P.X = x;
                P.Y = y;
                if (Distance(P, maxCircleCenter) <= panel1.Height / 2)   //generate random points that fit inside 
                                                                         //the biggest circle
                {
                    g.DrawEllipse(redPen, x, y, 2, 2);

                    pointsList.Add(P);

                    pointsNr--;
                }
            }
           
            
            {
                for (int i = 0; i < pointsList.Count - 1; i++)    //go over all pairs of points that could
                                                                  //generate the minimal circle
                {
                    for (int j = i + 1; j < pointsList.Count; j++)
                    {
                        Point Center = Middle(pointsList[i], pointsList[j]);
                        float diameter = Distance(pointsList[i], pointsList[j]);
                       //  g.DrawEllipse(yellowPen, Center.X - diameter / 2, Center.Y - diameter / 2, diameter, diameter);
                       
                        if (ContainsAllPoints(pointsList, Center, diameter) && diameter < minCircleDiam)
                        {
                            minCircleDiam = diameter;
                            minCircleCenter = Center;
                           
                            //g.DrawEllipse(yellowPen, minCircleCenter.X - minCircleDiam / 2, minCircleCenter.Y - minCircleDiam / 2, minCircleDiam, minCircleDiam);
                        }
                    }
                }
                for (int i = 0; i < pointsList.Count - 2; i++)    //go over all combinations of three points that 
                {                                              //could generate the minimal circle
                    for (int j = i + 1; j < pointsList.Count - 1; j++)
                    {
                       
                        for (int k = j + 1; k < pointsList.Count; k++)
                        {
                            Point A = pointsList[i];
                            Point B = pointsList[j];
                            Point C = pointsList[k];

                            Point Center = CircleCenter(A, B, C);

                            float diameter = 2 * Distance(Center, A);
                          
                            if (ContainsAllPoints(pointsList, Center, diameter) && diameter < minCircleDiam)
                            {
                                minCircleDiam = diameter;
                                minCircleCenter = Center;

                             
                            }
                        }
                    }
                }
            }
            g.DrawEllipse(bluePen, minCircleCenter.X - minCircleDiam / 2, minCircleCenter.Y - minCircleDiam / 2, minCircleDiam, minCircleDiam);



        }
        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private static float Distance(Point A, Point B)
        {
            return (float)Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
        }
        /// <summary>
        /// Calculates the middle point between two other points
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private static Point Middle(Point A, Point B)
        {
           Point C = new Point();
            C.X = (A.X + B.X) / 2;
            C.Y = (A.Y + B.Y) / 2;
            return C;
        }
        /// <summary>
        /// Calculates the center of a circle when three points are given
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        private static Point CircleCenter(Point A, Point B, Point C)
        {
            float a = A.X * (B.Y - C.Y) - A.Y * (B.X - C.X) + B.X * C.Y - C.X * B.Y;
           float b = (A.X * A.X + A.Y * A.Y) * (C.Y - B.Y)
                    + (B.X * B.X + B.Y * B.Y) * (A.Y - C.Y)
                    + (C.X * C.X + C.Y * C.Y) * (B.Y - A.Y);
           float c = (A.X * A.X + A.Y * A.Y) * (B.X - C.X)
                    + (B.X * B.X + B.Y * B.Y) * (C.X - A.X)
                    + (C.X * C.X + C.Y * C.Y) * (A.X - B.X);
            Point Center = new Point();
            Center.X = (-b / (2 * a));
            Center.Y = (-c / (2 * a));
            return Center;
        }
        /// <summary>
        /// Checks if a given point is inside or on the edge of a given circle
        /// </summary>
        /// <param name="A"></param>
        /// <param name="Center"></param>
        /// <param name="diam"></param>
        /// <returns></returns>
        private static bool IsInsideCircle(Point A, Point Center, float diam )
        {
            return 2 * Distance(A, Center) <= diam ;
        }
        /// <summary>
        /// Checks if all points in a list are inside a given circle
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Center"></param>
        /// <param name="diam"></param>
        /// <returns></returns>
        private static bool ContainsAllPoints(List<Point> list, Point Center, float diam)
        {
            for (int i = 0; i < list.Count; i++)
            {
               if(IsInsideCircle(list[i], Center, diam) == false)
               {
                    return false;
               }
            }
            return true;
        }
    }
}
