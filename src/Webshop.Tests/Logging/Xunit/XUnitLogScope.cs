﻿using System;
using System.Threading;

namespace Webshop.Tests.Logging.Xunit
{
    /// <summary>
    /// A class representing a scope for logging. This class cannot be inherited.
    /// </summary>
    internal sealed class XUnitLogScope
    {
        /// <summary>
        /// The scope for the current thread.
        /// </summary>
        private static readonly AsyncLocal<XUnitLogScope> _value = new AsyncLocal<XUnitLogScope>();

        /// <summary>
        /// The state object for the scope.
        /// </summary>
        private readonly object _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogScope"/> class.
        /// </summary>
        /// <param name="state">The state object for the scope.</param>
        internal XUnitLogScope(object state)
        {
            _state = state;
        }

        /// <summary>
        /// Gets or sets the current scope.
        /// </summary>
        internal static XUnitLogScope Current
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }

        /// <summary>
        /// Gets the parent scope.
        /// </summary>
        internal XUnitLogScope Parent { get; private set; }

        /// <inheritdoc />
        public override string ToString()
            => _state.ToString();

        /// <summary>
        /// Pushes a new value into the scope.
        /// </summary>
        /// <param name="state">The state object for the scope.</param>
        /// <returns>
        /// An <see cref="IDisposable"/> that pops the scope.
        /// </returns>
        internal static IDisposable Push(object state)
        {
            var temp = Current;

            Current = new XUnitLogScope(state)
            {
                Parent = temp,
            };

            return new DisposableScope();
        }

        /// <summary>
        /// A class the disposes of the current scope. This class cannot be inherited.
        /// </summary>
        private sealed class DisposableScope : IDisposable
        {
            /// <inheritdoc />
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}