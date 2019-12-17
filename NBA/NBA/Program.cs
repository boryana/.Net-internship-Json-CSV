using Json.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace NBA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the path to the json file.");
            var pathToJson = Console.ReadLine();
            if (!File.Exists(pathToJson))
            {
                Console.WriteLine("File does not exist.");
                throw new ArgumentException("You must supply a valid path");
            }
            string path = pathToJson; 
            StreamReader str = new StreamReader(path);
            string file = str.ReadToEnd();
            
            var players = JsonConvert.DeserializeObject<List<NBA>>(file);

            Console.WriteLine("Please enter a maximum number of years the player has played in the league");
            string InputMaxYears = Console.ReadLine();
            uint enteredMaximumYears;
            if (!uint.TryParse(InputMaxYears, out enteredMaximumYears))
            {
                throw new System.ArgumentException("Parameter must be a positive number", "Max age value Error");
            }
            long minimumYear = DateTime.Now.Year - enteredMaximumYears;

            Console.WriteLine("Please enter a minimum rating the player should have to qualify");
            string inputMinimumRating = Console.ReadLine();
            decimal enteredMinimumRating;
            if (!decimal.TryParse(inputMinimumRating, out enteredMinimumRating))
            {
                throw new System.ArgumentException("Parameter must be a real number");
            }
            
            List<NBA> list = new List<NBA>();
            foreach(var p in players)
            {
                if ((p.Rating > enteredMinimumRating) & (p.PlayerSince < minimumYear))
                {
                    list.Add(p);
                }
                        
            }
            list = list.OrderByDescending(x => x.Rating).ThenBy(x=>x.Name).ToList();
            string headerNames = "Name,Rating\r\n";
            string csv = headerNames;
            foreach (var l in list)
            {
                csv += string.Format("{0}, {1}\r\n", l.Name, l.Rating);
            }
            Console.WriteLine("Please enter the path to the CSV file.");
            string destinationPath = Console.ReadLine();
            File.WriteAllText(destinationPath, csv);
            csv = null;
        }
    }
}
