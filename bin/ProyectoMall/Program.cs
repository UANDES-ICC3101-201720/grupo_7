using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ProyectoMall
{
    class Program
    {
        static void Main(string[] args)
        {
            int horas_dia = -1;
            int dinero_inicial = -1;
            int precioArriendo = 100;
            List<Piso> pisos = new List<Piso>();

            int number; // se ocupa para la funcion int.TryParse


            //horas de trabajo, asegurandose que sean mayores a 1 y menores a 24, además de que sea un int valido
            while (horas_dia <= 0)
            {
                Console.WriteLine("Indique la cantidad de horas que trabajará su mall (entre 1 y 24)");
                string input = Console.ReadLine();
                if ((int.TryParse(input, out number) == true))
                {
                    if (int.Parse(input) > 0 && int.Parse(input) <= 24)
                    {
                        horas_dia = int.Parse(input);
                    }
                }
            }


            //Dinero inicial, asegurandose que sea un int mayor a 1
            while (dinero_inicial < 1)
            {
                Console.WriteLine("Indique su dinero inicial");
                string input = Console.ReadLine();
                if ((int.TryParse(input, out number) == true))
                {
                    dinero_inicial = int.Parse(input);
                }
            }


            //inicio
            // Creando pisos, y asegurandose que el input sea un numero completo mayor a 1
            int numero_pisos = -1;
            while (numero_pisos < 1)
            {
                Console.WriteLine("Indique la cantidad de pisos de su mall");
                string input = Console.ReadLine();
                if ((int.TryParse(input, out number) == true))
                {
                    numero_pisos = int.Parse(input);
                }
            }

            int n = 1;
            int tamanio = 2147483647;
            while (n <= numero_pisos)
            {
                Console.WriteLine("indique el tamaño del piso "+n.ToString());
                string input = Console.ReadLine();
                if ((int.TryParse(input, out number) == true))  // asegurandome que sea un int, y que sea mayor a el tamaño del piso anterior
                {
                    if (tamanio >= int.Parse(input) && int.Parse(input) > 10)
                    {
                        tamanio = int.Parse(input);
                        n++;
                        pisos.Add(new Piso(n, tamanio,new List<Local>()));
                    }
                    else if (tamanio < int.Parse(input) || int.Parse(input) <= 10)
                    {
                        Console.WriteLine("el tamaño debe ser menor o igual al del piso anterior, el cual era "+tamanio+" además de ser un numero mayor a 10");
                    }
                }
            }
            // pisos listos

            // ahora los locales
            n = 1;
            // el siguiente while crea los locales con sus atributos default, pero asegurandose que quepen en cada piso
            while (n <= numero_pisos)
            {
                int area_piso = pisos[(n - 1)].area;
                int numero_locales = -1;
                List<Local> locales = new List<Local>();
                while (numero_locales < 1)
                {
                    Console.WriteLine("Indique la cantidad de locales para el piso " + n+", si esta es igual o menor a 0, será reemplasada por 1");
                    // asegurandose de que ponga bien el numero de tiendas
                    string input = Console.ReadLine();
                    if ((int.TryParse(input, out number) == true)) 
                    {
                        numero_locales = int.Parse(input);
                        if (numero_locales <= 0)
                        {
                            numero_locales = 1;
                        }

                        // ahora voy a preguntar el area de cada local
                        int nn = 1;
                        while (nn <= numero_locales)
                        {
                            Console.WriteLine("area que queda en el piso actual: "+area_piso);
                            Console.WriteLine("ingrese el area del local " +nn);
                            string inputt = Console.ReadLine();
                            if ((int.TryParse(inputt, out number) == true))  // asegurandome que sea un numero
                            {
                                if (int.Parse(inputt) >= 10 && int.Parse(inputt) <= area_piso && int.Parse(inputt) <= 100)  // siesque el numero puesto es valido, crear tienda
                                {
                                    pisos[(n-1)].listaLocales.Add(new Local("default1", "default1", 0, int.Parse(inputt),0,0));
                                    area_piso -= int.Parse(inputt);
                                    nn++;
                                }

                                else if (int.Parse(inputt) < 10 || int.Parse(inputt) > area_piso || int.Parse(inputt) > 100) // siesque el numero puesto no es valido, preguntar denuevo
                                {
                                    Console.WriteLine("su area no puede ser menor a 10, ser mayor a 100, o ser mayor al area que queda");
                                }
                            }

                            if (area_piso < 10)
                            {
                                Console.WriteLine("El piso se quedó sin area para poner más locales, saltando al siguiente piso");
                                nn += numero_locales;
                            }
                        }
                        n++;
                    }

                }

            } //este while contiene la creacion de las areas de los locales

            Console.WriteLine("Locales listos, ahora rellenemos su informacion");
            n = 1;
            foreach (Piso piso in pisos)
            {
                foreach (Local local in piso.listaLocales)
                {
                    Console.WriteLine("nombre local " + n);
                    local.nombre = Console.ReadLine();

                    //elijiendo categoria del local
                    string eleccion = "asd";
                    while (eleccion != "a" && eleccion != "b" && eleccion != "c")
                    {
                        Console.WriteLine("Categoria del local " + n + "\n a)Comercial \n b)Comida \n c)Entretencion ");
                        eleccion = Console.ReadLine();
                    }

                    if (eleccion == "a") //si la categia es Comercial...
                    {
                        eleccion = "asd";
                        while (eleccion != "a" && eleccion != "b" && eleccion != "c" && eleccion != "d" && eleccion != "e")
                        {
                            Console.WriteLine(" a)Ropa \n b)Hogar \n c)Alimento \n d)Ferreteria \n e)Tecnologia");
                            eleccion = Console.ReadLine();
                        }

                        if (eleccion == "a") { local.categoria = "ropa"; }
                        if (eleccion == "b") { local.categoria = "hogar"; }
                        if (eleccion == "c") { local.categoria = "alimento";}
                        if (eleccion == "d") { local.categoria = "ferreteria"; }
                        if (eleccion == "e") { local.categoria = "tecnologia"; }
                    }

                    else if (eleccion == "b") //si la categia es Comida...
                    {
                        eleccion = "asd";
                        while (eleccion != "a" && eleccion != "b")
                        {
                            Console.WriteLine(" a)Rapida \n b)Restaurant");
                            eleccion = Console.ReadLine();
                        }
                        
                        if (eleccion == "a") { local.categoria = "comida rapida"; }
                        if (eleccion == "b") { local.categoria = "restaurat"; }
                    }

                    else if (eleccion == "c") //si la categia es Entretencion...
                    {
                        eleccion = "asd";
                        while (eleccion != "a" && eleccion != "b" && eleccion != "c")
                        {
                            Console.WriteLine(" a)Cine \n b)Juegos \n c)Bowling");
                            eleccion = Console.ReadLine();
                        }

                        if (eleccion == "a") { local.categoria = "Cine"; }
                        if (eleccion == "b") { local.categoria = "Juegos"; }
                        if (eleccion == "c") { local.categoria = "Bowling"; }
                    }

                    // ahora a preguntar empleados, asegurandose que sea un int positivo
                    bool variable = true;
                    while (variable == true)
                    {
                        Console.WriteLine("ingrese cantidad de empleados, esta debe ser mayor a 0");
                        string input = Console.ReadLine();
                        if ((int.TryParse(input, out number) == true))
                        {
                            if (int.Parse(input) > 0)
                            {
                                variable = false;
                                local.numEmpleados = int.Parse(input);
                            }
                        }
                    }

                    // ahora a preguntar el precio minimo y maximo
                    variable = true;
                    while (variable == true)
                    {
                        Console.WriteLine("ingrese el precio minimo de su local (numero positivo)");
                        string input = Console.ReadLine();
                        if ((int.TryParse(input, out number) == true))
                        {
                            if (int.Parse(input) >= 0)
                            {
                                variable = false;
                                local.precioMin = int.Parse(input);
                            }
                        }
                    }

                    //precio maximo
                    variable = true;
                    while (variable == true)
                    {
                        Console.WriteLine("ingrese el precio minimo de su local (numero positivo, y mayor al minimo, el cual fue: " + local.precioMin + ")");
                        string input = Console.ReadLine();
                        if ((int.TryParse(input, out number) == true))
                        {
                            if (int.Parse(input) >= local.precioMin)
                            {
                                variable = false;
                                local.precioMax = int.Parse(input);
                            }
                        }
                    }
                    n++;
                }
            }
            // AQUI TERMINA LA INICIACION

            File.Create("C:\\Desktop\\reporte.txt");
            FileStream fs = new FileStream("reporte.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);

            int dia = 1;
            while (dia <= 10)
            {
                foreach (Piso piso in pisos)
                {
                    foreach (Local local in piso.listaLocales)
                    {
                        //aqui va la parte de los clientes
                        int c = local.clientesDiaAnterior;
                        int a = local.areaLocal;
                        int p = ((local.precioMin) + (local.precioMax)) / 2;
                        int e = local.numEmpleados;
                        int cmax = a + (a / 10) * Math.Max(100 - p, 0) / 100 * e; // formula cmax de la guia
                        Random r = new Random();
                        int clientes = r.Next(0, cmax);
                        local.clientesDiaAnterior = clientes;
                        local.clientesTotales += clientes;


                        //aqui van las ganancias
                        int v = r.Next(local.precioMin, local.precioMax);
                        int costoArriendo = local.areaLocal * precioArriendo;
                        int ganancia = v * clientes - (e + costoArriendo);
                        local.gananciasDiaAnterior = ganancia;
                        local.gananciasTotales += ganancia;
                        
                    }
                }
                // REPORTES

                // reporte 1 cantidad de clientes recepcionades y reporte 2 cantidad promedio clientes
                //reporte 3 cantida de ganancias totales y reporte 4 ganancias pormedio por dia
                // reporte 6 y 7, mayor y menor ganancia de locales, reportes 7 y 8 locales mayor y menor cantidad clientes
                int clientes_totales = 0;
                int promedio_clientes;
                int ganancias_totales = 0;
                int promedio_ganancias;
                int numero_tiendas = 0;
                
                // las siguientes variables serviran para guardar los mejores locales de cada reporte
                Local menor_ganancia = new Local("a", "a", 0, 0, 0, 0) { gananciasDiaAnterior = 2147483647 };
                Local mayor_ganancia = new Local("a", "a", 0, 0, 0, 0);
                Local menor_cliente = new Local("a", "a", 0, 0, 0, 0) { clientesDiaAnterior = 2147483647 };
                Local mayor_cliente = new Local("a", "a", 0, 0, 0, 0);
                foreach (Piso piso in pisos)
                {
                    foreach (Local local in piso.listaLocales)
                    {
                        numero_tiendas++;
                        clientes_totales += local.clientesDiaAnterior;
                        ganancias_totales += local.gananciasDiaAnterior;


                        if (local.gananciasDiaAnterior < menor_ganancia.gananciasTotales) // local con menor ganancia
                        {
                            menor_ganancia = local;
                        }
                        if (local.gananciasDiaAnterior > mayor_ganancia.gananciasTotales)// mayor ganancia
                        {
                            mayor_ganancia = local;
                        }
                        if (local.clientesDiaAnterior < menor_cliente.gananciasDiaAnterior) // menores clientes
                        {
                            menor_cliente = local;
                        }
                        if (local.clientesDiaAnterior > mayor_cliente.clientesDiaAnterior) //mayores clientes
                        {
                            mayor_cliente = local;
                        }
                    }
                }
                promedio_clientes = clientes_totales / numero_tiendas;
                promedio_ganancias = ganancias_totales / numero_tiendas;

                Console.WriteLine("Cantidad de clientes  recepcionados: "+clientes_totales+"\nCantidad de clientes promedio "+promedio_clientes+"\nGanancia total "+ganancias_totales+"\nGanancia promedio del dia "+promedio_ganancias);
                sw.WriteLine("cantidad de clientes recepcionados: " + clientes_totales + "\nCantidad de clientes promedio " + promedio_clientes + "\nGanancia total" + ganancias_totales+"\nGanancia promedio del dia"+promedio_ganancias);
                Console.WriteLine("Local con menor ganancia y su ganancia total\nnombre: "+menor_ganancia.nombre+"\nganancias del dia: "+menor_ganancia.gananciasDiaAnterior+"\nganancias totales"+menor_ganancia.gananciasTotales);
                sw.WriteLine("Local con menor ganancia y su ganancia total\nnombre: " + menor_ganancia.nombre + "\nganancias del dia: " + menor_ganancia.gananciasDiaAnterior + "\nganancias totales" + menor_ganancia.gananciasTotales);
                Console.WriteLine("Local con mayor ganancia y su ganancia total\nnombre: " + mayor_ganancia.nombre + "\nganancias del dia: " + mayor_ganancia.gananciasDiaAnterior + "\nganancias totales" + mayor_ganancia.gananciasTotales);
                sw.WriteLine("Local con mayor ganancia y su ganancia total\nnombre: " + mayor_ganancia.nombre + "\nganancias del dia: " + mayor_ganancia.gananciasDiaAnterior + "\nganancias totales" + mayor_ganancia.gananciasTotales);
                Console.WriteLine("Local con menor cantidad de clientes atendidos: "+menor_cliente.nombre);
                sw.WriteLine("Local con menor cantidad de clientes atendidos: " + menor_cliente.nombre);
                Console.WriteLine("Local con mayor cantidad de clientes atendidos: " +mayor_cliente.nombre);
                sw.WriteLine("Local con mayor cantidad de clientes atendidos: " + mayor_cliente.nombre);
                Console.ReadLine();

                DateTime beginWait = DateTime.Now;
                while (!Console.KeyAvailable && DateTime.Now.Subtract(beginWait).TotalSeconds < 1)
                    Thread.Sleep(250);

                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().KeyChar == 'p')
                    {
                        int pausa = 1;
                        Console.WriteLine("pausa");
                        while (pausa == 1)
                        {
                            ;
                            if (Console.KeyAvailable)
                            {
                                if (Console.ReadKey().KeyChar == 'p')
                                {
                                    pausa = 0;
                                }
                            }
                        }
                    }
                }
                dia++;
            }

            fs.Close();
            sw.Close();
            Console.ReadLine();
        }
    }
}