using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TransactionMicroservice.Models;

namespace TransactionMicroservice.Repository
{
    public class TransactionRep : ITransactionRep
    {
        Uri baseAddress = new Uri("http://20.241.72.11/api");

        Uri baseAddress2 = new Uri("http://52.159.81.113/api");
        //Port No.
        HttpClient client, client2;

        public TransactionRep()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;

            client2 = new HttpClient();
            client2.BaseAddress = baseAddress2;

        }
        public TransactionMsg deposit(WithdrawDeposit dwacc)
        {

            string data = JsonConvert.SerializeObject(dwacc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            string token = TokenInfo.StringToken;
            client2.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = client2.PostAsync(client2.BaseAddress + "/Account/Deposit/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string data1 = response.Content.ReadAsStringAsync().Result;
                TransactionMsg msg = JsonConvert.DeserializeObject<TransactionMsg>(data1);
                return msg;
            }
            return null;
        }

        public TransactionMsg transfer(Transfers t)
        {
            WithdrawDeposit wAcc = new WithdrawDeposit { AccountId = t.SourceAccountId, Amount = t.Amount };
            TransactionMsg msg = new TransactionMsg();
            string withdrawData = JsonConvert.SerializeObject(wAcc);
            StringContent withdrawContent = new StringContent(withdrawData, Encoding.UTF8, "application/json");


            string token = TokenInfo.StringToken;
            client.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Rules/evaluateMinBal/", withdrawContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                RuleStatus s1 = JsonConvert.DeserializeObject<RuleStatus>(data);
                if (s1.Warning)
                {
                    msg.Message = "Warning";
                    return msg;
                }

                client2.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response1 = client2.PostAsync(client2.BaseAddress + "/Account/Withdraw/", withdrawContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data1 = response1.Content.ReadAsStringAsync().Result;
                    TransactionMsg msg1 = JsonConvert.DeserializeObject<TransactionMsg>(data1);
                    if(msg1.Message == "Inscufficient Balance")
                    {
                        return msg1;
                    }
                    WithdrawDeposit dAcc = new WithdrawDeposit { AccountId = t.DestinationAccountId, Amount = t.Amount };
                    string depositData = JsonConvert.SerializeObject(dAcc);
                    StringContent depositContent = new StringContent(depositData, Encoding.UTF8, "application/json");

                    client2.DefaultRequestHeaders.Add("Authorization", token);
                    HttpResponseMessage response2 = client2.PostAsync(client2.BaseAddress + "/Account/Deposit/", depositContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data2 = response2.Content.ReadAsStringAsync().Result;
                        TransactionMsg msg2 = JsonConvert.DeserializeObject<TransactionMsg>(data2);
                        msg1.Message = "Succesfully Transferred Acc No." + t.Amount + " from Acc No." +  msg1.AccountId + " to " + msg2.AccountId;
                        return msg1;
                    }
                }
               
            }
            return null;
        }

        public TransactionMsg withdraw(WithdrawDeposit dwacc)
        {
            TransactionMsg msg = new TransactionMsg();
            string data = JsonConvert.SerializeObject(dwacc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            string token = TokenInfo.StringToken;
            client.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Rules/evaluateMinBal/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string data1 = response.Content.ReadAsStringAsync().Result;
                RuleStatus s1 = JsonConvert.DeserializeObject<RuleStatus>(data1);
                if (s1.Warning)
                {
                    msg.Message = "Warning";
                    return msg;
                }

                client2.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response1 = client2.PostAsync(client2.BaseAddress + "/Account/Withdraw/", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data2 = response1.Content.ReadAsStringAsync().Result;
                    msg = JsonConvert.DeserializeObject<TransactionMsg>(data2);
                    return msg;
                }
            }
            return null;
        }
    }
}
