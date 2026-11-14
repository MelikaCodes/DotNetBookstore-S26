using Microsoft.AspNetCore.Identity;

namespace DotNetBookstore.Models;

public class Order
{
    public int OrderId { get; set; }
    public required string CustomerId { get; set; }
    public IdentityUser Customer { get; set; } = null!;
    public Guid CheckoutId { get; set; }
    public DateTime PlacedAtUtc { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
}

public class OrderLine
{
    public int OrderLineId { get; set; }
    public int OrderId { get; set; }
    public required string Title { get; set; }
    public int Quantity { get; set; }
    public int UnitPriceCents { get; set; }
}

public record CartLine(Book Book, int Quantity);

public record CartViewModel(IReadOnlyList<CartLine> Lines, Guid CheckoutId);
