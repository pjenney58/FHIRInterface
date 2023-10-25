using DataShapes.Model;
using Microsoft.Extensions.Logging;

namespace Collector.CollectorFactory
{
    public class CollectorFactory
    {
        internal readonly ILogger _logger;
        internal readonly DataShapeContext? _context;

        public CollectorFactory(ILogger logger, DataShapeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<T> Get<T>(T type)
        {
            _logger.LogInformation($"Created new type ");
            throw new NotImplementedException();
        }
    }
}

