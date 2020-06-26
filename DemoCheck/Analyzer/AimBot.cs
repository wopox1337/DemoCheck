using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using System.Text;

using DemoCheck.Tools;
using DemoParser.Demo_stuff.GoldSource;
using System.Runtime.CompilerServices;

namespace DemoCheck.Analyzer
{
    class AimBot : Base
    {
        public AimBot()
        {
            Name = "AimBot analyze";
        }
        public override string StartAnalyze()
        {
            return "StartAnalyze()";
        }

        const int FRAMES_COUNT = 2;
        LimitedStack<GoldSource.FramesHren> frames = new LimitedStack<GoldSource.FramesHren>(FRAMES_COUNT);

        public override void Frame(GoldSource.FramesHren frameData)
        {
            if (frameData.Key.Type == GoldSource.DemoFrameType.ClientData)
                frames.Push(frameData);

            if (frames.Count() < FRAMES_COUNT)
            {
                // Console.WriteLine($"Not enough frames for analyze! frames.Count()");
                return;
            }
            Analyze();
        }

        public void Analyze()
        {
            var frames_arr = frames.ToArray();
            var frames_arr_length = frames_arr.Length;

            for (int i = 0; i < frames_arr_length; i++)
            {
                if (i > 0)
                {
                    var current_frame = (GoldSource.ClientDataFrame)frames_arr[i].Value;
                    var prev_frame = (GoldSource.ClientDataFrame)frames_arr[i - 1].Value;
                    var distance = GetDistance(current_frame.Viewangles.X, current_frame.Viewangles.Y, prev_frame.Viewangles.X, prev_frame.Viewangles.Y);

                    //if (distance > 0)
                    {
                        var str = $"{Name} => distance: ";
                        if (distance > 10)
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine(str + $"{distance}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

        }
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow(GetAngle(y2, y1), 2));
        }

        private static double GetAngle(double x1, double x2)
        {
            return Math.Atan2(Math.Sin(x1 - x2), Math.Cos(x1 - x2));
        }

    }
}