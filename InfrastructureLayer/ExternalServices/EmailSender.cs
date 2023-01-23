namespace InfrastructureLayer.ExternalServices
{
  using InfrastructureLayer.ExternalServices.Dto;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net.Http;
  using System.Net.Http.Json;
  using System.Text;
  using System.Threading.Tasks;

  public class EmailSender : IEmailSender
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public EmailSender(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    public async Task SendEmail(string toAddress, string message)
    {
      var client = _httpClientFactory.CreateClient(nameof(EmailSender));

      HttpResponseMessage responseMessage = await client.PostAsync("/api/email", JsonContent.Create(new Email
      {
        Message = message,
        To = toAddress
      }));

      responseMessage.EnsureSuccessStatusCode();
    }
  }
}
