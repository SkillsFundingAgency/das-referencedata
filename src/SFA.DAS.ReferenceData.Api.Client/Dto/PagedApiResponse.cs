using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Api.Client.Dto
{
    public class PagedApiResponse<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
