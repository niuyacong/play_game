#define CSHARP2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    class Program
    {

        static void Main(string[] args)
        {
            //测试
            //int[, ,] a = new int[1, 3, 4];
            //Console.WriteLine("数组有{0}页", a.GetLength(0));
            //Console.WriteLine("数组有{0}行", a.GetLength(1));
            //Console.WriteLine("数组有{0}列", a.GetLength(2));//括号中要是3就会报错了  
            //Console.ReadKey();  
            //Console.ReadLine();

            //存储每个玩家的位置
            int[] playPositions = { 0, 0 };
            //将当前玩家设置为 player 1
            int currentPlayer = 1;
            //赢家
            int winner = 0;
            string input = null;

            //显示棋盘，并提示当前玩家下一步的动作
            for (int turn = 1; turn <= 10; ++turn)
            {
                DisplayBoard(playPositions);

                #region Check for end Game
                if (EndGame(winner, turn, input))
                {
                    break;
                }
                #endregion End Check for end Game
            
            input = NextMove(playPositions,currentPlayer);
            winner = DetermineWinner(playPositions);
                //交换玩家
            currentPlayer = (currentPlayer == 2) ? 1 : 2;
            }
        }

        private static int DetermineWinner(int[] playPositions)
        {
            int winner = 0;
            //确定是否有赢家
            int[] winningMasks = { 7, 56, 448, 73, 146, 292, 84, 273 };
            foreach (int mask in winningMasks)
            {
                if ((mask & playPositions[0]) == mask)
                {
                    winner = 1;
                    break;
                }
                else if((mask&playPositions[1])==mask)
                {
                    winner = 2;
                    break;
                }
            }
            return winner;
        }

        private static string NextMove(int[] playPositions, int currentPlayer)
        {
            string input;
            bool validMove;
            //不断地提示玩家移动直到输入有效的移动
            do
            {
                //请求当前玩家移动
                System.Console.Write("\nPlayer{0}-Enter move:", currentPlayer);
                input = System.Console.ReadLine();
                validMove = ValidateAndMove(playPositions, currentPlayer, input);
            } while (!validMove);
            return input;
        }

        private static bool ValidateAndMove(int[] playPositions, int currentPlayer, string input)
        {
            bool valid = false;
            //检查当前用户输入
            switch (input)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
#warning "same move allowed multiple times"//相同的动作多次被允许
                    //为了设置比特位而改变的位置
                    int shifter;
                    //要设置的位
                    int position;
                    //int.parse()将“输入”转换为整数“int.parse(输入)-1”
                    //因为数组是从零开始的
                    shifter = int.Parse(input) - 1;
                    //0000 0000 0000 0000 0000 0000 0000 0001:1左移position位
                    position = 1 << shifter;
                    if ((position | playPositions[currentPlayer == 2 ? 0:1]) == playPositions[currentPlayer == 2 ? 0:1])
                    {
                        System.Console.WriteLine("\nERROR:不能覆盖！");
                    }
                    else
                    {
                        //当前玩家的位置和position做或运算，然后赋值到当前玩家的位置
                        playPositions[currentPlayer - 1] |= position;
                        valid = true;
                    }
                    break;
                case "":
                case "quit":
                    valid = true;
                    break;
                default:
                //如果没有遇到其他的case语句，那么文本就是无效的
                    System.Console.WriteLine("\nERROR:Enter a value from 1-9.Push ENTER to quit");
                    break;
            }
            return valid;
        }

        static void DisplayBoard(int[] playPositions)
        {

            //这显示每个单元格之间的边界
            string[] borders = { "|", "|", "\n---+---\n", "|", "|", "\n---+---+---\n", "|", "|", "" };

            //显示当前边界
            int border = 0;//设置第一个边界：（border[0]="|"）
#if CSHARP2

            System.Console.Clear();
#endif
            for (int position = 1; position <= 256; position <<= 1, border++)
            {
                char token = CalculateToken(playPositions,position);

                //写出一个单元格的值和后面的边框             
                System.Console.Write(" {0}{1}",token,borders[border]);
            }
        }

        private static char CalculateToken(int[] playPositions, int position)
        {
            //将玩家初始化为 'x'和'0'
            char[] players = { 'x', '0' };
            char token;
            //如果玩家有位置设置，则给该玩家设置token
            if ((position & playPositions[0]) == position)
            {
                token = players[0];
            }
            else if ((position & playPositions[1]) == position)
            {
                token = players[1];
            }
            else
            {
                //位置清空
                token=' ';
            }
            return token;
        }
#line 113 "TicTacToe.cs"
        //生成的代码会在这里
#line default
        static bool EndGame(int winner, int turn, string input)
        {
            bool endGame = false;
            if (winner > 0)
            {
                System.Console.WriteLine("\nPlayer {0} has won!!!",winner);
                endGame = true;
            }
            else if (turn == 10)
            {
                //在第10次显示后，退出，而不是再次提示用户
                System.Console.WriteLine("\nThe game was a tie!");
                endGame = true;
            }
            else if (input == "" || input == "quit")
            {
                //检查用户是否通过点击进入没有任何字符或输入“退出”
                System.Console.WriteLine("The last player quit");
                endGame = true;
            }
            return endGame;
        }
    }
}
