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

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateContact_ReturnsNotFound_WhenResourceWasNotFound()
    {
        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/contacts/{Guid.NewGuid()}", new UpdateContactRequest("testname", "fschapdick@gmail.com", "+492111234567"));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenNameIsNotValid()
    {
        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/contacts/{Guid.NewGuid()}", new UpdateContactRequest("ab"));
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenEmailIsNotValid()
    {
        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/contacts/{Guid.NewGuid()}", new UpdateContactRequest("abc", "fschapdick@@gmail.com"));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenMobileIsNotValid()
    {
        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/contacts/{Guid.NewGuid()}", new UpdateContactRequest("abc", Mobile: "+499999999999"));
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
    }
}