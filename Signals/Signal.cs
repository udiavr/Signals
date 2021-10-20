using System;

namespace Signals
{
    public class Signal
    {
        public readonly DateTime TimeStamp;
        public int Value { get; set; }

        public SignalType SignalType { get; set; }

        public Signal(int value, SignalType signalType)
        {
            TimeStamp = DateTime.Now;
            SignalType = signalType;
            Value = value;
        }
    }
}
