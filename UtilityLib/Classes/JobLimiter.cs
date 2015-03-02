// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityLib {
    /// <summary>
    ///     A bit like a semaphore to control the overall number of complex jobs 
    ///     where a concurrent task scheduler might not be effective.
    ///     Supports job cancelation, dynamic job limit and job count notifications.
    /// </summary>
    /// <example>
    ///     <code>
    ///         myLimiter = new JobLimiter(10);
    ///         private void DoJob() {
    ///             Task gateTask = null;
    ///             try {
    ///                 ... preparation
    ///                 gateTask = myLimiter.JobStart(CancelToken);
    ///                 await gateTask;
    ///                 ... do job 
    ///             } finally {
    ///                 if (gateTask != null) myLimiter.JobDone();
    ///             }
    ///         }
    ///     </code>
    /// </example>
    public class JobLimiter : INotifyPropertyChanged {
        private Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();
        private object _sync = new object();
        private int _limit;

        /// <summary>
        ///     Dynamically set maximum number of jobs allowed.  If lowered, jobs are not canceled and 
        ///     allowed to finish, but while Count is above the limit, no new job will be started.
        /// </summary>
        public int Limit { 
            get { lock (_sync) return _limit; } 
            set { if (value < 1) throw new Exception("Limit must be positive integer."); 
                  List<TaskCompletionSource<bool>> tasks = new List<TaskCompletionSource<bool>>();
                  lock (_sync) {
                      int delta = (value - _limit); // limit is increased (delta is positive), then release these tasks from queue.
                      _limit = value; 
                      while (delta-- > 0 && _waiters.Count > 0) tasks.Add(_waiters.Dequeue());
                  }
                  RaiseCountChanged();
                  if (tasks.Count > 0) tasks.ForEach(tcs => Task.Run(() => tcs.SetResult(true) ) ); // SetResult() continues to where orig Task left off and continues on same thread as SetResult(), so spawn off new thread here, don't do all on same thread.
            } 
        }

        public int CurrentProcessing { get { return _count - _waiters.Count; } }
        public int CurrentQueue { get { return _waiters.Count; } }
        public int CurrentTotal { get { return _count; } }
            private int _count = 0;

        /// <summary>Initialize JobLimmiter with maximum number of jobs allowed.</summary>
        /// <param name="Limit">Initial maximum number of jobs.</param>
        public JobLimiter(int Limit) {
            this.Limit = Limit;
        }

        /// <summary>
        ///     Called at the 'gate' where a job starts.
        /// </summary>
        /// <param name="CancelWaitToken">Called to cancel job.  Will result in thrown exception.</param>
        /// <returns>bool return indicates if gate was unlocked an job was allowed to continue</returns>
        /// <exception cref="TaskCanceledException">Thrown if job is canceled before gate is lifted.</exception>
        public Task<bool> JobStart() { return JobStart(CancellationToken.None); }
        public Task<bool> JobStart(CancellationToken CancelWaitToken) {
            Task<bool> waitTask = null;
            lock (_sync) {
                _count++;
                TaskCompletionSource<bool> waitCompletion = new TaskCompletionSource<bool>(); 
                waitTask = waitCompletion.Task;
                if (_count > _limit) {
                    if (CancelWaitToken != CancellationToken.None) CancelWaitToken.Register( () => Task.Run(() => waitCompletion.SetCanceled()) ); // spin off so that awaiter gets it's own task/thread and cancelations can continue quickly.
                    _waiters.Enqueue(waitCompletion);
                } else {
                    waitCompletion.SetResult(true);
                }
            }
            RaiseCountChanged();
            return waitTask; 
        }

        /// <summary>
        ///     Called at end of job to let next in queue proceed.  One JobDone() must be called 
        ///     for each JobStart(), so ideally put in finally{} clause of a try-finally block.
        /// </summary>
        public void JobDone() {
            TaskCompletionSource<bool> next = null;
            lock (_sync) {
                _count = (_count == 0) ? 0 : _count - 1; // on the odd chance that JobDone may be called twice by some threads, error check here.
                if (CurrentProcessing < Limit && _waiters.Count > 0) next = _waiters.Dequeue();
            }
            RaiseCountChanged();
            if (next != null) Task.Run(() => next.SetResult(true) ); // SetResult() continues to where orig JobDone task left off and continues on same thread as SetResult(), so spawn off new thread here, don't do all on same thread.
        }

        public event PropertyChangedEventHandler PropertyChanged;
            private void RaiseCountChanged() { 
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(this.MemberName(x => x.CurrentTotal))); 
                    PropertyChanged(this, new PropertyChangedEventArgs(this.MemberName(x => x.CurrentQueue))); 
                    PropertyChanged(this, new PropertyChangedEventArgs(this.MemberName(x => x.CurrentProcessing))); 
                }
            }
    }
}
