using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace CZBK.ItcastOA.BLL
{
    /// <summary>
    /// TestRecursionWithLINQ
    /// </summary>
    /// <remark>Author : PetterLiu 2009-03-29 11:28  http://wintersun.cnblogs.com </remark>
    public class TestRecursionWithLINQ
    {

        /// <summary>
        /// Factorials this instance.
        /// </summary>
        /// <remark>Author : PetterLiu 2009-03-29 11:28  http://wintersun.cnblogs.com </remark>
        public void Factorial()
        {
            Func<int, int> fib = null;
            fib = n => (n == 1) ? 1 : fib(n - 1) * n;
            Console.WriteLine(fib(5));
        }

        /// <summary>
        /// Fibonaccis this instance.
        /// </summary>
        /// <remark>Author : PetterLiu 2009-03-29 11:28  http://wintersun.cnblogs.com </remark>
        public void Fibonacci()
        {
            Func<int, int> fib = null;
            fib = n => n > 1 ? fib(n - 1) + fib(n - 2) : n;
            Console.WriteLine(fib(6));
        }


        /// <summary>
        /// Recursions the get files.
        /// </summary>
        /// <remark>Author : PetterLiu 2009-03-29 11:27  http://wintersun.cnblogs.com </remark>
        public void RecursionGetFiles()
        {
            var RecGetFiles =
                Functional.Y<string, IEnumerable<string>>
                (f => d => Directory.GetFiles(d).Concat(Directory.GetDirectories(d).SelectMany(f)));

            foreach (var f in RecGetFiles(Directory.GetCurrentDirectory()))
                Console.WriteLine(f);

        }

        /// <summary>
        /// Factorial2s this instance.
        /// </summary>
        /// <remark>Author : PetterLiu 2009-03-29 11:28  http://wintersun.cnblogs.com </remark>
        public void Factorial2()
        {
            var dd = Functional.Y<int, int>(h => m => (m == 1) ? 1 : h(m - 1) * m);
            Console.WriteLine(dd(5));
        }
    }

    /// <summary>
    /// Functional
    /// </summary>
    /// <remark>Author : Wes Dyer</remark>
    public class Functional
    {
        /// <summary>
        ///delegate  Func<A, R>
        /// </summary>
        private delegate Func<A, R> Recursive<A, R>(Recursive<A, R> r);
        /// <summary>
        /// Ys the specified f.
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
        {
            Recursive<A, R> rec = r => a => f(r(r))(a);
            return rec(rec);
        }
    }
}
