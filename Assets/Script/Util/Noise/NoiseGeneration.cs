using ANoiseGPU;
using UnityEditor;
using UnityEngine;

namespace Util.Noise
{
    public static class NoiseGeneration
    {
        public static ComputeShader Shader = AssetDatabase.LoadAssetAtPath<ComputeShader>("Assets/Util/Noise/Runtime/Shaders/ANoiseMain.compute");
        public static float[] GenNoiseData(int width,int octave,float freq,
            float minVal = 0.0f, float maxVal = 1f,
            float lacunarity = 2f,float gain = .5f,
            NoiseType noiseType = NoiseType.PERLIN,
            FractalType fractalType = FractalType.FBM,int seed = 2331)
        {
            MBase.Shader = Shader;
            MFractal fractal = new MFractal()
                .SetSeed(seed)
                .SetOctave(octave)
                .SetFrequency(freq)
                .SetLacunarity(lacunarity)
                .SetGain(gain)
                .SetNoiseType(noiseType)
                .SetFractalType(fractalType)
                .Build();
            MAutoCorrect autoCorrect1 = new MAutoCorrect()
                .SetSource(fractal)
                .SetResolution(width)
                .SetRange(minVal, maxVal)
                .Build();
            ComputeBuffer buffer = autoCorrect1.Get2(width);
            var values = new float[width * width];
            buffer.GetData(values);
            MBase.Dispose();
            buffer.Dispose();
            return values;
        }
    }
}