﻿using System.Web;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ReferenceData.Api.App_Start
{
    public sealed class RequestContext : IWebLoggingContext
    {
        public RequestContext(HttpContextBase context)
        {
            IpAddress = context?.Request.UserHostAddress;
            Url = context?.Request.RawUrl;
        }

        public string IpAddress { get; private set; }

        public string Url { get; private set; }
    }
}