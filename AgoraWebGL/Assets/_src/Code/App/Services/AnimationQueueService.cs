using System;
using System.Collections.Generic;
using _src.Code.Core.Delegates;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;

namespace _src.Code.App.Services
{
    public class AnimationQueueService : IAnimationQueueService
    {
        private readonly Queue<AnimationDelegate> _animationQueue = new Queue<AnimationDelegate>();
        private bool _playingQueue = false;
        
        [Inject]
        public AnimationQueueService()
        {
        }
        
        // METHODS
        
        public void Push(AnimationDelegate animationDelegate)
        {
            _animationQueue.Enqueue(animationDelegate);
            if (!_playingQueue)
                PlayFirstAnimationFromQueue();
        }
        
        public void Next()
        {
            if (_animationQueue.Count > 0)
                PlayFirstAnimationFromQueue();
            else
                _playingQueue = false;
        }

        public void Clear()
        {
            _animationQueue.Clear();
            _playingQueue = false;
        }

        // PRIVATE
        
        private void PlayFirstAnimationFromQueue()
        {
            if (_animationQueue.Count == 0)
            {
                _playingQueue = false;
                return;
            }

            _playingQueue = true;
            var animation = _animationQueue.Dequeue();

            try
            {
                animation.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred during animation: {ex.Message}");
                Debug.LogError(ex.StackTrace);
            }
        }
    }
}