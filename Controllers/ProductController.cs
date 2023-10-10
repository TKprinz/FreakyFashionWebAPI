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
    public IEnumerable<ProductDto> GetProducts()
    {
        var products = context.Products.ToList();

        // [].map() = projicering
        var response = products.Select(product => new ProductDto   // List<ProductDto>
        {
            ProductName = product.ProductName,
            StockKeepingUnit = product.StockKeepingUnit,
            Description = product.Description,
            Image = product.Image,
            Price = product.Price
        });

        return response;
    }

    [HttpPost]
    public ActionResult CreateProduct(CreateProductRequest request)
    {
        // CreateProductRequest är en DTO = Data Transfer Object

        var product = new Product
        {
            ProductName = request.ProductName,
            StockKeepingUnit = request.StockKeepingUnit,
            Description = request.Description,
            Image = request.Image,
            Price = request.Price

        };

        context.Products.Add(product);

        context.SaveChanges();

        var productDto = new ProductDto
        {
            ProductName = product.ProductName,
            StockKeepingUnit = product.StockKeepingUnit,
            Description = product.Description,
            Image = product.Image,
            Price = product.Price
        };

        return Created("", productDto);  // 201 Created
    }
}


public class CreateProductRequest
{
    public required string ProductName { get; set; }

    public required string StockKeepingUnit { get; set; }

    public required string Description { get; set; }

    public required string Image { get; set; }

    public required string Price { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }

    public required string ProductName { get; set; }

    public required string StockKeepingUnit { get; set; }

    public required string Description { get; set; }

    public required string Image { get; set; }

    public required string Price { get; set; }
}