using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionMicroservice.Models;

namespace TransactionMicroservice.Repository
{
    public interface ITransactionRep
    {
        public TransactionMsg deposit(WithdrawDeposit value);
        public TransactionMsg withdraw(WithdrawDeposit value);
        public TransactionMsg transfer(Transfers value);
    }
}
