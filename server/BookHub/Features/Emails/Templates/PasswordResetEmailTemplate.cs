namespace BookHub.Features.Emails.Templates;

public static class PasswordResetEmailTemplate
{
    public static string Build(string resetUrl)
    {
        return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <title>Reset your BookHub password</title>
</head>
<body style=""margin:0;padding:0;background-color:#f5f5f5;font-family:Arial, Helvetica, sans-serif;"">
    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
        <tr>
            <td align=""center"" style=""padding: 40px 16px;"">
                <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px;background-color:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 2px 6px rgba(0,0,0,0.05);"">
                    <tr>
                        <td style=""background:linear-gradient(135deg,#4f46e5,#6366f1);padding:24px 32px;color:#ffffff;"">
                            <h1 style=""margin:0;font-size:24px;font-weight:700;"">Reset your password</h1>
                            <p style=""margin:8px 0 0;font-size:14px;opacity:0.9;"">
                                Click the button below to set a new password.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding:24px 32px;color:#111827;font-size:14px;line-height:1.6;"">
                            <p style=""margin-top:0;"">
                                If you requested a password reset, use the link below. If you didn’t, you can ignore this email.
                            </p>

                            <p style=""margin:24px 0;"">
                                <a href=""{resetUrl}""
                                   style=""display:inline-block;padding:12px 24px;border-radius:999px;
                                          background-color:#4f46e5;color:#ffffff;text-decoration:none;
                                          font-weight:600;font-size:14px;"">
                                    Reset password
                                </a>
                            </p>

                            <p style=""margin:0;color:#6b7280;font-size:12px;"">
                                This link expires after a short time.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding:16px 32px;background-color:#f9fafb;color:#6b7280;font-size:12px;text-align:center;"">
                            If you didn’t request this, no action is needed.
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