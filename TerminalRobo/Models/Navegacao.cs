using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;
using roboEDI.Model;
using TerminalRobo.DataBase;
using CefSharp.WinForms.Internals;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TerminalRobo.Models
{

    public class JsDialogHandler : IJsDialogHandler
    {


        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {

            suppressMessage = true;
            //callback.Continue(true);
            callback.Continue(true);
            Navegacao.bAlert = true;

            return false;
        }


        /*
        public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            callback.Dispose();
            return false;
        }
        */
        public bool OnBeforeUnloadDialog(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        {
            return true;
        }

        public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
        {

        }
        /*
        public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {

            var control = (Control)browserControl;

            control.InvokeOnUiThreadIfRequired(delegate ()
            {
                var selectedCertificateCollection = X509Certificate2UI.SelectFromCollection(certificates, "Certificates Dialog", "Select Certificate for authentication", X509SelectionFlag.SingleSelection);

                //X509Certificate2UI.SelectFromCollection returns a collection, we've used SingleSelection, so just take the first
                //The underlying CEF implementation only accepts a single certificate
                callback.Select(selectedCertificateCollection[0]);
            });

            return true;
        }
        */
    }

    public class CookieVisitor : ICookieVisitor
    {
        public string CookieValue { get; private set; } = string.Empty;

        // Implementação do Visit
        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool delete)
        {
            if (cookie.Name == "TAS_SessionId")
            {
                CookieValue = cookie.Value;
                delete = false; // Não excluir o cookie
                return false;   // Para a visitação após encontrar o cookie
            }

            return true;
        }

        // Método chamado quando a visitação é concluída.
        public void Dispose()
        {
            // Nada a limpar
        }
    }

    /*
    public class WinFormsRequestHandler : RequestHandler
    {
        public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            
            var control = (Control)browserControl;

            control.InvokeOnUiThreadIfRequired(delegate()
            {
                var selectedCertificateCollection = X509Certificate2UI.SelectFromCollection(certificates, "Certificates Dialog", "Select Certificate for authentication", X509SelectionFlag.SingleSelection);

                //X509Certificate2UI.SelectFromCollection returns a collection, we've used SingleSelection, so just take the first
                //The underlying CEF implementation only accepts a single certificate
                callback.Select(selectedCertificateCollection[0]);
            });
            
            return false;
        }
       
    }*/

    class Navegacao
    {
        Dados dados = new Dados();
        public ChromiumWebBrowser chromeBrowser;




        public ToolStripStatusLabel tsStatus;
        public ToolStripStatusLabel tsTerminal;

        public enum site
        {
            Santosbrasil = 1,
            BTP = 2,
            Embraporte = 3
        }

        public bool bCarregado;
        const int tmpEspera = 60;
        public int tbmEsperaAjax = 90;

        const int idTermSB = 188;
        const int idTermEmbraport = 206;
        const int idBTP = 210;
        const int idTermVilaConde = 335;
        const int idTermItajai = 2072;
        const int idTermItapoa = 366;
        const int idTermTCP = 220;
        int tentativa = 0;
        public static bool bAlert = false;
        public string statusPagina = "";

        //const string siteSantosBrasil = "https://www.santosbrasil.com.br/tecon-santos-sistemas/login.asp";
        const string siteSantosBrasil = "https://www.santosbrasil.com.br/tecon-santos-sistemas/login.asp";
        const string siteBTP = "https://portaldocliente.btp.com.br/sistemas/processos-logisticos";
        const string siteEmbraport = "http://www.embraportonline.com.br/Main";

        //const string siteEmbraport = "https://www.embraportonline.com.br/Account/LogOn?service=EOL&returnurl=http://www.embraportonline.com.br/Account/LogOnIntegrado";
        //const string siteEmbraport = "https://www.embraportonline.com.br/Account/LogOn";


        


      

        const string siteVilaConde = "https://www.santosbrasil.com.br/tecon-convicon/login.asp";
        const string siteItajai = "https://itajai.apmterminals.com.br/portal#/booking";
        const string siteItapoa = "https://clientes.portoitapoa.com/#/relatorios/booking";
        string sUsuarioSB = ConfigurationManager.AppSettings["usuarioSB"].ToString();
        string sSenhaSB = ConfigurationManager.AppSettings["senhaSB"].ToString();

        string sUsuarioEmbraport = ConfigurationManager.AppSettings["usuarioEmbraport"].ToString();
        string sSenhaEmbraport = ConfigurationManager.AppSettings["senhaEmbraport"].ToString();

        string sUsuarioBTP = ConfigurationManager.AppSettings["usuarioBTP"].ToString();
        string sSenhaBTP = ConfigurationManager.AppSettings["senhaBTP"].ToString();

        string sUsuarioVilaConde = ConfigurationManager.AppSettings["usuarioVilaConde"].ToString();
        string sSenhaVilaConde = ConfigurationManager.AppSettings["senhaVilaConde"].ToString();

        string sUsuarioItajai = ConfigurationManager.AppSettings["usuarioItajai"].ToString();
        string sSenhaItajai = ConfigurationManager.AppSettings["senhaItajai"].ToString();

        string sUsuarioItapoa = ConfigurationManager.AppSettings["usuarioItapoa"].ToString();
        string sSenhaItapoa = ConfigurationManager.AppSettings["senhaItapoa"].ToString();




        InsereDados novoDado = new InsereDados();
        bool bVerifica;

        public void AtualizaLoginsSenhas()
        {
            ListaLoginsSenhas l = dados.RetornaLoginSenhas();
            sUsuarioSB = l.USUARIOSB;
            sSenhaSB = l.SENHASB;
            sUsuarioEmbraport = l.USUARIOEMBRAPORT;
            sSenhaEmbraport = l.SENHAEMBRAPORT;
            sUsuarioVilaConde = l.USUARIOVILACONDE;
            sSenhaVilaConde = l.SENHAVILACONDE;
        }

        public void exibirMensagem(string tipo, string mensagem)
        {

            if (tipo == "T")
            {
                tsTerminal.Text = mensagem;
            }
            if (tipo == "S")
            {
                tsStatus.Text = mensagem;

            }

            Application.DoEvents();

        }

        public void exibirMensagemTerminal()
        {


            Application.DoEvents();
        }

        #region Metodo para aguardar Pagina carregar

        public bool AguardaPaginaCarregar()
        {
            int tmp = 0;

            while (!bCarregado)
            {

                statusPagina = "Carregando página...";
                exibirMensagem("S", statusPagina);

                if (tmp > tmpEspera)
                {
                    bCarregado = true;

                }
                tmp++;
                Thread.Sleep(1000);
                Application.DoEvents();
            }
            exibirMensagem("S", statusPagina);
            return (tmp < tmpEspera);
        }
        #endregion

        public Navegacao(ref ToolStripStatusLabel Status, ref ToolStripStatusLabel terminal)
        {

            tsStatus = Status;
            tsTerminal = terminal;
            //CefSettings settings = new CefSettings();

            var setting = new CefSettings();
            /*

                        //language setting   


                        setting.Locale = "pt-BR";
                        // cef set userAgent
                        setting.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                        // Configure the browser path
                        // setting.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";
                        setting.CefCommandLineArgs.Add("ignore-urlfetcher-cert-requests", "1"); // Solve the certificate problem
                        setting.CefCommandLineArgs.Add("ignore-certificate-errors", "1"); // Solve the certificate problem
                        //Cef.Initialize(setting, performDependencyCheck: true, browserProcessHandler: null);
                        */

            Cef.Initialize(setting);
            chromeBrowser = new ChromiumWebBrowser("http://www.sateldespachos.com.br")
            {
                RequestHandler = new CustomRequestHandler()
            };
            //chromeBrowser = new ChromiumWebBrowser("http://www.embraportonline.com.br/");

            chromeBrowser.Dock = DockStyle.Fill;

            chromeBrowser.LoadingStateChanged += chromeBrowser_LoadingStateChanged;



            JsDialogHandler jss = new JsDialogHandler();
            chromeBrowser.JsDialogHandler = jss;
            //chromeBrowser.RequestHandler = new WinFormsRequestHandler();

        }




        public ChromiumWebBrowser CarregarBrowser()
        {
            return chromeBrowser;
        }

        private bool VerificaJanelaAjaxCarregada(string campo, string tipo)
        {

            //Verifica se a janela Ajax carregada foi carregada
            string sJanelaCarregada = "";
            if (tipo == "id")
            {
                sJanelaCarregada = "var j = document.getElementById('" + campo + "');";
                sJanelaCarregada += " if (j != null) { true} else {false}";
            }
            else if (tipo == "class")
            {
                sJanelaCarregada = "var j = document.getElementsByClassName('" + campo + "')[0];";
                sJanelaCarregada += " if (j != null) { true} else {false}";
            }
            else if (tipo == "class-visible")
            {
                sJanelaCarregada = "var j = (document.getElementsByClassName('" + campo + "')[0].style.display == 'none');";
                sJanelaCarregada += " if (j == true) { true} else {false}";
            }
            else if (tipo == "visible")
            {
                sJanelaCarregada = "var j = (document.getElementById('" + campo + "').style.display == 'block');";
                sJanelaCarregada += " if (j == true) { true} else {false}";
            }
            else if (tipo == "input")
            {
                sJanelaCarregada = "var j = (document.getElementById('" + campo + "').value != '');";
                sJanelaCarregada += " if (j == true) { true} else {false}";

            }
            else if (tipo == "class-opacity")
            {
                sJanelaCarregada = "var j = (document.getElementsByClassName('" + campo + "')[0].style.opacity == 0);";
                sJanelaCarregada += " if (j == true) { true} else {false}";
            }

            bool bOk = false;
            int cont = 0;
            do
            {
                var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bOk = response.Result.ToString().ToUpper() == "TRUE";

                }
                else
                {

                }
                Application.DoEvents();
                Thread.Sleep(500);
                cont++;
            } while (!bOk && cont < tbmEsperaAjax);

            return (cont <= tbmEsperaAjax);
        }


        void chromeBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                statusPagina = "Página carregada com sucesso";
                bCarregado = true;
            }
        }
        #region Metodos Santos Brasil
        //Entrar na página
        //=================================================================
        public bool EntrarPaginaSantosBrasil()
        {
            bCarregado = false;
            try
            {
                string url = "";
                int icontador = 0;
                do
                {
                    bCarregado = false;

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteSantosBrasil + "';");
                    chromeBrowser.Load("document.location ='" + siteSantosBrasil + "';");
                    bCarregado = AguardaPaginaCarregar();

                    //Verifica se conseguiu acessar a página.
                    url = chromeBrowser.Address;
                    icontador++;

                } while (url != siteSantosBrasil && !bCarregado && icontador < 3);



                bCarregado = false;
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('login_').value = '" + sUsuarioSB + "';");

                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('senha_').value = '" + sSenhaSB + "';");
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("log_act();");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('g-recaptcha')[0].click();");
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('foption').options[1].selected = 'selected';");
                //Thread.Sleep(1000);
                //Application.DoEvents();
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnForm').click();");

                //chromeBrowser.ExecuteScriptAsync("document.getElementById('login_').value = '" + sUsuarioSB + "';");
                //chromeBrowser.ExecuteScriptAsync("document.getElementById('senha_').value = '" + sSenhaSB + "';");
                //chromeBrowser.ExecuteScriptAsync("log_act();");
                bCarregado = AguardaPaginaCarregar();
            }
            catch
            {
                bCarregado = false;
            }
            return bCarregado;
        }
        //================================================================
        //Consultar container
        //================================================================
        public bool ConsultarContainerSantosBrasil(ListaDeCampos conteudo)
        {

            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            bCarregado = false;
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('container cntr')[0].value = '" + conteudo.NR_CONTAINER + "';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('Export')[0].click();");
            //AguardaPaginaCarregar(ref tsStatus);

            Application.DoEvents();

            //Pegar se o container foi depositado no terminal de embarque


            //Verificar se a janela com informações do container já foi carregada

            string sJanelaCarregada = "var j = document.getElementsByClassName('infConteiner janela')[0];";
            sJanelaCarregada += " if (j != null) { true} else {false}";
            bool bOk = false;
            tentativa = 0;
            bAlert = false;
            do
            {
                var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bOk = response.Result.ToString() == "True";

                }
                Application.DoEvents();
                Thread.Sleep(750);
                tentativa++;
            } while ((!bOk && tentativa < 40) && (!bAlert));
            if (tentativa >= 40)
            {
                //Não encontrou o container
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "ESGOTADO TENTATIVAS (20). CONTAINER NÃO ENCONTRADO", novoDado);
                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
                /*
                novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                novoDado.DT_CONSULTA = DateTime.Now;
                novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                novoDado.DS_STATUS = "NÃO ENCONTROU";
                novoDado.NM_TERMINAL = "SANTOS BRASIL";
                novoDado.NM_NAVIO = conteudo.NM_NAVIO;
                novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                 
                dados.InsereConsulta(novoDado);
                */
                return false;
            }
            if (bAlert)
            {
                //Não encontrou o container
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "CONTAINER NÃO ENCONTRADO", novoDado);
                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
                /*
                novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                novoDado.DT_CONSULTA = DateTime.Now;
                novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                novoDado.DS_STATUS = "NÃO ENCONTROU";
                novoDado.NM_TERMINAL = "SANTOS BRASIL";
                novoDado.NM_NAVIO = conteudo.NM_NAVIO;
                novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                 
                dados.InsereConsulta(novoDado);
                */
                return true;
            }

            string sDataDeposito = "function retornaValor(){var janela = document.getElementsByClassName('infConteiner janela');";
            sDataDeposito += " var conteudo = [];";
            sDataDeposito += " var info = janela[0].getElementsByClassName('info');";
            sDataDeposito += " var li = info[0].getElementsByTagName('li');";
            sDataDeposito += " conteudo[0] = ''; conteudo[1] = '';";
            sDataDeposito += " conteudo[2] = ''; conteudo[3] = '';";
            sDataDeposito += " conteudo[4] = ''; conteudo[5] = '';";
            sDataDeposito += " conteudo[6] = ''; conteudo[7] = '';";
            sDataDeposito += " ";

            sDataDeposito += " for(var i=0;i< li.length;i++){";
            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Entr. terminal'){";
            sDataDeposito += " conteudo[0] = li[i].getElementsByTagName('span')[0].innerText;}";                                // DATA DE DEPÓSITO NO TERMINAL
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Embarque'){";
            sDataDeposito += " conteudo[4] = li[i] != null ? li[i].getElementsByTagName('span')[0].innerText : '';}}";          // DATA DE EMBARQUE

            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Booking'){";
            sDataDeposito += " conteudo[2] = li[i].getElementsByTagName('span')[0].innerText;}}";

            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText.includes('DUE')){";
            sDataDeposito += " conteudo[1] = li[i].getElementsByTagName('span')[0].innerText;}}";

            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Navio'){";
            sDataDeposito += " conteudo[3] = li[i].getElementsByTagName('span')[0].innerText;}}}";

            sDataDeposito += " if (info[0].getElementsByClassName('aviso').length == 0){";
            sDataDeposito += " conteudo[5] = document.getElementsByClassName('status')[0].innerText;";
            sDataDeposito += " if (!conteudo[5].includes('Sa�do do Terminal') && !conteudo[5].includes('Saído do Terminal')) conteudo[4] = '';}";
            sDataDeposito += " else{";
            sDataDeposito += " conteudo[5] = info[0].getElementsByClassName('aviso')[0].innerText;}";

            //PEGAR LACRES//
            sDataDeposito += "var lacres = document.getElementsByClassName('janela lacre_fld')[1].getElementsByClassName('lacreCNTR');";
            sDataDeposito += "if (lacres != null && lacres.length != 0) {";
            sDataDeposito += "lacres = lacres[1].getElementsByClassName('inner')[0].getElementsByTagName('TR');";
            sDataDeposito += "for (let index = 1; index < lacres.length; index++) {";
            sDataDeposito += "if (lacres[index].getElementsByTagName('td')[1].innerText == 'Lacre Armador'){";
            sDataDeposito += "conteudo[6] = lacres[index].getElementsByTagName('td')[0].innerText;}";
            sDataDeposito += "if (lacres[index].getElementsByTagName('td')[1].innerText == 'Lacre Veterinário(SIF)'){";
            sDataDeposito += "conteudo[7] = lacres[index].getElementsByTagName('td')[0].innerText;}}";
            if (!string.IsNullOrEmpty(conteudo.DS_LACRE_SIF))
            {
                sDataDeposito += "if(conteudo[7] == ''){";
                sDataDeposito += "for (let index = 1; index < lacres.length; index++) {";
                sDataDeposito += "if (conteudo[7] == '' && lacres[index].getElementsByTagName('td')[1].innerText == 'Lacre Exportador'){";
                sDataDeposito += "conteudo[7] = lacres[index].getElementsByTagName('td')[0].innerText;}";
                sDataDeposito += "if (lacres[index].getElementsByTagName('td')[0].innerText.includes('" + conteudo.DS_LACRE_SIF.Replace("/", "").Replace("SIF", "").Replace("SENACSA", "") + "')){";
                sDataDeposito += "conteudo[7] = lacres[index].getElementsByTagName('td')[0].innerText;}}}}";
            }
            else
            {
                sDataDeposito += "}";
            }

  
            /* LÓGICA FRABICIANA
            //sDataDeposito += " if (li.length < 15){";
            //sDataDeposito += " conteudo[0] = ''; } ";
            //sDataDeposito += " else{ ";
            //sDataDeposito += " if (li.length == 15){";
            //sDataDeposito += " conteudo[0] = li[14].getElementsByTagName('span')[0].innerText;}";
            //sDataDeposito += " else{";
            //sDataDeposito += " conteudo[0] = li[15].getElementsByTagName('span')[0].innerText;}}"; //Data do Terminal
            
            sDataDeposito += " if(li.length >= 6){";
            sDataDeposito += " conteudo[1] = li[6].getElementsByTagName('span')[0].innerText;"; //Número da DUE e situação
            sDataDeposito += " if (document.getElementsByClassName('status')[0].innerText == 'Aguardando transfer�ncia'){";
            sDataDeposito += " conteudo[2] = li[1].getElementsByTagName('span')[0].innerText; ";
            sDataDeposito += " }";
            sDataDeposito += " else{";
            sDataDeposito += " conteudo[2] = li[4].getElementsByTagName('span')[0].innerText;}"; //Número do Booking
            sDataDeposito += " if (li[1].getElementsByTagName('span').length > 0){";
            sDataDeposito += " conteudo[3] = li[1].getElementsByTagName('span')[0].innerText;}"; //Nome do Navio
            sDataDeposito += " else{";
            sDataDeposito += " conteudo[3] = li[1].getElementsByTagName('p')[0].innerText;}}";
            sDataDeposito += " conteudo[5] = document.getElementsByClassName('status')[0].innerText;";
            */

            //sDataDeposito += " if (li.length == 16){";
            //sDataDeposito += " conteudo[4] = li[15] != null ? li[15].getElementsByTagName('span')[0].innerText : '';} else{ conteudo[4] = li[15] != null ? li[16].getElementsByTagName('span')[0].innerText : '';}"; //Data do Embarque
            sDataDeposito += " return conteudo} retornaValor(); ";
            bool retorno = false;
            tentativa = 0;
            do
            {

                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                task1.Wait();
                var response1 = task1.Result;

                retorno = ValidarSituacaoContainer(idTermSB, conteudo, task1);

                bCarregado = false;

                Thread.Sleep(300);
                Application.DoEvents();
                tentativa++;
            } while (!retorno && tentativa < 20);
            //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
            if (tentativa >= 20)
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "ESGOTADO TENTATIVAS (2) - 2. DESLOGANDO E PARANDO", novoDado);
                deslogandoSantosBrasil();
                bCarregado = false;
                return false;
            }



            bCarregado = false;
            //Sair do site.

            /*
            chromeBrowser.EvaluateScriptAsync(sDataDeposito). ContinueWith(x =>
            {
                var response = x.Result;
                if (response.Success && response.Result != null)
                    MessageBox.Show(response.Result.ToString());
            });
             */
            /*
            chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName('Export')[1].value;").ContinueWith(x =>
            {
                var response = x.Result;
                if (response.Success && response.Result != null)
                    MessageBox.Show(response.ToString());
            });
            */
            return retorno;
        }
        /*
        public class RenderProcessMessageHandler : IRenderProcessMessageHandler
        {
            // Wait for the underlying JavaScript Context to be created. This is only called for the main frame.
            // If the page has no JavaScript, no context will be created.
            void IRenderProcessMessageHandler.OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
            {
                const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";

                frame.ExecuteJavaScriptAsync(script);
            }
        }
        */



        //======================================================================
        public void deslogandoSantosBrasil()
        {

            exibirMensagem("T", "Deslogando Santos Brasil");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.santosbrasil.com.br/tecon-santos-sistemas/restrito/desloga.asp';");
            exibirMensagem("S", "Saindo da página...");
            bCarregado = false;
            AguardaPaginaCarregar();
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");

        }
        #endregion


        #region Metodos Embraport
        //Entrar na página
        //=================================================================
        public bool EntrarPaginaEmbraport()
        {

            int icontador = 0;

            string url = "";
            try
            {
                do
                {
                    /*
                    chromeBrowser.RenderProcessMessageHandler = new RenderProcessMessageHandler();

                    //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
                    chromeBrowser.LoadingStateChanged += (sender, args) =>
                    {
                        //Wait for the Page to finish loading
                        if (args.IsLoading == false)
                        {
                            chromeBrowser.ExecuteScriptAsync("alert('All Resources Have Loaded');");
                        }
                    };

                    //Wait for the MainFrame to finish loading
                    chromeBrowser.FrameLoadEnd += (sender, args) =>
                    {
                        //Wait for the MainFrame to finish loading
                        if (args.Frame.IsMain)
                        {
                            args.Frame.ExecuteJavaScriptAsync("alert('MainFrame finished loading');");
                        }
                    };
                     */

                    //chromeBrowser.ExecuteScriptAsync("document.location ='" + siteEmbraport + "';");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteEmbraport + "';");
                    //chromeBrowser.Load("document.location ='" + siteEmbraport + "';");
                    //chromeBrowser.Load(siteEmbraport);
                    bCarregado = false;
                    bCarregado = AguardaPaginaCarregar();
                    //Verifica se conseguiu acessar a página.
                    url = chromeBrowser.Address;
                    icontador++;
                } while ((url != siteEmbraport || !bCarregado) && icontador < 3);


                //add isidro 29/01
                //if (bCarregado)
                if ((bCarregado) && (url != siteEmbraport))
                    {
                        //Verifica se conseguiu carregar a página

                        bCarregado = false;


                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('UserName').value = '" + sUsuarioEmbraport + "';");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('Password').value = '" + sSenhaEmbraport + "';");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btn-login').click();");


                    //chromeBrowser.ExecuteScriptAsync("document.getElementById('UserName').value = '" + sUsuarioEmbraport + "';");
                    //chromeBrowser.ExecuteScriptAsync("document.getElementById('Password').value = '" + sSenhaEmbraport + "';");
                    //chromeBrowser.ExecuteScriptAsync("document.getElementById('btn-login').click();");
                    bCarregado = AguardaPaginaCarregar();
                    int tmp = 0;
                    while (tmp < 25)
                    {

                        statusPagina = "Carregando página...";
                        exibirMensagem("S", statusPagina);


                        tmp++;
                        Thread.Sleep(1000);
                        Application.DoEvents();
                    }

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#divMaskPopupMensagemTela-421274').css('opacity', '0');");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#divModalFormPopupMensagemTela-421274').css('display', 'none');");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#EOLCarouselAviso').css('display', 'none');");

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("LoadItemMenu('/Conteiner/ConsultaConteiner');");
                    Thread.Sleep(300);
                    Application.DoEvents();

                }
                //add isidro 29/01
                else
                {

                    Thread.Sleep(1000);
                    Application.DoEvents();


                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#divMaskPopupMensagemTela-421274').css('opacity', '0');");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#divModalFormPopupMensagemTela-421274').css('display', 'none');");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#EOLCarouselAviso').css('display', 'none');");

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("LoadItemMenu('/Conteiner/ConsultaConteiner');");
                    Thread.Sleep(300);
                    Application.DoEvents();


                }
            }
            catch
            {
                //Se der erro força logar novamente.
                EntrarPaginaEmbraport();
                return false;
            }

            //bCarregado = true;
            return bCarregado;
        }
        //================================================================
        //Consultar container
        //================================================================
        public bool ConsultarContainerEmbraport(ListaDeCampos conteudo)
        {
            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            bCarregado = false;
            //Chama a tela de pesquisa do container
            bVerifica = VerificaJanelaAjaxCarregada("edNroConteiner", "id");

            if (bVerifica)
            {

                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btLimpar').click();");
                Thread.Sleep(300);
                Application.DoEvents();

                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnPopupMensagemTela-421274').click();");
                Thread.Sleep(300);
                Application.DoEvents();

                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('edNroConteiner').value = '" + conteudo.NR_CONTAINER + "';");
                Application.DoEvents();
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btPesquisar').click();");

                bVerifica = VerificaJanelaAjaxCarregada("loading", "class-visible");

                Application.DoEvents();
                Thread.Sleep(500);


                //Verifica se mostra mensagem de container não existe
                string sJanelaCarregada = "var j = document.getElementsByClassName('box-error').length > 0 ? document.getElementsByClassName('box-error')[0].style.display == 'none' : true;";
                sJanelaCarregada += " if (j == true) { true} else {false}";
                bool bNaoEncontrou = false;
                var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bNaoEncontrou = response.Result.ToString() == "True";

                }
                if (!bNaoEncontrou)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "NÃO ENCONTROU O CONTAINER", novoDado);
                    sJanelaCarregada = "if(document.getElementsByClassName('box-error')[0] != null) document.getElementsByClassName('box-error')[0].click();";
                    task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                    task.Wait();
                    //Thread.Sleep(300);
                    //bVerifica = VerificaJanelaAjaxCarregada("box-error", "class-visible");
                    return true;
                }





                if (bVerifica)
                {


                    //Verifica se o container já está depositado
                    Thread.Sleep(300);
                    Application.DoEvents();
                    Thread.Sleep(300);
                    Application.DoEvents();
                    string sNaoEncontrou = "var erro = document.getElementsByClassName('box-error')[0].style.display == 'none';";
                    sNaoEncontrou += "!erro;";
                    var taskn = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sNaoEncontrou);
                    taskn.Wait();
                    var responsen = taskn.Result;
                    bool resultn = responsen.Success;
                    bNaoEncontrou = true;

                    if (resultn)
                    {
                        bNaoEncontrou = responsen.Result.ToString() == "True";

                    }
                    else
                        bNaoEncontrou = false;



                    //Pegar os dados da tela
                    if (!bNaoEncontrou)
                    {
                        bool result1 = false;
                        string sDataDeposito = "function retornaValor(){var dv = document.getElementById('dataTable');";
                        sDataDeposito += " var conteudo = [];";
                        sDataDeposito += " var tb = dv.getElementsByTagName('table');";
                        sDataDeposito += " var tb1 = tb[1].getElementsByTagName('td');";
                        sDataDeposito += " var tb2 = tb[2].getElementsByTagName('td');";
                        sDataDeposito += " var tb3 = tb[6].getElementsByTagName('td');";
                        //sDataDeposito += " var tb4 = tb[12].innerText.includes('Lacre M') ? tb[12].getElementsByTagName('td') : tb[13].innerText.includes('Lacre M') ? tb[13].getElementsByTagName('td') : tb[14].getElementsByTagName('td');";
                        sDataDeposito += " var tb4 = tb[12].innerText.includes('Lacre M') ? tb[12].getElementsByTagName('td') : tb[13] != null ? tb[13].innerText.includes('Lacre M')  ? tb[13].getElementsByTagName('td') : tb[14] != null ? tb[14].getElementsByTagName('td') : '' : '';";
                        sDataDeposito += " var tb5;";
                        sDataDeposito += " var n = 7;";
                        sDataDeposito += " do";
                        sDataDeposito += " {";
                        sDataDeposito += " n++;";

                        sDataDeposito += " if (tb[n] != undefined)";
                        sDataDeposito += " {";
                        sDataDeposito += " if (tb[n].innerText.includes('Transp'))";
                        sDataDeposito += " {";
                        sDataDeposito += " tb5 = tb[n].getElementsByTagName('td');";
                        sDataDeposito += " }";
                        sDataDeposito += " }";
                        sDataDeposito += " } while (!tb[n].innerText.includes('Transp'));";

                        //sDataDeposito += " var tb5 = tb[9].innerText.includes('Transp') ? tb[9].getElementsByTagName('td') : tb[10].getElementsByTagName('td');";
                        sDataDeposito += " conteudo[0] = tb2[20].innerText;"; //Data do Embarque
                        //sDataDeposito += " conteudo[1] = tb2[25].innerText;"; // Número DUE / Situacao
                        sDataDeposito += " conteudo[1] = tb2[27].innerText;"; // Número DUE / Situacao
                        sDataDeposito += " conteudo[2] = tb3[2].innerText;"; // Booking
                        sDataDeposito += " conteudo[3] = tb1[1].innerText;"; // Navio
                        sDataDeposito += " conteudo[4] = tb2[21] != null ? tb2[21].innerText : '';"; // Data da Saída do navio
                        sDataDeposito += " conteudo[5] = tb2[5].innerText;"; // Categoria
                        sDataDeposito += " if (tb4[3] != null ) { conteudo[6] = tb4[3].innerText; } else { conteudo[6] = ''}";
                        sDataDeposito += " if (tb4[7] != null ) { conteudo[7] = tb4[7].innerText; } else { conteudo[7] = ''}";
                        sDataDeposito += " if (conteudo[2] == 'Nro:') { conteudo[2] = tb3[3].innerText}";
                        sDataDeposito += " if (conteudo[4] != '') { var dt =  conteudo[4]; dt = dt.replace('Data Saída:', ''); dt = dt.trimLeft(); conteudo[4] = dt}";//Retirar caracteres e espaços do texto
                        sDataDeposito += " if (conteudo[3] == 'Sem navio definido') { conteudo[4] = '';}"; // SE NÃO TIVER NAVIO, ENTÃO CONTAINER NÃO EMBARCOU
                        sDataDeposito += " var dt1 = dt.split(' ')[0].split('/').reverse().join('') + dt.split(' ')[1];";
                        sDataDeposito += " var dt2 = tb5[3].innerText.split(' ')[0].split('/').reverse().join('') + tb5[3].innerText.split(' ')[1];";
                        sDataDeposito += " if (dt1 < dt2) { conteudo[0] = '';}";
                        sDataDeposito += " return conteudo} retornaValor(); ";

                        tentativa = 0;
                        do
                        {

                            var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                            task1.Wait();
                            var response1 = task1.Result;

                            result1 = ValidarSituacaoContainer(idTermEmbraport, conteudo, task1);

                            bCarregado = false;

                            Thread.Sleep(300);
                            Application.DoEvents();
                            tentativa++;
                        } while (!result1 && tentativa < 20);
                        //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
                        if (tentativa >= 20)
                        {
                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "HOUVE PROBLEMA PARA CONSULTAR (20). TENTANDO NOVAMENTE", novoDado);
                            deslogandoEmbraport();
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        /*
                        novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                        novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                        novoDado.DT_CONSULTA = DateTime.Now;
                        novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                        novoDado.CD_NUMERO_PROCESSO = conteudo.CD_NUMERO_PROCESSO;
                        novoDado.DS_STATUS = "NÃO ENCONTROU";
                        novoDado.NM_TERMINAL = "DP WORLD";
                        dados.InsereConsulta(novoDado);
                        */
                        return true;
                    }

                }
                else
                {
                    //Caso não tenha carregado os dados sai e tenta novamente.
                    deslogandoEmbraport();
                    return false;
                }
            }
            else
            {
                //Caso não tenha conseguindo encontrar o campo de consulta do container desloga do sistema e tenta novamente
                deslogandoEmbraport();
                return false;
            }


            /*
            string sJanelaCarregada = "var j = document.getElementsByClassName('infConteiner janela')[0];";
            sJanelaCarregada += " if (j != null) { true} else {false}";
            bool bOk = false;
            do
            {
                var task = chromeBrowser.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bOk = response.Result.ToString() == "True";

                }
                Application.DoEvents();
                Thread.Sleep(500);

            } while (!bOk);

            string sDataDeposito = "function retornaValor(){var janela = document.getElementsByClassName('infConteiner janela');";
            sDataDeposito += " var conteudo = [];";
            sDataDeposito += " var info = janela[0].getElementsByClassName('info');";
            sDataDeposito += " var li = info[0].getElementsByTagName('li');";
            sDataDeposito += " conteudo[0] = li[14].getElementsByTagName('span')[0].innerText;";
            sDataDeposito += " conteudo[1] = li[6].getElementsByTagName('span')[0].innerText;";
            sDataDeposito += " conteudo[2] = li[4].getElementsByTagName('span')[0].innerText;";
            sDataDeposito += " conteudo[3] = li[1].getElementsByTagName('span')[0].innerText;";
            sDataDeposito += " return conteudo} retornaValor(); ";

            var task1 = chromeBrowser.EvaluateScriptAsync(sDataDeposito);
            task1.Wait();
            var response1 = task1.Result;
            bool result1 = response1.Success;
            if (result1)
            {
                MessageBox.Show(response1.Result.ToString());

            }
            bCarregado = false;
            //Sair do site.
            */
            /*
            chromeBrowser.EvaluateScriptAsync(sDataDeposito). ContinueWith(x =>
            {
                var response = x.Result;
                if (response.Success && response.Result != null)
                    MessageBox.Show(response.Result.ToString());
            });
             */
            /*
            chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName('Export')[1].value;").ContinueWith(x =>
            {
                var response = x.Result;
                if (response.Success && response.Result != null)
                    MessageBox.Show(response.ToString());
            });
            */

        }
        //======================================================================
        public void deslogandoEmbraport()
        {
            //exibirMensagem("T", "Deslogando Embraport");
            //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='http://www.embraportonline.com.br/Account/LogOff';");

            exibirMensagem("S", "Saindo da página...");

            bCarregado = false;
            AguardaPaginaCarregar();
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");
        }
        #endregion
        /*
        public bool EntrarSite(int val) {
            if (val == (int)site.Santosbrasil) {
                SantosBrasil santosbrasil = new SantosBrasil();
                santosbrasil.EntrarPagina(ref tsStatus, ref chromeBrowser, false);
            }

            return true;
        }
        public bool ConsultarContainer(string nrContainer, int val) {

            if (val == (int)site.Santosbrasil)
            {
                SantosBrasil santosbrasil = new SantosBrasil();
                santosbrasil.ConsultarContainer(nrContainer, ref tsStatus, ref chromeBrowser, false);
            }

            return true;
        } 
        */
        #region Metodos BTP


     
        //Entrar na página
        //=================================================================

     


        public bool EntrarPaginaBTP()
        {         

            int icontador = 0;
            bCarregado = false;
            string url = "";

            string cookieValor = ConfigurationManager.AppSettings["CookieLoginBTP"].ToString();

            var cookieManager = Cef.GetGlobalCookieManager();
            CefSharp.Cookie idpCookie = new CefSharp.Cookie // <== Especificando CefSharp.Cookie
            {
                Name = "idp",
                Value = cookieValor,
                Domain = "idp.btp.com.br",
                Path = "/",
                HttpOnly = false,
                Secure = false,
                Expires = DateTime.Now.AddDays(30)
            };

            // Adiciona o cookie ao gerenciador global
            cookieManager.SetCookieAsync("https://idp.btp.com.br", idpCookie);

            do
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteBTP + "';");
          
                bCarregado = AguardaPaginaCarregar();
 
                url = chromeBrowser.Address;
                icontador++;

            } while (url != siteBTP && !bCarregado && icontador < 3);


            //mEsperaAjax = 30;
            Thread.Sleep(7000);
            Application.DoEvents();



            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('login').value = '" + sUsuarioBTP + "';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('senha').value = '" + sSenhaBTP + "';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('entrar').click();");

         
            Thread.Sleep(6000);
            Application.DoEvents();


            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.querySelectorAll('a[mat-raised-button]')[2].click();");

             
            Thread.Sleep(5000);
            Application.DoEvents();

            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.querySelectorAll('mat-dialog-container button.mat-button .mat-button-wrapper')[0].click();");






       
            Thread.Sleep(5000);
            Application.DoEvents();

            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(
                "document.querySelector('#wrapper > div > btp-toolbar > mat-toolbar > div > div.full-height > button').click();"
            );

         
            Thread.Sleep(3000);
            Application.DoEvents();

            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(@"
    const dueElement = Array.from(document.querySelectorAll('span.nav-link-title[translate=""no""]'))
        .find(span => span.textContent.trim() === 'Due');
    if (dueElement) {
        dueElement.click();
    }
");

           
            Thread.Sleep(2000);
            Application.DoEvents();







            string x = "";
            var cookieVisitor = new CookieVisitor();

            // Obtendo o cookie
            cookieManager.VisitAllCookies(cookieVisitor);

            Thread.Sleep(2000);

            // Criando um novo cookie com o mesmo valor
            if (!string.IsNullOrEmpty(cookieVisitor.CookieValue))
            {

                CefSharp.Cookie newCookie = new CefSharp.Cookie
                {
                    Name = "TAS_SessionId",
                    Value = cookieVisitor.CookieValue,  // Usando o valor do cookie capturado
                    Domain = ".btp.com.br",
                    Path = "/",
                    HttpOnly = true,
                    Secure = true
                };

                // Definindo o novo cookie
                cookieManager.SetCookie("https://novo-tas.btp.com.br", newCookie);
            }

            AguardaPaginaCarregar();
            Thread.Sleep(5000);
            Application.DoEvents();


            // Continuar a navegação
            string ConsultaContainer = "https://novo-tas.btp.com.br/b2b/consultadue";

            int icontadorX = 0;
            do
            {
                // Carregar uma página em branco antes de acessar o ConsultaContainer
                chromeBrowser.GetBrowser().MainFrame.LoadUrl("about:blank");

                // Espera para garantir que a página em branco tenha carregado


                AguardaPaginaCarregar();
                Thread.Sleep(5000);
                Application.DoEvents();

                // Agora que a página em branco foi carregada, acessar a página ConsultaContainer
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync($"document.location = '{ConsultaContainer}';");

                // Aguarda a página ConsultaContainer carregar
                bCarregado = AguardaPaginaCarregar();

                url = chromeBrowser.Address;
                icontadorX++;

            } while (url != ConsultaContainer && !bCarregado && icontadorX < 3);




            AguardaPaginaCarregar();
            Thread.Sleep(5000);
            Application.DoEvents();

 

            return bCarregado;
        }
        //================================================================
        //Consultar container
        //================================================================



        public bool ConsultarContainerBTP2(ListaDeCampos conteudo, bool bPrimeiraVez)
        {


            //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnLimpar').click();");



            var frame = chromeBrowser.GetBrowser().MainFrame;

            // Script para verificar se o elemento existe antes de clicar
            var scriptExiste = @"
        (function() {
            var btn = document.getElementById('btnLimpar');
            if (btn) {
                btn.click();
                return true;
            }
            return false;
        })();
    ";

            var taskX = frame.EvaluateScriptAsync(scriptExiste);
            taskX.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var responseEx = t.Result;
                    if (responseEx.Success && responseEx.Result != null && (bool)responseEx.Result)
                    {
                        
                    }
                    else
                    {
                        EntrarPaginaBTP();
                    }
                }
            });


            Thread.Sleep(2000);
            Application.DoEvents();


            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('txtConteiner').value = '" + conteudo.NR_CONTAINER + "';");


            Thread.Sleep(3000);
            Application.DoEvents();


            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnAddConteiner').click();");

            Thread.Sleep(3000);
            Application.DoEvents();


            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnPesquisar').click();");




             Application.DoEvents();
             Thread.Sleep(10000);


            var browser = chromeBrowser.GetBrowser();
            var mainFrame = browser.MainFrame;

            var responseX = mainFrame.EvaluateScriptAsync("document.getElementById('msgFiltro') !== null && document.getElementById('msgFiltro').innerText.includes('20 segundos')").Result;

            if (responseX.Success && responseX.Result != null && (bool)responseX.Result)
            {
                Console.WriteLine("Mensagem de espera detectada. Tentando novamente em 20 segundos...");
                Thread.Sleep(21000); // Aguarda 20 segundos
                mainFrame.EvaluateScriptAsync("document.getElementById('btnPesquisar').click();");

                Application.DoEvents();
                Thread.Sleep(5000);
            }

            Application.DoEvents();



            string scriptF = @"
    function executarCodigo() {
        let rows = document.querySelectorAll('#jTableLista .jtable tbody tr');
        let found = false;

        rows.forEach(row => {
            if (found) return; // Interrompe a execução após o primeiro encontrado

            let bookingCell = row.querySelector('td:nth-child(3)');
            if (bookingCell && bookingCell.textContent.trim() === '" + conteudo.NR_BOOKING.Trim() + @"') {
                let checkbox = row.querySelector('input[type=checkbox]');
                if (checkbox) {
                    checkbox.click();
                    found = true; // Marca como encontrado
                }
            }
        });

        console.log(found); // Vai imprimir true ou false no console
        return found; // Retorna se encontrou e clicou no checkbox
    }

    // Chama a função para execução
    executarCodigo();
