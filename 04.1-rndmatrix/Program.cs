using System.Text.RegularExpressions;

#region Questions

//? 1. .net pattern limitation

// it's a pity that C# has such concise templates or i don't understand?
// anyway how to do this in c#

// class RandomMatrix<T t, constexpr int nrows, constexpr int ncols> { /* ... */ }

//? 2. if i want value type limitation in template is this correct limitattion?

// class RandomMatrixGeneric<T> where T : struct { /* ... */ }

//? 3. is in c# some like
// #ifdefine usafe {} #endif?

//? 4. How to do cast conversion with type parameter

/*
switch (typeof(T))
{
    case Type t when t.Equals(typeof(double)):
        matrix[rank, j] = rnd.NextDouble(); //! cast error
        rnd.NextDouble();
        break;
    case Type t when t.Equals(typeof(int)):
        matrix[rank, j] = rnd.Next(); //! cast error
        break;
    default:
        throw new System.InvalidOperationException("not implemented");
};
*/

//? 5. Can I pass tuple as optional param, like that?

// public RandomMatrix(int rows, int cols, (int?, int?) lim = (null, null))

#endregion

#region Main

string? dimSpec = null;
Regex rxDim = new Regex(@"^\s*(\d+)\s*[:x]\s*(\d+)\s*$", RegexOptions.IgnoreCase);
Match? mDim;

do {
    // dimension spec in form: rows x cols || rows:cols
    Console.Write("Dimensions [rows:columns]? ");
    dimSpec = Console.ReadLine();
    mDim = dimSpec != null ? rxDim.Match(dimSpec) : mDim = null;
} while (mDim == null ? true : (!mDim!.Success && mDim!.Groups.Count != 3));

RandomMatrix randomMatrix = new RandomMatrix(int.Parse(mDim.Groups[1].Value),
                                             int.Parse(mDim.Groups[2].Value),
                                             0, 99);

Console.WriteLine(randomMatrix);

#endregion

#region Bad experience with C# templates

// in task requirements type of numbers not specified, so i need generic class

//* idea was to do this
// RandomMatrix<int> randomMatrix = new RandomMatrix<int>(3, 3);
// RandomMatrix<double> randomMatrix = new RandomMatrix<double>(3, 3);

class RandomMatrixGeneric<T>
    where T : struct //? is this correct limitattion?
{
    const int kMemLimit = 0X7FEFFFFF;
    T[,] matrix;
    int nrows = 0 , ncols = 0;
    public RandomMatrixGeneric (int rows, int cols) // in case of rectangular matrix
    {
        // fixme: unsafe { typeSize = sizeof(T); }
        int typeSize = sizeof(double);

        if (rows * cols >= Array.MaxLength || rows * cols * typeSize >= kMemLimit)
            throw new System.InvalidOperationException("requested matrix size too long");

        nrows = rows;
        ncols = cols;
        matrix = new T[nrows, ncols];

        Random rnd = new Random();

        // using nested loops, due task requirements
        // iterators will be added later

        for (int rank = 0; rank < matrix.Rank; rank++) // i == rank in .net terminology
        {
            for (int j = 0; j < matrix.GetLength(rank); j++)
            {
                //! why this not working?
                /*
                matrix[rank, j] = typeof(T) switch
                {
                    Type t when t.Equals(typeof(double)) => (T) rnd.NextDouble(),
                    Type t when t.Equals(typeof(int)) => (T) rnd.Next(),
                    _ => throw new System.InvalidOperationException("not implemented"),
                };
                */

                //! ugly and not working for value types
                /*
                if (itemType.Equals(typeof(double)))
                    matrix[rank,j]= (T) rnd.NextDouble();
                else if (itemType.Equals(typeof(int)))
                    matrix[rank,j]= rnd.Next() as T;
                else
                    throw new System.InvalidOperationException("not implemented");
                */

                //! even more ugly, also not working
                /*
                switch (typeof(T))
                {
                    case Type t when t.Equals(typeof(double)):
                        matrix[rank, j] = rnd.NextDouble(); //! cast error
                        rnd.NextDouble();
                        break;
                    case Type t when t.Equals(typeof(int)):
                        matrix[rank, j] = rnd.Next(); //! cast error
                        break;
                    default:
                        throw new System.InvalidOperationException("not implemented");
                };
                */

            }
        }
    }
}

#endregion

#region Working with int only

class RandomMatrix
{
    int[,] matrix;
    int[] total;
    public RandomMatrix(int rows, int cols, int? min = null, int? max = null)
    {
        const int kMemLimit = 0X7FEFFFFF;

        // removed because if (rows < 1 || cols < 1) is meanless
        // if rows or cols <= 0 attemp to create array will raise exception
        // if (rows < 2 || cols < 2)
        //     throw new System.ArgumentException("some mess in matrix dimension");

        if (rows * cols >= Array.MaxLength || rows * cols * sizeof(int) >= kMemLimit)
            throw new System.InvalidOperationException("requested matrix size too long");

        matrix = new int[rows, cols];
        total = new int[cols];

        Random rnd = new Random();

        // task requires creating a 2d array, overcomplicate not on this time
        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                switch (min, max)
                {
                    case (null, null):
                        matrix[i,j] = rnd.Next();
                        break;
                    case (null, not null):
                        matrix[i,j] = rnd.Next(max.Value);
                        break;
                    case (not null, not null):
                        matrix[i,j] = rnd.Next(min.Value, max.Value);
                        break;
                    default:
                        throw new System.ArgumentException("some mess in limits");
                }

        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                total[j] += matrix[i, j];
    }

    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                str += matrix[i, j].ToString() + "\t";
            str += "\n";
        }

        for (int j = 0; j < matrix.GetLength(1); j++)
            str += "--\t";

        str += "\n";

        for (int j = 0; j < matrix.GetLength(1); j++)
            str += total[j].ToString() + "\t";

        return str;
    }
}

#endregion