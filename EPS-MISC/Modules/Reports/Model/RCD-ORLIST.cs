using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Reports.Model
{
    public class RCD_ORLIST
    {
        public string FormType { get; set; }
        public int NoIssued { get; set; }
        public string FromOR { get; set; }
        public string ToOR { get; set; }
        public double Amount { get; set; }
    }
}
