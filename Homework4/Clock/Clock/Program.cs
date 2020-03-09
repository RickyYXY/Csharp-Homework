using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Clock
{
    class Program
    {
        static void OnTick1(ClockEventArgs args)
        {
            Console.WriteLine($"Tick: {args.Minute}:{args.Second}");
        }

        static void OnAlarm1(ClockEventArgs args)
        {
            Console.WriteLine("It's time!\a");
            Console.WriteLine($"{args.Minute}:{args.Second}");
        }

        static void Main(string[] args)
        {
            Clock clk1 = new Clock(0, 10);
            clk1.OnTick += OnTick1;
            clk1.OnAlarm += OnAlarm1;
            clk1.Start();
        }
    }

    delegate void TickHandler(ClockEventArgs args);
    delegate void AlarmHandler(ClockEventArgs args);

    class ClockEventArgs
    {
        public int Minute { get; set; }
        public int Second { get; set; }
    }
    class Clock
    {
        private int alarmMin, alarmSec;
        public event TickHandler OnTick;
        public event AlarmHandler OnAlarm;
        public void Start()
        {
            int min = 0, sec = 0;
            while (min < 2)
            {
                Thread.Sleep(1000);
                min = (min + (sec + 1) / 60) % 60;
                sec = (sec + 1) % 60;
                if(min == alarmMin && sec == alarmSec)
                {
                    ClockEventArgs arg2 = new ClockEventArgs() { Minute = min, Second = sec };
                    OnAlarm(arg2);
                }
                else
                {
                    ClockEventArgs arg1 = new ClockEventArgs() { Minute = min, Second = sec };
                    OnTick(arg1);
                }
            }
        }
        public Clock(int min, int sec) { alarmMin = min;alarmSec = sec; }
    }
    
}
