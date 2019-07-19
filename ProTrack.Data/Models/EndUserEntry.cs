namespace ProTrack.Data.Models
{
    public class EndUserEntry
    {
        public int Id { get; set; }
        public string ManufacturerName { get; set; }
        public string ProductName { get; set; }
        public string LocationName { get; set; }
        public virtual Device Device { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public Product Product { get; set; }
        public virtual Location Location { get; set; }
    }
}
