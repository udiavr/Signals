using System;
using System.Threading;
using System.Threading.Tasks;

namespace Signals
{
    public class Transmitter
    {
        public delegate void TransmitSignal(Signal signal);
        public event TransmitSignal Transmit;
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
                    Thread.Sleep(5);

                    SignalsTimer++;

                    if (AnomalyTimer == 0)
                    {
                        RenewAnomalyTimer();
                    }
                    else
                    {
                        if (SignalsTimer == AnomalyTimer)
                        {
                            TransmitAnomalySignal();
                            AnomalyTimer = SignalsTimer = 0;
                        }
                        else
                        {
                            TransmitGoodSignal();
                        }
                    }
                }
            });
        }

        private void TransmitGoodSignal()
        {
            int amplitude = new Random().Next(0, 32);
            int value = new Random().Next(256, 4095);

            Signal sine = new Signal(amplitude, SignalType.Sine);
            Signal state = new Signal(value, SignalType.State);

            Transmit(sine);
            Transmit(state);
        }

        private void TransmitAnomalySignal()
        {
            int amplitude = new Random().Next(33, 50); // out of normal values
            int value = new Random().Next(1, 250); // out of normal values

            Signal sine = new Signal(amplitude, SignalType.Sine);
            Signal state = new Signal(value, SignalType.State);

            Transmit(sine);
            Transmit(state);
        }

        private void RenewAnomalyTimer()
        {
            AnomalyTimer = new Random().Next(2 * (1000 / 5), 5 * (1000 / 5)); // random between 2-5 seconds to perform anomaly signal
        }
    }
}
