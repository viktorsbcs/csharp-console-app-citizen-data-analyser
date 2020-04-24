using declared_persons_analyser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace declared_persons_analyser.Services
{
    public class WebServices
    {
        
        public List<Value> FetchData(string url)
        {
            WebClient webClient = new WebClient();

            var urlToDataQuery = url + "&$skip=";
            int skipValue = 0;
            bool hasMoreData = true;
            var result = new List<Value>();

            while (hasMoreData)
            {
                var tempUrl = urlToDataQuery + skipValue.ToString();

                try
                {
                    var fetchedData = webClient.DownloadString(tempUrl);
                    var tempJson = JsonConvert.DeserializeObject<OData>(fetchedData);

                    result.AddRange(tempJson.Value);

                    if (fetchedData.Contains("odata.nextLink"))
                    {
                        skipValue += 1500;
                    }
                    else
                    {
                        hasMoreData = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Pārbaudiet -source parametra pareizību un meginiet velreiz");
                    break;
                }
            }
            return result;
        }

        public string GenerateUrlFromDistrictId(int disctrictId, string userUrl)
        {
            return userUrl + "?$filter=district_id%20eq%20" + disctrictId.ToString();
        }
    }
}
