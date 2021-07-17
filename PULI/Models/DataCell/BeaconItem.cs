using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataCell
{
    public class BeaconItem
    {
        LimitedQueue<double> previousDistances;
        const double tolerance = 0.2;

        private bool isBLE = true;
        public bool isView { get; set; }

        public double PreviousAverage;
        public DateTime MovementChangeTimestamp;
        public Movement CurrentMovement;
        public DateTime ProximityChangeTimestamp;

        public string Minor { get; set; }
        public Proximity Proximity { get; set; }

        public string Adr { get; set; }

        private double rssi;
        public double Rssi
        {
            get { return calculateDistance(rssi); }
            set { rssi = value; }
        }

        private string name;
        public string Name
        {
            get { return string.IsNullOrEmpty(name) ? name : name; }
            set { name = value; }
        }

        private string MinorToName(string minor)
        {
            switch (minor)
            {
                case "17284":
                    minor = "文心蘭盆花";
                    break;
                case "41505":
                    minor = "牡丹盆花";
                    break;

                case "52865":
                    minor = "日月潭樹石展";
                    break;

                case "45346":
                    minor = "五葉松";
                    break;

                default:
                    break;
            }
            return minor;
        }

        private double calculateDistance(double rssi)
        {
            var txPower = -59; //hard coded power value. Usually ranges between -59 to -65


            if (rssi == 0)
            {
                return -1.0;
            }

            var ratio = rssi * 1.0 / txPower;
            if (ratio < 1.0)
            {
                return Math.Pow(ratio, 10);
            }
            else
            {
                var distance = (0.89976) * Math.Pow(ratio, 7.7095) + 0.111;
                return distance;
            }
        }

        public string DistanceString
        {
            get { return isBLE ? Rssi + "m" : CurrentDistance + "m"; }
        }

        public double CurrentDistance
        {
            get { return previousDistances.Last(); }
            set
            {
                isBLE = false;
                if (previousDistances != null && previousDistances.Count > 0)
                {
                    PreviousAverage = previousDistances.Average();
                }
                previousDistances.Enqueue(value);
                var newMovement = GetMovement(previousDistances.Average() - PreviousAverage);

                if (CurrentMovement == Movement.None)
                {
                    CurrentMovement = newMovement;
                    MovementChangeTimestamp = DateTime.Now;
                }
                else if (CurrentMovement != newMovement)
                {
                    CurrentMovement = newMovement;
                    MovementChangeTimestamp = DateTime.Now;
                }
            }
        }

        public BeaconItem()
        {
            previousDistances = new LimitedQueue<double>(5);
            MovementChangeTimestamp = DateTime.Now;
            ProximityChangeTimestamp = DateTime.Now;
        }

        public double GetAverage()
        {
            return previousDistances.Average();
        }

        public Movement GetMovement(double diff)
        {
            if (Math.Abs(diff) < tolerance)
            {
                return Movement.Stationary;
            }
            else if (diff > 0)
            {
                return Movement.Away;
            }
            else
            {
                return Movement.Toward;
            }
        }
    }

    public enum Movement
    {
        None,
        Stationary,
        Toward,
        Away
    }

    public enum Proximity
    {
        Unknown,
        Immediate,
        Near,
        Far
    }

    public class LimitedQueue<T> : Queue<T>
    {
        private int limit = -1;

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        public LimitedQueue(int limit)
            : base(limit)
        {
            this.Limit = limit;
        }

        public new void Enqueue(T item)
        {
            if (this.Count >= this.Limit)
            {
                this.Dequeue();
            }
            base.Enqueue(item);
        }
    }
}