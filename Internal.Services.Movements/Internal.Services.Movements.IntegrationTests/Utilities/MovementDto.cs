using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Services.Movements.IntegrationTests.Utilities
{
    public class MovementDto
    {
        public int MovementId { get; set; }
        public string Account { get; set; }
        public string MovementType { get; set; }
        public decimal Amount { get; set; }
        public string AccountFrom { get; set; }
        public string AccountTo { get; set; }
    }

    public class GetMovementsResponseDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<MovementDto> Movements { get; set; }
    }
}
