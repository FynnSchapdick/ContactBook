using System.Net;
using System.Net.Http.Json;
using ContactBook.Api.Domain;
using ContactBook.Api.Endpoints;

namespace ContactBook.Api.IntegrationTests.EndpointTests;

public sealed class GetContactEndpointTests : BaseContactEndpointTests
{
    public GetContactEndpointTests(ContactBookApiFactory apiFactory) : base(apiFactory)
    {
    }
    
    [Fact]
    public async Task GetContact_ReturnsOkWithDto_WhenResourceWasFound()
    {
        //Arrange
        Contact contact = Contact.CreateNew("Dummy", "dummy@dummymail.com", "+492111234567");
        await ApiFactory.ArrangeForEndpointTesting(contact);
        
        // Act
        HttpResponseMessage response = await Client.GetAsync($"/contacts/{contact.Id}");
        ContactDto? dto = await response.Content.ReadFromJsonAsync<ContactDto>();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        Assert.NotNull(dto);
        Assert.Equal(contact.Id, dto.ContactId);
        Assert.Equal(contact.Email, dto.ContactEmail);
        Assert.Equal(contact.Name, dto.ContactName);
        Assert.Equal(contact.Mobile, dto.ContactMobile);
    }
    
    [Fact]
    public async Task GetContact_ReturnsNotFound_WhenResourceWasNotFound()
    {
        // Act
        HttpResponseMessage response = await Client.GetAsync($"/contacts/{Guid.NewGuid()}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }
}