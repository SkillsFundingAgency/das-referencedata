﻿using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Serializer;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater;
using SFA.DAS.ReferenceData.Infrastructure.Factories;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using StructureMap;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS"));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });
           
            For<IEducationalOrgsUpdater>().Use<EducationalOrgsUpdater>();
            For<IEdubaseClientFactory>().Use<EdubaseClientFactory>();
            For<IEdubaseService>().Use<EdubaseService>();
            For<IEducationalOrgainsationSerialiser>().Use<EducationalOrgainsationSerialiser>();
            For<IAzureStorageUploader>().Use<AzureStorageUploader>();

            RegisterMapper();
            RegisterMediator();
            RegisterLogger();
        }

        private void RegisterMediator()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }

        private void RegisterMapper()
        {
            var profiles = Assembly.Load("SFA.DAS.ReferenceData.Infrastructure").GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t))
                .Select(t => (Profile)Activator.CreateInstance(t)).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                profiles.ForEach(cfg.AddProfile);
            });

            var mapper = config.CreateMapper();

            For<IConfigurationProvider>().Use(config).Singleton();
            For<IMapper>().Use(mapper).Singleton();
        }

        private void RegisterLogger()
        {
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                null,
                null)).AlwaysUnique();
        }
    }
}
