using System;
using Random = Unity.Mathematics.Random;

public static class NoiseUtil
{
    static int FastFloor(float x)
    {
        return x > 0 ? (int)x : (int)x - 1;
    }

    static float dot(int[] a, float b1, float b2, float b3, float b4)
    {
        return a[0] * b1 + a[1] * b2 + a[2] * b3 + a[3] * b4;
    }

    // 4D raw Simplex noise
    static float RawNoise4d(float x, float y, float z, float w)
    {
        // The skewing and unskewing factors are hairy again for the 4D case
        float F4 = (float)((Math.Sqrt(5.0) - 1.0) / 4.0);
        float G4 = (float)((5.0 - Math.Sqrt(5.0)) / 20.0);
        float n0, n1, n2, n3, n4; // Noise contributions from the five corners

        // Skew the (x,y,z,w) space to determine which cell of 24 simplices we're in
        float s = (x + y + z + w) * F4; // Factor for 4D skewing
        int i = FastFloor(x + s);
        int j = FastFloor(y + s);
        int k = FastFloor(z + s);
        int l = FastFloor(w + s);
        float t = (i + j + k + l) * G4; // Factor for 4D unskewing
        float X0 = i - t; // Unskew the cell origin back to (x,y,z,w) space
        float Y0 = j - t;
        float Z0 = k - t;
        float W0 = l - t;

        float x0 = x - X0; // The x,y,z,w distances from the cell origin
        float y0 = y - Y0;
        float z0 = z - Z0;
        float w0 = w - W0;

        // For the 4D case, the simplex is a 4D shape I won't even try to describe.
        // To find out which of the 24 possible simplices we're in, we need to
        // determine the magnitude ordering of x0, y0, z0 and w0.
        // The method below is a good way of finding the ordering of x,y,z,w and
        // then find the correct traversal order for the simplex we're in.
        // First, six pair-wise comparisons are performed between each possible pair
        // of the four coordinates, and the results are used to add up binary bits
        // for an integer index.
        int c1 = (x0 > y0) ? 32 : 0;
        int c2 = (x0 > z0) ? 16 : 0;
        int c3 = (y0 > z0) ? 8 : 0;
        int c4 = (x0 > w0) ? 4 : 0;
        int c5 = (y0 > w0) ? 2 : 0;
        int c6 = (z0 > w0) ? 1 : 0;
        int c = c1 + c2 + c3 + c4 + c5 + c6;

        int i1, j1, k1, l1; // The integer offsets for the second simplex corner
        int i2, j2, k2, l2; // The integer offsets for the third simplex corner
        int i3, j3, k3, l3; // The integer offsets for the fourth simplex corner

        // simplex[c] is a 4-vector with the numbers 0, 1, 2 and 3 in some order.
        // Many values of c will never occur, since e.g. x>y>z>w makes x<z, y<w and x<w
        // impossible. Only the 24 indices which have non-zero entries make any sense.
        // We use a thresholding to set the coordinates in turn from the largest magnitude.
        // The number 3 in the "simplex" array is at the position of the largest coordinate.
        i1 = NoiseLookUpTable.Simplex[c][0] >= 3 ? 1 : 0;
        j1 = NoiseLookUpTable.Simplex[c][1] >= 3 ? 1 : 0;
        k1 = NoiseLookUpTable.Simplex[c][2] >= 3 ? 1 : 0;
        l1 = NoiseLookUpTable.Simplex[c][3] >= 3 ? 1 : 0;
        // The number 2 in the "simplex" array is at the second largest coordinate.
        i2 = NoiseLookUpTable.Simplex[c][0] >= 2 ? 1 : 0;
        j2 = NoiseLookUpTable.Simplex[c][1] >= 2 ? 1 : 0;
        k2 = NoiseLookUpTable.Simplex[c][2] >= 2 ? 1 : 0;
        l2 = NoiseLookUpTable.Simplex[c][3] >= 2 ? 1 : 0;
        // The number 1 in the "simplex" array is at the second smallest coordinate.
        i3 = NoiseLookUpTable.Simplex[c][0] >= 1 ? 1 : 0;
        j3 = NoiseLookUpTable.Simplex[c][1] >= 1 ? 1 : 0;
        k3 = NoiseLookUpTable.Simplex[c][2] >= 1 ? 1 : 0;
        l3 = NoiseLookUpTable.Simplex[c][3] >= 1 ? 1 : 0;
        // The fifth corner has all coordinate offsets = 1, so no need to look that up.

        float x1 = x0 - i1 + G4; // Offsets for second corner in (x,y,z,w) coords
        float y1 = y0 - j1 + G4;
        float z1 = z0 - k1 + G4;
        float w1 = w0 - l1 + G4;
        float x2 = (float)(x0 - i2 + 2.0 * G4); // Offsets for third corner in (x,y,z,w) coords
        float y2 = (float)(y0 - j2 + 2.0 * G4);
        float z2 = (float)(z0 - k2 + 2.0 * G4);
        float w2 = (float)(w0 - l2 + 2.0 * G4);
        float x3 = (float)(x0 - i3 + 3.0 * G4); // Offsets for fourth corner in (x,y,z,w) coords
        float y3 = (float)(y0 - j3 + 3.0 * G4);
        float z3 = (float)(z0 - k3 + 3.0 * G4);
        float w3 = (float)(w0 - l3 + 3.0 * G4);
        float x4 = (float)(x0 - 1.0 + 4.0 * G4); // Offsets for last corner in (x,y,z,w) coords
        float y4 = (float)(y0 - 1.0 + 4.0 * G4);
        float z4 = (float)(z0 - 1.0 + 4.0 * G4);
        float w4 = (float)(w0 - 1.0 + 4.0 * G4);

        // Work out the hashed gradient indices of the five simplex corners
        int ii = i & 255;
        int jj = j & 255;
        int kk = k & 255;
        int ll = l & 255;
        int gi0 = NoiseLookUpTable.Perm[ii + NoiseLookUpTable.Perm[jj + NoiseLookUpTable.Perm[kk + NoiseLookUpTable.Perm[ll]]]] % 32;
        int gi1 = NoiseLookUpTable.Perm[ii + i1 + NoiseLookUpTable.Perm[jj + j1 + NoiseLookUpTable.Perm[kk + k1 + NoiseLookUpTable.Perm[ll + l1]]]] % 32;
        int gi2 = NoiseLookUpTable.Perm[ii + i2 + NoiseLookUpTable.Perm[jj + j2 + NoiseLookUpTable.Perm[kk + k2 + NoiseLookUpTable.Perm[ll + l2]]]] % 32;
        int gi3 = NoiseLookUpTable.Perm[ii + i3 + NoiseLookUpTable.Perm[jj + j3 + NoiseLookUpTable.Perm[kk + k3 + NoiseLookUpTable.Perm[ll + l3]]]] % 32;
        int gi4 = NoiseLookUpTable.Perm[ii + 1 + NoiseLookUpTable.Perm[jj + 1 + NoiseLookUpTable.Perm[kk + 1 + NoiseLookUpTable.Perm[ll + 1]]]] % 32;

        // Calculate the contribution from the five corners
        float t0 = (float)(0.6 - x0 * x0 - y0 * y0 - z0 * z0 - w0 * w0);
        if (t0 < 0) n0 = 0.0f;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * dot(NoiseLookUpTable.Grad4[gi0], x0, y0, z0, w0);
        }

