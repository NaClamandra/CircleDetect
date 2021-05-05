using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CircleDetect
{
    public class ARM
    {
        List<Grafo.Vertices> vertices;
        int pesoT;
        public ARM(List<Grafo.Vertices> vertices=null)
        {
            if (vertices==null)
            {
                this.vertices = new List<Grafo.Vertices>();
            }
            this.pesoT = 0;
        }
        public void agregarVTree(Grafo.Vertices vertice)
        {
            vertices.Add(vertice);
        }
        public Grafo.Vertices encuentraV(string id)
        {
            foreach (Grafo.Vertices item in vertices)
            {
                if (item.name == id)
                {
                    return item;
                }
            }
            return null;
        }
        public void mostrarTree(Bitmap clon)
        {
            SolidBrush cola1 = new SolidBrush(Color.Red);
            StringFormat cola = new StringFormat();
            Font FF = new Font("Arial", 12);
            Graphics g = Graphics.FromImage(clon);
            Pen p = new Pen(Color.Blue, 4);
            foreach (Grafo.Vertices item in vertices)
            {
                foreach (Grafo.Arista item2 in item.ListAr)
                {
                    g.DrawLine(p, item.punto, item2.sig.punto);
                    g.DrawString(item2.peso.ToString(), FF, cola1, (item.punto.X + item2.sig.punto.X) / 2, (item.punto.Y + item2.sig.punto.Y) / 2);
                }
            }
            foreach (Grafo.Vertices item in vertices)
            {
                SolidBrush drawBrush = new SolidBrush(Color.Green);
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                Font drawFont = new Font("Arial", 24);
                g.DrawString(item.name, drawFont, drawBrush, item.punto.X, item.punto.Y, drawFormat);
            }
            foreach (Grafo.Vertices vert in vertices)
            {
                g.DrawString("g:" + vert.grupo.ToString(), FF, cola1, vert.punto.X - 10, vert.punto.Y - 10);
            }
        }

    }
}
