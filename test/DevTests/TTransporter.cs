using System.Net;
using Transporter.Model;
using Transporter.MLLP.Model;
using Transporter.Interface;

namespace DevTests
{
    internal class TTransporter
    {
        public TTransporter()
        {
        }

        public async Task Connect()
        {
            try
            {
                ITransporter mllptransporter = new MllpClient();

                Assert.NotNull(mllptransporter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}