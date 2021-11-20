using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MyGraph
    {
        public class TreeNode
        {
            public int Value;
            public int Count = 0;
            public TreeNode Left;
            public TreeNode Right;
        }
        public TreeNode Node; // экземпляр класса 

        // добавление значения в дерево 
        public void Add(int value)
        {
            AddRecursion(ref Node, value);
        }
        private void AddRecursion(ref TreeNode node, int val)
        {
            if (node == null)
            {
                node = new TreeNode();
                node.Value = val;
                node.Count = 1;
            }
            else
            {
                if (node.Value == val)
                {
                    node.Count++;
                }
                else
                {
                    if ((node.Value) > val)
                    {
                        AddRecursion(ref node.Left, val);
                    }
                    else
                    {
                        AddRecursion(ref node.Right, val);
                    }
                }
            }
        }

        // печать дерева (публичный метод)
        public void TreeTraversal(ref string str)
        {
            // прямой обход (в глубину)
            void TreeTraversalType_pr(TreeNode node, ref string s, bool detailed)
            {
                s = ""; // обнуляем строку
                if (node != null)
                {
                    if (detailed)
                    {
                        s += node.Value.ToString() + Environment.NewLine;
                    }
                    else
                    {
                        s += node.Value.ToString() + " ";
                    }
                    if (detailed)
                    {
                        s += " " + Environment.NewLine;

                    }
                    TreeTraversalType_pr(node.Left, ref s, detailed); // левое поддерево
                    if (detailed)
                    {
                        s += " " + Environment.NewLine;
                    }
                    TreeTraversalType_pr(node.Right, ref s, detailed); // правое поддерево
                }
                else if (detailed)
                {
                    s += "Дерево пустое" + Environment.NewLine;
                }
                Console.WriteLine(s);
            }

            // обратный обход (в глубину)
            void TreeTraversalType_obr(TreeNode node, ref string s, bool detailed)
            {
                s = ""; // обнуляем строку
                if (node != null)
                {
                    if (detailed)
                    {
                        s += " " + Environment.NewLine;
                    }
                    TreeTraversalType_obr(node.Left, ref s, detailed); // левое поддерево
                    if (detailed)
                    {
                        s += " " + node.Value.ToString() + Environment.NewLine;
                    }
                    else
                    {
                        s += node.Value.ToString() + " ";
                    }
                    if (detailed)
                    {
                        s += " " + Environment.NewLine;
                    }
                    TreeTraversalType_obr(node.Right, ref s, detailed); // правое поддерево
                }
                else if (detailed)
                {
                    s += "Дерево пустое" + Environment.NewLine;
                }
                Console.WriteLine(s);
            }

            // симметричный обход (в глубину)
            void TreeTraversalType_sim(TreeNode node, ref string s, bool detailed)
            {
                s = ""; // обнуляем строку
                if (node != null)
                {
                    if (detailed)
                    {
                        s += " " + Environment.NewLine;
                    }
                    TreeTraversalType_sim(node.Right, ref s, detailed); // правое поддерево
                    if (detailed)
                    {
                        s += " " + node.Value.ToString() + Environment.NewLine;
                    }
                    else
                    {
                        s += node.Value.ToString() + " ";
                    }
                    if (detailed)
                    {
                        s += " " + Environment.NewLine;
                    }
                    TreeTraversalType_sim(node.Left, ref s, detailed); // левое поддерево
                }
                else if (detailed)
                {
                    s += "Дерево пустое" + Environment.NewLine;
                }
                Console.WriteLine(s);
            }

            // обход в ширину 
            void TreeTraversalType_Across(TreeNode node, ref string s, bool detailed)
            {
                s = ""; // обнуляем строку
                var queue = new Queue<TreeNode>(); // создать новую очередь
                if (detailed)
                {
                    s += "    заносим в очередь значение " + node.Value.ToString() + Environment.NewLine; queue.Enqueue(node); // поместить в очередь первый уровень
                }
                while (queue.Count != 0) // пока очередь не пуста
                {
                    //если у текущей ветви есть листья, их тоже добавить в очередь
                    if (queue.Peek().Left != null)
                    {
                        if (detailed) s += "    заносим в очередь значение " + queue.Peek().Left.Value.ToString() + " из левого поддерева" + Environment.NewLine;
                        queue.Enqueue(queue.Peek().Left);
                    }
                    if (queue.Peek().Right != null)
                    {
                        if (detailed) s += "    заносим в очередь значение " + queue.Peek().Right.Value.ToString() + " из правого поддерева" + Environment.NewLine;
                        queue.Enqueue(queue.Peek().Right);
                    }
                    //извлечь из очереди информационное поле последнего элемента
                    if (detailed)
                    {
                        s += "    извлекаем значение из очереди: " + queue.Peek().Value.ToString() + Environment.NewLine;
                    }
                    else
                    {
                        s += queue.Peek().Value.ToString() + " "; // убрать последний элемент очереди
                    }
                    queue.Dequeue();
                }
                Console.WriteLine(s);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyGraph g = new MyGraph();

            g.Add(5);
            g.Add(4);
            g.Add(6);
            g.Add(3);

            /*foreach (Node n in g.TreeTraversal(TreeTraversalType_pr))
            {
                Console.WriteLine(n);
            }*/
            Console.WriteLine(str);

            Console.WriteLine();
        }
    }
}
