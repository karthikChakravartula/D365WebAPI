using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CRMAPI.CRMFunctions
{
    public class CRMBaseMethods
    {
        private HttpClient httpClient { get; set; }
        private Authentication auth { get; set; }
        public CRMBaseMethods()
        {
            auth = new Authentication();

            httpClient = new HttpClient();

            httpClient.Timeout = new TimeSpan(0, 2, 0);  // 2 minutes time out period.

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Result.AccessToken);
        }
        
        public JObject GetEntityData(string EntityType)
        {
            //using (httpClient)
            //{       
            var chkd = RetrieveMultiple("systemusers", "systemuserid", "contains(domainname,'karthik')");

            if (chkd != null && chkd["value"].Any())
            {
                httpClient.DefaultRequestHeaders.Add("CallerObjectId", chkd["value"][0]["systemuserid"].ToString());
            }
            using (var Response = httpClient.GetAsync(auth.CRMOrgURL + "api/data/v9.1/" + EntityType + "?$select=*"))
                {
                    Response.Wait();

                    if (Response.Result.IsSuccessStatusCode)
                    {
                        return JObject.Parse(Response.Result.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return new JObject("Failed");
                    }
                }              

            //}

        }
        public JObject RetrieveMultiple(string EntityType, string ColumnSet = "*", string filter = null)
        {
            //using (httpClient)
            //{
           // var chkd = RetrieveMultiple("systemusers", "systemuserid", "contains(domainname,'karthik')");

            //if (chkd != null && chkd["value"].Any())
            //{
                //httpClient.DefaultRequestHeaders.Add("CallerObjectId", "e39c5d16-675b-48d1-8e67-667427e9c084");
           // }

            using (var Response = httpClient.GetAsync(auth.CRMOrgURL + "api/data/v9.1/" + EntityType + "?$select=" + ColumnSet + ""+(filter != null ? "&$filter="+filter+"":"")))
                {
                    Response.Wait();

                    if (Response.Result.IsSuccessStatusCode)
                    {
                        return JObject.Parse(Response.Result.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return null;
                    }
                }

            //}

        }
        public JObject Retrieve(string EntityType, Guid guid, string ColumnSet = "*")
        {
            using (httpClient)
            {
                using (var Response = httpClient.GetAsync(auth.CRMOrgURL + "api/data/v9.1/" + EntityType + "(" + guid + ")?$select=" + ColumnSet + ""))
                {
                    Response.Wait();

                    if (Response.Result.IsSuccessStatusCode)
                    {
                        return JObject.Parse(Response.Result.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return new JObject("Failed");
                    }
                }

            }

        }

        public object Create(string EntityType, object Data)
        {
            //using (httpClient)
            //{
            var chkd = RetrieveMultiple("systemusers", "systemuserid", "contains(domainname,'karthik')");

            if (chkd != null && chkd["value"].Any())
            {
                httpClient.DefaultRequestHeaders.Add("CallerObjectId", "a884f36d-9d5c-49f6-b6e2-5806e328ba56");
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(Data), Encoding.UTF8, "application/json");

                using (var Response = httpClient.PostAsync(auth.CRMOrgURL + "api/data/v9.1/" + EntityType, stringContent))
                {
                    Response.Wait();

                    if (Response.Result.IsSuccessStatusCode)
                    {
                        return Response.Result;
                    }
                    else
                    {
                        return null;
                    }
                }

           // }

        }


        public object Update(string EntityType,Guid guid, object Data, string ColumnSet = "*")
        {
            //using (httpClient)
           // {
                //var chkd = RetrieveMultiple("systemusers", "systemuserid", "contains(domainname,'" + ((JObject)Data)["callingUser"].ToString() + "')");

                //if(chkd !=null && chkd["value"].Any())
                //{
                    httpClient.DefaultRequestHeaders.Add("CallerObjectId", "a884f36d-9d5c-49f6-b6e2-5806e328ba56");
                //}

                var stringContent = new StringContent(JsonConvert.SerializeObject(Data), Encoding.UTF8, "application/json");

                using (var Response = httpClient.PatchAsync(auth.CRMOrgURL + "api/data/v9.1/" + EntityType + "(" + guid + ")?$select="+ColumnSet+"", stringContent))
                {
                    Response.Wait();

                    if (Response.Result.IsSuccessStatusCode)
                    {
                        return Response.Result;
                    }
                    else
                    {
                        return null;
                    }
                }

            //}

        }

        public object Delete(string EntityType, Guid guid)
        {
            using (httpClient)
            {
                using (var Response = httpClient.DeleteAsync(auth.CRMOrgURL + "api/data/v9.1/" + EntityType + "(" + guid + ")"))
                {
                    Response.Wait();

                    if (Response.Result.IsSuccessStatusCode)
                    {
                        return Response.Result;
                    }
                    else
                    {
                        return null;
                    }
                }

            }

        }

    }
}
