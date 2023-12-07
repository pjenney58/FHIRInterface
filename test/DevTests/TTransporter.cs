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
                global::Transporters.Model.Transporter mllptransporter = TransporterFactory.CreateTransporter("mllp");
                Assert.NotNull(mllptransporter);

                global::Transporters.Model.Transporter tcptransporter = TransporterFactory.CreateTransporter("tcpip");
                Assert.NotNull(tcptransporter);

                global::Transporters.Model.Transporter resttransporter = TransporterFactory.CreateTransporter("rest");
                Assert.NotNull(resttransporter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}