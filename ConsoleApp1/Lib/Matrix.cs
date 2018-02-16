using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Lib
{
    class Matrix
    {
        public int rows;
        public int cols;
        public float[,] data;

        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            data = new float[rows,cols];
        }

        public static Matrix fromArray(float[] arr)
        {
            Matrix m = new Matrix(arr.Length, 1);
            for (int i = 0; i < arr.Length; i++)
            {
                m.data[i,0] = arr[i];
            }
            return m;
        }

        public void randomize(Random r)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i,j] = (float)r.NextDouble() * 2 - 1;
                }
            }
        }

        public static Matrix transpose(Matrix m)
        {
            Matrix output = new Matrix(m.cols, m.rows);
            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.cols; j++)
                {
                    output.data[j,i] = m.data[i,j];
                }
            }
            return output;
        }

        public void multiply(float n)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i,j] *= n;
                }
            }
        }

        public void hadamard(Matrix n)
        {
            if(!(rows == n.rows && cols == n.cols))
            {
                Console.WriteLine("Matrices must have the same dimentions");
                return;
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i,j] *= n.data[i,j];
                }
            }
        }

        public static Matrix multiply(Matrix a, Matrix b)
        {
            if (a.cols != b.rows)
            {
                Console.WriteLine("Cols of A and Rows of B must be equaled");
                return null;
            }
            Matrix output = new Matrix(a.rows, b.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.cols; j++)
                {
                    float sum = 0;
                    for (int k = 0; k < a.cols; k++)
                    {
                        sum += a.data[i,k] * b.data[k,j];
                    }
                    output.data[i,j] = sum;
                }
            }
            return output;
        }

        public void add(int n)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i,j] += n;
                }
            }
        }

        public void add(Matrix m)
        {
            if (!(rows == m.rows && cols == m.cols))
            {
                Console.WriteLine("Matrices must have the same dimentions");
                return;
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i,j] += m.data[i,j];
                }
            }
        }

        public void subtract(int n)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] -= n;
                }
            }
        }

        public void subtract(Matrix m)
        {
            if (!(rows == m.rows && cols == m.cols))
            {
                Console.WriteLine("Matrices must have the same dimentions");
                return;
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] -= m.data[i, j];
                }
            }
        }

        public static Matrix subtract(Matrix a, Matrix b)
        {
            if (!(a.rows == b.rows && a.cols == b.cols))
            {
                Console.WriteLine("Matrices must have the same dimentions");
                return null;
            }
            Matrix output = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.cols; j++)
                {
                    output.data[i, j] = a.data[i, j] - b.data[i,j];
                }
            }
            return output;
        }

        public void map(Func<float, float> function)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i,j] = function(data[i,j]);
                }
            }
        }

        public static Matrix map(Func<float, float> function, Matrix m)
        {
            Matrix output = new Matrix(m.rows, m.cols);
            for(int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.cols; j++)
                {
                    output.data[i, j] = function(m.data[i, j]);
                }
            }
            return output;
        }

        public static void table(Matrix m)
        {
            if (m == null) return;
            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.cols; j++)
                {
                    Console.Write(m.data[i,j] + ", ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
}
