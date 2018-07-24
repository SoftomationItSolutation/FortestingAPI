using Braintree;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FortestingAPI.Controllers
{
    public class PaymentController : ApiController
    {
        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };


        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

      

        public class ClientToken
        {
            public string token { get; set; }

            public ClientToken(string token)
            {
                this.token = token;
            }
        }

        public class Nonce
        {
            public string nonce { get; set; }
            public decimal chargeAmount { get; set; }

            public Nonce(string nonce)
            {
                this.nonce = nonce;
                this.chargeAmount = chargeAmount;
            }
        }

        [Route("api/braintree/getclienttoken")]
        public HttpResponseMessage GetClientToken()
        {

            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = ConfigurationManager.AppSettings["BraintreeMerchantId"],
                PublicKey = ConfigurationManager.AppSettings["BraintreePublicKey"],
                PrivateKey = ConfigurationManager.AppSettings["BraintreePrivateKey"]
            };

            ClientToken ct = new ClientToken(ConfigurationManager.AppSettings["BraintreeTokenizationKeys"]);
            return Request.CreateResponse(HttpStatusCode.OK, ct, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/braintree/createpurchase")]
        public HttpResponseMessage Post([FromBody]Nonce nonce)
        {
            string TransactionId;
            TranscationDetails dtl = new TranscationDetails();
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = ConfigurationManager.AppSettings["BraintreeMerchantId"],
                PublicKey = ConfigurationManager.AppSettings["BraintreePublicKey"],
                PrivateKey = ConfigurationManager.AppSettings["BraintreePrivateKey"]
            };

            var request = new TransactionRequest
            {
                Amount = nonce.chargeAmount,
                PaymentMethodNonce = nonce.nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
           
            if (result.IsSuccess())
            {
                TransactionId = result.Target.Id;
               
            }
            else if (result.Transaction != null)
            {
                TransactionId = result.Transaction.Id;
               
            }
            else
            {
                TransactionId = "0";
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                dtl.TransactionId = TransactionId;
                dtl.header = "Transaction Failed";
                dtl.icon = "fail";
                dtl.message = errorMessages;
                
            }
            if (TransactionId != "0")
            {
                Transaction transaction = gateway.Transaction.Find(TransactionId);
                if (transactionSuccessStatuses.Contains(transaction.Status))
                {
                    dtl.TransactionId = TransactionId;
                    dtl.header = "Sweet Success!";
                    dtl.icon = "success";
                    dtl.message = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
                }
                else
                {
                    dtl.TransactionId = TransactionId;
                    dtl.header = "Transaction Failed";
                    dtl.icon = "fail";
                    dtl.message = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
                }
            }
            HttpResponseMessage response = Request.CreateResponse(dtl);
            return response;
        }

        [Route("api/braintree/showdetails")]
        public HttpResponseMessage Show(String id) {
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = ConfigurationManager.AppSettings["BraintreeMerchantId"],
                PublicKey = ConfigurationManager.AppSettings["BraintreePublicKey"],
                PrivateKey = ConfigurationManager.AppSettings["BraintreePrivateKey"]
            };
            Transaction transaction = gateway.Transaction.Find(id);
            HttpResponseMessage response = Request.CreateResponse(transaction);
            return response;
        }

        public class TranscationDetails
        {
            public string TransactionId { get; set; }
            public string header { get; set; }
            public string icon { get; set; }
            public string message { get; set; }
        }
    }
}
