using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace CircleDetect
{
    public partial class Form_Principal : Form
    {
        Color Black = Color.FromArgb(255, 0, 0, 0);
        Color White = Color.FromArgb(255, 255, 255, 255);
        int circulo = 0;
        int blanco = 1; 
        Grafo grafo = new Grafo();
        List<Dijstra> eDijstra;
        List<Grafo.Vertices> vCamino;
        Bitmap copia;
        Bitmap Camino;
        Grafo.Vertices VOrigen = null;
        Grafo.Vertices VDestino = null;
        List<Circulo> Circulos = new List<Circulo>();
        List<Point> puntC = new List<Point>();
        List<Circulo> masCercanos = new List<Circulo>();
        bool eGrafo;
        Image estrella = Grafo.scaleSize(new Size(40,40), Image.FromFile("..\\..\\..\\images\\estrella.png"));
        Image flor = Grafo.scaleSize(new Size(40, 40), Image.FromFile("..\\..\\..\\images\\flor.png"));
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
            var ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen|*.jpg;*.png";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                grafo = new Grafo();
                eGrafo = false;
                pictureBox1.Size = new Size(1123, 794);
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                VOrigen = null;
                VDestino = null;
                label3.Text = "";
                label6.Text = "";
                sGrafos.Text = "";
                var img_C = new Bitmap(Image.FromFile(ofd.FileName));
                img_C = grafo.escalaImg(pictureBox1, img_C);
                pictureBox1.Image = img_C;
                copia = (Bitmap)img_C;
                progressBar1.Value = 0;
                button2.Enabled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
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

            progressBar1.Maximum = (Circulos.Count())*2;
            foreach (Circulo item in Circulos)
            {
                grafo.añadirVert(new Grafo.Vertices(item.puntoC, item.radio, item.area));
            }
            grafo.calcularVertices(copia, grafo, progressBar1);

            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            Font drawFont = new Font("Arial", 15);
            eGrafo = true;
            
            grafo.subGrafos();
            grafo.mostrarGrafo(copia,progressBar1);

            foreach (Circulo item in Circulos)
            {
                grafo.añadirVert(new Grafo.Vertices(item.puntoC, item.radio, item.area));
            }
            var grafoP = grafo;
            button2.Enabled = false;
            sGrafos.Text = grafo.LSubGrafos.Count.ToString();
            Circulos.Clear();
            puntC.Clear();
            masCercanos.Clear();
            Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = Camino;
            pictureBox1.BackgroundImage = copia;
            button2.Enabled = false;
        }
        public Grafo.Vertices Pertenece(int x, int y, Bitmap im)
        {
            Grafo.Vertices vertEn = null;
            foreach (Grafo.Vertices item in grafo.List_Vert)
            {
                if (grafo.Distancia(new Point(x, y), item.punto) - item.radio < 0)
                {
                    return item;
                }
            }
            return vertEn;
        }

        //Click
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (eGrafo==true)
            {         
                Color pixel = copia.GetPixel(e.X, e.Y);
                Grafo.Vertices VertClk = Pertenece(e.X, e.Y, copia);
                if (VertClk!=null)
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            {
                                label3.Text = VertClk.name;
                                VOrigen = VertClk;
                                Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
                                pictureBox1.Image = Camino;
                                if (VDestino!=null)
                                {
                                    FuncionesDjstr.dibujaPart(VDestino.punto, Camino, flor);
                                }
                                FuncionesDjstr.dibujaPart(VOrigen.punto, Camino, estrella);
                            }
                            break;
                        case MouseButtons.Right:
                            {
                                label6.Text = VertClk.name;
                                VDestino = VertClk;
                                Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
                                pictureBox1.Image = Camino;
                                if (VOrigen!=null)
                                {
                                    FuncionesDjstr.dibujaPart(VOrigen.punto, Camino, estrella);
                                }
                                FuncionesDjstr.dibujaPart(VDestino.punto, Camino, flor);
                            }
                            break;
                        default:
                            break;
                    }
                    if ((VDestino!=null&&VOrigen!=null)&&(VDestino.grupo != VOrigen.grupo))
                    {
                        eDijstra = null;
                        vCamino = null;
                        button1.Enabled = false;
                        button3.Enabled = false;
                        button4.Enabled = false;
                    }
                    else if (VDestino != null && VOrigen != null)
                    {
                        eDijstra = FuncionesDjstr.elementoDjistra(grafo, VOrigen);
                        vCamino = FuncionesDjstr.obtenerVertices(eDijstra, VDestino, VOrigen);
                        button1.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = Camino;
            pictureBox1.BackgroundImage = copia;
            for (int i = 0; i < vCamino.Count-1; i++)
            {
                Graphics f = Graphics.FromImage(Camino);
                Pen p = new Pen(Color.Red, 2);
                f.DrawLine(p, vCamino[i].punto, vCamino[i + 1].punto);
            }
            FuncionesDjstr.dibujaPart(VOrigen.punto, Camino, estrella);
            FuncionesDjstr.dibujaPart(VDestino.punto, Camino, flor);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<List<Point>> bressen = new List<List<Point>>();
            button3.Enabled = false;
            button1.Enabled = false;
            for (int i = 0; vCamino.Count() - 1  > i; i++)
            {
                foreach (var ar in vCamino[i].ListAr)
                {
                    if (ar.sig == vCamino[i+1])
                    {
                        bressen.Add(ar.camino);
                        break;
                    }
                }
            }
            foreach (List<Point> camino in bressen)
            {
                foreach (Point punto in camino)
                {
                    Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
                    pictureBox1.Image = Camino;
                    for (int i = 0; i < vCamino.Count - 1; i++)
                    {
                        Graphics f = Graphics.FromImage(Camino);
                        Pen p = new Pen(Color.Red, 1);
                        f.DrawLine(p, vCamino[i].punto, vCamino[i + 1].punto);
                    }
                    FuncionesDjstr.dibujaPart(punto, Camino, estrella);
                    FuncionesDjstr.dibujaPart(VDestino.punto, Camino, flor);
                    pictureBox1.Refresh();
                    System.Threading.Thread.Sleep(1);
                }
            }
            Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = Camino;
            label3.Text = VDestino.name;
            VOrigen = VDestino;
            FuncionesDjstr.dibujaPart(VOrigen.punto, Camino, estrella);
            FuncionesDjstr.dibujaPart(VDestino.punto, Camino, estrella);
            eDijstra = FuncionesDjstr.elementoDjistra(grafo, VOrigen);
            vCamino = FuncionesDjstr.obtenerVertices(eDijstra, VDestino, VOrigen);
            button3.Enabled = true;
            button1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Grafo.Vertices> camino;
            Camino = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = Camino;
            foreach (Grafo.Vertices v in grafo.LSubGrafos[VOrigen.grupo-1])
            {
                camino = FuncionesDjstr.obtenerVertices(eDijstra, v, VOrigen);
                FuncionesDjstr.dCamino(camino,Camino, Color.Red, 2);
            }
            FuncionesDjstr.dibujaPart(VOrigen.punto, Camino, estrella);
            FuncionesDjstr.dibujaPart(VDestino.punto, Camino, flor);
        }
    }
}
