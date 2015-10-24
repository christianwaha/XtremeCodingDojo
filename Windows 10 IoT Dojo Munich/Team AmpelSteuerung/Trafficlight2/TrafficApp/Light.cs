using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace TrafficApp
{
    public enum Color { Red = 12, Yellow = 26, Green = 21 }
    public class Light
    {
        GpioController controller = GpioController.GetDefault();
        GpioPin gpiPin;
        Color color;
        public static List<object> Sequence { get; set; }
        public static Dictionary<Color,Light> Leds { get; set; }
        public static void InitLights()
        {
            Sequence = new List<object>();
            Leds = new Dictionary<Color, Light>();
            foreach (Color c in Enum.GetValues(typeof(Color)))
            {
                Leds.Add(c, new Light(c));
            }
        }
        public Light(Color color)
        {

            if (controller == null)
            {
                return;
            }

            if (this.gpiPin == null)
            {
                this.color = color;
                this.gpiPin = controller.OpenPin((int)color, GpioSharingMode.Exclusive);
                this.gpiPin.SetDriveMode(GpioPinDriveMode.Output);
            }

        }

        //public void Close()
        //{
        //    gpiPin.Dispose();
        //}

        public bool IsOn
        {
            get
            {
                return this.gpiPin.Read() == GpioPinValue.High;
            }
        }
        public void SwitchOn()
        {
            gpiPin.Write(GpioPinValue.High);
            Sequence.Add(new Tuple<Color, string>(this.color, gpiPin.Read().ToString()));
        }

        internal void SwitchOff()
        {
            gpiPin.Write(GpioPinValue.Low);
            Sequence.Add(new Tuple<Color, string>(this.color, gpiPin.Read().ToString()));
        }

        internal void RunSequence(bool clearlist = true)
        {
            if (clearlist)
            {
                Sequence.Clear();
            }
            this.SwitchOn();
            Task.Delay(3000).Wait();
            Sequence.Add(new Tuple<Color, string>(this.color, "3000ms"));
            this.SwitchOff();
        }
    }
}
