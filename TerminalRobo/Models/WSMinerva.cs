using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using TerminalRobo.DataBase;

namespace TerminalRobo.Models
{
    public class WSMinerva : Dados
    {
        WEBSATELEntities db = new WEBSATELEntities();
        DUE due = new DUE();
        //Email email = new Email();
        List<string> anexo = new List<string>();
        

        public class Retorno
        {
            public bool status { get; set; }
            public string msgErro { get; set; }
        }

        public class camposAviso
        {
            public string nrProcesso { get; set; }
            public string nrOrdem { get; set; }
            public string Mensagem { get; set; }
            public string nmUsuario { get; set; }
            public int idAviso { get; set; }
            public DateTime? dtAviso { get; set; }
            public int? idUsuario { get; set; }
            public int? idProcesso { get; set; }
        }

        public class dadosContainer
        {
            public DateTime? DT_EMBARQUE { get; set; }
            public DateTime? DT_TERMINAL_EMBARQUE { get; set; }
            public DateTime? DT_TERMINAL_REDEX { get; set; }
            public string NR_CSI { get; set; }
            public string NR_ORDEM { get; set; }
        }


        public void geraLogEnvioMinerva(int idProcesso, string sRef, string nrChave, string nrDUE, string nrRUC, DateTime? dtRegistro, DateTime? dtDesembaraco,
            DateTime? dtEmbarque, string nrBL, string nrLpco, string dsTerminalRedex, string dsTerminalEmbarque, DateTime? dtTerminalRedex, DateTime? dtTerminalEmbarque, DateTime? dtSaidaNavio, DateTime? dtEnvioDraft,
            DateTime? dtChegadaCSI, DateTime? dtEnvioDocs, string dsEnvioDocs, DateTime? dtEnvioCourrier, DateTime? dtAverbado, string nrCourrier, string nmCourrier, string nrCSI, DateTime? DtLiberacaoBL)
        {
            LOG_ENVIO_MINERVA logenvio = new LOG_ENVIO_MINERVA();




            logenvio.CD_PROCESSO = idProcesso;
            logenvio.DT_LOG = DateTime.Now;
            logenvio.NR_ORDEM = sRef;
            logenvio.NR_CHAVE_ACESSO = nrChave;
            logenvio.NR_DUE = nrDUE;
            logenvio.NR_RUC = nrRUC;
            logenvio.DT_REGISTRO = dtRegistro;
            logenvio.DT_DESEMBARACO = dtDesembaraco;
            logenvio.DT_EMBARQUE = dtEmbarque;
            logenvio.NR_BL = nrBL;
            logenvio.NR_LPCO = nrLpco;
            logenvio.DT_TERMINAL_EMBARQUE = dtTerminalEmbarque;
            logenvio.DT_TERMINAL_REDEX = dtTerminalRedex;
            logenvio.DS_TERMINAL_EMBARQUE = dsTerminalEmbarque;
            logenvio.DS_TERMINAL_REDEX = dsTerminalRedex;
            logenvio.DT_SAIDA_NAVIO = dtSaidaNavio;
            logenvio.DS_SISTEMA = "ROBOTERMINAL";
            logenvio.SG_STATUS = null;
            logenvio.DT_CHEGADA_CSI = dtChegadaCSI;
            logenvio.DT_ENVIO_DRAFT = dtEnvioDraft;
            logenvio.DT_ENVIO_DOCS = dtEnvioDocs;
            logenvio.DS_ENVIO_DOCS = dsEnvioDocs;
            logenvio.DT_ENVIO_COURRIER = dtEnvioCourrier;
            logenvio.NR_COURRIER = nrCourrier;
            logenvio.NM_COURRIER = nmCourrier;
            logenvio.DT_AVERBADO = dtAverbado;
            logenvio.MSG_ERRO = null;
            logenvio.NR_CSI = nrCSI;
            logenvio.DT_BL_REAL = DtLiberacaoBL;

            //logenvio.NR_CSI = nrCSI;

            db.LOG_ENVIO_MINERVA.Add(logenvio);
            db.SaveChanges();


        }

        public dadosContainer RetornaDadosContainer(int idProcessoReserva, string nrOrdemCliente, int nrOrdemContainer)
        {

            //tenta localizar o container pela ordem do Minerva
            var lstContainer = (from pr in db.PROCESSORESERVA
                                join prc in db.PROCESSORESERVACONTAINER on pr.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                                from prcOri in prcLeft.DefaultIfEmpty()

                                where pr.CD_PROCESSORESERVA == idProcessoReserva && prcOri.NR_ORDDEM_CLIENTE == nrOrdemCliente
                                select new dadosContainer
                                {
                                    DT_EMBARQUE = prcOri.DT_EMBARQUE,
                                    DT_TERMINAL_EMBARQUE = prcOri.DT_DEPOSITO_TERM_EMBARQUE,
                                    DT_TERMINAL_REDEX = prcOri.DT_REDEX,
                                    NR_CSI = prcOri.NR_CSI,
                                    NR_ORDEM = prcOri.NR_ORDDEM_CLIENTE
                                }).FirstOrDefault();

            if (lstContainer == null)
            {
                //Senão encontrar então busca pela ordem dos containers
                var lstContainer2 = (from pr in db.PROCESSORESERVA
                                     join prc in db.PROCESSORESERVACONTAINER on pr.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                                     from prcOri in prcLeft.DefaultIfEmpty()

                                     where pr.CD_PROCESSORESERVA == idProcessoReserva && prcOri.NR_ORDEM_CONTAINER == nrOrdemContainer
                                     select new dadosContainer
                                     {
                                         DT_EMBARQUE = prcOri.DT_EMBARQUE,
                                         DT_TERMINAL_EMBARQUE = prcOri.DT_DEPOSITO_TERM_EMBARQUE,
                                         DT_TERMINAL_REDEX = prcOri.DT_REDEX,
                                         NR_CSI = prcOri.NR_CSI,
                                         NR_ORDEM = prcOri.NR_ORDDEM_CLIENTE
                                     }).FirstOrDefault();
                if (lstContainer2 != null)
                {
                    //Se encotrou verifica se existe o número da ordem e se a mesma é igual
                    if (lstContainer2.NR_ORDEM == null || lstContainer2.NR_ORDEM == "")
                    {
                        return lstContainer2;
                    }
                    else
                    {
                        if (lstContainer2.NR_ORDEM == nrOrdemCliente)
                            return lstContainer2;
                        else
                            return null;
                    }
                }
                return null;
            }
            else
                return lstContainer;
        }

