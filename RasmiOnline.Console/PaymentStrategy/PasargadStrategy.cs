namespace RasmiOnline.Console.PaymentStrategy
{
    using System;
    using ZarinPal;
    using System.Net;
    using Properties;
    using System.Web;
    using Domain.Dto;
    using Domain.Enum;
    using Domain.Entity;
    using Business.Protocol;
    using Gnu.Framework.Core;
    using Business.Observers;
    using DependencyResolver.Ioc;
    using Gnu.Framework.Core.Authentication;
    using System.Net.Http;
    using System.Text;
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using Gnu.Framework.Core.Log;
    using System.EnterpriseServices;

    public class PasargadStrategy : IPaymentStrategy
    {
        readonly PaymentGatewayImplementationServicePortTypeClient _zarinPal;
        readonly IUserBusiness _userBusiness;
        readonly ITransactionBusiness _transactionBusiness;
        readonly IOrderBusiness _orderBusiness;
        readonly Lazy<IObserverManager> _observerManager;
        public PasargadStrategy()
        {
            _zarinPal = new PaymentGatewayImplementationServicePortTypeClient();
            _userBusiness = IocInitializer.GetInstance<IUserBusiness>();
            _transactionBusiness = IocInitializer.GetInstance<ITransactionBusiness>();
            _orderBusiness = IocInitializer.GetInstance<IOrderBusiness>();
            _observerManager = IocInitializer.GetInstance<Lazy<IObserverManager>>();
        }
        public string GetSign(string data)
        {
            var cs = new CspParameters { KeyContainerName = "PaymentTest" };
            var rsa = new RSACryptoServiceProvider(cs) { PersistKeyInCsp = false };
            rsa.Clear();
            rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(@"<RSAKeyValue><Modulus>l3/3pY/dp1/Brpr4N1RVnmmrFDxXyVcT81p5QnmaPUR7D0XORHCxf8aSa0yFUIBV9hkHvcjBg52H3sxpQc7QUjTi3fWgcszf/mU96+jFBImx/bJy0CJr8LoFQUpcc880xmHBNp3UO6uMX5IXAWG0ZSiIgHzYQ+g3pFfXY13RS48=</Modulus><Exponent>AQAB</Exponent><P>zeW4fMes29uwxkHc7vvbi3lWC6NargVh5c1Jtrt0jHqZpD5MNRtzkItAm3B5YeCzRRO0CyWKtheBY4t/bUFCfQ==</P><Q>vF2U1zhW1kycXDtl76KOBlKOfjQLPNGkkcPqMixTJx3KOSJqCKhArNIYgf1UBgU4bqchAxCuqM7TmyodGzF3+w==</Q><DP>O+CppUggYhPitdVjnfCKqWYQ+vN3pJIWJVFtNYIQY+YnllczrGIeWpPUpx+vfSJuoBEZ1AAH9eqiC0P/O9O1tQ==</DP><DQ>Sxxx3oQ7tEZmTyzsT+sdAQ5ofZCZLo5WNj3OJGWiXsW1PqUnbP5Sk9dPKH5ww9nPD+ia9FLxoqSeoo/ffVlzgw==</DQ><InverseQ>Q11pTeLrqvkedc3hltnPvRn10WJXrdfeJvwE5HyHnB5w0LS1g19J/FrtKvUNgcb1mSOjnqdmmdexel8hp280ng==</InverseQ><D>HI5ZXpKkhATvm/rH6J3z2X0vawwIvAG21/roulfzc7Lxwyo0PABF5MDIHoN46XiXO2DybZpmp2lND/jVqjrSVzIiC3/7npWZpEbhy7sHFdS+QWW/DaFWZ2WJPHBvLnWx33v2OJxi4TQqlvvsXMisW4ZhsfuTfYrukZdKX5EeVxk=</D></RSAKeyValue>");
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            return sign;
        }

        private string CreateTimeSpan(DateTime dt)
        {
            var month = dt.Month < 10 ? "0" + dt.Month.ToString() : dt.Month.ToString();
            var day = dt.Day < 10 ? "0" + dt.Day.ToString() : dt.Day.ToString();
            var h = dt.Hour < 10 ? "0" + dt.Hour.ToString() : dt.Hour.ToString();
            var m = dt.Minute < 10 ? "0" + dt.Minute.ToString() : dt.Minute.ToString();
            var s = dt.Second < 10 ? "0" + dt.Second.ToString() : dt.Second.ToString();
            return $"{dt.Year}/{month}/{day} {h}:{m}:{s}";
        }

        public IActionResponse<string> Do(PaymentGateway gateway, TransactionModel model)
        {
            var currentUser = _userBusiness.Find(model.UserId);
            if (currentUser == null)
            {
                return new ActionResponse<string>()
                {
                    Message = LocalMessage.UsernameIsWrong
                };
            }
            var transaction = new Transaction
            {
                OrderId = model.OrderId,
                Price = model.Price,
                PaymentGatewayId = model.PaymentGatewayId,
                Authority = "100",
                Status = "100",
                InsertDateMi = DateTime.Now,
                InsertDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date)
            };
            var doTransaction = _transactionBusiness.Do(transaction);
            if (!doTransaction.IsSuccessful)
                return new ActionResponse<string>
                {
                    IsSuccessful = false,
                    Message = LocalMessage.Exception
                };
            var dataModel = new {
                InvoiceNumber= model.OrderId.ToString(),
                InvoiceDate= transaction.InsertDateSh,
                TerminalCode = int.Parse(gateway.Username),//TODO:Place Real In Db
                MerchantCode= int.Parse(gateway.MerchantId),//TODO:Place Real In Db
                Amount = model.Price.ToString(),
                RedirectAddress = AppSettings.TransactionRedirectUrl_Pasargad,
                Timestamp= CreateTimeSpan(transaction.InsertDateMi),
                Action="1003",
                Mobile=currentUser.MobileNumber,
                currentUser.Email
            };
            var data = JsonConvert.SerializeObject(dataModel);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://pep.shaparak.ir/Api/v1/Payment/GetToken"),
                Method = HttpMethod.Post,
                Content = content
            };
            requestMessage.Headers.Add("Sign", GetSign(data));
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.SendAsync(requestMessage).Result;
            var deserializeResponse = JsonConvert.DeserializeObject<PasargadGetTokenReponse>(Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
            if(deserializeResponse.IsSuccess)
                return new ActionResponse<string>
                {
                    IsSuccessful = true,
                    Result = $"https://pep.shaparak.ir/payment.aspx?n={deserializeResponse.Token}"
                };
            else return new ActionResponse<string>
            {
                IsSuccessful = true,
                Message = deserializeResponse.Message
            };
        }

        public IActionResponse<string> Verify(PaymentGateway gateway, Transaction model, object responseGateway = null)
        {
            try
            {
                var deserializeResponse = new PasargadVerifyResponse();
                if (responseGateway == null || ((PayRedirectModel)(responseGateway)).status == 0)
                    return new ActionResponse<string>
                    {
                        IsSuccessful = false,
                        Message = "عملیات پرداخت از سمت درگاه تایید نشد، لطفا مجددا عملیات پرداخت را تکرار نمایید",
                    };

                using (HttpClient http = new HttpClient())
                {
                    var dataModel = new
                    {
                        InvoiceNumber = model.OrderId.ToString(),
                        InvoiceDate = model.InsertDateSh,
                        TerminalCode = int.Parse(gateway.Username),//TODO:Place Real In Db
                        MerchantCode = int.Parse(gateway.MerchantId),//TODO:Place Real In Db
                        TransactionReferenceID = (string)responseGateway
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(dataModel), Encoding.UTF8, "application/json");
                    var response = http.PostAsync("https://pep.shaparak.ir/Api/v1/Payment/CheckTransactionResult", content).Result;
                    FileLoger.Info("webservice response : " + response.Content.ReadAsStringAsync().Result, GlobalVariable.LogPath);
                    deserializeResponse = JsonConvert.DeserializeObject<PasargadVerifyResponse>(response.Content.ReadAsStringAsync().Result);
                }

                if (deserializeResponse.IsSuccess)
                {
                    model.IsSuccess = true;
                    model.TrackingId = ((PayRedirectModel)(responseGateway)).transId.ToString();
                    model.Status = "1";
                    if (model.OrderId != 0)
                        _orderBusiness.UpdateStatus(model.OrderId);
                    _transactionBusiness.Update(model);

                    _observerManager.Value.Notify(ConcreteKey.Transaction_Add, new ObserverMessage
                    {
                        SmsContent = string.Format(LocalMessage.Transaction_Add_Sms, (HttpContext.Current.User as ICurrentUserPrincipal).FullName, model.OrderId),
                        BotContent = string.Format(LocalMessage.Transaction_Add_Bot, (HttpContext.Current.User as ICurrentUserPrincipal).FullName,
                                                model.OrderId, gateway.BankName.GetLocalizeDescription(),
                                                model.Price.ToString("0,0"),
                                                model.TrackingId),
                        Key = nameof(Transaction),
                        UserId = (HttpContext.Current.User as ICurrentUserPrincipal).UserId,
                    });
                    return new ActionResponse<string>
                    {
                        IsSuccessful = true,
                        Message = "عملیات پرداخت با موفقیت انجام شد",
                        Result = model.TrackingId.ToString()
                    };
                }
                else
                {
                    model.IsSuccess = false;
                    model.TrackingId = model.TrackingId.ToString();
                    model.Status = "-1";
                    _transactionBusiness.Update(model);
                    return new ActionResponse<string>
                    {
                        IsSuccessful = false,
                        Message = "عملیات پرداخت از سمت درگاه تایید نشد، لطفا مجددا عملیات پرداخت را تکرار نمایید",
                        Result = model.TrackingId.ToString()
                    };
                }
            }
            catch (Exception e)
            {
                FileLoger.Error(e, GlobalVariable.LogPath);
                return new ActionResponse<string>
                {
                    IsSuccessful = false,
                    Message = "عملیات پرداخت از سمت درگاه تایید نشد، لطفا مجددا عملیات پرداخت را تکرار نمایید",
                    Result = model.TrackingId.ToString()
                };
            }
        }
    }
}