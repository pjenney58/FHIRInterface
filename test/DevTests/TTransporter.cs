using System.Net;
using Transporters.Model;
using Transporters.Interface;

namespace DevTests.Transporter
{
    public class TTransporter
    {
        public TTransporter()
        {
        }

        [Fact]
        public async Task Connect()
        {
            try
            {
                Transporters.Model.Transporter mllptransporter = TransporterFactory.Create("mllp");
                Assert.NotNull(mllptransporter);

                Transporters.Model.Transporter tcptransporter = TransporterFactory.Create("tcpip");
                Assert.NotNull(tcptransporter);

                Transporters.Model.Transporter resttransporter = TransporterFactory.Create("rest");
                Assert.NotNull(resttransporter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}