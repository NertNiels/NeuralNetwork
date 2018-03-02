using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib
{
    class Trainer
    {
        TrainingData[] data;


        public Trainer(TrainingData[] data)
        {
            this.data = data;
        }

        public Trainer()
        {
            data = null;
        }

        public void Train(NeuralNetwork nn)
        {
            List<TrainingData> list = new List<TrainingData>(data);

            Random r = NeuralNetwork.random;

            int index = 0;

            while(list.Count > 0)
            {
                int i = r.Next(0, list.Count);
                float loss = nn.train(list[i].inputs, list[i].labels);

                list.RemoveAt(i);

                float percentage = (int)Math.Round((double)((((float)(data.Length - list.Count) / (float)data.Length) * (float)100)));
                Console.Write("\rDone: {0}%, Loss: {1}, Iteration: {2}    ", percentage, loss, index);

                i++;
            }
        }

    }

    class TrainingData
    {
        public Matrix inputs;
        public Matrix labels;

        public TrainingData(Matrix inputs, Matrix labels)
        {
            this.inputs = inputs;
            this.labels = labels;
        }
    }
}
