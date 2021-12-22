﻿// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection
{
    public readonly struct DictionaryAddEvent<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue Value;

        public DictionaryAddEvent(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public readonly struct DictionaryRemoveEvent<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue Value;

        public DictionaryRemoveEvent(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public readonly struct DictionaryReplaceEvent<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue OldValue;
        public readonly TValue NewValue;

        public DictionaryReplaceEvent(TKey key, TValue oldValue, TValue newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
