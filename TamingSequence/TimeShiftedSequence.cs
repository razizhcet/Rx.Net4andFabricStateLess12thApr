using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.TamingSequence
{
    class TimeShiftedSequence
    {
        static void Main(string[] args)
        {
            //SizeBuffer();
            //OverlappingBuffer();
            //DelayMethod();
            TimeOutMethod();
            Console.ReadKey();
        }

        static void SizeBuffer()
        {
            var idealBatchSize = 15;
            var maxTimeDelay = TimeSpan.FromSeconds(3);
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10)
            .Concat(Observable.Interval(TimeSpan.FromSeconds(0.01)).Take(100));
            source.Buffer(maxTimeDelay, idealBatchSize)
            .Subscribe(
            buffer => Console.WriteLine("Buffer of {1} @ {0}", DateTime.Now, buffer.Count),
            () => Console.WriteLine("Completed"));
        }

        static void OverlappingBuffer()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
            source.Buffer(3, 1)
            .Subscribe(
            buffer =>
            {
                Console.WriteLine("--Buffered values");
                foreach (var value in buffer)
                {
                    Console.WriteLine(value);
                }
            }, () => Console.WriteLine("Completed"));
        }

        static void DelayMethod()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1))
                                    .Take(5)
                                    .Timestamp();
            var delay = source.Delay(TimeSpan.FromSeconds(2));
            source.Subscribe(
            value => Console.WriteLine("source : {0}", value),
            () => Console.WriteLine("source Completed"));
            delay.Subscribe(
            value => Console.WriteLine("delay : {0}", value),
            () => Console.WriteLine("delay Completed"));
        }

        static void TimeOutMethod()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(10)
                                .Concat(Observable.Interval(TimeSpan.FromSeconds(2)));
            var timeout = source.Timeout(TimeSpan.FromSeconds(1));
            timeout.Subscribe(
            Console.WriteLine,
            Console.WriteLine,
            () => Console.WriteLine("Completed"));
        }
    }
}
