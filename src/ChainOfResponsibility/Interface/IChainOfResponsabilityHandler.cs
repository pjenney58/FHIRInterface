using DataShapes.Model;
using Microsoft.AspNetCore.Identity;
using Collector.Interface;

namespace ChainOfResponsibility.Interface
{
    public interface IChainOfResponsabilityHandler
    {
        IChainOfResponsabilityHandler Next(IChainOfResponsabilityHandler handler);
        ICollector HandleRequest(Hl7Version version);
    }
}