using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Lib.Layers;

namespace NeuralNetwork.Lib
{
    class NeuralNetwork
    {
        public static float lr = 0.1f;

        public static bool isTraining = false;

        public Layer[] layers;

        public static Random random;

        static NeuralNetwork()
        {
            random = new Random();
        }

        public NeuralNetwork(Random r, int[] layer_nodes)
        {
            layers = new Layer[layer_nodes.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new LeakyReluLayer(layer_nodes[i]);

                if (i != layers.Length - 1)
                {
                    layers[i].weights = new Matrix(layer_nodes[i + 1], layer_nodes[i]);
                    layers[i].weights.randomize(r);
                }

                layers[i].bias = new Matrix(layer_nodes[i], 1);
                layers[i].bias.randomize(r);
            }

        }

        public NeuralNetwork(Layer[] layers)
        {
            this.layers = layers;
        }

        public NeuralNetwork(Random r, Layer[] layers)
        {
            this.layers = layers;
            for(int i = 0; i < layers.Length-1; i++)
            {
                this.layers[i].weights.randomize(r);
                this.layers[i].bias.randomize(r);
            }
        }

        public Matrix feedforward(float[] input_arr)
        {
            layers[0].values = Matrix.fromArray(input_arr);

            for(int i = 1; i < layers.Length; i++)
            {
                Layer prev = layers[i - 1];
                Layer cur = layers[i];
                cur.values = Matrix.multiply(prev.weights, prev.values);
                cur.values.add(cur.bias);
                cur.values.map(Activation.activation);
            }

            return layers.Last().values;
        }

        public Matrix feedforwardLayer(float[] input_arr)
        {
            layers[0].values = Matrix.fromArray(input_arr);
            
            for(int i = 1; i < layers.Length; i++)
            {
                layers[i].doFeedForward(layers[i - 1]);
            }

            return layers.Last().values;
        }
        
        public void train(float[] input_arr, float[] target_arr)
        {
            Matrix outputs = feedforward(input_arr);

            Matrix targets = Matrix.fromArray(target_arr);

            for (int i = layers.Length - 1; i > 0; i--)
            {
                // Calculating Errors
                if (i == layers.Length - 1) layers[i].errors = Matrix.subtract(targets, outputs);
                else
                {
                    Matrix weights_T = Matrix.transpose(layers[i].weights);
                    layers[i].errors = Matrix.multiply(weights_T, layers[i + 1].errors);
                }

                // Calculating Gradient
                Matrix gradient = Matrix.map(Activation.derivative, layers[i].values);
                gradient.hadamard(layers[i].errors);
                gradient.multiply(lr);

                // Calculating Deltas
                Matrix prevLayer_T = Matrix.transpose(layers[i - 1].values);
                Matrix deltas = Matrix.multiply(gradient, prevLayer_T);

                // Updating Weights and Biases
                layers[i - 1].weights.add(deltas);
                layers[i].bias.add(gradient);
            }

        }

        public void trainLayers(float[] input_arr, float[] target_arr)
        {
            isTraining = true;

            Matrix outputs = feedforwardLayer(input_arr);

            Matrix targets = Matrix.fromArray(target_arr);

            for (int i = layers.Length - 1; i > 0; i--)
            {
                Layer nextLayer = null;
                if (i != layers.Length - 1) nextLayer = layers[i + 1];
                layers[i].doTrain(layers[i - 1], nextLayer, targets, outputs);
            }

            isTraining = false;
        }


        
    }

    
}
