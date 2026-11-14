using System.Text.Json;
using DotNetBookstore.Data;
using DotNetBookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DotNetBookstore.Controllers;

public class CartController(ApplicationDbContext database, UserManager<IdentityUser> users) : Controller
{
    private const string SessionKey = "bookstore-cart";
    private const string CheckoutKey = "bookstore-checkout-id";
    private const int MaxQuantity = 20;

    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(new CartViewModel(await GetLinesAsync(cancellationToken), ReadCheckoutId()));

    [HttpPost]
    public async Task<IActionResult> Add(int id, CancellationToken cancellationToken)
    {
        if (!await database.Books.AnyAsync(book => book.BookId == id, cancellationToken))
            return NotFound();

        var cart = ReadCart();
        cart[id] = Math.Min(cart.GetValueOrDefault(id) + 1, MaxQuantity);
        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Update(int id, int quantity)
    {
        if (!ModelState.IsValid || quantity < 1 || quantity > MaxQuantity)
        {
            TempData["CartMessage"] = "Quantity must be between 1 and 20.";
            return RedirectToAction(nameof(Index));
        }

        var cart = ReadCart();
        if (cart.ContainsKey(id))
        {
            cart[id] = quantity;
            SaveCart(cart);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var cart = ReadCart();
        cart.Remove(id);
        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Checkout(Guid checkoutId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || checkoutId == Guid.Empty)
            return BadRequest();

        var customer = await users.GetUserAsync(User);
        if (customer is null)
            return Challenge();

        if (await OrderExistsAsync(customer.Id, checkoutId, cancellationToken))
            return RedirectToAction("Index", "Orders");

        if (checkoutId != ReadCheckoutId())
        {
            TempData["CartMessage"] = "Your cart changed. Review it and try again.";
            return RedirectToAction(nameof(Index));
        }

        var validLines = await GetLinesAsync(cancellationToken);
        if (validLines.Count == 0)
            return RedirectToAction(nameof(Index));

        var order = new Order
        {
            CustomerId = customer.Id,
            CheckoutId = checkoutId,
            PlacedAtUtc = DateTime.UtcNow,
            Lines = validLines.Select(line => new OrderLine
            {
                Title = line.Book.Title,
                UnitPriceCents = line.Book.PriceCents,
                Quantity = line.Quantity
            }).ToList()
        };

        database.Orders.Add(order);
        try
        {
            await database.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is
            SqliteException { SqliteExtendedErrorCode: 2067 } or
            PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            if (await OrderExistsAsync(customer.Id, checkoutId, cancellationToken))
                return RedirectToAction("Index", "Orders");

            throw;
        }
        HttpContext.Session.Remove(SessionKey);
        HttpContext.Session.Remove(CheckoutKey);
        TempData["OrderMessage"] = $"Demo order #{order.OrderId} placed.";
        return RedirectToAction("Index", "Orders");
    }

    private Task<bool> OrderExistsAsync(string customerId, Guid checkoutId, CancellationToken cancellationToken) =>
        database.Orders.AnyAsync(order => order.CustomerId == customerId && order.CheckoutId == checkoutId, cancellationToken);

    private Dictionary<int, int> ReadCart()
    {
        var json = HttpContext.Session.GetString(SessionKey);
        return json is null ? [] : JsonSerializer.Deserialize<Dictionary<int, int>>(json) ?? [];
    }

    private Guid ReadCheckoutId() =>
        Guid.TryParse(HttpContext.Session.GetString(CheckoutKey), out var id) ? id : Guid.Empty;

    private void SaveCart(Dictionary<int, int> cart)
    {
        HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(cart));
        HttpContext.Session.SetString(CheckoutKey, Guid.NewGuid().ToString());
    }

    private async Task<List<CartLine>> GetLinesAsync(CancellationToken cancellationToken)
    {
        var cart = ReadCart();
        if (cart.Count == 0)
            return [];

        var bookIds = cart.Keys.ToArray();
        var books = await database.Books.AsNoTracking().Where(book => bookIds.Contains(book.BookId))
            .OrderBy(book => book.Title).ToListAsync(cancellationToken);
        return books.Select(book => new CartLine(book, cart[book.BookId])).ToList();
    }
}
