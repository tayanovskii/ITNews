namespace ITNews.DTO.Rating
{
    public class CreateRatingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public short Value { get; set; }
        public int NewsId { get; set; }
    }
}
