using System;
using System.Linq;
using System.Web.Mvc;
using MusicStoree.Models;
using MusicStoree.EFContext;
using System.Net.Mail;
using System.Net;

namespace MusicStoree.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        MusicStoreDB db = new MusicStoreDB();
        const string PromoCode = "FREE";
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
         

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult SendMailToUser(string Email,string FirstName)
        {
            Order user = new Order {Email = Email, FirstName=FirstName};
            bool result = false;
            result = SendEmail(user.Email=Email, "Faleminderit per Blerjen Tuaj!", "<p>Përshendetje " + FirstName + ",<br />Porosia juaj u dergua me sukses.<br /> <br /> Ju mirëpresim sërisht, <br />MusicStore Teams </p>");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SendEmail(string toEmail, string subject, string emailBody)
        {

            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.UTF8Encoding.UTF8;
                client.Send(mailMessage);

                return true;

            }
            catch (Exception)
            {
                return false;

            }

        }
    }
}