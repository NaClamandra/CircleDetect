using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace CircleDetect
{
    class FuncionesDjstr
    {
        public static List<Dijstra> inicializarPesos(List<Grafo.Vertices> subgrafo, Grafo.Vertices Origen)
        {
            List<Dijstra> vector = new List<Dijstra>();
            foreach (Grafo.Vertices item in subgrafo)
            {
                Dijstra ED = new Dijstra();
                if (item == Origen)
                {
                    ED.verticeDij = item;
                    ED.esDefinitivo = false;
                    ED.VOrigen = Origen;
                    ED.PesoA = 0;
                }
                else
                {
                    ED.verticeDij = item;
                    ED.PesoA = -1;
                    ED.esDefinitivo = false;
                }
                vector.Add(ED);
            }
            return vector;
        }

        public static bool solucionDjst(List<Dijstra> vectorPesos)
        {
            bool solucion = true;
            foreach (Dijstra item in vectorPesos)
            {
                if (!item.esDefinitivo)
                {
                    solucion = false;
                    break;
                }
            }
            return solucion;
        }


        public static Grafo.Vertices seleccionaDef(List<Dijstra> vectorPesos, List<Grafo.Vertices> subgrafo)
        {
            Dijstra definitivo = vectorPesos[0];
            int i = 0;
            int j = 0;
            foreach (Dijstra item in vectorPesos)
            {
                if (((item.PesoA < definitivo.PesoA && item.PesoA!= -1)||(definitivo.PesoA == -1 && item.PesoA != -1 )|| definitivo.esDefinitivo==true) && item.esDefinitivo==false)
                {
                    definitivo = item;
                    i = j;
                }
                j++;
            }
            return subgrafo[i];
        }
        public static List<Dijstra> actualizaVD(List<Dijstra> vectorPesos, Grafo.Vertices vd, List<Grafo.Vertices> subgrafo)
        {
            List<Grafo.Arista> aristasDef = vd.ListAr;
            int i = 0;
            Dijstra elemDef = new Dijstra();
            foreach (Grafo.Vertices v in subgrafo)
            {
                if (v == vd)
                {
                    vectorPesos[i].esDefinitivo = true;
                    elemDef = vectorPesos[i];
                    if (vectorPesos[i].VOrigen==null)
                    {
                        vectorPesos[i].VOrigen = subgrafo[i];
                    }
                    break;
                }
                i++;
            }
            i = 0;
            foreach (Grafo.Vertices v in subgrafo)
            {
                if (v == vd)
                {
                    vectorPesos[i].esDefinitivo = true;
                }
                foreach (Grafo.Arista a in aristasDef)
                {
                    if (v == a.sig)
                    {
                        if (a.peso+ elemDef.PesoA < vectorPesos[i].PesoA || vectorPesos[i].PesoA == -1)
                        {
                            vectorPesos[i].PesoA = a.peso + elemDef.PesoA;
                            vectorPesos[i].VOrigen = vd;
                        }                 
                    }
                }
                i++;
            }
            return vectorPesos;
        }
        public static List<Dijstra> elementoDjistra(Grafo grafo, Grafo.Vertices verticeO)
        {
            Grafo.Vertices v_d;
            List<Grafo.Vertices> subgrafo = grafo.LSubGrafos[verticeO.grupo-1];
            List<Dijstra> vectorPesos = inicializarPesos(subgrafo, verticeO);
            while (!solucionDjst(vectorPesos))
            {
                v_d = seleccionaDef(vectorPesos,subgrafo);
                vectorPesos = actualizaVD(vectorPesos, v_d, subgrafo);
            }
            return vectorPesos;
        }

        public static List<Grafo.Vertices> obtenerVertices(List<Dijstra> solucion, Grafo.Vertices destino, Grafo.Vertices origen)
        {
            List<Grafo.Vertices> verticesCamino = new List<Grafo.Vertices>();         
            while (destino != origen)
            {
                foreach (Dijstra dj in solucion)
                {
                    if (dj.verticeDij == destino)
                    {
                        verticesCamino.Add(destino);
                        destino = dj.VOrigen;
                        break;
                    }
                }
            }
            verticesCamino.Add(origen);
            verticesCamino.Reverse();
            return verticesCamino;
        }

        public static void dibujaPart(Point punto, Image image,Image part)
        {
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(part, punto.X - (part.Width / 2 - 1), punto.Y - (part.Width / 2 - 1));
        }

        public static void dCamino(List<Grafo.Vertices> vCamino, Image img, Color color, int size)
        {
            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(color, size);
            for (int i = 0; i < vCamino.Count() - 1; i++)
            {
                g.DrawLine(p, vCamino[i].punto, vCamino[i + 1].punto);
            }
        }
    }
}
