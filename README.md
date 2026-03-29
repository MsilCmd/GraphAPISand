# GraphAPISand
A GraphAPI Sandbox
# Microsoft Graph Sample – Me and Messages

Small console app that uses Microsoft Graph to:

- Sign in with device code flow
- Call `/me` to get the signed-in user
- Call `/me/messages` to list the top 5 email messages

## Prerequisites

- .NET 8 SDK (or 6+)
- A Microsoft 365 tenant (dev or test)
- An app registration in Entra ID (Azure AD)

## Setup

1. Create an app registration in Entra ID:
   - Redirect URI type: `Public client/native (mobile & desktop)`
   - Redirect URI: `https://login.microsoftonline.com/common/oauth2/nativeclient`
   - Allow public client flows (device code)

2. Add API permissions:
   - `User.Read` (Delegated)
   - `Mail.Read` (Delegated)
   - Grant admin consent if needed.

3. Copy the **Application (client) ID** and paste it into `Program.cs`:

   ```csharp
   private const string TenantId = "common";
   // ...
   ClientId = "YOUR_CLIENT_ID_HERE",

