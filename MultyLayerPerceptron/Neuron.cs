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

        public void DoJob(Perceptron perceptron)
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
                    int axonNeuronIndex = axon.NeuronIndex;
                    Neuron neuron = perceptron.GetByIndex(axonNeuronIndex);
                    neuron.DoJob(perceptron); // Call value calculator
                    summ += neuron.Value * axon.Weight; // Adding value to summ
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
                childContainer.SetAttribute("Id", axon.NeuronIndex.ToString());
                childContainer.SetAttribute("Weight", axon.Weight.ToString());
                
                element.AppendChild(childContainer);
            }


            return element;
        }

        internal static Neuron FromXml(XmlElement element, Perceptron host)
        {
            // Check if neuron exists
            Neuron neuron = new Neuron();
            neuron._index = Int32.Parse(element.GetAttribute("Id"));
            string functionName = element.GetAttribute("FunctionName");
            neuron._activationFunction = Perceptron.GetFunction(functionName);

            XmlNodeList axonsNodes = element.ChildNodes;

            foreach (XmlElement axonNode in axonsNodes)
            {
                int childIndex = Convert.ToInt32(axonNode.GetAttribute("Id"));
                double weight = Convert.ToDouble(axonNode.GetAttribute("Weight"));

                neuron.Add(new Axon(childIndex, weight));
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
