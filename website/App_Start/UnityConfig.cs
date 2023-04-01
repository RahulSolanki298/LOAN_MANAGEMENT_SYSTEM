using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using website.Interface;
using website.Repository;

namespace website
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IBranchRepository, BranchRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}