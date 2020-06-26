using DemoParser.Demo_stuff.GoldSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCheck.Analyzer
{
    class Base
    {
 
        public int Warinigs { get; set; } = 0;
        public string Name { get; set; } = "Base analyze";
        public bool Enabled { get; set; } = true;

        public virtual void Init() { }

        public virtual string StartAnalyze()
        {
            return "StartAnalyze()";
        }

        public virtual void Frame(GoldSource.FramesHren frameData) { }
    }
}
