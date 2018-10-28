using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EasingFuncs
{
    public static float Reverse(Func<float, float> function, float value)
    {
        return 1 - function(1 - value);
    }
    public static float ToInOut(Func<float, float> function, float value)
    {
        return .5f * (value < .5f ? function(2 * value) : (2 - function(2 - 2 * value)));
    }

    public static Func<float, float> Step = x => x >= 1 ? 1 : 0;
    public static Func<float, float> Linear = x => x;

    public static Func<float, float> QuadIn = x => x * x;
    public static Func<float, float> QuadOut = x => Reverse(QuadIn, x);
    public static Func<float, float> QuadInOut = x => ToInOut(QuadIn, x);
    public static Func<float, float> CubicIn = x => x * x * x;
    public static Func<float, float> CubicOut = x => Reverse(CubicIn, x);
    public static Func<float, float> CubicInOut = x => ToInOut(CubicIn, x);
    public static Func<float, float> QuartIn = x => x * x * x * x;
    public static Func<float, float> QuartOut = x => Reverse(QuartIn, x);
    public static Func<float, float> QuartInOut = x => ToInOut(QuartIn, x);
    public static Func<float, float> QuintIn = x => x * x * x * x * x;
    public static Func<float, float> QuintOut = x => Reverse(QuintIn, x);
    public static Func<float, float> QuintInOut = x => ToInOut(QuintIn, x);

    public static Func<float, float> SineIn = x => 1 - Mathf.Cos(x * Mathf.PI / 2);
    public static Func<float, float> SineOut = x => Reverse(SineIn, x);
    public static Func<float, float> SineInOut = x => ToInOut(SineIn, x);

    public static Func<float, float> ExpoIn = x => Mathf.Pow(2, 10 * (x - 1));
    public static Func<float, float> ExpoOut = x => Reverse(ExpoIn, x);
    public static Func<float, float> ExpoInOut = x => ToInOut(ExpoIn, x);

    public static Func<float, float> CircIn = x => 1 - Mathf.Sqrt(1 - x * x);
    public static Func<float, float> CircOut = x => Reverse(CircIn, x);
    public static Func<float, float> CircInOut = x => ToInOut(CircIn, x);

    public static Func<float, float> BackIn = x => x * x * ((1.70158f + 1) * x - 1.70158f);
    public static Func<float, float> BackOut = x => Reverse(BackIn, x);
    public static Func<float, float> BackInOut = x => ToInOut((y) => y * y * ((1.70158f * 1.525f + 1f) * y - 1.70158f * 1.525f), x);

    public static Func<float, float> BounceIn = x => Reverse(BounceOut, x);
    public static Func<float, float> BounceOut = x => x < 1f / 2.75f ? 7.5625f * x * x : x < 2f / 2.75f ? 7.5625f * (x -= (1.5f / 2.75f)) * x + .75f : x < 2.5f / 2.75f ? 7.5625f * (x -= (2.25f / 2.75f)) * x + .9375f : 7.5625f * (x -= (2.625f / 2.75f)) * x + .984375f;
    public static Func<float, float> BounceInOut = x => ToInOut(BounceIn, x);

    public static Func<float, float> ElasticIn = x => Reverse(ElasticOut, x);
    public static Func<float, float> ElasticOut = x => Mathf.Pow(2, -10 * x) * Mathf.Sin((x - 0.075f) * (2 * Mathf.PI) / .3f) + 1;
    public static Func<float, float> ElasticOutHalf = x => Mathf.Pow(2, -10 * x) * Mathf.Sin((0.5f * x - 0.075f) * (2f * Mathf.PI) / .3f) + 1;
    public static Func<float, float> ElasticOutQuarter = x => Mathf.Pow(2, -10 * x) * Mathf.Sin((0.25f * x - 0.075f) * (2 * Mathf.PI) / .3f) + 1;
    public static Func<float, float> ElasticInOut = x => ToInOut(ElasticIn, x);

}
