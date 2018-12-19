using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyLayerPerceptron
{
    interface IActivationFunction
    {
        double Activation(double x);
        double Derivative(double x);
    }
}
