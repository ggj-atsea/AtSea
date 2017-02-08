using UnityEngine;
using System;
using System.Collections;

public static class MathHelper
{
    public static Color Interp(Color a, Color b, float percent) {
        return new Color(a.r + (b.r - a.r) * percent,
                         a.g + (b.g - a.g) * percent,
                         a.b + (b.b - a.b) * percent,
                         a.a + (b.a - a.a) * percent);

    }

    public static float Interp(float a, float b, float percent) {
        return a + (b - a) * percent;
    }
    public static float Wrap(float num, float min, float max) {
        float range = max - min;
        while (num > max)   num -= range;
        while (num < min)   num += range;
        return num;
    }
}

