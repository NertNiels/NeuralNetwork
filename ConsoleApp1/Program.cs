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
        {
            #region TestPrograms

            /*

            TrainingData[] training_data = new TrainingData[]
            {
                new TrainingData(new float[]{ 0, 1 }, new float[] { 1 }),
                new TrainingData(new float[]{ 1, 1 }, new float[] { 0 }),
                new TrainingData(new float[]{ 0, 0 }, new float[] { 0 }),
                new TrainingData(new float[]{ 1, 0 }, new float[] { 1 }),
            };

            Matrix.table(nn.feedforward(new float[] { 1, 1 }));
            Matrix.table(nn.feedforward(new float[] { 0, 0 }));
            Matrix.table(nn.feedforward(new float[] { 0, 1 }));
            Matrix.table(nn.feedforward(new float[] { 1, 0 }));

            for(int i = 0; i < 50000; i++)
            {
                int currentIndex = (int)Math.Floor(Lib.NeuralNetwork.random.NextDouble() * training_data.Length);
                TrainingData train_data = training_data[currentIndex];
                nn.train(train_data.inputs, train_data.targets);
            }

            Matrix.table(nn.feedforward(new float[] { 1, 1 }));
            Matrix.table(nn.feedforward(new float[] { 0, 0 }));
            Matrix.table(nn.feedforward(new float[] { 0, 1 }));
            Matrix.table(nn.feedforward(new float[] { 1, 0 }));
            */

            /*
            Matrix m1 = new Matrix(5, 1);
            m1.data[0, 0] = 4;
            m1.data[1, 0] = 44;
            m1.data[2, 0] = 42;
            m1.data[3, 0] = 50;
            m1.data[4, 0] = 41;
            Matrix.table(m1);

            Console.WriteLine(Matrix.sum(m1));

            Matrix m2 = Activation.softmax(m1);
            Matrix.table(m2);

            Console.WriteLine(Matrix.sum(m2));


            Matrix m3 = Matrix.map(Activation.dsoftmax, m2);
            Matrix.table(m3);

            Console.WriteLine(Matrix.sum(m3));
            */

            #endregion

            Layer[] layer = new Layer[]
            {
                new ConvolutionalLayer(2, 2, 3, 1, 1),
                new ConvolutionalLayer(2, 2, 3, 0, 1),
                new Lib.Layers.Convolutional.LeakyReluLayer(),
                new ConvolutionalLayer(2, 2, 3, 0, 1),
                new Lib.Layers.Convolutional.LeakyReluLayer(),
                new FullyConnectedLayer(12),
                new SoftmaxLayer(2)
            };

            Lib.NeuralNetwork neuralNetwork = new Lib.NeuralNetwork(Lib.NeuralNetwork.random, layer);

            Matrix input = new Matrix(4, 4);
            input.data = new float[,]
            {
                {1, 2, 3, 4 },
                {5, 6, 7, 8 },
                {9, 1, 2, 3 },
                {4, 5, 6, 7 }
            };

            Matrix output = neuralNetwork.feedforward(input);
            Matrix.table(output);

            for (int i = 0; i < 20; i++)
            {
                neuralNetwork.train(input, new float[] { 1, 0 });
                Console.WriteLine("Iteration: " + (i + 1));
                output = neuralNetwork.feedforward(input);
                Matrix.table(output);
            }

            

        }

        static void Train(String[] words)
        {
            try
            {
                if (nn == null)
                {
                    Console.WriteLine("you must first initialize a Neural Network.");
                    return;
                }

                Random r = new Random();

                TrainingData[] training_data = new TrainingData[]
                {
                new TrainingData(new float[]{ 0, 1 }, new float[] { 1 }),
                new TrainingData(new float[]{ 1, 1 }, new float[] { 0 }),
                new TrainingData(new float[]{ 0, 0 }, new float[] { 0 }),
                new TrainingData(new float[]{ 1, 0 }, new float[] { 1 }),
                };

                if (words.Length > 1)
                {
                    if (!words[1].Equals(""))
                    {
                        int training_times = 1;
                        try { training_times = int.Parse(words[1]); }
                        catch { Console.WriteLine("The parameter you've entered is invalid, training stops."); return; }

                        Console.WriteLine("Training in progress...");

                        for (int i = 0; i < training_times; i++)
                        {
                            float percentage = (int)Math.Round((double)((((float)i / (float)training_times) * (float)100)));
                            Console.Write("\r{0}%   ", percentage);


                            int currentIndex = (int)Math.Floor(r.NextDouble() * training_data.Length);
                            TrainingData train_data = training_data[currentIndex];
                            nn.train(train_data.inputs, train_data.targets);
                        }
                        Console.WriteLine("");
                        Console.WriteLine("Training is completed succesfully!");
                        return;
                    }
                }

                int index = (int)Math.Floor(r.NextDouble() * training_data.Length);
                TrainingData td = training_data[index];
                nn.train(td.inputs, td.targets);
            } catch (Exception e) { Console.WriteLine("Training failed!"); printError(e); }
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
