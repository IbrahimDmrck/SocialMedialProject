using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ClaimDto:IDto
    {
        public int UserId { get; set; }
        public int ClaimId { get; set; }
        public string UserName { get; set; }
        public string ClaimName { get; set; }
    }
}
