using System.Collections.Generic;

namespace SFA.DAS.ReferenceData.Domain.Models.Data
{
    public class PagedResult<T>
    {
        public ICollection<T> Data { get; set; }
        public int Page { get; set; }
        public int TotalPages  { get; set; }
    }
}
