// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityLib {
    public class JobLimiter : INotifyPropertyChanged {
        private Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();
        private object _sync = new object();
        private int _limit;
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
        public JobLimiter(int Limit) {
            this.Limit = Limit;
        }
        public Task<bool> JobStart() { return JobStart(CancellationToken.None); }
        public Task<bool> JobStart(CancellationToken cancelWaitToken) {
            Task<bool> waitTask = null;
            lock (_sync) {
                _count++;
                TaskCompletionSource<bool> waitCompletion = new TaskCompletionSource<bool>(); 
                waitTask = waitCompletion.Task;
                if (_count > _limit) {
                    if (cancelWaitToken != CancellationToken.None) cancelWaitToken.Register( () => Task.Run(() => waitCompletion.SetCanceled()) ); // spin off so that awaiter gets it's own task/thread and cancelations can continue quickly.
                    _waiters.Enqueue(waitCompletion);
                } else {
                    waitCompletion.SetResult(true);
                }
            }
            RaiseCountChanged();
            return waitTask; 
        }
        public void JobDone() {
            TaskCompletionSource<bool> next = null;
            lock (_sync) {
                _count = (_count == 0) ? 0 : _count - 1; // on the odd chance that JobDone may be called twice by some threads, error check here.
                if (CurrentProcessing < Limit && _waiters.Count > 0) next = _waiters.Dequeue();
            }
            RaiseCountChanged();
            if (next != null) Task.Run(() => next.SetResult(true) ); // SetResult() continues to where orig Task left off and continues on same thread as SetResult(), so spawn off new thread here, don't do all on same thread.
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
