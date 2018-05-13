using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers.Convolutional_2
{
    class FlattenLayer : Layer
    {
        public FlattenLayer(int nodes)
        {
            this.nodes = nodes;
        }

        public override void doFeedForward(Layer prev)
        {
            Matrix output = new Matrix((prev.featureMaps[0].width * prev.featureMaps[0].height) * prev.featureMaps.Length, 1);

            int i = 0;
            for (int f = 0; f < prev.featureMaps.Length; f++)
            {
                for (int x = 0; x < prev.featureMaps[f].width; x++)
                {
                    for (int y = 0; y < prev.featureMaps[f].height; y++)
                    {
                        output.data[i, 0] = prev.featureMaps[f].map.data[x, y];

                        if (float.IsInfinity(output.data[i, 0]))
                        {
                            Console.WriteLine("ja");
                        }

                        i++;
                    }
                }
            }


            values = output;

            featureMaps = prev.featureMaps;

        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            

            int i = 0;
            for (int f = 0; f < prev.featureMaps.Length; f++)
            {
                prev.featureMaps[f].errors = new Matrix(prev.featureMaps[f].width, prev.featureMaps[f].height);
                for (int x = 0; x < prev.featureMaps[f].width; x++)
                {
                    for (int y = 0; y < prev.featureMaps[f].height; y++)
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
