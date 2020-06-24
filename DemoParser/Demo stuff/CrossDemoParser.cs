﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoParser.Demo_stuff.GoldSource;

namespace DemoParser.Demo_stuff
{
    /// <summary>
    /// Type of the demo
    /// </summary>
    public enum Parseresult
    {
        /// <summary>
        /// Not a demo/Unsupported
        /// </summary>
        UnsupportedFile,
        /// <summary>
        /// GoldSource demo
        /// </summary>
        GoldSource,
        /// <summary>
        /// HLS:OOE Demo
        /// </summary>
        Hlsooe,
        /// <summary>
        /// Demo from the L4D2 Branch eg.: Portal 2,Left 4 Dead 2, Alien Swarm
        /// </summary>
        L4D2Branch,
        /// <summary>
        /// Portal 1 demo
        /// </summary>
        Portal,
        /// <summary>
        /// Source engine demo
        /// </summary>
        Source
    }

    /// <summary>
    /// Different importance levels for demo details
    /// </summary>
    public enum DemoDataLevel
    {
        /// <summary>
        /// Not necesarry but important
        /// </summary>
        Aditional,
        /// <summary>
        /// Normal
        /// </summary>
        Netural,
        /// <summary>
        /// Really important
        /// </summary>
        Important
    }
    /// <summary>
    /// Data about the demo
    /// </summary>
    public class CrossParseResult
    {
        /// <summary>
        /// The data about the Source engine demo
        /// </summary>
        //public SourceDemoInfo Sdi;
        /// <summary>
        /// The data about the GoldSource demo
        /// </summary>
        public GoldSourceDemoInfo GsDemoInfo;
        /// <summary>
        /// The data about the HLS:OOE demo
        /// </summary>
        public GoldSourceDemoInfoHlsooe HlsooeDemoInfo;
        /// <summary>
        /// The data about the L4D2 Branch demo
        /// </summary>
        //public L4D2BranchDemoInfo L4D2BranchInfo;
        /// <summary>
        /// Type of the demo
        /// </summary>
        public Parseresult Type;
        /// <summary>
        /// The first values are exapnded to the same length (the length of the longest)
        /// with spaces the seconds ones are as long as they are
        /// this lets you print the data of the demo in human readable form
        /// </summary>
        public List<Tuple<string, string>> DisplayData;
        /// <summary>
        /// Full constructor
        /// </summary>
        public CrossParseResult(GoldSourceDemoInfoHlsooe gsdi, Parseresult pr, object sdi, GoldSourceDemoInfo gd, object lbi, List<Tuple<string, string>> dd)
        {
            HlsooeDemoInfo = gsdi;
            Type = pr;
            //Sdi = sdi;
            GsDemoInfo = gd;
            //L4D2BranchInfo = lbi;
            DisplayData = dd;
        }
        /// <summary>
        /// Empty constructor
        /// </summary>
        public CrossParseResult() { }
    }

    /// <summary>
    /// Checking the type of the demo and parsing it accordingly
    /// </summary>
    public static class CrossDemoParser
    {

        /// <summary>
        /// Parsing multiple demos asynchronously
        /// </summary>
        /// <param name="filenames">String array with the paths to the files</param>
        /// <returns></returns>
        public static CrossParseResult[] MultiDemoParse(string[] filenames)
        {
            var results = new List<CrossParseResult> {new CrossParseResult()};
            //filenames.Select(AsyncParse).ToArray();
            return results.ToArray();
        }

        /// <summary>
        /// This does an asynchronous demo parse.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static async Task<CrossParseResult> AsyncParse(string filepath)
        {
            return await new Task<CrossParseResult>(() => Parse(filepath));
        }

