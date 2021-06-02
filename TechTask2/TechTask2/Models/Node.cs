using System.Collections.Generic;

namespace TechTask2.Models
{
    public class Node
    {
        public string Name;
        public string Text;
        public List<Node> Children = new List<Node>();
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public override string ToString() => Name;
    }
}
