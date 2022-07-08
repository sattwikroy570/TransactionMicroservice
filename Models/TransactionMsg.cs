using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMicroservice.Models
{
    public class TransactionMsg
    {
        public int AccountId { get; set; }
        public string Message { get; set; }        
        public double Balance { get; set; }
    }
}
