using DotNetBookstore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetBookstore.Controllers;

[Authorize]
public class OrdersController(ApplicationDbContext database, UserManager<IdentityUser> users) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var customerId = users.GetUserId(User);
        if (customerId is null)
            return Challenge();

        var orders = await database.Orders.AsNoTracking().Include(order => order.Lines)
            .Where(order => order.CustomerId == customerId)
            .OrderByDescending(order => order.OrderId).ToListAsync(cancellationToken);
        return View(orders);
    }
}
