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
using System.IO;
using System.Collections.ObjectModel;

namespace ProyectoMall3
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {

        StreamWriter sw;


        int horas_dia = -1;
        int dinero_inicial = -1;
        int numero_pisos;
        List<Piso> pisos = new List<Piso>();

        int dia;
        int precioArriendo = 100;

        int number; // se ocupa para la funcion int.TryParse

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer(); //timer que se encarga que la simulacion no corra completa de una

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
            ButtonAceptar2.Visibility = Visibility.Visible;
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
                //ComboBoxItem a = new ComboBoxItem(); a.Content = "ropa";
                //ComboBoxItem b = new ComboBoxItem(); b.Content = "hogar";
                //ComboBoxItem c = new ComboBoxItem(); c.Content = "alimento";
                //ComboBoxItem d = new ComboBoxItem(); d.Content = "ferreteria";
                //ComboBoxItem f = new ComboBoxItem(); f.Content = "tecnologia";

                string a =  "ropa";
                string b =  "hogar";
                string c =  "alimento";
                string d =  "ferreteria";
                string f =  "tecnologia";

                ComboBox_Sub_Categorias_Locales.Items.Add(a); ComboBox_Sub_Categorias_Locales.Items.Add(b); ComboBox_Sub_Categorias_Locales.Items.Add(c); ComboBox_Sub_Categorias_Locales.Items.Add(d); ComboBox_Sub_Categorias_Locales.Items.Add(f);


                //  ComboBox_Sub_Categorias_Locales.Items.Add(a);
            }
            if (ComboBox_Categorias_Locales.SelectedIndex == 1)
            {
                //ComboBoxItem a = new ComboBoxItem(); a.Content = "comida rapida";
                //ComboBoxItem b = new ComboBoxItem(); b.Content = "restaurant";

                string a = "comida rapida";
                string b = "restaurant";

                ComboBox_Sub_Categorias_Locales.Items.Add(a); ComboBox_Sub_Categorias_Locales.Items.Add(b);
                

            }


            if (ComboBox_Categorias_Locales.SelectedIndex == 2)
            {
                string a = "Cine";
                string b =  "Juegos";
                string c = "Bowling";

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
                Label_error_locales.Content = "";
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

        //comiensa la simulacion aqui
        private void Comensar_simulacion_Click(object sender, RoutedEventArgs e)
        {


            File.Delete("reportes.txt");
            

           

            foreach (Piso piso in pisos)
            {
                Combobox_pisos_simulacion.Items.Add(piso);
            }


            Comensar_simulacion.Visibility = Visibility.Hidden;
            Simulacion.Visibility = Visibility.Visible;


            
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();


        }

        // esta ese la simulacion en si
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            

            dia++;
            if (dia <= 10)
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
                Local asd = new Local("a", "a", 0, 23, 1, 23, 4);
                Local menor_ganancia = new Local("ninguno", "a", 0, 0, 0, 0, 0) { gananciasDiaAnterior = 2147483647 };
                Local mayor_ganancia = new Local("ninguno", "a", 0, 0, 0, 0, 0);
                Local menor_cliente = new Local("ninguno", "a", 0, 0, 0, 0, 0) { clientesDiaAnterior = 2147483647 };
                Local mayor_cliente = new Local("ninguno", "a", 0, 0, 0, 0, 0);

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

                L_rep_2.Content = "Cantidad de Clientes Recepcionados: " + clientes_totales;
                L_rep_3.Content = "Cantidad de Cliente Promedio por Dia: " + promedio_clientes;
                L_rep_4.Content = "Ganancia total del dia: " + ganancias_totales;
                L_rep_5.Content = "Ganancia del dia:" + promedio_ganancias;
                L_rep_6.Content = "Local con mayor ganancia total: " + mayor_ganancia.nombre;
                L_rep_7.Content = "Local con menor ganancia total: " + menor_ganancia.nombre;
                L_rep_8.Content = "Local con mayor cantidad de clientes: " + mayor_cliente;
                L_rep_9.Content = "Local con menor cantidad de clientes: " + menor_cliente;


                string path = "reportes.txt";
                using (StreamWriter sw = File.AppendText(path))
                {

                    sw.Write("dia: " + dia + "\r\n");
                    sw.Write("Cantidad de Clientes Recepcionados: " + clientes_totales + "\r\n");
                    sw.Write("Cantidad de Cliente Promedio por Dia: " + promedio_clientes + "\r\n");
                    sw.Write("Ganancia total del dia: " + ganancias_totales + "\r\n");
                    sw.Write("Ganancia del dia:" + promedio_ganancias + "\r\n");
                    sw.Write("Local con mayor ganancia total: " + mayor_ganancia.nombre + "\r\n");
                    sw.Write("Local con menor ganancia total: " + menor_ganancia.nombre + "\r\n");
                    sw.Write("Local con mayor cantidad de clientes: " + mayor_cliente + "\r\n");
                    sw.Write("Local con menor cantidad de clientes: " + menor_cliente + "\r\n");

                }



                dinero_inicial += ganancias_totales;

                Label_Dia_actual.Content = dia;
                Label_Dinero_actual.Content = dinero_inicial;

                if (dia == 10)
                {
                    Terminar.Visibility = Visibility.Visible;
                    Pausa.Visibility = Visibility.Hidden;

                }

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



        private void Button_elejir_partida_grabada_Click(object sender, RoutedEventArgs e)
        {

      
            Microsoft.Win32.OpenFileDialog browser = new Microsoft.Win32.OpenFileDialog();

            browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            browser.DefaultExt = ".txt";


            Nullable<bool> result = browser.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = browser.FileName;
                textBox1.Text = filename;
            }


        }

        int index_combox_pisos;
        private void Combobox_pisos_simulacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Stackpanel_detalles_local_simulacion.Visibility = Visibility.Hidden;
            index_listbox_locales_2 = -1;
            Combobox_Locales_simulacion.SelectedIndex = -1;
            Combobox_Locales_simulacion.Items.Clear();
            int n = Combobox_pisos_simulacion.SelectedIndex;
            foreach (Local local in pisos[n].listaLocales)
            {
                Combobox_Locales_simulacion.Items.Add(local);
            }
            index_combox_pisos = Combobox_pisos_simulacion.SelectedIndex;
        }


        int index_listbox_locales_2;
        private void Combobox_Locales_simulacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index_listbox_locales_2 = Combobox_Locales_simulacion.SelectedIndex;
            if (index_listbox_locales_2 != -1)
            {
                index_listbox_locales_2 = Combobox_Locales_simulacion.SelectedIndex;
                Stackpanel_detalles_local_simulacion.Visibility = Visibility.Visible;
                Local local = pisos[index_combox_pisos].listaLocales[index_listbox_locales_2];
                Label_nombre_local_simulacion.Content = local.nombre;
                Label_numero_empleados_simulacion.Content = local.numEmpleados;
                Label_categoria_local_simulacion.Content = local.categoria;
                Label_area_local_simulacion.Content = local.areaLocal;
                Label_sueldo_promedio_simulacion.Content = local.promedioSueldos;
                Label_precio_maximo_simulacion.Content = local.precioMax;
                Label_precio_minimo_simulacion.Content = local.precioMin;
            }
          

        }

        private void Pausa_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            Pausa.Visibility = Visibility.Hidden;
            Resumir.Visibility = Visibility.Visible;
        }

        private void Resumir_Click(object sender, RoutedEventArgs e)
        {
            Pausa.Visibility = Visibility.Visible;
            Resumir.Visibility = Visibility.Hidden;
            dispatcherTimer.Start();
        }

        private void Terminar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            Grid_guardar.Visibility = Visibility.Visible;
            Simulacion.Visibility = Visibility.Hidden;
            dispatcherTimer.Stop();
   
        }

        private void Boton_seleccionar_ruta_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog browser = new Microsoft.Win32.SaveFileDialog();

            browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            browser.DefaultExt = ".txt";


            Nullable<bool> result = browser.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = browser.FileName;
                Tbox_ruta_a_cargar.Text = filename;
            }

        }

        private void Boton_Cancelar_guardar_Click(object sender, RoutedEventArgs e)
        {
            Grid_guardar.Visibility = Visibility.Hidden;
            Simulacion.Visibility = Visibility.Visible;
            dispatcherTimer.Start();
            Pausa.Visibility = Visibility.Visible;
            Resumir.Visibility = Visibility.Hidden;
        }

        private void Boton_guardar_aceptar_Click(object sender, RoutedEventArgs e)
        {
            string ruta = Tbox_ruta_a_cargar.Text;

            File.Delete(ruta);
            StreamWriter saver = File.CreateText(ruta);
            
            saver.Write("qo9872i21efoh" + "\r\n");
            saver.Write("dia " + dia + "\r\n");
            saver.Write("dinero " + dinero_inicial + "\r\n");

            saver.Write("iniciando pisos \r\n");
            foreach (Piso piso in pisos)
            {
                saver.Write(piso.altura.ToString() + " " + piso.area.ToString() + "\r\n");
                saver.Write("locales \r\n");
                foreach(Local loc in piso.listaLocales)
                {
                    saver.Write(loc.nombre + " " +  loc.categoria+ " " + loc.numEmpleados +" " + loc.areaLocal + " " + loc.precioMin + " " + loc.precioMax + " " + loc.clientesDiaAnterior + " " + loc.clientesTotales + " " + loc.gananciasDiaAnterior + " " + loc.gananciasDiaAnterior+ " " + loc.promedioSueldos);
                    saver.Write("\r\n");
                }
                saver.Write("fin_locales \r\n");

            }
            saver.Write("terminando");
            saver.Close();
            Grid_guardar.Visibility = Visibility.Hidden;
            Simulacion.Visibility = Visibility.Visible;
            dispatcherTimer.Start();
            Pausa.Visibility = Visibility.Visible;
            Resumir.Visibility = Visibility.Hidden;

        }

        int altura_piso;
        int area_piso;
        
        private void Button_YEP_Click(object sender, RoutedEventArgs e)
        {
            string ruta = textBox1.Text;
            List<Local> locales = new List<Local>();
            if (File.Exists(ruta))
            {

                bool edit_piso = false;
                bool edit_locales = false;
                StreamReader sr = new StreamReader(ruta);
                string linea = sr.ReadLine();
                if (linea == "qo9872i21efoh")
                {
                    while((linea = sr.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(' ');
                        
                        if (datos[0] == "dia"){ dia = Int32.Parse(datos[1]);}
                        if (datos[0] == "dinero") {dinero_inicial = Int32.Parse(datos[1]);}
                        

                        if (edit_piso == true && edit_locales == false)
                        {
                            string a = datos[0];
                            string b = datos[1];
                            int altura_piso = Int32.Parse(a);
                            int area_piso = Int32.Parse(b);
                        }

                        if (datos.Length > 4) ;
                        {
                            string d0 = datos[0];
                            string d1 = datos[1];
                            string d2 = datos[2];
                            int w2 = Int32.Parse(d2);
                            string d3 = datos[3];
                            int w3 = Int32.Parse(d3);
                            string d4 = datos[4];
                            int w4 = Int32.Parse(d4);
                            string d5 = datos[5];
                            int w5 = Int32.Parse(d5);
                            string d6 = datos[6];
                            int w6 = Int32.Parse(d6);
                            string d7 = datos[7];
                            int w7 = Int32.Parse(d7);

                            Local a = new Local(d1,d2,w3,w4,w5,w6,w7);
                            locales.Add(a);
                        }

                        if (datos[0] == "locales")
                        {
                            edit_piso = false;
                            edit_locales = true;
                        }
                        //if (datos[0] == "iniciando") { edit_piso = true; }
                        
                        if (datos[0] == "fin_locales")
                        {
                            edit_locales = false;
                            edit_piso = false;
                            Piso b = new Piso(altura_piso, area_piso, locales);
                            locales = new List<Local>();
                            pisos.Add(b);
                        }

                    }
                }
            }

            Grid_Cargar_Juego.Visibility = Visibility.Hidden;
            Simulacion_comensar.Visibility = Visibility.Visible;
        }

        private void Button_NOPE_Click(object sender, RoutedEventArgs e)
        {
            Grid_Cargar_Juego.Visibility = Visibility.Hidden;
            Grid_Pantalla_Inicio.Visibility = Visibility.Visible;
        }

        private void Button_cargar_vista_previa_Click(object sender, RoutedEventArgs e)
        {
            string ruta = textBox1.Text;
            if (File.Exists(ruta))
            {

                StreamReader sr = new StreamReader(ruta);
                string linea = sr.ReadLine();
                if (linea == "qo9872i21efoh")
                {
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(' ');

                        if (datos[0] == "dia") { Label_dia_cargar.Content = datos[1]; }
                        if (datos[0] == "dinero") { Label_Dinero_Cargar.Content = datos[1]; }
                    }
                }
            }
        }
    }
}

