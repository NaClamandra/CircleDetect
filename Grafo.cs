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
        public int nSubGrafo = 1;
        public List<List<Vertices>> LSubGrafos;
        public Grafo()
        {
            this.List_Vert = new List<Vertices>();
            this.numVert = 1;
            this.nSubGrafo = 1;
        }
        public void añadirVert(Vertices vertice)
        {
            vertice.name = numVert.ToString();
            List_Vert.Add(vertice);
            foreach (Arista item in vertice.ListAr)
            {
                item.verId = vertice.name;
            }
            this.numVert += 1;
        }
        public class Arista
        {
            public Vertices sig;
            public string verId;
            public int peso;
            public List<Point> camino;
            public Arista(Vertices vertice, int peso, string id = "")
            {
                this.sig = vertice;
                this.peso = peso;
                this.verId = id;
            }
        }
        public void subGrafos()
        {
            this.nSubGrafo = 0;
            List<Vertices> verticeCola = new List<Vertices>();
            foreach (Vertices Vertice in List_Vert)
            {
                if (Vertice.grupo == 0)
                {
                    verticeCola.Add(Vertice);
                    this.nSubGrafo++;
                    Vertice.grupo = nSubGrafo;
                    Vertices padreVer = Vertice;

                    while (verticeCola.Count > 0)
                    {
                        if (verticeCola[0].grupo == 0)
                        {
                            verticeCola[0].grupo = nSubGrafo;
                        }
                        foreach (Arista arista in verticeCola[0].ListAr)
                        {
                            if (arista.sig.grupo == 0)
                            {
                                verticeCola.Add(arista.sig);
                            }
                        }
                        verticeCola.RemoveAt(0);
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
            foreach (var item3 in LSubGrafos)
            {
                foreach (var item2 in item3)
                {
                    hola += item2.name + ',';
                }
                hola += "\n";
            }
        }
        public class Vertices
        {
            public List<Arista> ListAr;
            public Point punto;
            public string name;
            public int radio;
            public double area;
            public int grupo;
            public Vertices(Point punto, int radio, double area, string id = "")
            {
                this.ListAr = new List<Arista>();
                this.grupo = 0;
                this.punto = punto;
                this.name = id;
                this.radio = radio;
                this.area = area;
            }
            public void añadeArista(Vertices vertice, int peso, List<Point> camino)
            {
                Arista nArista = new Arista(vertice, peso);
                nArista.camino = camino;
                this.ListAr.Add(nArista);
            }
        }
        public double Distancia(Point a, Point b)
        {
            float dX = b.X - a.X;
            float dY = b.Y - a.Y;
            return Math.Sqrt((dX * dX) + (dY * dY));
        }

        public List<Point> bresenham(Bitmap bresen, Grafo.Vertices circ1, Grafo.Vertices circ2, Grafo grf)
        {
            int peso = 0;
            List<Point> caminos = new List<Point>();
            Graphics g = Graphics.FromImage(bresen);
            SolidBrush white = new SolidBrush(White);
            SolidBrush black = new SolidBrush(Color.Black);
            Color pixel;
            g.FillEllipse(white, circ1.punto.X - circ1.radio - 2, circ1.punto.Y - circ1.radio - 1, circ1.radio * 2 + 4, circ1.radio * 2 + 4);
            g.FillEllipse(white, circ2.punto.X - circ2.radio - 2, circ2.punto.Y - circ2.radio - 1, circ2.radio * 2 + 4, circ2.radio * 2 + 4);
            int x0 = circ1.punto.X, y0 = circ1.punto.Y, x1 = circ2.punto.X, y1 = circ2.punto.Y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            while (true)
            {
                caminos.Add(new Point(x0, y0));
                pixel = bresen.GetPixel(x0, y0);
                if ((pixel.R != 255 || pixel.G != 255 || pixel.B != 255))
                {
                    caminos.Clear();
                    g.FillEllipse(black, circ1.punto.X - circ1.radio - 2, circ1.punto.Y - circ1.radio - 1, circ1.radio * 2 + 4, circ1.radio * 2 + 4);
                    g.FillEllipse(black, circ2.punto.X - circ2.radio - 2, circ2.punto.Y - circ2.radio - 1, circ2.radio * 2 + 4, circ2.radio * 2 + 4);
                    return caminos;
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
                peso++;
            }
            g.FillEllipse(black, circ1.punto.X - circ1.radio - 2, circ1.punto.Y - circ1.radio - 1, circ1.radio * 2 + 4, circ1.radio * 2 + 4);
            g.FillEllipse(black, circ2.punto.X - circ2.radio - 2, circ2.punto.Y - circ2.radio - 1, circ2.radio * 2 + 4, circ2.radio * 2 + 4);
            return caminos;
        }

        public List<Vertices> calcularVertices(Bitmap pic, Grafo grf, ProgressBar barra)
        {
            pic = (Bitmap)pic.Clone();
            List<Vertices> nwVert = grf.List_Vert;
            for (int i = 0; i < nwVert.Count; i++)
            {              
                for (int j = 0; j < nwVert.Count; j++)
                {
                    if (i != j)
                    {
                        int pesoAr;
                        List<Point> caminoAr;
                        caminoAr = bresenham((Bitmap)pic, nwVert[i], nwVert[j], grf);
                        if (caminoAr.Count > 0)
                        {
                            pesoAr = (int)Distancia(nwVert[j].punto, nwVert[i].punto);
                            nwVert[i].añadeArista(nwVert[j], pesoAr, caminoAr);
                        }
                    }
                }
                barra.PerformStep();
                barra.Refresh();
            }
            return nwVert;
        }
        public void mostrarGrafo(Bitmap clon, ProgressBar barra)
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
                    //g.DrawString(item2.peso.ToString(), FF, cola1, (item.punto.X+item2.sig.punto.X)/2, (item.punto.Y + item2.sig.punto.Y) / 2);
                }
                barra.PerformStep();
                barra.Refresh();
            }
            /*foreach (Vertices item in List_Vert)
            {
                SolidBrush drawBrush = new SolidBrush(Color.OrangeRed);
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                Font drawFont = new Font("Arial", 24);
                g.DrawString(item.name, drawFont, drawBrush, item.punto.X, item.punto.Y, drawFormat);
            }*/
            foreach (Vertices vert in List_Vert)
            {
                //g.DrawString("g:" + vert.grupo.ToString(), FF, cola1, vert.punto.X - 10, vert.punto.Y - 10);
            }
        }
        public Bitmap escalaImg(PictureBox picture, Bitmap img_C)
        {       
            if (img_C.Width > picture.Width || img_C.Height > picture.Height)
            {
                float scale;
                float ScaWi = (float)picture.Width / (float)img_C.Width;
                float ScaHe = (float)picture.Height / (float)img_C.Height;
                if (ScaWi < ScaHe)
                {
                    scale = ScaWi;
                }
                else
                {
                    scale = ScaHe;
                }
                var sW = (int)(img_C.Width * scale);
                var sH = (int)(img_C.Height * scale);
                img_C = new Bitmap(img_C, new Size(sW, sH));
            }
            return (Bitmap)img_C;
        }

        public static Bitmap scaleSize(Size taman, Image img_C)
        {
            if (img_C.Width > taman.Width || img_C.Height > taman.Height)
            {
                float scale;
                float ScaWi = (float)taman.Width / (float)img_C.Width;
                float ScaHe = (float)taman.Height / (float)img_C.Height;
                if (ScaWi < ScaHe)
                {
                    scale = ScaWi;
                }
                else
                {
                    scale = ScaHe;
                }
                var sW = (int)(img_C.Width * scale);
                var sH = (int)(img_C.Height * scale);
                img_C = new Bitmap(img_C, new Size(sW, sH));
            }
            return (Bitmap)img_C;
        }
    }
}
