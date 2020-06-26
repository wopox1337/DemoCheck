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
        LimitedStack<GoldSource.FramesHren> frames = new LimitedStack<GoldSource.FramesHren>(30);

        // Frames on ground before Jump (or Duck)
        private int FOG;

        public override void Frame(GoldSource.FramesHren frameData)
        {
            if (frameData.Key.Type == GoldSource.DemoFrameType.NetMsg)
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
                if (i < 1)
                    continue;

                var prev_frame = (GoldSource.NetMsgFrame)frames_arr[i - 1].Value;
                var current_frame = (GoldSource.NetMsgFrame)frames_arr[i].Value;

                if (current_frame.RParms.Onground == 1)
                {
                    FOG = Math.Min(++FOG, 50);
                }
                else FOG = 0;


                Check_BhopHack_Type1(prev_frame, current_frame);
            }
        }

        private void Check_BhopHack_Type1(GoldSource.NetMsgFrame prev_frame, GoldSource.NetMsgFrame current_frame)
        {
           // Console.WriteLine($"{Name}=>Check_BhopHack_Type1 Analize:");

            int jmp_button = (int)GoldSrc_Constants.ButtonsType.IN_JUMP;

            var prev_jumped = (prev_frame.UCmd.Buttons & jmp_button) == jmp_button;
            var current_jumped = (current_frame.UCmd.Buttons & jmp_button) == jmp_button;
           
            var prev_onground = prev_frame.RParms.Onground == 1;
            var current_onground = current_frame.RParms.Onground == 1;

            var just_in_ground = !prev_onground && current_onground;
            var just_in_air = prev_onground && !current_onground;

            var just_jumped = !prev_jumped && current_jumped;
            var just_notjumped = prev_jumped && !current_jumped;

            if (prev_onground && just_jumped) Console.WriteLine($"JustJumped: FOG:{FOG}");


            if (just_in_air && just_notjumped)
                Console.WriteLine($"{Name} => Bhop Warn Type #1");

            if (just_in_ground && just_jumped)
                Console.WriteLine($"{Name} => Bhop Warn Type #2");
        }
    }
}
