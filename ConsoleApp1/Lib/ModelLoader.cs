using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Lib.Layers;

namespace NeuralNetwork.Lib
{
    class ModelLoader
    {
        public static NeuralNetwork loadNetwork(String file_name)
        {
            String text;
            String[] sections;

            Layer[] layers;

            using(StreamReader streamReader = new StreamReader("models\\" + file_name+".network", Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
                sections = text.Split("#".ToCharArray());
            }

            String[] values = sections[0].Split(";".ToCharArray());
            layers = new Layer[values.Length];

            for (int i = 0; i < sections.Length; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < layers.Length; j++)
                    {
                        
                        String[] curText = values[j].Split(" ".ToCharArray());

                        if (curText[0] == "relu")
                        {
                            int layer_nodes = int.Parse(curText[1]);

                            layers[j] = new ReluLayer(layer_nodes);
                        } else if (curText[0] == "do")
                        {
                            int layer_nodes = int.Parse(curText[1]);

                            layers[j] = new DropoutLayer(layer_nodes);
                        } else if (curText[0] == "lrelu")
                        {
                            int layer_nodes = int.Parse(curText[1]);

                            layers[j] = new LeakyReluLayer(layer_nodes);
                        }
                        if (j == layers.Length - 1)
                        {
                            layers[j].weights = new Matrix(1, 1);
                        }
                    }
                } else if(layers[i-1].weights == null)
                {
                    int index = i - 1;
                    values = sections[i].Split(";".ToCharArray());

                    int rows = values.Length;
                    int cols = values[0].Split(" ".ToCharArray()).Length;

                    Matrix m = new Matrix(rows, cols);
                    
                    for(int j = 0; j < values.Length; j++)
                    {
                        String[] comp = values[j].Split(" ".ToCharArray());

                        for(int k = 0; k < comp.Length; k++)
                        {
                            m.data[j, k] = float.Parse(comp[k]);
                        }
                    }

                    Matrix.table(m);

                    layers[index].weights = m;
                }
            }

            NeuralNetwork nn = new NeuralNetwork(layers);
            
            return nn;
        }
    }
}
