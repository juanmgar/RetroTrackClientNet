namespace RetroTrack.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        public string? ScreenshotUrl { get; set; }
        public string? Description { get; set; }

    }
}
