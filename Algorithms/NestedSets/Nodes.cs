namespace NestedSets
{
    public class Node
    {
        public int Left, Right;
        public int Level;
        public decimal Value;
        public override string ToString() => $"{Value}({Left} - {Right} - {Level})";
    }

    public class ClassicNode
    {
        public decimal Id;
        public decimal? ParentId;
    }
}