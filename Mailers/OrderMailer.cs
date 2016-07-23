using System.Net.Mail;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.Notification;
using Microsoft.Practices.ServiceLocation;
using Mvc.Mailer;

namespace livemenu.Mailers
{
    public class OrderMailer : MailerBase, IOrderMailer 	
	{
        private readonly EmailSenderServerSettings _emailSettings;

        public OrderMailer(EmailSenderServerSettings emailSettings)
        {
            _emailSettings = emailSettings;
            MasterName="_Layout";
        }

        public virtual MvcMailMessage OrderCreated(EmailMailerModel<Order> model)
        {
          
            ViewData.Model = model.Object;
			var message = Populate(x =>
			{
                x.From = new MailAddress(_emailSettings.UserName);

			    x.IsBodyHtml = true;

				x.Subject = "Поступил новый заказ";
                x.ViewName = "OrderCreated";

                foreach (var to in model.To)
			    {
                    x.To.Add(to);
			    }
		
			});

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
		}

        public MvcMailMessage ClientOrderCreated(EmailMailerModel<Order> model)
        {
            var mcs = ServiceLocator.Current.GetInstance<IMainConfigService>();
            var isRuEn = mcs.ProjectID.ToString() == "43d4606f-d72d-479f-940f-2f1a5fd1a857";

            var theme = !isRuEn ? "Заказ оформлен" : "Заказ оформлен/The order is confirmed";
            var template = !isRuEn ? "ClientOrderCreated" : "ClientOrderCreatedRuEn";
            
            ViewData.Model = model.Object;
            var message = Populate(x =>
            {
                x.From = new MailAddress(_emailSettings.UserName);

                x.IsBodyHtml = true;

                x.Subject = theme;
                x.ViewName = template;

                foreach (var to in model.To)
                {
                    x.To.Add(to);
                }

            });

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
        }

        public virtual MvcMailMessage OrderIncomeRecived(EmailMailerModel<Order> model)
        {
            ViewData.Model = model.Object;
            var message = Populate(x =>
            {
                x.From = new MailAddress(_emailSettings.UserName);

                x.IsBodyHtml = true;

                x.Subject = "Оплата получена";
                x.ViewName = "OrderIncomeRecieved";

                foreach (var to in model.To)
                {
                    x.To.Add(to);
                }

            });

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
        }

        public MvcMailMessage ClientOrderIncomeRecived(EmailMailerModel<Order> model)
        {
            ViewData.Model = model.Object;
            var message = Populate(x =>
            {
                x.From = new MailAddress(_emailSettings.UserName);

                x.IsBodyHtml = true;

                x.Subject = "Оплата получена";
                x.ViewName = "ClientOrderIncomeRecived";

                foreach (var to in model.To)
                {
                    x.To.Add(to);
                }

            });

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
        }

        


        public MvcMailMessage ClientActivationRequest(EmailMailerModel<Client> model)
        {
            ViewData.Model = model.Object;
            var message = Populate(x =>
            {
                x.From = new MailAddress(_emailSettings.UserName);

                x.IsBodyHtml = true;

                x.Subject = "Подтверждение регистрации";
                x.ViewName = "ClientActivationRequest";

                foreach (var to in model.To)
                {
                    x.To.Add(to);
                }

            });

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
        }

        public MvcMailMessage ClientFourDigitActivationRequest(EmailMailerModel<Client> model)
        {
            var mcs = ServiceLocator.Current.GetInstance<IMainConfigService>();
            var isRuEn = mcs.ProjectID.ToString() == "43d4606f-d72d-479f-940f-2f1a5fd1a857";

            var theme = !isRuEn ? "Подтверждение регистрации" : "Подтверждение регистрации/Confirmation of registration";
            var template = !isRuEn ? "ClientFourDigitActivationRequest" : "ClientFourDigitActivationRequestRuEn";


            ViewData.Model = model.Object;
            var message = Populate(x =>
            {
                x.From = new MailAddress(_emailSettings.UserName);

                x.IsBodyHtml = true;

                x.Subject = theme;
                x.ViewName = template;

                foreach (var to in model.To)
                {
                    x.To.Add(to);
                }

            });

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
        }
        

        public MvcMailMessage ClientForgotPassword(EmailMailerModel<Client> model)
        {
            var mcs = ServiceLocator.Current.GetInstance<IMainConfigService>();
            var isRuEn = mcs.ProjectID.ToString() == "43d4606f-d72d-479f-940f-2f1a5fd1a857";

            var theme = !isRuEn ? "Восстановление пароля" : "Восстановление пароля/Password Recovery";
            var template = !isRuEn ? "ClientForgotPassword" : "ClientForgotPasswordRuEn";


            ViewData.Model = model.Object;
            var message = Populate(x =>
            {
                x.From = new MailAddress(_emailSettings.UserName);

                x.IsBodyHtml = true;

                x.Subject = theme;
                x.ViewName = template;

                foreach (var to in model.To)
                {
                    x.To.Add(to);
                }

            });

            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body).Html;
            return message;
        }
        
	}
}