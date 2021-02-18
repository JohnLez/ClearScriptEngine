using Microsoft.ClearScript.JavaScript;
using Microsoft.JScript;
using Newtonsoft.Json;
using ScriptingLib;
using ScriptingLib.DAL;
using ScriptingLib.WebAccess;
using ScriptsTester.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptsTester
{
    class Program
    {
        static Stopwatch timer = new Stopwatch();
        static void Main(string[] args)
        {
            Thread.Sleep(100);
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string connectionString = "Server=JB-DESKTOP\\SQLEXPRESS01;Database=ScriptEngineDemoDb;Integrated Security = true;";
            DbAccessProvider dbAccess = new DbAccessProvider(connectionString);
            WebProvider webAccess = new WebProvider();
            var provider = new ScriptingProvider(dbAccess, webAccess);


            //Example 1
            //input -> object[] with 3 integers inside
            //output of script -> json string describing new object[] with property "name" and value, one of the above array values
            //output of ExecuteScript -> the deserialized result of the script to the defined object of type Example1Object
            Console.WriteLine("Executing 1st example..");

            timer.Start();
            var scriptPath = Path.Combine(currentDir, "Scripts/example1.js");
            var content = File.ReadAllText(scriptPath);
            var result = provider.ExecuteScript<List<Example1Object>>(content, new object[] { 1, 2, 3 }); // <------- script executes here
            timer.Stop();
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
            timer.Reset();
            result.ForEach(i => Console.WriteLine(i.name));
            Console.WriteLine("\nPress any key to execute next example");
            Console.ReadLine();

            //Example 2
            //input => an array of cars to be processed inside the script
            //output of script => the same car[] passed in after modifying the values
            //output of ExecuteScript => It will be a dynamic. So in order to manipulate it as a car[] we need to cast it back
            Console.WriteLine("Executing 2nd example..");

            var sampleCars = GenerateCars();
            Console.WriteLine("Car values before they enter the script");
            sampleCars.ForEach(c => Console.WriteLine($"Car{c.Id} values:\n" + c));
            Console.WriteLine();
            timer.Start();
            scriptPath = Path.Combine(currentDir, "Scripts/carsExample.js");
            content = File.ReadAllText(scriptPath);
            var modifiedCars = provider.ExecuteScript(content, sampleCars.ToArray());
            Car[] carsCastedBack = (Car[])modifiedCars;
            timer.Stop();
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
            timer.Reset();
            Console.WriteLine("\nPress any key to execute next example");
            Console.ReadLine();


            //Example 3
            //input => A generated car[] 
            //Script => it will modify the [] then insert the records in database 
            //Script output => null 
            //ExecuteScript output => null
            Console.WriteLine("Executing 3d example..");

            var sampleCars2 = GenerateCars();
            Console.WriteLine("Car values before they enter the script");
            sampleCars2.ForEach(c => Console.WriteLine($"Car{c.Id} values:\n" + c));
            Console.WriteLine();
            timer.Start();
            scriptPath = Path.Combine(currentDir, "Scripts/carsExampleSQL.js");
            content = File.ReadAllText(scriptPath);
            var modifiedCars2 = provider.ExecuteScript(content, sampleCars2.ToArray());
            timer.Stop();
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
            timer.Reset();
            Console.WriteLine("\nPress any key to execute next example");
            Console.ReadLine();


            //Example 4
            //input => null
            //Script => it queries database and accesses object values by property to print them
            // Script & Execute script output => null
            Console.WriteLine("Executing 4th example..");

            timer.Start();
            scriptPath = Path.Combine(currentDir, "Scripts/carsQueryExampleSQL.js");
            content = File.ReadAllText(scriptPath);
            var modifiedCars3 = provider.ExecuteScript(content, null);
            timer.Stop();
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
            timer.Reset();
            Console.WriteLine("\nPress any key to execute next example");
            Console.ReadLine();


            //Example 5 
            //input => null
            //Script => first it pings google and prints the httpresponsemessage object
            //then it calls an api with Jokes. After receiving the response it deserializes it and uses the value to print the jokes
            //Script & Method outputs => null
            Console.WriteLine("Executing 5th example..");

            timer.Start();
            scriptPath = Path.Combine(currentDir, "Scripts/webRequestExample.js");
            content = File.ReadAllText(scriptPath);
            var modifiedCars4 = provider.ExecuteScript(content, null);
            timer.Stop();
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
            timer.Reset();
            Console.WriteLine("\nPress any key to execute next example");
            Console.ReadLine();



            //Example 6
            //input => ScriptsTest.Models.Car Type 
            //Scrit => creates a C# genericList of type Car and populates it with cars based on a condition
            //Script output => a List<cars>
            //Execute method output => dynamic , so a cast is requred
            Console.WriteLine("Executing 6th example..");
            timer.Start();
            scriptPath = Path.Combine(currentDir, "Scripts/extendedFunctionsExample.js");
            content = File.ReadAllText(scriptPath);
            var output = provider.ExecuteScript(content, null, new List<Type>() { typeof(Car) });
            timer.Stop();
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
            Console.WriteLine("Casting script output to List<car> and printing information");
            ((List<Car>)output).ForEach(c => Console.WriteLine(c));
            timer.Reset();
            Console.WriteLine();



            Console.ReadLine();
        }


        private static List<Car> GenerateCars()
        {
            var returnList = new List<Car>();
            for (int i = 0; i < 5; i++)
            {
                returnList.Add(new Car()
                {
                    Id = i,
                    Brand = $"TestBrand{i}",
                    ModelName = $"TestModel{i}",
                    DateManufactured = DateTime.Now
                });
            }
            return returnList;
        }
    }
}
