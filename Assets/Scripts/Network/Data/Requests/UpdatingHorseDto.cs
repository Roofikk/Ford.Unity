namespace Ford.WebApi.Data
{
    public class UpdatingHorseDto
    {
        public long HorseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}