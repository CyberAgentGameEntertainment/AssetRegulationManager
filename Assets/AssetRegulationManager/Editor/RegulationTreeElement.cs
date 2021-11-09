using System.Collections.Generic;

namespace AssetsRegulation
{
    public class RegulationTreeElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RegulationTreeElement Parent { get; private set; }
        private List<RegulationTreeElement> _children = new List<RegulationTreeElement>();
        public List<RegulationTreeElement> Children { get { return _children; } }

        /// <summary>
        /// 子を追加する
        /// </summary>
        public void AddChild(RegulationTreeElement child)
        {
            // 既に親がいたら削除
            if (child.Parent != null) {
                child.Parent.RemoveChild(child);
            }
            // 親子関係を設定
            Children.Add(child);
            child.Parent    = this;
        }

        /// <summary>
        /// 子を削除する
        /// </summary>
        public void RemoveChild(RegulationTreeElement child)
        {
            if (Children.Contains(child)) {
                Children.Remove(child);
                child.Parent        = null;
            }
        }
    }
}