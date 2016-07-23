using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.CRM.Clients;
using livemenu.Kernel.CRM.Orders;
using livemenu.Kernel.Notification;
using livemenu.Mailers;
using Microsoft.Practices.ServiceLocation;
using Mvc.Mailer;

namespace livemenu.Areas.CRM.Services
{
    public interface IEmailSenderService
    {
        void OrderCreated(long orderID);
        void OrderIncomeRecived(long orderID);
        
        void ClientActivationRequest(long clientID);
        void ClientFourDigitActivationRequest(long clientID);
        void ClientForgotPassword(long clientID);
        void Test();
    }
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ISettingsService _settingsService;

        public EmailSenderService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private EmailSenderServerSettings GetEmailSettings()
        {
            return _settingsService.GetEmailSenderServerSettings();
            
        }

        public void Test()
        {
            var service = ServiceLocator.Current.GetInstance<IClientService>();
            var clientObj = service.Get(56);

            if (string.IsNullOrEmpty(clientObj.ActivationCode))//если кода нет
            {
                clientObj.ActivationCode = Common.Kernel.Clients.ClientService.GenerateFourDigitActivationCode();
                service.Update(clientObj);
            }

            try
            {
                var ems = GetEmailSettings();
                var orderClientEmail = clientObj.Email ?? clientObj.Login;
                if (!string.IsNullOrEmpty(orderClientEmail))
                {
                    using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                    {
                        EnableSsl = ems.UseSSL
                    })
                    {
                        client.UseDefaultCredentials = false;

                        client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        new OrderMailer(ems).ClientFourDigitActivationRequest(new EmailMailerModel<Client>
                        {
                            To = new List<string>()
                            {
                                orderClientEmail
                            },
                            Object = clientObj
                        }).Send(new SmtpClientWrapper(client));
                    }
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void OrderCreated(long orderID)
        {
            var service = ServiceLocator.Current.GetInstance<IEmailNotificationService>();
            var targets = service.GetAll();

            var order = ServiceLocator.Current.GetInstance<IOrderService>().DeepLoadOrderForEmailSending(orderID);


            try
            {
                var ems = GetEmailSettings();
                var orderClientEmail = order.Client.Email ?? order.Client.Login;
                if (!string.IsNullOrEmpty(orderClientEmail))
                {
                    using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                    {
                        EnableSsl = ems.UseSSL
                    })
                    {
                        client.UseDefaultCredentials = false;

                        client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        new OrderMailer(ems).ClientOrderCreated(new EmailMailerModel<Order>
                        {
                            To = new List<string>()
                            {
                                orderClientEmail
                            },
                            Object = order
                        }).Send(new SmtpClientWrapper(client));
                    }
                }
               
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                var ems = GetEmailSettings();

                using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                {
                    EnableSsl = ems.UseSSL
                })
                {
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    new OrderMailer(ems).OrderCreated(new EmailMailerModel<Order>
                    {
                        To = targets.Select(x=>x.Email).ToList(),
                        Object = order
                    }).Send(new SmtpClientWrapper(client));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void OrderIncomeRecived(long orderID)
        {
            var service = ServiceLocator.Current.GetInstance<IEmailNotificationService>();
            var targets = service.GetAll();

            var order = ServiceLocator.Current.GetInstance<IOrderService>().DeepLoadOrderForEmailSending(orderID);


            try
            {
                var ems = GetEmailSettings();
                var orderClientEmail = order.Client.Email ?? order.Client.Login;
                if (!string.IsNullOrEmpty(orderClientEmail))
                {
                    using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                    {
                        EnableSsl = ems.UseSSL
                    })
                    {
                        client.UseDefaultCredentials = false;

                        client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        new OrderMailer(ems).ClientOrderIncomeRecived(new EmailMailerModel<Order>
                        {
                            To = new List<string>()
                            {
                                orderClientEmail
                            },
                            Object = order
                        }).Send(new SmtpClientWrapper(client));
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                var ems = GetEmailSettings();

                using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                {
                    EnableSsl = ems.UseSSL
                })
                {
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    new OrderMailer(ems).OrderIncomeRecived(new EmailMailerModel<Order>
                    {
                        To = targets.Select(x => x.Email).ToList(),
                        Object = order
                    }).Send(new SmtpClientWrapper(client));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        

        public void ClientFourDigitActivationRequest(long clientID)
        {
            var service = ServiceLocator.Current.GetInstance<IClientService>();
            var clientObj = service.Get(clientID);

            if (string.IsNullOrEmpty(clientObj.ActivationCode))//если кода нет
            {
                clientObj.ActivationCode = Common.Kernel.Clients.ClientService.GenerateFourDigitActivationCode();
                service.Update(clientObj);
            }

            try
            {
                var ems = GetEmailSettings();
                var orderClientEmail = clientObj.Email ?? clientObj.Login;
                if (!string.IsNullOrEmpty(orderClientEmail))
                {
                    using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                    {
                        EnableSsl = ems.UseSSL
                    })
                    {
                        client.UseDefaultCredentials = false;

                        client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        new OrderMailer(ems).ClientFourDigitActivationRequest(new EmailMailerModel<Client>
                        {
                            To = new List<string>()
                            {
                                orderClientEmail
                            },
                            Object = clientObj
                        }).Send(new SmtpClientWrapper(client));
                    }
                }
            }

            catch (Exception e)
            {
                throw e;
            }

        }


        public void ClientActivationRequest(long clientID)
        {
            var clientObj = ServiceLocator.Current.GetInstance<IClientService>().Get(clientID);
             
            try
            {
                var ems = GetEmailSettings();
                var orderClientEmail = clientObj.Email ?? clientObj.Login;
                if (!string.IsNullOrEmpty(orderClientEmail))
                {
                    using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                    {
                        EnableSsl = ems.UseSSL
                    })
                    {
                        client.UseDefaultCredentials = false;

                        client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        new OrderMailer(ems).ClientActivationRequest(new EmailMailerModel<Client>
                        {
                            To = new List<string>()
                            {
                               orderClientEmail
                            },
                            Object = clientObj
                        }).Send(new SmtpClientWrapper(client));
                    }
                }
            }

            catch (Exception e)
            {
                throw e;
            }
           
        }


        public void ClientForgotPassword(long clientID)
        {
            var clientObj = ServiceLocator.Current.GetInstance<IClientService>().Get(clientID);
            if(clientObj == null)
                return;
            try
            {
                var ems = GetEmailSettings();

                var email = clientObj.Email ?? clientObj.Login;


                if (!string.IsNullOrEmpty(email))
                {
                    using (var client = new SmtpClient(ems.HostAddress, ems.HostPort)
                    {
                        EnableSsl = ems.UseSSL
                    })
                    {
                        client.UseDefaultCredentials = false;

                        client.Credentials = new NetworkCredential(ems.UserName, ems.UserPassword);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        new OrderMailer(ems).ClientForgotPassword(new EmailMailerModel<Client>
                        {
                            To = new List<string>()
                            {
                                email
                            },
                            Object = clientObj
                        }).Send(new SmtpClientWrapper(client));
                    }
                }
            }

            catch (Exception e)
            {
                throw e;
            }

        }
    }
}