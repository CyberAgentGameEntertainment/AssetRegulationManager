// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Editor.Foundation.Observable
{
    [Serializable]
    public struct Empty : IEquatable<Empty>
    {
        public static Empty Default { get; } = new Empty();

        public bool Equals(Empty other)
        {
            return true;
        }

        public static bool operator ==(Empty first, Empty second)
        {
            return true;
        }

        public static bool operator !=(Empty first, Empty second)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Empty;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}