namespace ProTrack.Models.Search
{
    public class SearchListingModel
    {
        public int Id { get; set; }
        public string Firmware { get; set; }
        public string MacAddress { get; set; }
        public int Quantity { get; set; }
        public string ManufacturerName { get; set; }
        public int ManufacturerId { get; set; }
        public string ProductName { get; set; }
        public string LocationName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