";


            // Executa o script no navegador
            var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(scriptF);
            task.Wait();
            var response = task.Result;
            bool result = response.Success;
            bool bEncontrou = false;

            if (result)
            {
                bEncontrou = response.Result.ToString() == "True";
            }
            else
            {
                bEncontrou = false;
            }

            // Se o booking NÃO foi encontrado, verifica se há a mensagem "Não existem registros a listar!"
            bool bNaoEncontrou = false;
            if (!bEncontrou)
             {
                string sNaoEncontrou = @"
                    var janela = document.getElementsByClassName('jtable');
                    if (janela.length > 0) {
                        var tabela = janela[0];
                        var col = tabela.getElementsByTagName('td');
                        if (col.length > 0) {
                            var linha = col[0].innerText;
                            linha == 'Não existem registros a listar!';
                        } else {
                            false;
                        }
                    } else {
                        false;
                    }";

                var taskn = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sNaoEncontrou);
                taskn.Wait();
                var responsen = taskn.Result;
                bool resultn = responsen.Success;

                if (resultn)
                {
                    bEncontrou = false;
                }
            }

            if (bEncontrou)
            {
                
                //precisa verificar se é de entrada ou saída quando encontrar.
                bool bOk = false;
                int cont = 0;


                Thread.Sleep(5000);
                Application.DoEvents();
                //return true;





                var sDataDeposito = @"
                function retornaValor() {
                    let resultado = [];

                    let tabelaRecepcao = document.querySelector('#jtable-recepcao .jtable');
                    if (tabelaRecepcao) {
                        let rowRecepcao = tabelaRecepcao.querySelector('.jtable-data-row');
                        if (rowRecepcao) {
                            let cells = rowRecepcao.querySelectorAll('td');
                            resultado[0] = cells[3]?.textContent.trim() || ''; // Data Recepção (índice 3)
                            resultado[5] = cells[4]?.textContent.trim() || ''; // Status Recepção (índice 4)
                        }
                    }

                    let tabelaDocumentacao = document.querySelector('#jTableListaDocumentacao .jtable');
                    if (tabelaDocumentacao) {
                        let rowDocumentacao = tabelaDocumentacao.querySelector('.jtable-data-row');
                        if (rowDocumentacao) {
                            let cells = rowDocumentacao.querySelectorAll('td');
                            resultado[1] = cells[4]?.textContent.trim() || ''; // DUE (índice 4)
                            resultado[6] = cells[7]?.textContent.trim() || ''; // Status de Desembaraço (índice 5)
                            resultado[7] = cells[10]?.textContent.trim() || ''; // Local Embarque (índice 10)
                            resultado[8] = cells[11]?.textContent.trim() || ''; // Local CCT (índice 11)
                        }
                    }

                    let rowSelecionada = document.querySelector('.jtable-row-selected');
                    if (rowSelecionada) {
                        let cells = rowSelecionada.querySelectorAll('td');
                        resultado[2] = cells[2]?.textContent.trim() || '';  // Booking (índice 2)
                        resultado[3] = cells[10]?.textContent.trim() || '';  // Navio (índice 8)
                    }

                    let tabelaEntrega = document.querySelector('#jtable-entrega .jtable');
                    if (tabelaEntrega) {
                        let rowEntrega = tabelaEntrega.querySelector('.jtable-data-row');
                        if (rowEntrega) {
                            let cells = rowEntrega.querySelectorAll('td');
                            resultado[4] = cells[2]?.textContent.trim() || ''; // Data Entrega (índice 3)
                        }
                    }

                    // O return agora está dentro da função
                    return resultado;
                }

                // Chama a função que contém o return
                retornaValor();
                ";



                //var sDataDeposito = @" function retornaValor() {
                //    let resultado = [];
                //    let tabelaRecepcao = document.querySelector('#jtable-recepcao .jtable');
                //    let tabelaDocumentacao = document.querySelector('#jTableListaDocumentacao .jtable');
                //    let tabelaEntrega = document.querySelector('#jtable-entrega .jtable');
                //    let rowSelecionada = document.querySelector('.jtable-row-selected');

                //    if (rowSelecionada)
                //    {
                //        let cells = rowSelecionada.querySelectorAll('td');
                //        let sbooking = cells[2]?.textContent.trim() || ''; // Booking
                //        let nmStatus = cells[8]?.textContent.trim() || ''; // Status

                //        resultado[0] = '';

                //        if (tabelaRecepcao)
                //        {
                //            let rowRecepcao = tabelaRecepcao.querySelector('.jtable-data-row');
                //            if (rowRecepcao)
                //            {
                //                let cellsRecepcao = rowRecepcao.querySelectorAll('td');
                //                resultado[0] = document.getElementById('txtRecepcaoDataEntrada')?.value || ''; // Data Entrada no terminal
                //                resultado[5] = document.getElementById('txtRecepcaoStatusResposta')?.value || ''; // Status resposta
                //            }
                //        }

                //        if (tabelaDocumentacao)
                //        {
                //            let rowDocumentacao = tabelaDocumentacao.querySelector('.jtable-data-row');
                //            if (rowDocumentacao)
                //            {
                //                let cellsDoc = rowDocumentacao.querySelectorAll('td');
                //                resultado[1] = cellsDoc[2]?.textContent.trim() || ''; // DUE
                //                resultado[6] = cellsDoc[4]?.textContent.trim() || ''; // Status Desembaraço
                //                resultado[7] = cellsDoc[7]?.textContent.trim() || ''; // Local Embarque
                //                resultado[8] = cellsDoc[8]?.textContent.trim() || ''; // Local CCT
                //            }
                //        }

                //        resultado[2] = sbooking;
                //        resultado[3] = cells[10]?.textContent.trim() || ''; // Navio

                //        if (nmStatus === 'SAÍDO')
                //        {
                //            resultado[4] = document.getElementById('txtEntregaDataEmbarque')?.value || '';
                //        }
                //        else
                //        {
                //            resultado[4] = '';
                //        }

                //        if ((nmStatus === 'NO TERMINAL' || nmStatus === 'AVISADO') || nmStatus === 'SAÍDO')
                //        {
                //            return resultado;
                //        }
                //    }

                //    return resultado;
                //}

                //retornaValor();";



                tentativa = 0;
                try
                {
                    do
                    {

                        var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                        task1.Wait();
                        var response1 = task1.Result;

                        result = ValidarSituacaoContainer(idBTP, conteudo, task1);

                        bCarregado = false;

                        Thread.Sleep(600);
                        Application.DoEvents();
                        tentativa++;
                    } while (!result && tentativa < 20);
                    //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
                    if (tentativa >= 20)
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "TENTATIVAS ESGOTADAS (20). TENTANDO NOVAMENTE.", novoDado);
                      //deslogandoBTP();
                        return false;
                    }

                    bCarregado = false;
                }
                catch (Exception e)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "ERRO INESPERADO. " + e.Message, novoDado);
                    return false;
                }

            }
            else
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
            }        
            return true;
        }


        //public bool ConsultarContainerBTP(ListaDeCampos conteudo, bool bPrimeiraVez)
        //{
        //    //dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "INICIANDO CONSULTA", novoDado);
        //    //if (bPrimeiraVez)
        //    //{
        //    bCarregado = false;
        //    //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://tas.btp.com.br/b2b/consultadue';");
        //    // chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('SubMenu-2')[0].getElementsByTagName('a')[4].click();");

        //    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('SubMenu-2')[0].getElementsByTagName('a')[3].click();");

        //    //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://tas.btp.com.br/b2b/consultaconteiner?fromActionLink=True';");

        //    AguardaPaginaCarregar();


        //    //}

        //    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('txtConteiner').value = '" + conteudo.NR_CONTAINER + "';");
        //    bool bEncontrou20 = true;
        //    do
        //    {
        //        Application.DoEvents();
        //        Thread.Sleep(20000);
        //        Application.DoEvents();

        //        chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnPesquisar').click()");


        //        VerificaJanelaAjaxCarregada("jtable-busy-message", "class-visible");

        //        //Verifica se tem mensagem de 20 segundos
        //        //Se tiver espera o tempo necessário para consultar novamente
        //        string sTempoVinteSegundos = "var div = document.getElementById('msgFiltro');";
        //        sTempoVinteSegundos += " var sMsg = document.getElementById('msgFiltro').innerHTML;";
        //        sTempoVinteSegundos += " var isHidden = window.getComputedStyle(div).display === 'block';";
        //        sTempoVinteSegundos += " (isHidden && sMsg == 'Você poderá executar novamente essa ação em 20 segundos.');";
        //        var task20 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sTempoVinteSegundos);
        //        task20.Wait();
        //        var response20 = task20.Result;
        //        bool result20 = response20.Success;

        //        if (result20)
        //        {
        //            bEncontrou20 = response20.Result.ToString() == "True";

        //        }
        //        else
        //            bEncontrou20 = false;

        //        if (bEncontrou20)
        //        {
        //            //espera 20 segundos para consultar novamente
        //            Application.DoEvents();
        //            Thread.Sleep(1000);
        //            Application.DoEvents();


        //        }
        //    }
        //    while (bEncontrou20);

        //    //Verifica se o container já está depositado
        //    string sNaoEncontrou = "var janela = document.getElementsByClassName('jtable');";
        //    sNaoEncontrou += "var tabela = janela[0];";
        //    sNaoEncontrou += "var col = tabela.getElementsByTagName('td');";
        //    sNaoEncontrou += "var linha = col[0].innerText;";
        //    sNaoEncontrou += "linha == 'Não existem registros a listar!';";
        //    var taskn = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sNaoEncontrou);
        //    taskn.Wait();
        //    var responsen = taskn.Result;
        //    bool resultn = responsen.Success;
        //    bool bNaoEncontrou = true;
        //    if (resultn)
        //    {
        //        bNaoEncontrou = responsen.Result.ToString() == "True";

        //    }
        //    else
        //        bNaoEncontrou = false;

        //    if (!bNaoEncontrou)
        //    {
        //        //Verifica se já carregou os dados na tela
        //        //=======================================================

        //        string sJanelaCarregada = "function Carregou(listadue) { var h = 0; do { sleep(500); x++; } while(listadue[2] == null && x < 8) }";
        //        sJanelaCarregada += "function sleep(milliseconds) {const date = Date.now();let currentDate = null;do{currentDate = Date.now();} while (currentDate - date < milliseconds);}";
        //        sJanelaCarregada += "var bOk = false;";
        //        sJanelaCarregada += "var janela = document.getElementsByClassName('jtable');";
        //        sJanelaCarregada += " var x = janela[0].getElementsByTagName('tr');";
        //        sJanelaCarregada += " var y = janela[2].getElementsByTagName('td');";
        //        sJanelaCarregada += " for(var i=x.length - 1;i> 0;i--){";
        //        sJanelaCarregada += "   var k = x[i].getElementsByTagName('td');";
        //        sJanelaCarregada += "   var sbooking = k[2].innerText;";
        //        sJanelaCarregada += "   var nmStatus = k[8].innerText; ";
        //        if (conteudo.DT_DEPOSITO != null)
        //        {
        //            sJanelaCarregada += "   if (sbooking.trim().includes('" + conteudo.NR_BOOKING.Trim() + "') || (nmStatus == 'NO TERMINAL')){";
        //            sJanelaCarregada += "   if ((nmStatus == 'NO TERMINAL') || (nmStatus =='AVISADO') || (nmStatus == 'SAÍDO')) {";
        //            sJanelaCarregada += "      var s = k[8].innerText; ";
        //            sJanelaCarregada += "      var chek = k[0].getElementsByTagName('input'); ";
        //            sJanelaCarregada += "      chek[0].click(); Carregou(y);";
        //            sJanelaCarregada += "      if (y[2] != null) {bOk = true;} else {if (y[0].innerText == 'Não existem registros a listar!'){bOk = true;}} "; //DUE
        //            sJanelaCarregada += "      break;";
        //            sJanelaCarregada += "   }}}";
        //        }
        //        else
        //        {
        //            sJanelaCarregada += "   if ((nmStatus == 'NO TERMINAL') || (nmStatus =='AVISADO')) {";
        //            sJanelaCarregada += "      var s = k[8].innerText; ";
        //            sJanelaCarregada += "      if (s != 'SAÍDO'){ ";
        //            sJanelaCarregada += "      var chek = k[0].getElementsByTagName('input'); ";
        //            sJanelaCarregada += "      chek[0].click(); Carregou(y);";
        //            sJanelaCarregada += "      if (y[2] != null) {bOk = true;} else {if (y[0].innerText == 'Não existem registros a listar!'){bOk = true;}} "; //DUE
        //            sJanelaCarregada += "      break;";
        //            sJanelaCarregada += "      } else { bOk = false}";
        //            sJanelaCarregada += "   }}";
        //        }
        //        sJanelaCarregada += "   bOk";
        //        var taskx = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
        //        taskx.Wait();
        //        if (taskx.Result.Result.ToString().ToUpper() == "FALSE")
        //        {
        //            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
        //            return true;
        //        }
        //        //precisa verificar se é de entrada ou saída quando encontrar.
        //        bool bOk = false;
        //        int cont = 0;
        //        do
        //        {
        //            var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("var bretorno = []; bretorno[0] = false; bretorno[1] = false; if (y[2] != null) {bretorno[0] = true;} else {if (y[0].innerText == 'Não existem registros a listar!' || !bok){bretorno[0] = true;}} bretorno;"); task.Wait();
        //            var response = task.Result;
        //            bool result = response.Success;
        //            if (result)
        //            {
        //                if (((List<object>)response.Result)[1].ToString() == "True")
        //                    return true;
        //                bOk = ((List<object>)response.Result)[0].ToString() == "True";

        //            }
        //            else
        //            {

        //            }
        //            Application.DoEvents();
        //            Thread.Sleep(700);
        //            cont++;
        //        } while (!bOk && cont < tbmEsperaAjax);
        //        //===========================================================
        //        //AguardaPaginaCarregar(ref tsStatus);
        //        //Verifica se puxou os dados.
        //        VerificaJanelaAjaxCarregada("txtRecepcaoDataEntrada", "input");

        //        Application.DoEvents();
        //        //return true;



        //        string sDataDeposito = "function retornaValor(){var janela = document.getElementsByClassName('jtable');";
        //        sDataDeposito += " var x = janela[0].getElementsByTagName('tr');";
        //        sDataDeposito += " var y = janela[2].getElementsByTagName('td');";
        //        sDataDeposito += " var conteudo = [];";
        //        sDataDeposito += " for(var i=x.length-1;i>0;i--){";
        //        sDataDeposito += "   var k = x[i].getElementsByTagName('td');";
        //        sDataDeposito += "   var sbooking = k[2].innerText; ";
        //        sDataDeposito += "   var nmStatus = k[8].innerText; ";

        //        sDataDeposito += "   conteudo[0] = '';";
        //        if (conteudo.DT_DEPOSITO != null)
        //        {
        //            sDataDeposito += "   if (sbooking.trim().includes('" + conteudo.NR_BOOKING.Trim() + "') || (nmStatus == 'NO TERMINAL')){";
        //            sDataDeposito += "   if ((nmStatus == 'NO TERMINAL') || (nmStatus =='AVISADO') || (nmStatus == 'SAÍDO')) {";
        //            sDataDeposito += "      conteudo[0] = document.getElementById('txtRecepcaoDataEntrada').value; "; //Data Entrada no terminal
        //            sDataDeposito += "      if (y[2] != null){"; //DUE
        //            sDataDeposito += "          conteudo[1] =  y[2].innerText;"; //DUE
        //            sDataDeposito += "          conteudo[6] =  y[4].innerText; "; //Status de Desembaraço
        //            sDataDeposito += "          conteudo[7] =  y[7].innerText; "; //Local de Embarque
        //            sDataDeposito += "          conteudo[8] =  y[8].innerText; }"; //Local CCT
        //            sDataDeposito += "      else"; //DUE
        //            sDataDeposito += "      conteudo[1] =  '';"; //DUE
        //            sDataDeposito += "      conteudo[2] = sbooking; "; //Nr. Booking
        //            sDataDeposito += "      conteudo[3] = k[10].innerText; "; //Navio

        //            sDataDeposito += "      conteudo[4] = nmStatus == 'SAÍDO' ? document.getElementById('txtEntregaDataEmbarque').value : ''; "; //Status da resposta
        //            sDataDeposito += "      conteudo[5] = document.getElementById('txtRecepcaoStatusResposta').value; "; //Status da resposta


        //            sDataDeposito += "      break;";
        //            sDataDeposito += "      }";
        //            sDataDeposito += "   }";
        //        }
        //        else
        //        {
        //            sDataDeposito += "   if ((nmStatus == 'NO TERMINAL') || (nmStatus =='AVISADO')) {";
        //            sDataDeposito += "      conteudo[0] = document.getElementById('txtRecepcaoDataEntrada').value; "; //Data Entrada no terminal
        //            sDataDeposito += "      if (y[2] != null){"; //DUE
        //            sDataDeposito += "          conteudo[1] =  y[2].innerText;"; //DUE
        //            sDataDeposito += "          conteudo[6] =  y[4].innerText; "; //Status de Desembaraço
        //            sDataDeposito += "          conteudo[7] =  y[7].innerText; "; //Local de Embarque
        //            sDataDeposito += "          conteudo[8] =  y[8].innerText; }"; //Local CCT
        //            sDataDeposito += "      else"; //DUE
        //            sDataDeposito += "      conteudo[1] =  '';"; //DUE
        //            sDataDeposito += "      conteudo[2] = sbooking; "; //Nr. Booking
        //            sDataDeposito += "      conteudo[3] = k[10].innerText; "; //Navio

        //            sDataDeposito += "      conteudo[4] = document.getElementById('txtEntregaDataEmbarque').value; "; //Status da resposta
        //            sDataDeposito += "      conteudo[5] = document.getElementById('txtRecepcaoStatusResposta').value; "; //Status da resposta


        //            sDataDeposito += "      break;";
        //            sDataDeposito += "      }";
        //        }
        //        sDataDeposito += " }";
        //        sDataDeposito += " return conteudo} retornaValor(); ";
        //        tentativa = 0;
        //        try
        //        {
        //            do
        //            {

        //                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
        //                task1.Wait();
        //                var response1 = task1.Result;

        //                resultn = ValidarSituacaoContainer(idBTP, conteudo, task1);

        //                bCarregado = false;

        //                Thread.Sleep(600);
        //                Application.DoEvents();
        //                tentativa++;
        //            } while (!resultn && tentativa < 20);
        //            //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
        //            if (tentativa >= 20)
        //            {
        //                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "TENTATIVAS ESGOTADAS (20). TENTANDO NOVAMENTE.", novoDado);
        //                deslogandoBTP();
        //                return false;
        //            }

        //            bCarregado = false;
        //        }
        //        catch (Exception e)
        //        {
        //            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "ERRO INESPERADO. " + e.Message, novoDado);
        //            return false;
        //        }

        //    }
        //    else
        //    {
        //        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
        //    }

        //    return true;
        //}
        //======================================================================



        public void deslogandoBTP()        
        {
            exibirMensagem("T", "Deslogando BTP");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://tas.btp.com.br/Home/Logout';");
            exibirMensagem("S", "Saindo da página...");

            bCarregado = false;
            AguardaPaginaCarregar();
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");

        }
        #endregion




        #region Metodos Vila do Conde
        public bool EntrarPaginaVilaConde()
        {
            bCarregado = false;
            try
            {
                string url = "";
                int icontador = 0;
                do
                {
                    bCarregado = false;

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteVilaConde + "';");
                    chromeBrowser.Load("document.location ='" + siteVilaConde + "';");
                    bCarregado = AguardaPaginaCarregar();

                    //Verifica se conseguiu acessar a página.
                    url = chromeBrowser.Address;
                    icontador++;

                } while (url != siteVilaConde && !bCarregado && icontador < 3);



                bCarregado = false;
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('login_').value = '" + sUsuarioVilaConde + "';");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('senha_').value = '" + sSenhaVilaConde + "';");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('g-recaptcha')[0].click();");

                bCarregado = AguardaPaginaCarregar();
            }
            catch
            {
                bCarregado = false;
            }
            return bCarregado;
        }

        public bool ConsultarContainerVilaConde(ListaDeCampos conteudo)
        {

            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "VILA DO CONDE", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            bCarregado = false;
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('container cntr')[0].value = '" + conteudo.NR_CONTAINER + "';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('Export')[0].click();");
            //AguardaPaginaCarregar(ref tsStatus);

            Application.DoEvents();

            //Pegar se o container foi depositado no terminal de embarque


            //Verificar se a janela com informações do container já foi carregada

            string sJanelaCarregada = "var j = document.getElementsByClassName('infConteiner janela')[0];";
            sJanelaCarregada += " if (j != null) { true} else {false}";
            bool bOk = false;
            tentativa = 0;
            bAlert = false;
            do
            {
                var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bOk = response.Result.ToString() == "True";

                }
                Application.DoEvents();
                Thread.Sleep(750);
                tentativa++;
            } while ((!bOk && tentativa < 40) && (!bAlert));
            if (tentativa >= 40)
            {
                //Não encontrou o container
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "VILA DO CONDE", DateTime.Now, "ESGOTADO TENTATIVAS (20). CONTAINER NÃO ENCONTRADO", novoDado);
                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
                return false;
            }
            if (bAlert)
            {
                //Não encontrou o container
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "VILA DO CONDE", DateTime.Now, "CONTAINER NÃO ENCONTRADO", novoDado);
                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
                return true;
            }

            string sDataDeposito = "function retornaValor(){var janela = document.getElementsByClassName('infConteiner janela');";
            sDataDeposito += " var conteudo = [];";
            sDataDeposito += " var info = janela[0].getElementsByClassName('info');";
            sDataDeposito += " var li = info[0].getElementsByTagName('li');";
            sDataDeposito += " conteudo[0] = ''; conteudo[1] = '';";
            sDataDeposito += " conteudo[2] = ''; conteudo[3] = '';";
            sDataDeposito += " conteudo[4] = ''; conteudo[5] = '';";
            sDataDeposito += " conteudo[6] = ''; conteudo[7] = '';";
            sDataDeposito += " ";
            sDataDeposito += " for(var i=0;i< li.length;i++){";
            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Entr. terminal'){";
            sDataDeposito += " conteudo[0] = li[i].getElementsByTagName('span')[0].innerText;}";                                // DATA DE DEPÓSITO NO TERMINAL
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Embarque'){";
            sDataDeposito += " conteudo[4] = li[i] != null ? li[i].getElementsByTagName('span')[0].innerText : '';}}";          // DATA DE EMBARQUE
            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Booking'){";
            sDataDeposito += " conteudo[2] = li[i].getElementsByTagName('span')[0].innerText;}}";
            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText.includes('DUE')){";
            sDataDeposito += " conteudo[1] = li[i].getElementsByTagName('span')[0].innerText;}}";
            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Navio'){";
            sDataDeposito += " conteudo[3] = li[i].getElementsByTagName('span')[0].innerText;}}}";
            sDataDeposito += " if (info[0].getElementsByClassName('aviso').length == 0){";
            sDataDeposito += " conteudo[5] = document.getElementsByClassName('status')[0].innerText;";
            sDataDeposito += " if (!conteudo[5].includes('Sa�do do Terminal') && !conteudo[5].includes('Saído do Terminal')) conteudo[4] = '';}";
            sDataDeposito += " else{";
            sDataDeposito += " conteudo[5] = info[0].getElementsByClassName('aviso')[0].innerText;}";

            //PEGAR LACRES//
            sDataDeposito += "var lacres = document.getElementsByClassName('janela lacre_fld')[1].getElementsByClassName('lacreCNTR');";
            sDataDeposito += "if (lacres != null && lacres.length != 0) {";
            sDataDeposito += "lacres = lacres[1].getElementsByClassName('inner')[0].getElementsByTagName('TR');";
            sDataDeposito += "for (let index = 1; index < lacres.length; index++) {";
            sDataDeposito += "if (lacres[index].getElementsByTagName('td')[1].innerText == 'Lacre Armador'){";
            sDataDeposito += "conteudo[6] = lacres[index].getElementsByTagName('td')[0].innerText;}";
            sDataDeposito += "if (lacres[index].getElementsByTagName('td')[1].innerText == 'Lacre Veterinário(SIF)'){";
            sDataDeposito += "conteudo[7] = lacres[index].getElementsByTagName('td')[0].innerText;}}";
            if (!string.IsNullOrEmpty(conteudo.DS_LACRE_SIF))
            {
                sDataDeposito += "if(conteudo[7] == ''){";
                sDataDeposito += "for (let index = 1; index < lacres.length; index++) {";
                sDataDeposito += "if (conteudo[7] == '' && lacres[index].getElementsByTagName('td')[1].innerText == 'Lacre Exportador'){";
                sDataDeposito += "conteudo[7] = lacres[index].getElementsByTagName('td')[0].innerText;}";
                sDataDeposito += "if (lacres[index].getElementsByTagName('td')[0].innerText.includes('" + conteudo.DS_LACRE_SIF.Replace("/", "").Replace("SIF", "").Replace("SENACSA", "") + "')){";
                sDataDeposito += "conteudo[7] = lacres[index].getElementsByTagName('td')[0].innerText;}}}}";
            }
            else
            {
                sDataDeposito += "}";
            }

            sDataDeposito += " return conteudo} retornaValor(); ";
            bool retorno = false;
            tentativa = 0;
            do
            {

                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                task1.Wait();
                var response1 = task1.Result;

                retorno = ValidarSituacaoContainer(idTermVilaConde, conteudo, task1);

                bCarregado = false;

                Thread.Sleep(300);
                Application.DoEvents();
                tentativa++;
            } while (!retorno && tentativa < 20);
            //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
            if (tentativa >= 20)
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "VILA DO CONDE", DateTime.Now, "ESGOTADO TENTATIVAS (2) - 2. DESLOGANDO E PARANDO", novoDado);
                deslogandoVilaConde();
                bCarregado = false;
                return false;
            }
            bCarregado = false;
            return retorno;
        }

        public void deslogandoVilaConde()
        {

            exibirMensagem("T", "Deslogando Vila do Conde");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.santosbrasil.com.br/desloga.asp';");
            exibirMensagem("S", "Saindo da página...");
            bCarregado = false;
            AguardaPaginaCarregar();
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");

        }
        #endregion

        #region Metodo Itajai

        public bool EntrarPaginaItajai()
        {
            bCarregado = false;
            try
            {
                string url = "";
                int icontador = 0;
                do
                {
                    bCarregado = false;

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteItajai + "';");
                    chromeBrowser.Load("document.location ='" + siteItajai + "';");
                    bCarregado = AguardaPaginaCarregar();

                    //Verifica se conseguiu acessar a página.
                    url = chromeBrowser.Address;
                    icontador++;

                } while (url != siteItajai && !bCarregado && icontador < 3);



                bCarregado = false;
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('fieldCpf').value = '" + sUsuarioItajai + "';");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('fieldPass').value = '" + sSenhaItajai + "';");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btn btn-info btn-lg btn-block text-uppercase btn-rounded')[0].click();");
                bCarregado = AguardaPaginaCarregar();

                bCarregado = false;
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#perfil').val(7);"); // 30 para paulo e 7 para andreia
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btn btn-info btn-lg btn-block text-uppercase btn-rounded')[0].click();");
                bCarregado = AguardaPaginaCarregar();
            }
            catch
            {
                bCarregado = false;
            }
            return bCarregado;
        }
        public bool ConsultarContainerItajai(ListaDeCampos conteudo, bool bPrimeiraVez)
        {
            //deslogandoItajai();
            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            //if (bPrimeiraVez)
            //{
            bCarregado = false;
            Thread.Sleep(1000);
            //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('.menuFilho')[7].click();");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteItajai + "';");
            AguardaPaginaCarregar();
            Application.DoEvents();

            Thread.Sleep(1000);
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('codBooking').value = '" + conteudo.NR_BOOKING + "';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('actConsultar').click()");
            AguardaPaginaCarregar();
            string msgEncontrado = "if(document.getElementsByClassName('jq-toast-single jq-has-icon jq-icon-error')[0] != null) {";
            msgEncontrado += "var j = (document.getElementsByClassName('jq-toast-single jq-has-icon jq-icon-error')[0].style.display == 'none');";
            msgEncontrado += " if (j == true) { true} else {false}} else {true}";


            var task0 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(msgEncontrado);
            task0.Wait();
            var response0 = task0.Result;
            bool resultn = false;

            Application.DoEvents();

            if (response0.Success)
                resultn = response0.Result.ToString() == "True";

            Thread.Sleep(500);
            if (resultn)
            {
                //Verifica se já carregou os dados na tela
                //=======================================================
                string msgClick = "var tabela = document.getElementById('tbListBooking').getElementsByTagName('tr');";
                msgClick += "for(var i = 1; i < tabela.length; i++) {";
                msgClick += "var navio = tabela[i].getElementsByTagName('td')[1];";
                msgClick += "if (navio.innerText.includes('" + conteudo.NM_NAVIO + "'))";
                msgClick += "navio.click() }";
                var taskNavio = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(msgClick);
                taskNavio.Wait();

                if (!taskNavio.Result.Success)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
                    return true;
                }
                Thread.Sleep(500);

                string sJanelaCarregada = "var conteudo = [];";
                sJanelaCarregada += "var p = document.getElementById('divBookingDetalhe');";
                sJanelaCarregada += "if (p.getElementsByClassName('row')[0] != null && p.getElementsByClassName('row')[1] == null) { }";
                sJanelaCarregada += "else if (p.getElementsByClassName('row')[0] != null) {";
                sJanelaCarregada += "var o = p.getElementsByClassName('row')[1].getElementsByTagName('tr');";
                sJanelaCarregada += "for (var i = 1; i < o.length; i++) {";
                sJanelaCarregada += "var q = o[i].getElementsByTagName('td');";
                sJanelaCarregada += "if(q[0].innerText.includes('" + conteudo.NR_CONTAINER + "')) {";
                sJanelaCarregada += "conteudo[0] = q[1].innerText;";
                sJanelaCarregada += "conteudo[4] = q[2].innerText;";
                sJanelaCarregada += "conteudo[1] = q[3].innerText;";
                sJanelaCarregada += "conteudo[3] = navio.innerText;";
                sJanelaCarregada += "conteudo[2] = '" + conteudo.NR_BOOKING + "'; } } }";
                //sJanelaCarregada += "if (p.getElementsByClassName('row')[0] != null && p.getElementsByClassName('row')[1] == null) conteudo[0] = '';";
                sJanelaCarregada += "conteudo;";

                try
                {
                    tentativa = 0;
                    do
                    {

                        var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                        task1.Wait();
                        var response1 = task1.Result;

                        resultn = ValidarSituacaoContainer(idTermItajai, conteudo, task1);

                        bCarregado = false;

                        Thread.Sleep(600);
                        Application.DoEvents();
                        tentativa++;
                    } while (!resultn && tentativa < 20);
                    //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
                    if (tentativa >= 20)
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "TENTATIVAS ESGOTADAS (20). TENTANDO NOVAMENTE.", novoDado);
                        deslogandoItajai();
                        return false;
                    }

                    bCarregado = false;
                }
                catch (Exception e)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "ERRO INESPERADO. " + e.Message, novoDado);
                    return false;
                }

            }
            else
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
            }
            //Sair do site.

            /*
            chromeBrowser.EvaluateScriptAsync(sDataDeposito). ContinueWith(x =>
            {
                var response = x.Result;
                if (response.Success && response.Result != null)
                    MessageBox.Show(response.Result.ToString());
            });
             */
            /*
            chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName('Export')[1].value;").ContinueWith(x =>
            {
                var response = x.Result;
                if (response.Success && response.Result != null)
                    MessageBox.Show(response.ToString());
            });
            */
            return true;
        }
        //======================================================================
        public void deslogandoItajai()
        {
            exibirMensagem("T", "Deslogando Itajai");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://itajai.apmterminals.com.br/portal/login/logout';");
            exibirMensagem("S", "Saindo da página...");

            bCarregado = false;
            AguardaPaginaCarregar();
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");

        }
        #endregion

        #region Metodo Itapoa
        public bool EntrarPaginaItapoa()
        {
            bCarregado = false;
            try
            {
                string url = "";
                int icontador = 0;
                do
                {
                    bCarregado = false;

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteItapoa + "';");
                    chromeBrowser.Load("document.location ='" + siteItapoa + "';");
                    bCarregado = AguardaPaginaCarregar();

                    //Verifica se conseguiu acessar a página.
                    url = chromeBrowser.Address;
                    icontador++;

                } while (url != siteItapoa && !bCarregado && icontador < 3);

                string urlAtual = "";
                int i = 0;
                do
                {
                    bCarregado = false;
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('input')[0].value = '" + sUsuarioItapoa + "';");
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('input')[1].value = '" + sSenhaItapoa + "';");
                    System.Threading.Thread.Sleep(100);

                    System.Drawing.Point p = new System.Drawing.Point();
                    p.X = 5000;//100;
                    p.Y = 160;//100;
                    Form1.LeftMouseClick(p);
                    Application.DoEvents();
                    p.X = 5000;//1100;
                    p.Y = 260;//270;
                    Form1.LeftMouseClick(p);
                    Application.DoEvents();
                    p.X = 5000;//1100;
                    p.Y = 320;//330;
                    Form1.LeftMouseClick(p);
                    Thread.Sleep(200);
                    Application.DoEvents();
                    p.X = 5000;//100;
                    p.Y = 160;//100;
                    Form1.LeftMouseClick(p);
                    Thread.Sleep(200);
                    Application.DoEvents();
                    /* ambiente de teste
                    p.X = 100;
                    p.Y = 100;
                    Form1.LeftMouseClick(p);
                    Application.DoEvents();
                    p.X = 1100;
                    p.Y = 270;
                    Form1.LeftMouseClick(p);
                    Application.DoEvents();
                    p.X = 1100;
                    p.Y = 330;
                    Form1.LeftMouseClick(p);
                    Thread.Sleep(200);
                    Application.DoEvents();
                    p.X = 100;
                    p.Y = 100;
                    Form1.LeftMouseClick(p);
                    Thread.Sleep(200);
                    Application.DoEvents();*/

                    Thread.Sleep(500);
                    Application.DoEvents();
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('kt_submit').click();");
                    bCarregado = AguardaPaginaCarregar();
                    urlAtual = chromeBrowser.Address;
                    i++;

                } while (urlAtual != "https://clientes.portoitapoa.com/#/dashboard" && i <= 4);

                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteItapoa + "';");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('kt-menu__link-text')[5].click()");
                Thread.Sleep(500);
                Application.DoEvents();
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("var g = document.getElementsByClassName('porto-menu')[0]; g.getElementsByTagName('a')[0].click(); ");
                bCarregado = false;
                AguardaPaginaCarregar();
                Thread.Sleep(1000);
                Application.DoEvents();
            }
            catch
            {
                bCarregado = false;
            }
            return bCarregado;
        }

        public bool ConsultarContainerItapoa(ListaDeCampos conteudo, bool bPrimeiraVez)
        {
            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            //if (bPrimeiraVez)
            //{
            bCarregado = false;
            Thread.Sleep(1000);
            //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('.menuFilho')[7].click();");

            bool Encontrado = false;
            int tentativas = 0;

            do
            {
                System.Drawing.Point p = new System.Drawing.Point();
                p.X = 4420;
                p.Y = 370;
                Form1.LeftMouseClick(p);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('input')[0].value = '" + conteudo.NR_BOOKING + "';");
                Application.DoEvents();
                Thread.Sleep(1000);
                Application.DoEvents();
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('button')[5].click()");
                Application.DoEvents();
                Thread.Sleep(1000);
                Application.DoEvents();
                var taskErro = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('swal2-container swal2-center swal2-backdrop-show')[0] == null");
                taskErro.Wait();
                if (taskErro.Result.Result.ToString() != "True")
                {
                    var taskEncontrado = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('swal2-content').innerText == 'Booking não encontrado!'");
                    taskEncontrado.Wait();
                    if (taskEncontrado.Result.Result.ToString() == "True")
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
                        return true;
                    }
                    else
                    {
                        Encontrado = false;
                    }
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('swal2-confirm swal2-styled')[0].click();");
                }
                else
                {
                    Encontrado = true;
                }
                tentativas++;
            } while (!Encontrado && tentativas < 3);
            //Application.DoEvents();
            //bCarregado = false;
            //AguardaPaginaCarregar();
            //bool Encontrado = VerificaJanelaAjaxCarregada("swal2-container swal2-center swal2-backdrop-show", "class");

            Thread.Sleep(1000);
            if (Encontrado)
            {
                //Verifica se já carregou os dados na tela
                //=======================================================
                /*int j = 1;
                do
                {
                    var taskCntr = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('td')[" + j.ToString() + "] == null");
                    taskCntr.Wait();

                    if (taskCntr.Result.Result.ToString() == "True")
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
                        return true;
                    }
                    Thread.Sleep(500);

                    var taskCntr2 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('td')[" + j.ToString() + "].innerText == '" + conteudo.NR_CONTAINER + "';");
                    taskCntr2.Wait();

                    if (taskCntr2.Result.Result.ToString() == "True")
                        break;

                    j += 19;
                } while (true);
                Thread.Sleep(500);*/

                bool carregaDados = VerificaJanelaAjaxCarregada("v-overlay__scrim", "class-opacity");

                string sJanelaCarregada = "var conteudo = [];";
                sJanelaCarregada += "var g = document.getElementsByTagName('tr');";
                sJanelaCarregada += "for(var i = 1; i < g.length; i++) { ";
                sJanelaCarregada += "var f = g[i].getElementsByTagName('td')[1];";
                sJanelaCarregada += "if (f.innerText == '" + conteudo.NR_CONTAINER + "') {";
                sJanelaCarregada += "conteudo[0] = g[i].getElementsByTagName('td')[12].innerText;";
                sJanelaCarregada += "conteudo[1] = g[i].getElementsByTagName('td')[11].innerText;";
                sJanelaCarregada += "conteudo[2] = '" + conteudo.NR_BOOKING + "';";
                sJanelaCarregada += "conteudo[3] = '" + conteudo.NM_NAVIO + "';";
                sJanelaCarregada += "conteudo[4] = g[i].getElementsByTagName('td')[13].innerText; } }";
                sJanelaCarregada += "conteudo;";


                try
                {
                    bool resultn = false;
                    tentativa = 0;
                    do
                    {

                        var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                        task1.Wait();
                        var response1 = task1.Result;

                        resultn = ValidarSituacaoContainer(idTermItapoa, conteudo, task1);

                        bCarregado = false;

                        Thread.Sleep(600);
                        Application.DoEvents();
                        tentativa++;
                    } while (!resultn && tentativa < 20);
                    //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
                    if (tentativa >= 20)
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "TENTATIVAS ESGOTADAS (20). TENTANDO NOVAMENTE.", novoDado);
                        deslogandoItajai();
                        return false;
                    }

                    bCarregado = false;
                }
                catch (Exception e)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "ERRO INESPERADO. " + e.Message, novoDado);
                    return false;
                }

            }
            else
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
            }
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByTagName('button')[5].click();");
            return true;
        }
        //======================================================================
        public void deslogandoItapoa()
        {
            exibirMensagem("T", "Deslogando Itapoa");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btn btn - label btn - label - brand btn - sm btn - bold')[0].click()");
            exibirMensagem("S", "Saindo da página...");

            bCarregado = false;
            AguardaPaginaCarregar();
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");

        }
        #endregion

        public bool ConsultarContainerParanagua(List<ListaDeCampos> ListaContainers, ref List<int> lstVA)
        {
            try
            {
                var client = new ServiceRefTCP.WebservicesClientes_ExportacaoCheioPortTypeClient("WebservicesClientes_ExportacaoCheioHttpSoap11Endpoint");

                var basicAuthBehavior = new BasicAuthEndpointBehavior("wssatel", "!wssatel&2024%");
                client.Endpoint.EndpointBehaviors.Add(basicAuthBehavior);
                /*
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServiceRefTCP.RetornoValidacao validacaoTCP2 = new ServiceRefTCP.RetornoValidacao();
                var t = client.ConsultarDue("24BR0011243277", out validacaoTCP2);
                */

                ServiceRefTCP.RequestHeader HeaderTCP = new ServiceRefTCP.RequestHeader();
                HeaderTCP.EmpresaSelecionada = ""; // 67620377000114

                ServiceRefTCP.RetornoValidacao validacaoTCP = new ServiceRefTCP.RetornoValidacao();

                DateTime dtInicio = DateTime.Today.AddDays(-6);
                //DateTime dtInicio = DateTime.Parse("2024-08-15");
                DateTime dtFim = DateTime.Today.AddDays(1);
                //DateTime dtFim = DateTime.Parse("2024-08-17");

                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.Open(); // Abrir a conexão com o serviço
                    var containersRetornados = client.ConsultarMovimentacao(HeaderTCP, dtInicio, dtFim, out validacaoTCP);
                    if (containersRetornados != null && validacaoTCP == null)
                    {
                        string nmTerminal = "TCP";

                        foreach (var containerConsultado in containersRetornados)
                        {
                            var conteudo = ListaContainers.Where(y => y.NR_CONTAINER == containerConsultado.Conteiner).FirstOrDefault();
                            if (conteudo != null)
                            {
                                lstVA.Add(conteudo.CD_VIAGEM_ARMADOR);

                                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                                string terminalSistema = "";

                                if (conteudo.CD_TERMINAL_EMBARQUE == idTermEmbraport)
                                {
                                    terminalSistema = "DP WORLD";
                                }
                                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermSB)
                                {
                                    terminalSistema = "SANTOS BRASIL";
                                }
                                else if (conteudo.CD_TERMINAL_EMBARQUE == idBTP)
                                {
                                    terminalSistema = "BTP";
                                }
                                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermVilaConde)
                                {
                                    terminalSistema = "VILA DO CONDE";
                                }
                                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItajai)
                                {
                                    terminalSistema = "ITAJAI";
                                }
                                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItapoa)
                                {
                                    terminalSistema = "ITAPOA";
                                }
                                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermTCP)
                                {
                                    terminalSistema = "TCP";
                                }

                                novoDado.IC_TIPO = conteudo.IC_TIPO;

                                ListaDeCampos dadosAtualizado = dados.atualizaDados(conteudo.CD_PROCESSORESERVA);
                                conteudo.NR_BOOKING = dadosAtualizado.NR_BOOKING;
                                conteudo.NM_NAVIO = dadosAtualizado.NM_NAVIO;

                                if (containerConsultado.Booking.Contains(conteudo.NR_BOOKING) && containerConsultado.Navio.ToUpper().Contains(conteudo.NM_NAVIO))
                                {
                                    dados.AtualizaDataDeposito(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, containerConsultado.DataChegada, containerConsultado.DataEmbarque);

                                    /*
                                    //VER SE ESTÁ PROTOCOLADO
                                    if (containerConsultado.OrdemDeEmbarque != null)
                                    {
                                        dados.AtualizaDataProtocolado(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, conteudo.NR_CONTAINER, novoDado);
                                        novoDado.DT_PROTOCOLO = DateTime.Now;
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                    }*/

                                    if (containerConsultado.DataChegada != null)
                                    {
                                        if (containerConsultado.DataEmbarque != null)
                                        {
                                            novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                                            novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                                            novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                                            novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                                            novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                                            novoDado.NM_TERMINAL = nmTerminal;
                                            novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                                            novoDado.NM_NAVIO = conteudo.NM_NAVIO;

                                            novoDado.NR_BOOKING_TERMINAL = containerConsultado.Booking;
                                            novoDado.NM_NAVIO_TERMINAL = containerConsultado.Navio.ToUpper();
                                            novoDado.NR_DUE = conteudo.NR_DUE;
                                            novoDado.NR_DUE_TERMINAL = containerConsultado.DdeDue;

                                            novoDado.DT_DEPOSITO = containerConsultado.DataChegada;
                                            novoDado.DT_EMBARQUE = containerConsultado.DataEmbarque;
                                            novoDado.DS_STATUS = "EMBARCADO";
                                            novoDado.NM_TERMINAL_SISTEMA = terminalSistema;

                                            novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                                            novoDado.IC_TIPO = "E";
                                            novoDado.DT_ETA = conteudo.DT_ETA;

                                            novoDado.DS_LACRE_AGENCIA_TERMINAL = containerConsultado.LacreArmador ?? "";
                                            novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA ?? "";
                                            novoDado.DS_LACRE_SIF_TERMINAL = containerConsultado.LacreSif ?? "";
                                            novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF ?? "";

                                            if (conteudo.IC_TIPO != "C")
                                                dados.InsereConsulta(novoDado);
                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "CONTAINER EMBARCADO", novoDado);
                                        }
                                        else
                                        {
                                            string status = "OK - " + containerConsultado.DdeDue;
                                            novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                                            novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                                            novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                                            novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                                            novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                                            novoDado.NM_TERMINAL = nmTerminal;
                                            novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                                            novoDado.NM_NAVIO = conteudo.NM_NAVIO;

                                            novoDado.NR_BOOKING_TERMINAL = containerConsultado.Booking;
                                            novoDado.NM_NAVIO_TERMINAL = containerConsultado.Navio.ToUpper();
                                            novoDado.NR_DUE = conteudo.NR_DUE;
                                            novoDado.NR_DUE_TERMINAL = containerConsultado.DdeDue;

                                            novoDado.DT_DEPOSITO = containerConsultado.DataChegada;
                                            novoDado.DT_EMBARQUE = containerConsultado.DataEmbarque;
                                            if (conteudo.CD_TERMINAL_EMBARQUE != 220)
                                                status = "TERMINAL DIVERGENTE";
                                            novoDado.DS_STATUS = status;
                                            novoDado.DT_ETA = conteudo.DT_ETA;

                                            novoDado.DS_LACRE_AGENCIA_TERMINAL = containerConsultado.LacreArmador ?? "";
                                            novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA ?? "";
                                            novoDado.DS_LACRE_SIF_TERMINAL = containerConsultado.LacreSif ?? "";
                                            novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF ?? "";

                                            novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                                            novoDado.NM_TERMINAL_SISTEMA = terminalSistema;
                                            if (conteudo.IC_TIPO == "D")
                                                dados.InsereConsulta(novoDado);

                                            if (conteudo.IC_TIPO == "C")
                                            {
                                                status = "CONTAINER DESEMBARCADO";
                                                novoDado.DS_STATUS = status;
                                                dados.InsereConsulta(novoDado);
                                            }

                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, status, novoDado);
                                        }
                                        if (!dados.retornaLacreValidado(conteudo.CD_PROCESSORESERVACONTAINER))
                                        {
                                            ValidaDivergenciaLacres(novoDado, conteudo.CD_CLIENTE);
                                            dados.AtualizaLacreValidado(conteudo.CD_PROCESSORESERVACONTAINER);
                                        }
                                        LimpaDados();
                                    }
                                    else
                                    {
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK, MAS NÃO DEPOSITOU" + containerConsultado.DdeDue, novoDado);
                                    }
                                }
                                else
                                {
                                    //if (containerConsultado.DataChegada != null) COMENTADO PARA PARANAGUA
                                    {
                                        //if (containerConsultado.DataChegada > conteudo.DT_ABERTURA.Value.AddDays(-7) || containerConsultado.Booking.Contains(conteudo.NR_BOOKING.Trim())) COMENTADO PARA PARANAGUA
                                        {
                                            //    //Não embarcou ainda e os dados estão divergente, então grava os dados para avisar o responsável
                                            //    if (dtAux != null)
                                            //    {
                                            //        //Container consta como depositado no terminal de embarque então grava no sistema que já está ok

                                            //        dados.AtualizaDataDeposito(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, dtAux, dtEmbarque);
                                            //        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                            //    }
                                            string status = "DADOS DIVERGENTE";

                                            novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                                            novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                                            novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                                            novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                                            novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                                            novoDado.NM_TERMINAL = nmTerminal;
                                            novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                                            novoDado.NM_NAVIO = conteudo.NM_NAVIO;

                                            novoDado.NR_BOOKING_TERMINAL = containerConsultado.Booking;
                                            novoDado.NM_NAVIO_TERMINAL = containerConsultado.Navio.ToUpper();
                                            novoDado.NR_DUE = conteudo.NR_DUE;
                                            novoDado.NR_DUE_TERMINAL = containerConsultado.DdeDue;

                                            novoDado.DT_EMBARQUE = containerConsultado.DataEmbarque;
                                            novoDado.DT_DEPOSITO = containerConsultado.DataChegada;
                                            novoDado.DS_STATUS = status + " - " + containerConsultado.DdeDue;
                                            if (conteudo.CD_TERMINAL_EMBARQUE != 220)
                                            {
                                                status = "TERMINAL DIVERGENTE";
                                                novoDado.DS_STATUS = status;
                                            }
                                            novoDado.NM_TERMINAL_SISTEMA = terminalSistema;
                                            novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                                            novoDado.IC_TIPO = "D";

                                            novoDado.DT_ETA = conteudo.DT_ETA;

                                            novoDado.DS_LACRE_AGENCIA_TERMINAL = containerConsultado.LacreArmador ?? "";
                                            novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA ?? "";
                                            novoDado.DS_LACRE_SIF_TERMINAL = containerConsultado.LacreSif ?? "";
                                            novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF ?? "";

                                            if (conteudo.IC_TIPO == "C" && containerConsultado.DataEmbarque == null)
                                            {
                                                status = "CONTAINER DESEMBARCADO E DADOS DIVERGENTE";
                                                novoDado.DS_STATUS = status;
                                                //dados.InsereConsulta(novoDado);
                                            }
                                            dados.InsereConsulta(novoDado);
                                            /*if (conteudo.IC_TIPO != "C")
                                                dados.InsereConsulta(novoDado);*/
                                            dados.AtualizaDataDeposito(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, containerConsultado.DataChegada, containerConsultado.DataEmbarque);
                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, status + " - " + containerConsultado.DdeDue, novoDado);
                                            if (!dados.retornaLacreValidado(conteudo.CD_PROCESSORESERVACONTAINER))
                                            {
                                                ValidaDivergenciaLacres(novoDado, conteudo.CD_CLIENTE);
                                                dados.AtualizaLacreValidado(conteudo.CD_PROCESSORESERVACONTAINER);
                                            }
                                            LimpaDados();
                                        }
                                        //else COMENTADO PARA PARANAGUA
                                        //{
                                        //    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "NÃO CONFERE COM OS DADOS DO CONTAINER.", novoDado);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (((System.Xml.XmlCharacterData)((System.Xml.XmlNode[])validacaoTCP.ListaMensagemRetornoWs[0].Codigo)[0]).Data != "WSC-003")
                        {
                            // SE DER ERRO E NÃO FOR "SEM MOVIMENTACOES", MANDAR EMAIL PRO SUPORTE
                        }
                    }
                }
                catch// (Exception e)
                {

                    throw;
                }
                finally
                {
                    if (client.State == CommunicationState.Faulted)
                        client.Abort();
                    else
                        client.Close();
                }
            }
            catch// (Exception e2)
            {

            }

            return true;
        }

        public void EncerrarNavegador()
        {
            Cef.Shutdown();
        }

        private bool ValidarSituacaoContainer(int idTerminal, ListaDeCampos conteudo, Task<JavascriptResponse> task1)
        {

            //Limpa dados 
            LimpaDados();
            //Pega os dados para gravar em uma tabela teste...
            var response1 = task1.Result;
            bool result1 = response1.Success;
            string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);

            if (result1)
            {
                //Pega os dados para gravar em uma tabela teste...
                string terminalSistema = "";
                string nmTerminal = "";
                if (idTerminal == idTermEmbraport)
                {
                    nmTerminal = "DP WORLD";
                    exibirMensagem("T", "Consultando DP World");
                }
                else if (idTerminal == idTermSB)
                {
                    nmTerminal = "SANTOS BRASIL";
                    exibirMensagem("T", "Consultando SANTOS BRASIL");

                }
                else if (idTerminal == idBTP)
                {
                    nmTerminal = "BTP";
                    exibirMensagem("T", "Consultando BTP");
                }
                else if (idTerminal == idTermVilaConde)
                {
                    nmTerminal = "VILA DO CONDE";
                    exibirMensagem("T", "Consultando Vila do Conde");
                }
                else if (idTerminal == idTermItajai)
                {
                    nmTerminal = "ITAJAI";
                    exibirMensagem("T", "Consultando Itajai");
                }
                else if (idTerminal == idTermItapoa)
                {
                    nmTerminal = "ITAPOA";
                    exibirMensagem("T", "Consultando Itapoa");
                }

                if (conteudo.CD_TERMINAL_EMBARQUE == idTermEmbraport)
                {
                    terminalSistema = "DP WORLD";
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermSB)
                {
                    terminalSistema = "SANTOS BRASIL";
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idBTP)
                {
                    terminalSistema = "BTP";
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermVilaConde)
                {
                    terminalSistema = "VILA DO CONDE";
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItajai)
                {
                    terminalSistema = "ITAJAI";
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItapoa)
                {
                    terminalSistema = "ITAPOA";
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermTCP)
                {
                    terminalSistema = "TCP";
                }

                string auxBooking = ""; //BOOKING
                string auxNavio = ""; //NAVIO
                string nrDUEAux = ""; //DUE
                string auxDesembaraco = "";
                string auxLocalEmbarque = "";
                string auxLocalCCT = "";
                string auxStatus = "";
                string auxLacreAgencia = "";
                string auxLacreSIF = "";

                if (response1.Result is List<object> resultList && resultList.Count > 1)
                {
                    auxBooking = resultList[2]?.ToString().Trim() ?? string.Empty; // BOOKING
                    auxNavio = resultList[3]?.ToString().Trim() ?? string.Empty; // NAVIO
                    nrDUEAux = resultList[1]?.ToString() ?? string.Empty; // DUE

                    if (idTerminal == idBTP)
                    {
                        if (resultList.Count > 6)
                        {
                            auxDesembaraco = resultList[6]?.ToString() ?? string.Empty; // Desembaraço
                            auxLocalEmbarque = resultList[7]?.ToString() ?? string.Empty; // Local Embarque
                            auxLocalCCT = resultList[8]?.ToString() ?? string.Empty; // Local CCT
                        }
                    }

                    if (idTerminal == idTermSB || idTerminal == idTermVilaConde)
                    {
                        if (resultList.Count > 5)
                            auxStatus = resultList[5]?.ToString() ?? string.Empty;

                        if (resultList.Count > 6)
                            auxLacreAgencia = resultList[6]?.ToString() ?? string.Empty;

                        if (resultList.Count > 7)
                            auxLacreSIF = resultList[7]?.ToString() ?? string.Empty;
                    }
                }

                //Se o terminal for DP World verifica se é importação, se for ignora.
                if (idTermEmbraport == idTerminal)
                {

                    string sgModalidade = ((List<object>)response1.Result)[5].ToString();
                    if (sgModalidade == "Importação" || sgModalidade == "Cabotagem")
                    {
                        //Container encontrado mas não pertence a exportação
                        //Nesse caso somente ignora
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "CONTAINER ENCONTRADO MAS NÃO EXPORTAÇÃO", novoDado);
                        return true;
                    }

                    auxLacreAgencia = ((List<object>)response1.Result)[6].ToString();
                    auxLacreSIF = ((List<object>)response1.Result)[7].ToString();
                }
                if (idTermSB == idTerminal || idTerminal == idTermVilaConde)
                {
                    if (auxStatus == "Container sem passagem no terminal.")
                    {
                        //Container não teve passagem no terminal então só ignora a consulta
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "CONTAINER SEM PASSAGEM NO TERMINAL", novoDado);
                        return true;
                    }
                }
                //Dados comum
                if (idBTP == idTerminal)
                {
                    //Verifica se trouxe resultados na consulta
                    if (((List<object>)response1.Result).Count == 1)
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "NÃO TROUXE RESULTADO NA CONSULTA", novoDado);
                        return true;
                    }
                }

                if (idTerminal == idTermItajai && ((List<object>)response1.Result).Count == 0)
                {
                    if (conteudo.IC_TIPO == "C")
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "CONTAINER DESEMBARCADO", novoDado);
                    }
                    else
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAJAI", DateTime.Now, "NÃO TROUXE RESULTADO NA CONSULTA", novoDado);
                    }
                    return true;
                }

                if (idTerminal == idTermItapoa && ((List<object>)response1.Result).Count == 0)
                {
                    if (conteudo.IC_TIPO == "C")
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "CONTAINER DESEMBARCADO", novoDado);
                    }
                    else
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "ITAPOA", DateTime.Now, "NÃO TROUXE RESULTADO NA CONSULTA", novoDado);
                    }
                    return true;
                }

                novoDado.IC_TIPO = conteudo.IC_TIPO;

                ListaDeCampos dadosAtualizado = dados.atualizaDados(conteudo.CD_PROCESSORESERVA);
                conteudo.NR_BOOKING = dadosAtualizado.NR_BOOKING;
                conteudo.NM_NAVIO = dadosAtualizado.NM_NAVIO;

                if (auxBooking.Contains(conteudo.NR_BOOKING.Trim()) && auxNavio.Contains(conteudo.NM_NAVIO))
                {
                    try
                    {
                        DateTime? dtAux = null;
                        if (((List<object>)response1.Result)[0] != null
                            && ((List<object>)response1.Result)[0].ToString() != ""
                            && ((List<object>)response1.Result)[0].ToString() != "01/01/0001 00:00:00"
                            && ((List<object>)response1.Result)[0].ToString() != "-")
                        {
                            dtAux = Convert.ToDateTime(((List<object>)response1.Result)[0].ToString());
                        }

                        DateTime? dtEmbarque = null;

                        if (((List<object>)response1.Result)[4] != null
                            && ((List<object>)response1.Result)[4].ToString() != ""
                            && ((List<object>)response1.Result)[4].ToString() != "-")
                        {
                            dtEmbarque = Convert.ToDateTime(((List<object>)response1.Result)[4].ToString());
                        }

                        if (dtAux != null || dtEmbarque != null)
                        {
                            //Container consta como depositado no terminal de embarque então grava no sistema que já está ok

                            dados.AtualizaDataDeposito(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, dtAux, dtEmbarque);

                            if (conteudo.CD_TERMINAL_EMBARQUE == idTerminal)
                            {
                                //Se for BTP verifica se tudo está ok para embarque
                                if (idTerminal == idBTP)
                                {
                                    //Baixa como protocolado
                                    if (auxDesembaraco == "Desembaraçada" && auxLocalEmbarque == auxLocalCCT && auxLocalEmbarque != "" && auxLocalCCT != "")
                                    {
                                        dados.AtualizaDataProtocolado(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, conteudo.NR_CONTAINER, novoDado);
                                        novoDado.DT_PROTOCOLO = DateTime.Now;
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                    }
                                    else
                                    {
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS NÃO OK PARA PROTOCOLAR", novoDado);
                                    }

                                }

                                //Se for Embraport e está tudo ok, então protocola
                                if (idTerminal == idTermEmbraport)
                                {
                                    //Baixa como protocolado
                                    if (nrDUEAux != "")
                                    {
                                        string nrDue = conteudo.NR_DUE == null ? "" : conteudo.NR_DUE;
                                        if (nrDUEAux.Contains(nrDue) && dados.dueDtDesembaraco(nrDue) != null)
                                        {
                                            dados.AtualizaDataProtocolado(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, conteudo.NR_CONTAINER, novoDado);
                                            novoDado.DT_PROTOCOLO = DateTime.Now;
                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                        }
                                        else
                                        {
                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS NÃO OK PARA PROTOCOLAR", novoDado);
                                        }
                                    }
                                }
                                //Se for Santos Brasil e tudo ok, então protocola
                                if (idTerminal == idTermSB || idTerminal == idTermVilaConde)
                                {
                                    //Baixa como protocolado
                                    if (nrDUEAux != "")
                                    {
                                        string nrDue = conteudo.NR_DUE == null ? "" : conteudo.NR_DUE;
                                        if (nrDUEAux.Contains(nrDue) && nrDUEAux.Contains("Desembaraçada"))
                                        {
                                            dados.AtualizaDataProtocolado(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, conteudo.NR_CONTAINER, novoDado);
                                            novoDado.DT_PROTOCOLO = DateTime.Now;
                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                        }
                                        else
                                        {
                                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS NÃO OK PARA PROTOCOLAR", novoDado);
                                        }
                                    }
                                }
                                //Se for Itajai e tudo ok, então protocola
                                if (idTerminal == idTermItajai)
                                {
                                    if (nrDUEAux == "S")
                                    {
                                        dados.AtualizaDataProtocolado(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, conteudo.NR_CONTAINER, novoDado);
                                        novoDado.DT_PROTOCOLO = DateTime.Now;
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                    }
                                    else
                                    {
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS NÃO OK PARA PROTOCOLAR", novoDado);
                                    }
                                }
                                //Se for Itapoa e tudo ok, então protocola
                                if (idTerminal == idTermItapoa)
                                {
                                    if (nrDUEAux == "SIM")
                                    {
                                        dados.AtualizaDataProtocolado(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, conteudo.NR_CONTAINER, novoDado);
                                        novoDado.DT_PROTOCOLO = DateTime.Now;
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                                    }
                                    else
                                    {
                                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS NÃO OK PARA PROTOCOLAR", novoDado);
                                    }
                                }
                            }
                            else
                            {
                                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "PROTOCOLADO EM TERMINAL DIVERGENTE", novoDado);
                            }

                            if (dtEmbarque != null)
                            {
                                novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                                novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                                novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                                novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                                novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                                novoDado.NM_TERMINAL = nmTerminal;
                                novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                                novoDado.NM_NAVIO = conteudo.NM_NAVIO;

                                novoDado.NR_BOOKING_TERMINAL = auxBooking;
                                novoDado.NM_NAVIO_TERMINAL = auxNavio;
                                novoDado.NR_DUE = conteudo.NR_DUE;
                                novoDado.NR_DUE_TERMINAL = nrDUEAux;

                                novoDado.DT_DEPOSITO = dtAux;
                                novoDado.DT_EMBARQUE = dtEmbarque;
                                novoDado.DS_STATUS = "EMBARCADO";
                                novoDado.NM_TERMINAL_SISTEMA = terminalSistema;

                                novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                                novoDado.IC_TIPO = "E";
                                novoDado.DT_ETA = conteudo.DT_ETA;

                                novoDado.DS_LACRE_AGENCIA_TERMINAL = auxLacreAgencia;
                                novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA;
                                novoDado.DS_LACRE_SIF_TERMINAL = auxLacreSIF;
                                novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF;

                                if (conteudo.IC_TIPO != "C")
                                    dados.InsereConsulta(novoDado);
                                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "CONTAINER EMBARCADO", novoDado);
                            }
                            else
                            {

                                string status = ""; // Valor padrão

                                if (response1.Result is List<object> resultListI && resultListI.Count > 1 && resultListI[1] != null)
                                {
                                    status = "OK - " + resultListI[1].ToString();
                                }
                                novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                                novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                                novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                                novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                                novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                                novoDado.NM_TERMINAL = nmTerminal;
                                novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                                novoDado.NM_NAVIO = conteudo.NM_NAVIO;

                                novoDado.NR_BOOKING_TERMINAL = auxBooking;
                                novoDado.NM_NAVIO_TERMINAL = auxNavio;
                                novoDado.NR_DUE = conteudo.NR_DUE;
                                novoDado.NR_DUE_TERMINAL = nrDUEAux;

                                novoDado.DT_DEPOSITO = dtAux;
                                novoDado.DT_EMBARQUE = dtEmbarque;
                                if (conteudo.CD_TERMINAL_EMBARQUE != idTerminal)
                                    status = "TERMINAL DIVERGENTE";
                                novoDado.DS_STATUS = status;
                                novoDado.DT_ETA = conteudo.DT_ETA;

                                novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                                novoDado.NM_TERMINAL_SISTEMA = terminalSistema;

                                novoDado.DS_LACRE_AGENCIA_TERMINAL = auxLacreAgencia;
                                novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA;
                                novoDado.DS_LACRE_SIF_TERMINAL = auxLacreSIF;
                                novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF;

                                if (conteudo.IC_TIPO == "D")
                                    dados.InsereConsulta(novoDado);

                                if (conteudo.IC_TIPO == "C")
                                {
                                    status = "CONTAINER DESEMBARCADO";
                                    novoDado.DS_STATUS = status;
                                    dados.InsereConsulta(novoDado);
                                }

                                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, status, novoDado);
                            }
                            //ValidaDivergenciaLacres(novoDado, conteudo.CD_CLIENTE);
                            LimpaDados();

                        }
                        else
                        {
                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK, MAS NÃO DEPOSITOU", novoDado);
                        }
                    }
                    catch (Exception e)
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "ERRO NÃO TRATADO. " + e.Message, novoDado);
                    }


                }
                else
                {
                    //Verifica se existe a data do Embarque, se existir realmente não confere com o container que estamos consultando

                    DateTime? dtAux = null;
                    if (((List<object>)response1.Result)[0].ToString() != "" && ((List<object>)response1.Result)[0].ToString() != "01/01/0001 00:00:00")
                        dtAux = Convert.ToDateTime(((List<object>)response1.Result)[0].ToString());

                    DateTime? dtEmbarque = null;
                    if (((List<object>)response1.Result)[4].ToString() != "")
                        dtEmbarque = Convert.ToDateTime(((List<object>)response1.Result)[4].ToString());

                    if (dtAux != null)
                    {
                        if (dtAux > conteudo.DT_ABERTURA.Value.AddDays(-7) || auxBooking.Contains(conteudo.NR_BOOKING.Trim()))
                        {
                            //    //Não embarcou ainda e os dados estão divergente, então grava os dados para avisar o responsável
                            //    if (dtAux != null)
                            //    {
                            //        //Container consta como depositado no terminal de embarque então grava no sistema que já está ok

                            //        dados.AtualizaDataDeposito(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, dtAux, dtEmbarque);
                            //        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "DADOS OK PARA PROTOCOLAR", novoDado);
                            //    }
                            string status = "DADOS DIVERGENTE";

                            novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                            novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                            novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                            novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                            novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                            novoDado.NM_TERMINAL = nmTerminal;
                            novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                            novoDado.NM_NAVIO = conteudo.NM_NAVIO;

                            novoDado.NR_BOOKING_TERMINAL = auxBooking;
                            novoDado.NM_NAVIO_TERMINAL = auxNavio;
                            novoDado.NR_DUE = conteudo.NR_DUE;
                            novoDado.NR_DUE_TERMINAL = nrDUEAux;

                            novoDado.DT_EMBARQUE = dtEmbarque;
                            novoDado.DT_DEPOSITO = dtAux;
                            novoDado.DS_STATUS = status + " - " + ((response1.Result as List<object>)?.ElementAtOrDefault(1)?.ToString() ?? "");
                            //novoDado.DS_STATUS = status + " - " + ((List<object>)response1.Result)[1].ToString();
                            if (conteudo.CD_TERMINAL_EMBARQUE != idTerminal)
                            {
                                status = "TERMINAL DIVERGENTE";
                                novoDado.DS_STATUS = status;
                            }
                            novoDado.NM_TERMINAL_SISTEMA = terminalSistema;
                            novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                            novoDado.IC_TIPO = "D";
                            novoDado.DT_ETA = conteudo.DT_ETA;

                            if (conteudo.IC_TIPO == "C" && dtEmbarque == null)
                            {
                                status = "CONTAINER DESEMBARCADO E DADOS DIVERGENTE";
                                novoDado.DS_STATUS = status;
                                //dados.InsereConsulta(novoDado);
                            }

                            novoDado.DS_LACRE_AGENCIA_TERMINAL = auxLacreAgencia;
                            novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA;
                            novoDado.DS_LACRE_SIF_TERMINAL = auxLacreSIF;
                            novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF;

                            dados.InsereConsulta(novoDado);
                            /*if (conteudo.IC_TIPO != "C")
                                dados.InsereConsulta(novoDado);*/
                            dados.AtualizaDataDeposito(conteudo.CD_PROCESSO, conteudo.CD_PROCESSORESERVA, conteudo.CD_PROCESSORESERVACONTAINER, dtAux, dtEmbarque);
                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, status + " - " + ((response1.Result as List<object>)?.ElementAtOrDefault(1)?.ToString() ?? ""), novoDado);
                            //ValidaDivergenciaLacres(novoDado, conteudo.CD_CLIENTE);
                            LimpaDados();
                        }
                        else
                        {
                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, nmTerminal, DateTime.Now, "NÃO CONFERE COM OS DADOS DO CONTAINER.", novoDado);
                        }
                    }
                }

            }
            return result1;
        }



        private void LimpaDados()
        {
            novoDado.CD_PROCESSO = 0;
            novoDado.CD_NUMERO_PROCESSO = null;
            novoDado.DS_REFERENCIA_CLIENTE = null;
            novoDado.NM_TERMINAL = null;
            novoDado.NR_CONTAINER = null;
            novoDado.DT_CONTAINER = null;
            novoDado.DT_EMBARQUE = null;
            novoDado.NR_BOOKING = null;
            novoDado.NM_NAVIO = null;
            novoDado.DS_STATUS = null;
            novoDado.NR_DUE = null;
            novoDado.DT_CONSULTA = null;
            novoDado.DT_PROTOCOLO = null;
            novoDado.NR_BOOKING_TERMINAL = null;
            novoDado.NM_NAVIO_TERMINAL = null;
            novoDado.NR_DUE_TERMINAL = null;
            novoDado.DT_DEPOSITO = null;
            novoDado.IC_TIPO = null;
            novoDado.DT_ETA = null;
        }

        public bool GerarPlanilhaExcel()
        {
            //Criar planilha

            string caminhoArquivo = "";

            string dsAssuntoEmail = "DADOS DIVERGENTES DO TERMINAL";
            string dsCorpoEmail = "Segue anexo planilha com os dados divergentes na consulta de deposito do container no(s) terminal(is).";

            caminhoArquivo = CriaPastaDoc("AVISODIVERGENCIA");





            List<InsereDados> lstPrc = dados.ConsultaDivergencia();
            if (lstPrc != null)
            {

                if (lstPrc.Count > 0)
                {

                    exibirMensagem("S", "Enviando e-mail para os analistas");
                    #region MONTAR EXCEL

                    clsExcel excel = new clsExcel();
                    //excel.MataExcelPerdido();


                    string sUsuario = "";
                    int? idUsuario = lstPrc.FirstOrDefault().CD_USUARIO == null ? 1 : lstPrc.FirstOrDefault().CD_USUARIO;


                    //Pega o nome do usuário
                    sUsuario = dados.RetornaEmailUsuario((int)idUsuario, "nome");

                    //int? cdGrupo = null;
                    int? cdUsuarioGrupo = null;

                    string PrefixoNmArquivo = "EDI_DEPOSITO_DIVERGENCIA_";
                    string nmArquivo = PrefixoNmArquivo + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";
                    //Gerar dados na planilha

                    excel.EscreverCelula(1, 1, "ANALISTA");
                    excel.EscreverCelula(1, 2, sUsuario);

                    excel.EscreverCelula(2, 1, "Processo");
                    excel.EscreverCelula(2, 2, "Ordem");
                    excel.EscreverCelula(2, 3, "Cliente");
                    excel.EscreverCelula(2, 4, "Container");
                    excel.EscreverCelula(2, 5, "Dead Line Container");
                    excel.EscreverCelula(2, 6, "Dt. Deposito");
                    excel.EscreverCelula(2, 7, "Dt. Protocolo");
                    excel.EscreverCelula(2, 8, "Dt. Embarque");
                    excel.EscreverCelula(2, 9, "Dt. ETA");
                    excel.EscreverCelula(2, 10, "Nr. Booking (Sistema)");
                    excel.EscreverCelula(2, 11, "Nr. Booking (Terminal)");
                    excel.EscreverCelula(2, 12, "Navio (Sistema)");
                    excel.EscreverCelula(2, 13, "Navio (Terminal)");
                    excel.EscreverCelula(2, 14, "Mensagem");
                    excel.EscreverCelula(2, 15, "Dt. Consulta");
                    excel.EscreverCelula(2, 16, "Terminal Consultado");
                    excel.EscreverCelula(2, 17, "Terminal Sistema");
                    excel.EscreverCelula(2, 18, "Status 2");
                    excel.EscreverCelula(2, 19, "Lacre Agência (Sistema)");
                    excel.EscreverCelula(2, 20, "Lacre Agência (Terminal)");
                    excel.EscreverCelula(2, 21, "Lacre SIF (Sistema)");
                    excel.EscreverCelula(2, 22, "Lacre SIF (Terminal)");
                    excel.FormatarFundo("A2", "V2");


                    int l = 3;
                    foreach (var conteudo in lstPrc)
                    {

                        cdUsuarioGrupo = conteudo.CD_USUARIO;

                        if (idUsuario != conteudo.CD_USUARIO)
                        {
                            //Pegar o e-mail do usuário
                            string to = dados.RetornaEmailUsuario((int)idUsuario, "email");

                            string copiaEmail = "";
                            List<UsuarioCopia> listaEmailCopia = new List<UsuarioCopia>();
                            listaEmailCopia = dados.emailUsuarioCopia(idUsuario);
                            if (listaEmailCopia.Count() > 0)
                            {

                                foreach (var itemEmail in listaEmailCopia)
                                {
                                    if (!string.IsNullOrEmpty(itemEmail.NM_EMAIL_USUARIO))
                                    {
                                        if (string.IsNullOrEmpty(copiaEmail))
                                        {
                                            copiaEmail = itemEmail.NM_EMAIL_USUARIO;
                                        }
                                        else
                                        {
                                            copiaEmail = copiaEmail + ";" + itemEmail.NM_EMAIL_USUARIO;
                                        }
                                    }
                                }
                            }/*
                            if (idUsuario == 27) // SE FOR VICTOR, MANDA NO MARFRIG@SATELDESPACHOS
                            {
                                to = "marfrig@sateldespachos.com.br";
                                copiaEmail = "";
                            }*/
                            sUsuario = dados.RetornaEmailUsuario((int)idUsuario, "nome");

                            if(idUsuario == 209)
                            {
                                to = "beatrizsilva@sateldespachos.com.br;erika@sateldespachos.com.br";
                            }

                            nmArquivo = PrefixoNmArquivo + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";

                            //dsAssuntoEmail = "DADOS DIVERGENTES DO TERMINAL";

                            //Gera o excel para enviar a planhilha para o cliente
                            excel.FormatarFonteEstilo("A2", "V2", true, false, false);
                            excel.FormatarBorda("A2", "V" + l.ToString());
                            excel.LarguraAuto();

                            excel.salvarComo(caminhoArquivo + "\\" + nmArquivo);
                            string caminhoExcel = caminhoArquivo + "\\" + nmArquivo;
                            excel.FecharExcel();

                            //Disparar Email para cliente
                            Email email = new Email();
                            email.EnviaEmailEDI(to, copiaEmail, dsAssuntoEmail, dsCorpoEmail, caminhoExcel);

                            idUsuario = conteudo.CD_USUARIO == null ? 1 : conteudo.CD_USUARIO;
                            sUsuario = conteudo.DS_GRUPO;

                            excel = new clsExcel();

                            l = 3;
                            excel.EscreverCelula(1, 1, "ANALISTA");
                            excel.EscreverCelula(1, 2, sUsuario);

                            excel.EscreverCelula(2, 1, "Processo");
                            excel.EscreverCelula(2, 2, "Ordem");
                            excel.EscreverCelula(2, 3, "Cliente");
                            excel.EscreverCelula(2, 4, "Container");
                            excel.EscreverCelula(2, 5, "Dead Line Container");
                            excel.EscreverCelula(2, 6, "Dt. Deposito");
                            excel.EscreverCelula(2, 7, "Dt. Protocolo");
                            excel.EscreverCelula(2, 8, "Dt. Embarque");
                            excel.EscreverCelula(2, 9, "Dt. ETA");
                            excel.EscreverCelula(2, 10, "Nr. Booking (Sistema)");
                            excel.EscreverCelula(2, 11, "Nr. Booking (Terminal)");
                            excel.EscreverCelula(2, 12, "Navio (Sistema)");
                            excel.EscreverCelula(2, 13, "Navio (Terminal)");
                            excel.EscreverCelula(2, 14, "Mensagem");
                            excel.EscreverCelula(2, 15, "Dt. Consulta");
                            excel.EscreverCelula(2, 16, "Terminal Consultado");
                            excel.EscreverCelula(2, 17, "Terminal Sistema");
                            excel.EscreverCelula(2, 18, "Status 2");
                            excel.EscreverCelula(2, 19, "Lacre Agência (Sistema)");
                            excel.EscreverCelula(2, 20, "Lacre Agência (Terminal)");
                            excel.EscreverCelula(2, 21, "Lacre SIF (Sistema)");
                            excel.EscreverCelula(2, 22, "Lacre SIF (Terminal)");
                            excel.FormatarFundo("A2", "V2");


                        }

                        string nrContainer = conteudo.NR_CONTAINER;
                        string nmCliente = conteudo.NM_CLIENTE;
                        string nrBooking = conteudo.NR_BOOKING + " ";
                        string nrBookingTerminal = conteudo.NR_BOOKING_TERMINAL + " ";
                        string nmNavio = conteudo.NM_NAVIO;
                        string nmNavioTerminal = conteudo.NM_NAVIO_TERMINAL;

                        string dtDeposito = "";
                        string dtEmbarque = "";
                        string dtConsulta = "";
                        string dsStatus = conteudo.DS_STATUS;
                        string nmTerminal = conteudo.NM_TERMINAL;
                        string nrProcesso = conteudo.CD_NUMERO_PROCESSO;
                        string nrOrdem = conteudo.DS_REFERENCIA_CLIENTE;
                        string dtContainer = "";
                        string dtProtocolo = "";
                        string dtEta = "";
                        string sTerminal = conteudo.NM_TERMINAL;
                        string sTerminalSistema = conteudo.NM_TERMINAL_SISTEMA;
                        string nmStatus2 = conteudo.NM_PROCESSO_STATUS2;

                        string nrLacreAgencia = conteudo.DS_LACRE_AGENCIA;
                        string nrLacreAgenciaTerminal = conteudo.DS_LACRE_AGENCIA_TERMINAL;
                        string nrLacreSIF = conteudo.DS_LACRE_SIF;
                        string nrLacreSIFTerminal = conteudo.DS_LACRE_SIF_TERMINAL;


                        if (conteudo.DT_DEPOSITO != null)
                        {
                            dtDeposito = DateTime.Parse(conteudo.DT_DEPOSITO.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }


                        if (conteudo.DT_CONSULTA != null)
                        {
                            dtConsulta = DateTime.Parse(conteudo.DT_CONSULTA.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }
                        if (conteudo.DT_CONTAINER != null)
                        {
                            dtContainer = DateTime.Parse(conteudo.DT_CONTAINER.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        if (conteudo.DT_PROTOCOLO != null)
                        {
                            dtProtocolo = DateTime.Parse(conteudo.DT_PROTOCOLO.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        if (conteudo.DT_EMBARQUE != null)
                        {
                            dtEmbarque = DateTime.Parse(conteudo.DT_EMBARQUE.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        if (conteudo.DT_ETA != null)
                        {
                            dtEta = DateTime.Parse(conteudo.DT_ETA.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        excel.EscreverCelula(l, 1, nrProcesso);
                        excel.EscreverCelula(l, 2, nrOrdem);
                        excel.EscreverCelula(l, 3, nmCliente);
                        excel.EscreverCelula(l, 4, nrContainer);
                        excel.EscreverCelula(l, 5, dtContainer);
                        excel.EscreverCelula(l, 6, dtDeposito);
                        excel.EscreverCelula(l, 7, dtProtocolo);
                        excel.EscreverCelula(l, 8, dtEmbarque);
                        //excel.FormatarTipoCelulaTexto("H" + (l).ToString(), "H" + (l).ToString());
                        excel.EscreverCelula(l, 9, dtEta);
                        excel.FormatarTipoCelulaTexto("J" + (l).ToString(), "J" + (l).ToString());
                        excel.EscreverCelula(l, 10, nrBooking);
                        excel.FormatarTipoCelulaTexto("K" + (l).ToString(), "K" + (l).ToString());
                        excel.EscreverCelula(l, 11, nrBookingTerminal);
                        excel.EscreverCelula(l, 12, nmNavio);
                        excel.EscreverCelula(l, 13, nmNavioTerminal);
                        excel.EscreverCelula(l, 14, dsStatus);
                        excel.EscreverCelula(l, 15, dtConsulta);
                        excel.EscreverCelula(l, 16, sTerminal);
                        excel.EscreverCelula(l, 17, sTerminalSistema);
                        excel.EscreverCelula(l, 18, nmStatus2);
                        excel.EscreverCelula(l, 19, nrLacreAgencia);
                        excel.EscreverCelula(l, 20, nrLacreAgenciaTerminal);
                        excel.EscreverCelula(l, 21, nrLacreSIF);
                        excel.EscreverCelula(l, 22, nrLacreSIFTerminal);
                        if (dtEmbarque != "" && conteudo.IC_TIPO == "D")
                        {
                            excel.FormatarFundoOrange("A" + l.ToString(), "V" + l.ToString());
                            excel.FormatarFonteEstilo("N" + l.ToString(), "N" + l.ToString(), true, false, false);
                        }
                        if (dsStatus == "TERMINAL DIVERGENTE")
                        {
                            PrefixoNmArquivo = "EDI_TERMINAL_DIVERGENTE_";
                            //nmArquivo = "EDI_TERMINAL_DIVERGENTE" + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";
                            //dsAssuntoEmail = "CONTAINERS EM TERMINAL DIVERGENTE";
                            excel.FormatarFundoBlue("A" + l.ToString(), "V" + l.ToString());
                            excel.FormatarFonteEstilo("N" + l.ToString(), "N" + l.ToString(), true, false, false);
                        }
                        if (conteudo.DT_ETA > conteudo.DT_EMBARQUE)
                        {
                            excel.FormatarFundoGreen("A" + l.ToString(), "V" + l.ToString());
                            excel.FormatarFonteEstilo("H" + l.ToString(), "I" + l.ToString(), true, false, false);
                        }
                        l++;
                    }

                    #endregion MONTAR EXCEL

                    //Pegar o e-mail do usuário
                    string to2 = dados.RetornaEmailUsuario((int)idUsuario, "email");


                    if (idUsuario == 209)
                    {
                        to2 = "beatrizsilva@sateldespachos.com.br;erika@sateldespachos.com.br";
                    }

                    string copiaEmail2 = "";

                    List<UsuarioCopia> listaEmailCopia2 = new List<UsuarioCopia>();
                    listaEmailCopia2 = dados.emailUsuarioCopia(cdUsuarioGrupo);
                    if (listaEmailCopia2.Count() > 0)
                    {

                        foreach (var itemEmail in listaEmailCopia2)
                        {
                            if (!string.IsNullOrEmpty(itemEmail.NM_EMAIL_USUARIO))
                            {
                                if (string.IsNullOrEmpty(copiaEmail2))
                                {
                                    copiaEmail2 = itemEmail.NM_EMAIL_USUARIO;
                                }
                                else
                                {
                                    copiaEmail2 = copiaEmail2 + ";" + itemEmail.NM_EMAIL_USUARIO;
                                }
                            }
                        }

                    }


                    sUsuario = dados.RetornaEmailUsuario((int)idUsuario, "nome");

                    //Gera o excel para enviar a planhilha para o cliente
                    excel.FormatarFonteEstilo("V2", "V2", true, false, false);
                    excel.FormatarBorda("A2", "V" + l.ToString());
                    excel.LarguraAuto();

                    nmArquivo = PrefixoNmArquivo + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";
                    //dsAssuntoEmail = "DADOS DIVERGENTES DO TERMINAL";

                    excel.salvarComo(caminhoArquivo + "\\" + nmArquivo);
                    string caminhoExcel2 = caminhoArquivo + "\\" + nmArquivo;
                    excel.FecharExcel();


                    //Disparar Email para cliente
                    Email email2 = new Email();
                    email2.EnviaEmailEDI(to2, copiaEmail2, dsAssuntoEmail, dsCorpoEmail, caminhoExcel2);

                }
            }

            exibirMensagem("S", "");
            return true;

        }


        public bool GerarPlanilhaExcelCliente(int ic_robo)
        {
            //Criar planilha

            string caminhoArquivo = "";

            string dsAssuntoEmail = "DADOS DIVERGENTES DO TERMINAL";
            string dsCorpoEmail = "Segue anexo planilha com os dados divergentes na consulta de deposito do container no(s) terminal(is).";

            caminhoArquivo = CriaPastaDoc("AVISODIVERGENCIA");





            List<InsereDados> lstPrc = dados.ConsultaDivergenciaCliente(ic_robo);
            if (lstPrc != null)
            {

                if (lstPrc.Count > 0)
                {

                    exibirMensagem("S", "Enviando e-mail para os analistas");
                    #region MONTAR EXCEL

                    clsExcel excel = new clsExcel();
                    //excel.MataExcelPerdido();


                    string sUsuario = "";
                    int? idUsuario = lstPrc.FirstOrDefault().CD_USUARIO == null ? 1 : lstPrc.FirstOrDefault().CD_USUARIO;


                    //Pega o nome do usuário
                    sUsuario = dados.RetornaEmailUsuario((int)idUsuario, "nome");

                    //int? cdGrupo = null;
                    int? cdUsuarioGrupo = null;

                    string PrefixoNmArquivo = "EDI_DEPOSITO_DIVERGENCIA_";
                    string nmArquivo = PrefixoNmArquivo + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";
                    //Gerar dados na planilha

                    excel.EscreverCelula(1, 1, "ANALISTA");
                    excel.EscreverCelula(1, 2, sUsuario);

                    excel.EscreverCelula(2, 1, "Processo");
                    excel.EscreverCelula(2, 2, "Ordem");
                    excel.EscreverCelula(2, 3, "Cliente");
                    excel.EscreverCelula(2, 4, "Container");
                    excel.EscreverCelula(2, 5, "Dead Line Container");
                    excel.EscreverCelula(2, 6, "Dt. Deposito");
                    excel.EscreverCelula(2, 7, "Dt. Protocolo");
                    excel.EscreverCelula(2, 8, "Dt. Embarque");
                    excel.EscreverCelula(2, 9, "Dt. ETA");
                    excel.EscreverCelula(2, 10, "Nr. Booking (Sistema)");
                    excel.EscreverCelula(2, 11, "Nr. Booking (Terminal)");
                    excel.EscreverCelula(2, 12, "Navio (Sistema)");
                    excel.EscreverCelula(2, 13, "Navio (Terminal)");
                    excel.EscreverCelula(2, 14, "Mensagem");
                    excel.EscreverCelula(2, 15, "Dt. Consulta");
                    excel.EscreverCelula(2, 16, "Terminal Consultado");
                    excel.EscreverCelula(2, 17, "Terminal Sistema");
                    excel.EscreverCelula(2, 18, "Status 2");
                    excel.EscreverCelula(2, 19, "Lacre Agência (Sistema)");
                    excel.EscreverCelula(2, 20, "Lacre Agência (Terminal)");
                    excel.EscreverCelula(2, 21, "Lacre SIF (Sistema)");
                    excel.EscreverCelula(2, 22, "Lacre SIF (Terminal)");
                    excel.FormatarFundo("A2", "V2");


                    int l = 3;
                    foreach (var conteudo in lstPrc)
                    {

                        cdUsuarioGrupo = conteudo.CD_USUARIO;

                        if (idUsuario != conteudo.CD_USUARIO)
                        {
                            //Pegar o e-mail do usuário
                            string to = dados.RetornaEmailUsuario((int)idUsuario, "email");

                            string copiaEmail = "";
                            List<UsuarioCopia> listaEmailCopia = new List<UsuarioCopia>();
                            listaEmailCopia = dados.emailUsuarioCopia(idUsuario);
                            if (listaEmailCopia.Count() > 0)
                            {

                                foreach (var itemEmail in listaEmailCopia)
                                {
                                    if (!string.IsNullOrEmpty(itemEmail.NM_EMAIL_USUARIO))
                                    {
                                        if (string.IsNullOrEmpty(copiaEmail))
                                        {
                                            copiaEmail = itemEmail.NM_EMAIL_USUARIO;
                                        }
                                        else
                                        {
                                            copiaEmail = copiaEmail + ";" + itemEmail.NM_EMAIL_USUARIO;
                                        }
                                    }
                                }
                            }/*
                            if (idUsuario == 27) // SE FOR VICTOR, MANDA NO MARFRIG@SATELDESPACHOS
                            {
                                to = "marfrig@sateldespachos.com.br";
                                copiaEmail = "";
                            }*/
                            sUsuario = dados.RetornaEmailUsuario((int)idUsuario, "nome");

                            if (idUsuario == 209)
                            {
                                to = "beatrizsilva@sateldespachos.com.br;erika@sateldespachos.com.br";
                            }

                            nmArquivo = PrefixoNmArquivo + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";

                            //dsAssuntoEmail = "DADOS DIVERGENTES DO TERMINAL";

                            //Gera o excel para enviar a planhilha para o cliente
                            excel.FormatarFonteEstilo("A2", "V2", true, false, false);
                            excel.FormatarBorda("A2", "V" + l.ToString());
                            excel.LarguraAuto();

                            excel.salvarComo(caminhoArquivo + "\\" + nmArquivo);
                            string caminhoExcel = caminhoArquivo + "\\" + nmArquivo;
                            excel.FecharExcel();

                            //Disparar Email para cliente
                            Email email = new Email();
                            email.EnviaEmailEDI(to, copiaEmail, dsAssuntoEmail, dsCorpoEmail, caminhoExcel);

                            idUsuario = conteudo.CD_USUARIO == null ? 1 : conteudo.CD_USUARIO;
                            sUsuario = conteudo.DS_GRUPO;

                            excel = new clsExcel();

                            l = 3;
                            excel.EscreverCelula(1, 1, "ANALISTA");
                            excel.EscreverCelula(1, 2, sUsuario);

                            excel.EscreverCelula(2, 1, "Processo");
                            excel.EscreverCelula(2, 2, "Ordem");
                            excel.EscreverCelula(2, 3, "Cliente");
                            excel.EscreverCelula(2, 4, "Container");
                            excel.EscreverCelula(2, 5, "Dead Line Container");
                            excel.EscreverCelula(2, 6, "Dt. Deposito");
                            excel.EscreverCelula(2, 7, "Dt. Protocolo");
                            excel.EscreverCelula(2, 8, "Dt. Embarque");
                            excel.EscreverCelula(2, 9, "Dt. ETA");
                            excel.EscreverCelula(2, 10, "Nr. Booking (Sistema)");
                            excel.EscreverCelula(2, 11, "Nr. Booking (Terminal)");
                            excel.EscreverCelula(2, 12, "Navio (Sistema)");
                            excel.EscreverCelula(2, 13, "Navio (Terminal)");
                            excel.EscreverCelula(2, 14, "Mensagem");
                            excel.EscreverCelula(2, 15, "Dt. Consulta");
                            excel.EscreverCelula(2, 16, "Terminal Consultado");
                            excel.EscreverCelula(2, 17, "Terminal Sistema");
                            excel.EscreverCelula(2, 18, "Status 2");
                            excel.EscreverCelula(2, 19, "Lacre Agência (Sistema)");
                            excel.EscreverCelula(2, 20, "Lacre Agência (Terminal)");
                            excel.EscreverCelula(2, 21, "Lacre SIF (Sistema)");
                            excel.EscreverCelula(2, 22, "Lacre SIF (Terminal)");
                            excel.FormatarFundo("A2", "V2");


                        }

                        string nrContainer = conteudo.NR_CONTAINER;
                        string nmCliente = conteudo.NM_CLIENTE;
                        string nrBooking = conteudo.NR_BOOKING + " ";
                        string nrBookingTerminal = conteudo.NR_BOOKING_TERMINAL + " ";
                        string nmNavio = conteudo.NM_NAVIO;
                        string nmNavioTerminal = conteudo.NM_NAVIO_TERMINAL;

                        string dtDeposito = "";
                        string dtEmbarque = "";
                        string dtConsulta = "";
                        string dsStatus = conteudo.DS_STATUS;
                        string nmTerminal = conteudo.NM_TERMINAL;
                        string nrProcesso = conteudo.CD_NUMERO_PROCESSO;
                        string nrOrdem = conteudo.DS_REFERENCIA_CLIENTE;
                        string dtContainer = "";
                        string dtProtocolo = "";
                        string dtEta = "";
                        string sTerminal = conteudo.NM_TERMINAL;
                        string sTerminalSistema = conteudo.NM_TERMINAL_SISTEMA;
                        string nmStatus2 = conteudo.NM_PROCESSO_STATUS2;

                        string nrLacreAgencia = conteudo.DS_LACRE_AGENCIA;
                        string nrLacreAgenciaTerminal = conteudo.DS_LACRE_AGENCIA_TERMINAL;
                        string nrLacreSIF = conteudo.DS_LACRE_SIF;
                        string nrLacreSIFTerminal = conteudo.DS_LACRE_SIF_TERMINAL;


                        if (conteudo.DT_DEPOSITO != null)
                        {
                            dtDeposito = DateTime.Parse(conteudo.DT_DEPOSITO.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }


                        if (conteudo.DT_CONSULTA != null)
                        {
                            dtConsulta = DateTime.Parse(conteudo.DT_CONSULTA.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }
                        if (conteudo.DT_CONTAINER != null)
                        {
                            dtContainer = DateTime.Parse(conteudo.DT_CONTAINER.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        if (conteudo.DT_PROTOCOLO != null)
                        {
                            dtProtocolo = DateTime.Parse(conteudo.DT_PROTOCOLO.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        if (conteudo.DT_EMBARQUE != null)
                        {
                            dtEmbarque = DateTime.Parse(conteudo.DT_EMBARQUE.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        if (conteudo.DT_ETA != null)
                        {
                            dtEta = DateTime.Parse(conteudo.DT_ETA.ToString()).ToString("MM/dd/yyyy HH:mm");
                        }

                        excel.EscreverCelula(l, 1, nrProcesso);
                        excel.EscreverCelula(l, 2, nrOrdem);
                        excel.EscreverCelula(l, 3, nmCliente);
                        excel.EscreverCelula(l, 4, nrContainer);
                        excel.EscreverCelula(l, 5, dtContainer);
                        excel.EscreverCelula(l, 6, dtDeposito);
                        excel.EscreverCelula(l, 7, dtProtocolo);
                        excel.EscreverCelula(l, 8, dtEmbarque);
                        //excel.FormatarTipoCelulaTexto("H" + (l).ToString(), "H" + (l).ToString());
                        excel.EscreverCelula(l, 9, dtEta);
                        excel.FormatarTipoCelulaTexto("J" + (l).ToString(), "J" + (l).ToString());
                        excel.EscreverCelula(l, 10, nrBooking);
                        excel.FormatarTipoCelulaTexto("K" + (l).ToString(), "K" + (l).ToString());
                        excel.EscreverCelula(l, 11, nrBookingTerminal);
                        excel.EscreverCelula(l, 12, nmNavio);
                        excel.EscreverCelula(l, 13, nmNavioTerminal);
                        excel.EscreverCelula(l, 14, dsStatus);
                        excel.EscreverCelula(l, 15, dtConsulta);
                        excel.EscreverCelula(l, 16, sTerminal);
                        excel.EscreverCelula(l, 17, sTerminalSistema);
                        excel.EscreverCelula(l, 18, nmStatus2);
                        excel.EscreverCelula(l, 19, nrLacreAgencia);
                        excel.EscreverCelula(l, 20, nrLacreAgenciaTerminal);
                        excel.EscreverCelula(l, 21, nrLacreSIF);
                        excel.EscreverCelula(l, 22, nrLacreSIFTerminal);
                        if (dtEmbarque != "" && conteudo.IC_TIPO == "D")
                        {
                            excel.FormatarFundoOrange("A" + l.ToString(), "V" + l.ToString());
                            excel.FormatarFonteEstilo("N" + l.ToString(), "N" + l.ToString(), true, false, false);
                        }
                        if (dsStatus == "TERMINAL DIVERGENTE")
                        {
                            PrefixoNmArquivo = "EDI_TERMINAL_DIVERGENTE_";
                            //nmArquivo = "EDI_TERMINAL_DIVERGENTE" + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";
                            //dsAssuntoEmail = "CONTAINERS EM TERMINAL DIVERGENTE";
                            excel.FormatarFundoBlue("A" + l.ToString(), "V" + l.ToString());
                            excel.FormatarFonteEstilo("N" + l.ToString(), "N" + l.ToString(), true, false, false);
                        }
                        if (conteudo.DT_ETA > conteudo.DT_EMBARQUE)
                        {
                            excel.FormatarFundoGreen("A" + l.ToString(), "V" + l.ToString());
                            excel.FormatarFonteEstilo("H" + l.ToString(), "I" + l.ToString(), true, false, false);
                        }
                        l++;
                    }

                    #endregion MONTAR EXCEL

                    //Pegar o e-mail do usuário
                    string to2 = dados.RetornaEmailUsuario((int)idUsuario, "email");


                    if (idUsuario == 209)
                    {
                        to2 = "beatrizsilva@sateldespachos.com.br;erika@sateldespachos.com.br";
                    }

                    string copiaEmail2 = "";

                    List<UsuarioCopia> listaEmailCopia2 = new List<UsuarioCopia>();
                    listaEmailCopia2 = dados.emailUsuarioCopia(cdUsuarioGrupo);
                    if (listaEmailCopia2.Count() > 0)
                    {

                        foreach (var itemEmail in listaEmailCopia2)
                        {
                            if (!string.IsNullOrEmpty(itemEmail.NM_EMAIL_USUARIO))
                            {
                                if (string.IsNullOrEmpty(copiaEmail2))
                                {
                                    copiaEmail2 = itemEmail.NM_EMAIL_USUARIO;
                                }
                                else
                                {
                                    copiaEmail2 = copiaEmail2 + ";" + itemEmail.NM_EMAIL_USUARIO;
                                }
                            }
                        }

                    }


                    sUsuario = dados.RetornaEmailUsuario((int)idUsuario, "nome");

                    //Gera o excel para enviar a planhilha para o cliente
                    excel.FormatarFonteEstilo("V2", "V2", true, false, false);
                    excel.FormatarBorda("A2", "V" + l.ToString());
                    excel.LarguraAuto();

                    nmArquivo = PrefixoNmArquivo + sUsuario + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";
                    //dsAssuntoEmail = "DADOS DIVERGENTES DO TERMINAL";

                    excel.salvarComo(caminhoArquivo + "\\" + nmArquivo);
                    string caminhoExcel2 = caminhoArquivo + "\\" + nmArquivo;
                    excel.FecharExcel();


                    //Disparar Email para cliente
                    Email email2 = new Email();
                    email2.EnviaEmailEDI(to2, copiaEmail2, dsAssuntoEmail, dsCorpoEmail, caminhoExcel2);

                }
            }

            exibirMensagem("S", "");
            return true;

        }

        public string CriaPastaDoc(string nmPasta)
        {
            string caminho = "";
            string caminhoRaiz = ConfigurationManager.AppSettings["nmCaminhoPlanilha"].ToString();

            if (caminhoRaiz != null)
                caminho = caminhoRaiz + nmPasta;

            var dirDOC = new DirectoryInfo(caminho);
            if (dirDOC.Exists == false)
            {
                dirDOC.Create();
                dirDOC.Attributes = FileAttributes.Normal;
            }

            //Verifica se a pasta do processo existe
            string ano = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.Day.ToString();

            string sCaminhoDestino = caminho + "\\" + ano;
            var dirProcesso = new DirectoryInfo(sCaminhoDestino);
            if (dirProcesso.Exists == false)
            {
                dirProcesso.Create();
                dirProcesso.Attributes = FileAttributes.Normal;
            }

            sCaminhoDestino = caminho + "\\" + ano + "\\" + mes;
            dirProcesso = new DirectoryInfo(sCaminhoDestino);
            if (dirProcesso.Exists == false)
            {
                dirProcesso.Create();
                dirProcesso.Attributes = FileAttributes.Normal;
            }

            sCaminhoDestino = caminho + "\\" + ano + "\\" + mes + "\\" + dia;
            dirProcesso = new DirectoryInfo(sCaminhoDestino);
            if (dirProcesso.Exists == false)
            {
                dirProcesso.Create();
                dirProcesso.Attributes = FileAttributes.Normal;
            }
            return sCaminhoDestino;
        }

        public void AdicionarNaoEncontrado(ListaDeCampos conteudo, string nmTerm)
        {
            string terminalSistema = "";
            if (conteudo.CD_TERMINAL_EMBARQUE == idTermEmbraport)
            {
                terminalSistema = "DP WORLD";
            }
            else if (conteudo.CD_TERMINAL_EMBARQUE == idTermSB)
            {
                terminalSistema = "SANTOS BRASIL";
            }
            else if (conteudo.CD_TERMINAL_EMBARQUE == idBTP)
            {
                terminalSistema = "BTP";
            }
            else if (conteudo.CD_TERMINAL_EMBARQUE == idTermVilaConde)
            {
                terminalSistema = "VILA DO CONDE";
            }
            else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItajai)
            {
                terminalSistema = "ITAJAI";
            }
            else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItapoa)
            {
                terminalSistema = "ITAPOA";
            }

            string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
            novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
            novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
            novoDado.CD_NUMERO_PROCESSO = nrProcesso;
            novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
            novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
            novoDado.NR_BOOKING = conteudo.NR_BOOKING;
            novoDado.NM_NAVIO = conteudo.NM_NAVIO;
            novoDado.DS_STATUS = "CONTAINER NÃO ENCONTRADO EM 3 TENTATIVAS";
            novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
            novoDado.IC_TIPO = "D";
            novoDado.NM_TERMINAL = nmTerm;
            novoDado.NM_TERMINAL_SISTEMA = terminalSistema;

            dados.InsereConsulta(novoDado);
            LimpaDados();
        }

        public bool GerarPlanilhaExcelNavio()
        {
            //Criar planilha

            string caminhoArquivo = "";

            string dsAssuntoEmail = "NAVIOS DIVERGENTES DO TERMINAL";
            string dsCorpoEmail = "Segue anexo planilha com os navios atracados divergentes com o sistema.";

            caminhoArquivo = CriaPastaDoc("AVISODIVERGENCIA");





            List<ListaNavio> lstPrc = dados.RetornaNaviosDivergentes();
            if (lstPrc != null)
            {

                if (lstPrc.Count > 0)
                {

                    exibirMensagem("S", "Enviando e-mail para os analistas");
                    #region MONTAR EXCEL

                    clsExcel excel = new clsExcel();
                    //excel.MataExcelPerdido();

                    //Pegar o e-mail do usuário
                    string to = ConfigurationManager.AppSettings["EmailNavio"].ToString();

                    string nmArquivo = "";
                    //Gerar dados na planilha

                    excel.EscreverCelula(1, 1, "Terminal");
                    excel.EscreverCelula(1, 2, "Viagem");
                    excel.EscreverCelula(1, 3, "Navio");
                    excel.EscreverCelula(1, 4, "Data ETA (Terminal)");
                    excel.EscreverCelula(1, 5, "Data ETA (Sistema)");
                    excel.FormatarFundo("A1", "E1");


                    int l = 2;

                    foreach (var conteudo in lstPrc)
                    {

                        string dtEtaTerminal = "";
                        string dtETA = "";
                        //string dtConsulta = "";

                        if (conteudo.DT_ETA_TERMINAL != null)
                        {
                            dtEtaTerminal = DateTime.Parse(conteudo.DT_ETA_TERMINAL.ToString()).ToString("dd/MM/yyyy HH:mm");
                            excel.FormatarTipoCelulaTexto("D" + (l).ToString(), "D" + (l).ToString());
                        }

                        if (conteudo.DT_ETA != null)
                        {
                            dtETA = DateTime.Parse(conteudo.DT_ETA.ToString()).ToString("dd/MM/yyyy HH:mm");
                            excel.FormatarTipoCelulaTexto("E" + (l).ToString(), "E" + (l).ToString());
                        }

                        excel.EscreverCelula(l, 1, conteudo.NM_TERMINAL);
                        excel.EscreverCelula(l, 2, conteudo.NR_VIAGEM);
                        excel.EscreverCelula(l, 3, conteudo.NM_NAVIO);
                        excel.EscreverCelula(l, 4, dtEtaTerminal);
                        excel.EscreverCelula(l, 5, dtETA);
                        l++;
                    }

                    #endregion MONTAR EXCEL

                    nmArquivo = "EDI_DEPOSITO_DIVERGENCIA_NAVIOS_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + ".xlsx";

                    //Gera o excel para enviar a planhilha para o cliente
                    excel.FormatarFonteEstilo("A1", "E1", true, false, false);
                    excel.FormatarBorda("A1", "E" + l.ToString());
                    excel.LarguraAuto();

                    excel.salvarComo(caminhoArquivo + "\\" + nmArquivo);
                    string caminhoExcel = caminhoArquivo + "\\" + nmArquivo;
                    excel.FecharExcel();

                    //Disparar Email para cliente
                    Email email = new Email();
                    email.EnviaEmailEDI(to, "", dsAssuntoEmail, dsCorpoEmail, caminhoExcel);

                }
            }

            exibirMensagem("S", "");
            return true;

        }

        public bool EntrarPaginaNavio(int? idTerminal)
        {
            bCarregado = false;
            try
            {
                string url = "";
                string site = "";
                DateTime dtAtual = DateTime.Now.AddDays(-3);

                switch (idTerminal)
                {
                    case 188:
                        site = "https://www.santosbrasil.com.br/v2021/lista-de-atracacao?titulo=Tecon+Santos&unidade=tecon-santos&lista=lista-de-atracacao&atracadouro=TECON&dataInicial=" + dtAtual.Year + "-" + dtAtual.Month + "-" + dtAtual.Day;
                        break;
                    case 206:
                        site = "http://www.embraportonline.com.br/Navios/Escala";
                        break;
                    case 210:
                        site = "https://tas.btp.com.br/ConsultasLivres/ListaAtracacaoIndex";
                        break;
                    case 220:
                        break;
                    default:
                        break;
                }

                int icontador = 0;
                do
                {
                    bCarregado = false;

                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + site + "';");
                    chromeBrowser.Load("document.location ='" + site + "';");
                    bCarregado = AguardaPaginaCarregar();

                    //Verifica se conseguiu acessar a página.
                    url = chromeBrowser.Address;
                    icontador++;

                } while (url != site && !bCarregado && icontador < 3);

                if (idTerminal == 210)
                {
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('SubMenu-1')[0].getElementsByTagName('a')[0].click();");
                    //AguardaPaginaCarregar(ref tsStatus);

                    Application.DoEvents();
                    Thread.Sleep(1500);
                }
            }
            catch
            {
                bCarregado = false;
            }
            return bCarregado;
        }

        public void deslogando()
        {

            exibirMensagem("T", "Deslogando");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");
            exibirMensagem("S", "Aguardando...");
            bCarregado = false;
            AguardaPaginaCarregar();
        }

        public bool ConsultarNavioSantosBrasil(ListaNavio conteudo)
        {
            try
            {
                //DateTime dtAtual = DateTime.Now.AddDays(20);
                if (!conteudo.NR_VIAGEM.Contains("SSZ_"))
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('pesquisa').value = '" + conteudo.NR_VIAGEM + "';");
                else
                    chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('pesquisa').value = '';");
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsById('dataInicial').value = '" + dtAtual.Year + "-" + dtAtual.Month + "-" + dtAtual.Day + "';");
                var taskx = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnPesquisa').click();");
                taskx.Wait();
                AguardaPaginaCarregar();

                Application.DoEvents();

                string sJanelaCarregada = "function PegarDados() {";
                sJanelaCarregada += "var tabela = document.getElementById('tableListaDeAtracacao');";
                sJanelaCarregada += "var corpo = tabela.getElementsByTagName('tbody');";
                sJanelaCarregada += "var linhas = corpo[0].getElementsByTagName('tr');";
                sJanelaCarregada += "if (linhas != null) { return true; }";
                sJanelaCarregada += "return false; }";
                sJanelaCarregada += "PegarDados();";
                bool bOk = false;
                tentativa = 0;
                bAlert = false;
                do
                {
                    var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                    task.Wait();
                    var response = task.Result;
                    bool result = response.Success;
                    if (result)
                    {
                        bOk = response.Result.ToString() == "True";
                    }
                    Application.DoEvents();
                    Thread.Sleep(750);
                    tentativa++;
                } while (!bOk && tentativa < 20);

                DateTime? dtETA = null;

                sJanelaCarregada = "function PegarDados() {";
                sJanelaCarregada += "var tabela = document.getElementById('tableListaDeAtracacao');";
                sJanelaCarregada += "var corpo = tabela.getElementsByTagName('tbody');";
                sJanelaCarregada += "var linhas = corpo[0].getElementsByTagName('tr');";
                sJanelaCarregada += "var dtAtracacao = [];";
                sJanelaCarregada += "for(var i = 0; i < linhas.length; i++) {";
                sJanelaCarregada += "var navio = linhas[i].getElementsByTagName('td')[2].innerText;";
                sJanelaCarregada += "var dtETA = linhas[i].getElementsByTagName('td')[5].innerText;";
                sJanelaCarregada += "if (navio.includes('" + conteudo.NM_NAVIO + "')){ ";
                sJanelaCarregada += "dtAtracacao[i] = linhas[i].getElementsByTagName('td')[5].innerText;";
                sJanelaCarregada += "} }";
                sJanelaCarregada += "return dtAtracacao.filter(Boolean); }";
                sJanelaCarregada += "PegarDados();";

                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task1.Wait();
                var response1 = task1.Result;
                bool result1 = response1.Success;
                if (result1)
                {
                    List<DateTime> listDatas = ((List<object>)response1.Result).Select(i => DateTime.Parse(i.ToString()).AddHours(-1).Date).ToList();// REMOVE UMA HORA, POIS O SITE ADIANTA UMA HORA
                    if (listDatas.Contains(DateTime.Parse(conteudo.DT_ETA.Value.ToShortDateString())))
                    {
                        conteudo.DT_ETA_TERMINAL = conteudo.DT_ETA;
                        dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "SANTOS BRASIL", DateTime.Now, "ETA IGUAL", conteudo);
                        return true;
                    }
                    dtETA = listDatas.Min();
                    if (Math.Abs(conteudo.DT_ETA.Value.Subtract(DateTime.Parse(dtETA.ToString())).Days) == 1)
                    {
                        conteudo.DT_ETA_TERMINAL = dtETA;
                        dados.AtualizaDataETA(conteudo.CD_VIAGEM, dtETA);
                        dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "SANTOS BRASIL", DateTime.Now, "ETA ALTERADO", conteudo);
                        return true;
                    }
                }
                Application.DoEvents();
                Thread.Sleep(750);

                conteudo.DT_ETA_TERMINAL = dtETA;
                dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "SANTOS BRASIL", DateTime.Now, "ETA DIVERGENTE", conteudo);
                dados.InsereConsultaNavio(conteudo);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConsultarNavioEmbraport(ListaNavio conteudo)
        {
            try
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('edNavio').value = '" + conteudo.NM_NAVIO + "';");
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('edDataInicial').value = '" + DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy") + "'");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#edNavio').change();");
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("$('#edDataInicial').change();");
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btPesquisar').click();");
                //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btn btn-outline btn-sm btn-success')[0].click();");
                AguardaPaginaCarregar();

                Application.DoEvents();
                Thread.Sleep(750);

                string sJanelaCarregada = "function PegarDados() {";
                sJanelaCarregada += "var tabela = document.getElementsByClassName('table-responsive')[1];";
                sJanelaCarregada += "var corpo = tabela.getElementsByTagName('tbody');";
                sJanelaCarregada += "var linhas = corpo[1].getElementsByTagName('tr');";
                sJanelaCarregada += "if (linhas != null) { return true; }";
                sJanelaCarregada += "return false; }";
                sJanelaCarregada += "PegarDados();";
                bool bOk = false;
                tentativa = 0;
                bAlert = false;
                do
                {
                    var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                    task.Wait();
                    var response = task.Result;
                    bool result = response.Success;
                    if (result)
                    {
                        bOk = response.Result.ToString() == "True";
                    }
                    Application.DoEvents();
                    Thread.Sleep(750);
                    tentativa++;
                } while (!bOk && tentativa < 20);

                DateTime? dtETA = null;

                sJanelaCarregada = "function PegarDados() {";
                sJanelaCarregada += "var tabela = document.getElementsByClassName('table-responsive')[1];";
                sJanelaCarregada += "var corpo = tabela.getElementsByTagName('tbody');";
                sJanelaCarregada += "var linhas = corpo[1].getElementsByTagName('tr');";
                sJanelaCarregada += "var dtAtracacao = []; var t = 0;";
                sJanelaCarregada += "if (linhas == null) { return ''; }";
                sJanelaCarregada += "for(var i = 0; i < linhas.length; i++) {";
                sJanelaCarregada += "var navio = linhas[i].getElementsByTagName('td')[0].innerText;";
                if (conteudo.NR_VIAGEM.Contains("SSZ_"))
                {
                    sJanelaCarregada += "var dtETA = linhas[i].getElementsByTagName('td')[7].innerText;";
                    sJanelaCarregada += "if (navio.includes('" + conteudo.NM_NAVIO + "') && linhas[i].getElementsByTagName('td')[9].innerText != ''){ ";
                }
                else
                {
                    sJanelaCarregada += "var viagem = linhas[i].getElementsByTagName('td')[1].innerText;";
                    sJanelaCarregada += "var dtETA = linhas[i].getElementsByTagName('td')[7].innerText;";
                    sJanelaCarregada += "if (viagem.includes('" + conteudo.NR_VIAGEM + "') && navio.includes('" + conteudo.NM_NAVIO + "')){ ";
                }
                sJanelaCarregada += "dtAtracacao[t] = linhas[i].getElementsByTagName('td')[7].innerText;t++;";
                sJanelaCarregada += "} }";
                //sJanelaCarregada += "dtAtracacao = dtAtracacao != '' ? '--' : dtAtracacao;";
                sJanelaCarregada += "return dtAtracacao; }";
                sJanelaCarregada += "PegarDados();";

                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task1.Wait();
                var response1 = task1.Result;
                bool result1 = response1.Success;
                if (result1)
                {
                    List<DateTime> listDatas = ((List<object>)response1.Result).Select(i => DateTime.Parse(i.ToString()).Date).ToList();
                    if (listDatas.Contains(DateTime.Parse(conteudo.DT_ETA.Value.ToShortDateString())))
                    {
                        conteudo.DT_ETA_TERMINAL = conteudo.DT_ETA;
                        dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "DP WORLD", DateTime.Now, "ETA IGUAL", conteudo);
                        return true;
                    }
                    if (listDatas.Count > 0)
                    {
                        dtETA = listDatas.Min();
                        if (Math.Abs(conteudo.DT_ETA.Value.Subtract(DateTime.Parse(dtETA.ToString())).Days) == 1)
                        {
                            conteudo.DT_ETA_TERMINAL = dtETA;
                            dados.AtualizaDataETA(conteudo.CD_VIAGEM, dtETA);
                            dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "DP WORLD", DateTime.Now, "ETA ALTERADO", conteudo);
                            return true;
                        }
                    }
                }
                Application.DoEvents();
                Thread.Sleep(750);

                conteudo.DT_ETA_TERMINAL = dtETA;
                dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "DP WORLD", DateTime.Now, "ETA DIVERGENTE", conteudo);
                dados.InsereConsultaNavio(conteudo);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConsultarNavioBTP(ListaNavio conteudo)
        {
            try
            {
                string sJanelaCarregada = "function PegarDados() {";
                sJanelaCarregada += "var tabela = document.getElementsByClassName('jtable')[0];";
                sJanelaCarregada += "var corpo = tabela.getElementsByTagName('tbody');";
                sJanelaCarregada += "var linhas = corpo[0].getElementsByTagName('tr');";
                sJanelaCarregada += "if (linhas == null) { return false; }";
                sJanelaCarregada += "return true; }";
                sJanelaCarregada += "PegarDados();";
                bool bOk = false;
                tentativa = 0;
                bAlert = false;
                do
                {
                    var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                    task.Wait();
                    var response = task.Result;
                    bool result = response.Success;
                    if (result)
                    {
                        bOk = response.Result.ToString() == "True";
                    }
                    Application.DoEvents();
                    Thread.Sleep(750);
                    tentativa++;
                } while (!bOk && tentativa < 20);

                DateTime? dtETA = null;

                sJanelaCarregada = "function PegarDados() {";
                sJanelaCarregada += "var tabela = document.getElementsByClassName('jtable')[0];";
                sJanelaCarregada += "var corpo = tabela.getElementsByTagName('tbody');";
                sJanelaCarregada += "var linhas = corpo[0].getElementsByTagName('tr');";
                sJanelaCarregada += "var dtAtracacao = [];";
                sJanelaCarregada += "if (linhas == null) { return ''; }";
                sJanelaCarregada += "for(var i = 0; i < linhas.length; i++) {";
                sJanelaCarregada += "var navio = linhas[i].getElementsByTagName('td')[1].innerText;";
                sJanelaCarregada += "var viagem = linhas[i].getElementsByTagName('td')[2].innerText;";
                sJanelaCarregada += "var dtETA = linhas[i].getElementsByTagName('td')[4].innerText;";
                if (!conteudo.NR_VIAGEM.Contains("SSZ_"))
                    sJanelaCarregada += "if (navio.includes('" + conteudo.NM_NAVIO + "') && viagem.includes('" + conteudo.NR_VIAGEM + "')){ ";
                else
                    sJanelaCarregada += "if (navio.includes('" + conteudo.NM_NAVIO + "')){ ";
                sJanelaCarregada += "dtAtracacao[i] = linhas[i].getElementsByTagName('td')[4].innerText;";
                sJanelaCarregada += "} }";
                //sJanelaCarregada += "dtAtracacao = dtAtracacao != '' ? '--' : dtAtracacao;";
                sJanelaCarregada += "return dtAtracacao.filter(Boolean); }";
                sJanelaCarregada += "PegarDados();";

                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task1.Wait();
                var response1 = task1.Result;
                bool result1 = response1.Success;
                if (result1)
                {
                    List<DateTime> listDatas = ((List<object>)response1.Result).Select(i => DateTime.Parse(i.ToString()).Date).ToList();
                    if (listDatas.Contains(DateTime.Parse(conteudo.DT_ETA.Value.ToShortDateString())))
                    {
                        conteudo.DT_ETA_TERMINAL = conteudo.DT_ETA;
                        dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "BTP", DateTime.Now, "ETA IGUAL", conteudo);
                        return true;
                    }
                    dtETA = listDatas.Min();
                    if (Math.Abs(conteudo.DT_ETA.Value.Subtract(DateTime.Parse(dtETA.ToString())).Days) == 1)
                    {
                        dados.AtualizaDataETA(conteudo.CD_VIAGEM, dtETA);
                        conteudo.DT_ETA_TERMINAL = dtETA;
                        dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "BTP", DateTime.Now, "ETA ALTERADO", conteudo);
                        return true;
                    }
                }
                Application.DoEvents();
                Thread.Sleep(750);

                conteudo.DT_ETA_TERMINAL = dtETA;
                dados.GeraLogNavioConsultado(conteudo.CD_VIAGEM, "BTP", DateTime.Now, "ETA DIVERGENTE", conteudo);
                dados.InsereConsultaNavio(conteudo);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConsultarLacreSantosBrasil(ListaDeCampos conteudo)
        {

            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            bCarregado = false;
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('container cntr')[0].value = '" + conteudo.NR_CONTAINER + "';");
            Application.DoEvents();
            Thread.Sleep(100);
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('Export')[0].click();");
            //AguardaPaginaCarregar(ref tsStatus);

            Application.DoEvents();

            string sJanelaCarregada = "var j = document.getElementsByClassName('infConteiner janela')[0];";
            sJanelaCarregada += " if (j != null) { true} else {false}";
            bool bOk = false;
            tentativa = 0;
            bAlert = false;
            do
            {
                var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bOk = response.Result.ToString() == "True";

                }
                Application.DoEvents();
                Thread.Sleep(750);
                tentativa++;
            } while ((!bOk && tentativa < 40) && (!bAlert));
            if (tentativa >= 40)
            {
                //Não encontrou o container
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "ESGOTADO TENTATIVAS (20). CONTAINER NÃO ENCONTRADO", novoDado);
                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");

                return false;
            }
            if (bAlert)
            {
                //Não encontrou o container
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "CONTAINER NÃO ENCONTRADO", novoDado);
                string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
                return true;
            }

            string sDataDeposito = "function retornaValor(){var janela = document.getElementsByClassName('infConteiner janela');";
            sDataDeposito += " var conteudo = [];";
            sDataDeposito += " var info = janela[0].getElementsByClassName('info');";
            sDataDeposito += " var li = info[0].getElementsByTagName('li');";
            sDataDeposito += " conteudo[0] = ''; conteudo[1] = '';";
            sDataDeposito += " conteudo[2] = ''; conteudo[3] = '';";
            sDataDeposito += " conteudo[4] = ''; conteudo[5] = '';";
            sDataDeposito += " conteudo[6] = ''; conteudo[7] = '';";
            sDataDeposito += " ";

            sDataDeposito += " for(var i=0;i< li.length;i++){";
            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Entr. terminal'){";
            sDataDeposito += " conteudo[0] = li[i].getElementsByTagName('span')[0].innerText;}";                                // DATA DE DEPÓSITO NO TERMINAL
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Embarque'){";
            sDataDeposito += " conteudo[4] = li[i] != null ? li[i].getElementsByTagName('span')[0].innerText : '';}}";          // DATA DE EMBARQUE

            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Booking'){";
            sDataDeposito += " conteudo[2] = li[i].getElementsByTagName('span')[0].innerText;}}";

            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText.includes('DUE')){";
            sDataDeposito += " conteudo[1] = li[i].getElementsByTagName('span')[0].innerText;}}";

            sDataDeposito += " if (li[i].getElementsByTagName('label').length > 0){";
            sDataDeposito += " if (li[i].getElementsByTagName('label')[0].innerText == 'Navio'){";
            sDataDeposito += " conteudo[3] = li[i].getElementsByTagName('span')[0].innerText;}}}";

            sDataDeposito += " if (info[0].getElementsByClassName('aviso').length == 0){";
            sDataDeposito += " conteudo[5] = document.getElementsByClassName('status')[0].innerText;";
            sDataDeposito += " if (!conteudo[5].includes('Sa�do do Terminal') && !conteudo[5].includes('Saído do Terminal')) conteudo[4] = '';}";
            sDataDeposito += " else{";
            sDataDeposito += " conteudo[5] = info[0].getElementsByClassName('aviso')[0].innerText;}";


            sDataDeposito += " var lacres = document.getElementById('tabelalacreCNTR').getElementsByTagName('tbody')[0].getElementsByTagName('tr');";
            sDataDeposito += " for (var i = 0; i < lacres.length; i++) {";
            sDataDeposito += " var numeroLacre = lacres[i].getElementsByTagName('td')[0].innerText.trim();";
            sDataDeposito += " var tipoLacre = lacres[i].getElementsByTagName('td')[1].innerText.trim();"; 
            sDataDeposito += " console.log('Tipo de Lacre:', tipoLacre, 'Número de Lacre:', numeroLacre);"; 
            sDataDeposito += " if (tipoLacre == 'Lacre Armador') { conteudo[6] = numeroLacre; }";
            sDataDeposito += " if (tipoLacre == 'Lacre Veterinário(SIF)') { conteudo[7] = numeroLacre; }";
            sDataDeposito += " }";





            sDataDeposito += " return conteudo} retornaValor(); ";
            bool retorno = false;
            tentativa = 0;
            do
            {

                var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                //task1.Wait();
                var response1 = task1.Result;

                retorno = ValidarSituacaoLacreSantosBrasil(conteudo, task1);

                bCarregado = false;

                Thread.Sleep(300);
                Application.DoEvents();
                tentativa++;
            } while (!retorno && tentativa < 20);
            //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('btnFecha close')[2].click();");
            if (tentativa >= 20)
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "SANTOS BRASIL", DateTime.Now, "ESGOTADO TENTATIVAS (2) - 2. DESLOGANDO E PARANDO", novoDado);
                deslogandoSantosBrasil();
                bCarregado = false;
                return false;
            }


            bCarregado = false;

            return retorno;
        }

        public bool ConsultarLacreEmbraport(ListaDeCampos conteudo)
        {
            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            bCarregado = false;
            //Chama a tela de pesquisa do container
            bVerifica = VerificaJanelaAjaxCarregada("edNroConteiner", "id");

            if (bVerifica)
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btLimpar').click();");
                Thread.Sleep(300);
                Application.DoEvents();
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('edNroConteiner').value = '" + conteudo.NR_CONTAINER + "';");
                Application.DoEvents();
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btPesquisar').click();");

                bVerifica = VerificaJanelaAjaxCarregada("loading", "class-visible");

                Application.DoEvents();
                Thread.Sleep(500);


                //Verifica se mostra mensagem de container não existe
                string sJanelaCarregada = "var j = document.getElementsByClassName('box-error').length > 0 ? document.getElementsByClassName('box-error')[0].style.display == 'none' : true;";
                sJanelaCarregada += " if (j == true) { true} else {false}";
                bool bNaoEncontrou = false;
                var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                task.Wait();
                var response = task.Result;
                bool result = response.Success;
                if (result)
                {
                    bNaoEncontrou = response.Result.ToString() == "True";
                }
                if (!bNaoEncontrou)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "NÃO ENCONTROU O CONTAINER", novoDado);
                    sJanelaCarregada = "if(document.getElementsByClassName('box-error')[0] != null) document.getElementsByClassName('box-error')[0].click();";
                    task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                    task.Wait();
                    //Thread.Sleep(300);
                    //bVerifica = VerificaJanelaAjaxCarregada("box-error", "class-visible");
                    return true;
                }

                if (bVerifica)
                {
                    //Verifica se o container já está depositado
                    Thread.Sleep(300);
                    Application.DoEvents();
                    Thread.Sleep(300);
                    Application.DoEvents();
                    string sNaoEncontrou = "var erro = document.getElementsByClassName('box-error')[0].style.display == 'none';";
                    sNaoEncontrou += "!erro;";
                    var taskn = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sNaoEncontrou);
                    taskn.Wait();
                    var responsen = taskn.Result;
                    bool resultn = responsen.Success;
                    bNaoEncontrou = true;

                    if (resultn)
                    {
                        bNaoEncontrou = responsen.Result.ToString() == "True";

                    }
                    else
                        bNaoEncontrou = false;


                    //Pegar os dados da tela
                    if (!bNaoEncontrou)
                    {
                        bool result1 = false;
                        string sDataDeposito = "function retornaValor(){var dv = document.getElementById('dataTable');";
                        sDataDeposito += " var conteudo = [];";
                        sDataDeposito += " var tb = dv.getElementsByTagName('table');";
                        sDataDeposito += " var tb1 = null;";
                        sDataDeposito += " for (let i = 1; i < tb.length; i++) {";
                        sDataDeposito += " if(tb[i].innerText.includes('Lacre M')) {";
                        sDataDeposito += " tb1 = tb[i];";
                        sDataDeposito += " break;";
                        sDataDeposito += " }}";
                        sDataDeposito += " var tb1 = tb1.getElementsByTagName('tr');;";
                        sDataDeposito += " for (let j = 0; j < tb1.length; j++) {";
                        sDataDeposito += " conteudo[j] = tb1[j].getElementsByTagName('td')[3].innerText;";
                        sDataDeposito += " }";
                        sDataDeposito += " return conteudo} retornaValor(); ";
                        tentativa = 0;
                        do
                        {

                            var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                            task1.Wait();
                            var response1 = task1.Result;

                            result1 = ValidarSituacaoLacre(conteudo, task1);

                            bCarregado = false;

                            Thread.Sleep(300);
                            Application.DoEvents();
                            tentativa++;
                        } while (!result1 && tentativa < 20);
                        //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
                        if (tentativa >= 20)
                        {
                            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "DP WORLD", DateTime.Now, "HOUVE PROBLEMA PARA CONSULTAR (20). TENTANDO NOVAMENTE", novoDado);
                            deslogandoEmbraport();
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        /*
                        novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                        novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                        novoDado.DT_CONSULTA = DateTime.Now;
                        novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                        novoDado.CD_NUMERO_PROCESSO = conteudo.CD_NUMERO_PROCESSO;
                        novoDado.DS_STATUS = "NÃO ENCONTROU";
                        novoDado.NM_TERMINAL = "DP WORLD";
                        dados.InsereConsulta(novoDado);
                        */
                        return true;
                    }

                }
                else
                {
                    //Caso não tenha carregado os dados sai e tenta novamente.
                    deslogandoEmbraport();
                    return false;
                }
            }
            else
            {
                //Caso não tenha conseguindo encontrar o campo de consulta do container desloga do sistema e tenta novamente
                deslogandoEmbraport();
                return false;
            }
        }

        public bool ConsultarLacreBTP(ListaDeCampos conteudo, bool bPrimeiraVez)
        {
            dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "INICIANDO CONSULTA", novoDado);
            //if (bPrimeiraVez)
            //{
            bCarregado = false;
            //chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://tas.btp.com.br/b2b/consultaconteiner';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementsByClassName('SubMenu-2')[0].getElementsByTagName('a')[3].click();");
            AguardaPaginaCarregar();
            //}
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('ddlCategoria').value = 'E';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('txtNumeroConteiner').value = '" + conteudo.NR_CONTAINER + "';");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById('btnPesquisaConteinerTAS').click();");


            VerificaJanelaAjaxCarregada("carregando-gif ico", "class-visible");

            //Verifica se o container já está depositado
            string sNaoEncontrou = "var janela = document.getElementsByClassName('jtable');";
            sNaoEncontrou += "var tabela = janela[0];";
            sNaoEncontrou += "var col = tabela.getElementsByTagName('td');";
            sNaoEncontrou += "var linha = col[0].innerText;";
            sNaoEncontrou += "linha == 'Não existem registros a listar!';";
            var taskn = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sNaoEncontrou);
            taskn.Wait();
            var responsen = taskn.Result;
            bool resultn = responsen.Success;
            bool bNaoEncontrou = true;
            if (resultn)
            {
                bNaoEncontrou = responsen.Result.ToString() == "True";

            }
            else
                bNaoEncontrou = false;

            if (!bNaoEncontrou)
            {
                //Verifica se já carregou os dados na tela
                //=======================================================

                string sJanelaCarregada = "function Carregou(listadue) { var h = 0; do { sleep(500); x++; } while(listadue[2] == null && x < 8) }";
                sJanelaCarregada += "function sleep(milliseconds) {const date = Date.now();let currentDate = null;do{currentDate = Date.now();} while (currentDate - date < milliseconds);}";
                sJanelaCarregada += "var bOk = false;";
                sJanelaCarregada += "var janela = document.getElementsByClassName('jtable');";
                sJanelaCarregada += " var x = janela[0].getElementsByTagName('tr');";
                sJanelaCarregada += " var y = janela[2].getElementsByTagName('td');";
                sJanelaCarregada += " for(var i=x.length - 1;i>0;i--){";
                sJanelaCarregada += "   var k = x[i].getElementsByTagName('td');";
                sJanelaCarregada += "   var sbooking = k[3].innerText;";
                sJanelaCarregada += "   if (sbooking.trim().includes('" + conteudo.NR_BOOKING.Trim() + "')){";
                sJanelaCarregada += "      var chek = k[0].getElementsByTagName('input'); ";
                sJanelaCarregada += "      chek[0].click(); Carregou(y);";
                sJanelaCarregada += "      if (y[2] != null) {bOk = true;} else {if (y[0].innerText == 'Não existem registros a listar!'){bOk = true;}} "; //DUE
                sJanelaCarregada += "      break;";
                sJanelaCarregada += "   }}";
                sJanelaCarregada += "   bOk";
                var taskx = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sJanelaCarregada);
                taskx.Wait();
                if (taskx.Result.Result.ToString().ToUpper() == "FALSE")
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
                    return true;
                }
                //precisa verificar se é de entrada ou saída quando encontrar.
                bool bOk = false;
                int cont = 0;
                do
                {
                    var task = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("var bretorno = []; bretorno[0] = false; bretorno[1] = false; if (y[2] != null) {bretorno[0] = true;} else {if (y[0].innerText == 'Não existem registros a listar!' || !bok){bretorno[0] = true;}} bretorno;"); task.Wait();
                    var response = task.Result;
                    bool result = response.Success;
                    if (result)
                    {
                        if (((List<object>)response.Result)[1].ToString() == "True")
                            return true;
                        bOk = ((List<object>)response.Result)[0].ToString() == "True";

                    }
                    else
                    {

                    }
                    Application.DoEvents();
                    Thread.Sleep(700);
                    cont++;
                } while (!bOk && cont < tbmEsperaAjax);
                //===========================================================
                //AguardaPaginaCarregar(ref tsStatus);
                //Verifica se puxou os dados.
                //VerificaJanelaAjaxCarregada("txtConteiner", "input");
                Thread.Sleep(1000);
                Application.DoEvents();
                //return true;



                string sDataDeposito = "function retornaValor(){var janela = document.getElementsByClassName('jtable');";
                sDataDeposito += " var x = janela[1].getElementsByTagName('tr');";
                sDataDeposito += " var conteudo = [];";
                sDataDeposito += " for(var i=1; i<x.length;i++){";
                sDataDeposito += "   conteudo[i-1] = x[i].getElementsByTagName('td')[1].innerText;";
                sDataDeposito += "   }";
                sDataDeposito += " return conteudo} retornaValor(); ";
                tentativa = 0;
                try
                {
                    do
                    {

                        var task1 = chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(sDataDeposito);
                        task1.Wait();
                        var response1 = task1.Result;

                        resultn = ValidarSituacaoLacre(conteudo, task1);

                        bCarregado = false;

                        Thread.Sleep(600);
                        Application.DoEvents();
                        tentativa++;
                    } while (!resultn && tentativa < 20);
                    //Caso ocorra algum erro na consulta, desloga do site e envia false para tentar novamente.
                    if (tentativa >= 20)
                    {
                        dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "TENTATIVAS ESGOTADAS (20). TENTANDO NOVAMENTE.", novoDado);
                        deslogandoBTP();
                        return false;
                    }

                    bCarregado = false;
                }
                catch (Exception e)
                {
                    dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "ERRO INESPERADO. " + e.Message, novoDado);
                    return false;
                }

            }
            else
            {
                dados.GeraLogContainerConsultado(conteudo.CD_PROCESSO, conteudo.NR_CONTAINER, "BTP", DateTime.Now, "NÃO ENCONTROU CONTAINER", novoDado);
            }

            return true;
        }

        private bool ValidarSituacaoLacre(ListaDeCampos conteudo, Task<JavascriptResponse> task1)
        {

            //Limpa dados 
            LimpaDados();
            //Pega os dados para gravar em uma tabela teste...
            var response1 = task1.Result;
            bool result1 = response1.Success;
            string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);

            if (result1)
            {
                //Pega os dados para gravar em uma tabela teste...
                string nmTerminal = "";
                if (conteudo.CD_TERMINAL_EMBARQUE == idTermEmbraport)
                {
                    nmTerminal = "DP WORLD";
                    exibirMensagem("T", "Consultando DP World");
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermSB)
                {
                    nmTerminal = "SANTOS BRASIL";
                    exibirMensagem("T", "Consultando SANTOS BRASIL");
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idBTP)
                {
                    nmTerminal = "BTP";
                    exibirMensagem("T", "Consultando BTP");
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermVilaConde)
                {
                    nmTerminal = "VILA DO CONDE";
                    exibirMensagem("T", "Consultando Vila do Conde");
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItajai)
                {
                    nmTerminal = "ITAJAI";
                    exibirMensagem("T", "Consultando Itajai");
                }
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermItapoa)
                {
                    nmTerminal = "ITAPOA";
                    exibirMensagem("T", "Consultando Itapoa");
                }

                List<object> objectList = (List<object>)response1.Result;
                List<string> stringList = objectList.Select(obj => obj.ToString().Trim()).ToList();
                if (stringList.Count == 0)
                {
                    return false;
                }
                string auxLacre = "";
                var aux = new Regex(@"^(.*?)(?=/|SIF)").Match(conteudo.DS_LACRE_SIF);
                if (aux.Success && aux.Groups[1].Value.Length >= 6)
                    auxLacre = aux.Groups[1].Value;
                string auxLacreSIF = "";

                string auxLacreAgencia = FindMostSimilarString(conteudo.DS_LACRE_AGENCIA, stringList);
                stringList.Remove(auxLacreAgencia);
                auxLacreSIF = stringList.FirstOrDefault(x => x.Contains(conteudo.DS_LACRE_SIF));
                stringList = stringList.Where(y => y.All(c => char.IsDigit(c) || c == 'S' || c == 'I' || c == 'F') || (System.Text.RegularExpressions.Regex.IsMatch(y, @"^A\d{6}"))).ToList();
                if (!string.IsNullOrEmpty(auxLacre) && string.IsNullOrEmpty(auxLacreSIF))
                    auxLacreSIF = stringList.FirstOrDefault(x => x.Contains(auxLacre));
                //if (string.IsNullOrEmpty(auxLacreSIF))
                //    auxLacreSIF = stringList.FirstOrDefault(x => x.Contains(Regex.Replace(conteudo.DS_LACRE_SIF, "[^0-9]", "")));
                if (string.IsNullOrEmpty(auxLacreSIF))
                    auxLacreSIF = FindMostSimilarString(conteudo.DS_LACRE_SIF.Replace("/", "").Replace("SIF", ""), stringList);

                InsereDados novoDado = new InsereDados();
                novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                novoDado.NM_TERMINAL = nmTerminal;
                novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                novoDado.NM_NAVIO = conteudo.NM_NAVIO;
                novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                novoDado.IC_TIPO = "D";
                novoDado.DT_ETA = conteudo.DT_ETA;
                novoDado.DS_LACRE_AGENCIA_TERMINAL = auxLacreAgencia ?? "";
                novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA ?? "";
                novoDado.DS_LACRE_SIF_TERMINAL = auxLacreSIF ?? "";
                novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF ?? "";

                ValidaDivergenciaLacres(novoDado, conteudo.CD_CLIENTE);
                dados.AtualizaLacreValidado(conteudo.CD_PROCESSORESERVACONTAINER);
                LimpaDados();
            }
            return result1;
        }


        private bool ValidarSituacaoLacreSantosBrasil(ListaDeCampos conteudo, Task<JavascriptResponse> task1)
        {

            //Limpa dados 
            LimpaDados();
            //Pega os dados para gravar em uma tabela teste...
            var response1 = task1.Result;
            bool result1 = response1.Success;
            string nrProcesso = "E-" + conteudo.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + conteudo.CD_NUMERO_PROCESSO.Substring(0, 2);

            if (result1)
            {
                //Pega os dados para gravar em uma tabela teste...
                string nmTerminal = "";

                if (conteudo.CD_TERMINAL_EMBARQUE == idTermSB)
                {
                    nmTerminal = "SANTOS BRASIL";
                    exibirMensagem("T", "Consultando SANTOS BRASIL");
                }              
                else if (conteudo.CD_TERMINAL_EMBARQUE == idTermVilaConde)
                {
                    nmTerminal = "VILA DO CONDE";
                    exibirMensagem("T", "Consultando Vila do Conde");
                }


                List<object> objectList = (List<object>)response1.Result;
                List<string> stringList = objectList.Select(obj => obj.ToString().Trim()).ToList();
                if (stringList.Count == 0)
                {
                    return false;
                }


                string auxLacreSIF = "";
                string auxLacreAgencia = "";
                
                auxLacreSIF = stringList[7]; 
                auxLacreAgencia = stringList[6]; 

                InsereDados novoDado = new InsereDados();
                novoDado.CD_PROCESSO = conteudo.CD_PROCESSO;
                novoDado.DT_CONTAINER = conteudo.DT_CONTAINER;
                novoDado.CD_NUMERO_PROCESSO = nrProcesso;
                novoDado.DS_REFERENCIA_CLIENTE = conteudo.DS_REFERENCIA_CLIENTE;
                novoDado.NR_CONTAINER = conteudo.NR_CONTAINER;
                novoDado.NM_TERMINAL = nmTerminal;
                novoDado.NR_BOOKING = conteudo.NR_BOOKING;
                novoDado.NM_NAVIO = conteudo.NM_NAVIO;
                novoDado.NM_PROCESSO_STATUS2 = conteudo.NM_PROCESSO_STATUS2;
                novoDado.IC_TIPO = "D";
                novoDado.DT_ETA = conteudo.DT_ETA;
                novoDado.DS_LACRE_AGENCIA_TERMINAL = auxLacreAgencia ?? "";
                novoDado.DS_LACRE_AGENCIA = conteudo.DS_LACRE_AGENCIA ?? "";
                novoDado.DS_LACRE_SIF_TERMINAL = auxLacreSIF ?? "";
                novoDado.DS_LACRE_SIF = conteudo.DS_LACRE_SIF ?? "";
               



                ValidaDivergenciaLacres(novoDado, conteudo.CD_CLIENTE);
                dados.AtualizaLacreValidado(conteudo.CD_PROCESSORESERVACONTAINER);
                LimpaDados();
            }
            return result1;
        }


        public void ValidaDivergenciaLacres(InsereDados dados, int? cdEntidade)
        {
            Dados d = new Dados();
            //if (dados.NM_TERMINAL != "BTP")
            {
                int[] cdMinerva = d.retornaCodigoMinerva();
                //if (cdMinerva.FirstOrDefault(x => x == cdEntidade) != 0)
                {

                    string part2 = "/";
                    string LacreSIFSistema = dados.DS_LACRE_SIF;
                    int sifIndex = dados.DS_LACRE_SIF.IndexOf("SIF");
                    if (sifIndex != -1 && !dados.DS_LACRE_SIF.StartsWith("SIF"))
                    {
                        // Extrai a parte antes e depois de "SIF"
                        string part1 = dados.DS_LACRE_SIF.Substring(0, sifIndex);
                        part2 = dados.DS_LACRE_SIF.Substring(sifIndex).Replace("SIF", "").Replace("/", "").Replace("-", "");

                        // Rearranja para "SIF + NÚMERO SIF + NÚMERO DO LACRE"
                        LacreSIFSistema = part1.Replace("/", "").Replace("-", "");
                    }
                    else
                    {
                        sifIndex = dados.DS_LACRE_SIF.IndexOf("/");
                        if (sifIndex != -1)
                        {
                            // Extrai a parte antes e depois de "SIF"
                            string part1 = dados.DS_LACRE_SIF.Substring(0, sifIndex);
                            part2 = dados.DS_LACRE_SIF.Substring(sifIndex).Replace("/", "").Replace("-", "");

                            // Rearranja para "SIF + NÚMERO SIF + NÚMERO DO LACRE"
                            LacreSIFSistema = part1.Replace("/", "").Replace("-", "");
                        }
                    }

                    string divergencias = "";
                    if (Regex.Replace(dados.DS_LACRE_AGENCIA, "[^0-9]", "") != Regex.Replace(dados.DS_LACRE_AGENCIA_TERMINAL, "[^0-9]", "")) // VOLTAR AQUI PARA IDENTIFICAR MELHOR
                    {
                        divergencias += "<b>Lacre agência manifestado:</b> " + dados.DS_LACRE_AGENCIA + "<br>";
                        divergencias += "<b>Lacre agência verificado:</b> " + dados.DS_LACRE_AGENCIA_TERMINAL + "<br>";
                    }
                    if (dados.DS_LACRE_SIF != "" && !((dados.DS_LACRE_SIF.Length == 6 || dados.DS_LACRE_SIF.Length == 7) && dados.DS_LACRE_SIF_TERMINAL.Contains(dados.DS_LACRE_SIF)))
                    {
                        if (LacreSIFSistema.Replace("/", "").Replace("-", "").Replace("SIF", "").Replace("SENACSA", "").TrimStart('0') != dados.DS_LACRE_SIF_TERMINAL.Replace("/", "").Replace("-", "").Replace("SIF", "").Replace("SENACSA", "").Replace(part2, "").TrimStart('0') && (part2 + LacreSIFSistema.Replace("/", "").Replace("-", "").Replace("SIF", "").Replace("SENACSA", "").TrimStart('0')) != dados.DS_LACRE_SIF_TERMINAL.Replace("/", "").Replace("-", "").Replace("SIF", "").Replace("SENACSA", "").TrimStart('0'))
                        {
                            divergencias += "<b>Lacre SIF manifestado:</b> " + dados.DS_LACRE_SIF + "<br>";
                            divergencias += "<b>Lacre SIF verificado:</b> " + dados.DS_LACRE_SIF_TERMINAL + "<br>";
                        }
                    }

                    if (divergencias != "")
                    {
                        string corpo = "<b>" + d.retornaCliente(cdEntidade) + "</b><br><b>Referência: </b>" + dados.DS_REFERENCIA_CLIENTE + "<br><b>Terminal: </b>" + dados.NM_TERMINAL + "<br><b>Booking: </b>" + dados.NR_BOOKING + "<br><b>Container: </b>" + dados.NR_CONTAINER + "<br>" + divergencias;


                        d.GeraLogLacreDivergente(dados, cdEntidade);


                        string to = "";
                        if (cdMinerva.FirstOrDefault(x => x == cdEntidade) != 0 && cdEntidade != 9620)
                            to = ConfigurationManager.AppSettings["EmailMinervaDivergenciaLacre"].ToString();
                        else
                            to = "suporte@sateldespachos.com.br";
                        Email e = new Email();
                        e.EnviaEmailDUE(to, "", "LACRE DIVERGENTE NO TERMINAL - " + dados.DS_REFERENCIA_CLIENTE + " - " + d.retornaPaisDestino(dados.CD_PROCESSO), corpo);
                    }
                }
            }
        }

        public int[] QuaisConsultas()
        {
            return ConfigurationManager.AppSettings["QuaisConsultas"].ToString().Split(';').Select(int.Parse).ToArray();
        }

        public static int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b?.Length ?? 0;
            if (string.IsNullOrEmpty(b)) return a.Length;

            int[,] distance = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                distance[i, 0] = i;

            for (int j = 0; j <= b.Length; j++)
                distance[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[a.Length, b.Length];
        }

        public static string FindMostSimilarString(string x, List<string> yList)
        {
            string mostSimilar = null;
            int minDistance = int.MaxValue;

            foreach (var y in yList)
            {
                int distance = LevenshteinDistance(x, y);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    mostSimilar = y;
                }
            }

            return mostSimilar;
        }


      


    }
}

