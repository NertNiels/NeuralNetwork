﻿using System;
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

            int width = (prev.featureMaps[0].width - filterWidth + (2 * padding)) / stride + 1;
            int height = (prev.featureMaps[0].height - filterHeight + (2 * padding)) / stride + 1;

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
                                    if (!(i + k >= prev.featureMaps[d].width || i + k < 0 || j + l >= prev.featureMaps[d].height || j + l < 0)) sum +=
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

                        featureMaps[f].map.data[mapX, mapY] = sum;
                        
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

            for (int f = 0; f < prev.featureMaps.Length; f++) prev.featureMaps[f].errors = new Matrix(prev.featureMaps[f].width, prev.featureMaps[f].height);

            Matrix[] gradients = new Matrix[filters.Length];

            //Calculating Gradients;
            for(int i = 0; i < gradients.Length; i++)
            {
                gradients[i] = Matrix.hadamard(next.featureMaps[i].derivatives, featureMaps[i].errors);
                gradients[i].multiply(NeuralNetwork.lr);
            }

            for(int f = 0; f < prev.filters.Length; f++)
            {
                //Calculating Deltas And Errors
                #region Calculating Deltas And Errors

                Matrix[] deltas = new Matrix[prev.filters[f].dimensions];

                int mapX = 0;
                int mapY = 0;

                int width = (prev.featureMaps[0].width - filterWidth + 2 * padding) / stride + 1;
                int height = (prev.featureMaps[0].height - filterHeight + 2 * padding) / stride + 1;

                //featureMaps[f] = new FeatureMap() { map = new Matrix(width, height) };

                for (int i = -padding; i < width + padding; i += stride)
                {
                    for (int j = -padding; j < height + padding; j += stride)
                    {
                        for (int d = 0; d < prev.filters[f].dimensions; d++)
                        {
                            Matrix flipped = prev.filters[f].kernels[d].flip();
                            deltas[d] = new Matrix(prev.filters[f].width(), prev.filters[f].height());
                            
                            for (int k = 0; k < prev.filterWidth; k++)
                            {
                                for (int l = 0; l < prev.filterHeight; l++)
                                {

                                    for (int g = 0; g < gradients.Length; g++)
                                    {

                                        if (!(i + k >= prev.featureMaps[d].width || i + k < 0 || j + l >= prev.featureMaps[d].height || j + l < 0))
                                        {
                                            deltas[d].data[k, l] +=
                                                prev.featureMaps[d].map.data[i + k, j + l] *
                                                gradients[g].data[mapX, mapY];

                                            if (deltas[d].data[k,l] > 1000 || deltas[d].data[k, l] < -1000)
                                            {
                                                Console.Write("\r");
                                            }

                                            prev.featureMaps[d].errors.data[i + k, j + l] +=
                                                featureMaps[f].errors.data[mapX, mapY] *
                                                flipped.data[k, l];

                                            if (prev.featureMaps[d].errors.data[i + k, j + l] > 1000 || prev.featureMaps[d].errors.data[i + k, j + l] < -1000)
                                            {
                                                Console.Write("\r");
                                            }
                                        }
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
                prev.filters[f].updateFilters(gradients[f], deltas);
                
                

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
