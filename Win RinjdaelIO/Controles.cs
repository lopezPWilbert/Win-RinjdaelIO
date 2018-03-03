using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Win_RinjdaelIO
{
    class Controles
    {
        /*
        -Declarar global
            Controles controles = new Controles();

        -en form load:
            controles.minimizar(pictureBox1, this);
            controles.restaurar_maxim(pictureBox2, this);
            controles.cerrar(pictureBox3, this);

        
        -Movimiento de ventana
            [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
            private extern static void ReleaseCapture();
            [DllImport("user32.DLL", EntryPoint = "SendMessage")]
            private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);   

        -Importar imagenes
            Minimizar_RdV_B
            Normal_RdV
            Maximizar_RdV
            Cerrar_RdV_A

        */
        bool normal = true;

        public void sobre_control(PictureBox control)
        {
            control.BackColor = Color.FromArgb(127, 127, 127);

        }
        public void dejar_control(PictureBox control)
        {
            control.BackColor = Color.FromArgb(83, 83, 83);
        }
        public void clic_control(PictureBox control)
        {
            control.BackColor = Color.FromArgb(61, 61, 61);
        }
        public void posicion(PictureBox control)
        {
            control.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
        public void minimizar(PictureBox control, Form forma)
        {
            control.Image = Rinjdael.IO.Properties.Resources.Minimizar_RdV_B;
            control.Click += delegate { forma.WindowState = FormWindowState.Minimized; };
            control.MouseDown += delegate { clic_control(control); };
            control.MouseLeave += delegate { dejar_control(control); };
            control.MouseMove += delegate { sobre_control(control); };
            posicion(control);
        }
        public void restaurar_maxim(PictureBox control, Form forma)
        {
            control.Image = Rinjdael.IO.Properties.Resources.Normal_RdV;

            control.Click += delegate {
                if (normal == false)
                {
                    //this.Width = 918;
                    //this.Height = 334;
                    forma.Size = new Size(550, 500);
                    forma.WindowState = FormWindowState.Normal;
                    control.Image = Rinjdael.IO.Properties.Resources.Normal_RdV;
                    normal = true;
                }
                else
                {
                    forma.Size = new Size(1000, 550);
                    //forma.WindowState = FormWindowState.Maximized;
                    control.Image = Rinjdael.IO.Properties.Resources.Maximizar_RdV;
                    normal = false;
                }
            };

            control.MouseDown += delegate { clic_control(control); };
            control.MouseLeave += delegate { dejar_control(control); };
            control.MouseMove += delegate { sobre_control(control); };
            posicion(control);
        }
        public void cerrar(PictureBox control, Form forma)
        {
            control.Image = Rinjdael.IO.Properties.Resources.Cerrar_RdV_A;
            control.Click += delegate { Application.Exit(); };
            control.MouseDown += delegate { clic_control(control); };
            control.MouseLeave += delegate { dejar_control(control); };
            control.MouseMove += delegate { sobre_control(control); };
            posicion(control);
        }
    }
}
