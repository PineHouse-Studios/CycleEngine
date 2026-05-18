// TERMS OF USE - EASING EQUATIONS
// 
// Open source under the BSD License. 
// 
// Copyright © 2001 Robert Penner
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;

namespace CycleEngine.Utils
{
    public static class Easing
    {
        public static class Back
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                float s = 1.70158f;
                return c * (t /= d) * t * ((s + 1) * t - s) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="s">Overshoot amount; higher values overshoot more.</param>
            public static float In(float t, float b, float c, float d, float s)
            {
                return c * (t /= d) * t * ((s + 1) * t - s) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                float s = 1.70158f;
                return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
            }
            
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="s">Overshoot amount; higher values overshoot more.</param>
            public static float Out(float t, float b, float c, float d, float s)
            {
                return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                float s = 1.70158f;
                if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
                return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="s">Overshoot amount; higher values overshoot more.</param>
            public static float InOut(float t, float b, float c, float d, float s)
            {
                if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
                return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
            }
        }

        public class Bounce
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return c - Out(d - t, 0, c, d) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                if ((t /= d) < (1 / 2.75f))
                {
                    return c * (7.5625f * t * t) + b;
                }
                else if (t < (2 / 2.75f))
                {
                    return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
                }
                else if (t < (2.5 / 2.75))
                {
                    return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
                }
                else {
                    return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
                }
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if (t < d / 2) return In(t * 2, 0, c, d) * .5f + b;
                else return Out(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
            }
        }

        public class Circ
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return -c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if ((t /= d / 2) < 1) return -c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b;
                return c / 2 * ((float)Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
            }
        }

        public class Cubic
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return c * (t /= d) * t * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return c * ((t = t / d - 1) * t * t + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t + 2) + b;
            }
        }

        public class Elastic
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                float p = d * .3f;
                float a = c;
                float s = p / 4;
                return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="a">Amplitude of the elastic oscillation.</param>
            /// <param name="p">Period of the elastic oscillation.</param>
            public static float In(float t, float b, float c, float d, float a, float p)
            {
                float s;
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                if (a < Math.Abs(c)) { a = c; s = p / 4; }
                else { s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a); }
                return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                float p = d * .3f;
                float a = c;
                float s = p / 4;
                return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) + c + b);
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="a">Amplitude of the elastic oscillation.</param>
            /// <param name="p">Period of the elastic oscillation.</param>
            public static float Out(float t, float b, float c, float d, float a, float p)
            {
                float s;
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                if (a < Math.Abs(c)) { a = c; s = p / 4; }
                else { s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a); }
                return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) + c + b);
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if (t == 0) return b; if ((t /= d / 2) == 2) return b + c;
                float p = d * (.3f * 1.5f);
                float a = c;
                float s = p / 4;
                if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
                return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) * .5f + c + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="a">Amplitude of the elastic oscillation.</param>
            /// <param name="p">Period of the elastic oscillation.</param>
            public static float InOut(float t, float b, float c, float d, float a, float p)
            {
                float s;
                if (t == 0) return b; if ((t /= d / 2) == 2) return b + c;
                if (a < Math.Abs(c)) { a = c; s = p / 4; }
                else { s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a); }
                if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
                return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) * .5f + c + b;
            }
        }

        public class Expo
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return (t == 0) ? b : c * (float)Math.Pow(2, 10 * (t / d - 1)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return (t == d) ? b + c : c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if (t == 0) return b;
                if (t == d) return b + c;
                if ((t /= d / 2) < 1) return c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
                return c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2) + b;
            }
        }

        public class Linear
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float None(float t, float b, float c, float d)
            {
                return c * t / d + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return c * t / d + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return c * t / d + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                return c * t / d + b;
            }
        }

        public class Quad
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return c * (t /= d) * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return -c * (t /= d) * (t - 2) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t + b;
                return -c / 2 * ((--t) * (t - 2) - 1) + b;
            }
        }

        public class Quart
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return c * (t /= d) * t * t * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return -c * ((t = t / d - 1) * t * t * t - 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
                return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
            }
        }

        public class Quint
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return c * (t /= d) * t * t * t * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
            }
        }

        public class Sine
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float c, float d)
            {
                return -c * (float)Math.Cos(t / d * (Math.PI / 2)) + c + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float c, float d)
            {
                return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="c">Change between the beginning and destination value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float c, float d)
            {
                return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
            }
        }
    }
    
    public enum Ease
    {
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce,
        Undefine
    }

    public static class EaseFunctions
    {
        public static readonly Dictionary<string, Ease> Eases = new Dictionary<string, Ease>
        {
            { "isine", Ease.InSine },
            { "osine", Ease.OutSine },
            { "iosine", Ease.InOutSine },
            { "iquad", Ease.InQuad },
            { "oquad", Ease.OutQuad },
            { "ioquad", Ease.InOutQuad },
            { "icubic", Ease.InCubic },
            { "ocubic", Ease.OutCubic },
            { "iocubic", Ease.InOutCubic },
            { "iquart", Ease.InQuart },
            { "oquart", Ease.OutQuart },
            { "ioquart", Ease.InOutQuart },
            { "iquint", Ease.InQuint },
            { "oquint", Ease.OutQuint },
            { "ioquint", Ease.InOutQuint },
            { "iexpo", Ease.InExpo },
            { "oexpo", Ease.OutExpo },
            { "ioexpo", Ease.InOutExpo },
            { "icirc", Ease.InCirc },
            { "ocirc", Ease.OutCirc },
            { "iocirc", Ease.InOutCirc },
            { "iback", Ease.InBack },
            { "oback", Ease.OutBack },
            { "ioback", Ease.InOutBack },
            { "ielastic", Ease.InElastic },
            { "oelastic", Ease.OutElastic },
            { "ioelastic", Ease.InOutElastic },
            { "ibounce", Ease.InBounce },
            { "obounce", Ease.OutBounce },
            { "iobounce", Ease.InOutBounce }
        };
    }
}