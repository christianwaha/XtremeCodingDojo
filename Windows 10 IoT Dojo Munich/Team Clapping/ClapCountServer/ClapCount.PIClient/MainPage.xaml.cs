using ClapCounter.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClapCounter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Dieser Eingang sollte natürlich immer an die aktuelle Schaltung angepasst werden.
        private const int MICRO_INPUT = 6;

        private const double TIME_SHORT = 100;
        private const double TIME_LONG = 400;
        private const double TIME_PAUSE = 1000;

        private GpioPin _inputPin = null;

        public MainPage()
        {
            this.InitializeComponent();

            // Wurde die Hardware nicht richtig initialisiert, brauchen wir ab hier nicht weiter machen.
            if (initialize())
            {
                Task.Run(() =>
                {
                    detectSignals();
                });
            }
        }

        private bool initialize()
        {
            try
            {
                var gpio = GpioController.GetDefault();

                if (gpio != null)
                {
                    _inputPin = gpio.OpenPin(MICRO_INPUT);

                    if (_inputPin != null)
                    {
                        _inputPin.SetDriveMode(GpioPinDriveMode.Input);

                        return true;
                    }
                }
            }
            catch
            {
                lb_Output.Items.Add("Initialization failed!");
            }

            return false;
        }

        private void detectSignals()
        {
            Stopwatch stopWatch = new Stopwatch();

            while (true)
            {
                GpioPinValue currentValue = _inputPin.Read();

                // Wir müssen nur zwischen 2 Zuständen unterscheiden.
                // 1. Wir warten auf ein Low-Signal und beginnen dann die Zeitmessung
                // 2. Wir stoppen die im ersten Schritt gestartete Zeitmessung und werten anschließend die Zeit aus.
                // Da im ersten Schritt die Zeitmessung gestoppt ist und im zweiten nicht, benutzen wir hier die entsprechende Eigenschaft,
                // um zwischen den jeweiligen Schritten zu unterscheiden.
                if (!stopWatch.IsRunning)
                {
                    if (currentValue == GpioPinValue.Low)
                    {
                        stopWatch.Restart();
                    }
                }
                else if (currentValue == GpioPinValue.High)
                {
                    stopWatch.Stop();

                    var duration = stopWatch.ElapsedMilliseconds;


                    stopWatch.Reset();

                    // Unter dem Schwellwert von Short werten wir das Signal als zu kurz und damit ungültig.
                    // Ansonsten ordnen wir das Signal anhand der festgelegten Schwellwerte dem jeweiligen Typ zu.
                    if (duration >= TIME_SHORT)
                    {
                        SignalTypes signalType = SignalTypes.Short;

                        if (duration >= TIME_PAUSE)
                        {
                            signalType = SignalTypes.Pause;
                        }
                        else if (duration >= TIME_LONG)
                        {
                            signalType = SignalTypes.Long;
                        }

                        // Normalerweise würden wir hier ein entsprechendes Event feuern.
                        // Aber in diesem Fall helfen wir uns mit einer einfachen Ausgabe.
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            lb_Output.Items.Add(String.Format("{0}: {1} ({2} ms)", DateTime.Now.ToString("HH:mm:ss"), signalType.ToString(), duration));

                            if (lb_Output.Items.Count > 10)
                            {
                                lb_Output.Items.RemoveAt(0);
                            }
                        })
                        .AsTask().Wait();
                    }
                }

                // Damit verhindern wir, dass der arme Raspi heiß läuft ;-)
                // 100 Abtastungen pro Sekunde sollten in diesem Fall genügen.
                Task.Delay(10);
            }
        }
    }
}
