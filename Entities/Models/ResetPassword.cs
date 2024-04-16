using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string? Code { get; set; }
    }
}