        /// <summary>
        /// Parses a demo file from any engine
        /// </summary>
        /// <param name="filename">Path to the file</param>
        /// <returns></returns>
        public static CrossParseResult Parse(string filename)
        {
            var cpr = new CrossParseResult();

            switch (CheckDemoType(filename))
            {
                case Parseresult.GoldSource:
                    cpr.Type = Parseresult.GoldSource;
                    cpr.GsDemoInfo = GoldSourceParser.ReadGoldSourceDemo(filename);
                    break;
                case Parseresult.UnsupportedFile:
                    cpr.Type = Parseresult.UnsupportedFile;
                    Logger.Log("Demotype check resulted in an unsupported file.");
                    break;
                case Parseresult.Source:
                    cpr.Type = Parseresult.Source;
                    Logger.Log("Parseresult.Source");
                    break;
                case Parseresult.Hlsooe:
                    cpr.Type = Parseresult.Hlsooe;
                    cpr.HlsooeDemoInfo = GoldSourceParser.ParseDemoHlsooe(filename);
                    break;
                case Parseresult.L4D2Branch:
                    cpr.Type = Parseresult.L4D2Branch;
                    Logger.Log("Parseresult.L4D2Branch");
                    break;
                default:
                    cpr.Type = Parseresult.UnsupportedFile;
                    Logger.Log(
                        "No idea how the fuck did this happen but default happened at switch(CheckDemoType(filename))");
                    break;
            }
            if(cpr.Type == Parseresult.GoldSource)
            if (cpr.GsDemoInfo.ParsingErrors.Count > 0)
            {
                Logger.Log("Demo errors detected!");
            }
            cpr.DisplayData = GetDemoDataTuples(cpr);
            return cpr;
        }

        /// <summary>
        ///     This returns a nice string which can be assigned to the richtexbox only call it if you
        ///     are extremely sure the demo is not corrupt.
        /// </summary>
        /// <param name="demo"></param>
        /// <returns></returns>
        public static List<Tuple<string, string>> GetDemoDataTuples(CrossParseResult demo)
        {
            //Maybe enum as 3rd tuple item?
            var result = new List<Tuple<string,string>>();

            #region Print
            switch (demo.Type)
            {
                case Parseresult.UnsupportedFile:
                    result.Add(new Tuple<string, string>("Unsupported file!",""));
                    break;
                case Parseresult.GoldSource:
                    result = new List<Tuple<string, string>>()
                    {
                        new Tuple<string,string>($"Analyzed GoldSource engine demo file ({demo.GsDemoInfo.Header.GameDir}):",""),
                        new Tuple<string,string>($"Demo protocol",$"{demo.GsDemoInfo.Header.DemoProtocol}"),
                        new Tuple<string,string>($"Net protocol",$"{demo.GsDemoInfo.Header.DemoProtocol}"),
                        new Tuple<string,string>($"Directory Offset",$"{demo.GsDemoInfo.Header.DirectoryOffset}"),
                        new Tuple<string,string>($"MapCRC",$"{demo.GsDemoInfo.Header.MapCrc}"),
                        new Tuple<string,string>($"Map name",$"{demo.GsDemoInfo.Header.MapName}"),
                        new Tuple<string,string>($"Game directory",$"{demo.GsDemoInfo.Header.GameDir}"),
                        new Tuple<string,string>($"Length in seconds",$"{demo.GsDemoInfo.DirectoryEntries.Sum(x => x.TrackTime).ToString("n3")}s"),
                        new Tuple<string,string>($"Directory Offset",$"{demo.GsDemoInfo.Header.DirectoryOffset}"),
                        new Tuple<string,string>($"Frame count",$"{demo.GsDemoInfo.DirectoryEntries.Sum(x => x.FrameCount)}"),
                        new Tuple<string,string>($"Highest FPS",$"{(1 / demo.GsDemoInfo.AditionalStats.FrametimeMin).ToString("N2")}"),
                        new Tuple<string,string>($"Lowest FPS",$"{(1 / demo.GsDemoInfo.AditionalStats.FrametimeMax).ToString("N2")}"),
                        new Tuple<string,string>($"Average FPS",$"{(demo.GsDemoInfo.AditionalStats.Count / demo.GsDemoInfo.AditionalStats.FrametimeSum).ToString("N2")}"),
                        new Tuple<string,string>($"Lowest msec",$"{(1000.0 / demo.GsDemoInfo.AditionalStats.MsecMin).ToString("N2")} FPS"),
                        new Tuple<string,string>($"Highest msec",$"{(1000.0 / demo.GsDemoInfo.AditionalStats.MsecMax).ToString("N2")} FPS"),
                        new Tuple<string,string>($"Frame count",$"{demo.GsDemoInfo.DirectoryEntries.Sum(x => x.FrameCount)}"),
                        new Tuple<string,string>($"Average msec",$"{(1000.0 / (demo.GsDemoInfo.AditionalStats.MsecSum / (double)demo.GsDemoInfo.AditionalStats.Count)).ToString("N2")} FPS")
                    };
                    break;
                case Parseresult.Hlsooe:
                    result = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>($"Demo protocol", $"{demo.HlsooeDemoInfo.Header.DemoProtocol}"),
                        new Tuple<string, string>($"Net protocol", $"{demo.HlsooeDemoInfo.Header.NetProtocol}"),
                        new Tuple<string, string>($"Directory offset", $"{demo.HlsooeDemoInfo.Header.DirectoryOffset}"),
                        new Tuple<string, string>($"Map name", $"{demo.HlsooeDemoInfo.Header.MapName}"),
                        new Tuple<string, string>($"Game directory", $"{demo.HlsooeDemoInfo.Header.GameDir}"),
                        new Tuple<string, string>($"Length in seconds", $"{demo.HlsooeDemoInfo.DirectoryEntries.SkipWhile(x => x.FrameCount < 1).Max(x => x.Frames.Max(y => y.Key.Index))*0.015}s [{demo.HlsooeDemoInfo.DirectoryEntries.SkipWhile(x => x.FrameCount < 1).Max(x => x.Frames.Max(y => y.Key.Index))}ticks]"),
                        new Tuple<string,string>($"Save flag:",$"{demo.HlsooeDemoInfo.DirectoryEntries.SkipWhile(x => x.FrameCount < 1).Max(x => x.Frames.Where((y => y.Key.Type == Hlsooe.DemoFrameType.ConsoleCommand)).FirstOrDefault(z => ((Hlsooe.ConsoleCommandFrame)(z.Value)).Command.Contains("#SAVE#")).Key.Index)*0.015} [{demo.HlsooeDemoInfo.DirectoryEntries.SkipWhile(x => x.FrameCount < 1).Max(x => x.Frames.Where((y => y.Key.Type == Hlsooe.DemoFrameType.ConsoleCommand)).FirstOrDefault(z => ((Hlsooe.ConsoleCommandFrame)(z.Value)).Command.Contains("#SAVE#")).Key.Index)}ticks]")
                    };
                    break;
                case Parseresult.Source:
                    break;
                case Parseresult.Portal:
                case Parseresult.L4D2Branch:
                    break;
            }
            #endregion

            return FormatTuples(result);
        }

