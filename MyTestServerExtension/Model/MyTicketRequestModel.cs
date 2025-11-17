using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestServerExtension.Model
{
    public class MyTicketRequestModel
    {
        public Guid DocumentId { get; set; }
        public string DateFrom {  get; set; }
        public string DateTo { get; set; }
        public Guid CityRef { get; set; }
    }
}
