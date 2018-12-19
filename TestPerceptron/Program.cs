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
        static void Main(string[] args)
        {
            Perceptron network = new Perceptron(2, 5, 4, 1);
            
            TestNetwork(network);

            if(File.Exists("../../../network.xml"))
                File.Delete("../../../network.xml");

            FileStream file = new FileStream("../../../network.xml", FileMode.OpenOrCreate);
            XmlDocument document = network.ToXml();
            document.Save(file);

            network = Perceptron.LoadFromXml(document);
            TestNetwork(network);

            file.Close();
            file = new FileStream("../../../network_resave.xml", FileMode.OpenOrCreate);
            network.ToXml().Save(file);

            Console.ReadKey();
        }

        static void TestNetwork(Perceptron perceptron)
        {
            perceptron.PutData(new double[] { 0.1, 0.3, 0.2, 0.5 });
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