        public DUE.camposDUE RetornaDadosProcessoRevenda(int idProcesso, int idReserva, string nrContainer)
        {
            var lstDUE = (from p in db.PROCESSOS
                          join e in db.ENTIDADE on p.CD_CLIENTE equals e.CD_ENTIDADE
                          join due in db.DUE on p.CD_PROCESSO equals due.CD_PROCESSO into dueLeft
                          from dueOri in dueLeft.DefaultIfEmpty()
                          join pr in db.PROCESSORESERVA on p.CD_PROCESSO equals pr.CD_PROCESSO
                          join r in db.RESERVAS on pr.CD_RESERVA equals r.CD_RESERVA
                          join prc in db.PROCESSORESERVACONTAINER on pr.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA
                          join c in db.CONTAINER on prc.CD_CONTAINER equals c.CD_CONTAINER
                          join va in db.VIAGEMARMADOR on r.CD_VIAGEM_ARMADOR equals va.CD_VIAGEM_ARMADOR
                          join termRedex in db.ENTIDADE on prc.CD_TERMINAL_REDEX == null ? 0 : prc.CD_TERMINAL_REDEX equals termRedex.CD_ENTIDADE into termRedexLeft
                          from termRedex in termRedexLeft.DefaultIfEmpty()
                          join termEmb in db.ENTIDADE on va.CD_TERMINAL_ATRACACAO == null ? 0 : va.CD_TERMINAL_ATRACACAO equals termEmb.CD_ENTIDADE into termEmbLeft
                          from termEmbOri in termEmbLeft.DefaultIfEmpty()

                          where r.CD_RESERVA == idReserva && c.CD_NUMERO_CONTAINER == nrContainer && p.CD_PROCESSO != idProcesso
                          select new DUE.camposDUE
                          {
                              CD_DUE = dueOri.CD_DUE,
                              CD_PROCESSO = p.CD_PROCESSO,
                              CD_PROCESSORESERVA = pr.CD_PROCESSORESERVA,
                              //CD_GRUPO_CLI = g.CD_GRUPOCLI,
                              NR_PROCESSO = p.CD_NUMERO_PROCESSO != null ? "E-" + p.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + p.CD_NUMERO_PROCESSO.Substring(0, 2) : "",
                              NR_PROCESSO_LOTE = FN_RETORNA_PROCESSO_LOTE_DUE(dueOri.CD_DUE),
                              SG_TIPO_DUE = dueOri.SG_TIPO_DUE,
                              DS_REFERENCIA = p.DS_REFERENCIA_CLIENTE,
                              CD_NUMERO_DUE = dueOri.CD_NUMERO_DUE,
                              CD_NUMERO_RUC = dueOri.CD_NUMERO_RUC,
                              NR_CONTAINER = c.CD_NUMERO_CONTAINER,
                              DT_DUE = dueOri.DT_DUE,
                              DT_TRANSMISSAO = dueOri.DT_TRANSMISSAO,
                              CD_USUARIO_DUE = dueOri.CD_USUARIO_DUE,
                              CD_USUARIO_TRANSMISSAO = dueOri.CD_USUARIO_TRANSMISSAO,
                              DT_DESEMBARACO = dueOri.DT_DESEMBARACO,
                              //NR_CONTAINER = p.CD_PROCESSO != null ? FN_RETORNA_CONTAINER_PROCESSO_DUE(p.CD_PROCESSO) : "",

                              CD_RECINTO_DESPACHO = dueOri.CD_RECINTO_DESPACHO,
                              IC_RECINTO_ADUANEIRO_DESPACHO = dueOri.IC_RECINTO_ADUANEIRO_DESPACHO,

                              STR_CODIGOUNIDADERF_EMBARQUE = dueOri.STR_CODIGOUNIDADERF_EMBARQUE,
                              CD_RECINTO_EMBARQUE = dueOri.CD_RECINTO_EMBARQUE,

                              IC_RECINTO_ADUANEIRO_EMBARQUE = dueOri.IC_RECINTO_ADUANEIRO_EMBARQUE,
                              IC_EMBUTIDO = dueOri.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                              DS_REFERENCIA_ENDERECO = dueOri.DS_REFERENCIA_ENDERECO,
                              DS_INFORMACOES_COMPLEMENTARES = dueOri.DS_INFORMACOES_COMPLEMENTARES,
                              CD_VIA_TRANSPORTE_ESPECIAL = dueOri.CD_VIA_TRANSPORTE_ESPECIAL,
                              VL_FRETE_INTERNACIONAL = dueOri.VL_FRETE_INTERNACIONAL,

                              VL_DESCONTO = dueOri.VL_DESCONTO,
                              IC_STATUS_PARA_ENVIO_TRANSMISSAO = dueOri.IC_STATUS_PARA_ENVIO_TRANSMISSAO,
                              SG_FRETE_INTERNACIONAL_EMBUTIDO = dueOri.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                              SG_SEGURO_INTERNACIONAL_EMBUTIDO = dueOri.SG_SEGURO_INTERNACIONAL_EMBUTIDO,
                              VL_SEGURO = dueOri.VL_SEGURO,
                              DS_ARQUIVO_XML = dueOri.DS_ARQUIVO_XML,
                              //DT_CANCELADO = dueOri.DT_CANCELADO,
                              IC_BLOQUEIO = dueOri.IC_BLOQUEIO,
                              IC_ADM = dueOri.IC_CONTROLE_ADM,
                              IC_SITUACAO_CARGA = dueOri.IC_SITUACAO_CARGA,
                              DT_ENTREGA_DESPACHO = dueOri.DT_ENTREGA_DESPACHO,
                              DT_CRIACAO_BL = (from ev in db.PROCESSOEVENTO where p.CD_PROCESSO == ev.CD_PROCESSO && (ev.CD_EVENTO == 24 || ev.CD_EVENTO == 25 || ev.CD_EVENTO == 26 || ev.CD_EVENTO == 46) select ev.DT_PROCESSOEVENTO).FirstOrDefault(),
                              DT_CANCELADO = (from ev in db.PROCESSOEVENTO where p.CD_PROCESSO == ev.CD_PROCESSO && (ev.CD_EVENTO == 51 || ev.CD_EVENTO == 23) select ev.DT_PROCESSOEVENTO).FirstOrDefault(),
                              CD_CHAVE_ACESSO = dueOri.CD_CHAVE_ACESSO,
                              DT_AVERBADO = dueOri.DT_ABERBADO,

                              DT_EMBARQUE = prc.DT_EMBARQUE,
                              DT_CANAL_VERMELHO = dueOri.DT_CANAL_VERMELHO,
                              SG_TIPO_DUE_ANT = dueOri.SG_TIPO_DUE_ANT,
                              CD_MOTIVO_DISPENSA_NF = dueOri.CD_MOTIVO_DISPENSA_NF,
                              STR_CODIGOVIATRANSPORTE = p.STR_CODIGOVIATRANSPORTE,
                              DT_SAIDA_NAVIO = va.DT_DESATRACACAO,
                              NM_TERMINAL_REDEX = termRedex.NM_FANTASIA_ENTIDADE,
                              DS_TERMINAL_ATRACACAO = termEmbOri.NM_FANTASIA_MINERVA,
                              DT_TERMINAL_REDEX = prc.DT_REDEX,
                              DT_TERMINAL_ATRACACAO = prc.DT_DEPOSITO_TERM_EMBARQUE,

                              DT_CHEGADA_CSI = prc.DT_CHEGADA_CSI,
                              DT_ENVIO_DOCS = p.DT_ENVIO_DOCS,
                              DS_ENVIO_DOCS = p.DS_ENVIO_DOCUMENTO,

                              DT_ENVIO_COURRIER = p.DT_ENVIO_COURIER,
                              DS_COURRIER = p.DS_COUREIR,
                              NR_COURRIER = p.NR_RASTREIO,
                              NR_BL = p.CD_NUMERO_BL_REAL,
                              NR_CSI = prc.NR_CSI,
                              DT_BL_REAL = p.DT_ORIGINAL_BL
                          });
            //string sql = lstDUE.ToString();
            return lstDUE.FirstOrDefault();
        }


