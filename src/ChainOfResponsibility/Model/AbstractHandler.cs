
using ChainOfResponsibility.Interface;
using DataShapes.Model;

namespace ChainOfResponsibility.Model
{
    public class AbstractHandler
    {
        private IChainOfResponsabilityHandler? _nextHandler = null;

        public delegate Task<object> TaskDelegate();

        public IChainOfResponsabilityHandler SetNext(IChainOfResponsabilityHandler handler)
        {
            this._nextHandler = handler;

            // Returning a handler from here will let us link handlers in a convenient way like
            // this: monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }

        public virtual object? Handle(string input, string output, InputVersion version, TaskDelegate func)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(input, output, version, func);
            }
            else
            {
                return null;
            }
        }
    }
}