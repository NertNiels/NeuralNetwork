using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Lib;

namespace NeuralNetwork.Lib.Layers
{
    class DropoutLayer : Layer
    {
        public static float dropoutChance = 0.25f;

        public DropoutLayer(int nodes)
        {
            this.nodes = nodes;
        }

        public override void doFeedForward(Layer prev)
        {
            values = Matrix.multiply(prev.weights, prev.values);
            values.add(bias);
            values.map(Activation.activation);
            if(NeuralNetwork.isTraining) doDropout();
        }

        void doDropout()
        {
            for(int i = 0; i < values.rows; i++)
            {
                for (int j = 0; j < values.cols; j++)
                {
                    float chance = (float)NeuralNetwork.random.NextDouble();
                    if(chance <= dropoutChance)
                    {
                        values.data[i, j] = 0;
                    }
                }
            }
        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            // Calculate Errors
            if (next == null) errors = Matrix.subtract(targets, outputs);
            else
            {
                Matrix weights_T = Matrix.transpose(weights);
                errors = Matrix.multiply(weights_T, next.errors);
            }

            // Calculating Gradient
            Matrix gradient = Matrix.map(Activation.derivative, values);
            gradient.hadamard(errors);
            gradient.multiply(NeuralNetwork.lr);

            // Calculating Deltas
            Matrix prevLayer_T = Matrix.transpose(prev.values);
            Matrix deltas = Matrix.multiply(gradient, prevLayer_T);

            // Updating Weights and Biases
            prev.weights.add(deltas);
            bias.add(gradient);
        }

        public override void initWeights(Random r, Layer prev, Layer next)
        {
            throw new NotImplementedException();
        }
    }
}
