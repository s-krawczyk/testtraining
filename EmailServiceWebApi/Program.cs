using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/api/email", (Email email) =>
{
  Console.WriteLine($"Got email => [{JsonSerializer.Serialize(email)}]");
  return email;
});

app.Run();

public class Email
{
  public string To { get; set; }
  public string Message { get; set; }
}