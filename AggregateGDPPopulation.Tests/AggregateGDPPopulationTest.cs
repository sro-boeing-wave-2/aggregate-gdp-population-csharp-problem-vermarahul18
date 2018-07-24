using System;
using Xunit;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1 c = new Class1();
            c.readcsv();

            JObject createdfile = JObject.Parse(File.ReadAllText(@"../../../../AggregateGDPPopulation/data/output.json"));
            JObject expectedfile = JObject.Parse(File.ReadAllText(@"../../../../AggregateGDPPopulation.Tests/expected-output.json"));
            
            Console.WriteLine(createdfile);
            Console.WriteLine(expectedfile);
            Assert.Equal(createdfile, expectedfile);
        }
    }
}
