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
            for(int i = 0; i < featureMaps.Length; i++)
            {
                featureMaps[i] = new FeatureMap() { width = prev.featureMaps[0].width, height = prev.featureMaps[0].height, map = Matrix.map(Activation.lrelu, prev.featureMaps[i].map) };
            }
        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {

        }
    }
}
