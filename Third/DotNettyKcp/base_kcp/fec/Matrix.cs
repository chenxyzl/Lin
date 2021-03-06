using System;
using System.Text;

namespace fec;

public class Matrix
{
    /**
     * The number of columns in the matrix.
     */
    private readonly int columns;

    /**
     * The data in the matrix, in row major form.
     *
     * To get element (r, c): data[r][c]
     *
     * Because this this is computer science, and not math,
     * the indices for both the row and column start at 0.
     */
    private readonly byte[][] data;

    /**
     * The number of rows in the matrix.
     */
    private readonly int rows;

    /**
     * Initialize a matrix of zeros.
     *
     * @param initRows The number of rows in the matrix.
     * @param initColumns The number of columns in the matrix.
     */
    public Matrix(int initRows, int initColumns)
    {
        rows = initRows;
        columns = initColumns;
        data = new byte [rows][];
        for (var r = 0; r < rows; r++) data[r] = new byte [columns];
    }

    /**
     * Initializes a matrix with the given row-major data.
     */
    public Matrix(byte[][] initData)
    {
        rows = initData.Length;
        columns = initData[0].Length;
        data = new byte [rows][];
        for (var r = 0; r < rows; r++)
        {
            if (initData[r].Length != columns) throw new Exception("Not all rows have the same number of columns");
            data[r] = new byte[columns];
            for (var c = 0; c < columns; c++) data[r][c] = initData[r][c];
        }
    }

    /**
     * Returns an identity matrix of the given size.
     */
    public static Matrix identity(int size)
    {
        var result = new Matrix(size, size);
        for (var i = 0; i < size; i++) result.set(i, i, 1);
        return result;
    }

    /**
     * Returns a human-readable string of the matrix contents.
     *
     * Example: [[1, 2], [3, 4]]
     */
    public string toString()
    {
        var result = new StringBuilder();
        result.Append('[');
        for (var r = 0; r < rows; r++)
        {
            if (r != 0) result.Append(", ");
            result.Append('[');
            for (var c = 0; c < columns; c++)
            {
                if (c != 0) result.Append(", ");
                result.Append(data[r][c] & 0xFF);
            }

            result.Append(']');
        }

        result.Append(']');
        return result.ToString();
    }

    /**
         * Returns a human-readable string of the matrix contents.
         * 
         * Example:
         * 00 01 02
         * 03 04 05
         * 06 07 08
         * 09 0a 0b
         */
    public string toBigString()
    {
        var result = new StringBuilder();
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < columns; c++)
            {
                int value = get(r, c);
                if (value < 0) value += 256;
                result.Append(string.Format("%02x ", value));
            }

