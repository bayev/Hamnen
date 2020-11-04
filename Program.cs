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
        static int crj = 0;

    
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

                        //Display all boats in console
                        DisplayBoat(harbour);

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

        private static void DisplayBoat(List<Boat>[] harbour)
        {
            Console.WriteLine($"Plats \t Båttyp \t ID \t Vikt \t MaxHastighet \t Unika egenskaper");
            Console.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < harbour.Length; i++)
            {
                if (harbour[i].Count == 0)
                {
                    Console.WriteLine($"{i}\t Tomt");
                }
                else
                {

                    foreach (Boat boat in harbour[i])
                    {
                        if (boat is RowBoat)
                        {
                            Console.WriteLine($"{i}\t {boat.BoatType} \t {boat.ID} \t {boat.Weight} \t {boat.MaxSpeed} km/h \t {(((RowBoat)boat).Passangers)}\tpassagerare");
                        }
                        else if (boat is SpeedBoat)
                        {
                            Console.WriteLine($"{i}\t {boat.BoatType} \t {boat.ID} \t {boat.Weight} \t {boat.MaxSpeed} km/h \t {(((SpeedBoat)boat).HorsePower)}\thästkrafter");
                        }
                        else if (boat is SailBoat)
                        {
                            Console.WriteLine($"{i}\t{boat.BoatType} \t {boat.ID} \t {boat.Weight} \t {boat.MaxSpeed} km/h \t {(((SailBoat)boat).BoatLenght)} \t m");
                        }
                        else if (boat is CargoBoat)
                        {
                            Console.WriteLine($"{i}\t{boat.BoatType} \t {boat.ID} \t {boat.Weight} \t {boat.MaxSpeed} km/h \t {(((CargoBoat)boat).conteiners)}\tContainrar");
                        }

                    }
                }


            }
        }

        private static void ParkingBoatsInHarbour(List<Boat>[] harbour, List<Boat> boats)
        {
            //Sorting the List
            List<Boat> SortedBoats = boats.OrderByDescending(b => b.BoatSize).ToList();

            //för varje båt i listan, försök att placera den i hamnen, börjar med största båten först
            foreach (Boat b in SortedBoats)
            {

                //metoden returnerar en bool om den fick lägga till eller inte
                if (PlaceBoatInHarbour(harbour, b))
                {
                    //boat is in harbour
                }
                else
                {
                    //Avvisa båt
                    Console.WriteLine("Avvisad båt" + b.ID + "\t" + b.BoatType);
                    crj++;

                }
            }
        }

        private static bool PlaceBoatInHarbour(List<Boat>[] harbour, Boat cboat)
        {
            for (int i = 0; i < harbour.Length; i++)
            {
                if (cboat is RowBoat && harbour[i].Count == 1 && harbour[i].First() is RowBoat)
                {

                    harbour[i].Add(cboat);
                    return true;

                }

                //Om platsen är tom och edge-case om listan tar slut
                if (harbour[i].Count == 0 && cboat.BoatSize + i < harbour.Length)
                {

                    int startIndex = i;
                    //Närliggande lediga platser
                    int numOfAdjacent = 0;
                    //Kollar om de nästkommande platserna också är tomma, t.o.m. båtens storlek
                    for (int j = startIndex; j < startIndex + cboat.BoatSize; j++)
                    {
                        //om dom är det adderar vi
                        if (harbour[j].Count == 0)
                        {
                            numOfAdjacent++;
                        }
                    }
                    //Om alla nästkommande platser tom båtens storlek är tomma kan får båten plats, så vi lägger till.
                    if (numOfAdjacent == cboat.BoatSize)
                    {

                        //samma loop som förut, men nu vet vi att alla platser är tomma så då lägger vi till båten på dessa platser/index. 
                        for (int j = startIndex; j < startIndex + cboat.BoatSize; j++)
                        {
                            harbour[j].Add(cboat);

                        }
                        //Vi kunde lägga till båten --> true
                        return true;

                    }
                   
                    
                }

            }
            //fall vi kör igenom hela for-loopen och inget händer får vi avvisa båten
            return false;

        }



        //Checking the days on bots

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
                        rBoat.BoatSize = 1;
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
                        spBoat.BoatSize = 1;
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
                        sBoat.BoatSize = 2;
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
                        cBoat.BoatSize = 4;
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
