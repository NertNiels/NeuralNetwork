using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib
{
    class Activation
    {
        public static float lreluSlope = 0.01f;

        public static Func<float, float> activation = lrelu;
        public static Func<float, float> derivative = dlrelu;

        public static float sigmoid(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }

        public static float dsigmoid(float x)
        {
            return sigmoid(x) * (1 - sigmoid(x));
        }

        public static float dsigmoidY(float y)
        {
            return y * (1 - y);
        }

        public static float lrelu(float x)
        {
            if (x > 0) return x;
            return x * lreluSlope;
        }

        public static float dlrelu(float x)
        {
            if (x > 0) return 1;
            return lreluSlope;
        }

        public static float relu(float x)
        {
            if (x > 0) return x;
            else return 0;
        }

        public static float drelu(float x)
        {
            if (x > 0) return 1;
            return 0;
        }

    }
}