            result.Append("\n");
        }

        return result.ToString();
    }

    /**
     * Returns the number of columns in this matrix.
     */
    public int getColumns()
    {
        return columns;
    }

    /**
     * Returns the number of rows in this matrix.
     */
    public int getRows()
    {
        return rows;
    }

    /**
     * Returns the value at row r, column c.
     */
    public byte get(int r, int c)
    {
        if (r < 0 || rows <= r) throw new Exception("Row index out of range: " + r);
        if (c < 0 || columns <= c) throw new Exception("Column index out of range: " + c);
        return data[r][c];
    }

    /**
     * Sets the value at row r, column c.
     */
    public void set(int r, int c, byte value)
    {
        if (r < 0 || rows <= r) throw new Exception("Row index out of range: " + r);
        if (c < 0 || columns <= c) throw new Exception("Column index out of range: " + c);
        data[r][c] = value;
    }

    /**
     * Returns true iff this matrix is identical to the other.
     */
    public bool equals(object other)
    {
        if (!(other is Matrix)) return false;
        for (var r = 0; r < rows; r++)
            if (!Equals(data[r], ((Matrix) other).data[r]))
                return false;
        return true;
    }

    /**
     * Multiplies this matrix (the one on the left) by another
     * matrix (the one on the right).
     */
    public Matrix times(Matrix right)
    {
        if (getColumns() != right.getRows())
            throw new Exception(
                "Columns on left (" + getColumns() + ") " +
                "is different than rows on right (" + right.getRows() + ")");
        var result = new Matrix(getRows(), right.getColumns());
        for (var r = 0; r < getRows(); r++)
        for (var c = 0; c < right.getColumns(); c++)
        {
            byte value = 0;
            for (var i = 0; i < getColumns(); i++) value ^= Galois.multiply(get(r, i), right.get(i, c));
            result.set(r, c, value);
        }

        return result;
    }

    /**
     * Returns the concatenation of this matrix and the matrix on the right.
     */
    public Matrix augment(Matrix right)
    {
        if (rows != right.rows) throw new Exception("Matrices don't have the same number of rows");
        var result = new Matrix(rows, columns + right.columns);
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < columns; c++) result.data[r][c] = data[r][c];
            for (var c = 0; c < right.columns; c++) result.data[r][columns + c] = right.data[r][c];
        }

        return result;
    }

    /**
     * Returns a part of this matrix.
     */
    public Matrix submatrix(int rmin, int cmin, int rmax, int cmax)
    {
        var result = new Matrix(rmax - rmin, cmax - cmin);
        for (var r = rmin; r < rmax; r++)
        for (var c = cmin; c < cmax; c++)
            result.data[r - rmin][c - cmin] = data[r][c];
        return result;
    }

    /**
     * Returns one row of the matrix as a byte array.
     */
    public byte[] getRow(int row)
    {
        var result = new byte [columns];
        for (var c = 0; c < columns; c++) result[c] = get(row, c);
        return result;
    }

    /**
     * Exchanges two rows in the matrix.
     */
    public void swapRows(int r1, int r2)
    {
        if (r1 < 0 || rows <= r1 || r2 < 0 || rows <= r2) throw new Exception("Row index out of range");
        var tmp = data[r1];
        data[r1] = data[r2];
        data[r2] = tmp;
    }

    /**
     * Returns the inverse of this matrix.
     *
     * @throws IllegalArgumentException when the matrix is singular and
     * doesn't have an inverse.
     */
    public Matrix invert()
    {
        // Sanity check.
        if (rows != columns) throw new Exception("Only square matrices can be inverted");

        // Create a working matrix by augmenting this one with
        // an identity matrix on the right.
        var work = augment(identity(rows));

        // Do Gaussian elimination to transform the left half into
        // an identity matrix.
        work.gaussianElimination();

        // The right half is now the inverse.
        return work.submatrix(0, rows, columns, columns * 2);
    }

    /**
     * Does the work of matrix inversion.
     *
     * Assumes that this is an r by 2r matrix.
     */
    private void gaussianElimination()
    {
        // Clear out the part below the main diagonal and scale the main
        // diagonal to be 1.
        for (var r = 0; r < rows; r++)
        {
            // If the element on the diagonal is 0, find a row below
            // that has a non-zero and swap them.
            if (data[r][r] == 0)
                for (var rowBelow = r + 1; rowBelow < rows; rowBelow++)
                    if (data[rowBelow][r] != 0)
                    {
                        swapRows(r, rowBelow);
                        break;
                    }

            // If we couldn't find one, the matrix is singular.
            if (data[r][r] == 0) throw new Exception("Matrix is singular");
            // Scale to 1.
            if (data[r][r] != 1)
            {
                var scale = Galois.divide(1, data[r][r]);
                for (var c = 0; c < columns; c++) data[r][c] = Galois.multiply(data[r][c], scale);
            }

            // Make everything below the 1 be a 0 by subtracting
            // a multiple of it.  (Subtraction and addition are
            // both exclusive or in the Galois field.)
            for (var rowBelow = r + 1; rowBelow < rows; rowBelow++)
                if (data[rowBelow][r] != 0)
                {
                    var scale = data[rowBelow][r];
                    for (var c = 0; c < columns; c++) data[rowBelow][c] ^= Galois.multiply(scale, data[r][c]);
                }
        }

        // Now clear the part above the main diagonal.
        for (var d = 0; d < rows; d++)
        for (var rowAbove = 0; rowAbove < d; rowAbove++)
            if (data[rowAbove][d] != 0)
            {
                var scale = data[rowAbove][d];
                for (var c = 0; c < columns; c++) data[rowAbove][c] ^= Galois.multiply(scale, data[d][c]);
            }
    }
}