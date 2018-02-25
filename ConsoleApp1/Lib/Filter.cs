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

        public Matrix[] filters;
    }

    class FeatureMap
    {
        public int width;
        public int height;

        public Matrix errors;

        public Matrix map;
    }
}
