using Collectors.Model;
using System.Diagnostics;

namespace DevTests
{
    public class TSchedular
    {
        public TSchedular()
        {
        }

        private int loopcount = 0;

        private void test_handler(object source, EventArgs args)
        {
            Assert.NotNull(source);
            Assert.NotNull(args);
            Debug.WriteLine($"loop: {loopcount++}");
        }

        [Fact]
        public async Task CycleTimer()
        {
            var _schedular = new Scheduler();
            _schedular.RegisterCallback(test_handler);
            _schedular.Seconds = 1;
            _schedular.StartTimer();

            while (loopcount < 61)
            {
                await Task.Delay(1000);
            }

            _schedular.Dispose();

            Assert.True(loopcount == 61);
        }

        [Fact]
        public async Task ResetTimer()
        {
            var _schedular = new Scheduler();
            _schedular.RegisterCallback(test_handler);
            _schedular.Seconds = 1;
            _schedular.StartTimer();

            while (loopcount < 31)
            {
                await Task.Delay(1000);
            }

            loopcount = 0;

            _schedular.ResetTimer();

            while (loopcount < 31)
            {
                await Task.Delay(1000);
            }

            _schedular.Dispose();

            Assert.True(loopcount == 31);
        }
    }
}