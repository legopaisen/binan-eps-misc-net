using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Reports.Model
{
    public class AbstractOfCollectionsFees
    {
        public DateTime? DateIssued { get; set; }
        public string OR { get; set; }
        public string Payor { get; set; }
        public double? FeesAmt { get; set; }
        public string Fees { get; set; }
    }
}
