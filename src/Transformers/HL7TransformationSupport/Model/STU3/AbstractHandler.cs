namespace Hl7Harmonizer.Adapters.Model.Stu3
{
    internal class AbstractHandler
    {
        private IHandler _nextHandler;

        public delegate Task<object> TaskDelegate();

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;

            // Returning a handler from here will let us link handlers in a convenient way like
            // this: monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }

        public virtual object Handle(string input, string output, Hl7Version version, TaskDelegate func)
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