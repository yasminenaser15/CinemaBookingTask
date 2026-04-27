namespace CinemaBookingTask.Services
{
    public enum MovieImgType
    {
        MainImg,
        SubImg
    }
    public class MovieService
    {
        public string? SaveImg(IFormFile img, MovieImgType imgType = MovieImgType.MainImg)
        {
            try
            {
                var fileName = $"{DateTime.Now.ToString("dd_MM_yyyy")}_{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";

                string filePath = string.Empty;

                switch (imgType)
                {
                    case MovieImgType.MainImg:
                        filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\movies", fileName);
                        break;
                    case MovieImgType.SubImg:
                        filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\movies\\movie_sub_imgs", fileName);
                        break;
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public bool RemoveImg(string imgName, MovieImgType imgType = MovieImgType.MainImg)
        {
            try
            {
                string filePath = string.Empty;

                switch (imgType)
                {
                    case MovieImgType.MainImg:
                        filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\movies", imgName);
                        break;
                    case MovieImgType.SubImg:
                        filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\movies\\movie_sub_imgs", imgName);
                        break;
                }

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
