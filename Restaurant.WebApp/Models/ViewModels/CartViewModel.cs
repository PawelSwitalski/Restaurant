namespace Restaurant.WebApp.Models.ViewModels
{
    public class CartViewModel
    {
        public Cart? Cart { get; set; } = new();

        public string ReturnUrl { get; set; } = "/";
    }
}
