using System;

namespace FoodTruckFunction
{
    internal class FoodTruck
    {
        public string ObjectId { get; set; }
        public string Applicant { get; set; }
        public string FacilityType { get; set; }
        public string Cnn { get; set; }
        public string Address { get; set; }
        public string BlockLot { get; set; }
        public string Block { get; set; }
        public string Lot { get; set; }
        public string Permit { get; set; }
        public string Status { get; set; }
        public string FoodItems { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Schedule { get; set; }
        public string Approved { get; set; }
        public string Received { get; set; }
        public string PriorPermit { get; set; }
        public string ExpirationDate { get; set; }
        public Location Location { get; set; }
        public string ComputedRegionYftqJ783 { get; set; }
        public string ComputedRegionP5ajWyqh { get; set; }
        public string ComputedRegionRxqgMtj9 { get; set; }
        public string ComputedRegionBh8sQ3mv { get; set; }
        public string ComputedRegionFyvsAhh9 { get; set; }
    }

    internal class Location
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string HumanAddress { get; set; }
    }
}
