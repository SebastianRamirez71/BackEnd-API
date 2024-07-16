using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Net.Mail;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly PPSContext _context;
        public string ErrorDescription { get; set; }

        public EmailService(PPSContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
            ErrorDescription = string.Empty;
        }
        

        public void Notify(Product product)
        {
            try
            {
                foreach (var user in _context.Users.Where(x => x.Notification == true))
                {
                    SendEmail(product, user);
                }
            }
            catch (Exception ex)
            {

                ErrorDescription = "Ocurrio un problema - Notify" + ex.Message;
            }
           
        }
        public void SendEmail(Product product, User user)
        {
            try
            {
                string senderEmail = "rss.store.tup@gmail.com";
                string senderPass = "zzmd vtyl khwe jttz";
                string to = user.Email;

                string subject = $"Hola {user.Name} tenemos ¡Nuevo producto en oferta!";
                string htmlTemplate = @"<!DOCTYPE html PUBLIC """"""""-//W3C//DTD XHTML 1.0 Transitional//EN"""""""" """"""""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"""""""">
<html xmlns=""""""""http://www.w3.org/1999/xhtml"""""""" xmlns:v=""""""""urn:schemas-microsoft-com:vml"""""""" xmlns:o=""""""""urn:schemas-microsoft-com:office:office"""""""" lang=""""""""en"""""""">
<head>
<title></title>
<meta charset=""""""""UTF-8"""""""" />
<meta http-equiv=""""""""Content-Type"""""""" content=""""""""text/html; charset=UTF-8"""""""" />
<!--[if !mso]>-->
<meta http-equiv=""""""""X-UA-Compatible"""""""" content=""""""""IE=edge"""""""" />
<!--<![endif]-->
<meta name=""""""""x-apple-disable-message-reformatting"""""""" content="""""""""""""""" />
<meta content=""""""""target-densitydpi=device-dpi"""""""" name=""""""""viewport"""""""" />
<meta content=""""""""true"""""""" name=""""""""HandheldFriendly"""""""" />
<meta content=""""""""width=device-width"""""""" name=""""""""viewport"""""""" />
<meta name=""""""""format-detection"""""""" content=""""""""telephone=no, date=no, address=no, email=no, url=no"""""""" />
<style type=""""""""text/css"""""""">
/* Your existing styles here */
</style>
<style type=""""""""text/css"""""""">
@media (min-width: 481px) {
.hd { display: none!important }
}
</style>
<style type=""""""""text/css"""""""">
@media (max-width: 480px) {
.hm { display: none!important }
}
</style>
<style type=""""""""text/css"""""""">
@media (min-width: 481px) {
/* Your existing styles here */
}
</style>
<style type=""""""""text/css"""""""" media=""""""""screen and (min-width:481px)"""""""">
.img{
    width:""""100px"""";
}
}
</style>
<!--[if !mso]>-->
<link href=""""""""https://fonts.googleapis.com/css2?family=Albert+Sans:wght@400;500;600&amp;display=swap"""""""" rel=""""""""stylesheet"""""""" type=""""""""text/css"""""""" />
<!--<![endif]-->
<!--[if mso]>
<style type=""""""""text/css"""""""">
/* Your existing styles here */
</style>
<![endif]-->
<!--[if mso]>
<xml>
<o:OfficeDocumentSettings>
<o:AllowPNG/>
<o:PixelsPerInch>96</o:PixelsPerInch>
</o:OfficeDocumentSettings>
</xml>
<![endif]-->
</head>
<body id=""""""""body"""""""" class=""""""""t22"""""""" style=""""""""min-width:100%;Margin:0px;padding:0px;background-color:#B0B5A5;""""""""><div class=""""""""t21"""""""" style=""""""""background-color:#B0B5A5;""""""""><table role=""""""""presentation"""""""" width=""""""""100%"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" border=""""""""0"""""""" align=""""""""center""""""""><tr><td class=""""""""t20"""""""" style=""""""""font-size:0;line-height:0;mso-line-height-rule:exactly;background-color:#B0B5A5;"""""""" valign=""""""""top"""""""" align=""""""""center"""""""">
<!--[if mso]>
<v:background xmlns:v=""""""""urn:schemas-microsoft-com:vml"""""""" fill=""""""""true"""""""" stroke=""""""""false"""""""">
<v:fill color=""""""""#B0B5A5""""""""/>
</v:background>
<![endif]-->
<table role=""""""""presentation"""""""" width=""""""""100%"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" border=""""""""0"""""""" align=""""""""center"""""""" id=""""""""innerTable""""""""><tr><td><div class=""""""""t4"""""""" style=""""""""mso-line-height-rule:exactly;font-size:1px;display:none;"""""""">&nbsp;&nbsp;</div></td></tr><tr><td>
<!--[if mso]>
<table class=""""""""t6"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" align=""""""""center"""""""">
<![endif]-->
<!--[if !mso]>-->
<table class=""""""""t6"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" style=""""""""Margin-left:auto;Margin-right:auto;"""""""">
<!--<![endif]-->
<tr>
<!--[if mso]>
<td width=""""""""630"""""""" class=""""""""t5"""""""" style=""""""""background-color:#F5F1E6;padding:30px 30px 30px 30px;"""""""">
<![endif]-->
<!--[if !mso]>-->
<td class=""""""""t5"""""""" style=""""""""background-color:#F5F1E6;width:420px;padding:30px 30px 30px 30px;"""""""">
<!--<![endif]-->
<table role=""""""""presentation"""""""" width=""""""""100%"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0""""""""><tr><td>
<!--[if mso]>
<table class=""""""""t2"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" align=""""""""left"""""""">
<![endif]-->
<!--[if !mso]>-->
<table class=""""""""t2"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" style=""""""""Margin-right:auto;"""""""">
<!--<![endif]-->
<tr>
<!--[if mso]>
<td width=""""""""540"""""""" class=""""""""t1"""""""">
<![endif]-->
<!--[if !mso]>-->
<td class=""""""""t1"""""""" style=""""""""width:420px;"""""""">
<!--<![endif]-->
<h1 class=""""""""t0"""""""" style=""""""""margin:0;Margin:0;font-family:Albert Sans,BlinkMacSystemFont,Segoe UI,Helvetica Neue,Arial,sans-serif;line-height:28px;font-weight:600;font-style:normal;font-size:30px;text-decoration:none;text-transform:none;letter-spacing:-1.35px;direction:ltr;color:#333333;text-align:left;mso-line-height-rule:exactly;mso-text-raise:-1px;"""""""">¡Descubre Nuestro Nuevo Producto en Oferta!</h1></td>
</tr></table>
</td></tr><tr><td><div class=""""""""t3"""""""" style=""""""""mso-line-height-rule:exactly;mso-line-height-alt:20px;line-height:20px;font-size:1px;display:block;"""""""">&nbsp;&nbsp;</div></td></tr></table></td>
</tr></table>
</td></tr><tr><td>
<!--[if mso]>
<table class=""""""""t19"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" align=""""""""center"""""""">
<![endif]-->
<!--[if !mso]>-->
<table class=""""""""t19"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" style=""""""""Margin-left:auto;Margin-right:auto;"""""""">
<!--<![endif]-->
<tr>
<!--[if mso]>
<td width=""""""""630"""""""" class=""""""""t18"""""""" style=""""""""background-color:#F5F1E6;padding:30px 30px 30px 30px;"""""""">
<![endif]-->
<!--[if !mso]>-->
<td class=""""""""t18"""""""" style=""""""""background-color:#F5F1E6;width:420px;padding:30px 30px 30px 30px;"""""""">
<!--<![endif]-->
<table role=""""""""presentation"""""""" width=""""""""100%"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0""""""""><tr><td>
<!--[if mso]>
<table class=""""""""t9"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" align=""""""""center"""""""">
<![endif]-->
<!--[if !mso]>-->
<table class=""""""""t9"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" style=""""""""Margin-left:auto;Margin-right:auto;"""""""">
<!--<![endif]-->
<tr>
<!--[if mso]>
<td width=""""""""540"""""""" class=""""""""t8"""""""">
<![endif]-->
<!--[if !mso]>-->
<td class=""""""""t8"""""""" style=""""""""width:420px;"""""""">
<!--<![endif]-->
<p class=""""t7"""" style=""""margin:0;Margin:0;font-family:Albert Sans,BlinkMacSystemFont,Segoe UI,Helvetica Neue,Arial,sans-serif;line-height:24px;font-weight:500;font-style:normal;font-size:18px;text-decoration:none;text-transform:none;letter-spacing:-0.66px;direction:ltr;color:#000000;text-align:center;mso-line-height-rule:exactly;mso-text-raise:2px;"""">
    
<a href=""http://localhost:3000/product/{productPath}"" target=""_blank"" rel=""noopener noreferrer"">
    <img src=""{productImg}"" alt=""Descripción de la imagen"" class=""img"" style=""width:100px;"">
</a>

</p>
<p>{productDescription} | ${productPrice} | {productGenre}</p>
</td>
</tr></table>
</td></tr><tr><td>
<!--[if mso]>
<table class=""""""""t13"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" align=""""""""center"""""""">
<![endif]-->
<!--[if !mso]>-->
<table class=""""""""t13"""""""" role=""""""""presentation"""""""" cellpadding=""""""""0"""""""" cellspacing=""""""""0"""""""" style=""""""""Margin-left:auto;Margin-right:auto;"""""""">
<!--<![endif]-->
<tr>
<!--[if mso]>
<td width=""""""""540"""""""" class=""""""""t12"""""""">
<![endif]-->
<!--[if !mso]>-->
<td class=""""""""t12"""""""" style=""""""""width:420px;"""""""">
<!--<![endif]-->
<p class=""""""""t11"""""""" style=""""""""margin:0;Margin:0;font-family:Albert Sans,BlinkMacSystemFont,Segoe UI,Helvetica Neue,Arial,sans-serif;line-height:22px;font-weight:500;font-style:normal;font-size:16px;text-decoration:none;text-transform:none;letter-spacing:-0.44px;direction:ltr;color:#000000;text-align:center;mso-line-height-rule:exactly;mso-text-raise:1px;"""""""">Visita nuestros nuevos productos en nuestro catalogo. <a href=""http://localhost:3000/home"" target=""_blank"" rel=""noopener noreferrer"">
    <img src=""https://dcdn.mitiendanube.com/stores/001/990/290/themes/common/logo-1889664424-1714051930-78818b5f4cbb4833eec760c042855ff01714051930-320-0.webp"" alt=""Home"" class=""img"" style=""width:100px;"">
</a></p></td>
</tr></table>
</td></tr></table>
</td>
</tr></table>
</td></tr></table>
</div>
</body>
</html>""";


                string htmlBody = htmlTemplate
                                .Replace("{productPath}", product.Description)
                                .Replace("{productImg}", product.Image)
                                .Replace("{productDescription}", product.Description)
                                .Replace("{productPrice}", product.Price.ToString())
                                .Replace("{productGenre}", product.Genre);

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {

                ErrorDescription = "Ocurrio un problema - SendEmail" + ex.Message;
            }
        }

        public void Subscribe(string email)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == email);
                user.Notification = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                ErrorDescription = "Ocurrio un problema - Subscribe" + ex.Message;
            }
           
        }

        public void UnSubscribe(string email)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == email);
                user.Notification = false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                ErrorDescription = "Ocurrio un problema - UnSubscribe" + ex.Message;
            }
           
        }
    }
}
    
