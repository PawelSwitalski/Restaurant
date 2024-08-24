using Restaurant.Business.Models;
using Restaurant.WebApp.Infrastructure;
using System.Text.Json.Serialization;

namespace Restaurant.WebApp.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        public override void AddItem(ProductModel product, int quantity)
        {
            base.AddItem(product, quantity);
            this.Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(ProductModel product)
        {
            base.RemoveLine(product);
            this.Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            this.Session?.Remove("Cart");
        }
    }
}
