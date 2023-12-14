using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;

namespace Transporters.Model
{
    public class TcpIpClient : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(TcpIpClient));
        private TCPClient client;

        public bool secure { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[] ApiKey { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public TcpIpClient()
        { }
    }
}