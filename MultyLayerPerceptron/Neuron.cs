using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MultyLayerPerceptron
{
    internal class Neuron : IList<Axon>
    {
        private IList<Axon> _children = new List<Axon>();

        private int _index;

        private double _value;

        private IActivationFunction _activationFunction;

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public Neuron(IActivationFunction activationFunction)
        {
            _activationFunction = activationFunction;
        }

        private Neuron()
        {
            
        }

        public double Value
        {
            get => _value;
            set => _value = value;
        }

        public void DoJob()
        {
            if (_children.Count == 0)
            {
                // Do nothing, we have no children
            }
            else
            {
                double summ = 0;
                foreach (Axon axon in _children)
                {
                    axon.Neuron.DoJob(); // Call value calculator
                    summ += axon.Neuron.Value * axon.Weight; // Adding value to summ
                }

                _value = _activationFunction.Activation(summ);
                Console.WriteLine("Value calculated: " + _value);
            }
        }

        public XmlNode ToXml(XmlDocument document)
        {
            XmlElement element = document.CreateElement("Neuron");
            element.SetAttribute("Id", _index.ToString());

            string functionName = "null";

            if (_activationFunction != null)
                functionName = _activationFunction.GetType().FullName;

            element.SetAttribute("FunctionName", functionName);

            foreach (var axon in _children)
            {
                XmlElement childContainer = document.CreateElement("Axon");
                childContainer.SetAttribute("Weight", axon.Weight.ToString());
                childContainer.AppendChild(axon.Neuron.ToXml(document));
                element.AppendChild(childContainer);
            }


            return element;
        }

        public static Neuron FromXml(XmlElement element, Perceptron host)
        {
            // Check if neuron exists
            Neuron neuron;
            int index = Int32.Parse(element.GetAttribute("Id"));

            neuron = host.GetByIndex(index);

            if (neuron != null)
                return neuron;
            
            neuron = new Neuron();
            host.Neurons.Add(neuron);
            host.NeuronIndexes.Add(index);

            neuron._index = index;
            string functionName = element.GetAttribute("FunctionName");
            neuron._activationFunction = Perceptron.GetFunction(functionName);

            XmlNodeList axons = element.ChildNodes;

            if (axons.Count == 0)
            {
                // This neuron is input neuron!
                Console.WriteLine("Input neuron detected! Index: " + index);
                host.AddInputNeuron(neuron);
            }
            else
            {
                foreach (XmlElement axonNode in axons)
                {
                    double weight = Convert.ToDouble(axonNode.GetAttribute("Weight"));

                    Neuron child = FromXml((XmlElement) axonNode.ChildNodes[0], host);

                    neuron._children.Add(new Axon(child, weight));
                }
            }

            return neuron;
        }

        #region ListImplementation
        public IEnumerator<Axon> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_children).GetEnumerator();
        }

        public void Add(Axon item)
        {
            _children.Add(item);
        }

        public void Clear()
        {
            _children.Clear();
        }

        public bool Contains(Axon item)
        {
            return _children.Contains(item);
        }

        public void CopyTo(Axon[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public bool Remove(Axon item)
        {
            return _children.Remove(item);
        }

        public int Count
        {
            get { return _children.Count; }
        }

        public bool IsReadOnly
        {
            get { return _children.IsReadOnly; }
        }

        public int IndexOf(Axon item)
        {
            return _children.IndexOf(item);
        }

        public void Insert(int index, Axon item)
        {
            _children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _children.RemoveAt(index);
        }

        public Axon this[int index]
        {
            get { return _children[index]; }
            set { _children[index] = value; }
        }


        #endregion

    }
}
