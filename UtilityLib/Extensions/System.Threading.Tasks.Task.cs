// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System; 
using System.Threading; 
using System.Threading.Tasks;

namespace UtilityLib {
    public static partial class TaskExtensions {
        // References: tutorials.csharp-online.net/Task_Combinators
        public static Task<TResult> WithCancel<TResult> (this Task<TResult> task, CancellationToken cancelToken) { 
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            CancellationTokenRegistration reg = cancelToken.Register(() => tcs.TrySetCanceled ());
            task.ContinueWith(ant => {
                    reg.Dispose();
                    if (ant.IsCanceled)     tcs.TrySetCanceled();
                    else if (ant.IsFaulted) tcs.TrySetException (ant.Exception.InnerException);
                    else                    tcs.TrySetResult(ant.Result);
                });
            return tcs.Task;
        }
        public async static Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks) {
            TaskCompletionSource<TResult[]> killJoy = new TaskCompletionSource<TResult[]>();
            foreach (var task in tasks) {
                Task discard = task.ContinueWith(ant => {
                        if (ant.IsCanceled)     killJoy.TrySetCanceled();
                        else if (ant.IsFaulted) killJoy.TrySetException(ant.Exception.InnerException);
                    });
            }
            await Task.Yield();
            return await await Task.WhenAny (killJoy.Task, Task.WhenAll (tasks));
        }
        public async static Task<TResult> WithTimeout<TResult> (this Task<TResult> task, TimeSpan timeout) {
            await Task.Yield();
            Task winner = await Task.WhenAny (task, Task.Delay(timeout));
            if (winner != task) throw new TimeoutException();
            return await task; // Unwrap result/re-throw
        }
    }
}
