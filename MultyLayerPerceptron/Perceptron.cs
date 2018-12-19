using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MultyLayerPerceptron
{
    public class Perceptron
    {
        private Random _rnd = new Random();
        private Neuron[] _input;
        private Neuron[] _output;

        private List<int> _neuronIndexes = new List<int>();
        private List<Neuron> _neurons = new List<Neuron>();

        class TangentHyperbolic : IActivationFunction
        {
            public double Activation(double x)
            {
                return Math.Tanh(x);
            }

            public double Derivative(double x)
            {
                double val = Activation(x);
                return 1 - val * val;
            }
        }

        internal static IActivationFunction GetFunction(string typeName)
        {
            if (typeName == "null")
                return null;

            Type functionType = Type.GetType(typeName);

            IActivationFunction instance = (IActivationFunction)Activator.CreateInstance(functionType);

            return instance;
        }

        private Perceptron()
        {
        }

        public Perceptron(int layerCount, int layerSize, int inputlayerSize, int outputLayerSize)
        {
            int index = 0;

            Neuron[] inputNeurons = new Neuron[inputlayerSize];
            for (int i = 0; i < inputNeurons.Length; i++)
            {
                inputNeurons[i] = new Neuron(null);
                inputNeurons[i].Index = index++;
            }

            Neuron[] layer = new Neuron[layerSize]; // Previous layer

            // Filling first layer
            for (int i = 0; i < layerSize; i++)
            {
                layer[i] = new Neuron(new TangentHyperbolic());
                layer[i].Index = index++;
                foreach (Neuron inputNeuron in inputNeurons)
                {
                    layer[i].Add(new Axon(inputNeuron, _rnd.NextDouble()));
                }
            }

            // Filling hidden layers
            for (int i = 1; i < layerCount; i++)
            {
                Neuron[] nLayer = new Neuron[layerSize];
                for (int j = 0; j < layerSize; j++)
                {
                    nLayer[j] = new Neuron(new TangentHyperbolic());
                    nLayer[j].Index = index++;
                    foreach (Neuron prevNeuron in layer)
                    {
                        nLayer[j].Add(new Axon(prevNeuron, _rnd.NextDouble()));
                    }
                }
                layer = nLayer;
            }

            // Filling output layer
            Neuron[] outputNeurons = new Neuron[outputLayerSize];

            for (int i = 0; i < outputLayerSize; i++)
            {
                outputNeurons[i] = new Neuron(new TangentHyperbolic());
                outputNeurons[i].Index = index++;
                foreach (Neuron prev in layer)
                {
                    outputNeurons[i].Add(new Axon(prev, _rnd.NextDouble()));
                }
            }

            _input = inputNeurons;
            _output = outputNeurons;

            for (int i = 0; i < index; i++)
            {
                _neuronIndexes.Add(i);
            }
        }

        public void PutData(double[] data)
        {
            for (var i = 0; i < _input.Length; i++)
            {
                Neuron input = _input[i];
                input.Value = data[i];
            }
        }

        public double[] GetResult()
        {
            List<double> result = new List<double>(_output.Length);

            foreach (var output in _output)
            {
                output.DoJob();
                result.Add(output.Value);
            }

            return result.ToArray();
        }

        public XmlDocument ToXml()
        {
            XmlDocument document = new XmlDocument();

            foreach (var outputNeuron in _output)
            {
                document.AppendChild(outputNeuron.ToXml(document));
            }

            return document;
        }

        internal void AddInputNeuron(Neuron n)
        {
            if (_input == null)
            {
                _input = new Neuron[1];
                _input[0] = n;
            }
            else
            {
                Array.Resize(ref _input, _input.Length  + 1);
                _input[_input.Length - 1] = n;
            }
        }

        private void AddOutputNeuron(Neuron n)
        {
            if (_output == null)
            {
                _output = new Neuron[1];
                _output[0] = n;
            }
            else
            {
                Array.Resize(ref _output, _output.Length);
                _output[_output.Length - 1] = n;
            }
        }

        internal Neuron GetByIndex(int index)
        {
            for (int i = 0; i < _neuronIndexes.Count; i++)
            {
                if (_neuronIndexes[i] == index)
                    return _neurons[i];
            }

            return null;
        }

        internal List<int> NeuronIndexes => _neuronIndexes;
        internal List<Neuron> Neurons => _neurons;
        public static Perceptron LoadFromXml(XmlDocument document)
        {
            Perceptron perceptron = new Perceptron();

            // Get output neurons
            //XmlNodeList outputNeuronsNodeList = document.GetElementsByTagName("Neuron");
            XmlNodeList outputNeuronsNodeList = document.ChildNodes;
            foreach (XmlElement neuronElement in outputNeuronsNodeList)
            {
                Neuron neuron = Neuron.FromXml(neuronElement, perceptron);
                perceptron.AddOutputNeuron(neuron);
            }

            return perceptron;
        }
    }
}
