using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using FreakyFashionWebAPI.Data;
using FreakyFashionWebAPI.Domain;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public ProductsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    // GET https://localhost:8000/products(?title=1)

    /// <summary>
    /// Hämta alla produkter
    /// </summary>
    /// <param name="title">Title att filtrera på</param>
    /// <returns>Produkter</returns>
    [HttpGet]
    [Produces("application/json")]
    public IEnumerable<ProductDto> GetProducts()
    {
        var products = context.Products.ToList();

        var response = products.Select(product => new ProductDto
        {
            Id = product.Id, // Add Id property
            ProductName = product.ProductName,
            StockKeepingUnit = product.StockKeepingUnit,
            Description = product.Description,
            Image = product.Image,
            Price = product.Price
        });

        return response;
    }

    // GET https://localhost:8000/products/1
    /// <summary>
    /// Hämta produkt
    /// </summary>
    /// <param name="id">ID på produkt</param>
    /// <returns>Produkt</returns>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ProductDto> GetProduct(int id)
    {
        var product = context.Products.FirstOrDefault(x => x.Id == id);

        if (product == null)
            return NotFound(); // 404 Not Found

        var productDto = new ProductDto
        {
            Id = product.Id, // Add Id property
            ProductName = product.ProductName,
            StockKeepingUnit = product.StockKeepingUnit,
            Description = product.Description,
            Image = product.Image,
            Price = product.Price
        };

        return productDto;
    }

    /// <summary>
    /// Lägg till produkt
    /// </summary>
    /// <param name="createProductRequest">Information om produkten</param>
    /// <returns>Skapad produkt</returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult CreateProduct(CreateProductRequest request)
    {
        // CreateProductRequest is a DTO = Data Transfer Object

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

        return Created("", productDto); // 201 Created
    }


    // DELETE /products/{id}

    /// <summary>
    /// Raderar produkt
    /// </summary>
    /// <param name="id">ID för produkt</param>
    [HttpDelete("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteProduct(int id)
    {
        var product = context.Products.FirstOrDefault(x => x.Id == id);

        if (product == null)
        {
            return NotFound(); // 404 Not Found if the product doesn't exist
        }

        context.Products.Remove(product);
        context.SaveChanges(); // Delete the product and save changes to the database

        return NoContent(); // 204 No Content after the product has been deleted
    }
}



/// <summary>
/// Information om ny produkt
/// </summary>
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