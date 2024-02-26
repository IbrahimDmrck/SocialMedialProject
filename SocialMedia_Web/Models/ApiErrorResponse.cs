namespace SocialMedia_Web.Models
{
    public class ApiErrorResponse
    {
        public List<ValidationError> ValidationErrors { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object AttemptedValue { get; set; }
        public object CustomState { get; set; }
        public int Severity { get; set; }
        public string ErrorCode { get; set; }
        public Dictionary<string, string> FormattedMessagePlaceholderValues { get; set; }
    }
}
