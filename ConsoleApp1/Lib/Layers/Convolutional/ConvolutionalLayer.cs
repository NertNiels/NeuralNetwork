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

            int width = (prev.featureMaps[0].width() - filterWidth + (2 * padding)) / stride + 1;
            int height = (prev.featureMaps[0].height() - filterHeight + (2 * padding)) / stride + 1;

            featureMaps = new FeatureMap[prev.filters.Length];
                        
            for(int f = 0; f < prev.filters.Length; f++)
            {
                int mapX = 0;
                int mapY = 0;

                featureMaps[f] = new FeatureMap() { map = new Matrix(width, height) };


                for (int i = -padding; i < width+padding; i += stride)
                {
                    for (int j = -padding; j < height+padding; j += stride)
                    {
                        float sum = 0;
                        for (int d = 0; d < prev.filters[f].dimensions; d++)
                        {
                            Matrix flipped = prev.filters[f].kernels[d].flip();

                            for (int k = 0; k < prev.filterWidth; k++)
                            {
                                for (int l = 0; l < prev.filterHeight; l++)
                                {
                                    if (!(i + k >= prev.featureMaps[0].width() || i + k < 0 || j + l >= prev.featureMaps[0].height() || j + l < 0)) sum +=
                                            prev.featureMaps[d].map.data[i + k, j + l] *
                                            flipped.data[k, l];
                                    if (float.IsInfinity(flipped.data[k, l]) || float.IsNaN(flipped.data[k, l]))
                                    {
                                        Console.WriteLine("moi");
                                    }
                                    if (float.IsInfinity(prev.featureMaps[d].map.data[i + k, j + l]) || float.IsNaN(prev.featureMaps[d].map.data[i + k, j + l]))
                                    {
                                        Console.WriteLine("moi");
                                    }
                                    if (float.IsInfinity(sum) || float.IsNaN(sum))
                                    {
                                        Console.WriteLine("moi");
                                    }
                                }
                            }
                        }

                        featureMaps[f].map.data[mapX, mapY] = sum + prev.filters[f].bias;
                        
                        if(float.IsInfinity(featureMaps[f].map.data[mapX, mapY]) || float.IsNaN(featureMaps[f].map.data[mapX, mapY]))
                        {
                            Console.WriteLine("jo");
                        }

                        mapY++;
                    }

                    mapX++;
                    mapY = 0;
                    
                }
            }

            

        }
        
        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            if (next.GetType() == typeof(ConvolutionalLayer)) {
                throw new Exception("A Convolutional layer must have an activation layer behind it in order to make it work.");
            }

            for (int f = 0; f < prev.featureMaps.Length; f++) prev.featureMaps[f].errors = new Matrix(prev.featureMaps[f].width(), prev.featureMaps[f].height());

            for(int f = 0; f < prev.filters.Length; f++)
            {
                //Calculating Gradient
                Matrix gradient = Matrix.hadamard(next.featureMaps[f].derivatives, featureMaps[f].errors);
                gradient.multiply(NeuralNetwork.lr);

                //Calculating Deltas And Errors
                #region Calculating Deltas And Errors

                Matrix deltas = new Matrix(prev.filters[f].width(), prev.filters[f].height());

                int mapX = 0;
                int mapY = 0;

                int width = (prev.featureMaps[0].width() - filterWidth + 2 * padding) / stride + 1;
                int height = (prev.featureMaps[0].height() - filterHeight + 2 * padding) / stride + 1;

                //featureMaps[f] = new FeatureMap() { map = new Matrix(width, height) };

                for (int i = -padding; i < width + padding; i += stride)
                {
                    for (int j = -padding; j < height + padding; j += stride)
                    {
                        for (int d = 0; d < prev.filters[f].dimensions; d++)
                        {
                            Matrix flipped = prev.filters[f].kernels[d].flip();

                            
                            for (int k = 0; k < prev.filterWidth; k++)
                            {
                                for (int l = 0; l < prev.filterHeight; l++)
                                {


                                    if (!(i + k >= prev.featureMaps[0].width() || i + k < 0 || j + l >= prev.featureMaps[0].height() || j + l < 0))
                                    {
                                        deltas.data[k, l] +=
                                            prev.featureMaps[d].map.data[i + k, j + l] *
                                            gradient.data[mapX, mapY];

                                        prev.featureMaps[d].errors.data[i + k, j + l] +=
                                            featureMaps[f].errors.data[mapX, mapY] *
                                            flipped.data[k, l];
                                    }
                                    
                                }
                            }
                        }




                        mapY++;
                    }

                    mapX++;
                    mapY = 0;

                }

                #endregion

                //Updating Weights and Biases
                // Idea: instead of summing up all deltas for each filter, do it for each dimension
                // instead. We can accomplish this by instead of Matrix deltas do Matrix[] deltas.
                prev.filters[f].updateFilters(gradient, deltas.flip());
                

            }

            #region ErrorTest
            /*
            for(int d = 0; d < prev.featureMaps.Length; d++)
            {
                Matrix errorMatrix = new Matrix(featureMaps[0].width(), featureMaps[0].height());

                int width = (featureMaps[0].width() - filterWidth + 2 * padding) / stride + 1;
                int height = (featureMaps[0].height() - filterHeight + 2 * padding) / stride + 1;

                int mapX = 0;
                int mapY = 0;

                for (int f = 0; f < filters.Length; f++)
                {
                    Matrix flipped = filters[f].kernels[d].flip();

                    for (int i = -padding; i < width + padding; i += stride)
                    {
                        for (int j = -padding; j < height + padding; j += stride)
                        {
                            for(int k = 0; k < flipped.rows; k++)
                            {
                                for(int l = 0; l < flipped.cols; l++)
                                {
                                    if (!(i + k >= prev.featureMaps[0].width() || i + k < 0 || j + l >= prev.featureMaps[0].height() || j + l < 0)) errorMatrix.data[i + k, j + l] +=
                                            next.featureMaps[f].errors.data[mapX, mapY] *
                                            filters[f].kernels[d].data[k, l];
                                }
                            }

                            mapY++;
                        }

                        mapX++;
                    }
                }

                featureMaps[d].errors = errorMatrix;
            }
            */
            #endregion

        }

        public override void initWeights(Random r, Layer prev, Layer next)
        {
            int num = 1;
            if (prev != null) num = prev.filters.Length;

            for(int f = 0; f < filters.Length; f++)
            {
                filters[f] = new Filter();
                filters[f].initFilter(r, num, filterWidth, filterHeight);
            }
        }
    }
}
