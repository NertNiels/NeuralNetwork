using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers.Convolutional
{
    class FullyConnectedLayer : Layer
    {
        public override void doFeedForward(Layer prev)
        {
            Matrix output = new Matrix((prev.featureMaps[0].width * prev.featureMaps[0].height) * prev.featureMaps.Length, 1);

            int i = 0;
            for(int f = 0; f < prev.featureMaps.Length; f++)
            {
                for(int x = 0; x < prev.featureMaps[f].width; x++)
                {
                    for (int y = 0; y < prev.featureMaps[f].height; y++)
                    {
                        output.data[i, 0] = prev.featureMaps[f].map.data[x, y];

                        i++;
                    } 
                }
            }

            output = Activation.softmax(output);

            values = output;

            featureMaps = prev.featureMaps;
            
        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            if (next == null) throw new Exception("The last layer of a neural network must not be a fully connected layer.");

            int i = 0;
            for(int f = 0; f < prev.featureMaps.Length; f++)
            {
                prev.featureMaps[f].errors = new Matrix(prev.featureMaps[f].width, prev.featureMaps[f].height);
                for(int x = 0; x < prev.featureMaps[f].width; x++)
                {
                    for(int y = 0; y < prev.featureMaps[f].height; y++)
                    {
                        prev.featureMaps[f].errors.data[x, y] = next.errors.data[i, 0];
                        i++;
                    }
                }
            }
        }
    }
}
