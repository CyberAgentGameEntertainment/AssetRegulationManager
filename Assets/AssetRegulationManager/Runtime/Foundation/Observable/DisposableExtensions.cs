﻿// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Runtime.Foundation.Observable
{
    public static class DisposableExtensions
    {
        internal static void DisposeWith(this IDisposable self, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(self);
        }
    }
}