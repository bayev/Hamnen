using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Linq;


namespace ThePort
{
    class Program
    {
        //List for incoming boats
        static List<Boat> boats = new List<Boat>();

        //List array for the harbour
        static List<Boat>[] harbour = new List<Boat>[64];

        //Days parked in the port, counting the days in port

        static int portDays = 0;

    
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            //Generating List in the array Harbour
            MakingListInArray(harbour);
            Console.WriteLine("");
            Console.WriteLine("********************** --> Welcome To THe Port!<-- **********************");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("To start the simulation of the Port please press [Enter] and ESC to exit");
            Console.WriteLine("");
            Console.WriteLine("");

            bool loop = true;

            while (loop)
            {
                
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    //Listning to Enter press 
                    case ConsoleKey.Enter:
                        //Adding days in PortDays
                        portDays++;
                        
                        //Creating new incoming boats to The Port
                        GenerateBoats(boats);

                        //Counter how many days left in the port
                        PortDays(harbour);

                        // Parkt he bots in harbour
                        ParkingBoatsInHarbour(harbour, boats);

                        Console.WriteLine($"***************Days in simulation: {portDays} ***************");
                        Console.WriteLine("--------------------------------------------------------------------------");
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("Bye Bye");
                        loop = false;
                        break;
                    
                }
            }

           

        }

        private static void ParkingBoatsInHarbour(List<Boat>[] harbour, List<Boat> boats)
        {
            throw new NotImplementedException();
        }

        private static void PortDays(List<Boat>[] harbour)
        {
            for (int i = 0; i < harbour.Length; i++)
            {
                if (harbour[i].Count == 0)
                {
                    continue;
                }
                else
                {
                    if (harbour[i + 1].Count == 0 || i == harbour.Length - 1)
                    {
                        harbour[i].First().DockingsDays--;
                    }
                    else if (harbour[i].First().ID != harbour[i + 1].First().ID)
                    {
                        harbour[i].First().DockingsDays--;
                    }
                }
            }
            for (int i = 0; i < harbour.Length; i++)
            {
                if (harbour[i].Count == 0)
                {
                    continue;
                }
                else
                {
                    if (harbour[i].First().DockingsDays == 0)
                    {
                        harbour[i].Clear();
                    }
                }
            }
        }




        //Generate Lists in each index in array
        private static void MakingListInArray(List<Boat>[] harbour)
        {
            for (int i = 0; i < harbour.Length; i++)
            {
                harbour[i] = new List<Boat>();
            }
        }




        // Generate Random incoming boats
        public static void GenerateBoats(List<Boat> boats)
        {

            int amount = 5;
            for (int i = 0; i < amount; i++)
            {
                int rndBoat = rnd.Next(1, 5);

                switch (rndBoat)
                {
                    case 1:
                        RowBoat rBoat = new RowBoat();
                        rBoat.Weight = rnd.Next(100, 300);
                        rBoat.MaxSpeed = rnd.Next(1, 3);
                        rBoat.Passangers = rnd.Next(1, 6);
                        rBoat.BoatType = "Row Boat";
                        rBoat.DockingsDays = 1;
                        string rboatID = GenerateID("R-");
                        rBoat.ID = rboatID;
                        boats.Add(rBoat);
                        break;
                    case 2:
                        SpeedBoat spBoat = new SpeedBoat();
                        spBoat.Weight = rnd.Next(200, 3000);
                        spBoat.MaxSpeed = rnd.Next(1, 60);
                        spBoat.HorsePower = rnd.Next(10, 1000);
                        spBoat.BoatType = "Speed Boat";
                        spBoat.DockingsDays = 3;
                        string spboatID = GenerateID("M-");
                        spBoat.ID = spboatID;
                        boats.Add(spBoat);
                        break;
                    case 3:
                        SailBoat sBoat = new SailBoat();
                        sBoat.Weight = rnd.Next(800, 6000);
                        sBoat.MaxSpeed = rnd.Next(1, 12);
                        sBoat.BoatLenght = rnd.Next(10, 60);
                        sBoat.BoatType = "Sail Boat";
                        sBoat.DockingsDays = 4;
                        string sboatID = GenerateID("S-");
                        sBoat.ID = sboatID;
                        boats.Add(sBoat);
                        break;
                    case 4:
                        CargoBoat cBoat = new CargoBoat();
                        cBoat.Weight = rnd.Next(3000, 20000);
                        cBoat.MaxSpeed = rnd.Next(1, 20);
                        cBoat.conteiners = rnd.Next(0, 500);
                        cBoat.BoatType = "Cargo Ship";
                        cBoat.DockingsDays = 6;
                        string cboatID = GenerateID("C-");
                        cBoat.ID = cboatID;
                        boats.Add(cBoat);
                        break;

                    default:
                        break;
                }
            }
        }


        // Generate unic ides to boats
        public static string GenerateID(string s)
        {
            for (int i = 0; i < 3; i++)
            {
                char randomChar = (char)rnd.Next('A', 'Z');
                s += randomChar;
            }
            return s;
        }


    }
}
