using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PED_Guia9_Ejercicio1
{
    public partial class FormMaximizante : Form
    {

        //Inicialización de variables
        int xo, yo, tam; //Variables para valor inicial de X, de Y y de tamaño
        bool ec = false; //Bandera booleana en false
        bool estado = false; //Estado inicializado en falso
        int n = 0, i = 0;//Inicialización de variables

        int[] ArregloNumeros; //Arreglo de números ingresados
        Button[] Arreglo; //Arreglo de botones paransimular valores ingresados

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Limpiamos pantalla y reiniciamos a valores iniciales
            n = 0;
            i = 1;
            tam = tabPage1.Width / 2;
            xo = tam;
            yo = 20;
            tabPage1.Controls.Clear();
            tabPage1.Refresh();
            Array.Resize<int>(ref ArregloNumeros, 1);
            Array.Resize<Button>(ref Arreglo, 1);
        }

        //Inicalización del formulario
        public FormMaximizante()
        {
            InitializeComponent();
            tam = tabPage1.Width / 2; //El tamaño será la mitad del ancho del tabpage
            xo = tam; //El tamaño inicial de X será la mitad del ancho del tabpage
            yo = 20; //El valor inicial en Y será de 20
            txtValor.Focus();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {

                int num = int.Parse(txtValor.Text); //Capturamos el valor ingresado

                Array.Resize<int>(ref ArregloNumeros, i + 1); //Incrementamos el arreglo en base al nuevo valor ingresado
                ArregloNumeros[i] = num; //Asignamos ese valo a la posición i del arreglo
                Array.Resize<Button>(ref Arreglo, i + 1); //Incrementamos el arreglo de botones
                Arreglo[i] = new Button(); //Creamos un nuevo boton i
                Arreglo[i].Text = ArregloNumeros[i].ToString(); //Texto del botón será el valor ingresado de posición i
                Arreglo[i].Height = 50; //Alto del botón
                Arreglo[i].Width = 50; //Ancho del botón
                Arreglo[i].BackColor = Color.GreenYellow; //Color GreenYellow para el botón
                Arreglo[i].Location = new Point(xo, yo) + new Size(-20, 0); //Punto de ubicación

                //Para poder dibujar el árbol y crear los niveles

                if ((i + 1) == Math.Pow(2, n + 1)) //Para saber el nivel en base a los nodos(Si tenemso qe cambiar de nivel)
                {

                    n++; //Incrementamos N
                    tam = tam / 2; //Dividimos de nuevo tam
                    xo = tam; //El valor inicial de xo será el nuevo tam
                    yo += 60; //Incrementamos el Y en 60 para que el siguiente nivel se dibuje 60 espacios más abajo en Y

                }
                else
                {
                    xo += (2 * tam); //Si no hay que cambiar de nivel solo movemos el valor de X
                }

                i++; //Incrementamos I 
                estado = true; //Pasamos estado a true
                ec = false;
                tabPage1.Refresh(); //Refrescamos el tabpage
                txtValor.Clear();
                txtValor.Focus();
                Ordenar();

            }
            catch { MessageBox.Show("Valor no valido"); }
            
        }

        //Metodo heap Maximizante
        public void HeapNumMax()
        {
            ec = true; //Pasamos bandera a true

            int x = Arreglo.Length; //Tomamos la longitud del arreglo
            for (int i = ((x)/2); i > 0; i--) //Desde la mitad de la longitud incrementamos
            {
                Max_num(ArregloNumeros, x, i, ref Arreglo);
            }
        }

        //Metodo para intercambiar valores de heap de numeros
        public void HPN()
        {
            int temp; //Variable temporal
            int x = ArregloNumeros.Length; //Longitud del arreglo valores ingresados

            for (int i = x -1; i >= 1; i--) //Desde un valor menos de la longitud total drecrementamos
            {
                intercambio(ref Arreglo, i, 1); //Intercambio
                temp = ArregloNumeros[i]; //El elemnto i del arreglo a temporal
                ArregloNumeros[i] = ArregloNumeros[1]; //El elemento 1 a la posicion 1
                ArregloNumeros[1] = temp; //El que estaba en temporal a la posición 1
                x--;
            }
        }

        //Metodo paint del tabpage
        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            if (estado) //Si estado es verdadero
            {
                try
                {
                    DibujarArreglo(ref Arreglo, ref tabPage1); //Dibujar arreglo
                    DibujarRamas(ref Arreglo, ref tabPage1, e); //Dibujar Ramas
                }
                catch 
                { }

                estado = false; //pasar estado
            }
        }

        //Metodo para dibujar arreglo
        public void DibujarArreglo(ref Button[] botones, ref TabPage tb)
        {
            for (int j = 1; j < botones.Length; j++)
            {
                tb.Controls.Add(botones[j]);
            }
        }

        //Metodo para dibujar ramas
        public void DibujarRamas(ref Button[] botones, ref TabPage tb, PaintEventArgs e)
        {
            Pen lapiz = new Pen(Color.Gray, 1.5f);
            Graphics g;
            g = e.Graphics;

            for (int j = 1; j < Arreglo.Length; j++) //Para todos los elementos del arreglo 
            {
                if (Arreglo[(2 * j)] != null) //Mientras el arreglo no esté vacio
                    g.DrawLine(lapiz, Arreglo[j].Location.X, Arreglo[j].Location.Y + 20, Arreglo[(2 * j)].Location.X + 20, Arreglo[(2 * j)].Location.Y);
                if (Arreglo[(2 * j) + 1] != null) //Mientra no haya solo un elemento
                    g.DrawLine(lapiz, Arreglo[j].Location.X + 40, Arreglo[j].Location.Y + 20, Arreglo[(2 * j) + 1].Location.X + 20, Arreglo[(2 * j) + 1].Location.Y);
            }
        }

        //Para numero maximo en heap
        public void Max_num(int[] a, int x, int indice, ref Button[] botones)
        {
            int left = (indice) * 2;
            int right = (indice * 2) + 1;
            int may = 0;

            if (left < x && a[left] > a[indice])
            {
                may = left;
            }
            else
            {
                may = indice;
            }
            if (right < x && a[left] > a[indice])
            {
                may = right;
            }

            if (may != indice) //Si mayor es distinto de indice
            {
                int temp = a[indice]; //Valor con idice a temporal
                a[indice] = a[may]; //El valor se almacena en posición del indice
                a[may] = temp; //El valor temporal se almacena en mayor

                intercambio(ref Arreglo, may, indice); //Se llama a intercambio
                Max_num(a, x, may, ref botones); //Llamada recursica a Max_num
            }
        }

        public void intercambio(ref Button[] botones, int a, int b)
        {
            string temp = botones[a].Text; //Dejamos valores en un temporal

            Point pa = botones[a].Location; //Sacamos los puntos ubicados en a
            Point pb = botones[b].Location; //Sacamos los puntos ubicados en b

            int diferenciaX = Math.Abs(pa.X - pb.X); //Sacamos la diferencia entre sus X
            int diferenciaY = Math.Abs(pa.Y - pb.Y); //Sacamos la diferencia entre sus X

            int x = 10;
            int y = 10;
            int t = 70;

            while (y != diferenciaY + 10) //Mientras no llegue a la posición esperada en Y
            {
                Thread.Sleep(t); //Dormir durante 70 milisegundos
                botones[a].Location += new Size(+10, 0); //Movemos +10 a
                botones[b].Location += new Size(-10, 0); //Movemos -10 b
                x += 10;
            }
            while (x != diferenciaX - diferenciaX % 10 +10) //Mienstras no llegue a la posición esperada en X
            {
                if (pa.X < pb.X) //Si X de a, es menor que X de b
                {
                    Thread.Sleep(t);
                    botones[a].Location += new Size(+10, 0); //Movemos +10 a
                    botones[b].Location += new Size(-10, 0); //Movemos -10 b
                    x += 10;
                }
                else
                {
                    Thread.Sleep(t);
                    botones[a].Location += new Size(-10, 0); //Movemos -10 a
                    botones[b].Location += new Size(+10, 0); //Movemos +10 b
                    x += 10;
                }
            }

            botones[a].Text = botones[b].Text; //Valor de B muestra en a
            botones[b].Text = temp; //El valor temporal almacenado se mstrará en b
            botones[b].Location = pb; //Nuevo pb, se almacenará ubicación 
            botones[a].Location = pa; //Nuevo pa, se almacenará ubicación
            estado = true; //Estado en true

            tabPage1.Refresh(); //Se refresca tabPage
        }

        public void Ordenar()
        {
            if (i == 0)
            {
                MessageBox.Show("No hay elementos que orden");
            }
            else
            {
                btnClear.Enabled = false; //Desactivamos el boton de limpiar
                btnadd.Enabled = false;
                this.Cursor = Cursors.WaitCursor; //Hacemos que el cursor espere y mostrarnos que está pasando

                if (!ec)
                {
                    HeapNumMax(); //Llamamos el heap num
                }
                else
                {
                    HPN(); //Llamamos hpn
                }

                btnadd.Enabled = true;
                btnClear.Enabled = true;
                this.Cursor = Cursors.Default; //Regresamos el cursor a su forma normal
            }

        }


    }
}
