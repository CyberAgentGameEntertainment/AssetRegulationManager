// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Editor.Foundation.Observable
{
    internal static class ConvertObservable
    {
        public static IObservable<TDst> Convert<TSrc, TDst>(this IObservable<TSrc> source, Func<TSrc, TDst> converter)
        {
            return new ConvertObservable<TSrc, TDst>(source, converter);
        }
    }

    internal class ConvertObservable<TSrc, TDst> : IObservable<TDst>
    {
        private readonly Func<TSrc, TDst> _converter;
        private readonly IObservable<TSrc> _source;

        public ConvertObservable(IObservable<TSrc> source, Func<TSrc, TDst> converter)
        {
            _source = source;
            _converter = converter;
        }

        public IDisposable Subscribe(IObserver<TDst> observer)
        {
            return _source.Subscribe(x =>
            {
                var dst = _converter.Invoke(x);
                observer.OnNext(dst);
            });
        }
    }
}