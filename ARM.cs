using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CircleDetect
{
    public class ARM
    {

        public List<Grafo.Vertices> vertices;
        public int pesoT;
        public List<Grafo.Arista> ordenA;
        public ARM(List<Grafo.Vertices> vertices=null)
        {
            if (vertices==null)
            {
                this.vertices = new List<Grafo.Vertices>();
            }
            this.pesoT = 0;
            ordenA = new List<Grafo.Arista>();
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
        public void treePeso()
        {
            int pesoTo = 0;
            foreach (var arista in ordenA)
            {
                pesoTo += arista.peso;
            }
            this.pesoT = pesoTo;
        }
        public override string ToString()
        {
            return String.Format("A:{0} Peso = {1}", vertices[0].grupo.ToString(), pesoT.ToString());
        }
        public void mostrarTree(Bitmap clon)
        {
            SolidBrush cola1 = new SolidBrush(Color.Red);
            StringFormat cola = new StringFormat();
            Font FF = new Font("Arial", 14);
            Graphics g = Graphics.FromImage(clon);
            Pen p = new Pen(Color.Blue, 4);
            foreach (Grafo.Vertices item in vertices)
            {
                foreach (Grafo.Arista item2 in item.ListAr)
                {
                    g.DrawLine(p, item.punto, item2.sig.punto);
                    g.DrawString(item2.peso.ToString(), FF, cola1, (item.punto.X + item2.sig.punto.X) / 2 + 10, (item.punto.Y + item2.sig.punto.Y) / 2 +10);
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
                g.DrawString("A:" + vert.grupo.ToString(), FF, cola1, vert.punto.X - 10, vert.punto.Y - 10);
            }

            int i = 0;
            foreach (Grafo.Arista aristaO in ordenA)
            {
                Grafo.Vertices item2 = this.encuentraV(aristaO.verId);
                g.DrawString(i.ToString(), FF, cola1, (item2.punto.X + aristaO.sig.punto.X) /2 -20, (item2.punto.Y + aristaO.sig.punto.Y)/2 - 20);
                i++;
            }
        }

    }
}
