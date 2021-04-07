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
        Grafo grafo = new Grafo();
        Bitmap copia, copiaBresenham;
        List<Circulo> Circulos = new List<Circulo>();
        List<Point> puntC = new List<Point>();
        List<Circulo> masCercanos = new List<Circulo>();
        /*List<Circulo> BubbleCirc = new List<Circulo>();
        List<Circulo> SelectCirc = new List<Circulo>();
        List<Circulo> InsertCirc = new List<Circulo>();*/

        public Form_Principal()
        {
            InitializeComponent();
        }
        //Bubble sort
        /*public static void exchange(List<Circulo> circulos, int m, int n)
        {
            Circulo temporary;

            temporary = circulos[m];
            circulos[m] = circulos[n];
            circulos[n] = temporary;
        }
        public static void BubbleSort(List<Circulo> circulos)
        {
            int i, j;
            int N = circulos.Count;

            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if (circulos[i].area < circulos[i + 1].area)
                        exchange(circulos, i, i + 1);
                }
            }
        }
        //Selection sort
        public static int Min(List<Circulo> circulo, int start)
        {
            int minPos = start;
            for (int pos = start + 1; pos < circulo.Count; pos++)
                if (circulo[pos].puntoC.X < circulo[minPos].puntoC.X)
                    minPos = pos;
            return minPos;
        }
        public static void SelectionSort(List<Circulo> circulo)
        {
            int i;
            int N = circulo.Count;

            for (i = 0; i < N - 1; i++)
            {
                int k = Min(circulo, i);
                if (i != k)
                    exchange(circulo, i, k);
            }
        }      
        //Insertion sort
        public static void InsertionSort(List<Circulo> circulo)
        {
            int i, j;
            int N = circulo.Count;

            for (j = 1; j < N; j++)
            {
                for (i = j; i > 0 && circulo[i].puntoC.Y < circulo[i - 1].puntoC.Y; i--)
                {
                    exchange(circulo, i, i - 1);
                }
            }
        }*/

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
            //h.DrawEllipse(otherPen, mitad - pixelCount , centro - pixelCount , pixelCount * 2 , pixelCount * 2 );
            circulo += 1;
            String drawString = circulo.ToString();
            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Orange);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // Draw string to screen.
            h.DrawString(drawString, drawFont, drawBrush, mitad +10, centro+10, drawFormat);
            /*copia.SetPixel(mitad, centro, Color.Green);
            copia.SetPixel(mitad, centro + 1, Color.Green);
            copia.SetPixel(mitad - 1, centro, Color.Green);
            copia.SetPixel(mitad + 1, centro, Color.Green);
            copia.SetPixel(mitad, centro - 1, Color.Green);
            copia.SetPixel(mitad, centro+2, Color.Green);
            copia.SetPixel(mitad - 2, centro, Color.Green);
            copia.SetPixel(mitad+2, centro, Color.Green);
            copia.SetPixel(mitad, centro - 2, Color.Green);*/

            double area = (3.1416 * (pixelCount * pixelCount));

            Circulo circo = new Circulo(circulo,mitad,centro,pixelCount,area);
            puntC.Add(circo.puntoC);
            Circulos.Add(circo);
            pictureBox1.Image = copia;
            //MessageBox.Show("Cambio");
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

        bool bresenham(Bitmap bresen, Circulo circ1, Circulo circ2)
        {
            Graphics g = Graphics.FromImage(bresen);
            SolidBrush white = new SolidBrush(White);
            Color pixel;
            g.FillEllipse(white, circ1.puntoC.X - circ1.radio - 2, circ1.puntoC.Y - circ1.radio - 1, circ1.radio * 2 + 4, circ1.radio * 2 + 4);
            g.FillEllipse(white, circ2.puntoC.X - circ2.radio - 2, circ2.puntoC.Y - circ2.radio - 1, circ2.radio * 2 + 4, circ2.radio * 2 + 4);
            //pictureBox2.Image = bresen;
            int x0 = circ1.puntoC.X,  y0= circ1.puntoC.Y,  x1=circ2.puntoC.X,  y1=circ2.puntoC.Y;
            bool obstacle= false;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            while(true)
            {
                //bresen.SetPixel(x0, y0, Color.Red);
                pixel = bresen.GetPixel(x0, y0);
                if(pixel.R!=255&&pixel.G!=255&&pixel.B!=255)
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
            if (masCercanos.Count == 2)
            {
                foreach (var item in masCercanos)
                {
                    h.DrawLine(otherPen, item.puntoC, new Point(item.puntoC.X, item.puntoC.Y + 1));
                    h.DrawLine(otherPen, item.puntoC, new Point(item.puntoC.X + 1, item.puntoC.Y));
                }
            }
            listBox3.DataSource = null;
            listBox3.DataSource = masCercanos;
            /*BubbleCirc = Circulos;
            SelectCirc = Circulos;
            InsertCirc = Circulos;

            BubbleSort(BubbleCirc);
            listBox2.DataSource = null;
            listBox2.DataSource = BubbleCirc;

            SelectionSort(SelectCirc);
            listBox3.DataSource = null;
            listBox3.DataSource = SelectCirc;

            InsertionSort(InsertCirc);
            listBox4.DataSource = null;
            listBox4.DataSource = InsertCirc;
            */

            bool resultado = bresenham(copiaBresenham, Circulos[0], Circulos[1]);
            if (resultado == false)
            {
                h.DrawLine(otherPen, Circulos[0].puntoC, Circulos[1].puntoC);
            }
            //MessageBox.Show(resultado.ToString());

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
