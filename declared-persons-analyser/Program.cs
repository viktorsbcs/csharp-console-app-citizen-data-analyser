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

            //CommandLine NuGet package automātiski pārso Options objektu no lietotāja cmd input parametriem
            Options userInputData = argsParser.ParseArgs(args);

            //Default URL ir definēts Source field attribūtā Options klasē, kas ar pārseri automātiski tiek iekļauts, 
            //gadījumos, ja lietotājs nav definējis savu URL
            string url = webServices.GenerateUrlFromDistrictId(userInputData.District, userInputData.Source);

            //Datu iegūšana no DeclaredPersons API, desiarelizēti no JSON string uz Value objektiem
            List<Value> response = webServices.FetchData(url);

            //Datu filtrēšana pēc lietotāja definētiem parametriem
            List<Value> filteredData = dataServices.FilterData(userInputData, response);

            //Atfiltrēto datu sagatavošana JSON serializācijai
            DataOutput outputData = outputServices.PrepareDataForOutput(filteredData);

            //Ģenerēta json string izvaddati
            string jsonString = outputServices.GenerateJsonString(outputData);

            //Dati tiek printēti konsolā ar ConsoleTable NuGet package palīdzību
            outputServices.PrintDataToConsole(outputData);

            //Text fails are json tiek saglabāts, ja lietotājs ir norādjis faila nosaukumu -out parametrā, 
            //pēc saglabāšans tekst fails ar json tiek automātiski atvērts. Saglabātais fails atrodas projekta direktorijā
            outputServices.SaveJsonToFile(userInputData.Out, jsonString);


        }
    }
}
