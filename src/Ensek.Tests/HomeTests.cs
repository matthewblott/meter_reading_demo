namespace Ensek.Tests
{
  using System.Net;
  using System.Net.Http;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using Shouldly;
  using WebUI;

  [TestFixture]
  public class HomeTests
  {
    private TestWebApplicationFactory _factory;
    private HttpClient _client;
    
    [OneTimeSetUp]
    public void Init()
    {
      // Arrange
      _factory = new TestWebApplicationFactory();
      _client = _factory.CreateClient();
    }
    
    [Test]
    public async Task Should_navigate_home_successfully()
    {
      const string url = "/";
      
      // Act
      var response = await _client.GetAsync(url);

      // Assert
      response
        .EnsureSuccessStatusCode()
        .StatusCode
        .ShouldBe(HttpStatusCode.OK);
      
    }
    
    [OneTimeTearDown]
    public void TearDown()
    {
      _client.Dispose();
      _factory.Dispose();
    }
    
  }

}