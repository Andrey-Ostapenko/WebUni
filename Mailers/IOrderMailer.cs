using System.Collections.Generic;
using livemenu.Common.Entities.Entities;
using Mvc.Mailer;

namespace livemenu.Mailers
{
    public class EmailMailerModel<T>
    {
        public T Object { get; set; }

        public List<string> To { get; set; }
    }
    public interface IOrderMailer
    {
        MvcMailMessage OrderCreated(EmailMailerModel<Order> model);
        MvcMailMessage ClientOrderCreated(EmailMailerModel<Order> model);
    }
}