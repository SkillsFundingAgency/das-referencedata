using System;

namespace SFA.DAS.ReferenceData.Api.Client.Exceptions
{
    public class ReferenceDataException : Exception
    {
        public ReferenceDataException(string message) : base(message)
        {
            // just call base        
        }
    }
}