using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestServerExtension.Model
{
    public class MyTestDateRequsetModel
    {
        public Guid DocumentId { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
    }
}
