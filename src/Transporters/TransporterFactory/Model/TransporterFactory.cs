using Transporter.Model;
using Transporter.Interface;
using System.Net.Sockets;
using Support.Model;

namespace Transporter.Model
{
    public class TransporterFactory
    {
        public TransporterFactory()
        { }

        public Transporter CreateTransporter(string transporterType)
        {
            switch (transporterType.ToLower())
            {
                case "mllp":
                    return new MllpClient();

                case "tcpip":
                    return new TcpIpClient();

                case "rest":
                    return new RestClient();

                default:
                    throw new Exception("Invalid transporter type");
            }
        }
    }
}