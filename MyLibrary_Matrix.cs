using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    public class Matrix
    {
        const double EPS = 10E-17;
        double[,] _value;

        public double this[int row, int col]
        {
            get
            {
                return _value[row, col];
            }
            set
            {
                _value[row, col] = value;
            }
        }
        public int RowNum { get { return _value.GetLength(0); } }
        public int ColNum { get { return _value.GetLength(1); } }

        public Matrix(int row, int col)
        {
            _value = new double[row, col];
        }

        public Matrix(double[,] value)
        {
            _value = value;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.RowNum != m2.RowNum || m1.ColNum != m2.ColNum) throw new ArgumentException("Canot operate the addition. Both matrix should have the same number of rows and columns");
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = m1[row, col] + m2[row, col];
                }
            }
            return ret;
        }

        public static Matrix operator +(double p, Matrix m1)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = m1[row, col] + p;
                }
            }
            return ret;
        }

        public static Matrix operator +(Matrix m1, double p)
        {
            return p + m1;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return m1 + (-1 * m2);
        }

        public static Matrix operator -(Matrix m1, double p)
        {
            return m1 + (-1 * p);
        }

        public static Matrix operator -(double p, Matrix m1)
        {
            return p + (m1 * -1);
        }

        public static Matrix operator *(Matrix m1, double p)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = p == 0.0 ? 0.0 : m1[row, col] * p;
                }
            }
            return ret;
        }

        public static Matrix operator *(double p, Matrix m1)
        {
            return m1 * p;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.ColNum != m2.RowNum) throw new ArgumentException("Cannot opereate the product. A number of m1 columns should be the same as a number of m2 rows.");
            var ret = new Matrix(m1.RowNum, m2.ColNum);

            for (int row = 0; row < ret.RowNum; row++)
            {
                for (int col = 0; col < ret.ColNum; col++)
                {
                    var sum = 0.0;

                    for (int idx = 0; idx < m1.ColNum; idx++)
                    {
                        sum += (m1[row, idx] == 0.0 || m2[idx, col] == 0.0) ? 0.0 : m1[row, idx] * m2[idx, col];
                    }

                    ret[row, col] = sum;
                }
            }
            return ret;
        }

        public static Matrix operator /(Matrix m1, double p)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = m1[row, col] / p;
                }
            }
            return ret;
        }

        public static Matrix operator /(double p, Matrix m1)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = p / m1[row, col];
                }
            }
            return ret;
        }

        public static Matrix operator /(Matrix m1, Matrix m2)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = m1[row, col] / m2[row, col];
                }
            }
            return ret;
        }

        public static Matrix operator ^(double p, Matrix m1)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = Math.Pow(p, m1[row, col]);
                }
            }
            return ret;
        }

        public static Matrix operator ^(Matrix m1, double p)
        {
            var ret = new Matrix(m1.RowNum, m1.ColNum);

            for (int row = 0; row < m1.RowNum; row++)
            {
                for (int col = 0; col < m1.ColNum; col++)
                {
                    ret[row, col] = Math.Pow(m1[row, col], p);
                }
            }
            return ret;
        }

        public Matrix Transpose()
        {
            var ret = new Matrix(ColNum, RowNum);
            for (int row = 0; row < RowNum; row++)
            {
                for (int col = 0; col < ColNum; col++)
                {
                    ret[col, row] = this[row, col];
                }
            }
            return ret;
        }

        public Matrix Inverse()
        {
            if (RowNum != ColNum) throw new InvalidOperationException("Rows and columns must have the same number of values.");
            var ret = Matrix.Eye(RowNum);

            var wk = this.Clone();
            for (int targetRow = 0; targetRow < RowNum; targetRow++)
            {
                //convert target row as [0,0,1,a,b,c]
                var div = wk[targetRow, targetRow];
                if (Math.Abs(div) < EPS) throw new InvalidOperationException("Cannot inverse the matrix.");
                for (int col = 0; col < ColNum; col++)
                {
                    wk[targetRow, col] /= div;
                    ret[targetRow, col] /= div;
                }

                //convert other rows using the target row
                for (int otherRow = 0; otherRow < RowNum; otherRow++)
                {
                    if (otherRow == targetRow) continue;
                    var mux = wk[otherRow, targetRow];
                    for (int col = targetRow; col < ColNum; col++)
                    {
                        wk[otherRow, col] -= (mux * wk[targetRow, col]);
                    }
                    for (int col = 0; col < ColNum; col++)
                    {
                        ret[otherRow, col] -= (mux * ret[targetRow, col]);
                    }
                }
            }

            return ret;
        }

        public Matrix Clone()
        {
            var value = new double[RowNum, ColNum];
            for (int row = 0; row < RowNum; row++)
            {
                for (int col = 0; col < ColNum; col++)
                {
                    value[row, col] = this[row, col];
                }
            }
            return new Matrix(value);
        }

        public static Matrix Eye(int n)
        {
            var ret = new Matrix(n, n);
            for (int i = 0; i < n; i++) ret[i, i] = 1;
            return ret;
        }

        public Matrix Mean()
        {
            var ret = new Matrix(1, ColNum);
            if (RowNum == 0) return ret;

            for (int col = 0; col < ColNum; col++)
            {
                var sum = 0.0;
                for (int row = 0; row < RowNum; row++)
                {
                    sum += this[row, col];
                }
                ret[0, col] = (sum / RowNum);
            }

            return ret;
        }

        public Matrix Std()
        {
            var ret = new Matrix(1, ColNum);
            if (RowNum == 0) return ret;

            for (int col = 0; col < ColNum; col++)
            {
                var sum = 0.0;
                for (int row = 0; row < RowNum; row++)
                {
                    sum += this[row, col];
                }
                var avg = (sum / RowNum);

                var stdDiv = 0.0;
                for (int row = 0; row < RowNum; row++)
                {
                    stdDiv += (this[row, col] - avg) * (this[row, col] - avg);
                }
                stdDiv /= (RowNum == 1 ? 1 : RowNum - 1);
                stdDiv = Math.Sqrt(stdDiv);

                ret[0, col] = stdDiv;
            }

            return ret;
        }

        public Matrix Log()
        {
            var ret = this.Clone();

            for (int row = 0; row < RowNum; row++)
            {
                for (int col = 0; col < ColNum; col++)
                {
                    ret[row, col] = Math.Log(this[row, col]);
                }
            }

            return ret;
        }

        public double Sum()
        {
            var ret = 0.0;
            for (int row = 0; row < RowNum; row++)
            {
                for (int col = 0; col < ColNum; col++)
                {
                    ret += this[row, col];
                }
            }
            return ret;
        }

        /// <summary>
        /// return tuple of [NormalizedValues, mu, sigma]
        /// </summary>
        public Tuple<Matrix, Matrix, Matrix> NormalizeFeature(bool ignoreFirstCol = true)
        {
            var xNorm = this.Clone();
            var mu = this.Mean();
            var sigma = this.Std();

            //xNorm -= mu;
            //xNorm /= sigma;
            for (int col = 0; col < ColNum; col++)
            {
                if (col == 0 && ignoreFirstCol) continue;
                for (int row = 0; row < xNorm.RowNum; row++)
                {
                    xNorm[row, col] -= mu[0, col];
                    if (sigma[0, col] != 0)
                        xNorm[row, col] /= sigma[0, col];
                }
            }

            return new Tuple<Matrix, Matrix, Matrix>(xNorm, mu, sigma);
        }
    }
}