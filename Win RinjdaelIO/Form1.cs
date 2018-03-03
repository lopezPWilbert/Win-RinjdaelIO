using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;

namespace Win_RinjdaelIO
{
    public partial class Form1 : Form
    {
        //Clipboard cop1;
        //string caption;
        Controles controles = new Controles();

        //Movimiento de ventana
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public void mouse()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        public Form1()
        {
            
            InitializeComponent();
            
            
        }
        string cadenaX;
        public string funcion(string cadena, string strKey,bool encriptar)
        {

            
            string strIv = File.ReadAllText(toolStripTextBox1.Text+"iv.rpt");

            int keySize = 32;
            int ivSize = 16;

            byte[] key = UTF8Encoding.UTF8.GetBytes(strKey);
            byte[] iv = UTF8Encoding.UTF8.GetBytes(strIv);

            Array.Resize(ref key, keySize);
            Array.Resize(ref iv, ivSize);

            if (encriptar==true)
            {
                 cadenaX = encryptString(cadena, key, iv);
                return cadenaX;
            }
            if (encriptar==false)
            {

                //descifrado
                 cadenaX = decryptString(cadena, key, iv);
                return cadenaX;
            }
            return cadenaX;
            
        }


        public static string encryptString(String plainMessage, byte[] Key, byte[] IV)
        {
            // Crear una instancia del algoritmo de Rijndael

            Rijndael RijndaelAlg = Rijndael.Create();

            // Establecer un flujo en memoria para el cifrado

            MemoryStream memoryStream = new MemoryStream();

            // Crear un flujo de cifrado basado en el flujo de los datos

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         RijndaelAlg.CreateEncryptor(Key, IV),
                                                         CryptoStreamMode.Write);

            // Obtener la representación en bytes de la información a cifrar

            byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(plainMessage);

            // Cifrar los datos enviándolos al flujo de cifrado

            cryptoStream.Write(plainMessageBytes, 0, plainMessageBytes.Length);

            cryptoStream.FlushFinalBlock();

            // Obtener los datos datos cifrados como un arreglo de bytes

            byte[] cipherMessageBytes = memoryStream.ToArray();

            // Cerrar los flujos utilizados

            memoryStream.Close();
            cryptoStream.Close();

            memoryStream.Dispose();
            cryptoStream.Dispose();
            // Retornar la representación de texto de los datos cifrados

            return Convert.ToBase64String(cipherMessageBytes);
        }
        public static string decryptString(String encryptedMessage, byte[] Key, byte[] IV)
        {
            // Obtener la representación en bytes del texto cifrado

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedMessage);

            // Crear un arreglo de bytes para almacenar los datos descifrados

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Crear una instancia del algoritmo de Rijndael

            Rijndael RijndaelAlg = Rijndael.Create();

