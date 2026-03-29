using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace GraphSample
{
    internal class Program
    {
        // Change this to your tenant ID if you want to lock it down
        private const string TenantId = "common";

        // Scopes required for this sample
        private static readonly string[] Scopes = new[]
        {
            "User.Read",
            "Mail.Read"
        };

        static async Task Main(string[] args)
        {
            Console.WriteLine("Microsoft Graph sample starting...");

            // Device code auth – great for dev/test tenants
            var options = new DeviceCodeCredentialOptions
            {
                TenantId = TenantId,
                ClientId = "YOUR_CLIENT_ID_HERE",
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.CompletedTask;
                }
            };

            var credential = new DeviceCodeCredential(options);

            var graphClient = new GraphServiceClient(credential, Scopes);

            try
            {
                // Get basic profile
                var me = await graphClient.Me.GetAsync();
                Console.WriteLine($"Hello, {me?.DisplayName} ({me?.UserPrincipalName})");

                // Get top 5 messages
                var messages = await graphClient.Me.Messages.GetAsync(config =>
                {
                    config.QueryParameters.Top = 5;
                    config.QueryParameters.Select = new[] { "subject", "from", "receivedDateTime" };
                    config.QueryParameters.Orderby = new[] { "receivedDateTime DESC" };
                });

                Console.WriteLine();
                Console.WriteLine("Top 5 messages:");
                Console.WriteLine("----------------");

                if (messages?.Value != null)
                {
                    foreach (var msg in messages.Value)
                    {
                        Console.WriteLine($"Subject: {msg.Subject}");
                        Console.WriteLine($"From:    {msg.From?.EmailAddress?.Name} <{msg.From?.EmailAddress?.Address}>");
                        Console.WriteLine($"Received:{msg.ReceivedDateTime}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No messages found.");
                }
            }
            catch (ODataError ex)
            {
                Console.WriteLine("Graph API error:");
                Console.WriteLine(ex.Error?.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
