using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using System.Text;

using DemoCheck.Tools;

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

        const int FRAMES_COUNT = 3;
        LimitedStack<int> frames = new LimitedStack<int>(FRAMES_COUNT);

        public override void Frame(int frameData)
        {
            frames.Push(frameData);

            if (frames.Count() < FRAMES_COUNT)
            {
                // Console.WriteLine($"Not enough frames for analyze! frames.Count()");
                return;
            }
            GetFrames();
        }

        public void GetFrames()
        {
            Console.WriteLine("\n\nStackData:");

            var frames_arr = frames.ToArray();
            var frames_arr_length = frames_arr.Length;
            /*
              // SOME Frame logic.
            for (int i = 0; i < frames_arr_length; i++)
            {
                Console.WriteLine("\tFrameData: " + frames_arr[i]);
                if (i > 0)
                {
                    var diffWithPrevFrame = Math.Abs(frames_arr[i - 1] - frames_arr[i]);
                    if (diffWithPrevFrame > 1)
                        Console.WriteLine($"\t !!!!BAD Prev! {diffWithPrevFrame}");
                }

                if (i < (frames_arr_length - 1))
                {
                    var diffWithNextFrame = Math.Abs(frames_arr[i + 1] - frames_arr[i]);
                    if (diffWithNextFrame > 1)
                        Console.WriteLine($"\t !!!!BAD Next! {diffWithNextFrame}");
                }
            }
            */
        }
    }
}