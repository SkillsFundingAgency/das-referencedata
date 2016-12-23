using System.Collections.Generic;

namespace SFA.DAS.ReferenceData.Api.Client.Dto
{
    public class PagedApiResponse<T>
    {
        public ICollection<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
