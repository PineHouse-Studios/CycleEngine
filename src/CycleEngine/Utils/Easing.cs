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
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                float s = 1.70158f;
                return c * (t /= d) * t * ((s + 1) * t - s) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="s">Overshoot amount; higher values overshoot more.</param>
            [Obsolete("Not supported by CES")]
            public static float In(float t, float b, float e, float d, float s)
            {
                // Delta Value
                float c = e - b;
                return c * (t /= d) * t * ((s + 1) * t - s) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                float s = 1.70158f;
                return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
            }
            
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="s">Overshoot amount; higher values overshoot more.</param>
            [Obsolete("Not supported by CES")]
            public static float Out(float t, float b, float e, float d, float s)
            {
                // Delta Value
                float c = e - b;
                return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                float s = 1.70158f;
                if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
                return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="s">Overshoot amount; higher values overshoot more.</param>
            [Obsolete("Not supported by CES")]
            public static float InOut(float t, float b, float e, float d, float s)
            {
                // Delta Value
                float c = e - b;
                if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
                return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
            }
        }

        public static class Bounce
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c - Out(d - t, 0, c, d) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
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
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if (t < d / 2) return In(t * 2, 0, c, d) * .5f + b;
                else return Out(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
            }
        }

        public static class Circ
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return -c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if ((t /= d / 2) < 1) return -c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b;
                return c / 2 * ((float)Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
            }
        }

        public static class Cubic
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * (t /= d) * t * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * ((t = t / d - 1) * t * t + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t + 2) + b;
            }
        }

        public static class Elastic
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                float p = d * .3f;
                float a = c;
                float s = p / 4;
                return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="a">Amplitude of the elastic oscillation.</param>
            /// <param name="p">Period of the elastic oscillation.</param>
            [Obsolete("Not supported by CES")]
            public static float In(float t, float b, float e, float d, float a, float p)
            {
                // Delta Value
                float c = e - b;
                float s;
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                if (a < Math.Abs(c)) { a = c; s = p / 4; }
                else { s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a); }
                return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                float p = d * .3f;
                float a = c;
                float s = p / 4;
                return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) + c + b);
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="a">Amplitude of the elastic oscillation.</param>
            /// <param name="p">Period of the elastic oscillation.</param>
            [Obsolete("Not supported by CES")]
            public static float Out(float t, float b, float e, float d, float a, float p)
            {
                // Delta Value
                float c = e - b;
                float s;
                if (t == 0) return b; if ((t /= d) == 1) return b + c;
                if (a < Math.Abs(c)) { a = c; s = p / 4; }
                else { s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a); }
                return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) + c + b);
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if (t == 0) return b; if ((t /= d / 2) == 2) return b + c;
                float p = d * (.3f * 1.5f);
                float a = c;
                float s = p / 4;
                if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
                return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) * .5f + c + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            /// <param name="a">Amplitude of the elastic oscillation.</param>
            /// <param name="p">Period of the elastic oscillation.</param>
            [Obsolete("Not supported by CES")]
            public static float InOut(float t, float b, float e, float d, float a, float p)
            {
                // Delta Value
                float c = e - b;
                float s;
                if (t == 0) return b; if ((t /= d / 2) == 2) return b + c;
                if (a < Math.Abs(c)) { a = c; s = p / 4; }
                else { s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a); }
                if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
                return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) * .5f + c + b;
            }
        }

        public static class Expo
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return (t == 0) ? b : c * (float)Math.Pow(2, 10 * (t / d - 1)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return (t == d) ? b + c : c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if (t == 0) return b;
                if (t == d) return b + c;
                if ((t /= d / 2) < 1) return c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
                return c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2) + b;
            }
        }

        public static class Linear
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float None(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * t / d + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * t / d + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * t / d + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * t / d + b;
            }
        }

        public static class Quad
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * (t /= d) * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return -c * (t /= d) * (t - 2) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if ((t /= d / 2) < 1) return c / 2 * t * t + b;
                return -c / 2 * ((--t) * (t - 2) - 1) + b;
            }
        }

        public static class Quart
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * (t /= d) * t * t * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return -c * ((t = t / d - 1) * t * t * t - 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
                return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
            }
        }

        public static class Quint
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * (t /= d) * t * t * t * t + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
            }
        }

        public static class Sine
        {
            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float In(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return -c * (float)Math.Cos(t / d * (Math.PI / 2)) + c + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float Out(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
            }

            /// <param name="t">Current time (or position) of the tween.</param>
            /// <param name="b">Beginning value.</param>
            /// <param name="e">Ending value.</param>
            /// <param name="d">Total duration of the tween.</param>
            public static float InOut(float t, float b, float e, float d)
            {
                // Delta Value
                float c = e - b;
                return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
            }
        }
        
        private static readonly Dictionary<string, EaseType> Eases = new Dictionary<string, EaseType>
        {
            { "linear", EaseType.Linear },
            { "isine", EaseType.InSine },
            { "osine", EaseType.OutSine },
            { "iosine", EaseType.InOutSine },
            { "iquad", EaseType.InQuad },
            { "oquad", EaseType.OutQuad },
            { "ioquad", EaseType.InOutQuad },
            { "icubic", EaseType.InCubic },
            { "ocubic", EaseType.OutCubic },
            { "iocubic", EaseType.InOutCubic },
            { "iquart", EaseType.InQuart },
            { "oquart", EaseType.OutQuart },
            { "ioquart", EaseType.InOutQuart },
            { "iquint", EaseType.InQuint },
            { "oquint", EaseType.OutQuint },
            { "ioquint", EaseType.InOutQuint },
            { "iexpo", EaseType.InExpo },
            { "oexpo", EaseType.OutExpo },
            { "ioexpo", EaseType.InOutExpo },
            { "icirc", EaseType.InCirc },
            { "ocirc", EaseType.OutCirc },
            { "iocirc", EaseType.InOutCirc },
            { "iback", EaseType.InBack },
            { "oback", EaseType.OutBack },
            { "ioback", EaseType.InOutBack },
            { "ielastic", EaseType.InElastic },
            { "oelastic", EaseType.OutElastic },
            { "ioelastic", EaseType.InOutElastic },
            { "ibounce", EaseType.InBounce },
            { "obounce", EaseType.OutBounce },
            { "iobounce", EaseType.InOutBounce }
        };

        public static EaseType GetType(string key)
        {
            return Eases.TryGetValue(key, out EaseType ease) ? ease : EaseType.Undefine;
        }
    }
    
    public enum EaseType
    {
        Linear,
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
}