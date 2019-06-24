using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using OFXUpload.Models;
using OFXUpload.Models.Interfaces;
using OFXUpload.Repositories;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OFXUpload
{
  public static class NinjectWebCommon
  {
    private static readonly Bootstrapper bootstrapper = new Bootstrapper();

    public static void Start()
    {
      DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
      DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
      bootstrapper.Initialize(CreateKernel);
    }

    public static void Stop()
    {
      bootstrapper.ShutDown();
    }
    public  static IKernel CreateKernel()
    {
      var kernel = new StandardKernel();
      try
      {
        kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
        kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

        kernel.Bind<IFinancialMovements>().To<FinancialMovements>();
        kernel.Bind<IOFXHandler>().To<OFXHandler>();
        kernel.Bind<IFinancialAccountRepository>().To<FinancialAccountRepository>();
        kernel.Bind<IFinancialMovementRepository>().To<FinancialMovementRepository>();
        kernel.Bind<IStoneRepository>().To<StoneRepository>();
        kernel.Bind<IMovementHandler>().To<MovementHandler>();
        kernel.Bind<IXmlHandler>().To<XmlHandler>();

        RegisterServices(kernel);
        return kernel;
      }
      catch
      {
        kernel.Dispose();
        throw;
      }
    }

    /// <summary>
    /// Load your modules or register your services here!
    /// </summary>
    /// <param name="kernel">The kernel.
    private static void RegisterServices(IKernel kernel)
    {
    }
  }
}