using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using TerminalRobo.DataBase;

namespace roboEDI.Model
{
    class Email
    {
        WEBSATELEntities db = new WEBSATELEntities();

        private string dsAssunto;
        private string dsCorpoEmail;
        private List<string> anexos = new List<string>();
        public string From;

        public bool DisparaEmailDraft(string ConteudoEmail, int? cliente, int? cdProcesso, string dsAssunto)
        {
            try
            {
                List<string> EmailResp;
                string EmailResponsaveis;
             
                    int? cdGrupo = (from p in db.PROCESSOS
                                    join u in db.USUARIO_CA on p.CD_RESPONSAVEL_BL equals u.CD_USUARIO into uLeft
                                    from uOri in uLeft.DefaultIfEmpty()
                                    join e in db.GRUPOS_EMAIL on uOri.CD_USUARIO equals e.CD_USUARIO into eLeft
                                    from eOri in eLeft.DefaultIfEmpty()
                                    where p.CD_PROCESSO == cdProcesso
                                    select eOri.CD_GRUPO).FirstOrDefault();

                    if (cdGrupo == null)
                    {
                        int? Usuariox = (from g in db.GRUPOCLI
                                         join ge in db.GRUPOCLI_ENTIDADE on g.CD_GRUPOCLI equals ge.CD_GRUPOCLI
                                         where ge.CD_ENTIDADE == cliente && g.SG_ANALISTA == true && g.SG_SETOR == "B"
                                         select g.CD_USUARIO).FirstOrDefault();

                        cdGrupo = (from gp in db.GRUPOS_EMAIL where gp.CD_USUARIO == Usuariox select gp.CD_GRUPO).FirstOrDefault();

                    }


                    


                    EmailResp = (from g in db.GRUPOS_EMAIL
                                 where g.CD_TIPO_GRUPO == 1 && g.CD_GRUPO == cdGrupo
                                 select g.DS_EMAIL).ToList();


                    if (EmailResp.Count == 0)
                    {
                        EmailResponsaveis = "suporte@sateldespachos.com.br;thyagosantos@sateldespachos.com.br;draftblminerva@sateldespachos.com.br";
                        ConteudoEmail += "<br><br>Não foi possível encontrar o e-mail do responsável do processo em questão.";
                    }
                    else
                    {
                        EmailResponsaveis = string.Join(";", EmailResp);
                    }


                    LOG_EMAILS_ENVIADOS insertLog = new LOG_EMAILS_ENVIADOS();
                    insertLog.DS_ASSUNTO = dsAssunto;
                    insertLog.DS_MENSAGEM = ConteudoEmail;
                    insertLog.DT_ENVIO = DateTime.Now;              
                    insertLog.DS_EMAIL_DESTINO = EmailResponsaveis;
                    db.LOG_EMAILS_ENVIADOS.Add(insertLog);
                    db.SaveChanges();
                
                Email EnviaEmail = new Email();
                EnviaEmail.EnviaEmailDUE(EmailResponsaveis,"", dsAssunto, ConteudoEmail);
                return true;
            }
            catch (Exception)
            {
                LOG_EMAILS_ENVIADOS insertLog = new LOG_EMAILS_ENVIADOS();
                insertLog.DS_ASSUNTO = "FALHA NO ENVIO: " + dsAssunto;
                insertLog.DS_MENSAGEM = ConteudoEmail;
                insertLog.DT_ENVIO = DateTime.Now;              
                db.LOG_EMAILS_ENVIADOS.Add(insertLog);
                db.SaveChanges();
                return false;
            }

        }
        public void EnviaEmailDUE(string to, string copiaEmail, string sAssunto, string dsMsg)
        {
            dsAssunto = sAssunto;
            dsCorpoEmail = dsMsg;
            From = ConfigurationManager.AppSettings["emailCliente"].ToString();
            anexos.Clear();

            //copiaEmail
            EnviarEmailComAnexo(to, copiaEmail);
        }

       
        public void EnviaEmailEDI(string to,string copiaEmail, string sAssunto, string dsMsg, string arquivo)
        {
            dsAssunto = sAssunto;
            dsCorpoEmail = dsMsg;
            From = ConfigurationManager.AppSettings["emailCliente"].ToString();
            anexos.Clear();
            anexos.Add(arquivo);
            //to += ";fabricioisidro@sateldespachos.com.br";

            //copiaEmail
            EnviarEmailComAnexo(to, copiaEmail);
        }


        private String EnviarEmailComAnexo(string to, string cc)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                #region Propriedade de Web.Config
                string emailCliente = ConfigurationManager.AppSettings["emailCliente"].ToString();
                string senhaCliente = ConfigurationManager.AppSettings["senhaCliente"].ToString();
                string ServidorEntradaSaida = ConfigurationManager.AppSettings["ServidorEntradaSaida"].ToString();
                #endregion

                #region Metodo de envio
                DateTime _dtNow = DateTime.UtcNow;
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(ConfigurationManager.AppSettings["emailSistema"].ToString());

                foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(new MailAddress(address));
                }

                message.Subject = dsAssunto;
                message.IsBodyHtml = true;
                message.Body = dsCorpoEmail;

                string[] sCC = cc.Split(';');

                for (int x = 0; x <= sCC.Length - 1; x++)
                {
                    if (sCC[x] != "")
                        message.CC.Add(sCC[x]);
                }

                foreach (var item in anexos)
                {
                    string attachmentFilename = item.ToString();
                    System.Net.Mail.Attachment itemx = new System.Net.Mail.Attachment(attachmentFilename);

                    message.Attachments.Add(itemx);
                }

                smtp.Port = 587;
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailSistema"].ToString(), senhaCliente);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
#endregion

