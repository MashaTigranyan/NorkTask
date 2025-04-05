using MariamApp.Data;
using MariamApp.Data.Entities;
using MariamApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Tests;

[TestFixture]
public class ProductServiceTests
{
    private DataContext _dbContext;
    private ProductService _productService;
    private string _dbName;
    
    private readonly Category _category = new Category
    {
        Id = 1, 
        Name = "Electronics"
    };
    private readonly Supplier _supplier = new Supplier
    {
        Id = 1,
        Name = "Supplier A",
        ContactInfo = "+35654322"
    };

    [SetUp]
    public void Setup()
    {
        _dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;

        _dbContext = new DataContext(options);
        
        _dbContext.Categories.Add(_category);
        _dbContext.Suppliers.Add(_supplier);
        _dbContext.SaveChanges();

        _productService = new ProductService(_dbContext);
        
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    private Product CreateTestProduct() => new Product
    {
        Name = "Test Product",
        CategoryId = 1,
        SupplierId = 1,
        Price = 234,
        StockQuantity = 36
    };
    
    // add product cases
    [Test]
    public async Task AddProduct_ValidData_ReturnsProduct()
    {
        var product = CreateTestProduct();
        var result = await _productService.AddProductAsync(product);

        Assert.IsNotNull(result);
        Assert.AreEqual(product.Name, result.Name);
        Assert.AreEqual(product.Name, result.Name);
        Assert.AreEqual(product.CategoryId, result.CategoryId);
        Assert.AreEqual(product.SupplierId, result.SupplierId);
        Assert.AreEqual(product.Price, result.Price);
        Assert.AreEqual(product.StockQuantity, result.StockQuantity);
    }
    
    [Test]
    public async Task AddProduct_InvalidCategory_ThrowsException()
    {
        var product = CreateTestProduct();
        product.CategoryId = 999;
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _productService.AddProductAsync(product));
    }

    [Test]
    public async Task AddProduct_InvalidSupplier_ThrowsException()
    {
        var product = CreateTestProduct();
        product.SupplierId = 999;
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _productService.AddProductAsync(product));
    }
    
    // get product cases
    [Test]
    public async Task GetProductById_ValidId_ReturnsProduct()
    {
        var product = CreateTestProduct();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        
        var result = await _productService.GetProductByIdAsync(product.Id);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(product.Id, result.Id);
        Assert.AreEqual(product.Name, result.Name);
    }

    [Test]
    public async Task GetProductById_InvalidId_ReturnsNull()
    {
        var result = await _productService.GetProductByIdAsync(999);
        
        Assert.IsNull(result); 
    }
    
    // update product cases
    [Test]
    public async Task UpdateProduct_ValidData_ReturnsUpdatedProduct()
    {
        var product = CreateTestProduct();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        product.Name = "Updated Name";
        
        var result = await _productService.UpdateProductAsync(product.Id, product);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(product.Name, result.Name);
    }

    [Test]
    public async Task UpdateProduct_NonExistentProduct_ThrowsException()
    {
        var product = CreateTestProduct();
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _productService.UpdateProductAsync(999, product));
    }
    
    // delete product cases
    [Test]
    public async Task DeleteProduct_ExistingProduct_RemovesProduct()
    {
        var product = CreateTestProduct();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        
        await _productService.DeleteProductAsync(product.Id);
        var result = await _dbContext.Products.FindAsync(product.Id);
        
        Assert.IsNull(result);
    }

    [Test]
    public async Task DeleteProduct_NonExistentProduct_ThrowsException()
    {
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _productService.DeleteProductAsync(999));
    }
}