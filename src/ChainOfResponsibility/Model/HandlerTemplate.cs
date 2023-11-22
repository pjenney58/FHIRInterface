
using CollectorBase.Interface;

namespace ChainOfResponsibility.Model
{
    public abstract class Handler
    {
        protected Handler? successor;

        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleRequest(ICollector request);
    }
}

