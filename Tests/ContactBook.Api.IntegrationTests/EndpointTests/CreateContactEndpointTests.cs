using System.Net;
using System.Net.Http.Json;
using ContactBook.Api.Domain;
using ContactBook.Api.Endpoints.CreateContact;

namespace ContactBook.Api.IntegrationTests.EndpointTests;

public sealed class CreateContactEndpointTests : BaseContactEndpointTests
{
    public CreateContactEndpointTests(ContactBookApiFactory apiFactory) : base(apiFactory)
    {
    }
    
    [Fact]
    public async Task CreateContact_ReturnsCreated_WhenRequestIsValid()
    {
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/contacts", new CreateContactRequest("testname", "fschapdick@gmail.com", "+492111234567"));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }
    
    [Fact]
    public async Task CreateContact_ReturnsBadRequest_WhenNameIsNotValid()
    {
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/contacts", new CreateContactRequest("ab"));
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        Assert.Null(response.Headers.Location);
    }
    
    [Fact]
    public async Task CreateContact_ReturnsBadRequest_WhenEmailIsNotValid()
    {
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/contacts", new CreateContactRequest("abc", "fschapdick@@gmail.com"));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        Assert.Null(response.Headers.Location);
    }
    
    [Fact]
    public async Task CreateContact_ReturnsBadRequest_WhenMobileIsNotValid()
    {
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/contacts", new CreateContactRequest("abc", Mobile: "+499999999999"));
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        Assert.Null(response.Headers.Location);
    }
    
    [Fact]
    public async Task CreateContact_ReturnsConflict_WhenViolatingConstraint()
    {
        //Arrange
        await ApiFactory.ArrangeForEndpointTesting(Contact.CreateNew("Dummy", "dummy@dummymail.com", "+492111234567"));
        
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/contacts", new CreateContactRequest("Dummy", "dummy@dummymail.com", "+492111234567"));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Conflict,response.StatusCode);
        Assert.Null(response.Headers.Location);
    }
}