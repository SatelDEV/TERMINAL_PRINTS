using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalRobo.DataBase;
using System.Configuration;
using roboEDI.Model;
using System.Data;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TerminalRobo.Models
{
  
        public class ItapoaAPI {

        WEBSATELEntities db = new WEBSATELEntities();

        public class Content
        {
            public int gkey { get; set; }
            public DateTime? timeIn { get; set; }
            public DateTime? timeLoad { get; set; }
            public string container { get; set; }
            public string iso { get; set; }
            public string armador { get; set; }
            public string category { get; set; }
            public double pesoBruto { get; set; }
            public string tstate { get; set; }
            public string vesselOb { get; set; }
            public DateTime? vesselObAts { get; set; }
            public string vesselObName { get; set; }
            public string oplTri { get; set; }
            public string polTri { get; set; }
            public string podTri { get; set; }
            public string podfinalTri { get; set; }
            public string ddeDesNbr { get; set; }
            public string shipper { get; set; }
            public double tempReqdC { get; set; }
            public string bookingNbr { get; set; }
            public int actualObCv { get; set; }
            public string bussinesGroup { get; set; }
            public DateTime? evtTaxaAtracacao { get; set; }
            public string tipoDocumento { get; set; }
            public object nDocumento { get; set; }
        }

        public class Root
        {
            public List<Content> content { get; set; }
            public int number { get; set; }
            public int size { get; set; }
            public int totalElements { get; set; }
            public int totalPages { get; set; }
            public bool hasContent { get; set; }
            public int numberOfElements { get; set; }
            public bool first { get; set; }
            public bool last { get; set; }
        }


        public class ContentAmarzenado
        {
            public int gkey { get; set; }
            public string container { get; set; }
            public string posicao { get; set; }
            public string iso { get; set; }
            public object opl { get; set; }
            public string pol { get; set; }
            public string pod { get; set; }
            public double pesoBruto { get; set; }
            public string categoria { get; set; }
            public string exportador { get; set; }
            public object importador { get; set; }
            public string armador { get; set; }
            public string booking { get; set; }
            public object viagemIb { get; set; }
            public string viagemOb { get; set; }
            public string vessel { get; set; }
            public string lacreArmadorInsp { get; set; }
            public object lacreExportadorDecInsp { get; set; }
            public string lacreSifDecInsp { get; set; }
            public object lacreOutrosDecInsp { get; set; }
            public string commodityDesc { get; set; }
            public double tempRequerida { get; set; }
            public DateTime dtChegada { get; set; }
            public object dtSaida { get; set; }
            public string liberadoEmbarque { get; set; }
            public DateTime? dtLiberacaoEmbarque { get; set; }
            public string bloqueioSiscarga { get; set; }
            public object dtSiscargaLiberacao { get; set; }
            public string nreserva { get; set; }
            public object envelopeEntregue { get; set; }
            public string bussinesGroup { get; set; }
            public string importadorExportador { get; set; }
            public object bussinesGroupI { get; set; }
            public string bussinesGroupE { get; set; }
            public string ddeDseNbr { get; set; }
            public string tipoDocumento { get; set; }
            public object nDocumento { get; set; }
        }

        public class RootAmarzenado
        {
            public List<ContentAmarzenado> content { get; set; }
            public int number { get; set; }
            public int size { get; set; }
            public int totalElements { get; set; }
            public int totalPages { get; set; }
            public bool hasContent { get; set; }
            public int numberOfElements { get; set; }
            public bool first { get; set; }
            public bool last { get; set; }
        }



        public string GetBearerToken()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                string authUrl = "https://authentication-prod-k8s.portoitapoa.com/auth";

                using (HttpClient client = new HttpClient())
                {
                    var requestBody = new
                    {
                        login = "u.minervasa.paivarod",
                        senha = "SATELapi123@"
                    };

                    string json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Executa a chamada POST de forma síncrona
                    HttpResponseMessage response = client.PostAsync(authUrl, content).GetAwaiter().GetResult();

                    response.EnsureSuccessStatusCode();

                    if (response.Headers.TryGetValues("Authorization", out var authHeaderValues))
                    {
                        string bearerToken = authHeaderValues.FirstOrDefault();
                        return bearerToken;
                    }
                    else
                    {
                        return null; // Ou lançar uma exceção se preferir
                    }
                }
            }
            catch (Exception ex)
            {
                // Log do erro, se necessário
                return null;
            }
        }



        public List<ContentAmarzenado> GetAllContainerArmazenado(string bearerToken, string shipName = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                string baseUrl = "https://api-publica-prod-k8s.portoitapoa.com/api/containers/find-by-container-armazenado";
                int page = 0;
                int size = 10;

                List<ContentAmarzenado> allData = new List<ContentAmarzenado>();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));

                    bool continuar = true;

                    while (continuar)
                    {
                        // Monta URL com paginação e filtro opcional
                        string url = $"{baseUrl}?size={size}&page={page}";
                        if (!string.IsNullOrEmpty(shipName))
                            url += $"&ship={Uri.EscapeDataString(shipName)}";

                        var response = client.GetAsync(url).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();

                        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var result = JsonConvert.DeserializeObject<RootAmarzenado>(json);

                        if (result?.content != null)
                            allData.AddRange(result.content);

                        // Verifica se chegamos à última página
                        page++;
                        if (result == null || page >= result.totalPages)
                            continuar = false;
                    }
                }

                return allData;
            }
            catch (Exception ex)
            {
                // Logar erro
                Console.WriteLine("Erro ao buscar dados: " + ex.Message);
                return new List<ContentAmarzenado>();
            }
        }


        public List<Content> GetAllContainerEmbarcado(string bearerToken, string shipName = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                string baseUrl = "https://api-publica-prod-k8s.portoitapoa.com/api/containers/find-by-container-embarcado";
                int page = 0;
                int size = 10;

                List<Content> allData = new List<Content>();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));

                    bool continuar = true;

                    while (continuar)
                    {
                        // Monta URL com paginação e filtro opcional
                        string url = $"{baseUrl}?size={size}&page={page}";
                        if (!string.IsNullOrEmpty(shipName))
                            url += $"&ship={Uri.EscapeDataString(shipName)}";

                        var response = client.GetAsync(url).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();

                        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var result = JsonConvert.DeserializeObject<Root>(json);

                        if (result?.content != null)
                            allData.AddRange(result.content);

                        // Verifica se chegamos à última página
                        page++;
                        if (result == null || page >= result.totalPages)
                            continuar = false;
                    }
                }

                return allData;
            }
            catch (Exception ex)
            {
                // Logar erro
                Console.WriteLine("Erro ao buscar dados: " + ex.Message);
                return new List<Content>();
            }
        }























    }
}
