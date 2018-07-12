using System;

namespace SFA.DAS.ReferenceData.Types.Exceptions
{
    public class ReferenceDataException : Exception
    {
        public ReferenceDataException(string message) : base(message)
        {
            // just call base        
        }
    }
}