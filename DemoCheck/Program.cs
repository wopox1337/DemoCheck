using System;
using System.IO;
using System.Linq;
using DemoCheck.Analyzer;
using DemoParser.Demo_stuff;
using DemoParser.Demo_stuff.GoldSource;


namespace DemoCheck
{
    class Program
    {
        const string CurrentFile = "test.dem";
        static CrossParseResult CurrentDemoFile;
        
        static void Main(string[] args)
        {
            Base[] checks =
            {
                new Base() { Enabled = false },
                new BunnyHop(),
                new AimBot(),
               // new StrafeChecker()
            };

            #region Test C# skills
            /*
            // simple frameData
            int[] frameData = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

            int i = 0;
            foreach(var frame in frameData)
            {
                i++;
                foreach (var check in checks)
                {
                    if (!check.Enabled)
                        continue;

                    Console.WriteLine($" StartAnalyze() = {check.Name} -> {check.StartAnalyze()} (frame:#{i})");

                    check.Frame(frame); // Check current frame by every Analyzer type (Bhop, Aim, movement ... )
                }
            }
            */
            #endregion

            if (!File.Exists(CurrentFile) || Path.GetExtension(CurrentFile) != ".dem")
            {
                Console.WriteLine($"Can't open {CurrentFile} / wrong format.");
                return;
            }
            Console.WriteLine(Environment.CurrentDirectory);
            CurrentDemoFile = CrossDemoParser.Parse(CurrentFile);
            Console.WriteLine($"Details: {CurrentDemoFile.DisplayData}");

            if (CurrentDemoFile.Type != Parseresult.GoldSource)
            {
                Console.WriteLine($"Not GoldSrc demo type ({CurrentDemoFile.Type}).");
                return;
            }

            Console.WriteLine($"MapName: {CurrentDemoFile.GsDemoInfo.Header.MapName}");

            var dir_entries_count = CurrentDemoFile.GsDemoInfo.DirectoryEntries.Count;
            for(int entry = 0; entry < dir_entries_count; entry++)
            {
                var frame_count = CurrentDemoFile.GsDemoInfo.DirectoryEntries[entry].Frames.Count;
                var frame_keys = CurrentDemoFile.GsDemoInfo.DirectoryEntries[entry].Frames.Keys.ToArray();


                for (var frame_index = 0; frame_index < frame_count; frame_index++)
                {
                    var dem_frame = frame_keys[frame_index];

                    var frame = CurrentDemoFile.GsDemoInfo.DirectoryEntries[entry].Frames[dem_frame];

                    // Start proceed this frame
                    foreach (var check in checks)
                    {
                        if (!check.Enabled)
                            continue;

                        Console.WriteLine($" StartAnalyze() = {check.Name} -> {check.StartAnalyze()} (frame:#{dem_frame.Index})");

                        //check.Frame(frame); // Check current frame by every Analyzer type (Bhop, Aim, movement ... )
                    }
                }
                
            }

            Console.WriteLine($"FILE opened!");

            Console.ReadKey();
        }
    }

}
