using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers.Convolutional
{
    class LeakyReluLayer : Layer
    {
        public override void doFeedForward(Layer prev)
        {
            featureMaps = new FeatureMap[prev.featureMaps.Length];
            filters = prev.filters;
            for(int i = 0; i < featureMaps.Length; i++)
            {
                featureMaps[i] = new FeatureMap() { map = Matrix.map(Activation.lrelu, prev.featureMaps[i].map) };
            }
        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            if (next == null) throw new Exception("A convolutional layer cannot be the final layer.");

            for(int f = 0; f < next.featureMaps.Length; f++)
            {
                prev.featureMaps[f].errors = featureMaps[f].errors;

                featureMaps[f].derivatives = Matrix.map(Activation.dlrelu, featureMaps[f].map);
            }
        }

        public override void initWeights(Random r, Layer prev, Layer next)
        {
            filters = prev.filters;
            filterWidth = prev.filterWidth;
            filterHeight = prev.filterHeight;
        }
    }
}
