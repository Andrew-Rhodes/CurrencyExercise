using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExercise
{
    public class ResponseModel
    {
        public string  Disclaimer { get; set; }
        public string  License { get; set; }
        public int  TimeStamp { get; set; }
        public string  Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
