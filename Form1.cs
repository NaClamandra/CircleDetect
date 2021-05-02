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
        Grafo grafo = new Grafo();
        Bitmap copia;
        List<Circulo> Circulos = new List<Circulo>();
        List<Point> puntC = new List<Point>();
        List<Circulo> masCercanos = new List<Circulo>();
        bool eGrafo;
        public Form_Principal()
        {
            InitializeComponent();
        }
        public void DetC(int j, int i, Bitmap img)
        {
            int pixelCount = 0, mitad, centro;
            while (img.GetPixel(j, i) != White && j!=img.Width-1)
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
            while (img.GetPixel(mitad, i) != White && i != img.Height-1)
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
                    if (buscar && bit.R == bit.G && bit.B == bit.R && bit.R != 255)
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
            eGrafo = false;
            pictureBox1.Size = new Size(1123,794);
            var ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen|*.jpg;*.png";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                var img_C = Image.FromFile(ofd.FileName);
                if (img_C.Width>pictureBox1.Width || img_C.Height>pictureBox1.Height)
                {
                    float scale;
                    float ScaWi = (float)pictureBox1.Width / (float)img_C.Width;
                    float ScaHe = (float)pictureBox1.Height / (float)img_C.Height;
                    //img_C = new Bitmap(img_C, new Size(pictureBox1.Width, pictureBox1.Height));
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
                pictureBox1.Image = img_C;
                copia = (Bitmap)img_C;
                //pictureBox1.Enabled = false;
                button2.Enabled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //pictureBox1.Enabled = true;
            dataGridView1.Columns.Clear();
            var img = (Bitmap)pictureBox1.Image;
            copia = (Bitmap)img.Clone();
            Bitmap senal = new Bitmap(800, 200);;
            FindCirc((Bitmap)pictureBox1.Image);
            circulo = 0;
            listBox1.DataSource = null;
            listBox1.DataSource = Circulos;
            Pen otherPen = new Pen(Color.Yellow, 50);
            Graphics h = Graphics.FromImage(copia);
            Graphics s = Graphics.FromImage(senal);
            SolidBrush gray = new SolidBrush(Color.LightGray);
            SolidBrush grn = new SolidBrush(Color.LightGreen);
            SolidBrush blk = new SolidBrush(Color.Black);
            masCercanos = ButeForce.lista_p(Circulos);
            foreach (Grafo.Vertices item in grafo.calcularVertices(copia, Circulos))
            {
                grafo.añadirVert(item);
            }  
            if (masCercanos.Count == 2)
            {
                foreach (var item in masCercanos)
                {
                    
                    h.FillEllipse(gray, item.puntoC.X - (item.radio/2), item.puntoC.Y-(item.radio / 2), item.radio, item.radio);
                }
            }
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            Font drawFont = new Font("Arial", 15);
            s.DrawString("Aristas", drawFont, drawBrush,120,5, drawFormat);
            s.FillEllipse(grn, 20, 0, 30, 30);
            s.DrawString("Puntos más cercanos", drawFont, drawBrush, 250, 45, drawFormat);
            s.FillEllipse(gray, 20, 40, 30, 30);
            s.DrawString("Vértices", drawFont, drawBrush, 130, 85, drawFormat);
            s.FillEllipse(blk, 20, 80, 30, 30);
            pictureBox2.Image = senal;
            eGrafo = true;
            grafo.mostrarGrafo(copia);
            grafo.matriz(dataGridView1); 
            button2.Enabled = false;
            Circulos.Clear();
            puntC.Clear();
            masCercanos.Clear();
            foreach (Circulo item in Circulos)
            {
                grafo.añadirVert(new Grafo.Vertices(item.puntoC, item.radio, item.area));
            }
        }
        public static double Distancia(Point a, Point b)
        {
            float dX = b.X - a.X;
            float dY = b.Y - a.Y;
            return Math.Sqrt((dX*dX) + (dY*dY));
        }
        public static Grafo.Vertices Pertenece(int x, int y, Grafo grafo, Bitmap im)
        {
            Grafo.Vertices vertEn = null;
            foreach (Grafo.Vertices item in grafo.List_Vert)
            {
                if(Distancia(new Point(x, y), item.punto)-item.radio<0)
                {
                    return item;
                }
            }
            return vertEn;
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (eGrafo==true)
            {
                Graphics g = Graphics.FromImage(copia);
                Color pixel = copia.GetPixel(e.X, e.Y);
                if (pixel.R == pixel.G && pixel.B == pixel.R && pixel.R != 255)
                {
                    Grafo.Vertices VertClk = Pertenece(e.X, e.Y, grafo, copia);
                    MessageBox.Show(VertClk.name);
                }
            }
        }

    }
}
