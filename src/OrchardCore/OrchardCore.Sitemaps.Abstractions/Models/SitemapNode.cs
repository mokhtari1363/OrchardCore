using System;
using System.Collections.Generic;
using System.Linq;
using OrchardCore.Navigation;

namespace OrchardCore.Sitemaps.Models
{
    public class SitemapNode 
    {
        public string Id { get; set; } 
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The child nodes.
        /// </summary>
        public List<SitemapNode> ChildNodes { get; set; }

        public SitemapNode GetSitemapNodeById(string id)
        {
            var tempStack = new Stack<SitemapNode>(new SitemapNode[] { this });

            while (tempStack.Any())
            {
                // evaluate first node
                SitemapNode item = tempStack.Pop();
                if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) return item;

                // not that one; continue with the rest.
                foreach (var i in item.ChildNodes) tempStack.Push((SitemapNode)i);
            }

            //not found
            return null;
        }

        // return boolean so that caller can check for success
        public bool RemoveSitemapNode(SitemapNode nodeToRemove)
        {
            var tempStack = new Stack<SitemapNode>(new SitemapNode[] { this });

            while (tempStack.Any())
            {
                // evaluate first
                var item = tempStack.Pop();
                if (item.ChildNodes.Contains(nodeToRemove))
                {
                    item.ChildNodes.Remove(nodeToRemove);
                    return true; //success
                }

                // not that one. continue
                foreach (var i in item.ChildNodes) tempStack.Push(i);
            }

            // failure
            return false;
        }


        public bool InsertSitemapNode(SitemapNode nodeToInsert, SitemapNode destinationNode, int position)
        {
            var tempStack = new Stack<SitemapNode>(new SitemapNode[] { this });
            while (tempStack.Any())
            {
                // evaluate first
                var node = tempStack.Pop();
                if (node.Equals(destinationNode))
                {
                    node.ChildNodes.Insert(position, nodeToInsert);
                    return true; // success
                }

                // not that one. continue
                foreach (var n in node.ChildNodes) tempStack.Push(n);
            }

            // failure
            return false;
        }
    }
}
