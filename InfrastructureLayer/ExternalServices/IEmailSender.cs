namespace InfrastructureLayer.ExternalServices
{
  using System.Threading.Tasks;

  public interface IEmailSender
  {
    Task SendEmail(string toAddress, string message);
  }
}