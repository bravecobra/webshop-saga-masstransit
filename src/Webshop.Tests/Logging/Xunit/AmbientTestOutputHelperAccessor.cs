using System.Threading;
using Xunit.Abstractions;

namespace Webshop.Tests.Logging.Xunit
{
    /// <summary>
    /// A class representing an implementation of <see cref="ITestOutputHelperAccessor"/> that
    /// stores the <see cref="ITestOutputHelper"/> as an asynchronous local value. This class cannot be inherited.
    /// </summary>
    internal sealed class AmbientTestOutputHelperAccessor : ITestOutputHelperAccessor
    {
        /// <summary>
        /// A backing field for the <see cref="ITestOutputHelper"/> for the current thread.
        /// </summary>
        private static readonly AsyncLocal<ITestOutputHelper> Current = new AsyncLocal<ITestOutputHelper>();

        /// <summary>
        /// Gets or sets the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public ITestOutputHelper OutputHelper
        {
            get => Current.Value;
            set => Current.Value = value;
        }

    }
}