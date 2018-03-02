using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers.Convolutional
{
    class FullyConnectedLayer : Layer
    {
        public FullyConnectedLayer(int nodes)
        {
            this.nodes = nodes;
        }

        public override void doFeedForward(Layer prev)
        {
            Matrix output = new Matrix((prev.featureMaps[0].width() * prev.featureMaps[0].height()) * prev.featureMaps.Length, 1);

            int i = 0;
            for(int f = 0; f < prev.featureMaps.Length; f++)
            {
                for(int x = 0; x < prev.featureMaps[f].width(); x++)
                {
                    for (int y = 0; y < prev.featureMaps[f].height(); y++)
                    {
                        output.data[i, 0] = prev.featureMaps[f].map.data[x, y];

                        i++;
                    } 
                }
            }


            values = output;

            featureMaps = prev.featureMaps;
            
        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            if (next == null) throw new Exception("The last layer of a neural network cannot be a fully connected layer.");

            Matrix weights_T = Matrix.transpose(weights);
            errors = Matrix.multiply(weights_T, next.errors);

            int i = 0;
            for(int f = 0; f < prev.featureMaps.Length; f++)
            {
                featureMaps[f].errors = new Matrix(featureMaps[f].width(), featureMaps[f].height());
                for(int x = 0; x < featureMaps[f].width(); x++)
                {
                    for(int y = 0; y <featureMaps[f].height(); y++)
                    {
                        prev.featureMaps[f].errors.data[x, y] = errors.data[i, 0];
                        i++;
                    }
                }
            }
        }

        public override void initWeights(Random r, Layer prev, Layer next)
        {
            weights = new Matrix(next.nodes, nodes);
            weights.randomize(r);
        }
    }
}
