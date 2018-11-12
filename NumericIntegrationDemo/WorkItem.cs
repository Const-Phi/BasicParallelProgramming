using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NumericIntegrationDemo
{
    public class WorkItem
    {
        public WorkItem(Range range, Func<double, double> function, double epsilon)
        {
            Range = range ?? throw new ArgumentNullException(nameof(range));
            Function = function ?? throw new ArgumentNullException(nameof(epsilon));
            Epsilon = epsilon;
        }

        public double Epsilon { get; }
        
        public double Area => (FLeft + FRight) * Range.Length / 2;       
      
        public WorkItem LeftPart => new WorkItem(Range.LeftPart, Function, Epsilon);
        
        public WorkItem RightPart => new WorkItem(Range.RightPart, Function, Epsilon);
        
        private Range Range { get; }

        private Func<double, double> Function { get; }
        
        private double FLeft => Function(Range.Left);

        private double FRight => Function(Range.Right);
    }

    public class Range
    {
        public Range(double left, double right)
        {
            if (left > right)
                throw new ArgumentException();
            
            Left = left;
            Right = right;
        }

        public double Left { get; }

        public double Right { get; }

        private double Middle => (Left + Right) / 2;

        public double Length => Math.Abs(Right - Left);

        public override string ToString() => $"[{Left}; {Right}]";
        
        public Range LeftPart => new Range(Left, Middle);
        
        public Range RightPart => new Range(Middle, Right);
    }

    public class Solver
    {
        private readonly ConcurrentQueue<WorkItem> queue;

        private Func<double, double> function;

        private double epsilon;
        
        private double answer = 0;
        
        private static volatile object locker = new object();
        
        public Solver(Range range, Func<double, double> function, double epsilon, IMethod method = null)
        {
            queue = new ConcurrentQueue<WorkItem>();
           
            this.function = function ?? throw new ArgumentNullException(nameof(function));

            this.epsilon = epsilon;


            queue.Enqueue(new WorkItem(range, function, epsilon));

        }

        public double Solve()
        {         
            while (!queue.IsEmpty)
            {
                if (!queue.TryDequeue(out var current))
                {
                    Console.WriteLine("Ошибка!");
                }

                var discreteArea = current.LeftPart.Area + current.RightPart.Area;
                if (Math.Abs(current.Area - discreteArea) <= current.Epsilon)
                {
                    lock (locker)
                    {
                        answer += discreteArea;
                    }
                }
                else
                {
                    queue.Enqueue(current.LeftPart);
                    queue.Enqueue(current.RightPart);
                }
            }

            return answer;
        }
    }

    public interface IMethod
    {
    }
}