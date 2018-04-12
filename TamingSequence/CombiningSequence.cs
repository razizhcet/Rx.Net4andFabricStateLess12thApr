using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.TamingSequence
{
    class CombiningSequence
    {
        static void Main(string[] args)
        {
            //ConcatSeq();
            //MergeSeq();
            //ZipMethod();
            //AndThenWhen1();
            AndThenWhen2();
            Console.ReadKey();
        }

        static void ConcatSeq()
        {
            //Generate values 0,1,2 
            var s1 = Observable.Range(0, 3);
            //Generate values 5,6,7,8,9 
            var s2 = Observable.Range(5, 5);
            s1.Concat(s2)
            .Subscribe(Console.WriteLine);
        }

        static void MergeSeq()
        {
            //Generate values 0,1,2 
            var s1 = Observable.Interval(TimeSpan.FromMilliseconds(250))
            .Take(3);
            //Generate values 100,101,102,103,104 
            var s2 = Observable.Interval(TimeSpan.FromMilliseconds(150))
            .Take(5)
            .Select(i => i + 100);
            s1.Merge(s2)
            .Subscribe(
            Console.WriteLine,
            () => Console.WriteLine("Completed"));
        }

        static void ZipMethod()
        {
            //Generate values 0,1,2 
            var nums = Observable.Interval(TimeSpan.FromMilliseconds(250))
            .Take(3);
            //Generate values a,b,c,d,e,f 
            var chars = Observable.Interval(TimeSpan.FromMilliseconds(150))
            .Take(6)
            .Select(i => Char.ConvertFromUtf32((int)i + 97));
            //Zip values together
            nums.Zip(chars, (lhs, rhs) => new { Left = lhs, Right = rhs })
            .Dump("Zip");
        }

        static void AndThenWhen1()
        {
            var one = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
            var two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(10);
            var three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(14);

            var pattern = one.And(two).And(three);
            var plan = pattern.Then((first, second, third) => new { One = first, Two = second, Three = third });
            var zippedSequence = Observable.When(plan);
            zippedSequence.Subscribe(
            Console.WriteLine,
            () => Console.WriteLine("Completed"));
        }

        static void AndThenWhen2()
        {
            var one = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
            var two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(10);
            var three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(14);

            var zippedSequence = Observable.When(
                one.And(two)
                .And(three)
                .Then((first, second, third) =>
                new {
                    One = first,
                    Two = second,
                    Three = third
                })
                );
            zippedSequence.Subscribe(
            Console.WriteLine,
            () => Console.WriteLine("Completed"));
        }
    }
}
