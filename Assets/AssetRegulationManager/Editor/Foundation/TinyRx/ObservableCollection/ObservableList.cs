// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection
{
    [Serializable]
    public class ObservableList<T> : ObservableListBase<T>
    {
        [SerializeField] private List<T> _internalList = new List<T>();

        protected override List<T> InternalList => _internalList;
    }
}
