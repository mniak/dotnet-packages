using System;
using System.Collections.Generic;

namespace Mniak.RequestCompression.Example.Server.Infrastructure.Compression
{
    internal class MultiDisposable : IDisposable
    {
        readonly Stack<IDisposable> stack = new Stack<IDisposable>();
        public void Dispose()
        {
            while (stack.Count > 0)
            {
                stack.Pop().Dispose();
            }
        }

        internal T Add<T>(T disposable) where T : IDisposable
        {
            stack.Push(disposable);
            return disposable;
        }
    }
}