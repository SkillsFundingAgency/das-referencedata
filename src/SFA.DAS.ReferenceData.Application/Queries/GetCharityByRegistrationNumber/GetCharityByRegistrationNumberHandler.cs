using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
namespace SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber
{
    public class GetCharityByRegistrationNumberHandler : IAsyncRequestHandler<GetCharityByRegistrationNumberQuery, GetCharityByRegistrationNumberResponse>
    {
        private readonly ICharityRepository _charityRepository;

        public GetCharityByRegistrationNumberHandler(ICharityRepository charityRepository)
        {
            _charityRepository = charityRepository;
        }

        public async Task<GetCharityByRegistrationNumberResponse> Handle(GetCharityByRegistrationNumberQuery query)
        {
            return new GetCharityByRegistrationNumberResponse()
            {
                Charity = await _charityRepository.GetCharityByRegistrationNumber(query.RegistrationNumber)
            };
        }
    }
}
