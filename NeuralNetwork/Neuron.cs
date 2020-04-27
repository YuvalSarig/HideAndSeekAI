using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNS.NeuralNetwork
{
    class Neuron
    {
        public double NBias { get; set; }
        public double[] NWeights { get; }
        public NeuronTypes NType { get; }

        // Constructor for Input Layer
        public Neuron()
        {
            NBias = 0;
            NType = NeuronTypes.INPUT;
        }

        // Constructor for neuron
        public Neuron(double Bias, NeuronTypes Type, int WeightsNum)
        {
            NBias = Bias;
            NType = Type;
            NWeights = new double[WeightsNum];
            SetRandomWeights(WeightsNum);
        }

        // Constructor for neuron
        public Neuron(double Bias, NeuronTypes Type, double[] Weights)
        {
            NBias = Bias;
            NType = Type;
            if (Weights != null)
            {
                NWeights = new double[Weights.Length];
                Array.Copy(Weights, NWeights, Weights.Length);
            }
        }

        // The function generate weights for the weights array
        private void SetRandomWeights(int WeightsNum)
        {
            for (int i = 0; i < WeightsNum; i++)
            {
                NWeights[i] = NeuralNetwork.GetRandomWeight();
            }
        }

        // The function return new similar neuron
        public Neuron Copy()
        {
            return new Neuron(NBias, NType, NWeights);
        }
    }
}

