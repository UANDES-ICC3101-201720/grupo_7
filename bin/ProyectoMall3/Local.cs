using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMall3
{
    class Local
    {
        public string nombre, categoria;
        public int numEmpleados, areaLocal, precioMin, precioMax, clientesDiaAnterior, clientesTotales, gananciasDiaAnterior, gananciasTotales, promedioSueldos;

        public Local(string nombre, string categoria, int numEmpleados, int areaLocal, int precioMin, int precioMax, int promedioSueldos)
        {
            this.nombre = nombre;
            this.categoria = categoria;
            this.numEmpleados = numEmpleados;
            this.areaLocal = areaLocal;
            this.precioMin = precioMin;
            this.precioMax = precioMax;
            this.promedioSueldos = promedioSueldos;
            clientesDiaAnterior = 0;
            clientesTotales = 0;
            gananciasDiaAnterior = 0;
            gananciasTotales = 0;
        }

        public override string ToString()
        {
            return this.nombre;
        }
    }
}

