using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientSpike
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient _client;

        public HttpClientWrapper(HttpClient client)
        {
            this._client = client;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _client.GetAsync(url);
        } 
    }

    public class Service
    {
        private IHttpClientWrapper _client;

        public Service(IHttpClientWrapper client)
        {
            this._client = client;
        }

        public void Execute()
        {
            _client.GetAsync("http://www.yahoo.com");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var collection = new ServiceCollection();
            collection.AddSingleton<Service>();
            collection.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
            var provider = collection.BuildServiceProvider();

            var service = provider.GetRequiredService<Service>();

            service.Execute();
        }
    }
}
