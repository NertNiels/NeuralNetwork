using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers
{
    class ConvolutionalLayer : Layer
    {


        public ConvolutionalLayer(int width, int height)
        {
            this.values = new Matrix(width, height);
        }

        public override void doFeedForward(Layer prev)
        {
            throw new NotImplementedException();
        }
        
        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            throw new NotImplementedException();
        }

        public Matrix doDropout(float dropoutChance)
        {
            for (int i = 0; i < values.rows; i++)
            {
                for (int j = 0; j < values.cols; j++)
                {
                    float chance = (float)NeuralNetwork.random.NextDouble();
                    if (chance <= dropoutChance)
                    {
                        values.data[i, j] = 0;
                    }
                }
            }

            return values;
        }

        public Matrix doRelu()
        {
            values.map(Activation.lrelu);

            return m;
        }
    }
}
