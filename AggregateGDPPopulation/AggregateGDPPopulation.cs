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
    public class Class1
    {
       
        public async void readcsv() {
            
            StreamReader file = new StreamReader(@"../../../../AggregateGDPPopulation/data/datafile.csv");
            Task<string> task1 = file.ReadToEndAsync();


            StreamReader js = new StreamReader(@"../../../../AggregateGDPPopulation/data/country-continent.json");
            Task<string> task2 = js.ReadToEndAsync();

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
           
            Console.Write("second lie");
           // JObject CountryVsContinent = JObject.Parse(File.ReadAllText(@"../../../../AggregateGDPPopulation/data/country-continent.json"));
           // Dictionary<string, Dictionary<string,double>> dict = new Dictionary<string, Dictionary<string, double>>();

            //string[][] array2d=await (datafile());
            //JObject CountryVsContinent = await Continent();
            //Task<string[][]> task1 = datafile();
            //Task<JObject> task2 = Continent();
            //await Task.WhenAll(task1, task2);

            Dictionary<string, class2> dict = new Dictionary<string, class2>();

            //Console.Write(CountryVsContinent[array2d[1][0]]);
            for (int i = 1; i < array2d.Length - 2; i++) {
                if (!dict.ContainsKey((string)CountryVsContinent[array2d[i][0]])) {
                    dict[(string)CountryVsContinent[array2d[i][0]]] = new class2();
                    dict[(string)CountryVsContinent[array2d[i][0]]].GDP_2012 = double.Parse(array2d[i][7]);
                    dict[(string)CountryVsContinent[array2d[i][0]]].POPULATION_2012 = double.Parse(array2d[i][4]);
                    //dict[(string)CountryVsContinent[array2d[i][0]]] = two;
                }
                else {
                    dict[(string)CountryVsContinent[array2d[i][0]]].GDP_2012 += double.Parse(array2d[i][7]);
                    dict[(string)CountryVsContinent[array2d[i][0]]].POPULATION_2012 += double.Parse(array2d[i][4]);
                }
            }
           
            
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
             System.IO.File.WriteAllText(@"../../../../AggregateGDPPopulation/data/output.json", json);
            //Console.WriteLine(json);
         
        }


        public class class2 {
            public double GDP_2012 { get; set; }
            public double POPULATION_2012 { get; set; }
        }
    }
}
