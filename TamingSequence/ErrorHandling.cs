using Rx.NetProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Rx.TamingSequence
{
    public static class ErrorHandling
    {
        public static void Dump<T>(this IObservable<T> source, string name)
        {
            source.Subscribe(
            i => Console.WriteLine("{0}-->{1}", name, i),
            ex => Console.WriteLine("{0} failed-->{1}", name, ex.Message),
            () => Console.WriteLine("{0} completed", name));
        }

        static void Main(string[] args)
        {
            //CatchError();
            //TimeOutEx();
            //ExceptionErrorCought();
            //FinallyWork();
            UsingMethod();
            Console.ReadKey();
        }

        static void CatchError()
        {
            var source = new Subject<int>();
            var result = source.Catch(Observable.Empty<int>());
            result.Dump("Catch");
            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new Exception("Fail!"));
        }

        static void TimeOutEx()
        {
            var source = new Subject<int>();
            var result = source.Catch<int, TimeoutException>(tx => Observable.Return(-1));
            result.Dump("Catch");
            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new TimeoutException());
        }

        static void ExceptionErrorCought()
        {
            var source = new Subject<int>();
            var result = source.Catch<int, TimeoutException>(tx => Observable.Return(-1));
            result.Dump("Catch");
            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new ArgumentException("Fail!"));
        }

        static void FinallyWork()
        {
            var source = new Subject<int>();
            var result = source.Catch<int, TimeoutException>(tx => Observable.Return(-1));
            result.Dump("Catch");
            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new ArgumentException("Fail!"));
        }

        static void UsingMethod()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1));
            var result = Observable.Using(
            () => new TimeIt("Subscription Timer"),
            timeIt => source);
            result.Take(5).Dump("Using");
        }
    }
}
