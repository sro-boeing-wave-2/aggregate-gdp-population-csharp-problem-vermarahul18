using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace AggregateGDPPopulation

{
    public class AggregateGDP
    {
        public async Task readcsv() {

            IoOperation obj = new IoOperation();
            var task1 = obj.ReadFile(@"../../../../AggregateGDPPopulation/data/datafile.csv");
            var task2 = obj.ReadFile(@"../../../../AggregateGDPPopulation/data/country-continent.json");

            await Task.WhenAll(task1, task2);

            string csv = task1.Result;
            string jos = task2.Result;
            JObject CountryVsContinent = JObject.Parse(jos);


            string[] csv1 = csv.Split('\n');
            string[][] array2d = new string[csv1.Length][];
            int r = 0;
            foreach (string s in csv1)
            {
                string trim = s.Replace("\"", "");
                array2d[r++] = trim.Split(',');
            }

            Dictionary<string, class2> dict = new Dictionary<string, class2>();

            for (int i = 1; i < array2d.Length - 2; i++) {
                if (!dict.ContainsKey((string)CountryVsContinent[array2d[i][0]])) {
                    dict[(string)CountryVsContinent[array2d[i][0]]] = new class2();
                    dict[(string)CountryVsContinent[array2d[i][0]]].GDP_2012 = double.Parse(array2d[i][7]);
                    dict[(string)CountryVsContinent[array2d[i][0]]].POPULATION_2012 = double.Parse(array2d[i][4]);
                }
                else {
                    dict[(string)CountryVsContinent[array2d[i][0]]].GDP_2012 += double.Parse(array2d[i][7]);
                    dict[(string)CountryVsContinent[array2d[i][0]]].POPULATION_2012 += double.Parse(array2d[i][4]);
                }
            }


            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            await obj.WriteFile(@"../../../../AggregateGDPPopulation/data/output.json", json);
        }



        public class class2
        { 
            public double GDP_2012 { get; set; }
            public double POPULATION_2012 { get; set; }
        }

        public class IoOperation
        {
            public async Task<string> ReadFile(string filepath)
            {
                StreamReader file = new StreamReader(filepath);
                string task = await file.ReadToEndAsync();
                return task;
            }

            public async Task WriteFile(string filepath, string json)
            {
                StreamWriter write = new StreamWriter(filepath);
                await write.WriteAsync(json);
                write.Close();
            }
        }
    }
}
