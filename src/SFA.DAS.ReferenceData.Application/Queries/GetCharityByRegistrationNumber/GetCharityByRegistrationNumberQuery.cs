using MediatR;

namespace SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber
{
    public class GetCharityByRegistrationNumberQuery : IAsyncRequest<GetCharityByRegistrationNumberResponse>
    {
        public int RegistrationNumber { get; set; }
    }
}
