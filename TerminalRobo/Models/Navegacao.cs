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
using static TerminalRobo.Models.ItapoaAPI;
using CefSharp.OffScreen;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Policy;
using TerminalRobo.ServiceRefMinerva;
using System.Runtime.ConstrainedExecution;


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

    class Navegacao
    {
        Dados dados = new Dados();
        public CefSharp.WinForms.ChromiumWebBrowser chromeBrowser;

        PrevisaoChegada prev = new PrevisaoChegada();


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



        //Consulta Telas de Atracação


        const string siteBTP = "https://portaldocliente.btp.com.br/sistemas";
        const string siteAtracacaoEmbraport = "http://www.embraportonline.com.br/Navios/Escala";
        const string siteAtracaoTecon = "https://www.santosbrasil.com.br/v2021/lista-de-atracacao?titulo=&unidade=tecon-santos&lista=lista-de-atracacao&atracadouro=TECON";
        const string siteAtracacaoPortoNave = "https://extranet.portonave.com.br/line-up/";
        const string siteSatel = "https://www.sateldespachos.com.br/";



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

        string caminhoRaizDocs = ConfigurationManager.AppSettings["caminhoRaizDocs"].ToString();




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

            var setting = new CefSharp.WinForms.CefSettings();

            // Configurações adicionais
            setting.Locale = "pt-BR";
            setting.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            setting.CefCommandLineArgs.Add("ignore-urlfetcher-cert-requests", "1");
            setting.CefCommandLineArgs.Add("ignore-certificate-errors", "1");

            // Adiciona caminho único para diretório de cache em cada instância para evitar conflito
            string uniqueCachePath = Path.Combine(Path.GetTempPath(), "cef_cache_" + Guid.NewGuid().ToString());
            setting.CachePath = uniqueCachePath;

            // Inicializa o Cef apenas se necessário
            if (!Cef.IsInitialized.HasValue)
            {
                Cef.Initialize(setting);
            }

            // Inicializa o browser com URL desejada
            chromeBrowser = new CefSharp.WinForms.ChromiumWebBrowser("http://www.sateldespachos.com.br")
            {
                RequestHandler = new CustomRequestHandler()
            };

            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.LoadingStateChanged += chromeBrowser_LoadingStateChanged;

            JsDialogHandler jss = new JsDialogHandler();
            chromeBrowser.JsDialogHandler = jss;
        }




        public CefSharp.WinForms.ChromiumWebBrowser CarregarBrowser()
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
        


     

        public void EncerrarNavegador()
        {
            Cef.Shutdown();
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

       


        public string CriaPastaDoc(string nmPasta, int tipo)
        {
            string caminho = "";

            if (tipo == 1)
            {
                string caminhoRaiz = ConfigurationManager.AppSettings["nmCaminhoPlanilha"].ToString();

                if (caminhoRaiz != null)
                    caminho = caminhoRaiz + nmPasta;

            }
            else
            {
                string caminhoRaiz = ConfigurationManager.AppSettings["caminhoRaizDocs"].ToString();

                if (caminhoRaiz != null)
                    caminho = caminhoRaiz + nmPasta;

            }


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
            string hora = DateTime.Now.Hour.ToString();

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

            if (tipo == 2)
            {
                sCaminhoDestino = caminho + "\\" + ano + "\\" + mes + "\\" + dia + "\\" + hora;
                dirProcesso = new DirectoryInfo(sCaminhoDestino);
                if (dirProcesso.Exists == false) {              
               
                    dirProcesso.Create();
                    dirProcesso.Attributes = FileAttributes.Normal;
                }
            }



            return sCaminhoDestino;
        }

   

        public void deslogando()
        {

            exibirMensagem("T", "Deslogando");
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='https://www.sateldespachos.com.br';");
            exibirMensagem("S", "Aguardando...");
            bCarregado = false;
            AguardaPaginaCarregar();
        }

        

        //Entrar na página
        //=================================================================


        #region Prints de Tela Programação de Navios


        public bool EntrarPaginaPortoNaveTela(TextBox txtContainer)
        {
            int icontador = 0;
            bCarregado = false;
            string url = "";

            prev.GetGatePortoNave(150);

            do
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteAtracacaoPortoNave + "';");
                bCarregado = AguardaPaginaCarregar();
                url = chromeBrowser.Address;
                icontador++;
            } while (url != siteAtracacaoPortoNave && !bCarregado && icontador < 3);

            Thread.Sleep(5000);
            Application.DoEvents();

            // 👉 Clica no botão da aba "Esperados"
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(@"
        var abaEsperados = document.getElementById('tabID-tab-2');
        if(abaEsperados) { abaEsperados.click(); }
    ");

            Thread.Sleep(3000); // Dá um tempo para o conteúdo da aba carregar
            Application.DoEvents();

            // 👉 Faz o scroll para o meio da página
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(@"
        var scrollHeight = document.body.scrollHeight;
        var clientHeight = window.innerHeight;
        window.scrollTo(0, (scrollHeight - clientHeight) / 2);
    ");

            txtContainer.Text = chromeBrowser.Address;

       

            Thread.Sleep(2000);
            Application.DoEvents();


           


            int registro = dados.GravarRegistroTerminal("PORTONAVE", 186);

            prev.GetGatePortoNave(registro);



            // 👉 Agora tira o print
            return TirarPrintTela("PORTONAVE", registro);
        }



        public bool EntrarPaginaEmbraportTela(TextBox txtContainer)
        {
            int icontador = 0;
            bCarregado = false;
            string url = "";

            do
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteAtracacaoEmbraport + "';");

                bCarregado = AguardaPaginaCarregar();

                url = chromeBrowser.Address;
                icontador++;

            } while (url != siteAtracacaoEmbraport && !bCarregado && icontador < 3);


            Thread.Sleep(5000);
            Application.DoEvents();


            txtContainer.Text = chromeBrowser.Address;

            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(@"
        var scrollHeight = document.body.scrollHeight;
        var clientHeight = window.innerHeight;
        window.scrollTo(0, (scrollHeight - clientHeight) / 2);
    ");

            Thread.Sleep(2000);
            Application.DoEvents();

            int registro = dados.GravarRegistroTerminal("EMBRAPORT",206);

            return TirarPrintTela("EMBRAPORT", registro);


            
        }

        public bool EntrarPaginaTeconTela(TextBox txtContainer)
        {
            int icontador = 0;
            bCarregado = false;
            string url = "";

            do
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteAtracaoTecon + "';");

                bCarregado = AguardaPaginaCarregar();

                url = chromeBrowser.Address;
                icontador++;

            } while (url != siteAtracaoTecon && !bCarregado && icontador < 3);


            Thread.Sleep(5000);
            Application.DoEvents();


            Thread.Sleep(2000);
            Application.DoEvents();

            // 👉 CLICAR NO BOTÃO "RECEBIMENTO DE EXPORTAÇÃO"
            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(@"
        var btn = document.getElementById('btnRecebimentoExportacao');
        if(btn) { btn.click(); }
    ");
            Thread.Sleep(3000); // Dá um tempo para o conteúdo carregar após o clique
            Application.DoEvents();


            txtContainer.Text = chromeBrowser.Address;
            var scrollHeightTask = chromeBrowser.EvaluateScriptAsync(@"
        return document.body.scrollHeight;
    ");
            scrollHeightTask.Wait();

            int scrollHeight = 0;
            if (scrollHeightTask.Result.Success && scrollHeightTask.Result.Result != null)
            {
                scrollHeight = Convert.ToInt32(scrollHeightTask.Result.Result);
            }
            else
            {
                // Se não conseguir pegar a altura, define um passo fixo só pra não travar
                scrollHeight = 3000;
            }

            // Calcula o scrollStep como 30% do total
            int scrollStep = (int)(scrollHeight * 0.30);
            if (scrollStep < 100) scrollStep = 100;  // Garante um mínimo de 100px por passo

            int posicaoAtual = 0;
            bool chegouNoFinal = false;
            int numeroPrint = 1;

            int registro = dados.GravarRegistroTerminal("TECON", 188);

            prev.GetGateTecon(registro);



            while (!chegouNoFinal)
            {
                // 👉 Tirar o print
                TirarPrintTela("TECON", registro);
                numeroPrint++;

                // 👉 Faz o scroll para a próxima posição
                string scriptScroll = $@"
    (function() {{
        var maxScroll = document.body.scrollHeight - window.innerHeight;
        var novaPosicao = window.scrollY + {scrollStep};
        if(novaPosicao > maxScroll) novaPosicao = maxScroll;
        window.scrollTo(0, novaPosicao);
        return (novaPosicao >= maxScroll);
    }})()
";

                var scrollTask = chromeBrowser.EvaluateScriptAsync(scriptScroll);
                scrollTask.Wait();

                if (scrollTask.Result.Success && scrollTask.Result.Result is bool)
                {
                    chegouNoFinal = (bool)scrollTask.Result.Result;
                }
                else
                {
                    chegouNoFinal = true; // Se houver erro no script, encerra o loop
                }

                Thread.Sleep(1500);
                Application.DoEvents();
            }


            do
            {
                chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync("document.location ='" + siteSatel + "';");

                bCarregado = AguardaPaginaCarregar();

                url = chromeBrowser.Address;
                icontador++;

            } while (url != siteSatel && !bCarregado && icontador < 3);

            return true;
        }


        public bool EntrarPaginaBTPTela(TextBox txtContainer)
        {

            int icontador = 0;
            bCarregado = false;
            string url = "";

            string cookieParametro = ConfigurationManager.AppSettings["CookieLoginBTP"].ToString();


            var da = new Dados();

            string cookieValor = da.ParametroCookieBTP(int.Parse(cookieParametro));




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


            Thread.Sleep(12000);
            Application.DoEvents();


          

            if (url != siteBTP)
            {

                chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('login').value = '" + sUsuarioBTP + "';");
                chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('senha').value = '" + sSenhaBTP + "';");
                chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('entrar').click();");


                Thread.Sleep(12000);
                Application.DoEvents();
            }








            chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.querySelectorAll('a[mat-raised-button]')[2].click();");

            Thread.Sleep(5000);
            Application.DoEvents();

            chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.querySelectorAll('mat-dialog-container button.mat-button .mat-button-wrapper')[0].click();");






            Thread.Sleep(4000);
            Application.DoEvents();

            var botaoScript = @"
(function() {
    var btn = document.querySelector('#wrapper > div > btp-toolbar > mat-toolbar > div > div.full-height > button');
    if (btn) {
        btn.click();
    }
})();";

            chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(botaoScript);



            Thread.Sleep(3000);
            Application.DoEvents();

            string ConsultaSite = "";


            if (true)
            {
                chromeBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(@"
        const ContainerElement = Array.from(document.querySelectorAll('span.nav-link-title[translate=""no""]'))
            .find(span => span.textContent.trim() === 'Programação De Atracação');
        if (ContainerElement) {
            ContainerElement.click();
        }
    ");


            }


            Thread.Sleep(2000);
            Application.DoEvents();







            ConsultaSite = "https://novo-tas.btp.com.br/consultaslivres/listaatracacaoindex";


            Thread.Sleep(2000);
            Application.DoEvents();







            string x = "";
            var cookieVisitor = new CookieVisitor();

            // Obtendo o cookie
            cookieManager.VisitAllCookies(cookieVisitor);



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



            chromeBrowser.GetBrowser().MainFrame.LoadUrl("about:blank");
            AguardaPaginaCarregar();
            Thread.Sleep(5000);
            Application.DoEvents();





            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync($"document.location = '{ConsultaSite}';");


            AguardaPaginaCarregar();
            Thread.Sleep(5000);
            Application.DoEvents();


            txtContainer.Text = chromeBrowser.Address;


            chromeBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(@"
    var scrollHeight = document.body.scrollHeight;
    var clientHeight = window.innerHeight;
    var maxScroll = scrollHeight - clientHeight;
    var targetScroll = maxScroll * 0.8;  // 80% do scroll máximo (faltando 20% do final)
    window.scrollTo(0, targetScroll);
");

            Thread.Sleep(2000);
            Application.DoEvents();


            int registro = dados.GravarRegistroTerminal("BTP",210);

            return TirarPrintTela("BTP", registro); 


        }

        private bool TirarPrintTela(string nmTerminal, int cdRegistro)
        {
            try
            {
                ImpersonateUser ui = new ImpersonateUser();
                ui.Impersonate(@"SRV-SIGA", "Administrator", "Tecnoponta2008");

                string pastaDestino = CriaPastaDoc(nmTerminal, 2);
                if (!Directory.Exists(pastaDestino))
                {
                    Directory.CreateDirectory(pastaDestino);
                }

                string nomeArquivo = $"{nmTerminal}_{DateTime.Now:HH_mm_ss}.png";
                string caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

                // 📌 Captura apenas a tela 2 (índice 1 no array)
                Rectangle bounds = Screen.AllScreens[1].Bounds;
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        // Aqui usamos bounds.Location para pegar o ponto inicial correto da segunda tela
                        g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                    }

                    bitmap.Save(caminhoCompleto, System.Drawing.Imaging.ImageFormat.Png);
                }

                string arquivoDestino = caminhoCompleto.Replace("C:\\Prints", "\\\\192.168.1.250\\SatelDOCS\\Prints");
                string pastaDestinoFinal = Path.GetDirectoryName(arquivoDestino);

                if (!Directory.Exists(pastaDestinoFinal))
                {
                    Directory.CreateDirectory(pastaDestinoFinal);
                }

                if (!System.IO.File.Exists(arquivoDestino))
                {
                    if (Directory.Exists("\\\\192.168.1.250\\SatelDOCS"))
                    {
                        System.IO.File.Copy(caminhoCompleto, arquivoDestino, true);
                    }
                }

                ui.Undo();

                string caminhoArquivoParaBanco = arquivoDestino.Replace("\\\\192.168.1.250\\SatelDOCS", "G:\\SatelDOCS");
                var retorno = dados.GravarRegistroTerminalPrint(cdRegistro, nomeArquivo, caminhoArquivoParaBanco);

                return retorno;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void TirarPrintOffScreen(CefSharp.WinForms.ChromiumWebBrowser chromeBrowser, string nomeArquivoPrefixo)
        {
            try
            {
                // 1 - Captura a URL atual do navegador WinForms
                string urlAtual = chromeBrowser.Address;

                if (string.IsNullOrEmpty(urlAtual))
                {
                    MessageBox.Show("URL do navegador está vazia.");
                    return;
                }

                // 2 - Cria uma instância OffScreen
                using (var offScreenBrowser = new CefSharp.OffScreen.ChromiumWebBrowser(urlAtual))
                {
                    // 3 - Aguarda o carregamento completo (síncrono aqui por simplicidade)
                    var taskLoad = offScreenBrowser.WaitForInitialLoadAsync();
                    taskLoad.Wait();

                    if (taskLoad.Result.Success)
                    {
                        // 4 - Tira o screenshot
                        var taskScreenshot = offScreenBrowser.ScreenshotAsync();
                        taskScreenshot.Wait();

                        var screenshot = taskScreenshot.Result;
                        if (screenshot != null)
                        {
                            string pastaDestino = @"C:\Prints";
                            if (!Directory.Exists(pastaDestino))
                            {
                                Directory.CreateDirectory(pastaDestino);
                            }

                            string nomeArquivo = $"{nomeArquivoPrefixo}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                            string caminhoCompleto = CriaPastaDoc("AVISODIVERGENCIA", 2);

                            screenshot.Save(caminhoCompleto, ImageFormat.Png);
                            screenshot.Dispose();

                            MessageBox.Show($"Print salvo em: {caminhoCompleto}");
                        }
                        else
                        {
                            MessageBox.Show("Falha ao capturar o screenshot OffScreen.");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Falha ao carregar a página OffScreen. Sucesso: {taskLoad.Result.Success}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao tirar o print OffScreen: {ex.Message}");
            }
        }


       
 
       
        #endregion
    }
}
