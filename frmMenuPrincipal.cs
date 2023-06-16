using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using CFACADEFUN;
using CFACADESTRUC;

namespace MENUPRINCIPAL
{
    public partial class frmMenuPrincipal : Form
    {
        CEstructura ep = new CEstructura();
        string sTitulo = "MENU PRINCIPAL...";
        //string sIp = "", sBd = "", sUsuario = "", sPass = "";

        public frmMenuPrincipal()
        {
            InitializeComponent();

            //this.IsMdiContainer = true;
        }

        /*
         * Catalogos:
         *  0 -> Usuario
         *  1 -> Coordinador
         *  2 -> Calidad
         *  3 -> Jefatura
         */

        private void frmMenuPrincipal_Load(object sender, EventArgs e)
        {
            string line = "", sRuta = @"C:\LESP\LOGIN.dll", NombreDelForm = "LOGIN.frmLogin";
            Int32 nContador = 0;

            if (File.Exists(@"C:\LESP\DarSys.txt"))
            {
                File.Delete(@"C:\LESP\DarSys.txt");
            }

            ep.IpMaquina = CFuncionesGral.consultarsIp();
            File.Copy(@"C:\LESP\DarSys.dll", @"C:\LESP\DarSys.txt");

            StreamReader file = new StreamReader(@"C:\LESP\DarSys.txt");
            while ((line = file.ReadLine()) != null)
            {
                switch (nContador)
                {
                    case 0:
                        ep.Ip = line.ToString().Trim();
                        break;
                    case 1:
                        ep.BaseDeDatos = line.ToString().Trim();
                        break;
                    case 2:
                        ep.Usuario = line.ToString().Trim();
                        break;
                    case 3:
                        ep.Puerto = Convert.ToInt32(line.ToString().Trim());
                        break;
                    default:
                        break;
                }

                nContador++;
            }
            file.Close();

            if (File.Exists(@"C:\LESP\DarSys.txt"))
            {
                File.Delete(@"C:\LESP\DarSys.txt");
            }

            fCargarDll(sRuta, NombreDelForm, 1);

            if (ep.AccederMenu != 1)
            {
                MessageBox.Show("Usuario no logueado.", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                this.Text = "MENU PRINCIPAL                                                                                                                                                                                          BIENVENIDO [ " + ep.NombreUsuario.ToString().Trim() + " ] ";
                MenuStrip menuPrincipal = new MenuStrip();
                List<Menu> listMenu = new List<Menu>();

                fMenu(ref listMenu);

                foreach (Menu m in listMenu)
                {
                    ToolStripMenuItem MenuItem = new ToolStripMenuItem(m.Nombre, null, menu_selected);

                    if (m.SubMenu != null)
                    {
                        foreach(SubMenu sub in m.SubMenu)
                        {
                            ToolStripMenuItem Sub = new ToolStripMenuItem(sub.Nombre, null, submenu_selected);
                            MenuItem.DropDownItems.Add(Sub);
                        }
                    }

                    menuPrincipal.Items.Add(MenuItem);
                }


                this.MainMenuStrip = menuPrincipal;
                Controls.Add(menuPrincipal);
            }
        }

        public static void fMenu(ref List<Menu> listMenu)
        {
            listMenu = new List<Menu>()
            {
                new Menu()
                {
                    Nombre = "Articulos en almacen",
                    SubMenu = new List<SubMenu>()
                    {
                        new SubMenu()
                        {
                            Nombre = "Alta de un articulo"
                        },
                        new SubMenu()
                        {
                            Nombre = "Cambio de un articulo"
                        },
                        new SubMenu()
                        {
                            Nombre = "Baja de un articulo"
                        },
                        new SubMenu()
                        {
                            Nombre = "---------------------"
                        },
                        new SubMenu()
                        {
                            Nombre = "Entrada al almacen"
                        },
                        new SubMenu()
                        {
                            Nombre = "Salida del almacen"
                        },
                        new SubMenu()
                        {
                            Nombre = "---------------------"
                        },
                        new SubMenu()
                        {
                            Nombre = "Listado de inventario"
                        }
                    }
                },
                new Menu()
                {
                    Nombre = "Salir"
                }
            };
        }

        public bool fCargarDll(string sRuta, string sNombreForm, Int16 nOpcion)
        {
            bool valorRegresa = false;

            try
            {
                if (nOpcion == 0)
                {
                    CControl objForm = new CControl();
                    Assembly DllaCargar = Assembly.LoadFile(sRuta);
                    Type DllType = DllaCargar.GetType(sNombreForm);
                    object miObjetoDll = Activator.CreateInstance(DllType, ep);
                    objForm = (CControl)miObjetoDll;
                    objForm.ShowDialog();
                }
                else
                {
                    Form objForm = new Form();
                    Assembly DllaCargar = Assembly.LoadFile(sRuta);
                    Type DllType = DllaCargar.GetType(sNombreForm);
                    object miObjetoDll = Activator.CreateInstance(DllType, ep);
                    objForm = (Form)miObjetoDll;
                    objForm.ShowDialog();
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(ioEx.Message.ToString(), "Error al Cargar DLL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), "Error al Cargar DLL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return valorRegresa;
        }

        private void submenu_selected(object sender, EventArgs e)
        {
            string sRuta = "", sNombreDelForm = "";
            bool bContinuar = true;
            ep.Opcion = 0;

            switch (sender.ToString())
            {
                case "Alta de un articulo":
                    {
                        sRuta = @"C:\LESP\Almacen.dll";
                        sNombreDelForm = "Almacen.frmAltaArticulos";
                    }
                    break;
                case "Cambio de un articulo":
                    {
                        sRuta = @"C:\LESP\Almacen.dll";
                        sNombreDelForm = "Almacen.frmAltaArticulos";
                        ep.Opcion = 1;
                    }
                    break;
                case "Baja de un articulo":
                    {
                        sRuta = @"C:\LESP\Almacen.dll";
                        sNombreDelForm = "Almacen.frmAltaArticulos";
                        ep.Opcion = 2;
                    }
                    break;
                case "Entrada al almacen":
                    {
                        sRuta = @"C:\LESP\MANEJOINVENTARIO.dll";
                        sNombreDelForm = "MANEJOINVENTARIO.frmManejoDeInventario";
                    }
                    break;
                case "Salida del almacen":
                    {
                        sRuta = @"C:\LESP\MANEJOINVENTARIO.dll";
                        sNombreDelForm = "MANEJOINVENTARIO.frmManejoDeInventario";
                        ep.Opcion = 1;
                    }
                    break;
                case "Listado de inventario":
                    {
                        sRuta = @"C:\LESP\CONSULTARINFOALMACEN.dll";
                        sNombreDelForm = "CONSULTARINFOALMACEN.frmConsultarInformacion";
                        ep.Opcion = 1;
                    }
                    break;
                default:
                    bContinuar = false;
                    break;
            }

            if (bContinuar)
            {
                fCargarDll(sRuta, sNombreDelForm, 0);
            }
        }

        private void menu_selected(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "Salir":
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        public class Menu
        {
            public string Nombre { get; set; }
            public List<SubMenu> SubMenu { get; set; }
        }

        public class SubMenu
        {
            public string Nombre { get; set; }
        }
    }
}
