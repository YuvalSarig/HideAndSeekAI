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

        /// <summary>
        /// Constructor for Input Layer
        /// </summary>
        public Neuron()
        {
            NBias = 0;
            NType = NeuronTypes.INPUT;
        }

        /// <summary>
        /// Constructor for neuron
        /// </summary>
        public Neuron(double Bias, NeuronTypes Type, int WeightsNum)
        {
            NBias = Bias;
            NType = Type;
            NWeights = new double[WeightsNum];
            SetRandomWeights(WeightsNum);
        }

        /// <summary>
        /// Constructor for neuron
        /// </summary>
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

        /// <summary>
        /// The function generate weights for the weights array
        /// </summary>
        /// <param name="WeightsNum">Number of weights that connect to neuron</param>
        private void SetRandomWeights(int WeightsNum)
        {
            for (int i = 0; i < WeightsNum; i++)
            {
                NWeights[i] = NeuralNetwork.GetRandomWeight();
            }
        }

        /// <summary>
        /// The function return new similar neuron
        /// </summary>
        /// <returns>Copy of neuron</returns>
        public Neuron Copy()
        {
            return new Neuron(NBias, NType, NWeights);
        }
    }
}

