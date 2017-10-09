using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMall
{
    class Local
    {
        public string nombre, categoria;
        public int numEmpleados, areaLocal, precioMin, precioMax, clientesDiaAnterior, clientesTotales, gananciasDiaAnterior, gananciasTotales;

        public Local(string nombre, string categoria, int numEmpleados, int areaLocal, int precioMin, int precioMax)
        {
            this.nombre = nombre;
            this.categoria = categoria;
            this.numEmpleados = numEmpleados;
            this.areaLocal = areaLocal;
            this.precioMin = precioMin;
            this.precioMax = precioMax;
            clientesDiaAnterior = 0;
            clientesTotales = 0;
            gananciasDiaAnterior = 0;
            gananciasTotales = 0;

        }


    }
}
