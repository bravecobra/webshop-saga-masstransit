using AutoMapper;
using Catalog.Controllers.dto;
using Xunit;

namespace webshop.tests
{
    public class AutomapperConfigurationShould
    {
        [Fact]
        public void AssertAutomapperCatalogProfiles()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<ProductsProfile>();
            });

            mappingConfig.AssertConfigurationIsValid();
        }
    }
}
