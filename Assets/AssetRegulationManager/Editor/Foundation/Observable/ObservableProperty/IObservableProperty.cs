﻿// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty
{
    public interface IObservableProperty<T> : IObservable<T>, IDisposable
    {
        /// <summary>
        ///     Current value.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        ///     Set a value and force notify.
        /// </summary>
        /// <param name="value"></param>
        void SetValueAndNotify(T value);

        /// <summary>
        ///     Set a value and never notify.
        /// </summary>
        /// <param name="value"></param>
        void SetValueAndNotNotify(T value);
    }

    public interface IReadOnlyObservableProperty<T> : IObservable<T>, IDisposable
    {
        /// <summary>
        ///     Current value.
        /// </summary>
        T Value { get; }
    }
}