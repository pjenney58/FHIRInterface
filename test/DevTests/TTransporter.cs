using System.Net;
using Transporter.Model;
using Transporter.Interface;

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
                global::Transporter.Model.Transporter mllptransporter = TransporterFactory.CreateTransporter("mllp");
                Assert.NotNull(mllptransporter);

                global::Transporter.Model.Transporter tcptransporter = TransporterFactory.CreateTransporter("tcpip");
                Assert.NotNull(tcptransporter);

                global::Transporter.Model.Transporter resttransporter = TransporterFactory.CreateTransporter("rest");
                Assert.NotNull(resttransporter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}