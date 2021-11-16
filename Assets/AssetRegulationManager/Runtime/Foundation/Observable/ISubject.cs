// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Runtime.Foundation.Observable
{
    internal interface ISubject<T> : IObserver<T>, IObservable<T>
    {
    }
}