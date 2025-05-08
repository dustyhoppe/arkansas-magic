namespace ArkansasMagic.Api.Configuration
{
    public class EventFiltersOptions
    {
        public const string Key = "EventFilters";
        public CoordinatesOptions Coordinates { get; set; }


        public class CoordinatesOptions
        {
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public int MaxMeters { get; set; }
        }
    }
}
