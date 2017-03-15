using System;
using UnityEngine;

/// <summary>
/// イージングライブラリ
/// 参考 http://easings.net/ja
/// </summary>
public class EasingUtility
{
    /// <summary>
    /// イージング関数デリゲート
    /// </summary>
    /// <param name="start">開始値</param>
    /// <param name="end">終了値</param>
    /// <param name="value">現在値</param>
    /// <returns>イージングが反映された値</returns>
    public delegate float EasingFunction(float start, float end, float value);

    /// <summary>
    /// イージングタイプ
    /// </summary>
    public enum EasingType
    {
        Linear = 0,
        Clerp,
        Spring,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine, 
        EaseOutSine,
        EaseInOutSine,
        EaseInExpo, 
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack, 
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic
    }

    /// <summary>
    /// イージング関数を返します
    /// </summary>
    /// <param name="type_">イージングタイプ</param>
    /// <returns></returns>
    public static EasingFunction GetEasingFunc(EasingType type_)
    {
        switch (type_)
        {
            case EasingType.Linear:
                return linear;
            case EasingType.Clerp:
                return clerp;
            case EasingType.Spring:
                return spring;
            case EasingType.EaseInQuad:
                return easeInQuad;
            case EasingType.EaseOutQuad:
                return easeOutQuad;
            case EasingType.EaseInOutQuad:
                return easeInOutQuad;
            case EasingType.EaseInCubic:
                return easeInCubic;
            case EasingType.EaseOutCubic:
                return easeOutCubic;
            case EasingType.EaseInOutCubic:
                return easeInOutCubic;
            case EasingType.EaseInQuart:
                return easeInQuart;
            case EasingType.EaseOutQuart:
                return easeOutQuart;
            case EasingType.EaseInOutQuart:
                return easeInOutQuart;
            case EasingType.EaseInQuint:
                return easeInQuint;
            case EasingType.EaseOutQuint:
                return easeOutQuint;
            case EasingType.EaseInOutQuint:
                return easeInOutQuint;
            case EasingType.EaseInSine:
                return easeInSine;
            case EasingType.EaseOutSine:
                return easeOutSine;
            case EasingType.EaseInOutSine:
                return easeInOutSine;
            case EasingType.EaseInExpo:
                return easeInExpo;
            case EasingType.EaseOutExpo:
                return easeOutExpo;
            case EasingType.EaseInOutExpo:
                return easeInOutExpo;
            case EasingType.EaseInCirc:
                return easeInCirc;
            case EasingType.EaseOutCirc:
                return easeOutCirc;
            case EasingType.EaseInOutCirc:
                return easeInOutCirc;
            case EasingType.EaseInBounce:
                return easeInBounce;
            case EasingType.EaseOutBounce:
                return easeOutBounce;
            case EasingType.EaseInOutBounce:
                return easeInOutBounce;
            case EasingType.EaseInBack:
                return easeInBack;
            case EasingType.EaseOutBack:
                return easeOutBack;
            case EasingType.EaseInOutBack:
                return easeInOutBack;
            case EasingType.EaseInElastic:
                return easeInElastic;
            case EasingType.EaseOutElastic:
                return easeOutElastic;
            case EasingType.EaseInOutElastic:
                return easeInOutElastic;
        }

        return linear;
    }

