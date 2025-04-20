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
            navegar.AtualizaLoginsSenhas();

            bool confirmaEmbarq = chbConfirma.Checked;
            tmrConsulta.Enabled = false;
            if (IniciarConsultaEmbraport(confirmaEmbarq)) {
                if (icEnviarEmailAnalista == "S")
                {
                    //Verifica se existe divergências para enviar aos analistas do trafego
                    navegar.GerarPlanilhaExcelCliente(icrobo);
                    //Antes de iniciar a consulta limpa a tabela temporária
                    dados.LimparTabelatemporaria(true);
                }
            }
            tmrConsulta.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();

            bool confirmaEmbarq = chbConfirma.Checked;
            tmrConsulta.Enabled = false;
            if (IniciarConsultaBTP(confirmaEmbarq)) {
                if (icEnviarEmailAnalista == "S")
                {
                    //Verifica se existe divergências para enviar aos analistas do trafego
                    navegar.GerarPlanilhaExcelCliente(icrobo);
                    //Antes de iniciar a consulta limpa a tabela temporária
                    dados.LimparTabelatemporaria(true);
                }
            }
            tmrConsulta.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();

            bool confirmaEmbarq = chbConfirma.Checked;
            tmrConsulta.Enabled = false;
            if (IniciarConsultaSantosBrasil(confirmaEmbarq))
            {
                if (icEnviarEmailAnalista == "S")
                {
                    //Verifica se existe divergências para enviar aos analistas do trafego
                    navegar.GerarPlanilhaExcelCliente(icrobo);
                    //Antes de iniciar a consulta limpa a tabela temporária
                    dados.LimparTabelatemporaria(true);
                }
            }
            tmrConsulta.Enabled = true;

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();

            bool confirmaEmbarq = chbConfirma.Checked;
            tmrConsulta.Enabled = false;
            if (IniciarConsultaVilaConde(confirmaEmbarq))
            {
                if (icEnviarEmailAnalista == "S")
                {
                    //Verifica se existe divergências para enviar aos analistas do trafego
                    navegar.GerarPlanilhaExcelCliente(icrobo);
                    //Antes de iniciar a consulta limpa a tabela temporária
                    dados.LimparTabelatemporaria(true);
                }
            }
            tmrConsulta.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();

            bool confirmaEmbarq = chbConfirma.Checked;
            tmrConsulta.Enabled = false;
            if (IniciarConsultaParanagua(confirmaEmbarq))
            {
                if (icEnviarEmailAnalista == "S")
                {
                    //Verifica se existe divergências para enviar aos analistas do trafego
                    navegar.GerarPlanilhaExcelCliente(icrobo);
                    //Antes de iniciar a consulta limpa a tabela temporária
                    dados.LimparTabelatemporaria(true);
                }
            }
            tmrConsulta.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();

            bool confirmaEmbarq = chbConfirma.Checked;
            tmrConsulta.Enabled = false;
            if (IniciarConsultaItapoa(confirmaEmbarq))
            {
                if (icEnviarEmailAnalista == "S")
                {
                    //Verifica se existe divergências para enviar aos analistas do trafego
                    navegar.GerarPlanilhaExcelCliente(icrobo);
                    //Antes de iniciar a consulta limpa a tabela temporária
                    dados.LimparTabelatemporaria(true);
                }
            }
            tmrConsulta.Enabled = true;
        }

        private bool IniciarConsultaSantosBrasil(bool confirmarEmbarque, bool TerminalDivergencia = false, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {

            
            //Entrar no site Santos Brasil
            bool bCarregado = false;
            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>(); 
            List<ListaDeCampos> lstReconsulta = new List<ListaDeCampos>();
            List<ListaDeCampos> lstReconsulta2 = new List<ListaDeCampos>();
            List<ListaDeCampos> lstErros = new List<ListaDeCampos>();

            List<int> lstVA = new List<int>();

            if (TerminalDivergencia)
                lstConsulta = dados.ConsultaContainerTerminal(idTermSB, txtContainer.Text == "" ? null : txtContainer.Text);
            else
            {
                if (confirmarEmbarque)
                    lstConsulta = dados.ConsultaContainerEmbarcados(idTermSB, txtContainer.Text == "" ? null : txtContainer.Text);
                else
                {
                    if (DivergenciaLacre)
                    {
                        lstConsulta = dados.ConsultaLacreTerminal(idTermSB, txtContainer.Text == "" ? null : txtContainer.Text);
                    }
                    else
                    {
                        if (EmbarqueAntesPrevisto)
                            lstConsulta = dados.ConsultaContainerTerminalEmbarque(idTermSB, txtContainer.Text == "" ? null : txtContainer.Text);
                        else
                            lstConsulta = dados.ConsultaContainerDeadLineDia(idTermSB, txtContainer.Text == "" ? null : txtContainer.Text);
                    }
                }
            }

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " CONTAINERS";
                //Loga na página
                tsTerminal.Text = "Entrando na página Santos Brasil";
                Application.DoEvents();
                bCarregado = navegar.EntrarPaginaSantosBrasil();
                tsTerminal.Text = "Consultando Container Santos Brasil";
                Application.DoEvents();
                if (bCarregado)
                {
                    //Consulta o Container
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count + lstReconsulta.Count) + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (DivergenciaLacre)
                        {
                            if (!navegar.ConsultarLacreSantosBrasil(conteudo))
                            {
                                lstReconsulta.Add(conteudo);
                                //Caso de algum erro na consulta tenta logar novamente no site
                                navegar.deslogandoSantosBrasil();
                                Thread.Sleep(300);
                                Application.DoEvents();
                                bCarregado = navegar.EntrarPaginaSantosBrasil();
                                if (!bCarregado)
                                    break;
                            }
                        }
                        else
                        {
                            if (!navegar.ConsultarContainerSantosBrasil(conteudo))
                            {
                                lstReconsulta.Add(conteudo);
                                //Caso de algum erro na consulta tenta logar novamente no site
                                navegar.deslogandoSantosBrasil();
                                Thread.Sleep(300);
                                Application.DoEvents();
                                bCarregado = navegar.EntrarPaginaSantosBrasil();
                                if (!bCarregado)
                                    break;
                            }
                            if (confirmarEmbarque)
                                lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);
                        }
                        i++;
                    }

                    foreach (var item in lstReconsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count + lstReconsulta.Count + lstReconsulta2.Count) + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarContainerSantosBrasil(item))
                        {
                            lstReconsulta2.Add(item);
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogandoSantosBrasil();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaSantosBrasil();
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }

                    foreach (var item in lstReconsulta2)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count + lstReconsulta.Count + lstReconsulta2.Count) + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarContainerSantosBrasil(item))
                        {
                            navegar.AdicionarNaoEncontrado(item, "SANTOS BRASIL");
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogandoSantosBrasil();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaSantosBrasil();
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }
                    tsContainer.Text = "AGUARDANDO";
                    //navegar.deslogandoSantosBrasil();
                    dados.EmbarqueConfirmado(lstVA);
                }
            }
            else {
                tsTerminal.Text = "Nenhum dado encontrado.";
            }
            return bCarregado;
        }

        private bool IniciarConsultaEmbraport(bool confirmarEmbarque, bool TerminalDivergencia = false, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {
            bool bCarregado = false;
            

            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>();
            List<int> lstVA = new List<int>();

            if (TerminalDivergencia)
                lstConsulta = dados.ConsultaContainerTerminal(idTermEmbraport, txtContainer.Text == "" ? null : txtContainer.Text);
            else
            {
                if (confirmarEmbarque)
                    lstConsulta = dados.ConsultaContainerEmbarcados(idTermEmbraport, txtContainer.Text == "" ? null : txtContainer.Text);
                else
                {
                    if (DivergenciaLacre)
                    {
                        lstConsulta = dados.ConsultaLacreTerminal(idTermEmbraport, txtContainer.Text == "" ? null : txtContainer.Text);
                    }
                    else
                    {
                        if (EmbarqueAntesPrevisto)
                            lstConsulta = dados.ConsultaContainerTerminalEmbarque(idTermEmbraport, txtContainer.Text == "" ? null : txtContainer.Text);
                        else
                            lstConsulta = dados.ConsultaContainerDeadLineDia(idTermEmbraport, txtContainer.Text == "" ? null : txtContainer.Text);
                    }
                }
            }


            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " CONTAINERS";
                //Loga na página
                tsTerminal.Text = "Entrando na página DPWord (Embraport)";
                Application.DoEvents();
                bCarregado = navegar.EntrarPaginaEmbraport();
                //Consulta o Container
                if (bCarregado)
                {
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + lstConsulta.Count + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (DivergenciaLacre)
                        {
                            if (!navegar.ConsultarLacreEmbraport(conteudo))
                            {
                                //Caso de algum erro na consulta tenta logar novamente no site
                                bCarregado = navegar.EntrarPaginaEmbraport();
                                if (!bCarregado)
                                    break;
                            }
                        }
                        else
                        {
                            if (!navegar.ConsultarContainerEmbraport(conteudo))
                            {
                                //Caso de algum erro na consulta tenta logar novamente no site
                                bCarregado = navegar.EntrarPaginaEmbraport();
                                if (!bCarregado)
                                    break;
                            }
                            if (confirmarEmbarque)
                                lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);
                        }
                        i++;
                    }
                    tsContainer.Text = "AGUARDANDO";
                    navegar.deslogandoEmbraport();
                    dados.EmbarqueConfirmado(lstVA);
                }
            }
            
            return bCarregado;
           
        }

        private bool IniciarConsultaBTP(bool confirmarEmbarque, bool TerminalDivergencia = false, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {

            string Grupo = ConfigurationManager.AppSettings["Grupo"].ToString();

            string GrupoNao = ConfigurationManager.AppSettings["Grupo_Nao"].ToString();   

            

             bool bCarregado = false;
            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>();
            List<int> lstVA = new List<int>();

            if (TerminalDivergencia)
                lstConsulta = dados.ConsultaContainerTerminalCliente(idBTP, txtContainer.Text == "" ? null : txtContainer.Text, Grupo, GrupoNao);
            else
            {
                if (confirmarEmbarque)
                    lstConsulta = dados.ConsultaContainerEmbarcadosClientes(idBTP, txtContainer.Text == "" ? null : txtContainer.Text, Grupo, GrupoNao);
                else
                {
                    if (DivergenciaLacre)
                    {
                        lstConsulta = dados.ConsultaLacreTerminalCliente(idBTP, txtContainer.Text == "" ? null : txtContainer.Text, Grupo, GrupoNao);
                    }
                    else
                    {
                        if (EmbarqueAntesPrevisto)
                            lstConsulta = dados.ConsultaContainerTerminalEmbarqueCliente(idBTP, txtContainer.Text == "" ? null : txtContainer.Text, Grupo, GrupoNao);
                        else
                            lstConsulta = dados.ConsultaContainerDeadLineDiaCliente(idBTP, txtContainer.Text == "" ? null : txtContainer.Text, Grupo, GrupoNao);
                    }
                }
            }
            try
            {
                if (lstConsulta.Count > 0)
                {
                    tsContainer.Text = "0 de " + lstConsulta.Count + " CONTAINERS";
                    //Loga na página
                    var xCarregado = navegar.EntrarPaginaBTP(DivergenciaLacre);
                    if (xCarregado == true)
                    {
                        //Consulta o Container
                        bool bPrimeiraConsulta = false;
                        int i = 1;
                        foreach (var conteudo in lstConsulta)
                        {
                            tsContainer.Text = i + " de " + lstConsulta.Count + " CONTAINERS";
                            Thread.Sleep(300);
                            Application.DoEvents();
                            if (DivergenciaLacre)
                            {
                                if (!navegar.ConsultarLacreBTP(conteudo, !bPrimeiraConsulta))
                                {
                                    //Caso de algum erro na consulta tenta logar novamente no site
                                    navegar.EntrarPaginaBTP(DivergenciaLacre);
                                }
                            }
                            else
                            {
                                if (!navegar.ConsultarContainerBTP2(conteudo, !bPrimeiraConsulta))
                                {
                                    //Caso de algum erro na consulta tenta logar novamente no site
                                    navegar.EntrarPaginaBTP(DivergenciaLacre);
                                }
                                if (confirmarEmbarque)
                                    lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);
                            }
                            bPrimeiraConsulta = true;
                            i++;
                        }
                        tsContainer.Text = "AGUARDANDO";
                        //navegar.deslogandoBTP();
                        dados.EmbarqueConfirmado(lstVA);
                    }              


                }           


            }
            catch //(Exception ex)
            {
                bCarregado = false;
            }
            return bCarregado;
        }

        private bool IniciarConsultaVilaConde(bool confirmarEmbarque, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {
         

            //Entrar no site Vila do Conde
            bool bCarregado = false;
            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>();
            List<ListaDeCampos> lstReconsulta = new List<ListaDeCampos>();
            List<ListaDeCampos> lstReconsulta2 = new List<ListaDeCampos>();
            List<ListaDeCampos> lstErros = new List<ListaDeCampos>();

            List<int> lstVA = new List<int>();

            if (confirmarEmbarque)
                lstConsulta = dados.ConsultaContainerEmbarcados(idTermVilaConde, txtContainer.Text == "" ? null : txtContainer.Text);
            else
            {
                if (DivergenciaLacre)
                {
                    lstConsulta = dados.ConsultaLacreTerminal(idTermVilaConde, txtContainer.Text == "" ? null : txtContainer.Text);
                }
                else
                {
                    if (EmbarqueAntesPrevisto)
                        lstConsulta = dados.ConsultaContainerTerminalEmbarque(idTermVilaConde, txtContainer.Text == "" ? null : txtContainer.Text);
                    else
                        lstConsulta = dados.ConsultaContainerDeadLineDia(idTermVilaConde, txtContainer.Text == "" ? null : txtContainer.Text);
                }
            }

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " CONTAINERS";
                //Loga na página
                tsTerminal.Text = "Entrando na página Vila do Conde";
                Application.DoEvents();
                bCarregado = navegar.EntrarPaginaVilaConde();
                tsTerminal.Text = "Consultando Container Vila do Conde";
                Application.DoEvents();
                if (bCarregado)
                {
                    //Consulta o Container
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count + lstReconsulta.Count) + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (DivergenciaLacre)
                        {
                            if (!navegar.ConsultarLacreSantosBrasil(conteudo))
                            {
                                lstReconsulta.Add(conteudo);
                                //Caso de algum erro na consulta tenta logar novamente no site
                                navegar.deslogandoVilaConde();
                                Thread.Sleep(300);
                                Application.DoEvents();
                                bCarregado = navegar.EntrarPaginaVilaConde();
                                if (!bCarregado)
                                    break;
                            }
                        }
                        else
                        {
                            if (!navegar.ConsultarContainerVilaConde(conteudo))
                            {
                                lstReconsulta.Add(conteudo);
                                //Caso de algum erro na consulta tenta logar novamente no site
                                navegar.deslogandoVilaConde();
                                Thread.Sleep(300);
                                Application.DoEvents();
                                bCarregado = navegar.EntrarPaginaVilaConde();
                                if (!bCarregado)
                                    break;
                            }
                            if (confirmarEmbarque)
                                lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);
                        }
                        i++;
                    }

                    foreach (var item in lstReconsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count + lstReconsulta.Count + lstReconsulta2.Count) + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarContainerVilaConde(item))
                        {
                            lstReconsulta2.Add(item);
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogandoVilaConde();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaVilaConde();
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }

                    foreach (var item in lstReconsulta2)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count + lstReconsulta.Count + lstReconsulta2.Count) + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarContainerVilaConde(item))
                        {
                            navegar.AdicionarNaoEncontrado(item, "VILA DO CONDE");
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogandoVilaConde();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaVilaConde();
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }
                    tsContainer.Text = "AGUARDANDO";
                    //navegar.deslogandoVilaConde();
                    dados.EmbarqueConfirmado(lstVA);
                }
            }
            else
            {
                tsTerminal.Text = "Nenhum dado encontrado.";
            }
            return bCarregado;
        }

        private bool IniciarConsultaItajai(bool confirmarEmbarque, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {
        
            bool bCarregado = false;
            
            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>();
            List<int> lstVA = new List<int>();

            if (confirmarEmbarque)
                lstConsulta = dados.ConsultaContainerEmbarcados(idTermItajai, txtContainer.Text == "" ? null : txtContainer.Text);
            else
            {
                if (DivergenciaLacre)
                {
                    lstConsulta = dados.ConsultaLacreTerminal(idBTP, txtContainer.Text == "" ? null : txtContainer.Text);
                }
                else
                {
                    if (EmbarqueAntesPrevisto)
                        lstConsulta = dados.ConsultaContainerTerminalEmbarque(idTermItajai, txtContainer.Text == "" ? null : txtContainer.Text);
                    else
                        lstConsulta = dados.ConsultaContainerDeadLineDia(idTermItajai, txtContainer.Text == "" ? null : txtContainer.Text);
                }
            }

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " CONTAINERS";
                //Loga na página
                bCarregado = navegar.EntrarPaginaItajai();
                if (bCarregado)
                {
                    //Consulta o Container
                    bool bPrimeiraConsulta = false;
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + lstConsulta.Count + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarContainerItajai(conteudo, !bPrimeiraConsulta))
                        {
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.EntrarPaginaItajai();
                        }
                        bPrimeiraConsulta = true;
                        if (confirmarEmbarque)
                            lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);
                        i++;
                    }
                    tsContainer.Text = "AGUARDANDO";
                    navegar.deslogandoItajai();
                    dados.EmbarqueConfirmado(lstVA);
                }
            }
            return bCarregado;
        }

        private bool IniciarConsultaItapoa(bool confirmarEmbarque, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {
            bool bCarregado = false;

            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>();
            List<int> lstVA = new List<int>();

            if (confirmarEmbarque)
                lstConsulta = dados.ConsultaContainerEmbarcados(idTermItapoa, txtContainer.Text == "" ? null : txtContainer.Text);
            else
            {
                if (EmbarqueAntesPrevisto)
                    lstConsulta = dados.ConsultaContainerTerminalEmbarque(idTermItapoa, txtContainer.Text == "" ? null : txtContainer.Text);
                else
                    lstConsulta = dados.ConsultaContainerDeadLineDia(idTermItapoa, txtContainer.Text == "" ? null : txtContainer.Text);
            }

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " CONTAINERS";
                //Loga na página
                bCarregado = navegar.EntrarPaginaItapoa();
                if (bCarregado)
                {
                    //Consulta o Container
                    bool bPrimeiraConsulta = false;
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + lstConsulta.Count + " CONTAINERS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarContainerItapoa(conteudo, !bPrimeiraConsulta))
                        {
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.EntrarPaginaItapoa();
                        }
                        bPrimeiraConsulta = true;
                        if (confirmarEmbarque)
                            lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);
                        i++;
                    }
                    tsContainer.Text = "AGUARDANDO";
                    navegar.deslogandoItapoa();
                    dados.EmbarqueConfirmado(lstVA);
                }
            }
            return bCarregado;
        }

        private bool IniciarConsultaParanagua(bool confirmarEmbarque, bool EmbarqueAntesPrevisto = false, bool DivergenciaLacre = false)
        {
            bool bCarregado = false;

            List<ListaDeCampos> lstConsulta = new List<ListaDeCampos>();
            List<int> lstVA = new List<int>();

            if (confirmarEmbarque)
                lstConsulta = dados.ConsultaContainerEmbarcados(idTermParanagua, txtContainer.Text == "" ? null : txtContainer.Text);
            else
            {
                if (EmbarqueAntesPrevisto)
                    lstConsulta = dados.ConsultaContainerTerminalEmbarque(idTermParanagua, txtContainer.Text == "" ? null : txtContainer.Text);
                else
                    lstConsulta = dados.ConsultaContainerDeadLineDia(idTermParanagua, txtContainer.Text == "" ? null : txtContainer.Text);
            }

            //int[] cdMinerva = dados.retornaCodigoMinerva();
            //lstConsulta = lstConsulta.Where(x => cdMinerva.Contains(x.CD_CLIENTE)).ToList();

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = lstConsulta.Count + " CONTAINERS";
                //Consulta o Container

                Thread.Sleep(300);
                Application.DoEvents();
                if (!navegar.ConsultarContainerParanagua(lstConsulta, ref lstVA))
                {

                }
                if (!confirmarEmbarque)
                    lstVA = new List<int>();

                tsContainer.Text = "AGUARDANDO";
                dados.EmbarqueConfirmado(lstVA);
            }
            bCarregado = true;

            return bCarregado;
        }

        private void tmrConsulta_Tick(object sender, EventArgs e)
        {
            tmrConsulta.Enabled = false;
            //navegar.GerarPlanilhaExcel();
            dados.RetornaHorarioConsulta();
            bool bConsulta = false;
            DateTime dtHorarioAux = DateTime.Now;

            //navegar.AtualizaLoginsSenhas();
            int[] QuaisConsultas = navegar.QuaisConsultas();

            #region Primeiro Horario
            tsTempo.Text = "1°Consulta";
            if (DateTime.Now > dados.dtprimeiroHorario)
            {
                dtHorarioAux = dados.dtprimeiroHorario;

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(1))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(1))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
               }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(1))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(1))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(1))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false)) COMENTADO APÓS PARAR DE FUNCIONAR
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(1))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }
            #endregion
            #region Segundo Horario
            tsTempo.Text = "2°Consulta";
            if (DateTime.Now > dados.dtsegundoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtsegundoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(2))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(2))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(2))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(2))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(2))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(2))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }
            #endregion
            #region Terceiro Horario
            tsTempo.Text = "3°Consulta";
            if (DateTime.Now > dados.dtterceiroHorario && bConsulta == false)
            {
                dtHorarioAux = dados.dtterceiroHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(3))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }

                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(3))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(3))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(3))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(3))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(3))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }
            #endregion
            #region Quarto Horário
            tsTempo.Text = "4°Consulta";
            if (DateTime.Now > dados.dtQuartoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtQuartoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(4))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }


                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(4))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(4))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(4))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(4))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(4))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }
            #endregion
            #region Quinto horário
            tsTempo.Text = "5°Consulta";
            if (DateTime.Now > dados.dtQuintoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtQuintoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(5))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }


                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(5))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(5))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(5))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(5))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(5))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }

            #endregion
            #region Sexto horário
            tsTempo.Text = "6°Consulta";
            if (DateTime.Now > dados.dtSextoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtSextoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(6))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }


                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(6))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(6))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(6))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(6))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(6))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }

            #endregion
            #region Setimo horário
            tsTempo.Text = "7°Consulta";
            if (DateTime.Now > dados.dtSetimoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtSetimoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(7))
                {

                    if (IniciarConsultaSantosBrasil(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }


                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(7))
                {
                    if (IniciarConsultaEmbraport(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(7))
                {
                    if (IniciarConsultaBTP(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(7))
                {
                    if (IniciarConsultaVilaConde(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(7))
                {
                    if (IniciarConsultaItajai(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(7))
                {
                    if (IniciarConsultaParanagua(false))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }

            #endregion
            #region Oitavo horário - Confirmação de Embarque
            tsTempo.Text = "8°Consulta";
            if (DateTime.Now > dados.dtOitavoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtOitavoHorario;
                if (/*!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) &&*/ (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(8))
                {

                    if (IniciarConsultaSantosBrasil(true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        //bConsulta = true;
                    }


                }

                if (/*!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) &&*/ (icConsultarEmbraport == "S") && QuaisConsultas.Contains(8))
                {
                    if (IniciarConsultaEmbraport(true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        //bConsulta = true;
                    }
                }

                if (/*!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) &&*/ (icConsultarBTP == "S") && QuaisConsultas.Contains(8))
                {
                    if (IniciarConsultaBTP(true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        //bConsulta = true;
                    }
                }

                if (/*!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) &&*/ (icConsultarVilaConde == "S") && QuaisConsultas.Contains(8))
                {
                    if (IniciarConsultaVilaConde(true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        //bConsulta = true;
                    }
                }
                if (/*!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) &&*/ (icConsultarItajai == "S") && QuaisConsultas.Contains(8))
                {
                    if (IniciarConsultaItajai(true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        //bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(true))
                //    {
                //        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                //if (/*!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) &&*/ (icConsultarParanagua == "S") && QuaisConsultas.Contains(8))
                //{
                //    if (IniciarConsultaParanagua(true))
                //    {
                //        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                //        //bConsulta = true;
                //    }
                //}
            }

            #endregion
            #region Nono horário - Terminal Divergência
            tsTempo.Text = "9°Consulta";
            if (DateTime.Now > dados.dtNonoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtNonoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(9))
                {

                    if (IniciarConsultaSantosBrasil(true, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(9))
                {
                    if (IniciarConsultaEmbraport(true, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(9))
                {
                    if (IniciarConsultaBTP(true, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }
            }

            #endregion

            #region Décimo horário - Atracação e Desatracação de Navios
            //if (DateTime.Now > dados.dtDecimoHorario && bConsulta == false)
            //{

            //    dtHorarioAux = dados.dtDecimoHorario;
            //    if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB))
            //    {

            //        if (IniciarConsultaNavioSantosBrasil())
            //        {
            //            dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
            //            bConsulta = true;
            //        }


            //    }

            //    if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport))
            //    {
            //        if (IniciarConsultaNavioEmbraport())
            //        {
            //            dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
            //            bConsulta = true;
            //        }
            //    }

            //    if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP))
            //    {
            //        if (IniciarConsultaNavioBTP())
            //        {
            //            dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
            //            bConsulta = true;
            //        }
            //    }
            //    /*
            //                                    if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde))
            //                                    {
            //                                        if (IniciarConsultaVilaConde(true))
            //                                        {
            //                                            dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
            //                                            bConsulta = true;
            //                                        }
            //                                    }
            //                                    if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai))
            //                                    {
            //                                        if (IniciarConsultaItajai(true))
            //                                        {
            //                                            dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
            //                                            bConsulta = true;
            //                                        }
            //                                    }
            //                                    if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
            //                                    {
            //                                        if (IniciarConsultaItapoa(true))
            //                                        {
            //                                            dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
            //                                            bConsulta = true;
            //                                        }
            //                                    }*/
            //    navegar.GerarPlanilhaExcelNavio();
            //    dados.LimparTabelatemporaria(false);
            //}
            #endregion

            #region Décimo primeiro horário - Container embarcado antes do previsto
            tsTempo.Text = "11°Consulta";
            if (DateTime.Now > dados.dtDecimoPrimeiroHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtDecimoPrimeiroHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(11))
                {

                    if (IniciarConsultaSantosBrasil(false, false, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(11))
                {
                    if (IniciarConsultaEmbraport(false, false, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(11))
                {
                    if (IniciarConsultaBTP(false, false, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(11))
                {
                    if (IniciarConsultaVilaConde(false, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        bConsulta = true;
                    }
                }
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItajai) && (icConsultarItajai == "S") && QuaisConsultas.Contains(11))
                {
                    if (IniciarConsultaItajai(false, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItajai);
                        bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermItapoa))
                //{
                //    if (IniciarConsultaItapoa(false, true))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermItapoa);
                //        bConsulta = true;
                //    }
                //}
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(11))
                {
                    if (IniciarConsultaParanagua(false, true))
                    {
                        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                        bConsulta = true;
                    }
                }
            }
            #endregion

            #region Décimo segundo horário - Divergencia Lacre
            tsTempo.Text = "12°Consulta";
            if (DateTime.Now > dados.dtDecimoSegundoHorario && bConsulta == false)
            {

                dtHorarioAux = dados.dtDecimoSegundoHorario;
                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermSB) && (icConsultarSantosBrasil == "S") && QuaisConsultas.Contains(12))
                {

                    if (IniciarConsultaSantosBrasil(false, false, false, true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermSB);
                        //bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermEmbraport) && (icConsultarEmbraport == "S") && QuaisConsultas.Contains(12))
                {
                    if (IniciarConsultaEmbraport(false, false, false, true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermEmbraport);
                        //bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idBTP) && (icConsultarBTP == "S") && QuaisConsultas.Contains(12))
                {
                    if (IniciarConsultaBTP(false, false, false, true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idBTP);
                        //bConsulta = true;
                    }
                }

                if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermVilaConde) && (icConsultarVilaConde == "S") && QuaisConsultas.Contains(12))
                {
                    if (IniciarConsultaVilaConde(false, false, true))
                    {
                        //dados.inseriHistoricoConsulta(dtHorarioAux, idTermVilaConde);
                        //bConsulta = true;
                    }
                }
                //if (!dados.retornaHistoricoConsultaDeposito(dtHorarioAux, idTermParanagua) && (icConsultarParanagua == "S") && QuaisConsultas.Contains(12))
                //{
                //    if (IniciarConsultaParanagua(false, false, true))
                //    {
                //        dados.inseriHistoricoConsulta(dtHorarioAux, idTermParanagua);
                //        bConsulta = true;
                //    }
                //} //COMENTADO POIS FAÇO DIRETO NAS CONSULTAS DE DADOS DIVERGENTES
            }
            #endregion

            if (icEnviarEmailAnalista == "S")
            {
                //Verifica se existe divergências para enviar aos analistas do trafego
                navegar.GerarPlanilhaExcelCliente(icrobo);
                //Antes de iniciar a consulta limpa a tabela temporária
                dados.LimparTabelatemporaria(true);
            }


            tmrConsulta.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string intervalString = ConfigurationManager.AppSettings["TimerInterval"];
            int interval;

            if (int.TryParse(intervalString, out interval))
            {
                tmrConsulta.Interval = interval;
                tmrConsulta.Start();
            }
            else
            {
                MessageBox.Show("Intervalo inválido no web.config.");
            }
            navegar.AtualizaLoginsSenhas();
            //MessageBox.Show(tmrConsulta.Interval.ToString());
            tmrConsulta.Enabled = true;

            tsNrRobo.Text = "Robo: " + dados.icRobo.ToString();
        }
        /*
        private void button6_Click(object sender, EventArgs e)
        {
            Point p = new Point();
            p.X = 100;
            p.Y = 100;
            LeftMouseClick(p);
            Application.DoEvents();
            p.X = 1100;
            p.Y = 270;
            LeftMouseClick(p);
            Application.DoEvents();
        }*/

        private bool IniciarConsultaNavioSantosBrasil()
        {

            //Entrar no site Santos Brasil
            bool bCarregado = false;
            List<ListaNavio> lstConsulta = new List<ListaNavio>();

            lstConsulta = dados.ConsultaNavioTerminal(idTermSB);

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " NAVIOS";
                //Loga na página
                tsTerminal.Text = "Entrando na página Santos Brasil";
                Application.DoEvents();
                bCarregado = navegar.EntrarPaginaNavio(idTermSB);
                tsTerminal.Text = "Consultando Navio Santos Brasil";
                Application.DoEvents();
                if (bCarregado)
                {
                    //Consulta o Navio
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count) + " NAVIOS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarNavioSantosBrasil(conteudo))
                        {
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogando();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaNavio(idTermSB);
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }
                }
                navegar.deslogando();
            }
            else
            {
                tsTerminal.Text = "Nenhum dado encontrado.";
            }
            return bCarregado;
        }

        private bool IniciarConsultaNavioEmbraport()
        {

            //Entrar no site Embraport
            bool bCarregado = false;
            List<ListaNavio> lstConsulta = new List<ListaNavio>();

            lstConsulta = dados.ConsultaNavioTerminal(idTermEmbraport);

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " NAVIOS";
                //Loga na página
                tsTerminal.Text = "Entrando na página Embraport";
                Application.DoEvents();
                bCarregado = navegar.EntrarPaginaNavio(idTermEmbraport);
                tsTerminal.Text = "Consultando Navio Embraport";
                Application.DoEvents();
                if (bCarregado)
                {
                    //Consulta o Navio
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count) + " NAVIOS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarNavioEmbraport(conteudo))
                        {
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogando();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaNavio(idTermEmbraport);
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }
                }
                navegar.deslogando();
            }
            else
            {
                tsTerminal.Text = "Nenhum dado encontrado.";
            }
            return bCarregado;
        }

        private bool IniciarConsultaNavioBTP()
        {

            //Entrar no site BTP
            bool bCarregado = false;
            List<ListaNavio> lstConsulta = new List<ListaNavio>();

            lstConsulta = dados.ConsultaNavioTerminal(idBTP);

            if (lstConsulta.Count > 0)
            {
                tsContainer.Text = "0 de " + lstConsulta.Count + " NAVIOS";
                //Loga na página
                tsTerminal.Text = "Entrando na página BTP";
                Application.DoEvents();
                bCarregado = navegar.EntrarPaginaNavio(idBTP);
                tsTerminal.Text = "Consultando Navio BTP";
                Application.DoEvents();
                if (bCarregado)
                {
                    //Consulta o Navio
                    int i = 1;
                    foreach (var conteudo in lstConsulta)
                    {
                        tsContainer.Text = i + " de " + (lstConsulta.Count) + " NAVIOS";
                        Thread.Sleep(300);
                        Application.DoEvents();
                        if (!navegar.ConsultarNavioBTP(conteudo))
                        {
                            //Caso de algum erro na consulta tenta logar novamente no site
                            navegar.deslogando();
                            Thread.Sleep(300);
                            Application.DoEvents();
                            bCarregado = navegar.EntrarPaginaNavio(idBTP);
                            if (!bCarregado)
                                break;
                        }
                        i++;
                    }
                }
                navegar.deslogando();
            }
            else
            {
                tsTerminal.Text = "Nenhum dado encontrado.";
            }
            return bCarregado;
        }

        private void btnSenhas_Click(object sender, EventArgs e)
        {
            navegar.AtualizaLoginsSenhas();
        }
    }
}
