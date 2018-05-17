using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NeuralNetwork.Lib;
using NeuralNetwork.Lib.Layers;
using NeuralNetwork.Lib.Layers.Convolutional;

namespace NeuralNetwork
{
    class Program
    {

        public static Lib.NeuralNetwork nn;
        public static bool printStackTrace;

        static void Main(string[] args)
        {
            Clear();

            bool running = true;
            while(running)
            {
                String command = Console.ReadLine();
                command = command.ToLower();
                String[] words = command.Split(" ".ToCharArray());

                if (words[0].Equals("stop") || words[0].Equals("exit")) running = false;
                else if (words[0].Equals("neuralnetwork") || words[0].Equals("nn")) createNeuralNetwork(words);
                else if (words[0].Equals("feedforward") || words[0].Equals("ff"))
                {
                    Matrix m = feedForwardAlgirithm(words);
                    if (m != null)
                    {
                        Matrix.table(m);
                    }
                }
                else if (words[0].Equals("stacktrace") || words[0].Equals("stack")) StackTrace(words);
                else if (words[0].Equals("clear")) Clear();
                else if (words[0].Equals("train")) Train(words);
                else if (words[0].Equals("test") || words[0].Equals("testprogram")) TestProgram();
                else if (words[0].Equals("help") || words[0].Equals("?")) printHelp();
                else if (words[0].Equals("trainingdata")) printTrainingData();
                else if (words[0].Equals("reset")) reset();
                else Console.WriteLine("That's not a valid command. Please enter a valid one.");
            }
        }

        static void reset()
        {
            Lib.NeuralNetwork.random = new Random();
            for(int i = 0; i < nn.layers.Length-1; i++)
            {
                nn.layers[i].weights.randomize(Lib.NeuralNetwork.random);
                nn.layers[i].bias.randomize(Lib.NeuralNetwork.random);

            }
        }

        static void printHelp()
        {
            String message = "Here are all the commands you can use in this program: \n" +
                "stacktrace/stack: Enables or disables the StackTrace function.\n" +
                "stacktrace/stack: boolean: Changes the StackTrace function to the boolean.\n" +
                "clear: Clears the console.\n" +
                "train: Trains the network once.\n" +
                "train n: Trains the network n times.\n" +
                "test: Runs the test program.\n" +
                "neuralnetwork/nn nodes: Creates a Neural Network with for each layer a specific number of nodes.\n" +
                "neuralnetwork/nn layerType nodes: Creates a Neural Network with different layertypes.\n" +
                "feedforward/ff parameters: Feedforwards the Neural Network with those parameters.\n" +
                "stop/exit: Closes the program." +
                "help: Runs the help sceen.";
            Console.WriteLine(message);
        }

        static void createNeuralNetwork(String[] words)
        {
            
            

            Random r = Lib.NeuralNetwork.random;

            try
            {
                if (Char.IsDigit(words[1].ToCharArray()[0]))
                {
                    int[] nodes = new int[words.Length - 1];
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodes[i] = int.Parse(words[i + 1]);
                    }
                    nn = new Lib.NeuralNetwork(r, nodes);
                } else
                {

                    Layer[] layers = new Layer[(words.Length - 1) / 2];
                    
                    for(int i = 1; i < words.Length; i += 2)
                    {
                        if(words[i].Equals("relu"))
                        {
                            int nodes = int.Parse(words[i + 1]);
                            layers[(i - 1) / 2] = new ReluLayer(nodes);
                        } else if (words[i].Equals("do"))
                        {
                            int nodes = int.Parse(words[i + 1]);
                            layers[(i - 1) / 2] = new DropoutLayer(nodes);
                        }
                        else if (words[i].Equals("lrelu"))
                        {
                            int nodes = int.Parse(words[i + 1]);
                            layers[(i - 1) / 2] = new Lib.Layers.LeakyReluLayer(nodes);
                        } else
                        {
                            Console.WriteLine("There is no layer type like \"" + words[i] + "\"");
                            return;
                        }

                    }

                    for(int i = 0; i < layers.Length; i++)
                    {
                        if (i != layers.Length - 1)
                        {
                            layers[i].weights = new Matrix(layers[i + 1].nodes, layers[i].nodes);
                            layers[i].weights.randomize(r);
                        }

                        layers[i].bias = new Matrix(layers[i].nodes, 1);
                        layers[i].bias.randomize(r);
                    }

                    nn = new Lib.NeuralNetwork(Lib.NeuralNetwork.random, layers);
                }
            } catch(Exception e)
            {
                printError(e);
                return;
            }
            Console.WriteLine("Neural Network was created succesfully!");
        }

