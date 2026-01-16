using _src.Code.Core.Delegates;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IAnimationQueueService
    {
        void Push(AnimationDelegate animationDelegate);
        void Next();
        void Clear();
    }
}