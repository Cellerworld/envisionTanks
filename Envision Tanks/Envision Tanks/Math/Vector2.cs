using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

namespace Envision.Tanks.Math
{
    public struct Vector2
    {
        /// <summary>
        /// The x-component of the vector.
        /// </summary>
        public float X;

        /// <summary>
        /// The y-component of the vector.
        /// </summary>
        public float Y;
        public float _X { get { return X; } set { X = value; } }
        public float _Y { get { return Y; } set { Y = value; } }

        /// <summary>
        /// Initializes a new instance of Vector2
        /// </summary>
        /// <param name="x">Initial value for the x-component of the vector.</param>
        /// <param name="y">Initial value for the y-component of the vector.</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region -- base overrides ---------------------------------------------

        public override string ToString()
        {
            return ToString(null);
        }

        /// <param name="format">Numeric format string applied to each component.</param>
        public string ToString(string format)
        {
            return String.Format("({0}, {1})", X.ToString(format), Y.ToString(format));
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                return Equals((Vector2)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        #endregion

        #region -- static properties ------------------------------------------

        private static Vector2 mZero = new Vector2(0, 0);
        private static Vector2 mOne = new Vector2(1, 1);
        private static Vector2 mUnitX = new Vector2(1, 0);
        private static Vector2 mUnitY = new Vector2(0, 1);
        private static Vector2 mMaxValue = new Vector2(Single.MaxValue, Single.MaxValue);
        private static Vector2 mMinValue = new Vector2(Single.MinValue, Single.MinValue);

        /// <summary>
        /// Returns a Vector2 with both of its components set to zero.
        /// </summary>
        public static Vector2 Zero
        {
            get { return mZero; }
        }

        /// <summary>
        /// Returns a Vector2 with both of its components set to one.
        /// </summary>
        public static Vector2 One
        {
            get { return mOne; }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return mUnitX; }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return mUnitY; }
        }

        /// <summary>
        /// Returns a Vector with both of its components set to the maximum single value.
        /// </summary>
        public static Vector2 MaxValue
        {
            get { return mMaxValue; }
        }

        /// <summary>
        /// Returns a Vector with both of its components set to the minimum single value.
        /// </summary>
        public static Vector2 MinValue
        {
            get { return mMinValue; }
        }

        #endregion

        #region -- public properties  -----------------------------------------

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return X;
                    case 1: return Y;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns a new normalized Vector2 from the current vector.
        /// </summary>
        public Vector2 Normalized
        {
            get
            {
                float length = Length();
                if (length != 0.0f)
                {
                    float div = 1.0f / length;
                    Vector2 result;
                    result.X = X * div;
                    result.Y = Y * div;
                    return result;
                }
                throw new InvalidOperationException("Error: can not normalize vector, the vector length is zero.");
            }
        }

        /// <summary>
        /// Returns a zero-based index representing the major axis of the current vector.
        /// '0' means x-axis; '1' means y-axis.
        /// </summary>
        public int MajorAxis
        {
            get
            {
                return System.Math.Abs(X) > System.Math.Abs(Y) ? 0 : 1;
            }
        }

        public float GetRotation()
        {
            float radians = (float)System.Math.Atan2(this.Y, this.X);
            float degrees = Rad2Deg(radians);

            if (degrees < 0)
            {
                degrees += 360;
            }

            return degrees;
        }

        public float Rad2Deg(float p_radian)
        {
            float degree = p_radian * (180 / (float)System.Math.PI);
            degree %= 360f;
            return degree;
        }

        #endregion

        #region -- public methods ---------------------------------------------

        /// <summary>
        /// Calculates the length of the current vector.
        /// </summary>
        /// <returns>The Length of the current vector.</returns>
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Squares the length of the current vector.
        /// </summary>
        /// <returns>The squared length of the current vector.</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        /// <summary>
        /// Normalizes the current vector
        /// </summary>
        public void Normalize()
        {
            float length = Length();
            if (length != 0.0f)
            {
                float div = 1.0f / length;
                X = X * div;
                Y = Y * div;
            }
            else
            {
                throw new Exception("Error: can not normalize vector, the vector length is zero.");
            }
        }

        /// <summary>
        /// Adds a given scalar value to each component of the current Vector2.
        /// </summary>
        /// <param name="value">The scalar value</param>
        public void Add(float value)
        {
            X = X + value;
            Y = Y + value;
        }

        /// <summary>
        /// Adds a given Vector2 to the current Vector2.
        /// </summary>
        /// <param name="other">The vector to be added.</param>
        public void Add(Vector2 other)
        {
            X = X + other.X;
            Y = Y + other.Y;
        }

