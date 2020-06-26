﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using DemoCheck.Analyzer;
using DemoParser.Demo_stuff;
using DemoParser.Demo_stuff.GoldSource;


namespace DemoCheck
{
    class Program
    {
        const string CurrentFile = "test.dem";
        static CrossParseResult CurrentDemoFile;
        static bool enableDataLogging = false;

        static void Main(string[] args)
        {
            Base[] checks =
            {
                new Base() { Enabled = false },
                new BunnyHop() { Enabled = true },
                new AimBot() { Enabled = false },
               // new StrafeChecker()
            };

            if (!File.Exists(CurrentFile) || Path.GetExtension(CurrentFile) != ".dem")
            {
                Console.WriteLine($"Can't open {CurrentFile} / wrong format.");
                return;
            }
            
            CurrentDemoFile = CrossDemoParser.Parse(CurrentFile);

            Console.WriteLine("Demo data:");
            foreach (var data in CurrentDemoFile.DisplayData)
            {
                Console.WriteLine($"\t{data}");
                /* 
Details: (Analyzed GoldSource engine demo file (cstrike):        , )
Details: (Demo protocol                                          , 5)
Details: (Net protocol                                           , 5)
Details: (Directory Offset                                       , 3382898)
Details: (MapCRC                                                 , 777915923)
Details: (Map name                                               , cs_assault)
Details: (Game directory                                         , cstrike)
Details: (Length in seconds                                      , 94,860s)
Details: (Directory Offset                                       , 3382898)
Details: (Frame count                                            , 5680)
Details: (Highest FPS                                            , ?)
Details: (Lowest FPS                                             , 18,58)
Details: (Average FPS                                            , 60,05)
Details: (Lowest msec                                            , ? FPS)
Details: (Highest msec                                           , 18,52 FPS)
Details: (Frame count                                            , 5680)
Details: (Average msec                                           , 60,05 FPS)
             */
            }

            if (CurrentDemoFile.Type != Parseresult.GoldSource)
            {
                Console.WriteLine($"Not GoldSrc demo type ({CurrentDemoFile.Type}).");
                return;
            }

            #region My code
            foreach(var entry in CurrentDemoFile.GsDemoInfo.DirectoryEntries)
            {
                if (entry.Type != 1) // Only PLAYBACK state
                    continue;
                
                int frame_index = 0;
                foreach (var frame in entry.Frames)
                {
                    foreach (var check in checks)
                    {
                        if (!check.Enabled)
                            continue;
                
                        check.Frame(frame);
                    }

                    if (!enableDataLogging)
                        continue;

                    var type = frame.Key.Type;
                    //Console.WriteLine($"Type: {type}");

                    switch (type)
                    {
                        case GoldSource.DemoFrameType.ConsoleCommand:
                            {
                                var Command = ((GoldSource.ConsoleCommandFrame)frame.Value).Command;
                                Console.WriteLine($" Command: {Command}");
                
                                break;
                            }
                            
                        case GoldSource.DemoFrameType.ClientData:
                            {
                                var CData = ((GoldSource.ClientDataFrame)frame.Value);
                                var CData_string = "ClientData: {\n"
                                    + $"\t Origin: [ {CData.Origin.X}, {CData.Origin.Y}, {CData.Origin.Z} ] \n"
                                    + $"\t Viewangles: [ {CData.Viewangles.X}, {CData.Viewangles.Y}, {CData.Viewangles.Z} ] \n"
                                    + $"\t WeaponBits: {CData.WeaponBits} \n"
                                    + $"\t Fov: {CData.Fov}"
                                    + "\n}";
                
                                Console.WriteLine(CData_string);
                               
                                break;
                            }
                            
                        case GoldSource.DemoFrameType.Event:
                            {
                                var EventData = ((GoldSource.EventFrame)frame.Value);
                                var EventData_string = "EventData: {\n"
                                    + $"\t Delay: [ {EventData.Delay} ] \n"
                                    + $"\t Index: {EventData.Index} \n"
                                    + "\t EventArguments: { \n"
                                        + $"\t\t EntityIndex: {EventData.EventArguments.EntityIndex} \n"
                                        + $"\t\t Velocity:[ {EventData.EventArguments.Velocity.X}, {EventData.EventArguments.Velocity.Y}, {EventData.EventArguments.Velocity.Z}] \n"
                                        + $"\t\t Angles: [ {EventData.EventArguments.Angles.X}, {EventData.EventArguments.Angles.Y}, {EventData.EventArguments.Angles.Z}] \n"
                                        + $"\t\t Flags: {EventData.EventArguments.Flags} \n"
                                        + $"\t\t Ducking: {EventData.EventArguments.Ducking} \n"
                                        + "\t }"
                                    + "\n}";
                
                                Console.WriteLine(EventData_string);
                
                                break;
                            }

                        case GoldSource.DemoFrameType.NetMsg:
                            {
                                var NetMsgData = ((GoldSource.NetMsgFrame)frame.Value);

                                var NetMsgData_string = "NetMsgData: {\n"
                                    + $"\t {NetMsgData.RParms.ClViewangles.X}"
                                    + "\n}";

                                Console.WriteLine(NetMsgData_string);
                                break;
                            }
                        
                        case GoldSource.DemoFrameType.WeaponAnim:
                            {
                                var WeaponAnim = ((GoldSource.WeaponAnimFrame)frame.Value);
                
                                break;
                            }
                        }
                
                    ++frame_index;
                }
            }
            #endregion

            Console.WriteLine($"Analyze finished!");
            Console.ReadKey();
        }
    }

}
