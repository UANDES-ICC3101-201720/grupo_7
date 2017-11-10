using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoMall3
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        int horas_dia = -1;
        int dinero_inicial = -1;
        int numero_pisos;
        List<Piso> pisos = new List<Piso>();

        int number; // se ocupa para la funcion int.TryParse

        public MainWindow()
        {
            InitializeComponent();
        }


        //este boton hace aparecer el inicio de un nuevo juego
        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {

            //los "ok", son para saber si el usuario entró numero validos, de lo contrario el boton no deberia hacer nada

            //revisando horas de trabajo
            int error = 0;
            string input_horas_trabajo = Tbox_horas_trabajo.Text;
            if ((int.TryParse(input_horas_trabajo, out number) == true))
            {
                if (int.Parse(input_horas_trabajo) > 0 && int.Parse(input_horas_trabajo) <= 24)
                {
                    horas_dia = int.Parse(input_horas_trabajo);
                    
                }
                else
                {
                    error = 1;
                    Label_Errores_inicio.Content = "Error, Horas de trabajo invalidas";
                }
                
            }
            else
            {
                error = 4;//pusieron letras
                Label_Errores_inicio.Content = "Error, Horas de trabajo invalidas";
            }

            //resvisando dinero inicial
            
            string input_dinero_inicial = Tbox_dinero_inicial.Text;
            if ((int.TryParse(input_dinero_inicial, out number) == true))
            {
                dinero_inicial = int.Parse(input_dinero_inicial);
                if(int.Parse(input_dinero_inicial) <= 0)
                {
                    error = 2;
                    Label_Errores_inicio.Content = "Error, Dinero inicial menor a 1";
                }
            }
            else
            {
                error = 4; //ingresaron LETRAS
                Label_Errores_inicio.Content = "El dinero no puede ser letras...";
            }

            //revisando cantidad de pisos
            string input_numero_pisos = Tbox_Cantidad_de_pisos.Text;
            if ((int.TryParse(input_numero_pisos, out number) == true))  // asegurandome que sea un int, y que sea mayor a el tamaño del piso anterior
            {
                if ((int.TryParse(input_numero_pisos, out number) == true))
                {
                    if(int.Parse(input_numero_pisos) > 0)
                    {
                        numero_pisos = int.Parse(input_numero_pisos);
                       
                    }
                    else
                    {
                        error = 3;
                        Label_Errores_inicio.Content = "Error, numero de pisos invalido";
                    }
                }
            }
            else
            {
                error = 4; //ingresaron LETRAS
                Label_Errores_inicio.Content = "Error, numero de pisos invalido";
            }

            if (error == 0)
            {
                Grid_Juego_Nuevo.Visibility = Visibility.Hidden;
                Grid_Juego_Nuevo_2.Visibility = Visibility.Visible;

                int n = 1;
                while (n <= numero_pisos)
                {
                    pisos.Add(new Piso(n, 0, new List<Local>()));
                    n++;
                }

                foreach(Piso piso in pisos)
                {
                    ListBoxItem a = new ListBoxItem();
                    a.Content = "piso " + piso.altura;
                    ListBox_Pisos.Items.Add(a);
                }


            }

        }


        //este index sirve para mostrar el tamaño de algun piso de la lista<Piso> pisos
        int index_listbox_pisos = -1;
        private void ListBox_Pisos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index_listbox_pisos = ListBox_Pisos.SelectedIndex;
            Tbox_tamanio_piso.Text = pisos[index_listbox_pisos].area.ToString();
            Tbox_cantidad_locales.Text = pisos[index_listbox_pisos].listaLocales.Count.ToString();
        }


        //este boton revisa que el numero ingresado como area y cantidad de locales de cada piso sea valido
        private void ButtonAceptarCambiosPiso_Click(object sender, RoutedEventArgs e)
        {
            //esto le quita el texto al label para mostrar errores
            Label_Errores_Pisos.Content = "";

            //esto es para ver si el piso es valido
            if ((int.TryParse(Tbox_tamanio_piso.Text, out number) == true))
            {
                if(index_listbox_pisos > 0)
                {
                    if (int.Parse(Tbox_tamanio_piso.Text) <= pisos[index_listbox_pisos - 1].area)
                    {
                        pisos[index_listbox_pisos].area = int.Parse(Tbox_tamanio_piso.Text);
                    }
                    else
                    {
                        Label_Errores_Pisos.Content = "El piso anterior es más pequeño que eso, no se guardó el cambio";
                    }
                }

                if (index_listbox_pisos == 0)
                {
                    pisos[index_listbox_pisos].area = int.Parse(Tbox_tamanio_piso.Text);
                }
            }

            if ((int.TryParse(Tbox_cantidad_locales.Text, out number) == true))
            {
                if (int.Parse(Tbox_cantidad_locales.Text) > 0)
                {
                    int n = 0;
                    pisos[index_listbox_pisos].listaLocales = new List<Local>();
                    while (n < int.Parse(Tbox_cantidad_locales.Text))
                    {
                        pisos[index_listbox_pisos].listaLocales.Add(new Local("default1", "default1", 0, 0 , 0, 0,0));
                        n++;
                    }
                }
            }


        }

        //este boton se asegura que todo funcione en el segundo grid, el que tiene todos los pisos y deja insertar sus areas.
        private void ButtonAceptar2_Click(object sender, RoutedEventArgs e)
        {
            int n = 1;
            int error = 0; //esto sirve para avisar para diferente errores, en caso de haberlos

            while (n < pisos.Count())
            {
                if (pisos[n].area > pisos[n-1].area)
                {
                    error = 1; //esto significa que hay algun piso con area más grande que su piso anterior
                }

                if (pisos[n].area <= 10)
                {
                    error = 2; //esto sigifica que hay algun piso con area menor al minimo de 10
                }
                
                if (pisos[n].listaLocales.Count() == 0)
                {
                    error = 3;
                }
                n++;
            }



            if (error == 1 ) {Label_Errores_Pisos.Content = "Algun piso no cumple con el requisito de ser menor al anterior"; }
            if (error == 2 ) { Label_Errores_Pisos.Content = "Algun piso no cumple con el requisito de tener el area mayor a 10"; }
            if (error == 3) { Label_Errores_Pisos.Content = "Algun piso no contiene ningun local"; }

            if (error == 0)
            {
                Grid_Juego_Nuevo_2.Visibility = Visibility.Hidden;
                Grid_Juego_Nuevo_3.Visibility = Visibility.Visible;

                n = pisos.Count();
                foreach (Piso piso in pisos)
                {
                    ListBoxItem a = new ListBoxItem();
                    a.Content = "piso " + piso.altura;
                    ListBox_Pisos_2.Items.Add(a);
                    n--;
                }

            }
        }

        int index_listbox_pisos_2 = -1;
        private void ListBox_Pisos_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            ListBox_Locales.SelectedIndex = -1;
            index_listbox_locales = -1;
            index_listbox_pisos_2 = ListBox_Pisos_2.SelectedIndex;
            ListBox_Locales.Items.Clear();


            


            Tbox_Nombre_Local.Clear();
            Tbox_Numero_Empleados.Clear();
            Tbox_promedio_sueldos.Clear();
            Tbox_Area_local.Clear();
            Tbox_precio_maximo.Clear();
            Tbox_Precio_minimo.Clear();
            ComboBox_Sub_Categorias_Locales.Text = "";

            StackPanel_Categorias.Visibility = Visibility.Hidden;


            int n = 1;
            foreach (Local local in pisos[index_listbox_pisos_2].listaLocales)
            {
                ListBoxItem a = new ListBoxItem();
                a.Content = "local " + n.ToString();
                ListBox_Locales.Items.Add(a);
                n++;
            }
        }

        int index_listbox_locales;
        private void ListBox_Locales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       

            StackPanel_Categorias.Visibility = Visibility.Visible;
            index_listbox_locales = ListBox_Locales.SelectedIndex;

            if(index_listbox_locales != -1)
            {
                Tbox_Nombre_Local.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].nombre;
                Tbox_Numero_Empleados.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].numEmpleados.ToString();
                Tbox_promedio_sueldos.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].promedioSueldos.ToString();
                Tbox_Area_local.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].areaLocal.ToString();
                Tbox_precio_maximo.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].precioMax.ToString();
                Tbox_Precio_minimo.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].precioMin.ToString();
                ComboBox_Sub_Categorias_Locales.Text = pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales].categoria;
            }
            

        }

        private void ComboBox_Categorias_Locales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox_Sub_Categorias_Locales.Items.Clear();
            ComboBox_Sub_Categorias_Locales.SelectedIndex = -1;


            //si elije comercial en el combobox de categorias
            if(ComboBox_Categorias_Locales.SelectedIndex == 0)
            {
                ComboBoxItem a = new ComboBoxItem(); a.Content = "ropa";
                ComboBoxItem b = new ComboBoxItem(); b.Content = "hogar";
                ComboBoxItem c = new ComboBoxItem(); c.Content = "alimento";
                ComboBoxItem d = new ComboBoxItem(); d.Content = "ferreteria";
                ComboBoxItem f = new ComboBoxItem(); f.Content = "tecnologia";

                ComboBox_Sub_Categorias_Locales.Items.Add(a); ComboBox_Sub_Categorias_Locales.Items.Add(b); ComboBox_Sub_Categorias_Locales.Items.Add(c); ComboBox_Sub_Categorias_Locales.Items.Add(d); ComboBox_Sub_Categorias_Locales.Items.Add(f);


                //  ComboBox_Sub_Categorias_Locales.Items.Add(a);
            }
            if (ComboBox_Categorias_Locales.SelectedIndex == 1)
            {
                ComboBoxItem a = new ComboBoxItem(); a.Content = "comida rapida";
                ComboBoxItem b = new ComboBoxItem(); b.Content = "restaurat";

                ComboBox_Sub_Categorias_Locales.Items.Add(a); ComboBox_Sub_Categorias_Locales.Items.Add(b);

            }


            if (ComboBox_Categorias_Locales.SelectedIndex == 2)
            {
                ComboBoxItem a = new ComboBoxItem(); a.Content = "Cine";
                ComboBoxItem b = new ComboBoxItem(); b.Content = "Juegos";
                ComboBoxItem c = new ComboBoxItem(); c.Content = "Bowling";

                ComboBox_Sub_Categorias_Locales.Items.Add(a); ComboBox_Sub_Categorias_Locales.Items.Add(b); ComboBox_Sub_Categorias_Locales.Items.Add(c);
            }

        }


        private void ComboBox_Sub_Categorias_Locales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
       
        private void ButtonAceptar_Cambios_Local_Click(object sender, RoutedEventArgs e)
        {
            int error = 0;
            string nombre = Tbox_Nombre_Local.Text;


            string categoria;
            if (ComboBox_Sub_Categorias_Locales.SelectedIndex == -1) { categoria = "restaurant"; }
            else { categoria = ComboBox_Sub_Categorias_Locales.Items[ComboBox_Sub_Categorias_Locales.SelectedIndex].ToString(); }


            int numero_empleados = 0;
            if (int.TryParse(Tbox_Numero_Empleados.Text, out number) == true)
            {
                numero_empleados = int.Parse(Tbox_Numero_Empleados.Text);
            }
            else { error = 1; }

            int area_local = 0;
            if (int.TryParse(Tbox_Area_local.Text, out number) == true)
            {
                area_local = int.Parse(Tbox_Area_local.Text);
            }
            else { error = 1; }

            int sueldo_promedio = 0;
            if (int.TryParse(Tbox_promedio_sueldos.Text, out number) == true)
            {
                sueldo_promedio = int.Parse(Tbox_promedio_sueldos.Text);
            }
            else { error = 1; }

            int precio_maximo = 0;
            if (int.TryParse(Tbox_precio_maximo.Text, out number) == true)
            {
                precio_maximo = int.Parse(Tbox_precio_maximo.Text);
            }
            else { error = 1; }

            int precio_minimo = 0;
            if (int.TryParse(Tbox_Precio_minimo.Text, out number) == true)
            {
                precio_minimo = int.Parse(Tbox_Precio_minimo.Text);
            }
            else { error = 1; }


            if (error == 0)
            {
                pisos[index_listbox_pisos_2].listaLocales[index_listbox_locales] = new Local(nombre, categoria, numero_empleados, area_local, precio_minimo, precio_maximo, sueldo_promedio);
            }
            else if (error == 1)
            {
                Label_error_locales.Content = "Error al ingresar un dato, no se guardará el local";
            }
        }

        private void ButtonAceptar4_Click(object sender, RoutedEventArgs e)
        {
            foreach(Piso piso in pisos)
            {
                int area_total = 0;
                foreach(Local local in piso.listaLocales)
                {
                    area_total += local.areaLocal;
                    if (area_total > piso.area)
                    {
                        Label_error_locales.Content = "algunos locales tienen más area que el piso en el que se encuentran";
                    }

                    else if( local.numEmpleados != 0 || local.areaLocal != 0 || local.precioMin != 0 || local.precioMax != 0 || local.clientesDiaAnterior != 0 || local.clientesTotales != 0 || local.gananciasDiaAnterior != 0 || local.gananciasTotales != 0 || local.promedioSueldos != 0)
                    {
                        Grid_Juego_Nuevo_3.Visibility = Visibility.Hidden;
                        Simulacion_comensar.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Label_error_locales.Content = "Algun local no cumple no tener ninguna variable en 0";
                    }
                }
            }
        }

        private void Comensar_simulacion_Click(object sender, RoutedEventArgs e)
        {
            Comensar_simulacion.Visibility = Visibility.Hidden;
            Simulacion.Visibility = Visibility.Visible;


            int precioArriendo = 100;
            int dia = 1;

            File.Create("C:\\Desktop\\reporte.txt");
            FileStream fs = new FileStream("reporte.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);           

            while (dia <= 10)
            {
                Label_Dia_actual.Content = dia;
                foreach (Piso piso in pisos)
                {
                    foreach (Local local in piso.listaLocales)
                    {
                        //aqui va la parte de los clientes
                        int c = local.clientesDiaAnterior;
                        int a = local.areaLocal;
                        int p = ((local.precioMin) + (local.precioMax)) / 2;
                        int h = local.numEmpleados;
                        int cmax = a + (a / 10) * Math.Max(100 - p, 0) / 100 * h; // formula cmax de la guia
                        Random r = new Random();
                        int clientes = r.Next(0, cmax);
                        local.clientesDiaAnterior = clientes;
                        local.clientesTotales += clientes;


                        //aqui van las ganancias
                        int v = r.Next(local.precioMin, local.precioMax);
                        int costoArriendo = local.areaLocal * precioArriendo;
                        int ganancia = v * clientes - (h + costoArriendo);
                        local.gananciasDiaAnterior = ganancia;
                        local.gananciasTotales += ganancia;

                    }
                }
                dia += 1;

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

                sw.WriteLine("cantidad de clientes recepcionados: " + clientes_totales + "\nCantidad de clientes promedio " + promedio_clientes + "\nGanancia total" + ganancias_totales+"\nGanancia promedio del dia"+promedio_ganancias);
                sw.WriteLine("Local con menor ganancia y su ganancia total\nnombre: " + menor_ganancia.nombre + "\nganancias del dia: " + menor_ganancia.gananciasDiaAnterior + "\nganancias totales" + menor_ganancia.gananciasTotales);
                sw.WriteLine("Local con mayor ganancia y su ganancia total\nnombre: " + mayor_ganancia.nombre + "\nganancias del dia: " + mayor_ganancia.gananciasDiaAnterior + "\nganancias totales" + mayor_ganancia.gananciasTotales);
                sw.WriteLine("Local con menor cantidad de clientes atendidos: " + menor_cliente.nombre);
                sw.WriteLine("Local con mayor cantidad de clientes atendidos: " + mayor_cliente.nombre);
            }
        }

        private void Button_Juego_Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Grid_Pantalla_Inicio.Visibility = Visibility.Hidden;
            Grid_Juego_Nuevo.Visibility = Visibility.Visible;
            Grid_Cargar_Juego.Visibility = Visibility.Hidden;
        }

        private void Button_Cargar_partida_Click(object sender, RoutedEventArgs e)
        {
            Grid_Pantalla_Inicio.Visibility = Visibility.Hidden;
            Grid_Juego_Nuevo.Visibility = Visibility.Hidden;
            Grid_Cargar_Juego.Visibility = Visibility.Visible;
        }
    }
}
