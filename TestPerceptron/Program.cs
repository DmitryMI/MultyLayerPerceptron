using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MultyLayerPerceptron;

namespace TestPerceptron
{
    class Program
    {
        private static Random _rnd = new Random();
        static void Main(string[] args)
        {
            Perceptron network = new Perceptron(2, 5, 4, 1);

            double[] src = GetRandomArray();

            Console.Write("Source data: ");
            PrintArray(src);

            Console.WriteLine("Correct answer: " + GetProduct(src));
            network.PutData(src);
            Console.WriteLine("Net result: ");
            double[] netFirstAnswer = network.GetResult();
            PrintArray(netFirstAnswer);

            Console.WriteLine("\nTraining...\n");
            
            // Train to multiply values
            double result = GetProduct(src);
            int iteration = 0;
            while(Math.Abs(result - netFirstAnswer[0]) > 0.01f)
            { 
                double[] arr = GetRandomArray();
                network.Train(arr, ToArray(GetProduct(arr)), 1);
                network.PutData(arr);
                netFirstAnswer = network.GetResult();
                iteration++;
                Console.WriteLine("Error: " + Math.Abs(result - netFirstAnswer[0]) + "\titeration: " + iteration);
            }

            network.PutData(src);
            Console.WriteLine("Net result: ");
            PrintArray(network.GetResult());

            Console.ReadKey();
        }

        static double[] GetRandomArray()
        {
            double[] result = new double[4];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = _rnd.NextDouble() - 0.5;
            }

            return result;
        }

        static void PrintArray(double[] data)
        {
            string result = "";
            foreach (var d in data)
            {
                result += d + " ";
            }

            Console.WriteLine(result);
        }

        static double GetSumm(double[] arr)
        {
            double summ = 0;
            foreach (var d in arr)
            {
                summ += d;
            }

            return summ;
        }

        static double GetFirst(double[] arr)
        {
            return arr[0];
        }

        static double GetProduct(double[] arr)
        {
            double mul = 1;
            foreach (var d in arr)
            {
                mul *= d;
            }

            return mul;
        }

        static double[] ToArray(params double[] p)
        {
            return p;
        }

        static void TestNetwork(Perceptron perceptron)
        {
            perceptron.PutData(new double[] { -0.1, 0.3, -0.2, 0.5 });
            double[] result = perceptron.GetResult();

            string log = "";

            for (int i = 0; i < result.Length; i++)
            {
                log += result[i] + " ";
            }

            Console.WriteLine(log);
        }
    }
}
