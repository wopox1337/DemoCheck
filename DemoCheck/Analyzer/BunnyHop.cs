using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using System.Text;
using System.Drawing;

using DemoCheck.Tools;
using DemoParser.Demo_stuff.GoldSource;
using System.Runtime.InteropServices.ComTypes;
using System.Net;

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

        const int FRAMES_COUNT = 5;
        LimitedStack<GoldSource.FramesHren> frames = new LimitedStack<GoldSource.FramesHren>(FRAMES_COUNT);

        public override void Frame(GoldSource.FramesHren frameData)
        {
            if (frameData.Key.Type != GoldSource.DemoFrameType.NetMsg)
                return;
            
            frames.Push(frameData);
            //prev_frame = frame;
            //frame = (GoldSource.NetMsgFrame)frameData.Value;
            //Check_BhopHack_Type1(prev_frame, frame);

            if (frames.Count() < FRAMES_COUNT)
            {
                // Console.WriteLine($"Not enough frames for analyze! frames.Count()");
                return;
            }
            Analyze();
        }

        private float current_frame_time;

        public void Analyze()
        {
            var frames_arr = frames.ToArray();
            //var frames_arr_length = frames_arr.Length;
            //Console.WriteLine($"0:{frames_arr[0].Key.FrameIndex}, 1:{frames_arr[1].Key.FrameIndex}, 2:{frames_arr[2].Key.FrameIndex}, 3:{frames_arr[3].Key.FrameIndex}");
            

            var current_frame = (GoldSource.NetMsgFrame)frames_arr[0].Value;
            var prev_frame = (GoldSource.NetMsgFrame)frames_arr[1].Value;

            var current_onground = (current_frame.RParms.Onground == 1);


            current_frame_time = frames_arr[0].Key.Time;


            Check_BhopHack_Type1(prev_frame, current_frame);
            CalculateFOG((current_frame.RParms.Onground == 1));             
        }

        private int jump_count;

        private void Check_BhopHack_Type1(GoldSource.NetMsgFrame prev_frame, GoldSource.NetMsgFrame current_frame)
        {
           // Console.WriteLine($"{Name}=>Check_BhopHack_Type1 Analize:");

            int jmp_button = (int)GoldSrc_Constants.ButtonsType.IN_JUMP;

            var prev_jumped = (prev_frame.UCmd.Buttons & jmp_button) == jmp_button;
            var current_jumped = (current_frame.UCmd.Buttons & jmp_button) == jmp_button;
           
            var prev_onground = prev_frame.RParms.Onground == 1;
            var current_onground = current_frame.RParms.Onground == 1;

            var just_in_ground = !prev_onground && current_onground;
            if (just_in_ground)
            {
               PrintWarn($"{Name} => just_in_ground. FOG: {FOG}", ConsoleColor.Cyan);
            }

            var just_in_air = prev_onground && !current_onground;
            if (just_in_air)
            {
                PrintWarn($"{Name} => just_in_air. FOG: {FOG}, jump_count:{++jump_count}", ConsoleColor.Cyan);
            }

            var just_jump_keypressed = !prev_jumped && current_jumped;
            if(just_jump_keypressed)
            {
                //PrintWarn($"{Name} => just_jump_keypressed. FOG: {FOG}", ConsoleColor.Cyan);
            }
            var just_jump_unkeypressed = prev_jumped && !current_jumped;

            //if (just_in_air && just_jump_unkeypressed)
              //  PrintWarn($"{Name} => Bhop Warn Type #1");

            if (just_in_ground && just_jump_keypressed)
                PrintWarn($"{Name} => Bhop Warn Type #2 (FOG:{FOG}, jump_count:{jump_count})");
        }

        private void Check_FOG()
        {
            if (prev_FOG != 0 && prev_FOG == FOG)
                PrintWarn($"{Name} => Bhop Warn Type #3");
        }

        // Frames on ground before Jump (or Duck)
        private int FOG;
        private int prev_FOG;
        private void CalculateFOG(bool OnGround)
        {
            const int MAX_FOG_FRAMES = 50;
            if (OnGround)
            {
                FOG = Math.Min(++FOG, MAX_FOG_FRAMES);
            }
            else
            {
                Check_FOG();
                prev_FOG = FOG;
                FOG = 0;
            }
        }

        private void PrintWarn(string buffer, ConsoleColor color = ConsoleColor.Red)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(buffer + $"\t\t(Time: {current_frame_time})");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
