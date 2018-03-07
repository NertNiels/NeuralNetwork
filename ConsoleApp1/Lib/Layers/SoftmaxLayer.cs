using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers
{
    class SoftmaxLayer : Layer
    {
        public SoftmaxLayer(int nodes)
        {
            this.nodes = nodes;
        }

        public override void doFeedForward(Layer prev)
        {
            Matrix newValues = Matrix.multiply(prev.weights, prev.values);
            newValues.add(bias);
            values = Activation.ssoftmax(newValues);
            for(int i = 0; i < values.rows; i++)
            {
                if (float.IsNaN(values.data[i, 0]))
                {
                    Console.WriteLine("Herewg");
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
            Matrix gradient = Matrix.map(Activation.dsoftmax, values);

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
            if (next != null)
            {
                weights = new Matrix(next.nodes, nodes);
                weights.randomize(r);
            }
            bias = new Matrix(nodes, 1);
            bias.randomize(r);
        }
    }
}
