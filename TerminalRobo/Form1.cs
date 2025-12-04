using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TerminalRobo.Models;
using System.Runtime.InteropServices;
using System.Configuration;
using static TerminalRobo.Models.ItapoaAPI;
using System.ServiceModel.Security;

namespace TerminalRobo
{
    public partial class Form1 : Form
    {

        Navegacao navegar;
        Dados dados = new Dados();
        const int idTermSB = 188;
        const int idTermEmbraport = 206;
        const int idBTP = 210;
        const int idTermVilaConde = 335;
        const int idTermItajai = 2072;
        const int idTermItapoa = 366;
        const int idTermParanagua = 220;



        string icConsultarSantosBrasil = ConfigurationManager.AppSettings["icConsultarSantosBrasil"].ToString();
        string icConsultarEmbraport = ConfigurationManager.AppSettings["icConsultarEmbraport"].ToString();
        string icConsultarBTP = ConfigurationManager.AppSettings["icConsultarBTP"].ToString();
        string icConsultarVilaConde = ConfigurationManager.AppSettings["icConsultarVilaConde"].ToString();
        string icConsultarItajai = ConfigurationManager.AppSettings["icConsultarItajai"].ToString();
        string icConsultarItapoa = ConfigurationManager.AppSettings["icConsultarItapoa"].ToString();
        string icConsultarParanagua = ConfigurationManager.AppSettings["icConsultarParanagua"].ToString();
        string icEnviarEmailAnalista = ConfigurationManager.AppSettings["icEnviarEmailAnalista"].ToString();
        string icrobo = ConfigurationManager.AppSettings["nrRobo"].ToString();

        

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x000000004,
            MIDDLEDOWN = 0X00000040,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x0000800,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        public static void LeftMouseClick(Point p)
        {
            Cursor.Position = p;
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        public Form1()
        {
            InitializeComponent();
            navegar = new Navegacao(ref tsStatus, ref tsTerminal);
            //this.Load += new EventHandler(Form1_Load);
            pnlCentro.Controls.Add(navegar.CarregarBrowser());

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
           
            tmrPrintsTelas.Enabled = false;


            IniciarConsultaEmbraportTela();


            tmrPrintsTelas.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            tmrPrintsTelas.Enabled = false;

            IniciarConsultaBTPTela();

            tmrPrintsTelas.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            tmrPrintsTelas.Enabled = false;

            navegar.EntrarPaginaPortoNaveTela(txtContainer);


            navegar.EntrarPaginaTeconTela(txtContainer);


            IniciarConsultasPrintsTelas();


            tmrPrintsTelas.Enabled = true;

        }
     


        private bool IniciarConsultasPrintsTelas()
        {

            navegar.EntrarPaginaPortoNaveTela(txtContainer);

            navegar.EntrarPaginaBTPTela(txtContainer);

            navegar.EntrarPaginaEmbraportTela(txtContainer);

            navegar.EntrarPaginaTeconTela(txtContainer);




            return true;


        }

        private bool IniciarConsultaPortoNave()
        {

            navegar.EntrarPaginaPortoNaveTela(txtContainer);
            return true;

        }


        private bool IniciarConsultaBTPTela()
        {

         
            navegar.EntrarPaginaBTPTela(txtContainer);

            return true;
        }

        private bool IniciarConsultaTECONTELA()
        {

            navegar.EntrarPaginaTeconTela(txtContainer);

            return true;
        }


        private bool IniciarConsultaEmbraportTela()
        {


            navegar.EntrarPaginaEmbraportTela(txtContainer);

            return true;
        }


   

      

        private void Form1_Load(object sender, EventArgs e)
        {
            string intervalString = ConfigurationManager.AppSettings["TimerInterval"];
            int interval;

            if (int.TryParse(intervalString, out interval))
            {
                tmrPrintsTelas.Interval = interval;
                tmrPrintsTelas.Start();
            }
            else
            {
                MessageBox.Show("Intervalo inválido no web.config.");
            }
            navegar.AtualizaLoginsSenhas();
            //MessageBox.Show(tmrConsulta.Interval.ToString());
            tmrPrintsTelas.Enabled = true;
           
        }


        private void btnSenhas_Click(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();
        }

        private void tmrPrintsTelas_Tick(object sender, EventArgs e)
        {
            tmrPrintsTelas.Enabled = false;

            
            IniciarConsultasPrintsTelas();

            tmrPrintsTelas.Enabled = true;

        }
    }
}
