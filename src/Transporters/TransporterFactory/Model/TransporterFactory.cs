namespace Transporters.Model
{
    public static class TransporterFactory
    {
        public static Transporter Create(string transporterType)
        {
            switch (transporterType.ToLower())
            {
                case "mllp":
                //return new MllpClient();

                case "tcpip":
                //return new TcpIpClient();

                case "rest":
                //return new RestClient();

                default:
                    throw new Exception("Invalid transporter type");
            }
        }
    }
}