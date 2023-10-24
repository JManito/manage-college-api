namespace ManageCollege.Models.Domain
{
    public class Error
    {
        public string ErrorMessage { get; set; } = "The error message could not be set";
        public int StatusCode { get; set; }
    }
}
