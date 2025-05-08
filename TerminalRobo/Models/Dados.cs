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
namespace TerminalRobo.Models
{

    public class ListaDeCampos
    {
        public int CD_PROCESSORESERVACONTAINER { get; set; }
        public int CD_PROCESSORESERVA { get; set; }
        public int CD_PROCESSO { get; set; }
        public int CD_CLIENTE { get; set; }
        public string DS_REFERENCIA_CLIENTE { get; set; }
        public string CD_NUMERO_PROCESSO { get; set; }
        public int? CD_TERMINAL_EMBARQUE { get; set; }
        public DateTime? DT_CONTAINER { get; set; }
        public DateTime? DT_ETA { get; set; }
        public string NR_CONTAINER { get; set; }
        public string NR_BOOKING { get; set; }
        public string NM_NAVIO { get; set; }
        public string NR_DUE { get; set; }
        public string NM_PROCESSO_STATUS2 { get; set; }
        public DateTime? DT_ABERTURA { get; set; }
        public DateTime? DT_DEPOSITO { get; set; }
        public string IC_TIPO { get; set; }
        public int CD_VIAGEM_ARMADOR { get; set; }
        public string DS_LACRE_AGENCIA { get; set; }
        public string DS_LACRE_SIF { get; set; }
    }

    public class InsereDados
    {
        public int? CD_PROCESSO { get; set; }
        public int? CD_USUARIO { get; set; }
        public string DS_GRUPO { get; set; }
        public string CD_NUMERO_PROCESSO { get; set; }
        public string NM_TERMINAL { get; set; }
        public string NR_CONTAINER { get; set; }
        public DateTime? DT_CONTAINER { get; set; }
        public DateTime? DT_EMBARQUE { get; set; }
        public DateTime? DT_DEPOSITO { get; set; }
        public DateTime? DT_ETA { get; set; }
        public string NR_BOOKING { get; set; }
        public string NM_NAVIO { get; set; }
        public string DS_STATUS { get; set; }
        public string NR_DUE { get; set; }
        public DateTime? DT_CONSULTA { get; set; }
        public DateTime? DT_PROTOCOLO { get; set; }
        public string NR_BOOKING_TERMINAL { get; set; }
        public string NM_NAVIO_TERMINAL { get; set; }
        public string NR_DUE_TERMINAL { get; set; }
        public string NM_CLIENTE { get; set; }
        public int? CD_GRUPO { get; set; }
        public string NM_PROCESSO_STATUS2 { get; set; }
        public string IC_TIPO { get; set; }
        public string NM_TERMINAL_SISTEMA { get; set; }
        public string DS_REFERENCIA_CLIENTE { get; set; }
        public string DS_LACRE_AGENCIA { get; set; }
        public string DS_LACRE_SIF { get; set; }
        public string DS_LACRE_AGENCIA_TERMINAL { get; set; }
        public string DS_LACRE_SIF_TERMINAL { get; set; }
        public int? IC_ROBO { get; set; }
    }

    public class UsuarioCopia
    {
        public int? CD_USUARIO { get; set; }
        public string NM_USUARIO { get; set; }
        public string NM_EMAIL_USUARIO { get; set; }

    }

    public class ListaAlertaRecinto
    {
        public string NR_PROCESSO { get; set; }
        public string DS_REFERENCIA { get; set; }
        public string NR_CONTAINER { get; set; }
        public string NR_DUE { get; set; }
        public int? CD_DUE { get; set; }
        public string NM_RECINTO_ANTERIOR { get; set; }
        public string NM_RECINTO { get; set; }
        public string NM_USUARIO { get; set; }
        public int? CD_PROCESSORESERVA { get; set; }
        public int? CD_PROCESSO { get; set; }
        public string NM_ENTIDADE { get; set; }
        public string NM_NAVIO { get; set; }
        public int? CD_CLIENTE { get; set; }

    }

    public class ListaNavio
    {
        public int? CD_VIAGEM { get; set; }
        public int? CD_TERMINAL { get; set; }
        public string NM_TERMINAL { get; set; }
        public string NM_NAVIO { get; set; }
        public DateTime? DT_ETA { get; set; }
        public DateTime? DT_ATRACACAO { get; set; }
        public string NR_VIAGEM { get; set; }
        public DateTime? DT_ETA_TERMINAL { get; set; }
        public DateTime? DT_DESATRACACAO { get; set; }
    }

    public class ListaLoginsSenhas
    {
        public string USUARIOSB { get; set; }
        public string SENHASB { get; set; }
        public string USUARIOEMBRAPORT { get; set; }
        public string SENHAEMBRAPORT { get; set; }
        public string USUARIOVILACONDE { get; set; }
        public string SENHAVILACONDE { get; set; }
    }

    public class Dados
    {
        public int? icRobo = int.Parse(ConfigurationManager.AppSettings["NrRobo"].ToString());

        WEBSATELEntities db = new WEBSATELEntities();

