using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyLayerPerceptron
{
    internal class Axon
    {

        private Neuron _neuron;
        private double _weight;

        public Axon(Neuron neuron, double weight)
        {
            _neuron = neuron;
            _weight = weight;
        }

        public Neuron Neuron
        {
            get { return _neuron; }
            set { _neuron = value; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
    }
}
