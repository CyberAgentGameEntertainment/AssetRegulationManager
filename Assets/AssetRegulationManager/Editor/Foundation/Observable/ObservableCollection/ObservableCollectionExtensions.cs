// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection
{
    public static class ObservableCollectionExtensions
    {
        public static IReadOnlyObservableList<TValue> ToReadOnly<TValue>(this IObservableList<TValue> self)
        {
            return (IReadOnlyObservableList<TValue>)self;
        }
    }
}
