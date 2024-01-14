using DataShapes.Model;
using Microsoft.AspNetCore.Identity;
using Collectors.Interface;

namespace ChainOfResponsibility.Interface
{
    public interface IChainOfResponsabilityHandler
    {
        IChainOfResponsabilityHandler Next(IChainOfResponsabilityHandler handler);
        ICollector HandleRequest(InputVersion version);
    }
}