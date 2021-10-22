using System;
using System.IO;
using System.Windows;

namespace Signals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EventHandler<Signal> SignalReceived;
        Transmitter transmitter;
        public bool isAnomaly;
        

        public MainWindow()
        {
            InitializeComponent();

            transmitter = new Transmitter();
            transmitter.Transmit += Transmitter_Transmit;
        }

        private void Transmitter_Transmit(Signal signal)
        {
            SaveSignal(signal);
            isAnomaly = CheckSignal(signal);
            if (isAnomaly)
            {
                SaveAlarm(signal);
            }
        }

        private void SaveAlarm(Signal signal)
        {
            //log out of bounds signal values
            File.AppendAllText("Alarm.csv", string.Format("{0}.{1},{2},{3}{4}", signal.TimeStamp, signal.TimeStamp.Millisecond, signal.Value, signal.SignalType, Environment.NewLine));
        }

        private bool CheckSignal(Signal signal)
        {
            //check if signal's values are in or out of bounds
            return ((signal.SignalType == SignalType.Sine && (signal.Value < Transmitter.MinAnomalySineValue || signal.Value > Transmitter.MaxAnomalySineValue)) ||
                (signal.SignalType == SignalType.State && (signal.Value < Transmitter.MinAnomalyStateValue || signal.Value > Transmitter.MaxAnomalyStateValue)));
        }

        private void SaveSignal(Signal signal)
        {
            //log signal transmitted
            File.AppendAllText("Signal.csv", string.Format("{0}.{1},{2},{3}{4}", signal.TimeStamp, signal.TimeStamp.Millisecond, signal.Value, signal.SignalType, Environment.NewLine));
        }
    }
}
