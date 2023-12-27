namespace SocialMedia_Web.Models
{
    public class ApiDataResponse
    {
        public List<ArticleDetailDto> Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
