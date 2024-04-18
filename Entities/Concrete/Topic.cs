using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Topic:IEntity
    {
        public int Id { get; set; }
        public string TopicTitle { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
