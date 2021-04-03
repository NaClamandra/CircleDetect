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
        Bitmap copia;
        List<Circulo> Circulos = new List<Circulo>();
        List<Circulo> BubbleCirc = new List<Circulo>();
        List<Circulo> SelectCirc = new List<Circulo>();
        List<Circulo> InsertCirc = new List<Circulo>();

        public Form_Principal()
        {
            InitializeComponent();
        }
        //Bubble sort
        public static void exchange(List<Circulo> circulos, int m, int n)
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
                if (circulo[pos].centrox < circulo[minPos].centrox)
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
                for (i = j; i > 0 && circulo[i].centroy < circulo[i - 1].centroy; i--)
                {
                    exchange(circulo, i, i - 1);
                }
            }
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

            Pen otherPen = new Pen(Color.Red,5);
            SolidBrush white = new SolidBrush(White);

            g.FillEllipse(white, mitad - pixelCount-2, centro - pixelCount-1, pixelCount * 2+4, pixelCount * 2+4);
            h.DrawEllipse(otherPen, mitad - pixelCount , centro - pixelCount , pixelCount * 2 , pixelCount * 2 );
            circulo += 1;
            String drawString = circulo.ToString();
            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Orange);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // Draw string to screen.
            h.DrawString(drawString, drawFont, drawBrush, mitad +10, centro+10, drawFormat);
            copia.SetPixel(mitad, centro, Color.Green);
            copia.SetPixel(mitad, centro + 1, Color.Green);
            copia.SetPixel(mitad - 1, centro, Color.Green);
            copia.SetPixel(mitad + 1, centro, Color.Green);
            copia.SetPixel(mitad, centro - 1, Color.Green);
            copia.SetPixel(mitad, centro+2, Color.Green);
            copia.SetPixel(mitad - 2, centro, Color.Green);
            copia.SetPixel(mitad+2, centro, Color.Green);
            copia.SetPixel(mitad, centro - 2, Color.Green);

            double area = (3.1416 * (pixelCount * pixelCount));

            Circulo circo = new Circulo(circulo,mitad,centro,pixelCount,area);
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
            
            var img = (Bitmap)pictureBox1.Image;
            copia = (Bitmap)img.Clone();
            FindCirc((Bitmap)pictureBox1.Image);
            circulo = 0;
            listBox1.DataSource = null;
            listBox1.DataSource = Circulos ;
            BubbleCirc = Circulos;
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

            button2.Enabled = false;
            Circulos.Clear();
        }
    }
}
