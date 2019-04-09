using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    class AnimalsGame
    {
        private class TreeNode
        {
            public TreeNode(string q)
            {
                Question = q;
            }

            public TreeNode(string q, TreeNode y, TreeNode n)
            {
                Question = q;
                Yes = y;
                No = n;
            }
            public string Question { get; }
            public TreeNode Yes { get; set; }
            public TreeNode No { get; set; }
        }

        private TreeNode root;

        public AnimalsGame()
        {
            root = 
            new TreeNode("Это млекопитающее?", 
                new TreeNode("Оно лает?", 
                    new TreeNode("Собака"),
                    new TreeNode("Кошка")),
                new TreeNode("Оно покрыто чешуей?", 
                    new TreeNode("Оно дышит в воде?",
                        new TreeNode("Рыба"),
                        new TreeNode("Змея")
                    ),
                    new TreeNode("Птица")
                )
            );
        }
        
        public void Start()
        {
            string ans;
            TreeNode cur, next = root;
            do
            {
                do
                {
                    Console.Write(next.Question + "(y/n): ");
                    ans = Console.ReadLine()?.ToUpper();
                } while (ans != "Y" && ans != "N");
                cur = next;
                next = ans == "Y" ? cur.Yes : cur.No;
            } while (next.Yes != null);

            string temp;
            do
            {
                Console.Write("Это " + next.Question + "?(y/n): ");
                temp = Console.ReadLine()?.ToUpper();
            } while (temp != "Y" && temp != "N");

            if (temp == "Y")
            {
                Console.WriteLine("That was easy :)");
                return;
            }

            Console.WriteLine("Какое животное вы загадали?");
            string animal = Console.ReadLine();
            Console.WriteLine("Какой вопрос поможет отличить '" + next.Question +"' от '" + animal + "'?");
            string question = Console.ReadLine();
            do
            {
                Console.Write("Для '" + animal + "' ответ утвердительный?(y/n): ");
                temp = Console.ReadLine()?.ToUpper();
            } while (temp != "Y" && temp != "N");
            TreeNode newTreeNode = new TreeNode(question);
            if (temp == "Y")
            {
                newTreeNode.Yes = new TreeNode(animal);
                newTreeNode.No = next;
            }
            else
            {
                newTreeNode.Yes = next;
                newTreeNode.No = new TreeNode(animal);
            }
            if (ans == "Y")
                cur.Yes = newTreeNode;
            else
                cur.No = newTreeNode;
            Console.WriteLine("Спасибо за новую информацию!");
        }
    }
}
