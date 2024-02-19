using ANoiseGPU;

public class ExampleTriangle : Example
{
    public float Period = 0.5f;
    public float Offset = 0.5f;

    protected override void Generate()
    {
        base.Generate();

        ModuleRun(() =>
        {
            MFractal fractal = new MFractal()
             .SetSeed(NoiseSet.Seed)
             .SetOctave(NoiseSet.Octave)
             .SetFrequency(NoiseSet.Frequency)
             .SetLacunarity(NoiseSet.Lacunarity)
             .SetGain(NoiseSet.Gain)
             .SetNoiseType(NoiseSet.NType)
             .SetFractalType(NoiseSet.FType)
             .Build();
            MAutoCorrect autoCorrect = new MAutoCorrect()
            .SetResolution(Width)
            .SetSource(fractal)
            .SetRange(0, 1)
            .Build();
            MTriangle triangle = new MTriangle()
            .SetSource(autoCorrect)
            .SetPeriod(Period)
            .SetOffset(Offset)
            .Build();

            Complete(triangle);
        });

        DrawImage();
    }
}
