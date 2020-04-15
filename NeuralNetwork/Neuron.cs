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

        public Neuron()
        {
            NBias = 0;
            NType = NeuronTypes.INPUT;
        }

        public Neuron(double Bias, NeuronTypes Type, int WeightsNums)
        {
            NBias = Bias;
            NType = Type;
            NWeights = new double[WeightsNums];
            SetRandomWeights(WeightsNums);
        }
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

        private void SetRandomWeights(int WeightsNums)
        {
            for (int i = 0; i < WeightsNums; i++)
            {
                NWeights[i] = NeuralNetwork.GetRandomWeight();
            }
        }

        public Neuron Copy()
        {
            return new Neuron(NBias, NType, NWeights);
        }
    }
}

