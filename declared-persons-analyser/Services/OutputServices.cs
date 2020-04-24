using ConsoleTables;
using declared_persons_analyser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace declared_persons_analyser.Services
{
    public class OutputServices
    {
        private readonly DataOutput dataOutput;

        public OutputServices()
        {
            this.dataOutput = new DataOutput();
        }

        public DataOutput PrepareDataForOutput(List<Value> filteredData)
        {
            this.dataOutput.data = CreateDataList(filteredData);
            this.dataOutput.summary = CreateSummary(filteredData);

            return this.dataOutput;
        }

        private List<Data> CreateDataList(List<Value> filteredData)
        {
            decimal change;
            List<Data> dataList = new List<Data>();

            for (int i = 0; i < filteredData.Count; i++)
            {
                change = (i == 0) ? 0 : filteredData[i].value - filteredData[i - 1].value;

                var data = new Data()
                {
                    district_name = filteredData[i].district_name,
                    year = filteredData[i].year,
                    month = filteredData[i].month,
                    day = filteredData[i].day,
                    value = filteredData[i].value,
                    change = change
                };
                dataList.Add(data);
            }
            return dataList;
        }


        private Summary CreateSummary(List<Value> filteredData)
        {
            string disctrictName = filteredData.First().district_name;
            var data = this.dataOutput.data;
            var average = data.Sum(x => x.value);

            var maxIncreaseValue = data.Max(x => x.change);
            var maxDropValue = data.Min(x => x.change);

            var maxIncreaseGroup = data.First(x => x.change == maxIncreaseValue);
            var maxDropGroup = data.First(x => x.change == maxDropValue);

            var summary = new Summary()
            {
                max = filteredData.Max(x => x.value),
                min = filteredData.Min(x => x.value),

                average = Math.Round(average / filteredData.Count(), 0),

                max_drop = new MaxDrop()
                {
                    value = maxDropValue,
                    group = GenerateSummaryGroupString(maxDropGroup),
                    disctrict_name = disctrictName
                },

                max_increase = new MaxIncrease()
                {
                    value = maxIncreaseValue,
                    group = GenerateSummaryGroupString(maxIncreaseGroup),
                    disctrict_name = disctrictName
                }
            };

            return summary;
        }

        private string GenerateSummaryGroupString(Data data)
        {
            List<string> output = new List<string>();
            if (data.year != 0) output.Add(data.year.ToString() + ".");
            if (data.month != 0) output.Add(data.month.ToString() + ".");
            if (data.day != 0) output.Add(data.day.ToString() + ".");

            string text = "";
            for (int i = 0; i < output.Count; i++)
            {
                text += output[i];
                if (i == output.Count - 1) text = text.Remove(text.Length - 1);
            }

            return text;
        }

        public string GenerateJsonString(DataOutput data)
        {
            var jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

            Console.WriteLine("\n\n\n" + jsonString + "\n\n\n");
            return jsonString;
        }

        public void PrintDataToConsole(DataOutput outputData)
        {
            var tableData = new ConsoleTable("nr","district_name", "year", "month", "value", "change").Configure(x => x.EnableCount = false);

            int nr = 1;

            foreach (var data in outputData.data)
            {
                tableData.AddRow(nr, data.district_name, data.year, data.month, data.value, data.change);
                nr++;
            }

            var s = outputData.summary;
            var tableSummary = new ConsoleTable("Max", "Min", "Average", "Max Increase", "Max Drop");
            tableSummary.AddRow(s.max, s.min, s.average, s.max_increase.value + " " + s.max_increase.group, s.max_drop.value + " " + s.max_drop.group).Configure(x => x.EnableCount = false);

            tableData.Write();
            tableSummary.Write();
            Console.WriteLine();
        }

        public void SaveJsonToFile(string fileName, string jsonString)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("\n\n\nJSON fails netika saglabāts, iekļaujiet -out parametrā faila nosaukumu");
                return;
            }

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(directory + fileName, jsonString);

            //Automatiska json text faila atvēršana
            string notepadPath = Environment.SystemDirectory + "\\notepad.exe";

            var startInfo = new ProcessStartInfo(notepadPath)
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                Arguments = fileName
            };

            Process.Start(startInfo);

            Console.WriteLine("\n\n\nJSON fails veiksmīgi saglabāts direktorijā:\n" + directory + fileName + "\n");

        }
    }
}
