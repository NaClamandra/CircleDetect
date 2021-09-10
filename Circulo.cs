using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CircleDetect
{
    public class Circulo
    {
        public int id;
        public Point puntoC;
        public int radio;
        public double area;

        public Circulo(int ID,int centro_x, int centro_y, int radio_, double area_)
        {
            id = ID;
            puntoC = new Point(centro_x, centro_y);
            radio = radio_;
            area = area_;
        }
        public override string ToString()
        {
            return String.Format("({4}) Centro({0},{1}), Area({2}), Radio({3})", puntoC.X,puntoC.Y,area,radio,id) ;
        }
    }
}
