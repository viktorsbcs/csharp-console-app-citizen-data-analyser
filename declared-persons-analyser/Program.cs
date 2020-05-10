using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using declared_persons_analyser.Models;
using declared_persons_analyser.Services;
using declared_persons_analyser.Utilities;
using Newtonsoft.Json;


namespace declared_persons_analyser
{
    public class Program
    {
        static void Main(string[] args)
        {
            ParserHandler argsParser = new ParserHandler();
            WebServices webServices = new WebServices();
            DataServices dataServices = new DataServices();
            OutputServices outputServices = new OutputServices();

            Options userInputData = argsParser.ParseArgs(args);

            string url = webServices.GenerateUrlFromDistrictId(userInputData.District, userInputData.Source);

            List<Value> response = webServices.FetchData(url);

            List<Value> filteredData = dataServices.FilterData(userInputData, response);

            DataOutput outputData = outputServices.PrepareDataForOutput(filteredData);

            string jsonString = outputServices.GenerateJsonString(outputData);

            outputServices.PrintDataToConsole(outputData);

            outputServices.SaveJsonToFile(userInputData.Out, jsonString);


        }
    }
}
