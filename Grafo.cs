using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CircleDetect
{
    public class Grafo
    {
        List<Vertices> List_Vert;

        public Grafo()
        {
            this.List_Vert = new List<Vertices>();

        }
        public void añadirVert(Vertices vertice)
        {
            List_Vert.Add(vertice);
        }
        public Bitmap mostrarGrafo(Bitmap img)
        {
            Graphics g = Graphics.FromImage(img);
            Pen yellowPen = new Pen(Color.Yellow, 10);
            foreach (Vertices item in List_Vert)
            {
                //g.DrawLine();   
            }
            return img;
        }
        class Arista
        {
            Vertices sig;
            public Arista(Vertices vertice)
            {
                sig = vertice;
            }

        }
        public class Vertices
        {
            List<Arista> ListAr;
            Point punto;
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
    }
    
}
