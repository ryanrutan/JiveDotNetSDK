using Net.Pokeshot.JiveSdk.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Net.Pokeshot.JiveSdk.Auth
{
   
        public class SignedRequest : ActionFilterAttribute
    {
        private static readonly string PARAM_ALGORITHM = "algorithm";
        private static readonly string PARAM_CLIENT_ID = "client_id";
        private static readonly string PARAM_JIVE_URL = "jive_url";
        private static readonly string PARAM_TENANT_ID = "tenant_id";
        private static readonly string PARAM_TIMESTAMP = "timestamp";
        private static readonly string PARAM_SIGNATURE = "signature";

        private static readonly string JIVE_EXTN = "JiveEXTN ";
        private JiveSdkContext db = new JiveSdkContext();

        private Dictionary<string, string> GetParametersFromAuthHeader(string authHeader)
        {
            authHeader = authHeader.Substring(JIVE_EXTN.Length);
            string[] parameters = authHeader.Split('&', '?');
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();
            foreach (string keyValue in parameters)
            {
                string[] tokens = keyValue.Split('=');
                if (tokens.Length != 2)
                {
                    //Windows Azure tracing. Replace with a logging mechanism of your choice
                    //Trace.WriteLine("Authorization header not formatted correctly");
                    throw new HttpRequestValidationException();
                }
                parameterDictionary.Add(HttpUtility.UrlDecode(tokens[0]), HttpUtility.UrlDecode(tokens[1]));

            }

            return parameterDictionary;
        }

        private string validateSignature(string parameterStrWithoutSignature, string clientSecret)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //byte[] secret = encoding.GetBytes(clientSecret);
            byte[] secret = Convert.FromBase64String(clientSecret);


            byte[] headerToValidate = encoding.GetBytes(parameterStrWithoutSignature);
            HMACSHA256 hmacsha256 = new HMACSHA256(secret);

            byte[] calculatedSignature = hmacsha256.ComputeHash(headerToValidate);

            return Convert.ToBase64String(calculatedSignature);
        }
        private string ToUrlBase64String(byte[] Input)
        {
            return Convert.ToBase64String(Input).Replace("=", String.Empty)
                                                .Replace('+', '-')
                                                .Replace('/', '_');
        }

        private byte[] SignWithHmac(byte[] dataToSign, byte[] keyBody)
        {
            using (var hmacAlgorithm = new HMACSHA256(keyBody))
            {
                hmacAlgorithm.ComputeHash(dataToSign);
                return hmacAlgorithm.Hash;
            }
        }


        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                //Example for retrieving config settings from the web config
                //bool oauthValidationEnabled = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsOauthValidationEnabled"]);
                bool oauthValidationEnabled = true;
                string uri = actionContext.Request.RequestUri.AbsolutePath;
                string host = actionContext.Request.RequestUri.Host;




                if (oauthValidationEnabled)
                {



                    if (actionContext.Request.Headers.Authorization != null)
                    {
                        try
                        {
                            var authTemp = actionContext.Request.Headers.Authorization;

                            string authHeader = authTemp.ToString();
                            string userId="0";

                            if (HttpContext.Current.Request.Headers["x-jive-user-id"]!=null){
                               userId =  HttpContext.Current.Request.Headers["x-jive-user-id"];
                            }
                           

                            

                            if (authHeader.StartsWith(JIVE_EXTN) == false || authHeader.Contains(PARAM_SIGNATURE) == false)
                            {
                                Trace.WriteLine("Authorization header not formatted correctly");
                                throw new HttpRequestValidationException("Authorization header not formatted correctly");
                            }

                            Dictionary<string, string> parameterDict = GetParametersFromAuthHeader(authHeader);
                            string signature = parameterDict[PARAM_SIGNATURE];
                            parameterDict.Remove(PARAM_SIGNATURE);
                            string algorithm = parameterDict[PARAM_ALGORITHM];
                            string clientId = parameterDict[PARAM_CLIENT_ID];
                            string jiveUrl = parameterDict[PARAM_JIVE_URL];
                            string tenantId = parameterDict[PARAM_TENANT_ID];
                            string timeStampStr = parameterDict[PARAM_TIMESTAMP];

                            long timestampMilliSeconds = Convert.ToInt64(timeStampStr);
                            DateTime timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestampMilliSeconds);
                            if (timestamp > DateTime.Now.AddMinutes(5) || DateTime.Now.AddMinutes(-5) > timestamp)
                            {
                                Trace.WriteLine("Timestamp older than 5 minutes");
                                int timestampDiff = timestamp.CompareTo(DateTime.Now);
                                throw new HttpRequestValidationException("Timestamp difference more than 5 minutes. Difference: " + timestampDiff.ToString());
                            }

                            var _myAddon = db.JiveAddons
                                .Include("JiveInstance")
                                .Where(a => a.JiveInstance.JiveInstanceId.Equals(tenantId));
                            if (_myAddon.Count() == 0)
                            {
                                Trace.WriteLine("Jive Instance not found");
                                throw new HttpRequestValidationException();
                            }
                            JiveAddon myAddon = _myAddon.Single();
                            if (myAddon.ClientId.Equals(clientId) == false)
                            {
                                Trace.WriteLine("Not the expected client id for this tenant");
                                throw new HttpRequestValidationException("Not the expected client id for this tenant");
                            }

                            string parameterStrWithoutSignature = authHeader.Substring(JIVE_EXTN.Length, authHeader.IndexOf(PARAM_SIGNATURE) - (PARAM_SIGNATURE.Length + 1));


                            string expectedSignature = validateSignature(parameterStrWithoutSignature, myAddon.ClientSecret);

                            if (expectedSignature.Equals(signature))
                            {
                                string ownerId = userId + "@" + tenantId;

                                GenericIdentity MyIdentity = new GenericIdentity(ownerId);

                                String[] MyStringArray = { "User" };
                                GenericPrincipal MyPrincipal =
                                    new GenericPrincipal(MyIdentity, MyStringArray);
                                Thread.CurrentPrincipal = MyPrincipal;
                            }
                            else
                            {
                                Trace.WriteLine("Signature not correctly validated");
                                throw new HttpRequestValidationException("Signature not correctly validated");
                            }


                        }
                        catch (HttpRequestValidationException authEx)
                        {
                            Trace.WriteLine(authEx.Message, "Error");
                            //NewRelic.Api.Agent.NewRelic.NoticeError(authEx);
                            actionContext.Response = new System.Net.Http.HttpResponseMessage();
                            actionContext.Response.Content = null;
                            actionContext.Response.StatusCode = HttpStatusCode.Unauthorized;

                        }
                    }


                }
                else
                {
                    if (actionContext.Request.Headers.Authorization != null)
                    {

                        var authTemp = actionContext.Request.Headers.Authorization;

                        string authString = authTemp.Parameter;

                        //string authString = HttpContext.Current.Request.Headers["authorization"];
                        string userId = HttpContext.Current.Request.Headers["x-jive-user-id"];

                        string[] authStringArray = authString.Split('&');
                        string tenantId = null;
                        string jiveUrl = null;
                        foreach (string authElement in authStringArray)
                        {
                            string[] keyValue = authElement.Split('=');
                            if (keyValue[0].Equals("tenant_id"))
                            {
                                tenantId = keyValue[1];
                            }

                            if (keyValue[0].Equals("jive_url"))
                            {
                                jiveUrl = HttpUtility.UrlDecode(keyValue[1]);
                            }
                        }
                        string ownerId = userId + "@" + tenantId;

                        GenericIdentity MyIdentity = new GenericIdentity(ownerId);

                        String[] MyStringArray = { "User" };
                        GenericPrincipal MyPrincipal =
                            new GenericPrincipal(MyIdentity, MyStringArray);
                        Thread.CurrentPrincipal = MyPrincipal;

                    }

                    else
                    {

                        throw new HttpRequestValidationException("Authorization header not formatted correctly");

                       
                    }
                }



            }
            catch (Exception ex)
            {
                //NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                actionContext.Response = new System.Net.Http.HttpResponseMessage();
                actionContext.Response.Content = null;
                actionContext.Response.StatusCode = HttpStatusCode.InternalServerError;

            }
        }
       

    }
}
