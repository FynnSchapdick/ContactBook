using System.Net;
using System.Net.Http.Json;
using ContactBook.Api.Domain;
using ContactBook.Api.Endpoints.UpdateContact;

namespace ContactBook.Api.IntegrationTests.EndpointTests;

public sealed class UpdateContactEndpointTests : BaseContactEndpointTests
{
    public UpdateContactEndpointTests(ContactBookApiFactory apiFactory) : base(apiFactory)
    {
    }
    
    [Fact]
    public async Task UpdateContact_ReturnsOk_WhenRequestIsValid()
    {
        //Arrange
        Contact contact = Contact.CreateNew("Dummy", "dummy@dummymail.com", "+492111234567");
        await ApiFactory.ArrangeForEndpointTesting(contact);
        
        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/contacts/{contact.Id}", new UpdateContactRequest("testname", "fschapdick@gmail.com", "+492111234567"));
        var content = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }
}