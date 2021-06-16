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
            public Arista(Vertices vertice, int peso, string id="")
            {
                this.sig = vertice;
                this.peso = peso;
                this.verId = id;
            }
        }
        public void subGrafos()
        {
            this.nSubGrafo = 0;
            List<Vertices> verticeCola = new  List<Vertices>();
            foreach (Vertices Vertice in List_Vert)
            {
                if (Vertice.grupo == 0)
                {
                    verticeCola.Add(Vertice);
                    this.nSubGrafo++;
                    Vertice.grupo = nSubGrafo;
                    Vertices padreVer = Vertice;

                    while( verticeCola.Count > 0)
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
            public Vertices(Point punto,int radio, double area, string id="")
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
        public Arista MinArista(List<Arista> aristas)
        {
            Arista Min = aristas[0];
            foreach (Arista itemA in aristas)
            {
                if (Min.peso>itemA.peso)
                {
                    Min = itemA;
                }
            }
            return Min;
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
        public List<Point> bresenham(Bitmap bresen, Circulo circ1, Circulo circ2)
        {
            int peso = 0;
            List<Point> caminos = new List<Point>();
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
                caminos.Add(new Point(x0, y0));
                pixel = bresen.GetPixel(x0, y0);
                if (pixel.R != 255 || pixel.G != 255 || pixel.B != 255)
                {
                    caminos.Clear();
                    return caminos;
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
                peso++;
            }
            return caminos;
        }
        public double Distancia(Point a, Point b)
        {
            float dX = b.X - a.X;
            float dY = b.Y - a.Y;
            return Math.Sqrt((dX * dX) + (dY * dY));
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
                        int pesoAr;
                        List<Point> caminoAr;
                        caminoAr = bresenham((Bitmap)pic.Clone(), Circulos[i], Circulos[j]);
                        if (caminoAr.Count > 0)
                        {
                            pesoAr = (int)Distancia(nwVert[j].punto, nwVert[i].punto);
                            nwVert[i].añadeArista(nwVert[j], pesoAr, caminoAr);
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
                    //g.DrawString(item2.peso.ToString(), FF, cola1, (item.punto.X+item2.sig.punto.X)/2, (item.punto.Y + item2.sig.punto.Y) / 2);
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
            foreach (Vertices vert in List_Vert)
            {
                //g.DrawString("g:" + vert.grupo.ToString(), FF, cola1, vert.punto.X - 10, vert.punto.Y - 10);
            }
        }
        public static bool enArbol(List<ARM> arbol, Vertices vertice)
        {
            foreach (var item in arbol)
            {
                foreach (Vertices ver in item.vertices)
                {
                    if (ver.name == vertice.name)
                    {
                        return true;
                    }
                }
            } 
            return false;
        }
        public ARM Kruscal(ARM arm, List<Vertices> sGrafo)
        {
                List<Vertices> visitados = new List<Vertices>();
                Arista minAr;
                List<Arista> candidato = new List<Arista>();
                List<Arista> promete = new List<Arista>();
                List<string> visited = new List<string>();
                List<List<Vertices>> cc = new List<List<Vertices>>();
                foreach (Vertices vertice in sGrafo)
                {
                    var nwrVert = new Vertices(vertice.punto, vertice.radio, vertice.area, vertice.name);
                    nwrVert.grupo = vertice.grupo;
                    //var nuevoV = new Vertices(itemV.punto, itemV.radio, itemV.area, itemV.name);
                    arm.agregarVTree(nwrVert);
                    List<Vertices> conexo = new List<Vertices>();
                    conexo.Add(vertice);
                    cc.Add(conexo);
                    foreach (Arista itemA in vertice.ListAr)
                    {
                        if (!visitados.Contains(itemA.sig))
                        {
                            candidato.Add(itemA);
                        }
                    }
                    visitados.Add(vertice);
                }
                while (cc.Count != 1)
                {
                    minAr = MinArista(candidato);
                    var c_1 = findVerticeCC(cc, findVertList(sGrafo, minAr.verId));
                    var c_2 = findVerticeCC(cc, minAr.sig);
                    if (c_1 != c_2)
                    {
                        List<Vertices> newcc = new List<Vertices>();
                        c_1.AddRange(c_2);
                        cc.Remove(c_2);
                        promete.Add(minAr);
                        arm.encuentraV(minAr.verId).ListAr.Add(new Grafo.Arista(minAr.sig, minAr.peso));
                    }

                    candidato.Remove(minAr);
                }
                arm.ordenA = promete;
                return arm;

        }
        public static Vertices findVertList(List<Vertices>lista, string id)
        {
            foreach (var Vert in lista)
            {
                if (Vert.name==id)
                {
                    return Vert;
                }
            }
            return null;
        }
        public static List<Vertices> findVerticeCC(List<List<Vertices>> CCs, Vertices vertice)
        {
            foreach (List<Vertices> item in CCs)
            {
                foreach (Vertices itemV in item)
                {
                    if (itemV==vertice)
                    {
                        return item;
                    }
                }
            }
            return null;
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
    }
}
