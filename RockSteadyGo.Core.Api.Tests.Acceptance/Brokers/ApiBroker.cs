using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
        }
    }
}