using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Webshop.Tests.Logging.Xunit
{
    /// <summary>
    /// A class containing extension methods for the <see cref="ITestOutputHelper"/> interface. This class cannot be inherited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TestOutputHelperExtensions
    {
        /// <summary>
        /// Returns an <see cref="ILoggerFactory"/> that logs to the output helper.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to create the logger factory from.</param>
        /// <returns>
        /// An <see cref="ILoggerFactory"/> that writes messages to the test output helper.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="outputHelper"/> is <see langword="null"/>.
        /// </exception>
        public static ILoggerFactory ToLoggerFactory(this ITestOutputHelper outputHelper)
        {
            if (outputHelper == null)
            {
                throw new ArgumentNullException(nameof(outputHelper));
            }

            return new LoggerFactory().AddXUnit(outputHelper);
        }

        /// <summary>
        /// Returns an <see cref="ILogger{T}"/> that logs to the output helper.
        /// </summary>
        /// <typeparam name="T">The type of the logger to create.</typeparam>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to create the logger from.</param>
        /// <returns>
        /// An <see cref="ILogger{T}"/> that writes messages to the test output helper.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="outputHelper"/> is <see langword="null"/>.
        /// </exception>
        public static ILogger<T> ToLogger<T>(this ITestOutputHelper outputHelper)
            => outputHelper.ToLoggerFactory().CreateLogger<T>();
    }
}