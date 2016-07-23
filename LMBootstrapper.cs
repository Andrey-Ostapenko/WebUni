using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using CommonServiceLocator.SimpleInjectorAdapter;
using FluentValidation;
using FluentValidation.Mvc;
using livemenu.Areas.BWP.Models;
using livemenu.Areas.CRM.Services;
using livemenu.Common;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.BWP;
using livemenu.Excel.CMS;
using livemenu.Excel.CRM;
using livemenu.Kernel;
using livemenu.Kernel.Account;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.CRM.Clients;
using livemenu.Kernel.MicroModules;
using livemenu.Models.Account;
using livemenu.Models.Notification;
using livemenu.Services;
using livemenu.SignalR;
using livemenu.Validators;
using livemenu.Validators.CMS;
using livemenu.Validators.CRM;
using livemenu.Validators.Profile;
using Microsoft.Practices.ServiceLocation;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web.Mvc;
using livemenu.Excel.Projects;

namespace livemenu
{
    public class LMBootstrapper
    {
        public static Container Initialize()
        {
            // Create the container as usual.
            var container = new Container();
            var adapter = new SimpleInjectorServiceLocatorAdapter(container);

            container.RegisterSingle<IServiceLocator>(adapter);
            ServiceLocator.SetLocatorProvider(() => adapter);
            RegisterTypes(container);


            //    PluginBootstrapper.RegisterServices(container);
            // KernelBootstrapper.Initialize(container); 
            //   container.RegisterPerWebRequest<IRepositoryContext, RepositoryContext>();
            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new ValidatorFactory(container);
            });


            container.RegisterMvcIntegratedFilterProvider();
            var kb = new KernelBootstrapper();
            kb.RegisterServices(container);

            //MMs после регистрации ядра
            RegisterMicroModules(container);


            if (Debugger.IsAttached)
            {
              //  container.Verify();
            }
            

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));


            PreStartInitialize();
            kb.PreStartInitialize(Debugger.IsAttached);
            //     PluginBootstrapper.In.tialize(container);
            return container;
        }

        private static void PreStartInitialize()
        {

        }



        private static void RegisterMicroModules(Container container)
        {
            var cf = ServiceLocator.Current.GetInstance<IContainerFactory>();
            cf.Register<IMicroModuleBase>(typeof(FieldListMicroModule), FieldListMicroModule.Type);
            cf.Register<IMicroModuleBase>(typeof(VerticalListMicroModule), VerticalListMicroModule.Type);
        }

        private static void RegisterTypes(Container container)
        {


            //   container.Register<IRepositoryContext>(() => container.GetInstance<IRepositoryContext>());

            // container.RegisterPerWebRequest<IRepositoryContext, RepositoryContext>();
            //     var mock1 = new Mock<ICatalogItemTypeMap>();


            ////todo: этот код в кастом сборку надо отправить 
            //mock1.Setup(map => map.Map).Returns(new Dictionary<long, CatalogItemDetailInfo>()
            //    {
            //        {2, new CatalogItemDetailInfo() {ModelType = typeof(TestSectionCatalogItemDetailModel), ViewName = MVC.TestTypeCatalogItem.Views.Index}}
            //    });

            //    container.RegisterSingle<ICatalogItemTypeMap>(mock1.Object);

            var tran = Lifestyle.Transient;
            container.AppendToCollection(typeof(INotificationTarget), tran.CreateRegistration<INotificationTarget>(container.GetInstance<ClientPushNotificationTarget>, container));
            container.RegisterPerWebRequest<IAccountManager, AccountManager>();

            container.RegisterSingle<IConnectionMapping<ConnectionUserAccount>, ConnectionMapping<ConnectionUserAccount>>();

            container.RegisterPerWebRequest<IPriceListExcelImportService, PriceListExcelImportService>();
            container.RegisterPerWebRequest<ISearchPropertiesImportService, SearchPropertiesImportService>();
            container.RegisterPerWebRequest<IStoragesExcelImportService, StoragesExcelImportService>();
            container.RegisterPerWebRequest<ISitemapExcelImportService, SitemapExcelImportService>();
            container.RegisterPerWebRequest<ISEOMetaExcelImportService, SEOMetaExcelImportService>();
            container.RegisterPerWebRequest<IAddressesExcelImportService, AddressesExcelImportService>();
            container.RegisterPerWebRequest<ICityExcelImportService, CityExcelImportService>();
            container.RegisterPerWebRequest<ICatalogItemBatchExcelImportService, CatalogItemBatchExcelImportService>();
            container.RegisterPerWebRequest<IDynamicTablesExcelImportService, DynamicTablesExcelImportService>();
            container.RegisterPerWebRequest<IDynamicColumnCodeExcelImportService, DynamicColumnCodeExcelImportService>();


            container.RegisterPerWebRequest<IClientLegalTypeService, ClientLegalTypeService>();


            container.RegisterPerWebRequest<IWebUniExporter, WebUniExporter>();

            container.RegisterPerWebRequest<IEmailSenderService, EmailSenderService>();

            container.RegisterSingle<IVersionService, VersionService>();
            container.RegisterSingle<ICurrentConfigurationService, CurrentConfigurationService>();

            RegisterValidators(container);
        }


        private static void RegisterValidators(Container container)
        {
            container.RegisterPerWebRequest<IValidator<Tag>, TagValidator>();
            container.RegisterPerWebRequest<IValidator<Property>, PropertyValidator>();
            container.RegisterPerWebRequest<IValidator<Position>, PositionValidator>();
            container.RegisterPerWebRequest<IValidator<CatalogItemCardModel>, CatalogItemCardModelValidator>();
            container.RegisterPerWebRequest<IValidator<User>, UserValidator>();
            container.RegisterPerWebRequest<IValidator<MyProfileModel>, MyProfileModelValidator>();
            container.RegisterPerWebRequest<IValidator<PriceListElement>, PriceListElementValidator>();
            container.RegisterPerWebRequest<IValidator<ClientNaturalPerson>, ClientNaturalPersonValidator>();
            container.RegisterPerWebRequest<IValidator<ClientLegalPersonality>, ClientLegalPersonalityValidator>();
            container.RegisterPerWebRequest<IValidator<ClientIndividualProprietor>, ClientIndividualProprietorValidator>();
            container.RegisterPerWebRequest<IValidator<AccountChangePasswordModel>, AccountChangePasswordModelValidator>();
            container.RegisterPerWebRequest<IValidator<SearchProperty>, SearchPropertyValidator>();

            container.RegisterPerWebRequest<IValidator<WebCounter>, WebCounterValidator>();
            container.RegisterPerWebRequest<IValidator<ClientRole>, ClientRoleValidator>();
            container.RegisterPerWebRequest<IValidator<ClientRoleStatus>, ClientRoleStatusValidator>();
            container.RegisterPerWebRequest<IValidator<DynamicTable>, DynamicTableValidator>();
            container.RegisterPerWebRequest<IValidator<DynamicColumn>, DynamicColumnValidator>();
            container.RegisterPerWebRequest<IValidator<DynamicColumnCode>, DynamicColumnCodeValidator>();

        }

    }
}