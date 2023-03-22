using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Vitro.Controllers
{
    [Authorize(Roles = "Administrador,Mercadotecnia")]
    public class MailController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(Models.MailViewModel model)
        {
            string query = @"SELECT U.FullName, U.Email FROM ASPNETUSERS U INNER JOIN ASPNETUSERROLES UR ON UR.USERID = U.ID INNER JOIN ASPNETROLES R ON R.ID = UR.ROLEID WHERE LOWER(R.NAME)=@rol";
            SqlParameter parameter = new SqlParameter("rol", "cliente");
            var datamail = db.Database.SqlQuery<Models.MailDataViewModel>(query, parameter);
            var configuracion = db.MailConfigs.FirstOrDefault();
            List<string> copiar = new List<string>();
            if (!string.IsNullOrEmpty(model.Bcc))
            {
                copiar = model.Bcc.Split(',').ToList();
            }
            //string[] copiar = model.Bcc.Split(',');

            using (MailMessage mail = new MailMessage())
            {
                foreach (var info in datamail)
                {
                    mail.To.Add(new MailAddress(info.Email));
                }
                mail.Subject = model.Subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = model.Message;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                mail.From = new MailAddress(configuracion.MailAccount);
                foreach (string copia in copiar)
                {
                    mail.Bcc.Add(new MailAddress(copia));
                }
                foreach (var file in model.Files)
                {
                    if (file != null)
                    {
                        mail.Attachments.Add(new Attachment(file.InputStream, file.FileName));
                    }
                }
                using (SmtpClient smtp = new SmtpClient(configuracion.Host))
                {
                    smtp.EnableSsl = configuracion.HabilitarSSL.Value;
                    smtp.Port = configuracion.Puerto;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Host = configuracion.Host;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(configuracion.MailAccount, configuracion.MailPassword);
                    try
                    {
                        smtp.Send(mail);
                        TempData["Message"] = $"Mensaje de correo electronico enviado con exito";
                        TempData["MessageType"] = "bg-green fg-white";
                    }
                    catch (SmtpException error)
                    {
                        TempData["Message"] = error.Message;
                        TempData["MessageType"] = "bg-red fg-white";
                        System.Diagnostics.Debug.WriteLine(error.Message);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}