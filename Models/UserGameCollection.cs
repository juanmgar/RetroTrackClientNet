using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RetroTrack.Models
{
    public class UserGameCollection
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string User { get; set; }

        public string GameTitle { get; set; }

    }
}
