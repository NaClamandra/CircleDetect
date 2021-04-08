using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CircleDetect
{
    public class Grafo
    {
        Color White = Color.FromArgb(255, 255, 255, 255);
        List<Vertices> List_Vert;
        public Grafo()
        {
            this.List_Vert = new List<Vertices>();
        }
        public void añadirVert(Vertices vertice)
        {
            List_Vert.Add(vertice);
        }
        public class Arista
        {
            public Vertices sig;
            public Arista(Vertices vertice)
            {
                sig = vertice;
            }
        }
        public class Vertices
        {
            public List<Arista> ListAr;
            public Point punto;
            public Vertices(Point punto)
            {
                this.ListAr = new List<Arista>();
                this.punto = punto;
            }
            public void añadeArista(Vertices vertice)
            {
                this.ListAr.Add(new Arista(vertice));
            }
        }
        public  bool bresenham(Bitmap bresen, Circulo circ1, Circulo circ2)
        {
            Graphics g = Graphics.FromImage(bresen);
            SolidBrush white = new SolidBrush(White);
            Color pixel;
            g.FillEllipse(white, circ1.puntoC.X - circ1.radio - 2, circ1.puntoC.Y - circ1.radio - 1, circ1.radio * 2 + 4, circ1.radio * 2 + 4);
            g.FillEllipse(white, circ2.puntoC.X - circ2.radio - 2, circ2.puntoC.Y - circ2.radio - 1, circ2.radio * 2 + 4, circ2.radio * 2 + 4);
            int x0 = circ1.puntoC.X, y0 = circ1.puntoC.Y, x1 = circ2.puntoC.X, y1 = circ2.puntoC.Y;
            bool obstacle = false;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            while (true)
            {
                pixel = bresen.GetPixel(x0, y0);
                if (pixel.R != 255 || pixel.G != 255 || pixel.B != 255)
                {
                    obstacle = true;
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
            return obstacle;
        }
        public List<Vertices> calcularVertices(Bitmap pic, List<Circulo> Circulos)
        {
            List<Vertices> nwVert = new List<Vertices>();
            foreach (Circulo item in Circulos)
            {
                nwVert.Add(new Vertices(item.puntoC));
            }
            for (int i = 0; i < Circulos.Count; i++)
            {
                for (int j = i + 1; j < Circulos.Count; j++)
                {
                    if (!bresenham((Bitmap)pic.Clone(),Circulos[i],Circulos[j]))
                    {
                        nwVert[i].añadeArista(nwVert[j]);
                    }
                }
            }
            return nwVert;
        }
        public void addVertice(Vertices Vertic)
        {
            List_Vert.Add(Vertic);
        }

        public void mostrarGrafo(Bitmap clon)
        {
            Graphics g = Graphics.FromImage(clon);
            Pen p = new Pen(Color.Yellow, 4);
            foreach (Vertices item in List_Vert)
            {
                foreach (Arista item2 in item.ListAr)
                {
                    g.DrawLine(p, item.punto, item2.sig.punto);
                }
            }
        }
    }

}
