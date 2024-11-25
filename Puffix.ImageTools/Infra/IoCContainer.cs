using Autofac;
using Microsoft.Extensions.Configuration;
using Puffix.IoC;
using Puffix.IoC.Configuration;
using System;
using System.Collections.Generic;

namespace Puffix.ImageTools.Infra;

public class IoCContainer : IIoCContainerWithConfiguration
{
    private readonly IContainer container;

    public IConfiguration Configuration { get; }

    public IoCContainer(ContainerBuilder containerBuilder, IConfiguration configuration)
    { 
        // Self-register the container.
        containerBuilder.Register(_ => this).As<IIoCContainerWithConfiguration>().SingleInstance();
        containerBuilder.Register(_ => this).As<IIoCContainer>().SingleInstance();

        container = containerBuilder.Build();
        Configuration = configuration;
    }

    public static IoCContainer BuildContainer(IConfiguration configuration)
    {
        ContainerBuilder containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterAssemblyTypes(typeof(IoCContainer).Assembly) // Current Assembly.
                        .AsSelf()
                        .AsImplementedInterfaces();

        containerBuilder.RegisterInstance(configuration).SingleInstance();

        return new IoCContainer(containerBuilder, configuration);
    }

    public ObjectT Resolve<ObjectT>(params IoCNamedParameter[] parameters)
        where ObjectT : class
    {
        ObjectT resolvedObject;
        if (parameters != null)
            resolvedObject = container.Resolve<ObjectT>(ConvertIoCNamedParametersToAutfac(parameters));
        else
            resolvedObject = container.Resolve<ObjectT>();

        return resolvedObject;
    }

    public object Resolve(Type objectType, params IoCNamedParameter[] parameters)
    {
        object resolvedObject;
        if (parameters != null)
            resolvedObject = container.Resolve(objectType, ConvertIoCNamedParametersToAutfac(parameters));
        else
            resolvedObject = container.Resolve(objectType);

        return resolvedObject;
    }

    private IEnumerable<NamedParameter> ConvertIoCNamedParametersToAutfac(IEnumerable<IoCNamedParameter> parameters)
    {
        foreach (var parameter in parameters)
        {
            if (parameter != null)
                yield return new NamedParameter(parameter.Name, parameter.Value);
        }
    }
}
