using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib
{
    abstract class Layer
    {
        public Matrix values;
        public Matrix weights;
        public Matrix bias;
        public Matrix errors;

        public int nodes;

        public abstract void doFeedForward(Layer prev);
        public abstract void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs);
        
    }
}