        public DUE.camposDUE RetornaDadosProcesso(int idProcesso)
        {
            var lstDUE = (from p in db.PROCESSOS
                          join e in db.ENTIDADE on p.CD_CLIENTE equals e.CD_ENTIDADE
                          join ge in db.GRUPOCLI_ENTIDADE on e.CD_ENTIDADE equals ge.CD_ENTIDADE
                          join g in db.GRUPOCLI on ge.CD_GRUPOCLI equals g.CD_GRUPOCLI
                          join due in db.DUE on p.CD_PROCESSO equals due.CD_PROCESSO into dueLeft
                          from dueOri in dueLeft.DefaultIfEmpty()
                          join pr in db.PROCESSORESERVA on p.CD_PROCESSO equals pr.CD_PROCESSO into prLeft
                          from prOri in prLeft.DefaultIfEmpty()
                          join prc in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                          from prcOri in prcLeft.DefaultIfEmpty()
                          join c in db.CONTAINER on prcOri.CD_CONTAINER equals c.CD_CONTAINER into cLeft
                          from cOri in cLeft.DefaultIfEmpty()
                          join r in db.RESERVAS on prOri.CD_RESERVA equals r.CD_RESERVA into rLeft
                          from rOri in rLeft.DefaultIfEmpty()
                          join va in db.VIAGEMARMADOR on rOri.CD_VIAGEM_ARMADOR equals va.CD_VIAGEM_ARMADOR into vaLeft
                          from vaOri in vaLeft.DefaultIfEmpty()
                          join termRedex in db.ENTIDADE on prcOri.CD_TERMINAL_REDEX == null ? 0 : prcOri.CD_TERMINAL_REDEX equals termRedex.CD_ENTIDADE into termRedexLeft
                          from termRedex in termRedexLeft.DefaultIfEmpty()
                          join termEmb in db.ENTIDADE on vaOri.CD_TERMINAL_ATRACACAO == null ? 0 : vaOri.CD_TERMINAL_ATRACACAO equals termEmb.CD_ENTIDADE into termEmbLeft
                          from termEmbOri in termEmbLeft.DefaultIfEmpty()

                          where p.CD_PROCESSO == idProcesso && (g.SG_ANALISTA == false || g.SG_ANALISTA == null)
                          select new DUE.camposDUE
                          {
                              CD_DUE = dueOri.CD_DUE,
                              CD_PROCESSO = p.CD_PROCESSO,
                              CD_PROCESSORESERVA = prOri.CD_PROCESSORESERVA,
                              CD_RESERVA = rOri.CD_RESERVA,
                              CD_GRUPO_CLI = g.CD_GRUPOCLI,
                              NR_PROCESSO = p.CD_NUMERO_PROCESSO != null ? "E-" + p.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + p.CD_NUMERO_PROCESSO.Substring(0, 2) : "",
                              NR_PROCESSO_LOTE = FN_RETORNA_PROCESSO_LOTE_DUE(dueOri.CD_DUE),
                              SG_TIPO_DUE = dueOri.SG_TIPO_DUE,
                              DS_REFERENCIA = p.DS_REFERENCIA_CLIENTE,
                              CD_NUMERO_DUE = FN_RETORNA_DUE_PROCESSO(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER),
                              CD_NUMERO_RUC = FN_RETORNA_RUC_PROCESSO(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER),
                              NR_CONTAINER = cOri.CD_NUMERO_CONTAINER,
                              //CD_NUMERO_DUE = dueOri.CD_NUMERO_DUE,
                              //CD_NUMERO_RUC = dueOri.CD_NUMERO_RUC,
                              DT_DUE = dueOri.DT_DUE,
                              DT_TRANSMISSAO = dueOri.DT_TRANSMISSAO,
                              CD_USUARIO_DUE = dueOri.CD_USUARIO_DUE,
                              CD_USUARIO_TRANSMISSAO = dueOri.CD_USUARIO_TRANSMISSAO,
                              DT_DESEMBARACO = FN_RETORNA_DUE_PROCESSO_DATAS(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER, 2),
                              //NR_CONTAINER = p.CD_PROCESSO != null ? FN_RETORNA_CONTAINER_PROCESSO_DUE(p.CD_PROCESSO) : "",

                              CD_RECINTO_DESPACHO = dueOri.CD_RECINTO_DESPACHO,
                              IC_RECINTO_ADUANEIRO_DESPACHO = dueOri.IC_RECINTO_ADUANEIRO_DESPACHO,

                              STR_CODIGOUNIDADERF_EMBARQUE = dueOri.STR_CODIGOUNIDADERF_EMBARQUE,
                              CD_RECINTO_EMBARQUE = dueOri.CD_RECINTO_EMBARQUE,

                              IC_RECINTO_ADUANEIRO_EMBARQUE = dueOri.IC_RECINTO_ADUANEIRO_EMBARQUE,
                              IC_EMBUTIDO = dueOri.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                              DS_REFERENCIA_ENDERECO = dueOri.DS_REFERENCIA_ENDERECO,
                              DS_INFORMACOES_COMPLEMENTARES = dueOri.DS_INFORMACOES_COMPLEMENTARES,
                              CD_VIA_TRANSPORTE_ESPECIAL = dueOri.CD_VIA_TRANSPORTE_ESPECIAL,
                              VL_FRETE_INTERNACIONAL = dueOri.VL_FRETE_INTERNACIONAL,

                              VL_DESCONTO = dueOri.VL_DESCONTO,
                              IC_STATUS_PARA_ENVIO_TRANSMISSAO = dueOri.IC_STATUS_PARA_ENVIO_TRANSMISSAO,
                              SG_FRETE_INTERNACIONAL_EMBUTIDO = dueOri.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                              SG_SEGURO_INTERNACIONAL_EMBUTIDO = dueOri.SG_SEGURO_INTERNACIONAL_EMBUTIDO,
                              VL_SEGURO = dueOri.VL_SEGURO,
                              DS_ARQUIVO_XML = dueOri.DS_ARQUIVO_XML,
                              //DT_CANCELADO = dueOri.DT_CANCELADO,
                              IC_BLOQUEIO = dueOri.IC_BLOQUEIO,
                              IC_ADM = dueOri.IC_CONTROLE_ADM,
                              IC_SITUACAO_CARGA = dueOri.IC_SITUACAO_CARGA,
                              DT_ENTREGA_DESPACHO = FN_RETORNA_DUE_PROCESSO_DATAS(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER, 3),
                              DT_CRIACAO_BL = (from ev in db.PROCESSOEVENTO where p.CD_PROCESSO == ev.CD_PROCESSO && (ev.CD_EVENTO == 24 || ev.CD_EVENTO == 25 || ev.CD_EVENTO == 26 || ev.CD_EVENTO == 46) select ev.DT_PROCESSOEVENTO).FirstOrDefault(),
                              DT_CANCELADO = (from ev in db.PROCESSOEVENTO where p.CD_PROCESSO == ev.CD_PROCESSO && (ev.CD_EVENTO == 51 || ev.CD_EVENTO == 23) select ev.DT_PROCESSOEVENTO).FirstOrDefault(),
                              CD_CHAVE_ACESSO = FN_RETORNA_DUE__CHAVE_ACESSO_PROCESSO(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER),
                              //DT_AVERBADO = due.DT_ABERBADO == null ? (from his in db.DUE_SITUACAO_HISTORICO where due.CD_DUE == his.CD_DUE && his.CD_SITUACAO == 70 select new { his.DT_SITUACAO }).FirstOrDefault().DT_SITUACAO : due.DT_ABERBADO
                              DT_AVERBADO = FN_RETORNA_DUE_PROCESSO_DATAS(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER, 4),
                              DT_EMBARQUE = (from prcx in db.PROCESSORESERVACONTAINER where prOri.CD_PROCESSORESERVA == prcx.CD_PROCESSORESERVA select new { DT_EMBARQUE = prcx.DT_EMBARQUE }).OrderBy(x => x.DT_EMBARQUE).FirstOrDefault().DT_EMBARQUE,
                              //DT_DEPOSITADO = (from prcx in db.PROCESSORESERVACONTAINER where prOri.CD_PROCESSORESERVA == prcx.CD_PROCESSORESERVA select new { DT_DEP = prcx.DT_REDEX != null ? prcx.DT_REDEX : prcx.DT_DEPOSITO_TERM_EMBARQUE }).OrderBy(p => p.DT_DEP).FirstOrDefault().DT_DEP,
                              //NM_TERMINAL_REDEX = prcOri.DT_REDEX != null && prcOri.DT_DEPOSITO_TERM_EMBARQUE == null ? (from termR in db.ENTIDADE where prcOri.CD_TERMINAL_REDEX == termR.CD_ENTIDADE select new { termR.NM_FANTASIA_ENTIDADE }).FirstOrDefault().NM_FANTASIA_ENTIDADE : null,
                              DT_CANAL_VERMELHO = FN_RETORNA_DUE_PROCESSO_DATAS(p.CD_PROCESSO, prcOri.CD_PROCESSORESERVACONTAINER, 1),
                              SG_TIPO_DUE_ANT = dueOri.SG_TIPO_DUE_ANT,
                              CD_MOTIVO_DISPENSA_NF = dueOri.CD_MOTIVO_DISPENSA_NF,
                              STR_CODIGOVIATRANSPORTE = p.STR_CODIGOVIATRANSPORTE,
                              DT_SAIDA_NAVIO = vaOri.DT_DESATRACACAO,
                              NM_TERMINAL_REDEX = termRedex.NM_FANTASIA_MINERVA == null ? termRedex.NM_FANTASIA_ENTIDADE : termRedex.NM_FANTASIA_MINERVA,
                              DS_TERMINAL_ATRACACAO = termEmbOri.NM_FANTASIA_MINERVA,
                              DT_TERMINAL_REDEX = prcOri.DT_REDEX,
                              DT_TERMINAL_ATRACACAO = prcOri.DT_DEPOSITO_TERM_EMBARQUE,

                              DT_CHEGADA_CSI = prcOri.DT_CHEGADA_CSI,
                              DT_ENVIO_DOCS = p.DT_ENVIO_DOCS,
                              DS_ENVIO_DOCS = p.DS_ENVIO_DOCUMENTO,

                              DT_ENVIO_COURRIER = p.DT_ENVIO_COURIER,
                              DS_COURRIER = p.DS_COUREIR,
                              NR_COURRIER = p.NR_RASTREIO,
                              NR_BL = p.CD_NUMERO_BL_REAL,
                              NR_CSI = FN_RETORNA_CSI_PROCESSO(prOri.CD_PROCESSORESERVA),
                              DT_BL_REAL = p.DT_ORIGINAL_BL
                          });
            return lstDUE.FirstOrDefault();
        }
        public class minerva
        {
            public int CD_PROCESSO { get; set; }
        }
        /*
        public List<minerva> ReenviarDadosMinerva()
        {
            /*
            DateTime dtInicio = Convert.ToDateTime("01/01/2020");
            DateTime dtFim = Convert.ToDateTime("13/03/2020 23:59:59");
            var reenvia = db.SP_TMP_RETORNA_BL_MINERVA(dtInicio, dtFim).ToList();
            */
            //Pegar todos as ordens lote
            /*
            List<minerva> conteudoMinerva = new List<minerva>();
            var tmpMinerva = (from tmp in db.TMP_MINERVA3
                              select new { tmp.NR_ORDEM }).ToList();

            foreach (var conteudo in tmpMinerva)
            {
                var reenvia = (from p in db.PROCESSOS
                               join e in db.PROCESSOEVENTO on p.CD_PROCESSO equals e.CD_PROCESSO
                               where p.DS_REFERENCIA_CLIENTE.Contains(conteudo.NR_ORDEM) && p.DT_CANCELAMENTO == null && e.CD_EVENTO != 51
                               select new minerva { CD_PROCESSO = p.CD_PROCESSO }).Distinct().ToList();
                foreach (var conteudo2 in reenvia)
                {
                    minerva item = new minerva();
                    item.CD_PROCESSO = conteudo2.CD_PROCESSO;
                    conteudoMinerva.Add(item);

                }
            }
             /

            var reenvia = (from p in db.PROCESSOS
                           join tmp in db.TMP_MINERVA2 on p.DS_REFERENCIA_CLIENTE equals tmp.NR_ORDEM
                           select new minerva { CD_PROCESSO = p.CD_PROCESSO }).ToList();


            return reenvia;

        }*/


