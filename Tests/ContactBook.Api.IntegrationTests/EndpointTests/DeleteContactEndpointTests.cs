using System.Net;
using ContactBook.Api.Domain;

namespace ContactBook.Api.IntegrationTests.EndpointTests;

public sealed class DeleteContactEndpointTests : BaseContactEndpointTests
{
    public DeleteContactEndpointTests(ContactBookApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task DeleteContact_ReturnsOk_WhenResourceWasFound()
    {
        // Arrange
        Contact contact = Contact.CreateNew("Dummy", "dummy@dummymail.com", "+492111234567");
        await ApiFactory.ArrangeForEndpointTesting(contact);
        
        // Act
        HttpResponseMessage response = await Client.DeleteAsync($"/contacts/{contact.Id}");
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteContact_ReturnsNotFound_WhenResourceWasNotFound()
    {
        // Act
        HttpResponseMessage response = await Client.DeleteAsync($"/contacts/{Guid.NewGuid()}");
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }
}