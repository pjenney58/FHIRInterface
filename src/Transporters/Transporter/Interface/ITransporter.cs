using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transporters.Interface
{
    public interface ITransporter
    {
        Task Connect();

        Task Disconnect();

        Task Authenticate();

        IEnumerable<string> Read();

        Task Write(string message);

        Task Cancel(CancellationToken cancellationToken);
    }
}