        public Retorno EnviaDados(int idProcesso, int idProcessoReserva, int idReserva, string nrReferencia, string nrChave, string nrDUE, string nrRUC, DateTime? dtRegistro, DateTime? dtDesembaraco,
            DateTime? dtEmbarque, string nrBL, string nrLpco, string dsTerminalRedex, string dsTerminalEmbarque, DateTime? dtRedex, DateTime? dtTerminalEmbarque,
            DateTime? dtSaidaNavio, DateTime? dtEnvioDraft, DateTime? dtCSI, DateTime? dtEnvioDocs, string dsEnvioDocs, DateTime? dtEnvioCourrier, DateTime? dtAverbado, string nrCourrier, string nmCourrier, string nrCSI, string nrContainer, DateTime? DtLiberacaoBL)
        {
            Retorno ret = new Retorno();

            string sRef = nrReferencia;
            string sReferencia = "";
            string sCSI = "";
            try
            {
                //Remove espaços no inicio e no fim do texto
                sRef = sRef.TrimStart(' ').TrimEnd(' ');
                string sRefAux = "";
                string[] aRef;
                if (!sRef.Contains(" "))
                {
                    aRef = sRef.Split('-');

                    if (aRef.Length > 1)
                    {
                        sRefAux = aRef[0];
                        string sRefAux2 = sRefAux;
                        for (int n = 1; n < aRef.Length; n++)
                        {
                            if (n == 1)
                                sRefAux = sRefAux + "-" + aRef[n] + ",";
                            else
                                sRefAux = sRefAux + sRefAux2 + "-" + aRef[n] + ",";

                        }
                        sRefAux = sRefAux.Substring(0, sRefAux.Length - 1);
                    }
                    else
                    {
                        //SE EXISTIR ESPAÇO NA REFERENCIA TROCA POR ;
                        aRef = sRef.Split(' ');

                        if (aRef.Length > 1)
                        {
                            string sRefAux2 = "";
                            for (int n = 0; n < aRef.Length; n++)
                            {

                                sRefAux = aRef[n];

                                if (!string.IsNullOrEmpty(sRefAux))
                                {
                                    sRefAux2 = sRefAux2 + sRefAux + ",";
                                }

                            }
                            sRefAux = sRefAux2.Substring(0, sRefAux2.Length - 1);
                        }
                        else
                        {
                            if (aRef.Length == 1)
                                sRefAux = aRef[0];
                        }

                    }

                }
                else
                {
                    aRef = sRef.Split(' ');
                    string[] aRef2;
                    string sRefAuxN = "";
                    if (aRef.Length > 1)
                    {
                        string sRefAux2 = "";

                        for (int n = 0; n < aRef.Length; n++)
                        {

                            sRefAux = aRef[n];

                            //======================
                            aRef2 = aRef[n].Split('-');

                            if (aRef2.Length > 1)
                            {

                                sRefAux = aRef2[0];
                                string sRefAux3 = sRefAux;
                                for (int x = 1; x < aRef2.Length; x++)
                                {
                                    if (x == 1)
                                        sRefAux = sRefAux + "-" + aRef2[x] + ",";
                                    else
                                        sRefAux = sRefAux + sRefAux3 + "-" + aRef2[x] + ",";

                                }
                                if (n == 0)
                                    sRefAuxN += sRefAux.Substring(0, sRefAux.Length - 1);
                                else
                                    sRefAuxN += "," + sRefAux.Substring(0, sRefAux.Length - 1);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(sRefAux))
                                {
                                    sRefAux2 = sRefAux2 + sRefAux + ",";
                                }
                                sRefAux = sRefAux2.Substring(0, sRefAux2.Length - 1);
                            }
                            //========================================================

                        }
                        if (sRefAuxN != "")
                            sRefAux = sRefAuxN;

                    }
                    else
                    {
                        if (aRef.Length == 1)
                            sRefAux = aRef[0];
                    }
                }


                ServiceRefMinerva.Ordem ordem = new ServiceRefMinerva.Ordem();
                ServiceRefMinerva.Autenticacao autentica = new ServiceRefMinerva.Autenticacao();
                ServiceRefMinerva.RetornoPadrao retorno = new ServiceRefMinerva.RetornoPadrao();
                
                ordem.CodEmpresa = 0;

                autentica.senha = "9CPADMWsf2Hp";
                autentica.usuario = "Satel";
                //ordem.

                if (nrCSI != null)
                    nrCSI = nrCSI.Replace(";", ",");
                else
                    nrCSI = "";

                string[] aReferencia = sRefAux.Split(',');

                string[] aCSI = nrCSI.Split(',');
                bool bCSI;

                for (int i = 0; i < aReferencia.Length; i++)
                {

                    bCSI = false;
                    sReferencia = aReferencia[i];
                    //Recuperar dados do container referente a ordem em questão
                    if (aReferencia.Length > 1)
                    {

                        dadosContainer _dadosContainer = RetornaDadosContainer(idProcessoReserva, sReferencia, i + 1);

                        if (_dadosContainer != null)
                        {
                            dtTerminalEmbarque = _dadosContainer.DT_TERMINAL_EMBARQUE;
                            dtRedex = _dadosContainer.DT_TERMINAL_REDEX;
                            dtEmbarque = _dadosContainer.DT_EMBARQUE;
                            if (_dadosContainer.NR_CSI != "")
                            {
                                nrCSI = _dadosContainer.NR_CSI;
                                bCSI = true;
                            }

                        }
                    }
                    else
                    {
                        //SE O PROCESSO NÃO TEM DUE VERIFICA SE EXISTE UM PROCESSO ANTERIOR CARACTERIZANDO UMA REVENDA
                        if (nrDUE == null || nrDUE == "")
                        {

                            DUE.camposDUE dadosRevenda = RetornaDadosProcessoRevenda(idProcesso, idReserva, nrContainer);
                            if (dadosRevenda != null)
                            {


                                nrChave = dadosRevenda.NR_CHAVE_NF;
                                dtRegistro = dadosRevenda.DT_TRANSMISSAO;
                                nrDUE = dadosRevenda.CD_NUMERO_DUE;
                                nrRUC = dadosRevenda.CD_NUMERO_RUC;
                                //ordem.Ordens = sRefAux;
                                ordem.CodEmpresa = 1;
                                dtDesembaraco = dadosRevenda.DT_DESEMBARACO;
                                dtEmbarque = dadosRevenda.DT_EMBARQUE;
                                nrBL = dadosRevenda.NR_BL;
                                nrLpco = null;
                                dsTerminalRedex = dadosRevenda.NM_TERMINAL_REDEX;
                                dsTerminalEmbarque = dadosRevenda.NM_EMBARQUE;

                                dtRedex = dadosRevenda.DT_TERMINAL_REDEX;
                                dtTerminalEmbarque = dadosRevenda.DT_TERMINAL_ATRACACAO;

                                dtSaidaNavio = dadosRevenda.DT_SAIDA_NAVIO;
                                dtCSI = dadosRevenda.DT_CHEGADA_CSI;
                                dtEnvioDraft = dadosRevenda.DT_CRIACAO_BL;
                                dtEnvioDocs = dadosRevenda.DT_ENVIO_DOCS;
                                dtEnvioCourrier = dadosRevenda.DT_ENVIO_COURRIER;
                                nrCourrier = dadosRevenda.NR_COURRIER;
                                nmCourrier = dadosRevenda.DS_COURRIER;
                                dsEnvioDocs = dadosRevenda.DS_ENVIO_DOCS;
                                dtAverbado = dadosRevenda.DT_AVERBADO;
                                bCSI = true;
                                nrCSI = dadosRevenda.NR_CSI;

                            }

                        }
                    }


                    ordem.ChaveAcesso = nrChave;
                    ordem.DtaRegistro = dtRegistro;
                    ordem.DUE = nrDUE;
                    ordem.RUC = nrRUC;
                    //ordem.Ordens = sRefAux;
                    ordem.Ordens = sReferencia;
                    ordem.CodEmpresa = 1;
                    ordem.DtaDesembaraco = dtDesembaraco;
                    ordem.DtaEmbarque = dtEmbarque;
                    ordem.Num_BL = nrBL;
                    ordem.Num_LPCO = nrLpco;
                    ordem.TerminalArmazenagem = dsTerminalRedex;
                    ordem.TerminalEmbarque = dsTerminalEmbarque;

                    ordem.DtaEntradaArmazenagem = dtRedex;
                    ordem.DtaEntradaTerminal = dtTerminalEmbarque;

                    if (dtEmbarque != null)
                        ordem.DtaSaidaNavio = dtSaidaNavio;
                    else
                        ordem.DtaSaidaNavio = null;
                    ordem.DtaCSI = dtCSI;
                    ordem.DtaDraft = dtEnvioDraft;
                    ordem.DtaDocsProntos = dtEnvioDocs;
                    ordem.DtaEnvioCourier = dtEnvioCourrier;
                    ordem.NrCourier = nrCourrier;
                    ordem.Courier = nmCourrier;
                    ordem.Observacoes = dsEnvioDocs;
                    ordem.DtaCargaExpTotal = dtAverbado;
                    ordem.DtaLiberacaoBL = DtLiberacaoBL;
                    if (bCSI)
                    {
                        ordem.CertificadoSanitario = nrCSI;
                    }
                    else
                    {
                        if (aCSI.Length > 1)
                        {
                            if (aReferencia.Length > 1)
                            {
                                try
                                {
                                    sCSI = aCSI[i];
                                    ordem.CertificadoSanitario = sCSI;
                                }
                                catch
                                {
                                    sCSI = "";
                                    ordem.CertificadoSanitario = sCSI;
                                }
                            }
                            else
                            {

                                sCSI = nrCSI.Replace(",", " ");
                                ordem.CertificadoSanitario = sCSI;
                            }
                        }
                        else
                        {
                            sCSI = nrCSI;
                            ordem.CertificadoSanitario = sCSI;
                        }
                    }
                    //if (nrCSI != null)
                    //    ordem.CertificadoSanitario = nrCSI.Replace(";", ",");
                    //else
                    //    ordem.CertificadoSanitario = "";

                    /*
                    ServiceRefMinerva.WSIntegraSatelSoapClient soapEnvia = new ServiceRefMinerva.WSIntegraSatelSoapClient();
                    retorno = soapEnvia.StatusOrdem(autentica, ordem);

                    if (!retorno.Retorno)
                    {
                        ret.msgErro = retorno.MsgRetorno;
                        ret.status = false;
                    }
                    else
                    {
                        ret.msgErro = retorno.MsgRetorno;
                        ret.status = true;
                    }*/

                    geraLogEnvioMinerva(idProcesso, sReferencia, nrChave, nrDUE, nrRUC, dtRegistro, dtDesembaraco, dtEmbarque, nrBL, null, dsTerminalRedex, dsTerminalEmbarque, dtRedex, dtTerminalEmbarque, dtSaidaNavio, dtEnvioDraft, dtCSI, dtEnvioDocs, dsEnvioDocs, dtEnvioCourrier, dtAverbado, nrCourrier, nmCourrier, sCSI, DtLiberacaoBL);
                }
            }
            catch (Exception e)
            {
                ret.msgErro = e.Message;
                ret.status = false;
                geraLogEnvioMinerva(idProcesso, sReferencia, nrChave, nrDUE, nrRUC, dtRegistro, dtDesembaraco, dtEmbarque, nrBL, null, dsTerminalRedex, dsTerminalEmbarque, dtRedex, dtTerminalEmbarque, dtSaidaNavio, dtEnvioDraft, dtCSI, dtEnvioDocs, dsEnvioDocs, dtEnvioCourrier, dtAverbado, nrCourrier, nmCourrier, sCSI, DtLiberacaoBL);
            }