        /// <summary>
        /// Subtracts a scalar value from each component of the current Vector2.
        /// </summary>
        /// <param name="value">The scalar value.</param>
        public void Subtract(float value)
        {
            X = X - value;
            Y = Y - value;
        }

        /// <summary>
        /// Subtracts a given Vector2 from the current Vector2.
        /// </summary>
        /// <param name="other">The vector to be subtracted.</param>
        public void Subtract(Vector2 other)
        {
            X = X - other.X;
            Y = Y - other.Y;
        }

        /// <summary>
        /// Multiplies each component of the current Vector2 by a given scalar value.
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        public void Multiply(float scalar)
        {
            X = X * scalar;
            Y = Y * scalar;
        }

        /// <summary>
        /// Multiplies the current Vector2 by another Vector2.
        /// </summary>
        /// <param name="other">The source vector.</param>
        public void Multiply(Vector2 other)
        {
            X = X * other.X;
            Y = Y * other.Y;
        }

        /// <summary>
        /// Divides each component of the current Vector2 by a given scalar value.
        /// </summary>
        /// <param name="divider">The scalar divider.</param>
        public void Divide(float divider)
        {
            if (divider == 0.0f)
            {
                throw new Exception("Error: divider is zero (Vector2.Divide).");
            }
            float s = 1.0f / divider;
            X = X * s;
            Y = Y * s;
        }

        /// <summary>
        /// Divides the current Vector2 by another Vector2.
        /// </summary>
        /// <param name="other">The vector divider.</param>
        public void Divide(Vector2 other)
        {
            if (other.X != 0.0f && other.Y != 0.0f)
            {
                X = X / other.X;
                Y = Y / other.Y;
            }
            else
            {
                throw new Exception("Error: divider vector contains zero (Vector2.Divide).");
            }
        }

        #endregion

        #region -- public static methods --------------------------------------

        /// <summary>
        /// Calculates the length of the given vector.
        /// </summary>
        /// <returns>The Length of the given vector.</returns>
        public static float Length(Vector2 value)
        {
            return value.Length();
        }

        /// <summary>
        /// Squares the length of the given vector.
        /// </summary>
        /// <returns>The squared length of the given vector.</returns>
        public static float LengthSquared(Vector2 value)
        {
            return value.LengthSquared();
        }