        float t1 = (float)(0.6 - x1 * x1 - y1 * y1 - z1 * z1 - w1 * w1);
        if (t1 < 0) n1 = 0.0f;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * dot(NoiseLookUpTable.Grad4[gi1], x1, y1, z1, w1);
        }

        float t2 = (float)(0.6 - x2 * x2 - y2 * y2 - z2 * z2 - w2 * w2);
        if (t2 < 0) n2 = 0.0f;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * dot(NoiseLookUpTable.Grad4[gi2], x2, y2, z2, w2);
        }

        float t3 = (float)(0.6 - x3 * x3 - y3 * y3 - z3 * z3 - w3 * w3);
        if (t3 < 0) n3 = 0.0f;
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * dot(NoiseLookUpTable.Grad4[gi3], x3, y3, z3, w3);
        }

        float t4 = (float)(0.6 - x4 * x4 - y4 * y4 - z4 * z4 - w4 * w4);
        if (t4 < 0) n4 = 0.0f;
        else
        {
            t4 *= t4;
            n4 = t4 * t4 * dot(NoiseLookUpTable.Grad4[gi4], x4, y4, z4, w4);
        }

        // Sum up and scale the result to cover the range [-1,1]
        return (float)(27.0 * (n0 + n1 + n2 + n3 + n4));
    }

    static float OctaveNoise4D(float octaves, float persistence, float scale, float x, float y, float z, float w)
    {
        float total = 0;
        float frequency = scale;
        float amplitude = 1;

        // We have to keep track of the largest possible amplitude,
        // because each octave adds more, and we need a value in [-1, 1].
        float maxAmplitude = 0;

        for (int i = 0; i < octaves; i++)
        {
            total += RawNoise4d(x * frequency, y * frequency, z * frequency, w * frequency) * amplitude;

            frequency *= 2;
            maxAmplitude += amplitude;
            amplitude *= persistence;
        }

        return total / maxAmplitude;
    }

    public static float ScaledOctaveNoise4D(float octaves, float persistence, float scale, float loBound, float hiBound, float x, float y, float z, float w)
    {
        return OctaveNoise4D(octaves, persistence, scale, x, y, z, w) * (hiBound - loBound) / 2 + (hiBound + loBound) / 2;
    }

    public static void CreateSlowNoise(float[,] map, uint seed)
    {
        Random randsource = new Random(seed);
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        float persistence = 0.25f;
        float noiseScale = 0.593f;
        float ka = 256 / randsource.NextFloat();
        float kb = seed * 567 % 256;
        float kc = (seed * seed) % 256;
        float kd = (567 - seed) % 256;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float fNX = x / (float)width;
                float fNY = y / (float)height;
                float fRdx = fNX * 2 * (float)Math.PI;
                float fRdy = fNY * 2 * (float)Math.PI; // 4 * (float)Math.PI;
                float fYSin = (float)Math.Sin(fRdy);
                float fRdsSin = 1.0f;
                float a = fRdsSin * (float)Math.Sin(fRdx);
                float b = fRdsSin * (float)Math.Cos(fRdx);
                float c = fRdsSin * (float)Math.Sin(fRdy);
                float d = fRdsSin * (float)Math.Cos(fRdy);

                float v = ScaledOctaveNoise4D(4.0f, persistence, 0.25f, 0.0f, 1.0f,
                    ka + a * noiseScale,
                    kb + b * noiseScale,
                    kc + c * noiseScale,
                    kd + d * noiseScale);

                map[y, x] = v;
            }
        }
    }

    public static void CreateSlowNoise(float[] map, uint width, uint height, uint seed)
    {
        Random randsource = new Random(seed);

        float persistence = 0.25f;
        float noiseScale = 0.593f;
        float ka = 256 / randsource.NextFloat();
        float kb = seed * 567 % 256;
        float kc = (seed * seed) % 256;
        float kd = (567 - seed) % 256;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float fNX = x / (float)width;
                float fNY = y / (float)height;
                float fRdx = fNX * 2 * (float)Math.PI;
                float fRdy = fNY * 4 * (float)Math.PI;
                float fYSin = (float)Math.Sin(fRdy);
                float fRdsSin = 1.0f;
                float a = fRdsSin * (float)Math.Sin(fRdx);
                float b = fRdsSin * (float)Math.Cos(fRdx);
                float c = fRdsSin * (float)Math.Sin(fRdy);
                float d = fRdsSin * (float)Math.Cos(fRdy);

                float v = ScaledOctaveNoise4D(4.0f, persistence, 0.25f, 0.0f, 1.0f,
                    ka + a * noiseScale,
                    kb + b * noiseScale,
                    kc + c * noiseScale,
                    kd + d * noiseScale);

                map[y * width + x] = v;
            }
        }
    }
}