        /// <summary>
        /// Formats the list of tuples' first value so all of them are padded to the same length as
        /// the longest on +8 spaces so its nice
        /// </summary>
        /// <param name="original">This is the original tuple list which we get from GetDemoDataTuples()</param>
        /// <param name="spacebetween">This is a not required parameter which supplies the space between the 
        /// two tuple items</param>
        /// <returns></returns>
        public static List<Tuple<string, string>> FormatTuples(List<Tuple<string, string>> original,int spacebetween = 8)
        {
            var result = original;
            var longest = result.Max(x => x.Item1.Length)+spacebetween;
            for (var index = 0; index < result.Count; index++)
                result[index] = new Tuple<string, string>(result[index].Item1.PadRight(longest), result[index].Item2);
            return result;
        }

        /// <summary>
        /// Checks the demo type a Parseresult is returned
        /// </summary>
        /// <param name="file">The path to the demo file to check</param>
        /// <returns></returns>
        public static Parseresult CheckDemoType(string file)
        {
            var attr = new FileInfo(file);
            if (attr.Length < 540)
            {
                return Parseresult.UnsupportedFile;
            }
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read,FileShare.Read))
            using (var br = new BinaryReader(fs))
            {
                var mw = Encoding.ASCII.GetString(br.ReadBytes(8)).TrimEnd('\0');
                switch (mw)
                {
                    case "HLDEMO": return br.ReadByte() <= 2 ? Parseresult.Hlsooe : Parseresult.GoldSource;
                    case "HL2DEMO": return br.ReadInt32() < 4 ? Parseresult.Source: Parseresult.L4D2Branch;//TODO: Remove L4D2 Branch once costumdata parsing is done
                    default: return Parseresult.UnsupportedFile;
                }
            }
        }
    }
}