using System;

namespace SFA.DAS.ReferenceData.Infrastructure.ExecutionPolicies
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RequiredPolicyAttribute : Attribute
    {
        public RequiredPolicyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
