namespace SocialMedia_Web.Models
{
    public class ApiListDataResponse<T>
    {
        public List<T> Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
