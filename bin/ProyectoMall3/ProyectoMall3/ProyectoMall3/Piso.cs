using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMall3
{
    class Piso
    {
        public int altura;
        public int area;
        public List<Local> listaLocales = new List<Local>();

        public Piso(int altura, int area, List<Local> listaLocales)
        {
            this.altura = altura;
            this.area = area;
            this.listaLocales = listaLocales;
        }
    }
}
