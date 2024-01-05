using System.Net;
using Transformers.Model;
using Transformers.Interface;
using Transporters.Model;
using Support.Model;
using DataShapes.Model;
using EasyNetQ;
using System.Diagnostics;

namespace DevTests.MessageQueue
{
    public class TestMessageQueue : PPM_MessageQueue
    {
            public TestMessageQueue(Guid tenantid, string commandbus, string payloadbus) 
                    : base(tenantid, commandbus, payloadbus)
            {
                Debug.WriteLine("Constructed TestMessageQueue");
            }

            protected override void RegisterCommmandHandler(MQCommandHandler mqcommandhandler)
            {
                Debug.WriteLine("Registering Command Handler");
            }

            protected override void RegisterTransformHandler(MQTransformHandler mqtransformhandler)
            {
                Debug.WriteLine("Registering Transform Handler");
            }

            protected override void WaitForCommandRequest()
            {
                Debug.WriteLine("Waiting for Command Request");
            }

            protected override void WaitForTransformRequest()
            {
                Debug.WriteLine("Waiting for Transform Request");
            }
        }

    public class TMessageQueue
    {
        public TMessageQueue()
        {
            // Check that RabbitMQ is running on localhost
            try
            {
                var bus = RabbitHutch.CreateBus("host=localhost");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Assert.Fail("RabbitMQ is not running on localhost");
            }
        }
        
        
        [Fact]
        public async Task CheckMessageQueue()
        {
            var tenantid = Guid.NewGuid();
            var commandbus = "commandbus";
            var payloadbus = "payloadbus";

            var mq = new TestMessageQueue(tenantid, commandbus, payloadbus);
            Assert.NotNull(mq);
        }
        
        [Fact]
        public async Task CheckCommands()
        {

        }

        [Fact]
        public async Task CheckTransforms()
        {
        }

    }

}