    #region Easing Curves
    static float linear(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value);
    }

    static float clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min)/2.0f);
        float retval = 0.0f;
        float diff = 0.0f;
        if ((end - start) < -half)
        {
            diff = ((max - start) + end)*value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start)*value;
            retval = start + diff;
        }
        else retval = start + (end - start)*value;
        return retval;
    }

    static float spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value*Mathf.PI*(0.2f + 2.5f*value*value*value))*Mathf.Pow(1f - value, 2.2f) + value)*
                (1f + (1.2f*(1f - value)));
        return start + (end - start)*value;
    }

    static float easeInQuad(float start, float end, float value)
    {
        end -= start;
        return end*value*value + start;
    }

    static float easeOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end*value*(value - 2) + start;
    }

    static float easeInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end/2*value*value + start;
        value--;
        return -end/2*(value*(value - 2) - 1) + start;
    }

    static float easeInCubic(float start, float end, float value)
    {
        end -= start;
        return end*value*value*value + start;
    }

    static float easeOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end*(value*value*value + 1) + start;
    }

    static float easeInOutCubic(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end/2*value*value*value + start;
        value -= 2;
        return end/2*(value*value*value + 2) + start;
    }

    static float easeInQuart(float start, float end, float value)
    {
        end -= start;
        return end*value*value*value*value + start;
    }

    static float easeOutQuart(float start, float end, float value)
    {
        value--;
        end -= start;
        return -end*(value*value*value*value - 1) + start;
    }

    static float easeInOutQuart(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end/2*value*value*value*value + start;
        value -= 2;
        return -end/2*(value*value*value*value - 2) + start;
    }

    static float easeInQuint(float start, float end, float value)
    {
        end -= start;
        return end*value*value*value*value*value + start;
    }

    static float easeOutQuint(float start, float end, float value)
    {
        value--;
        end -= start;
        return end*(value*value*value*value*value + 1) + start;
    }

    static float easeInOutQuint(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end/2*value*value*value*value*value + start;
        value -= 2;
        return end/2*(value*value*value*value*value + 2) + start;
    }

    static float easeInSine(float start, float end, float value)
    {
        end -= start;
        return -end*Mathf.Cos(value/1*(Mathf.PI/2)) + end + start;
    }

    static float easeOutSine(float start, float end, float value)
    {
        end -= start;
        return end*Mathf.Sin(value/1*(Mathf.PI/2)) + start;
    }

    static float easeInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end/2*(Mathf.Cos(Mathf.PI*value/1) - 1) + start;
    }

    static float easeInExpo(float start, float end, float value)
    {
        end -= start;
        return end*Mathf.Pow(2, 10*(value/1 - 1)) + start;
    }

    static float easeOutExpo(float start, float end, float value)
    {
        end -= start;
        return end*(-Mathf.Pow(2, -10*value/1) + 1) + start;
    }

    static float easeInOutExpo(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end/2*Mathf.Pow(2, 10*(value - 1)) + start;
        value--;
        return end/2*(-Mathf.Pow(2, -10*value) + 2) + start;
    }

    static float easeInCirc(float start, float end, float value)
    {
        end -= start;
        return -end*(Mathf.Sqrt(1 - value*value) - 1) + start;
    }

    static float easeOutCirc(float start, float end, float value)
    {
        value--;
        end -= start;
        return end*Mathf.Sqrt(1 - value*value) + start;
    }

    static float easeInOutCirc(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return -end/2*(Mathf.Sqrt(1 - value*value) - 1) + start;
        value -= 2;
        return end/2*(Mathf.Sqrt(1 - value*value) + 1) + start;
    }

    static float easeInBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        return end - easeOutBounce(0, end, d - value) + start;
    }

    static float easeOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < (1/2.75f))
        {
            return end*(7.5625f*value*value) + start;
        }
        else if (value < (2/2.75f))
        {
            value -= (1.5f/2.75f);
            return end*(7.5625f*(value)*value + .75f) + start;
        }
        else if (value < (2.5/2.75))
        {
            value -= (2.25f/2.75f);
            return end*(7.5625f*(value)*value + .9375f) + start;
        }
        else
        {
            value -= (2.625f/2.75f);
            return end*(7.5625f*(value)*value + .984375f) + start;
        }
    }

    static float easeInOutBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        if (value < d/2) return easeInBounce(0, end, value*2)*0.5f + start;
        else return easeOutBounce(0, end, value*2 - d)*0.5f + end*0.5f + start;
    }

    static float easeInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1;
        float s = 1.70158f;
        return end*(value)*value*((s + 1)*value - s) + start;
    }

    static float easeOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value = (value/1) - 1;
        return end*((value)*value*((s + 1)*value + s) + 1) + start;
    }

    static float easeInOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value /= .5f;
        if ((value) < 1)
        {
            s *= (1.525f);
            return end/2*(value*value*(((s) + 1)*value - s)) + start;
        }
        value -= 2;
        s *= (1.525f);
        return end/2*((value)*value*(((s) + 1)*value + s) + 2) + start;
    }

    static float punch(float amplitude, float value)
    {
        float s = 9;
        if (value == 0)
        {
            return 0;
        }
        if (value == 1)
        {
            return 0;
        }
        float period = 1*0.3f;
        s = period/(2*Mathf.PI)*Mathf.Asin(0);
        return (amplitude*Mathf.Pow(2, -10*value)*Mathf.Sin((value*1 - s)*(2*Mathf.PI)/period));
    }

    static float easeInElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d*.3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p/4;
        }
        else
        {
            s = p/(2*Mathf.PI)*Mathf.Asin(end/a);
        }

        return -(a*Mathf.Pow(2, 10*(value -= 1))*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p)) + start;
    }

    static float easeOutElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d*.3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p/4;
        }
        else
        {
            s = p/(2*Mathf.PI)*Mathf.Asin(end/a);
        }

        return (a*Mathf.Pow(2, -10*value)*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p) + end + start);
    }

    static float easeInOutElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d*.3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d/2) == 2) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p/4;
        }
        else
        {
            s = p/(2*Mathf.PI)*Mathf.Asin(end/a);
        }

        if (value < 1) return -0.5f*(a*Mathf.Pow(2, 10*(value -= 1))*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p)) + start;
        return a*Mathf.Pow(2, -10*(value -= 1))*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p)*0.5f + end + start;
    }

    #endregion
}

