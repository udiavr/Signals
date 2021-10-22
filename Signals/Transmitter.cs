using System;
using System.Threading;
using System.Threading.Tasks;

namespace Signals
{
    public class Transmitter
    {
        public delegate void TransmitSignal(Signal signal);
        public event TransmitSignal Transmit;

        const int MinAnomalyTimerValue = 2; 
        const int MaxAnomalyTimerValue = 5; 
        const int MinOutOfBoundsAnomalySineValue = 33;
        const int MaxOutOfBoundsAnomalySineValue = 50;
        const int MinOutOfBoundsAnomalyStateValue = 1;
        const int MaxOutOfBoundsAnomalyStateValue = 250;
        public const int MinAnomalySineValue = 0;
        public const int MaxAnomalySineValue = 32;
        public const int MinAnomalyStateValue = 256;
        public const int MaxAnomalyStateValue = 4095;
        const int SignalTransmitMilliSecond = 5;


        public Transmitter()
        {
            RunIOTDevice();
        }

        int AnomalyTimer = 0;
        int SignalsTimer = 0;

        private void RunIOTDevice()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(SignalTransmitMilliSecond);

                    SignalsTimer++;

                    if (AnomalyTimer == 0)
                    {
                        RenewAnomalyTimer();
                    }
                    else
                    {
                        if (SignalsTimer == AnomalyTimer)
                        {
                            Transmit(new Signal(new Random().Next(MinOutOfBoundsAnomalySineValue, MaxOutOfBoundsAnomalySineValue), SignalType.Sine));
                            Transmit(new Signal(new Random().Next(MinOutOfBoundsAnomalyStateValue, MaxOutOfBoundsAnomalyStateValue), SignalType.State));
                            AnomalyTimer = SignalsTimer = 0;
                        }
                        else
                        {
                            Transmit(new Signal(new Random().Next(MinAnomalySineValue, MaxAnomalySineValue), SignalType.Sine));
                            Transmit(new Signal(new Random().Next(MinAnomalyStateValue, MaxAnomalyStateValue), SignalType.State));
                        }
                    }
                }
            });
        }

        private void RenewAnomalyTimer()
        {
            AnomalyTimer = new Random().Next(MinAnomalyTimerValue * (1000 / SignalTransmitMilliSecond), MaxAnomalyTimerValue * (1000 / SignalTransmitMilliSecond)); 
        }
    }
}
