using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;

namespace NumericIntegrationDemo
{
    internal class Program
    {
        public class TaskChain
        {
            private readonly List<Task> tasks = new List<Task>();

            
            public TaskChain(params Action[] actions)
            {
                if (actions is null)
                    throw new ArgumentNullException(nameof(actions));

                if (actions.Length < 1)
                    return;
                
                var actionQueue = new Queue<Action>(actions);   
                tasks.Add(new Task(actionQueue.Dequeue()));
                while (actionQueue.Count > 0)
                {
                    tasks.Add(tasks.Last().ContinueWith(t => actionQueue.Dequeue()));
                }           
            }

            public void Start() => tasks[0].Start();

            public void Wait() => tasks.Last().Wait();
        }

        public static void Main()
        {
            // ParalleDemo();

            // TaskChainDemo();

            const int size = 5;
            var tasks = new Task[size];
            
            for (var i = 0; i < size; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                    WriteLine("Hello world!");
                });
            }


            var index = Task.WaitAny(tasks, 1000, CancellationToken.None);
            WriteLine($"{index} --> wait");
            if (tasks[index].IsCompleted)
            {
                WriteLine($"{index} --> IsCompleted");
                tasks[index].Start();
                tasks[index].Wait();
                WriteLine("end");
            }


            ReadKey();
        }

        private static void TaskChainDemo()
        {
            var actions = new Action[]
            {
                () => WriteLine("First task"),
                () => WriteLine("Second task"),
                () => WriteLine("Third task")
            };

            var chain = new TaskChain(actions);

            chain.Start();
            // chain.Wait();
        }

        private static void ParalleDemo()
        {
            var collection = new ConcurrentQueue<int>(Enumerable.Range(1, 32));

            WriteLine($"Parallel.ForEach()");
            Parallel.ForEach(collection, WriteLine);

            int start = 5, finish = 15;
            WriteLine($"Parallel.For(i = {start}; i < {finish}; i++)");
            // for (var i = start; i < finish; i++)
            Parallel.For(start, finish, WriteLine);
        }
    }
}