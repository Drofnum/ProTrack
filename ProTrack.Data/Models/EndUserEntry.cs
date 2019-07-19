namespace ProTrack.Data.Models
{
    public class EndUserEntry
    {
        public int Id { get; set; }

        public virtual Device Device { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Location Location { get; set; }
    }
}
