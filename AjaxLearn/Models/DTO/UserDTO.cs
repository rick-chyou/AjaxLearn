namespace AjaxLearn.Models.DTO
{
    public class UserDTO
    {
        public string? Name {  get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }

        public IFormFile? File { get; set; }
    }
}
