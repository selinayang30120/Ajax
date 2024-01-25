namespace WebApplication3.Models.DTO
{
    public class SpotPagingDTO
    {
        public int TotalPages {  get; set; }
        public List<SpotImagesSpot>? SpotsResult { get; set; }
    }
}
