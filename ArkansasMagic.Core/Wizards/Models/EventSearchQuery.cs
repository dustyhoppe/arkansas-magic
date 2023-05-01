namespace ArkansasMagic.Core.Wizards.Models
{
    public class EventSearchQuery
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsPremium { get; set; }
        public string Tag { get; set; } = "magic:_the_gathering";
        public string SearchType { get; set; } = "magic-events";
        public int MaxMeters { get; set; } = 150000;
        public int PageSize { get; set; } = 25;
        public int Page { get; set; } = 1;
        public string Sort { get; set; } = "date";
        public string SortDirection { get; set; } = "asc";
    }
}
