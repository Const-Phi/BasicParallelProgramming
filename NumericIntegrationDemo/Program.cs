using System;
using System.Threading;
using System.Threading.Tasks;

namespace NumericIntegrationDemo
{
    internal class Program
    {
        private static double result = 0;

        private static volatile object locker = new object();

        private static void lambda(object parameter)
        {
            Console.WriteLine($"{parameter:D3}: out of critical section (before)");
            lock (locker)
            {
                Console.WriteLine($"{parameter}: in critical section");
                result += (int) parameter;
            }

            Console.WriteLine($"{parameter:D3}: out of critical section (after)");
        }

        public static void Main()
        {
            /*
            const int poolSize = 100;
            
            // Создание пула потоков
            var threadPool = new Thread[poolSize];
            for (var i = 0; i < poolSize; ++i)
                threadPool[i] = new Thread(lambda);
            
            Console.WriteLine("До...");
            
            // Запуск потоков из пула
            for (var i = 0; i < poolSize; ++i)
                threadPool[i].Start(i);

            // Ожидание завершения всех потоков из пула
            for (var i = 0; i < poolSize; ++i)
                threadPool[i].Join();
            
            Console.WriteLine("... После");
            
            // Вывод результата
            Console.WriteLine($"RESULT IS {result}");
            */
            /*
         using (var task = new Task(() => Console.WriteLine("Hello world!")))
         {
             task.Start();
             task.Wait();
         }


         var t = new Task<double>(() => 42);
         t.Start();
         t.Wait();
         Console.WriteLine($"result is {t.Result}");
         t.Dispose();

         
         var taskWithStartParameter = new Task(o => { result += (int)o; }, 1);
         taskWithStartParameter.Start();
         taskWithStartParameter.Wait();
         taskWithStartParameter.Dispose();
         
         Console.WriteLine($"Result from task is {result}");
              */

            Console.WriteLine("124");

            var t = Wrapper();
            t.RunSynchronously();
            

        }

        private static async Task Wrapper()
        {
            Console.WriteLine("before...");
            await Say();
            Console.WriteLine("...after"); 
        }

        private static async Task Say()
        {
           var i = 0;
           for (; i < 100000; ++i);
            
           Console.WriteLine("Hello world!");
        }
    }
}