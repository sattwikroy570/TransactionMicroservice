using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMicroservice.Models
{
    public class WithdrawDeposit
    {

        [Required]
        public int AccountId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount can not be negative!")]
        public int Amount { get; set; }
    }
}
