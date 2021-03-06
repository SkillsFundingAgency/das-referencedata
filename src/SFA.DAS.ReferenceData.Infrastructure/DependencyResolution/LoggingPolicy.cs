﻿using System;
using System.Linq;
using NLog;
using SFA.DAS.NLog.Logger;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.ReferenceData.Infrastructure.DependencyResolution
{
    public class LoggingPolicy : ConfiguredInstancePolicy
    {
        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var logger = instance?.Constructor?.GetParameters().FirstOrDefault(x => x.ParameterType == typeof(ILog));

            if (logger != null)
            {
                instance.Dependencies.AddForConstructorParameter(logger, LogManager.GetLogger(pluginType.FullName));
            }
        }
    }
}