                /* METODO ANTERIOR
                #region Metodo de Envio de Email
                DateTime _dtNow = DateTime.UtcNow;

                ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
                myservice.Credentials = new WebCredentials(emailCliente, senhaCliente);
                myservice.Url = new Uri(ServidorEntradaSaida);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                EmailMessage message = new EmailMessage(myservice);

                message.Subject = dsAssunto;
                message.Body = dsCorpoEmail;

                string[] sPara = to.Split(';');

                for (int x = 0; x <= sPara.Length - 1; x++)
                {
                    if (sPara[x] != "")
                        message.ToRecipients.Add(sPara[x]);
                }

                string[] sCC = cc.Split(';');

                for (int x = 0; x <= sCC.Length - 1; x++)
                {
                    if (sCC[x] != "")
                        message.CcRecipients.Add(sCC[x]);
                }

                foreach (var item in anexos)
                {
                    string attachmentFilename = item.ToString();
                    message.Attachments.AddFileAttachment(attachmentFilename);
                }

                message.Send();
                #endregion Metodo de Envio de Email
                */

                /*  #region Propriedade de Web.Config
                string emailCliente = ConfigurationSettings.AppSettings["emailCliente"].ToString();
                string senhaCliente = ConfigurationSettings.AppSettings["senhaCliente"].ToString();
                string PortaEntrada = ConfigurationSettings.AppSettings["PortaEntrada"].ToString();
                string PortaSaida = ConfigurationSettings.AppSettings["PortaSaida"].ToString();
                string ServidorEntradaSaida = ConfigurationSettings.AppSettings["ServidorEntradaSaida"].ToString();
                string Autentica = ConfigurationSettings.AppSettings["Autenticacao"].ToString();
                #endregion

                #region Metodo de Envio de Email
                DateTime _dtNow = DateTime.UtcNow;

                MailMessage mailMessage = new MailMessage();

                if (From == "" || From == null)
                {
                    From = emailCliente;
                }

                mailMessage.From = new MailAddress(From.Replace(';', ','));

                string[] sPara = to.Split(';');

                for (int x = 0; x <= sPara.Length - 1; x++)
                {
                    if (sPara[x] != "")
                        mailMessage.To.Add(sPara[x]);
                }

                string[] sCC = cc.Split(';');

                for (int x = 0; x <= sCC.Length - 1; x++)
                {
                    if (sCC[x] != "")
                        mailMessage.CC.Add(sCC[x]);
                }

                //if (!String.IsNullOrEmpty(Cc))
                mailMessage.Subject = dsAssunto;

                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = dsCorpoEmail;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.Priority = MailPriority.Normal;

                foreach (var item in anexos)
                {
                    string attachmentFilename = item.ToString();
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    mailMessage.Attachments.Add(attachment);

                    //mailMessage.Attachments.Add(new Attachment(item.ToString()));
                }

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ServidorEntradaSaida;
                smtpClient.Port = int.Parse(PortaSaida);
                if (Autentica == "1") //Caso precise de autenticação
                {
                    smtpClient.Credentials = new NetworkCredential(emailCliente, senhaCliente);
                }

                smtpClient.Send(mailMessage);

                mailMessage.Dispose();
                #endregion
                */
                return "Dados enviado com sucesso.";
            }
            catch (Exception ex) { return "Nao foi possivel enviar dados. Tente mais tarde " + ex.Message; }
        }
        /*
        private String EnviarEmailComAnexo(string to, string cc)
        {
            try
            {
                #region Propriedade de Web.Config
                string emailCliente = ConfigurationSettings.AppSettings["emailCliente"].ToString();
                string senhaCliente = ConfigurationSettings.AppSettings["senhaCliente"].ToString();
                string PortaEntrada = ConfigurationSettings.AppSettings["PortaEntrada"].ToString();
                string PortaSaida = ConfigurationSettings.AppSettings["PortaSaida"].ToString();
                string ServidorEntradaSaida = ConfigurationSettings.AppSettings["ServidorEntradaSaida"].ToString();
                string Autentica = ConfigurationSettings.AppSettings["Autenticacao"].ToString();
                #endregion

                #region Metodo de Envio de Email
                DateTime _dtNow = DateTime.UtcNow;

                MailMessage mailMessage = new MailMessage();

                if (From == "" || From == null)
                {
                    From = emailCliente;
                }

                mailMessage.From = new MailAddress(From.Replace(';', ','));

                string[] sPara = to.Split(';');

                for (int x = 0; x <= sPara.Length - 1; x++)
                {
                    if (sPara[x] != "")
                        mailMessage.To.Add(sPara[x]);
                }

                string[] sCC = cc.Split(';');

                for (int x = 0; x <= sCC.Length - 1; x++)
                {
                    if (sCC[x] != "")
                        mailMessage.CC.Add(sCC[x]);
                }

                //if (!String.IsNullOrEmpty(Cc))
                mailMessage.Subject = dsAssunto;

                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = dsCorpoEmail;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.Priority = MailPriority.Normal;

                foreach (var item in anexos)
                {
                    string attachmentFilename = item.ToString();
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    mailMessage.Attachments.Add(attachment);

                    //mailMessage.Attachments.Add(new Attachment(item.ToString()));
                }

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ServidorEntradaSaida;
                smtpClient.Port = int.Parse(PortaSaida);
                if (Autentica == "1") //Caso precise de autenticação
                {
                    smtpClient.Credentials = new NetworkCredential(emailCliente, senhaCliente);
                }

                smtpClient.Send(mailMessage);

                mailMessage.Dispose();
                #endregion

                return "Dados enviado com sucesso.";
            }
            catch (Exception ex) { return "Nao foi possivel enviar dados. Tente mais tarde " + ex.Message; }
        }

        */
    }
}
