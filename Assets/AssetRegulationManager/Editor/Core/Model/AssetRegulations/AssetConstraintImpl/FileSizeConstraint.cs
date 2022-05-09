using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("File/Size", "File Size")]
    public sealed class FileSizeConstraint : AssetConstraint<Object>
    {
        public enum SizeUnit
        {
            Bytes,
            Kilobytes,
            Megabytes
        }

        [SerializeField] private long _size;
        [SerializeField] private SizeUnit _unit = SizeUnit.Bytes;

        private long _latestValue;

        public long Size
        {
            get => _size;
            set => _size = value;
        }

        public SizeUnit Unit
        {
            get => _unit;
            set => _unit = value;
        }

        public override string GetDescription()
        {
            var label = "Size";
            switch (_unit)
            {
                case SizeUnit.Bytes:
                    label += " (B)";
                    break;
                case SizeUnit.Kilobytes:
                    label += " (KB)";
                    break;
                case SizeUnit.Megabytes:
                    label += " (MB)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return $"{label}: {_size}";
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Object asset)
        {
            Assert.IsNotNull(asset);

            var assetPath = AssetDatabase.GetAssetPath(asset);
            var fileInfo = new FileInfo(assetPath);
            var bytes = fileInfo.Length;
            var size = ConvertSize(bytes, _unit);

            _latestValue = size;
            return size <= _size;
        }

        private static long ConvertSize(long bytes, SizeUnit unit)
        {
            switch (unit)
            {
                case SizeUnit.Bytes:
                    return bytes;
                case SizeUnit.Kilobytes:
                    return bytes / 1024;
                case SizeUnit.Megabytes:
                    return bytes / 1024 / 1024;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }
    }
}
