using PixelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelEngine
{
    internal class Mat3<T>
    {
        public Mat3()
        {
            dynamic zero = default(T);
            dynamic one = zero + 1;

            elements = new T[3, 3]{
                { (T)one,(T)zero,(T)zero },
                { (T)zero,(T)one,(T)zero },
                { (T)zero,(T)zero,(T)one }
            };
        }

        public Mat3(Mat3<T> rhs)
        {
            elements = rhs.elements;
        }

        public static Mat3<T> operator *(Mat3<T> lhs, T rhs)
        {
            dynamic dRhs = rhs;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    lhs.elements[row, col] *= dRhs;
                }
            }
            return lhs;
        }
        public static Mat3<T> operator *(Mat3<T> lhs, Mat3<T> rhs)
        {
            Mat3<T> result = new Mat3<T>();
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    dynamic sum = 0.0;
                    for (int i = 0; i < 3; i++)
                    {
                        dynamic l = lhs.elements[j, i];
                        dynamic r = rhs.elements[i, k];
                        sum += l * r;
                    }

                    result.elements[j, k] = (T)sum;
                }
            }
            return result;
        }
        public static Mat3<T> Identity()
        {
            dynamic zero = default(T);
            dynamic one = zero + 1;

            Mat3<T> result = new Mat3<T>();

            result.elements = new T[3, 3]{
                { (T)one,(T)zero,(T)zero },
                { (T)zero,(T)one,(T)zero },
                { (T)zero,(T)zero,(T)one }
            };

            return result;
        }

        public static Mat3<T> Scaling(T factor)
        {
            dynamic zero = default(T);
            dynamic dFactor = factor;

            Mat3<T> result = new Mat3<T>();

            result.elements = new T[3, 3]{
                { (T)factor,(T)zero,(T)zero },
                { (T)zero, (T)factor,(T)zero },
                { (T)zero,(T)zero, (T)factor }
            };

            return result;
        }
        public static Mat3<T> RotationZ(T theta)
        {
            dynamic dTheta = theta;
            dynamic sinTheta = Math.Sin(dTheta);
            dynamic cosTheta = Math.Cos(dTheta);

            dynamic zero = default(T);
            dynamic one = zero + 1;

            Mat3<T> result = new Mat3<T>();

            result.elements = new T[3, 3]{
                { (T)cosTheta, (T)sinTheta,  (T)zero },
                { (T)(-sinTheta), (T)cosTheta, (T)zero },
                { (T)zero,     (T)zero,  (T)one }
            };

            return result;
        }
        public static Mat3<T> RotationY(T theta)
        {
            dynamic dTheta = theta;
            dynamic sinTheta = Math.Sin(dTheta);
            dynamic cosTheta = Math.Cos(dTheta);

            dynamic zero = default(T);
            dynamic one = zero + 1;

            Mat3<T> result = new Mat3<T>();

            result.elements = new T[3, 3]{
                { (T)cosTheta, (T)zero, (T)(-sinTheta) },
                { (T)zero,   (T)one, (T)zero },
                { (T)sinTheta, (T)zero, (T)cosTheta }
            };

            return result;
        }
        public static Mat3<T> RotationX(T theta)
        {
            dynamic dTheta = theta;
            dynamic sinTheta = Math.Sin(dTheta);
            dynamic cosTheta = Math.Cos(dTheta);

            dynamic zero = default(T);
            dynamic one = zero + 1;

            Mat3<T> result = new Mat3<T>();

            result.elements = new T[3, 3]{
                { (T)one, (T)zero,   (T)zero },
                { (T)zero, (T)cosTheta, (T)sinTheta },
                { (T)zero, (T)(-sinTheta), (T)cosTheta }
            };

            return result;
        }

        public static Vec3<T> operator*( Vec3<T> lhs, Mat3<T> rhs )
        {
            dynamic lhs_d = lhs;
	        return new Vec3<T>(
		        lhs_d.x * rhs.elements[0,0] + lhs_d.y * rhs.elements[1,0] + lhs_d.z * rhs.elements[2,0],
                lhs_d.x * rhs.elements[0,1] + lhs_d.y * rhs.elements[1,1] + lhs_d.z * rhs.elements[2,1],
                lhs_d.x * rhs.elements[0,2] + lhs_d.y * rhs.elements[1,2] + lhs_d.z * rhs.elements[2,2]

            );
        }

        // [row, col]
        T[,] elements = new T[3,3];
    }
}
