using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficApp
{
    public class Test
    {
        static public bool assert(bool condition)
        {
            if (!condition)
            {
                throw new NotSupportedException("Assertion failed!");
            }
            else
                return true;
        }

        static public void RunTests()
        {
            Light.InitLights();
            assert(Test.TestIfRedLEDisOn());
            assert(Test.TestIfRedLEDisOff());

            assert(Test.TestIfYellowLEDisOn());
            assert(Test.TestIfYellowLEDisOff());

            assert(Test.TestIfGreenLEDisOn());
            assert(Test.TestIfGreenLEDisOff());

            assert(Test.TestRedSequence());
            assert(Test.TestYellowSequence());
            assert(Test.TestGreenSequence());

            assert(Test.TestCompleteSequence());

            assert(Test.TestIfAllLEDOn());
        }

        static public bool TestCompleteSequence()
        {
            var ledRed = Light.Leds[Color.Red];
            ledRed.RunSequence();
            var ledYellow = Light.Leds[Color.Yellow];
            ledYellow.RunSequence(false);
            var ledGreen = Light.Leds[Color.Green];
            ledGreen.RunSequence(false);
            ledYellow.RunSequence(false);
            ledRed.RunSequence(false);
            var expectedSequence = new List<object>();
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "Low"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "Low"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Green, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Green, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Green, "Low"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "Low"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "Low"));

            return (expectedSequence.SequenceEqual(Light.Sequence));
        }

        static internal bool TestIfRedLEDisOn()
        {
            var redled = Light.Leds[Color.Red];
            redled.SwitchOn();
            var value = redled.IsOn;

            return value;
        }

        static internal bool TestIfRedLEDisOff()
        {
            var redled = Light.Leds[Color.Red];
            redled.SwitchOff();
            var value = redled.IsOn;
            return !value;
        }

        static internal bool TestIfYellowLEDisOn()
        {
            var yellowled = Light.Leds[Color.Yellow];
            yellowled.SwitchOn();
            var value = yellowled.IsOn;
            return value;
        }

        static internal bool TestIfYellowLEDisOff()
        {
            var yellowled = Light.Leds[Color.Yellow];
            yellowled.SwitchOff();
            var value = yellowled.IsOn;
            return !value;
        }

        static internal bool TestIfGreenLEDisOn()
        {
            var Greenled = Light.Leds[Color.Green];
            Greenled.SwitchOn();
            var value = Greenled.IsOn;
            return value;
        }

        static internal bool TestIfGreenLEDisOff()
        {
            var Greenled = Light.Leds[Color.Green];
            Greenled.SwitchOff();
            var value = Greenled.IsOn;
            return !value;
        }

        static internal bool TestIfAllLEDOn()
        {
            var value = true;
            foreach (Color c in Enum.GetValues(typeof(Color)))
            {
                var led = Light.Leds[c];
                led.SwitchOn();
                value &= led.IsOn;
            }
            //foreach (Color c in Enum.GetValues(typeof(Color)))
            //{
            //    var led = new Light(c);
            //    value &= led.IsOn;
            //    led.Close();
            //}
            return value;
        }

        static internal bool TestRedSequence()
        {
            var redled = Light.Leds[Color.Red];
            redled.RunSequence();
            var expectedSequence = new List<object>();
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Red, "Low"));

            return (expectedSequence.SequenceEqual(Light.Sequence));
        }

        static internal bool TestYellowSequence()
        {
            var led = Light.Leds[Color.Yellow];
            led.RunSequence();
            var expectedSequence = new List<object>();
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Yellow, "Low"));

            return (expectedSequence.SequenceEqual(Light.Sequence));
        }

        static internal bool TestGreenSequence()
        {
            var led = Light.Leds[Color.Green];
            led.RunSequence();
            var expectedSequence = new List<object>();
            expectedSequence.Add(new Tuple<Color, string>(Color.Green, "High"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Green, "3000ms"));
            expectedSequence.Add(new Tuple<Color, string>(Color.Green, "Low"));

            return (expectedSequence.SequenceEqual(Light.Sequence));
        }
    }
}
