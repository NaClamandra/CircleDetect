using System;
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
        Bitmap copia, copiaBresenham;
        List<Circulo> Circulos = new List<Circulo>();
        List<Point> puntC = new List<Point>();
        List<Circulo> masCercanos = new List<Circulo>();

        public Form_Principal()
        {
            InitializeComponent();
        }

        public void DetC(int j, int i, Bitmap img)
        {
            int pixelCount = 0, mitad, centro;
            while (img.GetPixel(j, i) != White)
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
            while (img.GetPixel(mitad, i) != White)
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
            //String drawString = circulo.ToString();
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
            var ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen|*.jpg;*.png";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
                button2.Enabled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            Grafo grafo = new Grafo();
            var img = (Bitmap)pictureBox1.Image;
            copia = (Bitmap)img.Clone();
            copiaBresenham = (Bitmap)img.Clone();
            FindCirc((Bitmap)pictureBox1.Image);
            circulo = 0;
            listBox1.DataSource = null;
            listBox1.DataSource = Circulos ;
            Pen otherPen = new Pen(Color.Yellow, 20);
            Graphics h = Graphics.FromImage(copia);
            masCercanos = ButeForce.lista_p(Circulos);
            foreach (Grafo.Vertices item in grafo.calcularVertices(copia, Circulos))
            {
                grafo.añadirVert(item);
            }
            grafo.mostrarGrafo(copia);
            if (masCercanos.Count == 2)
            {
                foreach (var item in masCercanos)
                {
                    h.DrawLine(otherPen, item.puntoC, new Point(item.puntoC.X, item.puntoC.Y + 1));
                    h.DrawLine(otherPen, item.puntoC, new Point(item.puntoC.X + 1, item.puntoC.Y));
                }
            }
            grafo.matriz(dataGridView1); 
            button2.Enabled = false;
            Circulos.Clear();
            puntC.Clear();
            masCercanos.Clear();
            foreach (Circulo item in Circulos)
            {
                grafo.añadirVert(new Grafo.Vertices(item.puntoC));
            }
        }
    }
}
