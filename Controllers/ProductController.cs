using FreakyFashionWebAPI.Domain;
using FreakyFashionWebAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public ProductsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IEnumerable<Product> GetProducts()
    {
        var products = context.Products.ToList();

        return products;

    }
}