using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.SMTP
{
    public class SMTPDataModel
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> AttachmentFilenames { get; set; }
    }

    public class JWSmtpFactory
    {
        public static JWSmtpHelper CreateSMTPHelper(string connection, string userid, string password, int port)
        {
            return new JWSmtpHelper(connection, userid, password, port);
        }
    }

    public class JWSmtpHelper
    {
        string connection;
        string userid;
        string password;
        int port;

        public List<SMTPDataModel> WorkList
        {
            get { return workList; }
        }

        List<SMTPDataModel> workList = new List<SMTPDataModel>();

        public event Action<object, SMTPDataModel> SendResultEvent;

        public JWSmtpHelper(string connection, string userid, string password, int port)
        {
            this.connection = connection;
            this.userid = userid;
            this.password = password;
            this.port = port;
        }

        public JWSmtpHelper AddWork(SMTPDataModel model)
        {
            workList.Add(model);
            return this;
        }

        public void BeginSend()
        {
            foreach (var item in workList)
            {
                Send(item);
                if (SendResultEvent != null)
                    SendResultEvent(this, item);
            }

            workList.Clear();
        }

        private void Send(SMTPDataModel model)
        {
            var smtp = new JWSmtp(connection, userid, password, port);

            if (smtp != null)
            {
                smtp.SendEmail(
                    model.Name, model.From, model.To,
                    model.Body, model.Body,
                    model.AttachmentFilenames == null ? null : model.AttachmentFilenames.ToArray());

                IDisposable dispose = smtp as IDisposable;

                if (dispose != null) dispose.Dispose();
            }
        }
    }

    public class JWSmtp : IDisposable
    {
        public string SmtpConnection { get; private set; }
        public string Userid { get; private set; }
        public string Passwd { get; private set; }
        public int Port { get; private set; }
        public string AttFileName { get; private set; }

        private SmtpClient objSmtpClient;

        public JWSmtp(string connection, string userid, string password, int port)
        {
            this.SmtpConnection = connection;
            this.Userid = userid;
            this.Passwd = password;
            this.Port = port;

            objSmtpClient = new SmtpClient(SmtpConnection); // Server IP
            objSmtpClient.Port = Port;
            objSmtpClient.UseDefaultCredentials = false;
            objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            System.Net.NetworkCredential objSMTPUserInfo = new System.Net.NetworkCredential();
            objSMTPUserInfo = new System.Net.NetworkCredential(Userid, Passwd);
            objSmtpClient.Credentials = objSMTPUserInfo;
        }

        public void SendEmail(string name
                            , string strFrom
                            , string strTo
                            , string strSubject
                            , string strBody
                            , string[] attachmentFilenames)
        {
            MailMessage objMailMessage = new MailMessage();

            try
            {
                objMailMessage.From = new MailAddress(strFrom, name);
                objMailMessage.To.Add(new MailAddress(strTo));
                objMailMessage.BodyEncoding = Encoding.UTF8;//Encoding.GetEncoding("EUC-KR");                
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Subject = strSubject;
                objMailMessage.Body = strBody;

                string filePath = string.Empty;
                foreach (string item in attachmentFilenames)
                {
                    filePath = "";
                    if (!string.IsNullOrEmpty(item))
                    {
                        filePath = AttFileName + item;

                        if (File.Exists(filePath))
                        {
                            System.Net.Mail.Attachment attachment;
                            attachment = new System.Net.Mail.Attachment(filePath);
                            objMailMessage.Attachments.Add(attachment);
                        }
                    }
                }
                objSmtpClient.Send(objMailMessage);
            }
            catch (SmtpException ex)
            {
                throw;
            }
            finally
            {
                objMailMessage.Dispose();
                objMailMessage = null;
            }
        }

        public void Dispose()
        {
			objSmtpClient.Dispose();
        }
    }
}
