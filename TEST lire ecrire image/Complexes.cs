using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_info
{
    class Complexes
    {
        public double r;    //partie réelle
        public double i;    //partie imaginaire

        public Complexes(double r, double i)
        {
            this.r = r;
            this.i = i;
        }

        public void Square()
        {
            double temp = (r * r) - (i * i);
            i = 2.0 * r * i;
            r = temp;
        }
        public double Module()
        {
            return Math.Sqrt((r * r) + (i * i));
        }
        public void Add(Complexes c)
        {
            r += c.r;
            i += c.i;
        }
    }
}




