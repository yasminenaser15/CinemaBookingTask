namespace CinemaBookingTask.Models
{
    public class MovieSubImg
    {
        public int Id { get; set; }
        public string SubImg { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
