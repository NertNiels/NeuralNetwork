using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers
{
    class LeakyReluLayer : Layer
    {
        public LeakyReluLayer(int nodes)
        {
            this.nodes = nodes;
        }

        public override void doFeedForward(Layer prev)
        {
            values = Matrix.multiply(prev.weights, prev.values);
            values.add(bias);
            values.map(Activation.lrelu);
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
            Matrix gradient = Matrix.map(Activation.dlrelu, values);
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