        public DateTime dtprimeiroHorario;
        public DateTime dtsegundoHorario;
        public DateTime dtterceiroHorario;
        public DateTime dtQuartoHorario;
        public DateTime dtQuintoHorario;
        public DateTime dtSextoHorario;
        public DateTime dtSetimoHorario;
        public DateTime dtOitavoHorario;
        public DateTime dtNonoHorario;
        public DateTime dtDecimoHorario;
        public DateTime dtDecimoPrimeiroHorario;
        public DateTime dtDecimoSegundoHorario;

        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_DUE_PROCESSO")]
        public static string FN_RETORNA_DUE_PROCESSO(int CD_PROCESSO, int? CD_PROCESSORESERVACONTAINER)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }
        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_PROCESSO_LOTE_DUE")]
        public static string FN_RETORNA_PROCESSO_LOTE_DUE(int? CD_DUE)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }
        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_CONTAINER_PROCESSO_DUE")]
        public static string FN_RETORNA_CONTAINER_PROCESSO_DUE(int CD_PROCESSO_RESERVA)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }

        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_RUC_PROCESSO")]
        public static string FN_RETORNA_RUC_PROCESSO(int CD_PROCESSO, int? CD_PROCESSORESERVACONTAINER)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }

        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_DUE_PROCESSO_DATAS")]
        public static DateTime? FN_RETORNA_DUE_PROCESSO_DATAS(int CD_PROCESSO, int? CD_PROCESSORESERVACONTAINER, int TIPO)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }

        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_DUE__CHAVE_ACESSO_PROCESSO")]
        public static string FN_RETORNA_DUE__CHAVE_ACESSO_PROCESSO(int CD_PROCESSO, int? CD_PROCESSORESERVACONTAINER)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }

        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_CSI_PROCESSO")]
        public static string FN_RETORNA_CSI_PROCESSO(int CD_PROCESSORESERVA)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }

        [EdmFunction("WEBSATELModel.Store", "FN_RETORNA_STATUS")]
        public static string FN_RETORNA_STATUS(int CD_PROCESSORESERVA, int CD_CONTAINER)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }


        public GRUPOCLI retornaGrupoCli(int cdGrupo){
            GRUPOCLI gr = db.GRUPOCLI.FirstOrDefault(x => x.CD_GRUPOCLI == cdGrupo);
            return gr;
        }
        public void RetornaHorarioConsulta() { 
            //Retorna primeiro horário
           PARAMETRO parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO1");
            string sdata = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
            if (parametro != null) {

                dtprimeiroHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }
            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO2");
            
            if (parametro != null)
            {

                dtsegundoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO3");

            if (parametro != null)
            {

                dtterceiroHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO4");

            if (parametro != null)
            {

                dtQuartoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO5");

            if (parametro != null)
            {

                dtQuintoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO6");

            if (parametro != null)
            {

                dtSextoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO7");

            if (parametro != null)
            {

                dtSetimoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO8");

            if (parametro != null)
            {

                dtOitavoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO9");

            if (parametro != null)
            {

                dtNonoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO10");

            if (parametro != null)
            {

                dtDecimoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO11");

            if (parametro != null)
            {

                dtDecimoPrimeiroHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }

            parametro = db.PARAMETRO.FirstOrDefault(x => x.NM_PARAMETRO == "CONSULTA DEPOSITO12");

            if (parametro != null)
            {

                dtDecimoSegundoHorario = Convert.ToDateTime(sdata + " " + parametro.VR_PARAMETRO);
            }
        }

        public ListaLoginsSenhas RetornaLoginSenhas()
        {
            var parametros = db.PARAMETRO
                .Where(p => p.NM_PARAMETRO == "USUARIOSB" ||
                            p.NM_PARAMETRO == "SENHASB" ||
                            p.NM_PARAMETRO == "USUARIOEMBRAPORT" ||
                            p.NM_PARAMETRO == "SENHAEMBRAPORT" ||
                            p.NM_PARAMETRO == "USUARIOVILACONDE" ||
                            p.NM_PARAMETRO == "SENHAVILACONDE")
                .ToList();

            ListaLoginsSenhas retorno = new ListaLoginsSenhas
            {
                USUARIOSB = parametros.FirstOrDefault(p => p.NM_PARAMETRO == "USUARIOSB")?.VR_PARAMETRO,
                SENHASB = parametros.FirstOrDefault(p => p.NM_PARAMETRO == "SENHASB")?.VR_PARAMETRO,
                USUARIOEMBRAPORT = parametros.FirstOrDefault(p => p.NM_PARAMETRO == "USUARIOEMBRAPORT")?.VR_PARAMETRO,
                SENHAEMBRAPORT = parametros.FirstOrDefault(p => p.NM_PARAMETRO == "SENHAEMBRAPORT")?.VR_PARAMETRO,
                USUARIOVILACONDE = parametros.FirstOrDefault(p => p.NM_PARAMETRO == "USUARIOVILACONDE")?.VR_PARAMETRO,
                SENHAVILACONDE = parametros.FirstOrDefault(p => p.NM_PARAMETRO == "SENHAVILACONDE")?.VR_PARAMETRO
            };

            return retorno;
        }

        public void LimparTabelatemporaria(bool IcEmbarque) {
            if (IcEmbarque)
            {
                var qtRegistro = (from tmp in db.TMP_CONSULTA_EMBARQUE select new { tmp }).FirstOrDefault();
                //Se existir registro então limpa a tabela.
                if (qtRegistro != null)
                {
                    var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    objCtx.ExecuteStoreCommand(" DELETE FROM TMP_CONSULTA_EMBARQUE WHERE IC_ROBO = '" + icRobo + "'");
                    objCtx = null;
                }
            }
            else
            {
                var qtRegistro = (from tmp in db.TMP_CONSULTA_NAVIO select new { tmp }).FirstOrDefault();
                //Se existir registro então limpa a tabela.
                if (qtRegistro != null)
                {
                    var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    objCtx.ExecuteStoreCommand(" DELETE FROM TMP_CONSULTA_NAVIO");
                    objCtx = null;
                }
            }
        }

        public Boolean? inseriHistoricoConsulta(DateTime? dtEnvio, int idTerminal)
        {
            try
            {
                HIST_CONSULTA_DEPOSITO insere_hist = new HIST_CONSULTA_DEPOSITO();

                insere_hist.DT_CONSULTA_DEPOSITO = dtEnvio;
                insere_hist.CD_CLIENTE = idTerminal;
                db.HIST_CONSULTA_DEPOSITO.Add(insere_hist);
                db.SaveChanges();
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }
        public string RetornaEmailUsuario(int idUsuario, string tipo) {
            if (tipo == "email")
            {
                var emailUsuario = (from e in db.ENTIDADE
                                    join u in db.USUARIO_CA on e.CD_ENTIDADE equals u.CD_ENTIDADE
                                    where u.CD_USUARIO == idUsuario
                                    select new
                                    {
                                        e.EMAIL_ENTIDADE,
                                        e.NM_FANTASIA_ENTIDADE
                                    }).FirstOrDefault().EMAIL_ENTIDADE;
                return emailUsuario;
            }
            else {
                var emailUsuario = (from e in db.ENTIDADE
                                    join u in db.USUARIO_CA on e.CD_ENTIDADE equals u.CD_ENTIDADE
                                    where u.CD_USUARIO == idUsuario
                                    select new
                                    {
                                        e.EMAIL_ENTIDADE,
                                        e.NM_FANTASIA_ENTIDADE
                                    }).FirstOrDefault().NM_FANTASIA_ENTIDADE;
                return emailUsuario;
            }
        }
        public bool retornaHistoricoConsultaDeposito(DateTime dtEnvio, int idTerminal) {
            
            HIST_CONSULTA_DEPOSITO insere_hist = db.HIST_CONSULTA_DEPOSITO.FirstOrDefault(x=> x.DT_CONSULTA_DEPOSITO == dtEnvio && x.CD_CLIENTE == idTerminal);
            return (insere_hist != null);
        }
        
        public List<ListaDeCampos> ConsultaContainerDeadLineDia(int? idTerminal, string nrContainer)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS(idTerminal, nrContainer, false)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });
            /*
            int idgrupocliente = 0;
            DateTime data = DateTime.Now.AddDays(-1);

            var lstProcesso = (from p in db.PROCESSOS

                               join pr in db.PROCESSORESERVA on p.CD_PROCESSO equals pr.CD_PROCESSO
                               join res in db.RESERVAS on pr.CD_RESERVA equals res.CD_RESERVA
                               join prc in db.PROCESSORESERVACONTAINER on pr.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA
                               join c in db.CONTAINER on prc.CD_CONTAINER equals c.CD_CONTAINER
                               join via in db.VIAGEMARMADOR on res.CD_VIAGEM_ARMADOR equals via.CD_VIAGEM_ARMADOR into viaLeft
                               from viaOri in viaLeft.DefaultIfEmpty()
                               join arm in db.ENTIDADE on viaOri.CD_ARMADOR equals arm.CD_ENTIDADE into armLeft
                               from armOri in armLeft.DefaultIfEmpty()
                               join term in db.ENTIDADE on viaOri.CD_TERMINAL_ATRACACAO equals term.CD_ENTIDADE into termLeft
                               from termOri in termLeft.DefaultIfEmpty()
                               join transp in db.ENTIDADE on prc.CD_TRANSPORTADORA equals transp.CD_ENTIDADE into transpLeft
                               from transpOri in transpLeft.DefaultIfEmpty()
                               join termR in db.ENTIDADE on prc.CD_TERMINAL_REDEX equals termR.CD_ENTIDADE into termRLeft
                               from termROri in termRLeft.DefaultIfEmpty()
                               join ArmR in db.ENTIDADE on prc.CD_ARMAZEM equals ArmR.CD_ENTIDADE into ArmRLeft
                               from ArmROri in ArmRLeft.DefaultIfEmpty()
                               join viagem in db.VIAGEM on viaOri.CD_VIAGEM equals viagem.CD_VIAGEM into viagemLeft
                               from viagemOri in viagemLeft.DefaultIfEmpty()
                               join nav in db.NAVIOS on viagemOri.CD_NAVIO equals nav.CD_NAVIO into navLeft
                               from navOri in navLeft.DefaultIfEmpty()
                               join portolocal in db.LOCAIS on res.CD_PORTO_ORIGEM equals portolocal.CD_LOCAL into portolocalLeft
                               from portolocalOri in portolocalLeft.DefaultIfEmpty()
                               join paisDestino in db.PAIS on p.CD_PAIS_RESERVA equals paisDestino.CD_PAIS into paisDestinoLeft
                               from paisDestinoOri in paisDestinoLeft.DefaultIfEmpty()
                               join portodestino in db.LOCAIS on res.CD_PORTO_DESTINO equals portodestino.CD_LOCAL into portodestinoLeft
                               from portodestinoOri in portodestinoLeft.DefaultIfEmpty()
                               join status in db.STATUS_PROCESSO on prc.CD_STATUS_CONTAINER equals status.CD_STATUS_PROCESSO into statusLeft
                               from statusOri in statusLeft.DefaultIfEmpty()
                               join status2 in db.STATUS_PROCESSO on prc.CD_STATUS_CONTAINER2 equals status2.CD_STATUS_PROCESSO into status2Left
                               from status2Ori in status2Left.DefaultIfEmpty()
                               where (p.IC_CANCELADO == false) && (p.CD_SERVICO_PROCESSO != 2) && (((prc.DT_DEPOSITO_TERM_EMBARQUE == null || prc.DT_DESPACHO_PROTOCOLADO == null) || viagemOri.DT_ETA < data) && (prc.DT_EMBARQUE == null)) &&
                               
                               //((prc.CD_STATUS_CONTAINER2 != 9 && prc.CD_STATUS_CONTAINER2 != 3 && prc.CD_STATUS_CONTAINER2 != 7) || prc.CD_STATUS_CONTAINER2 == null) &&
                               (p.IC_ATIVO == true && (p.IC_ENCERRADO == false || p.IC_ENCERRADO == null))

                               //&& (c.CD_NUMERO_CONTAINER == "TTNU8214748" || c.CD_NUMERO_CONTAINER == "MEDU9829628")

                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = p.CD_PROCESSO,
                                   CD_CLIENTE = p.CD_CLIENTE,
                                   CD_PROCESSORESERVA = pr.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = prc.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = p.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = c.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = viaOri.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = res.CD_NUMERO_RESERVA,
                                   NM_NAVIO = navOri.NM_NAVIO,
                                   DT_CONTAINER = viaOri.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = FN_RETORNA_DUE_PROCESSO(p.CD_PROCESSO, prc.CD_PROCESSORESERVACONTAINER),
                                   NM_PROCESSO_STATUS2 = status2Ori.NM_STATUS_PROCESSO,
                                   DT_ABERTURA = p.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = prc.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = prc.DT_DEPOSITO_TERM_EMBARQUE != null && prc.DT_DESPACHO_PROTOCOLADO != null ? "E" : "D"
                               });

            if (idTerminal != null)
            {
                lstProcesso = lstProcesso.Where(x => x.CD_TERMINAL_EMBARQUE == idTerminal);
            }
            
            if (nrContainer != null) {
                lstProcesso = lstProcesso.Where(x => x.NR_CONTAINER == nrContainer);
            }
            
            if (idgrupocliente != 0)
            {
                List<int> lstCliente = (from gru in db.GRUPOCLI_ENTIDADE
                                        where gru.CD_GRUPOCLI == idgrupocliente
                                        select gru.CD_ENTIDADE).ToList();

                lstProcesso = lstProcesso.Where(x => lstCliente.Contains(x.CD_CLIENTE));
                //lstProcesso = lstProcesso.Where(x => x.CD_GRUPO_CLIENTE == idgrupocliente);
            }*/

            return lstProcesso.OrderByDescending(x=>x.NR_CONTAINER).ToList();
        }

        public List<ListaDeCampos> ConsultaContainerDeadLineDiaCliente(int? idTerminal, string nrContainer, string grupo, string grupo_nao)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS_CLIENTE(idTerminal, nrContainer, false, grupo, grupo_nao)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });
        
            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }

        public List<ListaDeCampos> ConsultaContainerEmbarcados(int? idTerminal, string nrContainer)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS(idTerminal, nrContainer, true)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = "C",
                                   CD_VIAGEM_ARMADOR = s.CD_VIAGEM_ARMADOR == null ? 0 : (int)s.CD_VIAGEM_ARMADOR,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });
           

            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }

        public List<ListaDeCampos> ConsultaContainerEmbarcadosClientes(int? idTerminal, string nrContainer,string grupo,string grupo_nao)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS_CLIENTE(idTerminal, nrContainer, true,grupo,grupo_nao)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = "C",
                                   CD_VIAGEM_ARMADOR = s.CD_VIAGEM_ARMADOR == null ? 0 : (int)s.CD_VIAGEM_ARMADOR,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });
           

            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }


        public List<ListaDeCampos> ConsultaContainerTerminal(int? idTerminal, string nrContainer)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS_TERMINAL(nrContainer, idTerminal)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });

           /* if (!string.IsNullOrEmpty(nrContainer))
            {
                lstProcesso = lstProcesso.Where(x => x.NR_CONTAINER == nrContainer);
            }*/
            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }

        public List<ListaDeCampos> ConsultaContainerTerminalCliente(int? idTerminal, string nrContainer, string grupo , string gruponao)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS_TERMINAL_CLIENTE(nrContainer, idTerminal, grupo, gruponao)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });

            /* if (!string.IsNullOrEmpty(nrContainer))
             {
                 lstProcesso = lstProcesso.Where(x => x.NR_CONTAINER == nrContainer);
             }*/
            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }



        public List<ListaDeCampos> ConsultaContainerTerminalEmbarque(int? idTerminal, string nrContainer)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS_EMBARQUE(idTerminal, nrContainer)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });

           if (!string.IsNullOrEmpty(nrContainer))
          {
              lstProcesso = lstProcesso.Where(x => x.NR_CONTAINER == nrContainer);
          }
            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }

        public List<ListaDeCampos> ConsultaContainerTerminalEmbarqueCliente(int? idTerminal, string nrContainer, string grupo, string grupo_nao)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_CONTAINERS_EMBARQUE_CLIENTE(idTerminal, nrContainer, grupo, grupo_nao)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NR_DUE = s.NR_DUE,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });

            if (!string.IsNullOrEmpty(nrContainer))
            {
                lstProcesso = lstProcesso.Where(x => x.NR_CONTAINER == nrContainer);
            }
            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }


        public List<ListaDeCampos> ConsultaLacreTerminal(int? idTerminal, string nrContainer)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_LACRES_TERMINAL(idTerminal, nrContainer)
                               select new ListaDeCampos
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });

            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }

        public List<ListaDeCampos> ConsultaLacreTerminalCliente(int? idTerminal, string nrContainer, string grupo, string grupo_nao)
        {
            var lstProcesso = (from s in db.SP_SELECT_CONSULTA_LACRES_TERMINAL_CLIENTE(idTerminal, nrContainer, grupo, grupo_nao)
                               select new ListaDeCampos                               
                               {
                                   CD_PROCESSO = s.CD_PROCESSO,
                                   CD_CLIENTE = s.CD_CLIENTE,
                                   DS_REFERENCIA_CLIENTE = s.DS_REFERENCIA_CLIENTE,
                                   CD_PROCESSORESERVA = s.CD_PROCESSORESERVA,
                                   CD_PROCESSORESERVACONTAINER = s.CD_PROCESSORESERVACONTAINER,
                                   CD_NUMERO_PROCESSO = s.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = s.CD_NUMERO_CONTAINER,
                                   CD_TERMINAL_EMBARQUE = s.CD_TERMINAL_ATRACACAO,
                                   NR_BOOKING = s.CD_NUMERO_RESERVA,
                                   NM_NAVIO = s.NM_NAVIO,
                                   DT_CONTAINER = s.DT_DEAD_LINE_CONTAINER,
                                   NM_PROCESSO_STATUS2 = s.NM_STATUS_PROCESSO2,
                                   DT_ABERTURA = s.DT_ABERTURA_PROCESSO,
                                   DT_DEPOSITO = s.DT_DEPOSITO_TERM_EMBARQUE,
                                   IC_TIPO = s.IC_TIPO,
                                   DT_ETA = s.DT_ETA,
                                   DS_LACRE_AGENCIA = s.DS_LACRE_AGENCIA,
                                   DS_LACRE_SIF = s.DS_LACRE_SIF
                               });

            return lstProcesso.OrderByDescending(x => x.NR_CONTAINER).ToList();
        }

        public List<UsuarioCopia> emailUsuarioCopia(int? idUsuario)
        {
            
            var lstUsuarioCopia = (from gu in db.GRUPOCLI_TERMINAL_CONSULTA_USUARIO_DESTINATARIO
                                   join u in db.USUARIO_CA on gu.CD_USUARIO_DESTINATARIO equals u.CD_USUARIO
                                   join e in db.ENTIDADE on u.CD_ENTIDADE equals e.CD_ENTIDADE
                                   where gu.CD_USUARIO == idUsuario                   
                               select new UsuarioCopia
                               {
                                   CD_USUARIO = u.CD_USUARIO,
                                   NM_USUARIO = u.NM_USUARIO,
                                   NM_EMAIL_USUARIO = e.EMAIL_ENTIDADE
                               });

            return lstUsuarioCopia.ToList();

        }

        public List<InsereDados> ConsultaDivergencia()
        {


            var lstProcesso = (from tmp in db.SP_CONSULTA_DEPOSITO_USUARIO()
                               select new InsereDados
                               {
                                   CD_PROCESSO = tmp.CD_PROCESSO,
                                   CD_USUARIO = tmp.IC_TIPO != "E" ? tmp.CD_USUARIO : tmp.CD_USUARIO_C,
                                   DS_GRUPO = tmp.DS_GRUPOCLI,
                                   CD_NUMERO_PROCESSO = tmp.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = tmp.NR_CONTAINER,
                                   NM_NAVIO = tmp.NM_NAVIO,
                                   NM_NAVIO_TERMINAL = tmp.NM_NAVIO_TERMINAL,
                                   NR_BOOKING = tmp.NR_BOOKING,
                                   NR_BOOKING_TERMINAL = tmp.NR_BOOKING_TERMINAL,
                                   DT_CONTAINER = tmp.DT_CONTAINER,
                                   NR_DUE = tmp.NR_DUE,
                                   DS_STATUS = tmp.DS_STATUS,
                                   DT_PROTOCOLO = tmp.DT_PROTOCOLADO,
                                   DT_CONSULTA = tmp.DT_CONSULTA,
                                   DT_EMBARQUE = tmp.DT_EMBARQUE,
                                   DT_DEPOSITO = tmp.DT_DEPOSITO,
                                   NM_TERMINAL = tmp.NM_TERMINAL,
                                   NM_CLIENTE = tmp.NM_FANTASIA_ENTIDADE,
                                   CD_GRUPO = tmp.CD_GRUPOCLI,
                                   NM_PROCESSO_STATUS2 = tmp.NM_PROCESSO_STATUS2,
                                   IC_TIPO = tmp.IC_TIPO,
                                   NM_TERMINAL_SISTEMA = tmp.NM_TERMINAL_SISTEMA,
                                   DT_ETA = tmp.DT_ETA,
                                   DS_REFERENCIA_CLIENTE = tmp.DS_REFERENCIA_CLIENTE,
                                   DS_LACRE_AGENCIA = tmp.DS_LACRE_AGENCIA,
                                   DS_LACRE_AGENCIA_TERMINAL = tmp.DS_LACRE_AGENCIA_TERMINAL,
                                   DS_LACRE_SIF = tmp.DS_LACRE_SIF,
                                   DS_LACRE_SIF_TERMINAL = tmp.DS_LACRE_SIF_TERMINAL,
                                   IC_ROBO = tmp.IC_ROBO
                               });

            lstProcesso = lstProcesso.Where(x=> x.IC_ROBO == icRobo);

            return lstProcesso.OrderBy(x => x.CD_USUARIO).ToList();
        }

        public List<InsereDados> ConsultaDivergenciaCliente(int ic_robo)
        {


            var lstProcesso = (from tmp in db.SP_CONSULTA_DEPOSITO_USUARIO_IC_ROBO(ic_robo)
                               select new InsereDados
                               {
                                   CD_PROCESSO = tmp.CD_PROCESSO,
                                   CD_USUARIO = tmp.IC_TIPO != "E" ? tmp.CD_USUARIO : tmp.CD_USUARIO_C,
                                   DS_GRUPO = tmp.DS_GRUPOCLI,
                                   CD_NUMERO_PROCESSO = tmp.CD_NUMERO_PROCESSO,
                                   NR_CONTAINER = tmp.NR_CONTAINER,
                                   NM_NAVIO = tmp.NM_NAVIO,
                                   NM_NAVIO_TERMINAL = tmp.NM_NAVIO_TERMINAL,
                                   NR_BOOKING = tmp.NR_BOOKING,
                                   NR_BOOKING_TERMINAL = tmp.NR_BOOKING_TERMINAL,
                                   DT_CONTAINER = tmp.DT_CONTAINER,
                                   NR_DUE = tmp.NR_DUE,
                                   DS_STATUS = tmp.DS_STATUS,
                                   DT_PROTOCOLO = tmp.DT_PROTOCOLADO,
                                   DT_CONSULTA = tmp.DT_CONSULTA,
                                   DT_EMBARQUE = tmp.DT_EMBARQUE,
                                   DT_DEPOSITO = tmp.DT_DEPOSITO,
                                   NM_TERMINAL = tmp.NM_TERMINAL,
                                   NM_CLIENTE = tmp.NM_FANTASIA_ENTIDADE,
                                   CD_GRUPO = tmp.CD_GRUPOCLI,
                                   NM_PROCESSO_STATUS2 = tmp.NM_PROCESSO_STATUS2,
                                   IC_TIPO = tmp.IC_TIPO,
                                   NM_TERMINAL_SISTEMA = tmp.NM_TERMINAL_SISTEMA,
                                   DT_ETA = tmp.DT_ETA,
                                   DS_REFERENCIA_CLIENTE = tmp.DS_REFERENCIA_CLIENTE,
                                   DS_LACRE_AGENCIA = tmp.DS_LACRE_AGENCIA,
                                   DS_LACRE_AGENCIA_TERMINAL = tmp.DS_LACRE_AGENCIA_TERMINAL,
                                   DS_LACRE_SIF = tmp.DS_LACRE_SIF,
                                   DS_LACRE_SIF_TERMINAL = tmp.DS_LACRE_SIF_TERMINAL,
                                   IC_ROBO = tmp.IC_ROBO
                               });

            //lstProcesso = lstProcesso.Where(x => x.IC_ROBO == icRobo);

            return lstProcesso.OrderBy(x => x.CD_USUARIO).ToList();
        }

        public bool InsereConsulta(InsereDados dados)
        {
            TMP_CONSULTA_EMBARQUE editTMP = db.TMP_CONSULTA_EMBARQUE.FirstOrDefault(x => x.NR_CONTAINER == dados.NR_CONTAINER && x.IC_ROBO == icRobo);
            if (editTMP != null)
            {
                editTMP.NR_CONTAINER = dados.NR_CONTAINER;
                editTMP.NR_BOOKING = dados.NR_BOOKING;
                editTMP.NM_NAVIO = dados.NM_NAVIO;
                editTMP.DT_EMBARQUE = dados.DT_EMBARQUE;
                editTMP.DT_DEPOSITO = dados.DT_DEPOSITO;
                editTMP.IC_TIPO = dados.IC_TIPO;
                editTMP.DT_CONSULTA = DateTime.Now;
                editTMP.DS_STATUS = dados.DS_STATUS;
                editTMP.NM_TERMINAL = dados.NM_TERMINAL;
                editTMP.CD_PROCESSO = dados.CD_PROCESSO;
                editTMP.CD_NUMERO_PROCESSO = dados.CD_NUMERO_PROCESSO;
                editTMP.DT_CONTAINER = dados.DT_CONTAINER;
                editTMP.NR_BOOKING_TERMINAL = dados.NR_BOOKING_TERMINAL;
                editTMP.NM_NAVIO_TERMINAL = dados.NM_NAVIO_TERMINAL;
                editTMP.NR_DUE_TERMINAL = dados.NR_DUE_TERMINAL;
                editTMP.DT_PROTOCOLADO = dados.DT_PROTOCOLO;
                editTMP.NM_PROCESSO_STATUS2 = dados.NM_PROCESSO_STATUS2;
                editTMP.NM_TERMINAL_SISTEMA = dados.NM_TERMINAL_SISTEMA;
                editTMP.DT_ETA = dados.DT_ETA;
                editTMP.DS_REFERENCIA_CLIENTE = dados.DS_REFERENCIA_CLIENTE;
                editTMP.DS_LACRE_AGENCIA = dados.DS_LACRE_AGENCIA;
                editTMP.DS_LACRE_SIF = dados.DS_LACRE_SIF;
                editTMP.DS_LACRE_AGENCIA_TERMINAL = dados.DS_LACRE_AGENCIA_TERMINAL;
                editTMP.DS_LACRE_SIF_TERMINAL = dados.DS_LACRE_SIF_TERMINAL;
                db.SaveChanges();
            }
            else
            {
                TMP_CONSULTA_EMBARQUE tbTMP = new TMP_CONSULTA_EMBARQUE();
                tbTMP.CD_CHAVE = 0;
                tbTMP.NR_CONTAINER = dados.NR_CONTAINER;
                tbTMP.NR_BOOKING = dados.NR_BOOKING;
                tbTMP.NM_NAVIO = dados.NM_NAVIO;
                tbTMP.DT_EMBARQUE = dados.DT_EMBARQUE;
                tbTMP.DT_DEPOSITO = dados.DT_DEPOSITO;
                tbTMP.IC_TIPO = dados.IC_TIPO;
                tbTMP.DT_CONSULTA = DateTime.Now;
                tbTMP.DS_STATUS = dados.DS_STATUS;
                tbTMP.NM_TERMINAL = dados.NM_TERMINAL;
                tbTMP.CD_PROCESSO = dados.CD_PROCESSO;
                tbTMP.CD_NUMERO_PROCESSO = dados.CD_NUMERO_PROCESSO;
                tbTMP.DT_CONTAINER = dados.DT_CONTAINER;
                tbTMP.NR_BOOKING_TERMINAL = dados.NR_BOOKING_TERMINAL;
                tbTMP.NM_NAVIO_TERMINAL = dados.NM_NAVIO_TERMINAL;
                tbTMP.NR_DUE_TERMINAL = dados.NR_DUE_TERMINAL;
                tbTMP.DT_PROTOCOLADO = dados.DT_PROTOCOLO;
                tbTMP.NM_PROCESSO_STATUS2 = dados.NM_PROCESSO_STATUS2;
                tbTMP.NM_TERMINAL_SISTEMA = dados.NM_TERMINAL_SISTEMA;
                tbTMP.DT_ETA = dados.DT_ETA;
                tbTMP.DS_REFERENCIA_CLIENTE = dados.DS_REFERENCIA_CLIENTE;
                tbTMP.DS_LACRE_AGENCIA = dados.DS_LACRE_AGENCIA;
                tbTMP.DS_LACRE_SIF = dados.DS_LACRE_SIF;
                tbTMP.DS_LACRE_AGENCIA_TERMINAL = dados.DS_LACRE_AGENCIA_TERMINAL;
                tbTMP.DS_LACRE_SIF_TERMINAL = dados.DS_LACRE_SIF_TERMINAL;
                tbTMP.IC_ROBO = icRobo;
                db.TMP_CONSULTA_EMBARQUE.Add(tbTMP);
                db.SaveChanges();
            }



            return true;

        }
        public bool AtualizaDataDeposito(int cdProcesso, int cdProcessoReserva, int cdProcessoReservaContainer, DateTime? dtDeposito, DateTime? dtEmbarque)
        {
            PROCESSORESERVACONTAINER processoreservacontainer = db.PROCESSORESERVACONTAINER.FirstOrDefault(x => x.CD_PROCESSORESERVA == cdProcessoReserva && x.CD_PROCESSORESERVACONTAINER == cdProcessoReservaContainer);
            if (processoreservacontainer != null)
            {
                bool mudanca = false;
                if (processoreservacontainer.DT_DEPOSITO_TERM_EMBARQUE == null && dtDeposito != null)
                {
                    processoreservacontainer.DT_DEPOSITO_TERM_EMBARQUE = dtDeposito;
                    db.SaveChanges();
                    InsereFollowUpProcesso(cdProcesso, 43, 1, dtDeposito);
                    mudanca = true;
                }
                if (processoreservacontainer.DT_EMBARQUE == null && dtEmbarque != null)
                {
                    processoreservacontainer.DT_EMBARQUE = dtEmbarque;
                    db.SaveChanges();
                    InsereFollowUpProcesso(cdProcesso, 2, 1, dtEmbarque);
                    atualizarStatusPreProcesso(cdProcesso, 5);
                    mudanca = true;
                }
                if (processoreservacontainer.DT_EMBARQUE != null && dtEmbarque == null)
                {
                    processoreservacontainer.DT_EMBARQUE = dtEmbarque;
                    db.SaveChanges();
                    InsereFollowUpProcesso(cdProcesso, 97, 1, DateTime.Today); //CONTAINER DESEMBARCADO
                    atualizarStatusPreProcesso(cdProcesso, 3);
                    mudanca = true;
                }
                if (mudanca)
                {
                    db.SP_ATUALIZA_STATUS_CONTAINER(cdProcessoReservaContainer, processoreservacontainer.CD_CONTAINER);

                    //string cdStatus = retornaStatusProcesso(cdProcessoReservaContainer, processoreservacontainer.CD_CONTAINER);
                    //if (cdStatus != "0" && cdStatus != "")
                    //{
                    //    string[] aStatus = cdStatus.Split('_');
                    //    UpdateProcessoStatus((int)processoreservacontainer.CD_PROCESSORESERVACONTAINER, int.Parse(aStatus[0]), "campoStatusEmbarque", 1);

                    //SELECIONAR OS PROCESSOS QUE SÃO MINERVA PARA ENVIAR VIA WS OS DADOS DO PROCESSO COM A SAÍDA DO NAVIO.
                    var _processos = (from p in db.PROCESSOS
                                          join ge in db.GRUPOCLI_ENTIDADE on p.CD_CLIENTE equals ge.CD_ENTIDADE
                                          join g in db.GRUPOCLI on ge.CD_GRUPOCLI equals g.CD_GRUPOCLI
                                          where p.CD_PROCESSO == cdProcesso && g.CD_GRUPOCLI == 1
                                          select new { p.CD_PROCESSO }).ToList();
                        if (_processos.Count > 0)
                        {
                            try
                            {
                                WSMinerva wsminerva = new WSMinerva();
                                foreach (var _conteudo in _processos)
                                {

                                    //Atualiza WebService do Minerva
                                    wsminerva.TransmitirDadosMinerva(_conteudo.CD_PROCESSO);
                                }

                            }
                            catch //(Exception ex)
                            {
                            }
                        }

                    //}

                    int? cdDUE = (from pr in db.PROCESSORESERVA
                                  join d in db.DUE on pr.CD_PROCESSO equals d.CD_PROCESSO
                                  where pr.CD_PROCESSORESERVA == cdProcessoReserva
                                  select d.CD_DUE).FirstOrDefault();

                    string alteracao = "";
                    string strRecintoDAnt = "";
                    string strRecintoEAnt = "";
                    string strRecintoD = "";
                    string strRecintoE = "";
                    bool embarque = false;
                    bool despacho = false;

                    if (cdDUE != null && cdDUE != 0)
                    {
                        DataBase.DUE d = db.DUE.FirstOrDefault(x => x.CD_DUE == cdDUE);

                        int? cdTerminal = (from p in db.SP_RETORNA_TERMINAL_DUE(cdProcessoReserva) select p).FirstOrDefault();

                        if ((from r in db.RELACIONAMENTOENTIDADE where r.CD_ENTIDADE == cdTerminal && r.CD_TIPO_RELAC_ENTIDADE == 71 select r.CD_ENTIDADE).Count() > 0)
                            alteracao = "embarque";
                        else
                            alteracao = "desembaraço";

                        string strCodigoRecinto = (from e in db.ENTIDADE
                                                   join r in db.RECIALFA on e.STR_CODIGORECIALFA equals r.STR_CODIGORECIALFA
                                                   where e.CD_ENTIDADE == cdTerminal
                                                   select r.STR_CODIGORECIALFA).FirstOrDefault();

                        if (alteracao == "embarque")
                        {
                            if (d.CD_RECINTO_EMBARQUE != strCodigoRecinto)
                            {
                                embarque = true;
                                strRecintoE = strCodigoRecinto;
                                strRecintoEAnt = d.CD_RECINTO_EMBARQUE;
                                if (d.DT_TRANSMISSAO == null)
                                {
                                    d.CD_RECINTO_EMBARQUE = strCodigoRecinto;
                                    db.SaveChanges();
                                }
                            }

                            if (strCodigoRecinto != d.CD_RECINTO_DESPACHO)
                            {
                                despacho = true;
                                strRecintoDAnt = d.CD_RECINTO_DESPACHO;
                                strRecintoD = strCodigoRecinto;
                                if (d.DT_TRANSMISSAO == null)
                                {
                                    d.CD_RECINTO_DESPACHO = strCodigoRecinto;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else if (alteracao == "desembaraço")
                        {
                            cdTerminal = (from pr in db.PROCESSORESERVA
                                          join r in db.RESERVAS on pr.CD_RESERVA equals r.CD_RESERVA into leftr
                                          from orir in leftr.DefaultIfEmpty()
                                          join va in db.VIAGEMARMADOR on orir.CD_VIAGEM_ARMADOR equals va.CD_VIAGEM_ARMADOR into leftva
                                          from oriva in leftva.DefaultIfEmpty()
                                          where pr.CD_PROCESSORESERVA == cdProcessoReserva
                                          select oriva.CD_TERMINAL_ATRACACAO).FirstOrDefault();

                            if (cdTerminal != null && cdTerminal != 0)
                            {
                                string strCodigoRecintoEmb = (from e in db.ENTIDADE
                                                              join r in db.RECIALFA on e.STR_CODIGORECIALFA equals r.STR_CODIGORECIALFA
                                                              where e.CD_ENTIDADE == cdTerminal
                                                              select r.STR_CODIGORECIALFA).FirstOrDefault();

                                if (string.IsNullOrEmpty(strCodigoRecinto))
                                    strCodigoRecinto = strCodigoRecintoEmb;

                                if (d.CD_RECINTO_EMBARQUE != strCodigoRecintoEmb)
                                {
                                    embarque = true;
                                    strRecintoEAnt = d.CD_RECINTO_EMBARQUE;
                                    strRecintoE = strCodigoRecinto;
                                    if (d.DT_TRANSMISSAO == null)
                                    {
                                        d.CD_RECINTO_EMBARQUE = strCodigoRecintoEmb;
                                        db.SaveChanges();
                                    }

                                }

                                if (d.CD_RECINTO_DESPACHO != strCodigoRecinto)
                                {
                                    despacho = true;
                                    strRecintoDAnt = d.CD_RECINTO_DESPACHO;
                                    strRecintoD = strCodigoRecinto;
                                    if (d.DT_TRANSMISSAO == null)
                                    {
                                        d.CD_RECINTO_DESPACHO = strCodigoRecinto;
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(strCodigoRecinto))
                                {
                                    if (d.CD_RECINTO_DESPACHO != strCodigoRecinto)
                                    {
                                        despacho = true;
                                        strRecintoDAnt = d.CD_RECINTO_DESPACHO;
                                        strRecintoD = strCodigoRecinto;
                                        if (d.DT_TRANSMISSAO == null)
                                            d.CD_RECINTO_DESPACHO = strCodigoRecinto;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                        if (despacho && d.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 10 && d.DT_CANCELADO == null)
                        {
                            NotificacaoDUE(cdProcessoReserva, cdDUE, strRecintoEAnt, strRecintoE, strRecintoDAnt, strRecintoD, 1, embarque);
                        }
                        else if (embarque && d.DT_TRANSMISSAO != null && d.DT_CANCELADO == null)
                        {
                            NotificacaoDUE(cdProcessoReserva, cdDUE, strRecintoEAnt, strRecintoE, null, null, 1, embarque);
                        }
                    }
                }
            }
            return true;
        }

        public bool AtualizaDataProtocolado(int cdProcesso, int cdProcessoReserva, int cdProcessoReservaContainer, string container, InsereDados data)
        {
            PROCESSORESERVACONTAINER processoreservacontainer = new PROCESSORESERVACONTAINER();
            processoreservacontainer = db.PROCESSORESERVACONTAINER.FirstOrDefault(x => x.CD_PROCESSORESERVA == cdProcessoReserva && x.CD_PROCESSORESERVACONTAINER == cdProcessoReservaContainer);
            GeraLogContainerConsultado(cdProcesso, container, processoreservacontainer == null ? cdProcessoReserva.ToString() + "_" + cdProcessoReservaContainer.ToString(): "PRC OK", DateTime.Now, "", data);
            if (processoreservacontainer != null)
            {
                DateTime? dtProtocolado = (from prc in db.PROCESSORESERVACONTAINER where prc.CD_PROCESSORESERVA == cdProcessoReserva && prc.CD_PROCESSORESERVACONTAINER == cdProcessoReservaContainer select prc.DT_DESPACHO_PROTOCOLADO).FirstOrDefault();
                GeraLogContainerConsultado(cdProcesso, container, dtProtocolado == null ? "PROTOCOLADO NULO" : dtProtocolado.ToString(), DateTime.Now, "", data);
                if (dtProtocolado == null)
                {
                    processoreservacontainer.DT_DESPACHO_PROTOCOLADO = DateTime.Now;
                    db.SaveChanges();
                    InsereFollowUpProcesso(cdProcesso, 18, 1, processoreservacontainer.DT_DEPOSITO_TERM_EMBARQUE);

                    db.SP_ATUALIZA_STATUS_CONTAINER(cdProcessoReservaContainer, processoreservacontainer.CD_CONTAINER);

                    //string cdStatus = retornaStatusProcesso(cdProcessoReservaContainer, processoreservacontainer.CD_CONTAINER);
                    //if (cdStatus != "0" && cdStatus != "")
                    //{
                    //    string[] aStatus = cdStatus.Split('_');
                    //    UpdateProcessoStatus((int)processoreservacontainer.CD_PROCESSORESERVACONTAINER, int.Parse(aStatus[0]), "campoStatusEmbarque", 1);
                    //SELECIONAR OS PROCESSOS QUE SÃO MINERVA PARA ENVIAR VIA WS OS DADOS DO PROCESSO COM A SAÍDA DO NAVIO.
                    var _processos = (from p in db.PROCESSOS
                                      join ge in db.GRUPOCLI_ENTIDADE on p.CD_CLIENTE equals ge.CD_ENTIDADE
                                      join g in db.GRUPOCLI on ge.CD_GRUPOCLI equals g.CD_GRUPOCLI
                                      where p.CD_PROCESSO == cdProcesso && g.CD_GRUPOCLI == 1
                                      select new { p.CD_PROCESSO }).ToList();

                    int? cdStatus2 = (from prc in db.PROCESSORESERVACONTAINER
                                    where prc.CD_PROCESSORESERVACONTAINER == cdProcessoReservaContainer
                                    select prc.CD_STATUS_CONTAINER2).FirstOrDefault();

                    if (cdStatus2 == 11)
                    {
                        PROCESSORESERVACONTAINER editarProcessoRC = db.PROCESSORESERVACONTAINER.SingleOrDefault(x => x.CD_PROCESSORESERVACONTAINER == cdProcessoReservaContainer);
                        editarProcessoRC.CD_STATUS_CONTAINER2 = null;
                        db.Entry(editarProcessoRC).Property(p => p.CD_STATUS_CONTAINER2).IsModified = true;
                        db.SaveChanges();
                    }

                    if (_processos.Count > 0)
                        {
                            try
                            {
                                WSMinerva wsminerva = new WSMinerva();
                                foreach (var _conteudo in _processos)
                                {

                                    //Atualiza WebService do Minerva
                                    wsminerva.TransmitirDadosMinerva(_conteudo.CD_PROCESSO);
                                }

                            }
                            catch //(Exception ex)
                            {
                            }
                        }
                    //}
                }
            }
            return true;
        }

        public bool InsereFollowUpProcesso(int cd_processo, int idEvento, int idUsuario, DateTime? dtData)
        {
            try
            {
                PROCESSOEVENTO _processoevento = new PROCESSOEVENTO();
                _processoevento.CD_PROCESSO = cd_processo;
                _processoevento.CD_EVENTO = idEvento;
                _processoevento.CD_USUARIO = idUsuario;
                _processoevento.DT_PROCESSOEVENTO = dtData;
                _processoevento.DT_LANCAMENTO_PROCESSOEVENTO = DateTime.Now;
                db.PROCESSOEVENTO.Add(_processoevento);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public string retornaStatusProcesso(int processoReservaContainer, int container)
        {
            /*
            return db.ExecuteStoreQuery<string>(
                  "SELECT [dbo].[fnCalculateNumberOFWorkDays](@leaveID)",
                  new SqlParameter { [ParameterName = "CD_PROCESSORESERVA", Value = processoReserva] }
               ).FirstOrDefault();
            
            int result = db.CreateQuery<int>(
                    "SELECT VALUE ModelWebSatel.Store.FN_RETORNA_STATUS(@CD_PROCESSORESERVA) FROM {1}",
                    new ObjectParameter("CD_PROCESSORESERVA", processoReserva)
                    ).First();
            */
            string retval = (from prc in db.PROCESSORESERVACONTAINER where prc.CD_PROCESSORESERVACONTAINER == processoReservaContainer select FN_RETORNA_STATUS(processoReservaContainer, container)).FirstOrDefault();

            //string retval = db.Database.SqlQuery<string>(String.Format(@"select dbo.FN_RETORNA_STATUS({0}, {1})", processoReservaContainer, container)).FirstOrDefault().ToString();
            return retval;
        }

        public string UpdateProcessoStatus(int cd_processo, int status, string name, int idUsuario)
        {
            PROCESSORESERVACONTAINER _processo = db.PROCESSORESERVACONTAINER.SingleOrDefault(x => x.CD_PROCESSORESERVACONTAINER == cd_processo);
            if (_processo != null)
            {
                if (name == "campoStatusEmbarque")
                {
                    if (status != 0)
                        _processo.CD_STATUS_CONTAINER = status;
                    else
                        _processo.CD_STATUS_CONTAINER = null;
                }
                
                db.SaveChanges();

                if (status != 0)
                {
                    STATUS_PROCESSO _statusProcesso = db.STATUS_PROCESSO.Single(x => x.CD_STATUS_PROCESSO == status);

                    return _statusProcesso.NM_STATUS_PROCESSO;
                }
                else
                    return "...";
            }
            else
            {
                return "";
            }

        }

        public Boolean? atualizarStatusPreProcesso(int cd_processo, int? cdStatus)
        {

            try
            {
                PRE_PROCESSOS editarProcesso = db.PRE_PROCESSOS.Single(c => c.CD_PROCESSO_REAL == cd_processo);
                if (editarProcesso != null)
                {
                    editarProcesso.SG_STATUS = cdStatus;
                    db.SaveChanges();
                }

                return true;
            }
            catch
            {

                return false;
            }
        }

        public void GeraLogContainerConsultado(int CD_PROCESSO, string NR_CONTAINER, string NM_TERMINAL, DateTime? DT_CONSULTA, string DS_STATUS, InsereDados Dados)
        {
            
            LOG_CONSULTA_TERMINAL log = new LOG_CONSULTA_TERMINAL();
            log.CD_PROCESSO = CD_PROCESSO;
            log.NR_CONTAINER = NR_CONTAINER;
            
            log.DT_CONSULTA = DT_CONSULTA;
            log.NM_TERMINAL = NM_TERMINAL;
            log.DS_STATUS = DS_STATUS;
            log.NR_BOOKING_TERMINAL = Dados.NR_BOOKING_TERMINAL;
            log.NM_NAVIO_TERMINAL = Dados.NM_NAVIO_TERMINAL;
            log.NR_DUE_TERMINAL = Dados.NR_DUE_TERMINAL;
            log.DT_PROTOCOLADO = Dados.DT_PROTOCOLO;
            log.DT_EMBARQUE = Dados.DT_EMBARQUE;
            log.DT_DEPOSITO = Dados.DT_DEPOSITO;
            log.NR_BOOKING = Dados.NR_BOOKING;
            log.NM_NAVIO = Dados.NM_NAVIO;
            log.NR_DUE = Dados.NR_DUE;
            db.LOG_CONSULTA_TERMINAL.Add(log);
            db.SaveChanges();
        }
		public DateTime? dueDtDesembaraco(string nrDue)
        {
            DateTime? dtDesembaraco = (from due in db.DUE
                          where due.CD_NUMERO_DUE == nrDue && due.DT_CANCELADO == null && due.DT_DESEMBARACO != null
                          select due.DT_DESEMBARACO).FirstOrDefault();

            return dtDesembaraco;
        }

        public void EmbarqueConfirmado(List<int> lstVA)
        {
            foreach (var item in lstVA.Distinct())
            {
                VIAGEMARMADOR VA = db.VIAGEMARMADOR.FirstOrDefault(x => x.CD_VIAGEM_ARMADOR == item);
                if (VA != null)
                {
                    VA.IC_EMBARQUE_CONFIRMADO = true;
                    db.SaveChanges();
                }
            }
        }

        public void NotificacaoDUE(int? cdPr, int? cdDue, string strRecintoEAnt, string strRecintoE, string strRecintoDAnt, string strRecintoD, int? idUsuario, bool embarque) // ALERTA VIA EMAIL PARA ALTERAÇÃO DE RECINTO
        {
            bool enviado = false;
            LOG_NOTIFICACAO_RECINTO log = db.LOG_NOTIFICACAO_RECINTO.OrderByDescending(x=> x.CD_LOG).FirstOrDefault(x => x.CD_DUE == cdDue);

            if (log != null)
                if (log.STR_DESPACHO == strRecintoD && log.STR_DESPACHO_ANT == strRecintoDAnt && log.STR_EMBARQUE == strRecintoE && log.STR_EMBARQUE_ANT == strRecintoEAnt)
                return;

            DataBase.DUE d = db.DUE.FirstOrDefault(x => x.CD_DUE == cdDue);

                ListaAlertaRecinto lst = (from pr in db.PROCESSORESERVA
                                          join p in db.PROCESSOS on pr.CD_PROCESSO equals p.CD_PROCESSO
                                          join res in db.RESERVAS on pr.CD_RESERVA equals res.CD_RESERVA into resLeft
                                          from resOri in resLeft.DefaultIfEmpty()
                                          join via in db.VIAGEMARMADOR on resOri.CD_VIAGEM_ARMADOR equals via.CD_VIAGEM_ARMADOR into viaLeft
                                          from viaOri in viaLeft.DefaultIfEmpty()
                                          join viagem in db.VIAGEM on viaOri.CD_VIAGEM equals viagem.CD_VIAGEM into viagemLeft
                                          from viagemOri in viagemLeft.DefaultIfEmpty()
                                          join nav in db.NAVIOS on viagemOri.CD_NAVIO equals nav.CD_NAVIO into navLeft
                                          from navOri in navLeft.DefaultIfEmpty()
                                          join en in db.ENTIDADE on p.CD_CLIENTE equals en.CD_ENTIDADE
                                          where pr.CD_PROCESSORESERVA == cdPr
                                          select new ListaAlertaRecinto
                                          {
                                              NR_PROCESSO = "E-" + p.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + p.CD_NUMERO_PROCESSO.Substring(0, 2),
                                              DS_REFERENCIA = p.DS_REFERENCIA_CLIENTE,
                                              CD_PROCESSORESERVA = pr.CD_PROCESSORESERVA,
                                              CD_PROCESSO = p.CD_PROCESSO,
                                              NM_ENTIDADE = en.NM_FANTASIA_ENTIDADE,
                                              NM_NAVIO = navOri.NM_NAVIO,
                                              CD_CLIENTE = p.CD_CLIENTE
                                          }).FirstOrDefault();

                string[] Containers = (from p in db.PROCESSOS
                                       join pr in db.PROCESSORESERVA on p.CD_PROCESSO equals pr.CD_PROCESSO into prLeft
                                       from prOri in prLeft.DefaultIfEmpty()
                                       join prc in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                                       from prcOri in prcLeft.DefaultIfEmpty()
                                       join c in db.CONTAINER on prcOri.CD_CONTAINER equals c.CD_CONTAINER into cLeft
                                       from cOri in cLeft.DefaultIfEmpty()
                                       where p.CD_PROCESSO == lst.CD_PROCESSO
                                       select cOri.CD_NUMERO_CONTAINER).ToArray();

                string container = Containers[0];
                for (int i = 1; i < Containers.Length; i++)
                {
                    container += " / " + Containers[i];
                }

                string nmUsuario = (from u in db.USUARIO_CA where u.CD_USUARIO == idUsuario select u.NM_LOGIN).FirstOrDefault();

                string recintoEAnt = (from r in db.RECIALFA where r.STR_CODIGORECIALFA == strRecintoEAnt select r.STR_CODIGORECIALFA + " - " + r.STR_DESCRICAO).FirstOrDefault();
                string recintoE = (from r in db.RECIALFA where r.STR_CODIGORECIALFA == strRecintoE select r.STR_CODIGORECIALFA + " - " + r.STR_DESCRICAO).FirstOrDefault();

                string recintoDAnt = (from r in db.RECIALFA where r.STR_CODIGORECIALFA == strRecintoDAnt select r.STR_CODIGORECIALFA + " - " + r.STR_DESCRICAO).FirstOrDefault();
                string recintoD = (from r in db.RECIALFA where r.STR_CODIGORECIALFA == strRecintoD select r.STR_CODIGORECIALFA + " - " + r.STR_DESCRICAO).FirstOrDefault();

                string alteracao = "";

                if (string.IsNullOrEmpty(lst.NM_NAVIO))
                    lst.NM_NAVIO = "Navio não definido";

                string dsCorpo = "Alterar DUE e retificar no SISCOMEX:<br>";
                dsCorpo += "<b>" + lst.NR_PROCESSO + "</b>";
                dsCorpo += "<br><b>Referência:</b> " + lst.DS_REFERENCIA;
                dsCorpo += "<br><b>Container(s):</b> " + container;
                dsCorpo += "<br><b>DUE:</b> " + d.CD_NUMERO_DUE;
                dsCorpo += "<br><b>Id DUE:</b> " + cdDue;
            if ((recintoDAnt != recintoD) && (d.IC_LOCAL_EMBARQUE_TRANSPOSICAO_FRONTEIRA == null || d.IC_LOCAL_EMBARQUE_TRANSPOSICAO_FRONTEIRA == false))
            {
                dsCorpo += "<br><b>Recinto Despacho:</b> " + recintoDAnt + " <b>=></b> " + recintoD;
                alteracao = "desembaraço";
            }
            if (recintoEAnt != recintoE)
                {
                    dsCorpo += "<br><b>Recinto Embarque:</b> " + recintoEAnt + " <b>=></b> " + recintoE;

                    if (alteracao == "")
                        alteracao = "embarque";
                    else
                        alteracao = "desembaraço/embarque";
                }
                dsCorpo += "<br><b>Alterado por:</b> " + nmUsuario;

                string dsAssunto = "Alteração recinto " + alteracao + " – " + lst.NM_NAVIO + " - " + lst.NM_ENTIDADE;

                string dsEmails = ConfigurationManager.AppSettings["emailDUERecinto"].ToString();

                if (embarque)
                {

                    int? cdUsuarioResp = (from ge in db.GRUPOCLI_ENTIDADE join g in db.GRUPOCLI on ge.CD_GRUPOCLI equals g.CD_GRUPOCLI where ge.CD_ENTIDADE == lst.CD_CLIENTE && g.SG_ANALISTA == true && g.SG_SETOR == "T" select g.CD_USUARIO).FirstOrDefault();
                    if (cdUsuarioResp != null && cdUsuarioResp != 0)
                    {
                        string emailResp = (from u in db.USUARIO_CA join en in db.ENTIDADE on u.CD_ENTIDADE equals en.CD_ENTIDADE where u.CD_USUARIO == cdUsuarioResp select en.EMAIL_ENTIDADE).FirstOrDefault();
                        dsEmails += ";" + emailResp;
                    }

                string emailAlterou = (from u in db.USUARIO_CA join en in db.ENTIDADE on u.CD_ENTIDADE equals en.CD_ENTIDADE where u.CD_USUARIO == idUsuario select en.EMAIL_ENTIDADE).FirstOrDefault();
                dsEmails += ";" + emailAlterou;
                if (cdUsuarioResp == 209)
                {
                    dsEmails += ";beatrizsilva@sateldespachos.com.br;victormoreira@sateldespachos.com.br";
                }
                else if (cdUsuarioResp == 470)
                {
                    dsEmails += ";erika@sateldespachos.com.br;victormoreira@sateldespachos.com.br";
                }

            }

                Email email2 = new Email();
                email2.EnviaEmailDUE(dsEmails, "", dsAssunto, dsCorpo);

                enviado = true;

            LOG_NOTIFICACAO_RECINTO Insertlog = new LOG_NOTIFICACAO_RECINTO();
            Insertlog.CD_DUE = cdDue;
            Insertlog.CD_USUARIO = idUsuario;
            Insertlog.DT_ENVIO = DateTime.Now;
            Insertlog.IC_ENVIADO = enviado;
            Insertlog.STR_DESPACHO = strRecintoD;
            Insertlog.STR_DESPACHO_ANT = strRecintoDAnt;
            Insertlog.STR_EMBARQUE = strRecintoE;
            Insertlog.STR_EMBARQUE_ANT = strRecintoEAnt;
            db.LOG_NOTIFICACAO_RECINTO.Add(Insertlog);
            db.SaveChanges();
        }

        public List<ListaNavio> ConsultaNavioTerminal(int? cdTerminal)
        {
            DateTime dtAgora = DateTime.Now.AddDays(+30);
            return (from v in db.VIAGEM
                    join va in db.VIAGEMARMADOR on v.CD_VIAGEM equals va.CD_VIAGEM
                    join n in db.NAVIOS on v.CD_NAVIO equals n.CD_NAVIO
                    join term in db.ENTIDADE on va.CD_TERMINAL_ATRACACAO equals term.CD_ENTIDADE
                    where v.CD_NAVIO != 6661 && dtAgora > v.DT_ETA && va.DT_ATRACACAO == null && v.IC_VIAGEM_ENCERRADA == false && va.CD_TERMINAL_ATRACACAO == cdTerminal
                    select new ListaNavio
                    {
                        CD_VIAGEM = v.CD_VIAGEM,
                        CD_TERMINAL = va.CD_TERMINAL_ATRACACAO,
                        NM_TERMINAL = term.NM_FANTASIA_ENTIDADE,
                        NM_NAVIO = n.NM_NAVIO,
                        DT_ETA = v.DT_ETA,
                        NR_VIAGEM = v.CD_NUMERO_VIAGEM,
                    }).Distinct().ToList();
        }

        public void InsereConsultaNavio(ListaNavio conteudo)
        {
            TMP_CONSULTA_NAVIO tmp = new TMP_CONSULTA_NAVIO();
            tmp.CD_VIAGEM = conteudo.CD_VIAGEM;
            tmp.CD_TERMINAL = conteudo.CD_TERMINAL;
            tmp.NM_TERMINAL = conteudo.NM_TERMINAL;
            tmp.DT_ETA = conteudo.DT_ETA;
            tmp.DT_ETA_TERMINAL = conteudo.DT_ETA_TERMINAL;
            tmp.DT_ATRACACAO = conteudo.DT_ATRACACAO;
            tmp.DT_DESATRACACAO = conteudo.DT_DESATRACACAO;
            tmp.DT_ATRACACAO = conteudo.DT_ATRACACAO;
            tmp.NM_NAVIO = conteudo.NM_NAVIO;
            tmp.NR_VIAGEM = conteudo.NR_VIAGEM;
            db.TMP_CONSULTA_NAVIO.Add(tmp);
            db.SaveChanges();
        }

        public List<ListaNavio> RetornaNaviosDivergentes()
        {
            return (from tmp in db.TMP_CONSULTA_NAVIO
                    select new ListaNavio
                    {
                        CD_TERMINAL = tmp.CD_TERMINAL,
                        CD_VIAGEM = tmp.CD_VIAGEM,
                        DT_ATRACACAO = tmp.DT_ATRACACAO,
                        DT_DESATRACACAO = tmp.DT_DESATRACACAO,
                        DT_ETA = tmp.DT_ETA,
                        DT_ETA_TERMINAL = tmp.DT_ETA_TERMINAL,
                        NM_NAVIO = tmp.NM_NAVIO,
                        NM_TERMINAL = tmp.NM_TERMINAL,
                        NR_VIAGEM = tmp.NR_VIAGEM
                    }).ToList();
        }

        public void AtualizaDataETA(int? cdViagem, DateTime? dtETA)
        {
            VIAGEM v = db.VIAGEM.FirstOrDefault(x => x.CD_VIAGEM == cdViagem);
            v.DT_ETA = dtETA;
            db.SaveChanges();
        }

        public void GeraLogNavioConsultado(int? cdViagem, string NM_TERMINAL, DateTime? DT_CONSULTA, string DS_STATUS, ListaNavio Dados)
        {

            LOG_CONSULTA_NAVIO log = new LOG_CONSULTA_NAVIO();
            log.CD_VIAGEM = cdViagem;
            log.NR_VIAGEM = Dados.NR_VIAGEM;
            log.NM_NAVIO = Dados.NM_NAVIO;
            log.DT_CONSULTA = DT_CONSULTA;
            log.NM_TERMINAL = NM_TERMINAL;
            log.DS_STATUS = DS_STATUS;
            log.DT_ETA = Dados.DT_ETA;
            log.DT_ETA_TERMINAL = Dados.DT_ETA_TERMINAL;
            log.DT_ATRACACAO_TERMINAL = Dados.DT_ATRACACAO;
            log.DT_DESATRACACAO_TERMINAL = Dados.DT_DESATRACACAO;
            db.LOG_CONSULTA_NAVIO.Add(log);
            db.SaveChanges();
        }

        public ListaDeCampos atualizaDados(int? cdProcessoReserva)
        {
            return (from pr in db.PROCESSORESERVA
                    join r in db.RESERVAS on pr.CD_RESERVA equals r.CD_RESERVA
                    join va in db.VIAGEMARMADOR on r.CD_VIAGEM_ARMADOR equals va.CD_VIAGEM_ARMADOR
                    join v in db.VIAGEM on va.CD_VIAGEM equals v.CD_VIAGEM
                    join n in db.NAVIOS on v.CD_NAVIO equals n.CD_NAVIO
                    where pr.CD_PROCESSORESERVA == cdProcessoReserva
                    select new ListaDeCampos
                    {
                        NR_BOOKING = r.CD_NUMERO_RESERVA,
                        NM_NAVIO = n.NM_NAVIO
                    }).FirstOrDefault();
        }

        public int[] retornaCodigoMinerva()
        {
            return (from g in db.GRUPOCLI_ENTIDADE where g.CD_GRUPOCLI == 1 select g.CD_ENTIDADE).ToArray();
        }

        public string retornaSIF(int? cdEntidade)
        {
            return (from e in db.RELAC_EXPORTADOR where e.CD_ENTIDADE == cdEntidade select e.CD_SIF).FirstOrDefault();
        }

        public string retornaCliente(int? cdEntidade)
        {
            return (from e in db.ENTIDADE where e.CD_ENTIDADE == cdEntidade select e.NM_FANTASIA_ENTIDADE).FirstOrDefault();
        }

        public void AtualizaLacreValidado(int? cdPRC)
        {
            PROCESSORESERVACONTAINER prc = db.PROCESSORESERVACONTAINER.FirstOrDefault(x => x.CD_PROCESSORESERVACONTAINER == cdPRC);
            prc.IC_LACRES_CONFERIDOS = true;
            db.SaveChanges();
        }

        public bool retornaLacreValidado(int? cdPRC)
        {
            return (from prc in db.PROCESSORESERVACONTAINER where prc.CD_PROCESSORESERVACONTAINER == cdPRC select prc.IC_LACRES_CONFERIDOS).FirstOrDefault() ?? false;
        }

        public string retornaPaisDestino(int? cdProcesso)
        {
            return (from pr in db.PROCESSORESERVA 
                    join r in db.RESERVAS on pr.CD_RESERVA equals r.CD_RESERVA
                    join p in db.PAIS on r.CD_PAIS_RESERVA equals p.CD_PAIS
                    where pr.CD_PROCESSO == cdProcesso
                    select p.STR_NOMEPAIS).FirstOrDefault();
        }

        public void GeraLogLacreDivergente(InsereDados dados, int? cd_entidade)
        {
            LOG_DIVERGENCIA_LACRE log_diver_lacre = new LOG_DIVERGENCIA_LACRE();

            log_diver_lacre.CD_CLIENTE = cd_entidade;
            log_diver_lacre.DS_LACRE_AGENCIA = dados.DS_LACRE_AGENCIA;
            log_diver_lacre.DS_LACRE_AGENCIA_TERMINAL = dados.DS_LACRE_AGENCIA_TERMINAL;
            log_diver_lacre.DS_LACRE_SIF = dados.DS_LACRE_SIF;
            log_diver_lacre.DS_LACRE_SIF_TERMINAL = dados.DS_LACRE_SIF_TERMINAL;
            log_diver_lacre.DS_REFERENCIA = dados.DS_REFERENCIA_CLIENTE;
            log_diver_lacre.NM_TERMINAL = dados.NM_TERMINAL;
            log_diver_lacre.NR_BOOKING = dados.NR_BOOKING;
            log_diver_lacre.NR_CONTAINER = dados.NR_CONTAINER;
            log_diver_lacre.NR_PROCESSO = dados.CD_NUMERO_PROCESSO;
            log_diver_lacre.DT_LOG = DateTime.Now;

            db.LOG_DIVERGENCIA_LACRE.Add(log_diver_lacre);
            db.SaveChanges();


        }



    }
}
