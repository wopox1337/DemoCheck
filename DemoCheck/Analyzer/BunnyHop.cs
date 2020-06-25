using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using System.Text;
using System.Drawing;

using DemoCheck.Tools;
using DemoParser.Demo_stuff.GoldSource;

namespace DemoCheck.Analyzer
{
    class BunnyHop : Base
    {
        public BunnyHop()
        {
            Name = "BunnyHop analyze";
        }

        public override string StartAnalyze()
        {
            return "StartAnalyze()";
        }

        const int FRAMES_COUNT = 2;
        LimitedStack<KeyValuePair<GoldSource.DemoFrame, GoldSource.IFrame>> frames = new LimitedStack<KeyValuePair<GoldSource.DemoFrame, GoldSource.IFrame>>(FRAMES_COUNT);

        public override void Frame(KeyValuePair<GoldSource.DemoFrame, GoldSource.IFrame> frameData)
        {
            if(frameData.Key.Type == GoldSource.DemoFrameType.ClientData)
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
            Console.WriteLine("\n\nStackData:");

            var frames_arr = frames.ToArray();
            var frames_arr_length = frames_arr.Length;
            
            for (int i = 0; i < frames_arr_length; i++)
            {
                
            }
        }
    }
}
