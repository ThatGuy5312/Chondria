using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chondria.Core;

public static class Time
{
    public static float GameTime { get; private set; }
    public static float DeltaTime { get; private set; }

    public static float TimeScale { get; set; } = 1f;

    public static float UnscaledGameTime { get; private set; }
    public static float UnscaledDeltaTime { get; private set; }

    public static float FrameCount { get; private set; }


    internal static void Update(double deltaTime)
    {
        UnscaledDeltaTime = (float)deltaTime;
        DeltaTime = UnscaledDeltaTime * TimeScale;
        UnscaledGameTime += UnscaledDeltaTime;
        GameTime += DeltaTime;
        FrameCount++;
    }
}
