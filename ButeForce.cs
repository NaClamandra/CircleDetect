using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CircleDetect
{
    public class ButeForce
    {
        public static List<Circulo> lista_p(List<Circulo> points)
        {
            List<Circulo> closerPoints = new List<Circulo>();
            if (points.Count<2)
            {
                return new List<Circulo>();
            }
            closerPoints.Add( points[0]);
            closerPoints.Add( points[1]);
            float minimum = distance(closerPoints[0].puntoC, closerPoints[1].puntoC);
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i+1; j < points.Count; j++)
                {
                    if (minimum>distance(points[i].puntoC,points[j].puntoC))
                    {
                        closerPoints[0] = points[i];
                        closerPoints[1] = points[j];
                        minimum = distance(points[i].puntoC, points[j].puntoC);
                    }
                }
            }
            return closerPoints;
        }
        public static float distance(Point po1, Point po2)
        {
            float resultadoX = po2.X - po1.X;
            float resultadoY = po2.Y - po1.Y;
            return resultadoX*resultadoX + resultadoY*resultadoY;
        }
    }
}
