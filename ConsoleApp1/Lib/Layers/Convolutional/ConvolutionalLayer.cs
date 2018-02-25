using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers.Convolutional
{
    class ConvolutionalLayer : Layer
    {
        int stride;
        int padding;


        public ConvolutionalLayer(int filterWidth, int filterHeight, int filterCount, int padding, int stride)
        {
            filters = new Filter[filterCount];

            this.padding = padding;
            this.stride = stride;
            

            this.filterWidth = filterWidth;
            this.filterHeight = filterHeight;
        }

        public override void doFeedForward(Layer prev)
        {
            //throw new NotImplementedException();

            int width = (prev.featureMaps[0].width - filterWidth + 2 * padding) / stride + 1;
            int height = (prev.featureMaps[0].height - filterHeight + 2 * padding) / stride + 1;

            featureMaps = new FeatureMap[filters.Length];
                        
            for(int f = 0; f < filters.Length; f++)
            {
                int mapX = 0;
                int mapY = 0;

                featureMaps[f] = new FeatureMap() { width = width, height = height };

                for (int i = -padding; i < width+padding; i += stride)
                {
                    for (int j = -padding; j < height+padding; j += stride)
                    {
                        float sum = 0;
                        for (int d = 0; d < filters[f].dimensions; f++)
                        {
                            for (int k = 0; k < filterWidth; k++)
                            {
                                for (int l = 0; l < filterHeight; l++)
                                {
                                    if (!(i + k >= prev.featureMaps[0].width || i + k < 0)) sum +=
                                            prev.featureMaps[d].map.data[k, l] *
                                            filters[f].filters[d].data[i + k, j + l];
                                }
                            }
                        }

                        featureMaps[f].map.data[mapX, mapY] = sum;
                        

                        mapY++;
                    }

                    mapX++;
                }
            }
            

        }
        
        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            throw new NotImplementedException();
        }

        public static Matrix doDropout(Matrix input, float dropoutChance)
        {
            for (int i = 0; i < input.rows; i++)
            {
                for (int j = 0; j < input.cols; j++)
                {
                    float chance = (float)NeuralNetwork.random.NextDouble();
                    if (chance <= dropoutChance)
                    {
                        input.data[i, j] = 0;
                    }
                }
            }

            return input;
        }

        public static Matrix doRelu(Matrix input)
        {
            Matrix m = Matrix.map(Activation.lrelu, input);

            return m;
        }
    }
}