            // Crear un flujo en memoria con la representación de bytes de la información cifrada

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Crear un flujo de descifrado basado en el flujo de los datos

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         RijndaelAlg.CreateDecryptor(Key, IV),
                                                         CryptoStreamMode.Read);

            // Obtener los datos descifrados obteniéndolos del flujo de descifrado

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            // Cerrar los flujos utilizados

            memoryStream.Close();
            cryptoStream.Close();
            memoryStream.Dispose();
            cryptoStream.Dispose();
            // Retornar la representación de texto de los datos descifrados

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static string iv()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[254];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        bool cifrar;
        string contraseña, apuntes;
       
        public void abrir()
        {
            cifrar = false;


           /* try
            {*/
                if (System.IO.File.Exists(toolStripTextBox1.Text + ".rpt"))
                {
                
                string archivo = toolStripTextBox1.Text + ".rpt";
                FileStream flujoarchivo = new FileStream(archivo, FileMode.Open, FileAccess.Read);
                StreamReader lector = new StreamReader(flujoarchivo);
                // textBox1 = openFileDialog1.FileName;
                //textBox2.Clear();
                while (lector.Peek() >= 0)
                {
                    apuntes += lector.ReadToEnd();
                }
                lector.Close();

                //apuntes = File.ReadAllText(toolStripTextBox1.Text + ".rpt");

                string cadenaX = funcion(apuntes, toolStripTextBox2.Text, cifrar);
                richTextBox1.Text = cadenaX;
                contraseña = toolStripTextBox2.Text;
            }
                else
                {
                nuevo();
            }
                
                /*
            }
            catch (Exception)
            {

                MessageBox.Show("ERROR: Contraseña incorrecta o cifrado erroneo.\r\nSi el archivo esta vacío, agregue nuevo texto y nueva contraseña.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripTextBox2.Clear();
                toolStripTextBox2.Focus();
            }*/
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {



            if (apuntes != richTextBox1.Text)
            {
                
                switch (MessageBox.Show("¿Desea guardar los cambios hechos?", "ADVERTENCIA", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        //Guardar texto
                        /*string vector = iv();
                        File.WriteAllText(toolStripTextBox1.Text+"iv.rpt", vector);

                        cifrar = true;
                        string cadenaX = funcion(richTextBox1.Text, toolStripTextBox2.Text, cifrar);

                        File.WriteAllText(toolStripTextBox1.Text+".rpt", cadenaX);
                       */
                        string vector = iv();
                        //File.WriteAllText(toolStripTextBox1.Text + "iv.rpt", vector);

                        StreamWriter archivoIV = File.CreateText(toolStripTextBox1.Text + "iv.rpt");
                        archivoIV.Write(vector);
                        archivoIV.Close();

                        cifrar = true;
                        string cadenaX = funcion(richTextBox1.Text, toolStripTextBox2.Text, cifrar);

                        StreamWriter archivo = File.CreateText(toolStripTextBox1.Text + ".rpt");
                        archivo.Write(cadenaX);
                        archivo.Close();

                        break;
                    case DialogResult.No:
                        e.Cancel = false;
                        break;
                    default:
                        break;
                }
            }

            }
        
        public  void guardar()
        {
            if (apuntes != richTextBox1.Text)
            {
                if (MessageBox.Show("¿Desea guardar los cambios hechos?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string vector = iv();
                    //File.WriteAllText(toolStripTextBox1.Text+"iv.rpt", vector);

                    StreamWriter archivoIV = File.CreateText(toolStripTextBox1.Text + "iv.rpt");
                    archivoIV.Write(vector);
                    archivoIV.Close();
                    cifrar = true;
                    string cadenaX = funcion(richTextBox1.Text, toolStripTextBox2.Text, cifrar);

                    StreamWriter archivo = File.CreateText(toolStripTextBox1.Text+".rpt");
                    archivo.Write(cadenaX);
                    archivo.Close();


                   // File.WriteAllText(toolStripTextBox1.Text+".rpt", cadenaX);
                }
                else
                {

                    reinicio();

                }
            }

            

        }

        public void reinicio()
        {
            toolStripTextBox1.Enabled = true;
            richTextBox1.Clear();
            richTextBox1.Enabled = false;
            toolStripTextBox1.Clear();
            toolStripTextBox2.Clear();
        }
        //nuevo archivo
        public void nuevo()
        {
            

            if (!File.Exists(toolStripTextBox1.Text+".rpt"))
            {
                FileStream crear = System.IO.File.Create(toolStripTextBox1.Text + ".rpt");
                crear.Close();
            }
            else
            {
                MessageBox.Show("ERROR: Este archivo ya existe.");
                
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            controles.minimizar(pictureBox2, this);
            controles.restaurar_maxim(pictureBox3, this);
            controles.cerrar(pictureBox4, this);

            richTextBox1.Enabled = false;
            toolStripTextBox2.Enabled = false;
            toolStripTextBox1.Focus();
            TextBox tb = this.toolStripTextBox2.Control as TextBox;
            tb.PasswordChar = '•';
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guardar();
            reinicio();
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                
                toolStripTextBox1.Enabled = false;
                toolStripTextBox2.Enabled = true;
                toolStripTextBox2.Focus();
            }
        }

        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                richTextBox1.Enabled = true;
                richTextBox1.Focus();
                toolStripTextBox2.Enabled = false;
                abrir();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string archivo = openFileDialog1.FileName;
                    FileStream flujoarchivo = new FileStream(archivo, FileMode.Open, FileAccess.Read);
                    StreamReader lector = new StreamReader(flujoarchivo);
                    // textBox1 = openFileDialog1.FileName;
                    //textBox2.Clear();
                    reinicio();
                    while (lector.Peek() >= 0)
                    {
                        richTextBox1.Text += lector.ReadLine() + "\r\n";
                    }
                    lector.Close();
                    
                }
            }
            catch (Exception V)
            {
                MessageBox.Show(V.StackTrace);
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guardar();

        }

        private void dESTRUIRToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mouse();
        }

        private void verAyudaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                abrir();
            }
            if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                Close();
            }
        }
        /*Advertencia si no hay caracter al cambiar contraseña.
         if (string.IsNullOrEmpty(toolStripTextBox2.Text) || string.IsNullOrWhiteSpace(toolStripTextBox2.Text))
            {
                if (MessageBox.Show("ADVERTENCIA: No hay contraseña, ¿Desea guardar el archivo sin contraseña?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    guardar();
                }
                else
                {
                    e.Cancel = true;
                    toolStripTextBox2.Clear();
                    toolStripTextBox2.Focus();
                }
            }
        */
    }
}