        /// <summary>
        /// Calculates the distance between two vectors. (manhatten/taxi-cab metrix)
        /// </summary>
        /// <param name="left">The source vector</param>
        /// <param name="right">The source vector</param>
        /// <returns>Distance between the two vectors.</returns>
        public static float DistanceManhatten(Vector2 left, Vector2 right)
        {
            return System.Math.Abs(left.X - right.X) + System.Math.Abs(left.Y - right.Y);
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="left">The source vector</param>
        /// <param name="right">The source vector</param>
        /// <returns>Distance between the two vectors.</returns>
        public static float Distance(Vector2 left, Vector2 right)
        {
            float dx = left.X - right.X;
            float dy = left.Y - right.Y;
            return (float)System.Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Squares the distance between two vectors.
        /// </summary>
        /// <param name="left">The source vector</param>
        /// <param name="right">The source vector</param>
        /// <returns>Squared distance between the two vectors.</returns>
        public static float DistanceSquared(Vector2 left, Vector2 right)
        {
            float dx = left.X - right.X;
            float dy = left.Y - right.Y;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static float Dot(Vector2 left, Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The cross product of the two vectors (this is the magnitude of the 3-dimensional cross product).</returns>
        public static float Cross(Vector2 left, Vector2 right)
        {
            return left.X * right.Y - left.Y * right.X;
        }

        /// <summary>
        /// Creates a unit vector from the specified vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The created unit vector.</returns>
        public static Vector2 Normalize(Vector2 value)
        {
            return value.Normalized;
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The source vector.</param>
        /// <returns>The minimized vector.</returns>
        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
            Vector2 result;
            result.X = System.Math.Min(value1.X, value2.X);
            result.Y = System.Math.Min(value1.Y, value2.Y);
            return result;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The source vector.</param>
        /// <returns>The maximized vector.</returns>
        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
            Vector2 result;
            result.X = System.Math.Max(value1.X, value2.X);
            result.Y = System.Math.Max(value1.Y, value2.Y);
            return result;
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>A new vector pointing in the opposite direction.</returns>
        public static Vector2 Negate(Vector2 value)
        {
            Vector2 result;
            result.X = -value.X;
            result.Y = -value.Y;
            return result;
        }

        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The source vector.</param>
        /// <returns>A new vector representing the sum of the source vectors.</returns>
        public static Vector2 Add(Vector2 value1, Vector2 value2)
        {
            Vector2 result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            return result;
        }

        /// <summary>
        /// Adds a given scalar value to each component of a given vector.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The scalar value to be added to the vector.</param>
        /// <returns>A new Vector2 representing the sum of the given vector and scalar.</returns>
        public static Vector2 Add(Vector2 value1, float value2)
        {
            Vector2 result;
            result.X = value1.X + value2;
            result.Y = value1.Y + value2;
            return result;
        }

        /// <summary>
        /// Subtracts a vector from another vector.
        /// </summary>
        /// <param name="value1">The vector to be subtracted from.</param>
        /// <param name="value2">The vector to be subtracted.</param>
        /// <returns>A new vector representing the result of the subtraction.</returns>
        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
        {
            Vector2 result;
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            return result;
        }

        /// <summary>
        /// Subtracts a given scalar value from each component of a given vector.
        /// </summary>
        /// <param name="value1">The vector to be subtracted from.</param>
        /// <param name="value2">The scalar value to subtracted.</param>
        /// <returns>A new Vector2 representing the result of the subtraction.</returns>
        public static Vector2 Subtract(Vector2 value1, float value2)
        {
            Vector2 result;
            result.X = value1.X - value2;
            result.Y = value1.Y - value2;
            return result;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other. 
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The source vector.</param>
        /// <returns>A new vector representing the result of the mulitiplication.</returns>
        public static Vector2 Multiply(Vector2 value1, Vector2 value2)
        {
            Vector2 result;
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>A new vector representing the result of the mulitiplication.</returns>
        public static Vector2 Multiply(Vector2 value, float scalar)
        {
            Vector2 result;
            result.X = value.X * scalar;
            result.Y = value.Y * scalar;
            return result;
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The divisor vector.</param>
        /// <returns>A new vector representing the result of the division.</returns>
        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            Vector2 result;
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            return result;
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <param name="divider">The divider</param>
        /// <returns>A new vector representing the result of the division.</returns>
        public static Vector2 Divide(Vector2 value, float divider)
        {
            float scalar = 1.0f / divider;
            Vector2 result;
            result.X = value.X * scalar;
            result.Y = value.Y * scalar;
            return result;
        }

        /// <summary>
        ///  Creates a new Vector2 with each component being the result of dividing a scalar value by the corresponding component of a vector.
        /// </summary>
        /// <param name="value">The scalar value to be divided.</param>
        /// <param name="divider">The divider vector</param>
        /// <returns>A new vector with each component being the result of dividing the scalar value by the corresponding component of the vector.</returns>
        public static Vector2 Divide(float value, Vector2 divider)
        {
            Vector2 result;
            result.X = value / divider.X;
            result.Y = value / divider.Y;
            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The source vector.</param>
        /// <param name="amount">The value between 0 and 1 indicating the weight of value2. '0.0' will cause value1 to be returned; '1.0' will cause value2 to be returned.</param>
        /// <returns>The linear interpolation of the two vectors.</returns>
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            Vector2 result;
            result.X = value1.X + (amount * (value2.X - value1.X));
            result.Y = value1.Y + (amount * (value2.Y - value1.Y));
            return result;
        }

        public static Vector2 RotationToVectorR(float radians)
        {
            return new Vector2((float)System.Math.Cos(radians), (float)System.Math.Sin(radians));
        }

        public static Vector2 RotationToVectorD(float degree)
        {
            float radians = degree * 0.01745329252f;
            return RotationToVectorR(radians);
        }

        #endregion

        #region -- operators --------------------------------------------------

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }

        public static Vector2 operator -(Vector2 value)
        {
            return Negate(value);
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return Add(left, right);
        }

        public static Vector2 operator +(Vector2 left, float right)
        {
            return Add(left, right);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return Subtract(left, right);
        }

        public static Vector2 operator -(Vector2 left, float right)
        {
            return Subtract(left, right);
        }

        public static Vector2 operator *(Vector2 left, Vector2 right)
        {
            return Multiply(left, right);
        }

        public static Vector2 operator *(Vector2 left, float right)
        {
            return Multiply(left, right);
        }

        public static Vector2 operator *(float left, Vector2 right)
        {
            return Multiply(right, left);
        }

        public static Vector2 operator /(Vector2 left, Vector2 right)
        {
            return Divide(left, right);
        }

        public static Vector2 operator /(Vector2 left, float right)
        {
            return Divide(left, right);
        }

        public static Vector2 operator /(float left, Vector2 right)
        {
            return Divide(left, right);
        }

        #endregion
    }
}

