using ANoiseGPU;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Util.Noise.Samples.Example
{
    public class ExampleBase : MonoBehaviour
    {
        public ComputeShader Shader;
        [PreviewField(100)]
        public Texture2D image;

        [Button]
        public void Test()
        {
            MBase.Shader = Shader;
            var Width = 512;
            MFractal fractal = new MFractal()
                .SetSeed(2331)
                .SetOctave(10)
                .SetFrequency(2f)
                .SetLacunarity(2)
                .SetGain(.5f)
                .SetNoiseType(NoiseType.PERLIN)
                .SetFractalType(FractalType.FBM)
                .Build();
            MAutoCorrect autoCorrect1 = new MAutoCorrect()
                .SetSource(fractal)
                .SetResolution(Width)
                .SetRange(0.2f, .8f)
                .Build();
            ComputeBuffer buffer = autoCorrect1.Get2(Width);
            var values = new float[Width * Width];
            buffer.GetData(values);
            MBase.Dispose();
            buffer.Dispose();

            image = new Texture2D(Width, Width);
            for (int y = 0, i = 0; y < Width; y++)
            {
                for (int x = 0; x < Width; x++, i++)
                {
                    float v = values[i];
                    Color c = new Color(v, v, v);
                    image.SetPixel(x, y, c);
                }
            }
            image.Apply();
        }
    }
}