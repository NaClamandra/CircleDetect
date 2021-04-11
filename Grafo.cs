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
        public int numVert;
        public Grafo()
        {
            this.List_Vert = new List<Vertices>();
            this.numVert = 1;
        }
        public void añadirVert(Vertices vertice)
        {
            vertice.name = numVert.ToString();
            List_Vert.Add(vertice);
            this.numVert += 1;
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
            public string name;
            public Vertices(Point punto)
            {
                this.ListAr = new List<Arista>();
                this.punto = punto;
                this.name = "";
            }
            public void añadeArista(Vertices vertice)
            {
                this.ListAr.Add(new Arista(vertice));
            }
        }
        public void matriz(DataGridView tablon)
        {
            int i = 0;
            foreach (Vertices item in this.List_Vert)
            {           
                tablon.Columns.Add(item.name, item.name);
                tablon.Columns[i].Width = 55;
                tablon.Rows.Add();
                tablon.Rows[i].HeaderCell.Value = item.name;
                i++;
            }
            i = 0;
            foreach (Vertices ver in this.List_Vert)
            {
                for (int j = 0; j < tablon.Columns.Count; j++)
                {
                    tablon.Rows[i].Cells[j].Value = 0;
                }
                foreach (Arista ari in ver.ListAr)
                {
                    tablon.Rows[i].Cells[tablon.Columns[ari.sig.name].Index].Value=1;
                }
                i++;
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
                for (int j = 0; j < Circulos.Count; j++)
                {
                    if (i!=j)
                    {
                        if (!bresenham((Bitmap)pic.Clone(), Circulos[i], Circulos[j]))
                        {
                            nwVert[i].añadeArista(nwVert[j]);
                        }
                    }
                }
            }
            return nwVert;
        }
        public void mostrarGrafo(Bitmap clon)
        {
            Graphics g = Graphics.FromImage(clon);
            Pen p = new Pen(Color.LightGreen, 4);
            foreach (Vertices item in List_Vert)
            {
                foreach (Arista item2 in item.ListAr)
                {
                    g.DrawLine(p, item.punto, item2.sig.punto);
                }             
            }
            foreach (Vertices item in List_Vert)
            {
                SolidBrush drawBrush = new SolidBrush(Color.OrangeRed);
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                Font drawFont = new Font("Arial", 24);
                g.DrawString(item.name, drawFont, drawBrush, item.punto.X, item.punto.Y, drawFormat);
            }
        }    
    }
}
