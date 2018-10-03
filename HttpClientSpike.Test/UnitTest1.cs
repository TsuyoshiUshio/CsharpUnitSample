using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace HttpClientSpike.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            var ExpectedURL = "https://www.yahoo.co.jp";

            var fixture = new Fixture();
            fixture.ExpectedContents = "hello";
            fixture.Setup();

            var client = fixture.Client;
            
            var result = await client.GetAsync(ExpectedURL);
            var resultString = await result.Content.ReadAsStringAsync();
            Assert.Equal(fixture.ExpectedContents, resultString); 
            Assert.Equal(ExpectedURL, fixture.ActualURL);
        }

        public class Fixture
        {
            public IHttpClientWrapper Client => _clientMock.Object;

            private Mock<IHttpClientWrapper> _clientMock;

            public Fixture()
            {
                _clientMock = new Mock<IHttpClientWrapper>();
            }

            public string ExpectedContents { get; set; }
            public string ActualURL { get; set; }

            public void Setup()
            {
                var message = new System.Net.Http.HttpResponseMessage();
                message.Content = new StringContent(ExpectedContents);

                _clientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(
                    message
                ).Callback<string>((url) => { ActualURL = url; });

            }
        }




    }
}
