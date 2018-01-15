using System;
using upload_api.Controllers;
using Xunit;

namespace upload_api.Test
{
    public class ValuesControllerTest
    {
        [Fact]
        public void gets_single_value()
        {
            Assert.Equal("value", new ValuesController().Get(0));
        }
    }
}
