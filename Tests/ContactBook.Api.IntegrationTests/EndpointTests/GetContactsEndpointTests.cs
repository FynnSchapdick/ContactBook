using System.Net;
using System.Net.Http.Json;
using ContactBook.Api.Domain;
using ContactBook.Api.Endpoints.GetContacts;

namespace ContactBook.Api.IntegrationTests.EndpointTests;

public sealed class GetContactsEndpointTests : BaseContactEndpointTests
{
    public GetContactsEndpointTests(ContactBookApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task GetContactsWithSearchFilter_ReturnsOkWithFilteredPaginatedResponse_WhenRequestIsValid()
    {
        // Arrange
        Contact[] contacts = {
            Contact.CreateNew("Dummy0", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy1", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy2", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy3", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy4", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy5", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy6", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy7", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy8", "dummy@dummymail.com", "+492111234567"),
            Contact.CreateNew("Dummy9", "dummy@dummymail.com", "+492111234567")
        };
        await ApiFactory.ArrangeForEndpointTesting(contacts);
        
        // Act
        HttpResponseMessage response = await Client.GetAsync($"/contacts?page=1&pageSize=10&searchFilter=Dummy1");
        PaginatedContactsResponse? paginatedContacts = await response.Content.ReadFromJsonAsync<PaginatedContactsResponse>();
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(paginatedContacts);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, paginatedContacts.Page);
        Assert.Equal(10, paginatedContacts.PageSize);
        Assert.Equal(1, paginatedContacts.TotalPages);
        Assert.Equal(10, paginatedContacts.TotalCount);
        Assert.Single(paginatedContacts.Contacts);
    }
}