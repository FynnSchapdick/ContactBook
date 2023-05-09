namespace ContactBook.Api.IntegrationTests.EndpointTests;

public abstract class BaseContactEndpointTests : IClassFixture<ContactBookApiFactory>
{
    protected ContactBookApiFactory ApiFactory { get; }
    protected HttpClient Client { get; }

    protected BaseContactEndpointTests(ContactBookApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = apiFactory.CreateDefaultClient();
    }
}