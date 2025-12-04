using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static TerminalRobo.Models.ItapoaAPI;
using static System.Net.WebRequestMethods;
using static TerminalRobo.Models.DUE;
using TerminalRobo.DataBase;
using System.Windows.Forms;
using HtmlAgilityPack;


namespace TerminalRobo.Models
{


        public class ProgramacaoGate
        {
            public int CD_PROG_NAVIO { get; set; }

            public int CD_REGISTRO { get; set; }

            public string NM_NAVIO { get; set; }

            public string NR_VIAGEM { get; set; }

            public DateTime? DT_PREVISAO_GATE { get; set; }

            public DateTime? DT_PREVISAO_GATE_DRY { get; set; }

            public DateTime? DT_PREVISAO_GATE_REEFER { get; set; }

            public DateTime? DT_GATE { get; set; }

            public DateTime? DT_GATE_DRY { get; set; }

            public DateTime? DT_GATE_REEFER { get; set; }

        }

    #region GATE TECON 

    public class Root
        {
            public bool Success { get; set; }
            public object Message { get; set; }
            public string DataTableName { get; set; }
            public string VDataRecebimento { get; set; }
            public object VAtracacao { get; set; }
            public object VAtracacaoTvc { get; set; }
            public object VAtracacaoImbituba { get; set; }
            public object VAtracacaoLogistica { get; set; }
            public List<VRecebimentoExportacao> VRecebimentoExportacao { get; set; }
            public List<VDataRecebimentoExp> VDataRecebimentoExp { get; set; }
            public object VRecebimentoExportacaoImb { get; set; }
            public object VDataRecebimentoExpImb { get; set; }
            public object VRecebimentoExportacaoTvc { get; set; }
            public object VDataRecebimentoExpTvc { get; set; }
        }

        public class VDataRecebimentoExp
        {
            public string DATA { get; set; }
        }

        public class VRecebimentoExportacao
        {
            public string ID { get; set; }
            public string BRC { get; set; }
            public string NAVIO { get; set; }
            public string VIAGEM_ARMADOR { get; set; }
            public string VIAGEM { get; set; }
            public string BERTH_WINDOWS { get; set; }
            public string DIA { get; set; }
            public string INICIO { get; set; }
            public string FIM { get; set; }
            public string DEADLINE { get; set; }
            public string PREVISAO_CHEGADA { get; set; }
            public string PREVISAO_LIBERACAO_DRY { get; set; }
            public string PREVISAO_LIBERACAO_REEFER { get; set; }
            public string LIBERACAO_DRY { get; set; }
            public string LIBERACAO_REEFER { get; set; }
        }


    public class PortoNaveItem
    {
        public string VIAGEM { get; set; }
        public string CODVIAGEM { get; set; }
        public string NOME_NAVIO { get; set; }
        public string DRY { get; set; }
        public string REEFER { get; set; }
        public LineupConfig LINEUP_CONFIG { get; set; }
    }

    public class LineupConfig
    {
        public string dataGateDry { get; set; }
        public string dataGateReefer { get; set; }
    }




    public class PrevisaoChegada
    {



        public List<ProgramacaoGate> GetGatePortoNave(int? CD_REGISTRO)
        {
            try
            {
                Dados dados = new Dados();
                List<ProgramacaoGate> programacoes = new List<ProgramacaoGate>();

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://extranet.portonave.com.br/lineup/list/esperados";

                using (HttpClient client = new HttpClient())
                {
                    var resultJson = client.GetStringAsync(url).Result;

                    // Deserializa a lista diretamente
                    var lista = JsonConvert.DeserializeObject<List<PortoNaveItem>>(resultJson);

                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {


                            programacoes.Add(new ProgramacaoGate
                            {
                                CD_REGISTRO = CD_REGISTRO ?? 0,
                                NM_NAVIO = item.NOME_NAVIO?.Trim(),

                                // CORRETO: VIAGEM = "0110E"
                                NR_VIAGEM = item.VIAGEM?.Trim(),

                                //// CORRETO: previsão (previsto) vem de LINEUP_CONFIG
                                //DT_PREVISAO_GATE_DRY = ParseDate(item.LINEUP_CONFIG?.dataGateDry),
                                //DT_PREVISAO_GATE_REEFER = ParseDate(item.LINEUP_CONFIG?.dataGateReefer),

                                // CORRETO: gate real (executado) vem de DRY/REEFER
                                DT_GATE_DRY = ParseDate(item.LINEUP_CONFIG?.dataGateDry),
                                DT_GATE_REEFER = ParseDate(item.LINEUP_CONFIG?.dataGateReefer),
                            });                           
                        }
                    }
                }

                dados.GravaProgramacaoGate(programacoes);
                return programacoes;
            }
            catch (Exception ex)
            {
                // e.EnviaEmailErro(emailErro, "ERRO AO CONSULTAR PORTONAVE", ex);
                return new List<ProgramacaoGate>();
            }
        }






        public List<ProgramacaoGate> GetGateTecon(int? CD_REGISTRO)
        {
            try
            {
                Dados dados = new Dados();

                List<ProgramacaoGate> programacoes = new List<ProgramacaoGate>();

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://www.santosbrasil.com.br/v2021/lista-de-atracacao/pesquisa?unidade=tecon-santos&lista=recebimento-de-exportacao";

                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string resultJson = response.Content.ReadAsStringAsync().Result;

                    Root listRoot = JsonConvert.DeserializeObject<Root>(resultJson);

                    if (listRoot?.VRecebimentoExportacao != null)
                    {
                        foreach (var item in listRoot.VRecebimentoExportacao)
                        {
                            programacoes.Add(new ProgramacaoGate
                            {
                                CD_REGISTRO = CD_REGISTRO ?? 0,
                                NM_NAVIO = item.NAVIO?.Trim(),
                                NR_VIAGEM = item.VIAGEM_ARMADOR?.Split(' ')[0].Trim(),                               

                                DT_PREVISAO_GATE_DRY = ParseDate(item.PREVISAO_LIBERACAO_DRY),
                                DT_PREVISAO_GATE_REEFER = ParseDate(item.PREVISAO_LIBERACAO_REEFER),

                                DT_GATE_DRY = ParseDate(item.LIBERACAO_DRY),
                                DT_GATE_REEFER = ParseDate(item.LIBERACAO_REEFER)
                            });
                        }
                    }
                }

                dados.GravaProgramacaoGate(programacoes);
                


                return programacoes;
            }
            catch (Exception ex)
            {
                // e.EnviaEmailErro(emailErro, "ERRO AO CONSULTAR SANTOS BRASIL", ex);
                return new List<ProgramacaoGate>();
            }
        }

        private DateTime? ParseDate(string input)
        {
            if (DateTime.TryParse(input, out DateTime parsedDate))
                return parsedDate;
            return null;
        }





        #endregion 







    }


}
