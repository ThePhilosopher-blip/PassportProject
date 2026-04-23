namespace PassportMD.Services;

public static class SoundGenerator
{
    private const int SampleRate = 44100;

    private static byte[] ToWav(float[] samples)
    {
        int dataSize = samples.Length * 2;
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write("RIFF".ToCharArray());
        w.Write(36 + dataSize);
        w.Write("WAVE".ToCharArray());

        w.Write("fmt ".ToCharArray());
        w.Write(16);
        w.Write((short)1);
        w.Write((short)1);
        w.Write(SampleRate);
        w.Write(SampleRate * 2);
        w.Write((short)2);
        w.Write((short)16);

        w.Write("data".ToCharArray());
        w.Write(dataSize);

        foreach (float s in samples)
            w.Write((short)(Math.Clamp(s, -1f, 1f) * 32767));

        return ms.ToArray();
    }

    private static void AddTone(float[] buf, int start, int len,
                                 float freq, float vol, float attack, float decay)
    {
        for (int i = 0; i < len && start + i < buf.Length; i++)
        {
            float t = (float)i / SampleRate;
            float env = 1f;

            float aSamples = attack * SampleRate;
            if (i < aSamples) env = i / aSamples;

            float dSamples = decay * SampleRate;
            float dStart = len - dSamples;
            if (i > dStart) env *= 1f - (i - dStart) / dSamples;

            buf[start + i] += (float)(Math.Sin(2 * Math.PI * freq * t) * vol * env);
        }
    }

    public static byte[] CreateTapSound()
    {
        int n = (int)(SampleRate * 0.06f);
        float[] buf = new float[n];
        AddTone(buf, 0, n, 880, 0.3f, 0.003f, 0.05f);
        AddTone(buf, 0, n, 1320, 0.12f, 0.002f, 0.04f);
        return ToWav(buf);
    }

    public static byte[] CreateSuccessChime()
    {
        int n = (int)(SampleRate * 0.7f);
        float[] buf = new float[n];
        int note = (int)(SampleRate * 0.2f);
        AddTone(buf, 0, note, 523.25f, 0.25f, 0.01f, 0.15f);
        AddTone(buf, (int)(SampleRate * 0.13f), note, 659.25f, 0.25f, 0.01f, 0.15f);
        AddTone(buf, (int)(SampleRate * 0.26f), (int)(SampleRate * 0.4f), 783.99f, 0.3f, 0.01f, 0.35f);
        AddTone(buf, (int)(SampleRate * 0.26f), (int)(SampleRate * 0.35f), 1567.98f, 0.08f, 0.01f, 0.3f);
        return ToWav(buf);
    }

    public static byte[] CreateWarningTone()
    {
        int n = (int)(SampleRate * 0.4f);
        float[] buf = new float[n];
        int note = (int)(SampleRate * 0.18f);
        AddTone(buf, 0, note, 659.25f, 0.2f, 0.01f, 0.1f);
        AddTone(buf, (int)(SampleRate * 0.16f), (int)(SampleRate * 0.24f), 523.25f, 0.25f, 0.01f, 0.18f);
        return ToWav(buf);
    }

    public static byte[] CreateSaveSound()
    {
        int n = (int)(SampleRate * 0.25f);
        float[] buf = new float[n];
        double phase = 0;
        for (int i = 0; i < n; i++)
        {
            float p = (float)i / n;
            float freq = 400 + p * 600;
            phase += 2 * Math.PI * freq / SampleRate;
            buf[i] = (float)(Math.Sin(phase) * 0.2f * Math.Sin(p * Math.PI));
        }
        return ToWav(buf);
    }

    public static byte[] CreateDeleteSound()
    {
        int n = (int)(SampleRate * 0.3f);
        float[] buf = new float[n];
        double phase = 0;
        for (int i = 0; i < n; i++)
        {
            float p = (float)i / n;
            float freq = 700 - p * 400;
            phase += 2 * Math.PI * freq / SampleRate;
            buf[i] = (float)(Math.Sin(phase) * 0.18f * (1f - p));
        }
        return ToWav(buf);
    }

    public static byte[] CreateAmbientLoop()
    {
        int n = (int)(SampleRate * 16f);
        float[] buf = new float[n];

        float[] c1 = { 130.81f, 164.81f, 196f, 261.63f };
        float[] v1 = { 0.05f, 0.035f, 0.035f, 0.025f };
        float[] c2 = { 220f, 261.63f, 329.63f };
        float[] v2 = { 0.025f, 0.018f, 0.018f };

        for (int i = 0; i < n; i++)
        {
            float t = (float)i / SampleRate;
            float lp = (float)i / n;

            float env = 1f;
            if (lp < 0.06f) env = lp / 0.06f;
            else if (lp > 0.94f) env = (1f - lp) / 0.06f;

            for (int f = 0; f < c1.Length; f++)
            {
                float lfo = 1f + 0.003f * (float)Math.Sin(2 * Math.PI * (0.1f + f * 0.03f) * t);
                buf[i] += (float)(Math.Sin(2 * Math.PI * c1[f] * lfo * t) * v1[f] * env);
            }

            float mix = 0.5f + 0.5f * (float)Math.Sin(2 * Math.PI * t / 16f * 2);
            for (int f = 0; f < c2.Length; f++)
            {
                float lfo = 1f + 0.003f * (float)Math.Sin(2 * Math.PI * 0.08f * t);
                buf[i] += (float)(Math.Sin(2 * Math.PI * c2[f] * lfo * t) * v2[f] * mix * env);
            }
        }
        return ToWav(buf);
    }
}
