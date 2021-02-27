using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Products;
using MediatR;
using Moq;
using Webshop.Shared.Ddd;
using Webshop.Tests.Logging.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Webshop.Tests.Catalog.Persistence.Products
{
    public class CatalogContextShould: IClassFixture<SharedCatalogDatabaseFixture>
    {
        private readonly ITestOutputHelper _outputHelper;

        public CatalogContextShould(SharedCatalogDatabaseFixture fixture, ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            Fixture = fixture;
            Fixture.InitializeNewDatabase(@"Server=localhost,1433;Database=CatalogContextShould;User Id=sa;Password=Password_123;ConnectRetryCount=0");
        }

        public SharedCatalogDatabaseFixture Fixture { get; }

        [Fact]
        public async Task AddAProductToTheDatabase()
        {
            //Arrange
            await using var context = Fixture.CreateContext(_outputHelper.ToLoggerFactory());
            var product = Product.CreateProduct("test");
            var id = product.Id;
            var mediatr = new Mock<IMediator>();
            var dispatcher = new DomainEventDispatcher(mediatr.Object, _outputHelper.ToLogger<DomainEventDispatcher>());

            //Act
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            await dispatcher.DispatchDomainEvents(product);

            //Assert
            var productlist = context.Products.Where(product1 => product1.Id == id).ToList();
            Assert.Equal("test", productlist.First().Description);
        }

        [Fact]
        public async Task AddAProductToTheDatabase2()
        {
            //Arrange
            await using var context = Fixture.CreateContext(_outputHelper.ToLoggerFactory());
            var product = Product.CreateProduct("test2");
            var id = product.Id;
            //Act
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            //Assert
            var productlist = context.Products.Where(p => p.Id == id).ToList();
            Assert.Equal("test2", productlist.First().Description);
        }
    }
}