            return ret;
        }
        /*
        public void TransmitirDadosMinerva(int idProcesso, DateTime? dtEmbarque)
        {

            Retorno retorno = new Retorno();
            DUE.camposDUE dadosDUE = RetornaDadosProcesso(idProcesso);
            if (dadosDUE != null)
            {
                if (dadosDUE.CD_GRUPO_CLI == 1)
                {
                    retorno = EnviaDados(dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC, dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, dtEmbarque, null, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO, dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL, dadosDUE.DT_CHEGADA_CSI);
                    geraLogEnvioMinerva(idProcesso, dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC, dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, null, null, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO, dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL, dadosDUE.DT_CHEGADA_CSI, retorno.status, retorno.msgErro);
                }
            }
        }
        
        public void TransmitirDadosMinervaCSI(int idProcesso, DateTime? dtCSI)
        {

            Retorno retorno = new Retorno();
            DUE.camposDUE dadosDUE = RetornaDadosProcesso(idProcesso);
            if (dadosDUE != null)
            {
                if (dadosDUE.CD_GRUPO_CLI == 1)
                {
                    retorno = EnviaDados(dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC, dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, dadosDUE.DT_EMBARQUE, null, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO, dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL, dadosDUE.DT_CHEGADA_CSI);
                    geraLogEnvioMinerva(idProcesso, dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC, dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, null, null, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO, dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL, dtCSI, retorno.status, retorno.msgErro);
                }
            }
        }
        
        
        public void TransmitirDadosMinerva(int idProcesso, string nrBL)
        {
            Retorno retorno = new Retorno();
            DUE.camposDUE dadosDUE = RetornaDadosProcesso(idProcesso);
            if (dadosDUE != null)
            {
                if (dadosDUE.CD_GRUPO_CLI == 1)
                {
                    retorno = EnviaDados(dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC, dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, dadosDUE.DT_EMBARQUE, nrBL, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO, dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL, dadosDUE.DT_CHEGADA_CSI);
                    geraLogEnvioMinerva(idProcesso, dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC, dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, dadosDUE.DT_EMBARQUE, nrBL, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO, dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL, dadosDUE.DT_CHEGADA_CSI, retorno.status, retorno.msgErro);
                }
            }
        }
        */
        public void TransmitirDadosMinerva(int idProcesso)
        {
            Retorno retorno = new Retorno();
             DUE.camposDUE dadosDUE = RetornaDadosProcesso(idProcesso);
            if (dadosDUE != null)
            {
                if (dadosDUE.CD_GRUPO_CLI == 1)
                {
                    int idProcessoreserva = 0;
                    int idReserva = 0;
                    if (dadosDUE.CD_PROCESSORESERVA != null)
                        idProcessoreserva = (int)dadosDUE.CD_PROCESSORESERVA;
                    if (dadosDUE.CD_RESERVA != null)
                        idReserva = (int)dadosDUE.CD_RESERVA;

                    retorno = EnviaDados(idProcesso, idProcessoreserva, idReserva, dadosDUE.DS_REFERENCIA, dadosDUE.CD_CHAVE_ACESSO, dadosDUE.CD_NUMERO_DUE, dadosDUE.CD_NUMERO_RUC,
                        dadosDUE.DT_TRANSMISSAO, dadosDUE.DT_DESEMBARACO, dadosDUE.DT_EMBARQUE, dadosDUE.NR_BL, null, dadosDUE.NM_TERMINAL_REDEX, dadosDUE.DS_TERMINAL_ATRACACAO,
                        dadosDUE.DT_TERMINAL_REDEX, dadosDUE.DT_TERMINAL_ATRACACAO, dadosDUE.DT_SAIDA_NAVIO, dadosDUE.DT_CRIACAO_BL,
                        dadosDUE.DT_CHEGADA_CSI, dadosDUE.DT_ENVIO_DOCS, dadosDUE.DS_ENVIO_DOCS, dadosDUE.DT_ENVIO_COURRIER, dadosDUE.DT_AVERBADO, dadosDUE.NR_COURRIER, dadosDUE.DS_COURRIER, dadosDUE.NR_CSI, dadosDUE.NR_CONTAINER, dadosDUE.DT_BL_REAL);

                    /*
                    TMP_MINERVA2 tmp = db.TMP_MINERVA2.FirstOrDefault(x => x.NR_ORDEM == dadosDUE.DS_REFERENCIA);
                    if (tmp != null)
                    {
                        tmp.DT_AVERBADO = dadosDUE.DT_AVERBADO;
                        tmp.IC_CONSULTADO = true;
                        tmp.DT_CANCELADO = dadosDUE.DT_CANCELADO;
                        db.SaveChanges();
                    }
                    */
                }
            }
        }

        public List<camposAviso> ListaAvisos()
        {
            var lstLPCO = (from p in db.PRE_PROCESSOS
                           join a in db.AVISOS_PRE_PROCESSO on p.CD_PROCESSO equals a.CD_PRE_PROCESSO
                           where (a.IC_LIDO == false || a.IC_LIDO == null)
                           select new camposAviso
                           {
                               idAviso = a.CD_AVISO,
                               nrProcesso = p.CD_NUMERO_PROCESSO,
                               Mensagem = a.DS_MENSAGEM,
                               nrOrdem = p.NR_ORDEM,
                               dtAviso = a.DT_AVISO,
                           });
            return lstLPCO.OrderBy(x => x.idAviso).ToList();
        }
    }
}