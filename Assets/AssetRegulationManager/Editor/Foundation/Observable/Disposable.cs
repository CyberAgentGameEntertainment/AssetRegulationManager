// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Editor.Foundation.Observable
{
    /// <summary>
    ///     Implementation of <see cref="IDisposable" /> that can be used anonymously.
    /// </summary>
    internal class Disposable : IDisposable
    {
        private readonly Action _disposed;
        private bool _didDispose;

        public Disposable(Action disposed)
        {
            _disposed = disposed;
        }

        public void Dispose()
        {
            if (_didDispose) return;
            _disposed?.Invoke();
            _didDispose = true;
        }
    }
}