﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircleDetect
{
    public partial class Form_Principal : Form
    {
        Color Black = Color.FromArgb(255, 0, 0, 0);
        Color White = Color.FromArgb(255, 255, 255, 255);
        int circulo = 0;
        int blanco = 1; 
        Grafo grafo = new Grafo();
        List<ARM> aPrim;
        List<ARM> aKruskal;
        ARM KruskalPulsado;
        ARM PrimPulsado;
        Bitmap copia;
        Bitmap prim;
        Bitmap kruskal;
        List<Circulo> Circulos = new List<Circulo>();
        List<Point> puntC = new List<Point>();
        List<Circulo> masCercanos = new List<Circulo>();
        bool eGrafo;
        public Form_Principal()
        {
            InitializeComponent();
        }
        public bool Blanco(Color color)
        {
            if (color.R >= 255 - blanco && color.G >= 255 -blanco && color.B >= 255-blanco)
            {
                return true;
            }
            return false;
        }
        public void DetC(int j, int i, Bitmap img)
        {
            int pixelCount = 0, mitad, centro;

            while (!Blanco(img.GetPixel(j, i)) && j!=img.Width-1)
            {
                pixelCount += 1;
                j++;

            }
            if (pixelCount % 2 != 0 && pixelCount != 0)
            {
                pixelCount = ((pixelCount - 1) / 2) + 1;
                mitad = j - pixelCount;
            }
            else
            {
                pixelCount = pixelCount / 2;
                mitad = j - pixelCount - 1;
            }
            pixelCount = 0;
            while (!Blanco(img.GetPixel(mitad, i)) && i != img.Height-1)
            {
                pixelCount += 1;
                i++;
            }
            if (pixelCount % 2 != 0 && pixelCount != 0)
            {
                pixelCount = ((pixelCount - 1) / 2) + 1;
                centro = i - pixelCount;
            }
            else
            {
                pixelCount = pixelCount / 2;
                centro = i - pixelCount - 1;
            }
            Graphics g = Graphics.FromImage(img);
            Graphics h = Graphics.FromImage(copia);
            Pen otherPen = new Pen(Color.Blue,7);
            SolidBrush white = new SolidBrush(White);
            g.FillEllipse(white, mitad - pixelCount-2, centro - pixelCount-1, pixelCount * 2+4, pixelCount * 2+4);
            circulo += 1;
            double area = (3.1416 * (pixelCount * pixelCount));
            Circulo circo = new Circulo(circulo,mitad,centro,pixelCount,area);
            puntC.Add(circo.puntoC);
            Circulos.Add(circo);
            pictureBox1.Image = copia;
        }
        public bool FindCirc(Bitmap img, int inicioy=0)
        {
            int h = img.Height;
            bool buscar=true;
            int w = img.Width;
            Color bit;
            for (int i = inicioy; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    bit = img.GetPixel(j, i);
                    if (buscar && bit.R == bit.G && bit.B == bit.R && bit.R < 255-blanco)
                    {
                        DetC(j, i, img);
                        return FindCirc(img,i);      
                    }
                }
            }
            return false;
        }
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            grafo = new Grafo();
            eGrafo = false;
            pictureBox1.Size = new Size(1123,794);
            var ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen|*.jpg;*.png";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                button1.Enabled = false;
                button3.Enabled = false;
                label3.Text = "";
                label6.Text = "";
                sGrafos.Text = "";
                /*b_prim.Enabled = false;
                b_grafo.Enabled = false;
                b_kruskal.Enabled = false;
                Kruskaln.Text = "";
                sGrafos.Text = "";
                Prim_n.Text = "";
                listBox2.DataSource = null;
                listBox3.DataSource = null;*/
                var img_C = new Bitmap(Image.FromFile(ofd.FileName));
                img_C = grafo.escalaImg(pictureBox1, img_C);
                pictureBox1.Image = img_C;
                copia = (Bitmap)img_C;
                kruskal = (Bitmap)copia.Clone();
                prim = (Bitmap)copia.Clone();
                button2.Enabled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            aKruskal = new List<ARM>();
            aPrim = new List<ARM>();
            //dataGridView1.Columns.Clear();
            var img = (Bitmap)pictureBox1.Image;
            copia = (Bitmap)img.Clone();
            Bitmap senal = new Bitmap(800, 200);;
            FindCirc((Bitmap)pictureBox1.Image);
            circulo = 0;
            listBox1.DataSource = null;
            listBox1.DataSource = Circulos;
            Pen otherPen = new Pen(Color.Yellow, 50);
            Graphics h = Graphics.FromImage(copia);
            SolidBrush gray = new SolidBrush(Color.LightGray);
            SolidBrush grn = new SolidBrush(Color.LightGreen);
            SolidBrush blk = new SolidBrush(Color.Black);
            foreach (Grafo.Vertices item in grafo.calcularVertices(copia, Circulos))
            {
                grafo.añadirVert(item);
               
                
            }  
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            Font drawFont = new Font("Arial", 15);
            eGrafo = true;


            grafo.subGrafos();
            grafo.mostrarGrafo(copia);
            //grafo.matriz(dataGridView1);

            foreach (var subGraph in grafo.LSubGrafos)
            {
                var nwKruskal = new ARM();
                aKruskal.Add(grafo.Kruscal(nwKruskal, subGraph));             
            }

            foreach (var a_kruskal in aKruskal)
            {
                a_kruskal.mostrarTree(kruskal);
                a_kruskal.treePeso();
            }

            foreach (Circulo item in Circulos)
            {
                grafo.añadirVert(new Grafo.Vertices(item.puntoC, item.radio, item.area));
            }
            var grafoP = grafo;
            button2.Enabled = false;
            //Kruskaln.Text = grafo.LSubGrafos.Count.ToString();
            sGrafos.Text = grafo.LSubGrafos.Count.ToString();

            /*listBox2.DataSource = null;
            listBox2.DataSource = aKruskal;*/

            Circulos.Clear();
            puntC.Clear();
            masCercanos.Clear();
            /*b_grafo.Enabled = true;
            b_kruskal.Enabled = true;
            b_prim.Enabled = true;*/
        }
        public Grafo.Vertices Pertenece(int x, int y, Bitmap im)
        {
            Grafo.Vertices vertEn = null;
            foreach (Grafo.Vertices item in grafo.List_Vert)
            {
                if(grafo.Distancia(new Point(x, y), item.punto)-item.radio<0)
                {
                    return item;
                }
            }
            return vertEn;
        }

        //Click izquierdo
        Grafo.Vertices VOrigen = null;
        Grafo.Vertices VDestino = null;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (eGrafo==true)
            {         
                Color pixel = copia.GetPixel(e.X, e.Y);
                Grafo.Vertices VertClk = Pertenece(e.X, e.Y, copia);

                //List<Tuple<String, Grafo.Arista>> tuplaAr = new List<Tuple<string, Grafo.Arista>>();
                ARM nwprim = new ARM();
                if (VertClk!=null && !Grafo.enArbol(aPrim,VertClk))
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            {
                                label3.Text = VertClk.name;
                                VOrigen = VertClk;
                            }
                            break;
                        case MouseButtons.Right:
                            {
                                label6.Text = VertClk.name;
                                VDestino = VertClk;
                            }
                            break;
                        default:
                            break;
                    }
                    if ((VDestino!=null&&VOrigen!=null)&&(VDestino.grupo != VOrigen.grupo))
                    {
                        button1.Enabled = false;
                        button3.Enabled = false;
                    }
                    else if (VDestino != null && VOrigen != null)
                    {
                        button1.Enabled = true;
                        button3.Enabled = true;
                    }
                }

            }
        }


        /*
private void b_kruskal_Click(object sender, EventArgs e)
{
pictureBox1.Image = kruskal;
b_kruskal.Enabled = false;
b_prim.Enabled = true;
b_grafo.Enabled = true;
}

private void b_prim_Click(object sender, EventArgs e)
{
pictureBox1.Image = prim;
b_kruskal.Enabled = true;
b_prim.Enabled = false;
b_grafo.Enabled = true;
}

private void b_grafo_Click(object sender, EventArgs e)
{
pictureBox1.Image = copia;
b_prim.Enabled = true;
b_kruskal.Enabled = true;
b_grafo.Enabled = false;
}*/
    }
}
