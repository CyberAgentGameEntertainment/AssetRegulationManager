﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection
{
    /// <summary>
    ///     The dictionary that can be observed changes of the items.
    /// </summary>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <typeparam name="TValue">Type of items.</typeparam>
    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>,
        IReadOnlyObservableDictionary<TKey, TValue>, IDisposable
    {
        private readonly Dictionary<TKey, TValue> _internalDictionary;

        private readonly Subject<DictionaryAddEvent<TKey, TValue>> _subjectAdd
            = new Subject<DictionaryAddEvent<TKey, TValue>>();

        private readonly Subject<Empty> _subjectClear = new Subject<Empty>();

        private readonly Subject<DictionaryRemoveEvent<TKey, TValue>> _subjectRemove
            = new Subject<DictionaryRemoveEvent<TKey, TValue>>();

        private readonly Subject<DictionaryReplaceEvent<TKey, TValue>> _subjectReplace
            = new Subject<DictionaryReplaceEvent<TKey, TValue>>();

        private bool _didDispose;

        public ObservableDictionary() : this(new Dictionary<TKey, TValue>())
        {
        }

        public ObservableDictionary(Dictionary<TKey, TValue> source)
        {
            _internalDictionary = source;
        }

        public void Dispose()
        {
            Assert.IsFalse(_didDispose);

            DisposeSubject(_subjectAdd);
            DisposeSubject(_subjectRemove);
            DisposeSubject(_subjectClear);
            DisposeSubject(_subjectReplace);

            _didDispose = true;
        }

        public IObservable<DictionaryAddEvent<TKey, TValue>> ObservableAdd => _subjectAdd;

        public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObservableRemove => _subjectRemove;

        public IObservable<Empty> ObservableClear => _subjectClear;

        public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObservableReplace => _subjectReplace;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => _internalDictionary[key];
            set
            {
                Assert.IsFalse(_didDispose);

                var oldValue = _internalDictionary[key];
                if (Equals(oldValue, value))
                {
                    return;
                }

                _internalDictionary[key] = value;
                _subjectReplace.OnNext(new DictionaryReplaceEvent<TKey, TValue>(key, oldValue, value));
            }
        }

        public ICollection<TKey> Keys => _internalDictionary.Keys;

        public ICollection<TValue> Values => _internalDictionary.Values;

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        public void Add(TKey key, TValue value)
        {
            Assert.IsFalse(_didDispose);

            _internalDictionary.Add(key, value);
            _subjectAdd.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Assert.IsFalse(_didDispose);

            Add(item.Key, item.Value);
        }

        public bool Remove(TKey key)
        {
            Assert.IsFalse(_didDispose);

            if (!_internalDictionary.TryGetValue(key, out var value))
            {
                return false;
            }

            _internalDictionary.Remove(key);
            _subjectRemove.OnNext(new DictionaryRemoveEvent<TKey, TValue>(key, value));
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            Assert.IsFalse(_didDispose);

            return Remove(item.Key);
        }

        public void Clear()
        {
            Assert.IsFalse(_didDispose);

            _internalDictionary.Clear();
            _subjectClear.OnNext(Empty.Default);
        }

        public int Count => _internalDictionary.Count;

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            Assert.IsFalse(_didDispose);

            return _internalDictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            Assert.IsFalse(_didDispose);

            return _internalDictionary.ContainsKey(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        private static void DisposeSubject<TSubjectValue>(Subject<TSubjectValue> subject)
        {
            if (subject.DidDispose)
            {
                return;
            }

            if (subject.DidTerminate)
            {
                subject.Dispose();
                return;
            }

            subject.OnCompleted();
            subject.Dispose();
        }
    }
}