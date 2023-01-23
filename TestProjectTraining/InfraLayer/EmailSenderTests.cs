namespace TestProjectTraining.InfraLayer
{
  using InfrastructureLayer.ExternalServices;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Text;
  using System.Threading.Tasks;
  using WireMock.RequestBuilders;
  using WireMock.ResponseBuilders;
  using WireMock.Server;
  using NSubstitute;

  public class EmailSenderTests
  {
    private WireMockServer _server;
    private IEmailSender _sut; 

    public EmailSenderTests()
    {
      _server = WireMockServer.Start();

      _server
        .Given(Request.Create().WithPath("/api/email").UsingPost())
        .RespondWith(
          Response.Create()
            .WithStatusCode(200)
            .WithBody(@"{ ""msg"": ""Email was send!"" }"));

      _server
        .Given(Request.Create().WithPath("/api/email").UsingGet())
        .RespondWith(
          Response.Create()
            .WithStatusCode(200)
            .WithBody(@"{ ""msg"": ""Email was send!"" }"));

      var httpClientFactory = Substitute.For<IHttpClientFactory>();
      httpClientFactory.CreateClient(null).ReturnsForAnyArgs(new HttpClient()
      {
        BaseAddress = new Uri(_server.Urls[0])
      });

      _sut = new EmailSender(httpClientFactory);
    }

    [Fact]
    public async Task SendEmail_ValidEmailData_Ok()
    {
      //arrange


      //act
      await _sut.SendEmail("my@email.com", "gfsdgfvsd@asss");

      //assert
      //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SendEmail_InValidEmailData_Ok()
    {
      //arrange


      //act
      await _sut.SendEmail("my@email.com", "gfsdgfvsd@asss");

      //assert
      //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
