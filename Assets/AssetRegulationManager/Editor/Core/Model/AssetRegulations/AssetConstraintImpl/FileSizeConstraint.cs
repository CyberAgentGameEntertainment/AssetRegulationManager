using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("File/File Size", "File Size")]
    public sealed class FileSizeConstraint : AssetConstraint<Object>
    {
        public enum SizeUnit
        {
            B,
            KB,
            MB
        }

        [SerializeField] private long _maxSize;
        [SerializeField] private SizeUnit _unit = SizeUnit.B;

        private long _latestValue;

        public long MaxSize
        {
            get => _maxSize;
            set => _maxSize = value;
        }

        public SizeUnit Unit
        {
            get => _unit;
            set => _unit = value;
        }

        public override string GetDescription()
        {
            var label = "Max File Size";
            switch (_unit)
            {
                case SizeUnit.B:
                    label += " (B)";
                    break;
                case SizeUnit.KB:
                    label += " (KB)";
                    break;
                case SizeUnit.MB:
                    label += " (MB)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return $"{label}: {_maxSize}";
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
            return size <= _maxSize;
        }

        private static long ConvertSize(long bytes, SizeUnit unit)
        {
            switch (unit)
            {
                case SizeUnit.B:
                    return bytes;
                case SizeUnit.KB:
                    return bytes / 1024;
                case SizeUnit.MB:
                    return bytes / 1024 / 1024;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }
    }
}
