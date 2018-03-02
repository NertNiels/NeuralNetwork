using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace NeuralNetwork.Lib
{
    class Loader
    {

        public static TrainingData[] loadTestMNSIT(String filePath)
        {
            FileStream ifsLabels = new FileStream(@"C:\Users\drumm\Desktop\MNIST\t10k-labels.idx1-ubyte", FileMode.Open);
            FileStream ifsImages = new FileStream(@"C:\Users\drumm\Desktop\MNIST\t10k-images.idx1-ubyte", FileMode.Open);

            BinaryReader brLabels = new BinaryReader(ifsLabels);
            BinaryReader brImages = new BinaryReader(ifsImages);

            int magic1 = brImages.ReadInt32();
            int numImages = brImages.ReadInt32();
            int numRows = brImages.ReadInt32();
            int numCols = brImages.ReadInt32();

            int magic2 = brLabels.ReadInt32();
            int numLabels = brLabels.ReadInt32();

            Matrix pixels = new Matrix(28, 28);

            TrainingData[] trainingData = new TrainingData[10000];

            for (int di = 0; di < trainingData.Length; di++)
            {
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        byte b = brImages.ReadByte();
                        pixels.data[i, j] = (float)b;
                    }
                }

                byte lbl = brLabels.ReadByte();

                Matrix label = new Matrix(9, 1);
                label.data[lbl, 0] = 1;

                trainingData[di] = new TrainingData(pixels, label);
            }

            return trainingData;
        }

    }
}
