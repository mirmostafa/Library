using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mohammad.Collections.Hierarchy
{
    [DebuggerDisplay(@"{Depth}: {Text} ({Children.Count} children)")]
    public class IndentTreeNode
    {
        public string Text { get; }
        public int Depth { get; }
        public IndentTreeNode Parent { get; }
        public List<IndentTreeNode> Children { get; } = new List<IndentTreeNode>();

        public IndentTreeNode(string text, int depth = 0, IndentTreeNode parent = null)
        {
            this.Text = text;
            this.Depth = depth;
            this.Parent = parent;
        }

        public void AddChild(IndentTreeNode child)
        {
            if (child != null)
                this.Children.Add(child);
        }

        public static List<IndentTreeNode> Parse(IEnumerable<string> lines, int rootDepth = 0, char indentChar = '\t')
        {
            var roots = new List<IndentTreeNode>();
            IndentTreeNode prev = null;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line.Trim(indentChar)))
                    throw new Exception(@"Empty lines are not allowed.");

                var lengthBefore = line.Length;
                var lengthAfter = line.TrimStart(indentChar).Length;
                var currentDepth = lengthBefore - lengthAfter;

                if (currentDepth == rootDepth)
                {
                    var root = new IndentTreeNode(line, rootDepth);
                    prev = root;

                    roots.Add(root);
                }
                else
                {
                    if (prev == null)
                        throw new Exception(@"Unexpected indention.");

                    if (currentDepth > prev.Depth)
                    {
                        var node = new IndentTreeNode(line.Trim(), currentDepth, prev);
                        prev.AddChild(node);

                        prev = node;
                    }
                    else if (currentDepth == prev.Depth)
                    {
                        var node = new IndentTreeNode(line.Trim(), currentDepth, prev.Parent);
                        prev.Parent.AddChild(node);

                        prev = node;
                    }
                    else
                    {
                        while (currentDepth < prev.Depth)
                            prev = prev.Parent;

                        var node = new IndentTreeNode(line.Trim(indentChar), currentDepth, prev.Parent);
                        prev.Parent.AddChild(node);
                    }
                }
            }
            return roots;
        }
    }
}