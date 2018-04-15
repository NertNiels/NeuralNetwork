using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Lib;

namespace NeuralNetwork.Lib
{
    class Filter
    {
        public int width()
        {
            if (kernels == null || kernels.Length == 0) return 0;
            return kernels[0].rows;
        }
        public int height()
        {
            if (kernels == null || kernels.Length == 0) return 0;
            return kernels[0].cols;
        }

        public int dimensions;

        public float bias;

        public Matrix[] kernels;

        public void updateFilters(Matrix gradient, Matrix[] deltas)
        {
            for (int d = 0; d < dimensions; d++)
            {
                kernels[d].add(deltas[d].flip());
            }

            bias += gradient.sum() / (gradient.rows * gradient.cols);
        }

        public void initFilter(Random r, int numOfLayers, int width, int height)
        {
            kernels = new Matrix[numOfLayers];
            dimensions = numOfLayers;
            for(int d = 0; d < dimensions; d++)
            {
                kernels[d] = new Matrix(width, height);

                kernels[d].add(2);
                kernels[d].multiply(0.5f);
            }
            bias = (float)r.NextDouble();
        }
    }

    class FeatureMap
    {
        public int width { get { if (map == null) return 0; return map.rows; } }
        public int height { get { if (map == null) return 0; return map.cols; } }


        public Matrix errors;
        public Matrix derivatives;
        public Matrix gradients { get { return Matrix.hadamard(derivatives, errors); } }

        public Matrix map;
    }
}
