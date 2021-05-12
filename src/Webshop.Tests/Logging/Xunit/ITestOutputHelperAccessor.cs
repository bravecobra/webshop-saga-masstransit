using Xunit.Abstractions;

namespace Webshop.Tests.Logging.Xunit
{
    /// <summary>
    /// Defines a property for accessing an <see cref="ITestOutputHelper"/>.
    /// </summary>
    public interface ITestOutputHelperAccessor
    {
        /// <summary>
        /// Gets or sets the <see cref="ITestOutputHelper"/> to use.
        /// </summary>
        ITestOutputHelper OutputHelper { get; set; }
    }
}