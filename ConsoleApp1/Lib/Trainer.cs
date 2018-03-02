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

        public NeuralNetwork Train(NeuralNetwork nn)
        {
            TrainingData[] randomized = data.OrderBy<TSource>

            for(int i = 0; i < data.Length; i++)
            {
                
            }
        }

        public TKey randomize(TrainingData td)
        {

        }

    }

    class TrainingData
    {
        Matrix inputs;
        Matrix labels;

        public TrainingData(Matrix inputs, Matrix labels)
        {
            this.inputs = inputs;
            this.labels = labels;
        }
    }
}
