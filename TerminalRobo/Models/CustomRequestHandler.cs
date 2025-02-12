using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;
using System.Security.Cryptography.X509Certificates;

namespace TerminalRobo.Models
{
    public class CustomRequestHandler : IRequestHandler
    {

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, IRequest callback)
        {
            // Manipulação de requisições antes de serem enviadas
            return false; // Permite a requisição
        }


        // Implementando o método OnDocumentAvailableInMainFrame
        public void OnDocumentAvailableInMainFrame(IWebBrowser browser, IBrowser browserControl)
        {
            // Este método é chamado quando o documento principal da página está disponível.
            // Você pode executar ações assim que o documento principal for carregado.
            System.Console.WriteLine("Documento principal disponível!");
        }

        // Outros métodos podem ser implementados conforme necessário
        public void OnResourceLoadComplete(IWebBrowser browser, IRequest request, IResponse response)
        {
            // Lógica de quando o recurso for completamente carregado
        }

        // Implementação do método OnAfterCreated, se necessário
        public void OnAfterCreated(IWebBrowser browser)
        {
            // Executar algo depois que o navegador for criado
        }


        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return null;
        }

        public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return false;
        }

        public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        public void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {
        }

        public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }

        public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
        }

        public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            // Verifica o URL da solicitação para decidir se deve abrir com um certificado
            if (browser.MainFrame.Url.ToUpper().Contains("BTP"))
            {
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.OpenExistingOnly);
                X509Certificate2 certificado = null;
                foreach (X509Certificate2 item in store.Certificates)
                {
                    if (item.GetNameInfo(X509NameType.SimpleName, false) == "CARLOS ALBERTO JOSE DOS SANTOS:13055644808")
                    //if (item.GetNameInfo(X509NameType.SimpleName, false) == "RICARDO BERMEJO DE MOURA:06997540899")
                    {
                        certificado = item;
                        break;
                    }
                }

                callback.Select(certificado);
                return true;
            }
            else
            {
                callback.Select(null);
                return false;
            }
        }

        public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status, int errorCode, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
