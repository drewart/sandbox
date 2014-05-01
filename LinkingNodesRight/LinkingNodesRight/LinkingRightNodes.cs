using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkingNodesRight
{

    class LinkRightNodes
    {

        /// <summary>
        /// given base Node class
        /// Modified: added ToString() & Name to help debug
        /// </summary>
        public class Node
        {
            public string Name;
            public Node[] Children;
            public Node Right;

            public override string ToString()
            {
                return Name;
            }
        }
        Node rootNode;




        /// <summary>
        /// Link right via Queue like method using prevous parent right branch node
        /// </summary>
        void LinkRightQueueLike()
        {
            Node current = rootNode;
            Node currentLevel,prevLevel,lastParent;
            
            currentLevel = new Node();
            currentLevel.Right = rootNode;
            prevLevel = new Node();
            prevLevel.Right = rootNode;
            lastParent = rootNode;

            currentLevel.Right = GoNextLevel(currentLevel);

            while(currentLevel.Right != null)
            {
                current = currentLevel.Right;
                
                while (current != null)
                {
                    Node rightSibling = SearchSiblingRight(current, ref lastParent);
                    
                    // update Right pointer
                    if (rightSibling != null)
                        current.Right = rightSibling;

                    current = rightSibling;
                }

                prevLevel.Right = currentLevel.Right;
                lastParent = prevLevel.Right;
                currentLevel.Right = GoNextLevel(currentLevel);
            }
        }

        /// <summary>
        /// go right until you can find first child of next level using right pointer
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <returns></returns>
        Node GoNextLevel(Node current)
        {
            if (current.Children != null)
                return current.Children[0];
            else if (current.Right != null)
                return GoNextLevel(current.Right);
            else
                return null;
        }

        /// <summary>
        /// search for Sibling on the right using parent right node
        /// </summary>
        /// <param name="current"></param>
        /// <param name="Parent">referance to last parent</param>
        /// <returns></returns>
        Node SearchSiblingRight(Node current,ref Node parent,int? index = null)
        {
            if (parent.Children != null)
            {
                // at end of children array
                if (parent.Right != null && parent.Children[parent.Children.Length - 1] == current)
                {
                    parent = parent.Right;  //move right
                    return SearchSiblingRight(current, ref parent,0);
                }
                    // at the end of children and right
                else if (parent.Right == null && parent.Children[parent.Children.Length - 1] == current)
                {
                    return null;
                }
                else
                {
                    if (index != null)
                        return parent.Children[index.Value];

                    for (int i = 0; i < (parent.Children.Length - 1); i++)
                    {
                        if (parent.Children[i] == current)
                            return parent.Children[++i];
                    }


                }
            }
            if (parent.Children == null && parent.Right != null)
            {
                parent = parent.Right; //move right
                return SearchSiblingRight(current, ref parent,0);
            }
            else
                return null;

        }


        /// <summary>
        ///   builds tree
        ///         a
        ///     /   |   \
        ///    b    c    d
        ///   / \    \    \
        ///  e   f    g    h 
        ///         a
        ///     /   |   \
        ///    b -> c -> d
        ///   / \    \    \
        ///  e ->f -> g -> h 
        /// </summary>
        void buildTreeA()
        {
            rootNode = new Node();
            rootNode.Name = "A";
            rootNode.Children = new Node[3];
            rootNode.Children[0] = new Node() { Name = "B" };
            rootNode.Children[1] = new Node() { Name = "C" };
            rootNode.Children[2] = new Node() { Name = "D" };
            rootNode.Children[0].Children = new Node[2];
            rootNode.Children[0].Children[0] = new Node { Name = "E"};
            rootNode.Children[0].Children[1] = new Node { Name = "F" };
            rootNode.Children[1].Children = new Node[1];
            rootNode.Children[1].Children[0] = new Node { Name = "G" };
            rootNode.Children[2].Children = new Node[1];
            rootNode.Children[2].Children[0] = new Node { Name = "H" };
        }

        #region print method for debug

        public void PrintTree()
        {
            PrintTreeHelper(rootNode);
        
        }

        void PrintTreeHelper(Node node)
        {
            if (node == null) return;

            Console.WriteLine(node.Name);

            if (node.Children == null) return;

            foreach(Node n in node.Children)
            {
                PrintTreeHelper(n);
            }
        }

        //
        public void PrintViaRight()
        {
            PrintViaRightHelper(rootNode);
        }

        /// <summary>
        /// print out tree using Right Node
        /// </summary>
        /// <param name="node"></param>
        private void PrintViaRightHelper(Node node)
        {
            if (node == null) return;

            Node level = new Node();
            level.Right = node;

            Node current = node;
           
            while(level.Right != null)
            {
                while(current != null)
                {
                    Console.Write("{0} ->", current.Name);
                    current = current.Right;
                }
                level.Right = GoNextLevel(level);
                current = level.Right;
                Console.WriteLine();
            }           
        }


        #endregion print method for debug


        /// <summary>
        /// main driver method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LinkRightNodes lrn = new LinkRightNodes();

            lrn.buildTreeA();
            lrn.PrintTree();
            lrn.LinkRightQueueLike();
            lrn.PrintViaRight();
        }
    }
}
