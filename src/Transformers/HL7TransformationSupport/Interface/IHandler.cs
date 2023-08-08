using Hl7Harmonizer.Adapters.Model;

namespace Hl7Harmonizer.Adapters.Interface
{
    internal interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(string input, string output, Hl7Version version, Delegate func);
    }
}