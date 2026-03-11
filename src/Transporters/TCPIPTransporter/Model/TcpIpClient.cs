using PalisaidMeta.Model;
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;

namespace Transporters.Model
{
    public class TcpIpClient : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(TcpIpClient));
        private TCPClient? client; // Initialize the client field with a default value

        public bool secure { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[]? ApiKey { get; set; } = new byte[0]; // Initialize the ApiKey property with an empty byte array
        public X509Certificate2 Certificate { get; set; } = new X509Certificate2(); // Initialize the Certificate property with a default certificate

        public TcpIpClient(CollectorConfig cconfig, Guid guid, string name, string description)
                        : base(cconfig, guid, name, description)
        {
            logger = new BaseEventLogger(name);

            if (secure)
            {
                //Certificate = new X509Certificate2(cconfig.CertificatePath, cconfig.CertificatePassword);
            }
        }

        /*
        public override async Task<IEnumerable<string?>> Read()
        {
            if (cconfig == null)
            {
                logger.ReportError("Config is null");
                yield return default;
            }

            using (client = new TCPClient(cconfig.TargetIp, cconfig.TargetPort))
            {
                if (client.IsSecure)
                {
                    //client.Certificate = Certificate;
                }

                using (client = new TCPClient())
                {
                    logger.ReportInfo($"Connected to {cconfig.TargetIp}:{cconfig.TargetPort}");

                    while (client.CanRead)
                    {
                        var data = client.Read();
                        if (data != null)
                        {
                            yield return data;
                        }
                    }
                }
            }
        }
        */
    }
}