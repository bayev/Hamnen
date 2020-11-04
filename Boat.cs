using System;
using System.Collections.Generic;
using System.Text;

namespace ThePort
{
    class Boat
    {

        public string ID { get; set; }
        public int Weight { get; set; }
        public int MaxSpeed { get; set; }
        public int DockingsDays { get; set; }
        public string BoatType { get; set; }
        public double BoatSize { get; set; }
    }

    class RowBoat : Boat
    {

        public int Passangers { get; set; }

    }

    class SpeedBoat : Boat
    {

        public int HorsePower { get; set; }
    }

    class SailBoat : Boat
    {

        public int BoatLenght { get; set; }
    }

    class CargoBoat : Boat
    {

        public int conteiners { get; set; }
    }
}
