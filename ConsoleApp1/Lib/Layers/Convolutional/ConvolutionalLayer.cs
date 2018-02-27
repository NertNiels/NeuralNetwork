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

                        
            for(int f = 0; f < prev.filters.Length; f++)
            {
                int mapX = 0;
                int mapY = 0;

                featureMaps[f] = new FeatureMap() { width = width, height = height };

                for (int i = -padding; i < width+padding; i += stride)
                {
                    for (int j = -padding; j < height+padding; j += stride)
                    {
                        float sum = 0;
                        for (int d = 0; d < prev.filters[f].dimensions; f++)
                        {
                            for (int k = 0; k < prev.filterWidth; k++)
                            {
                                for (int l = 0; l < prev.filterHeight; l++)
                                {
                                    if (!(i + k >= prev.featureMaps[0].width || i + k < 0 || j + l >= prev.featureMaps[0].height || j + l < 0)) sum +=
                                            prev.featureMaps[d].map.data[k, l] *
                                            prev.filters[f].filters[d].data[i + k, j + l];
                                }
                            }
                        }

                        featureMaps[f].map.data[mapX, mapY] = sum + prev.filters[f].bias;
                        
                        

                        mapY++;
                    }

                    mapX++;
                    
                }
            }

            

        }
        
        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            if (next.GetType() == typeof(ConvolutionalLayer)) {
                Console.WriteLine("A Convolutional layer must have an activation layer behind it in order to make it work.");
                return;
            }

            for(int f = 0; f < prev.filters.Length; f++)
            {
                //Calculating Gradient
                Matrix gradient = Matrix.hadamard(next.featureMaps[f].derivatives, next.featureMaps[f].errors);
                gradient.multiply(NeuralNetwork.lr);

                //Calculating Deltas
                #region Calculating Deltas

                Matrix deltas = new Matrix(prev.filters[f].width, prev.filters[f].height);

                int mapX = 0;
                int mapY = 0;

                int width = (prev.featureMaps[0].width - filterWidth + 2 * padding) / stride + 1;
                int height = (prev.featureMaps[0].height - filterHeight + 2 * padding) / stride + 1;

                featureMaps[f] = new FeatureMap() { width = width, height = height };

                for (int i = -padding; i < width + padding; i += stride)
                {
                    for (int j = -padding; j < height + padding; j += stride)
                    {
                        for (int d = 0; d < prev.filters[f].dimensions; f++)
                        {
                            for (int k = 0; k < prev.filterWidth; k++)
                            {
                                for (int l = 0; l < prev.filterHeight; l++)
                                {
                                    float currentMapValue = prev.featureMaps[f].map.data[i, j];

                                    if (!(i + k >= prev.featureMaps[0].width || i + k < 0 || j + l >= prev.featureMaps[0].height || j + l < 0)) deltas.data[k, l] +=
                                            prev.featureMaps[f].map.data[i, j] *
                                            gradient.data[mapX, mapY];
                                }
                            }
                        }




                        mapY++;
                    }

                    mapX++;

                }

                #endregion

                //Updating Weights and Biases
                prev.filters[f].updateFilters(gradient, deltas);

            }

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
