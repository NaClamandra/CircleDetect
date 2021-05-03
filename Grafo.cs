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
        public List<Vertices> List_Vert;
        public int numVert;
        public int nSubGrafo=1;
        public List<List<Vertices>> LSubGrafos;
        public Grafo()
        {
            this.List_Vert = new List<Vertices>();
            this.numVert = 1;
            this.nSubGrafo = 0;
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
            public int peso;
            public Arista(Vertices vertice, int peso)
            {
                this.sig = vertice;
                this.peso = peso;
            }
        }
        public void subGrafos()
        {
            this.nSubGrafo = 0;
            foreach (Vertices Vertice in List_Vert)
            {
                List<Vertices> subgrafo = new List<Vertices>(); ;
                if (Vertice.grupo == 0)
                {
                    this.nSubGrafo++; ;
                    Vertice.grupo = nSubGrafo;
                    foreach (Arista arista in Vertice.ListAr)
                    {
                        arista.sig.grupo = Vertice.grupo;
                        subgrafo.Add(arista.sig);
                    }
                }
                else
                {
                    foreach (Arista arista in Vertice.ListAr)
                    {
                        if (arista.sig.grupo == 0)
                        {
                            arista.sig.grupo = Vertice.grupo;
                        }
                    }
                }
            }
            LSubGrafos = new List<List<Vertices>>();
            for (int i = 0; i < nSubGrafo; i++)
            {
                LSubGrafos.Add(new List<Vertices>());
            }
            foreach (Vertices item in List_Vert)
            {
                LSubGrafos[item.grupo - 1].Add(item);
            }
            String hola = "";
            foreach (var item in LSubGrafos)
            {
                foreach (var item2 in item)
                {
                    hola += item2.name + ',';
                }
                hola += "\n";
            }
            MessageBox.Show(hola);
        }
        public class Vertices
        {
            public List<Arista> ListAr;
            public Point punto;
            public string name;
            public int radio;
            public double area;
            public int grupo;
            public Vertices(Point punto,int radio, double area, string id="")
            {
                this.ListAr = new List<Arista>();
                this.grupo = 0;
                this.punto = punto;
                this.name = id;
                this.radio = radio;
                this.area = area;
            }
            public void añadeArista(Vertices vertice, int peso)
            {
                this.ListAr.Add(new Arista(vertice,peso));
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
                    tablon.Rows[i].Cells[tablon.Columns[ari.sig.name].Index].Value = 1;
                }
                i++;
            }
        }
        public int bresenham(Bitmap bresen, Circulo circ1, Circulo circ2)
        {
            int peso = 0;
            Graphics g = Graphics.FromImage(bresen);
            SolidBrush white = new SolidBrush(White);
            Color pixel;
            g.FillEllipse(white, circ1.puntoC.X - circ1.radio - 2, circ1.puntoC.Y - circ1.radio - 1, circ1.radio * 2 + 4, circ1.radio * 2 + 4);
            g.FillEllipse(white, circ2.puntoC.X - circ2.radio - 2, circ2.puntoC.Y - circ2.radio - 1, circ2.radio * 2 + 4, circ2.radio * 2 + 4);
            int x0 = circ1.puntoC.X, y0 = circ1.puntoC.Y, x1 = circ2.puntoC.X, y1 = circ2.puntoC.Y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            while (true)
            {
                pixel = bresen.GetPixel(x0, y0);
                if (pixel.R != 255 || pixel.G != 255 || pixel.B != 255)
                {
                    return -1;
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
                peso++;
            }
            return peso;
        }
        public List<Vertices> calcularVertices(Bitmap pic, List<Circulo> Circulos)
        {
            List<Vertices> nwVert = new List<Vertices>();
            foreach (Circulo item in Circulos)
            {
                nwVert.Add(new Vertices(item.puntoC, item.radio, item.area));
            }
            for (int i = 0; i < Circulos.Count; i++)
            {
                for (int j = 0; j < Circulos.Count; j++)
                {
                    if (i != j)
                    {
                        int pesoAr = bresenham((Bitmap)pic.Clone(), Circulos[i], Circulos[j]);
                        if (pesoAr>=0)
                        {
                            nwVert[i].añadeArista(nwVert[j], pesoAr);
                        }
                    }
                }
            }
            return nwVert;
        }
        public void mostrarGrafo(Bitmap clon)
        {
            SolidBrush cola1 = new SolidBrush(Color.Red);
            StringFormat cola = new StringFormat();
            Font FF = new Font("Arial", 12);
            Graphics g = Graphics.FromImage(clon);
            Pen p = new Pen(Color.LightGreen, 4);
            foreach (Vertices item in List_Vert)
            {
                foreach (Arista item2 in item.ListAr)
                {
                    g.DrawLine(p, item.punto, item2.sig.punto);
                    g.DrawString(item2.peso.ToString(), FF, cola1, (item.punto.X+item2.sig.punto.X)/2, (item.punto.Y + item2.sig.punto.Y) / 2);
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
        /*public List<Arista> ObtenerArista()
        {
            foreach (Vertices item in List_Vert)
            {
                foreach (Arista itemA in List_Vert)
                {

                }
            }
        }*/
    }
}
