using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyLayerPerceptron
{
    internal class Axon
    {

        private int _neuronIndex;
        private double _weight;

        public Axon(int neuronIndex, double weight)
        {
            _neuronIndex = neuronIndex;
            _weight = weight;
        }

        public int NeuronIndex
        {
            get { return _neuronIndex; }
            set { _neuronIndex = value; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
    }
}