        static Matrix feedForwardAlgirithm(String[] words)
        {
            try
            {
                if (words.Length == 1)
                {
                    Console.WriteLine("Feed Forward must have some parameters!");
                    return null;
                }
                float[] input = new float[words.Length - 1];
                for (int i = 0; i < words.Length - 1; i++)
                {
                    input[i] = float.Parse(words[i + 1]);
                }
                if (nn == null)
                {
                    Console.WriteLine("You must first initialize a Neural Network");
                    return null;
                }
                return nn.feedforward(input);

            } catch (Exception e)
            {
                Console.WriteLine("Feed Forward Failed!");
                printError(e);
                return null;
            }
}

        static void printError(Exception e)
        {
            Console.WriteLine(e.Message);
            if (printStackTrace) Console.WriteLine(e.StackTrace);
        }

        static void StackTrace(String[] words)
        {
            try
            {
                if (words.Length == 2) printStackTrace = Boolean.Parse(words[1]);
                else printStackTrace = !printStackTrace;
                Console.WriteLine("Stack Trace has been set to " + printStackTrace);
            } catch (Exception e)
            {
                printError(e);
            }
        }

        static void Clear()
        {
            Console.Clear();
            Console.WriteLine("Neural Network Test Program.");
            Console.WriteLine("Enter your commands:");
        }

