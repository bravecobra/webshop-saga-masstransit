using Catalog.Domain.Products;
using Catalog.Domain.Products.Events;
using Xunit;

namespace Webshop.Tests.Catalog.Domain.Products
{
    public class ProductShould
    {
        [Fact]
        public void RaiseDomainEvents_WhenCreated()
        {
            var product = Product.CreateProduct("test");
            Assert.Equal(4, product.DomainEvents.Count);
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(NewProductAdded));
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductDescriptionUpdated));
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductPriceUpdatedEvent));
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductStockUpdatedEvent));
        }

        [Fact]
        public void RaiseDomainEvents_WhenDescriptionIsUpdated()
        {
            var product = Product.CreateProduct("test");
            Assert.Equal(4, product.DomainEvents.Count);
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(NewProductAdded));
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductDescriptionUpdated));
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductPriceUpdatedEvent));
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductStockUpdatedEvent));

            product.UpdateDescription("testing");
            Assert.Equal("testing", product.Description);
            Assert.Equal(5, product.DomainEvents.Count);
            Assert.Contains(product.DomainEvents, domainevent => domainevent.GetType() == typeof(ProductDescriptionUpdated));
        }
    }
}
