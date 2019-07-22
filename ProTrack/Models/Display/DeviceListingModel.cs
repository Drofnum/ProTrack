namespace ProTrack.Models.Display
{
    public class DeviceListingModel
    {
        public int Id { get; set; }
        public string Firmware { get; set; }
        public string MacAddress { get; set; }
        public int Quantity { get; set; }
        public string ManufacturerName { get; set; }
        public string ProductName { get; set; }
    }
}
