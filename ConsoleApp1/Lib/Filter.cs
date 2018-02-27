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
        public int width;
        public int height;

        public int dimensions;

        public float bias;

        public Matrix[] filters;

        public void updateFilters(Matrix gradient, Matrix deltas)
        {
            for (int d = 0; d < dimensions; d++)
            {
                filters[d].add(deltas);
            }

            bias = deltas.sum() / (gradient.rows * gradient.cols);
        }
    }

    class FeatureMap
    {
        public int width;
        public int height;

        public Matrix errors;
        public Matrix derivatives;

        public Matrix map;
    }
}
