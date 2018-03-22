using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib.Layers.Convolutional_2
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
            double outWF = ((prev.featureMaps[0].width() - prev.filterWidth + (2f * padding)) / stride) + 1;
            double outHF = ((prev.featureMaps[0].height() - prev.filterHeight + (2f * padding)) / stride) + 1;

            if (outWF - Math.Floor(outWF) != 0 || outHF - Math.Floor(outHF) != 0) throw new ArgumentException("This stride isn't valid for this layer.");

            int outW = (int)outWF;
            int outH = (int)outHF;

            featureMaps = new FeatureMap[prev.filters.Length];

            for(int f = 0; f < prev.filters.Length; f++)
            {
                int mapX = 0;
                int mapY = 0;

                featureMaps[f] = new FeatureMap() { map = new Matrix(outW, outH) };
             
                for (int d = 0; d < prev.filters[f].dimensions; d++)
                {
                    for (int x = -padding; x < prev.featureMaps[d].width() + padding; x += stride)
                    {
                        for (int y = -padding; y < prev.featureMaps[d].height() + padding; y += stride)
                        {
                            float sum = 0;

                            Matrix flip = prev.filters[f].kernels[d].flip();

                            for (int fx = 0; fx < prev.filterWidth; fx++)
                            {
                                for (int fy = 0; fy < prev.filterHeight; fy++)
                                {
                                    if (!(x + fx < 0 || x + fx >= prev.featureMaps[f].width() || y + fy < 0 || y + fy >= prev.featureMaps[f].height())) sum +=
                                              flip.data[fx, fy] *
                                              prev.featureMaps[f].map.data[x + fx, y + fy];
                                }
                            }

                            featureMaps[f].map.data[mapX, mapY] += sum;
                            mapY++;
                        }
                        mapX++;
                        mapY = 0;
                    }
                }
            }

        }

        public override void doTrain(Layer prev, Layer next, Matrix targets, Matrix outputs)
        {
            throw new NotImplementedException();
        }

        public override void initWeights(Random r, Layer prev, Layer next)
        {
            int num = 1;
            if (prev != null) num = prev.filters.Length;

            for (int f = 0; f < filters.Length; f++)
            {
                filters[f] = new Filter();
                filters[f].initFilter(r, num, filterWidth, filterHeight);
            }
        }
    }
}
