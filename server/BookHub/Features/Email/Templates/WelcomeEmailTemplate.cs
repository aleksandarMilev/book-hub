namespace BookHub.Features.Email.Templates;

public static class WelcomeEmailTemplate
{
    public static string Build(string userName)
    {
        return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <title>Welcome to BookHub</title>
</head>
<body style=""margin:0;padding:0;background-color:#f5f5f5;font-family:Arial, Helvetica, sans-serif;"">
    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
        <tr>
            <td align=""center"" style=""padding: 40px 16px;"">
                <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px;background-color:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 2px 6px rgba(0,0,0,0.05);"">
                    <tr>
                        <td style=""background:linear-gradient(135deg,#4f46e5,#6366f1);padding:24px 32px;color:#ffffff;"">
                            <h1 style=""margin:0;font-size:24px;font-weight:700;"">Welcome to BookHub</h1>
                            <p style=""margin:8px 0 0;font-size:14px;opacity:0.9;"">
                                Your new reading home is ready. 📚
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding:24px 32px;color:#111827;font-size:14px;line-height:1.6;"">
                            <p style=""margin-top:0;"">Hi {System.Net.WebUtility.HtmlEncode(userName)},</p>

                            <p>
                                Thanks for signing up for <strong>BookHub</strong>! You can now explore books,
                                manage your library, and discover your next great read.
                            </p>

                            <p style=""margin:24px 0;"">
                                <a href=""https://your-bookhub-domain.example"" 
                                   style=""display:inline-block;padding:12px 24px;border-radius:999px;
                                          background-color:#4f46e5;color:#ffffff;text-decoration:none;
                                          font-weight:600;font-size:14px;"">
                                    Open BookHub
                                </a>
                            </p>

                            <p style=""margin:0;"">
                                Happy reading,<br />
                                <span style=""font-weight:600;"">The BookHub Team</span>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding:16px 32px;background-color:#f9fafb;color:#6b7280;font-size:12px;text-align:center;"">
                            You received this email because you created an account on BookHub.
                            If this wasn’t you, you can safely ignore this message.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }
}
