using System.Collections.Generic;

namespace CycleEngine.Utils
{
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