        static void TestProgram()
        {/*
            Layer input = new Lib.Layers.Convolutional_2.ConvolutionalLayer(2, 2, 1, 0, 1);
            Layer cl1 = new Lib.Layers.Convolutional_2.ConvolutionalLayer(2, 2, 1, 0, 1);
            Layer relu1 = new Lib.Layers.Convolutional_2.LeakyReluLayer();
            Layer cl2 = new Lib.Layers.Convolutional_2.ConvolutionalLayer(2, 2, 1, 0, 1);
            Layer relu2 = new Lib.Layers.Convolutional_2.LeakyReluLayer();
            Layer fullyConnect = new Lib.Layers.Convolutional_2.FlattenLayer(16);

            input.initWeights(Lib.NeuralNetwork.random, null, cl1);
            cl1.initWeights(Lib.NeuralNetwork.random, null, cl2);

           // input.filters[0].kernels[0].data = new float[2, 2] { { 0, 1 }, { 2, 3 } };
           // cl1.filters[0].kernels[0].data = new float[2, 2] { { 0, 1 }, { 2, 3 } };


            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("Filter:");
                Matrix.table(input.filters[0].kernels[0]);
                input.featureMaps = new FeatureMap[1];
                input.featureMaps[0] = new FeatureMap();
                input.featureMaps[0].map = new Matrix(4, 4) { data = new float[4, 4] { { 1, 2, 3, 4 }, { 2, 4, 6, 8 }, { 3, 6, 9, 12 }, { 4, 8, 12, 16 } } };

                Console.WriteLine("Input:");
                Matrix.table(input.featureMaps[0].map);

                cl1.doFeedForward(input);
                relu1.doFeedForward(cl1);
                cl2.doFeedForward(relu1);
                relu2.doFeedForward(cl2);
                fullyConnect.doFeedForward(relu2);

                Console.WriteLine("Output:");
                Matrix.table(fullyConnect.values);

                Matrix targets = new Matrix(16, 1) { data = new float[16, 1] { { 1f }, { 2f }, { 3f }, { 4f }, { 5f }, { 6f }, { 7f }, { 8f }, { 9f }, { 10f }, { 11f }, { 12f }, { 13f }, { 14f }, { 15f }, { 16f }  } };

                Console.WriteLine("Targets:");
                Matrix.table(targets);

                Matrix errors = Matrix.subtract(targets, fullyConnect.values);

                Console.WriteLine("Errors:");
                Matrix.table(errors);

                Console.WriteLine("Learning Rate: " + Lib.NeuralNetwork.lr);
                //Lib.NeuralNetwork.setLearningRate(errors);
                Console.WriteLine("New Learning Rate: " + Lib.NeuralNetwork.lr);
                Console.WriteLine();

                fullyConnect.errors = errors;
                fullyConnect.doTrain(relu1, null, null, null);
                fullyConnect.errors = errors;
                fullyConnect.doTrain(relu1, null, null, null);
                relu1.doTrain(cl1, fullyConnect, null, null);
                Console.WriteLine("Derivatives:");
                Matrix.table(relu1.featureMaps[0].derivatives);

                cl1.doTrain(input, relu1, null, null);

            }*/

            Matrix output = new Matrix(2, 2) { data = new float[2, 2] { { 2, 3 }, { 3, 2 } } };
            Console.WriteLine("Output Errors");
            Matrix.table(output);

            Matrix filter = new Matrix(2, 2) { data = new float[2, 2] { { 2, 1.5f }, { 1, 0.5f } } };
            Console.WriteLine("Filter");
            Matrix.table(filter);

            Matrix flipFilter = filter.flip();
            Console.WriteLine("Flipped Filter");
            Matrix.table(flipFilter);

            Matrix input = new Matrix(3, 3);

            int width = 3;
            int height = 3;

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    for (int fx = 0; fx < 2; fx++)
                    {
                        for (int fy = 0; fy < 2; fy++)
                        {
                            if(!(x - 1 + fx < 0 || x - 1 + fx > 2-1 || y - 1 + fy < 0 || y - 1 + fy > 2-1))
                            {
                                input.data[x, y] +=
                                    output.data[x - 1 + fx, y - 1 + fy] *
                                    flipFilter.data[fx, fy];
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Input Errors");
            Matrix.table(input);

            


        }

        static void Train(String[] words)
        {
            Layer[] layer = new Layer[]
            {
                    new ConvolutionalLayer(3, 3, 3, 0, 1),
                    new ConvolutionalLayer(3, 3, 3, 0, 1),          //26
                    new Lib.Layers.Convolutional.LeakyReluLayer(),
                    new ConvolutionalLayer(2, 2, 3, 0, 1),          //24
                    new Lib.Layers.Convolutional.LeakyReluLayer(),
                    new ConvolutionalLayer(2, 2, 3, 0, 1),          //22
                    new Lib.Layers.Convolutional.LeakyReluLayer(),
                    new FullyConnectedLayer(1728),
                    new SoftmaxLayer(10)
            };

            nn = new Lib.NeuralNetwork(Lib.NeuralNetwork.random, layer);

            if (nn == null)
            {
                Console.WriteLine("you must first initialize a Neural Network.");
                return;
            }

            TrainingData[] data = Loader.loadTestMNSIT(@"C:\Users\drumm\Desktop\MNIST");
            Trainer trainer = new Trainer(data);

            Matrix.table(nn.feedforward(data[0].inputs));

            Matrix.table(data[0].labels);


            trainer.Train(nn);

            Matrix.table(data[0].labels);


            Matrix.table(nn.feedforward(data[0].inputs));
            Console.WriteLine("Training is completed succesfully!");
        }

        static void printTrainingData()
        {
            Matrix.table(nn.feedforward(new float[] { 1, 1 }));
            Matrix.table(nn.feedforward(new float[] { 0, 0 }));
            Matrix.table(nn.feedforward(new float[] { 0, 1 }));
            Matrix.table(nn.feedforward(new float[] { 1, 0 }));
        }
    }

    
}
