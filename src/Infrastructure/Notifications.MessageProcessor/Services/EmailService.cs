namespace Notifications.MessageProcessor.Services
{
    public class EmailService : IEmailService
    {
        #region Private variables

        private readonly EmailConfig _emailConfig;

        #endregion

        #region Constructors

        public EmailService(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        #endregion

        #region Public methods

        public async Task SendWelcomeEmailAsync(string toName, string toAddress, string password)
        {
            try
            {
                (string subject, string body) = UserEmail(toName, toAddress, password);

                await SendEmail(toName, toAddress, subject, body);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task SendUserProfileUpdatedEmailAsync(string toName, string toAddress)
        {
            try
            {
                (string subject, string body) = UserUpdateEmail(toName);

                await SendEmail(toName, toAddress, subject, body);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task SendPasswordResetEmailAsync(string toName, string toAddress, string password)
        {
            try
            {
                (string subject, string body) = UserResetPasswordEmail(toName, password);
                await SendEmail(toName, toAddress, subject, body);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task SendPasswordChangedEmailAsync(string toName, string toAddress, string password)
        {
            try
            {
                (string subject, string body) = UserPasswordEmail(toName);
                await SendEmail(toName, toAddress, subject, body);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task SendClientEmailAsync(string toName, string toAddress)
        {
            try
            {
                (string subject, string body) = ClientEmail(toName);
                await SendEmail(toName, toAddress, subject, body);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        #endregion

        #region Private methods
        private async Task SendEmail(string toName, string toAddress, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.Address));
            message.To.Add(new MailboxAddress(toName, toAddress));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(body)
            };

            using var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, true);

            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

        }

        private static (string, string) UserEmail(string toName, string email, string password)
        {
            string subject = "Auto Moreira: Registo de utilizador";
            string body = "<p style=\"text-align:center\"><img alt=\"\" src=\"https://auto-moreira-app.onrender.com/static/media/logo.9ebafe1ed9a8e5bc22b3.png\" style=\"height:100px; width:140px\" /></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:12px\">Bem-vindo(a)&nbsp;<strong>{toName}</strong>. </span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:12px\">Credenciais de acesso:</span></span></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:12px\"><strong>Email: </strong>{email}</span></span></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:12px\"><strong>Palavra-passe: </strong>{password}</span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\">Atentamente,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\"><strong>Equipa Auto Moreira</strong></span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📞 (+351) 231 472 555</span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📧 <a href=\"mailto:automoreiraportugal@gmail.com\">automoreiraportugal@gmail.com</a></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">🏎&nbsp;<a href=\"https://auto-moreira-app.onrender.com/\">autoMoreira</a></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<h3 style=\"text-align:center\"><span style=\"font-size:9px\">2024 @AutoMoreira | Todos os Direitos Reservados.</span></h3>";

            return (subject, body);
        }

        private static (string, string) UserUpdateEmail(string toName)
        {
            string subject = $"Auto Moreira: Atualização do registo de utilizador";
            string body = "<p style=\"text-align:center\"><img alt=\"\" src=\"https://auto-moreira-app.onrender.com/static/media/logo.9ebafe1ed9a8e5bc22b3.png\" style=\"height:100px; width:140px\" /></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Estimado(a) <strong>{toName}</strong>,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Esta mensagem confirma que os seus dados foram atualizados com sucesso. </span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Se n&atilde;o requisitou nenhuma altera&ccedil;&atilde;o dos seus dados, por favor&nbsp;</span></span></span><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">contacte-nos por chamada telef&oacute;nica ou em resposta a este e-mail.</span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\">Atentamente,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\"><strong>Equipa Auto Moreira</strong></span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📞 (+351) 231 472 555</span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📧 <a href=\"mailto:automoreiraportugal@gmail.com\">automoreiraportugal@gmail.com</a></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">🏎&nbsp;<a href=\"https://auto-moreira-app.onrender.com/\">autoMoreira</a></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<h3 style=\"text-align:center\"><span style=\"font-size:9px\">2024 @AutoMoreira | Todos os Direitos Reservados.</span></h3>";

            return (subject, body);
        }

        private static (string, string) UserResetPasswordEmail(string toName, string password)
        {
            string subject = $"Auto Moreira: Nova palavra-passe";
            string body = "<p style=\"text-align:center\"><img alt=\"\" src=\"https://auto-moreira-app.onrender.com/static/media/logo.9ebafe1ed9a8e5bc22b3.png\" style=\"height:100px; width:140px\" /></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Estimado(a) <strong>{toName}</strong>,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Esta mensagem confirma o seu pedido de uma nova palavra-passe. </span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:12px\">Nova palavra-passe:</span></span></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:12px\"><strong>Palavra-passe: </strong>{password}</span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\">Atentamente,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\"><strong>Equipa Auto Moreira</strong></span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📞 (+351) 231 472 555</span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📧 <a href=\"mailto:automoreiraportugal@gmail.com\">automoreiraportugal@gmail.com</a></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">🏎&nbsp;<a href=\"https://auto-moreira-app.onrender.com/\">autoMoreira</a></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<h3 style=\"text-align:center\"><span style=\"font-size:9px\">2024 @AutoMoreira | Todos os Direitos Reservados.</span></h3>";

            return (subject, body);
        }

        private static (string, string) UserPasswordEmail(string toName)
        {
            string subject = $"Auto Moreira: Atualização da palavra-passe";
            string body = "<p style=\"text-align:center\"><img alt=\"\" src=\"https://auto-moreira-app.onrender.com/static/media/logo.9ebafe1ed9a8e5bc22b3.png\" style=\"height:100px; width:140px\" /></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Estimado(a) <strong>{toName}</strong>,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Esta mensagem confirma que a palavra passe foi atualizada com sucesso. </span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Se n&atilde;o requisitou nenhuma altera&ccedil;&atilde;o dos seus dados, por favor&nbsp;</span></span></span><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">contacte-nos por chamada telef&oacute;nica ou em resposta a este e-mail.</span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\">Atentamente,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\"><strong>Equipa Auto Moreira</strong></span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📞 (+351) 231 472 555</span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📧 <a href=\"mailto:automoreiraportugal@gmail.com\">automoreiraportugal@gmail.com</a></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">🏎&nbsp;<a href=\"https://auto-moreira-app.onrender.com/\">autoMoreira</a></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<h3 style=\"text-align:center\"><span style=\"font-size:9px\">2024 @AutoMoreira | Todos os Direitos Reservados.</span></h3>";

            return (subject, body);
        }

        private static (string, string) ClientEmail(string toName)
        {
            string subject = $"Auto Moreira: Recebemos a sua mensagem {toName}";
            string body = "<p style=\"text-align:center\"><img alt=\"\" src=\"https://auto-moreira-app.onrender.com/static/media/logo.9ebafe1ed9a8e5bc22b3.png\" style=\"height:100px; width:140px\" /></p>";
            body += $"<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Estimado(a) <strong>{toName}</strong>,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Agradecemos desde j&aacute; o interesse que manifestou em n&oacute;s.</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">Recebemos a sua mensagem e entraremos&nbsp;em&nbsp;contacto&nbsp;consigo assim que poss&iacute;vel para esclarecer todas as suas quest&otilde;es.</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"color:#000000\">No caso de ter alguma quest&atilde;o que pretenda ver esclarecida no imediato, n&atilde;o hesite em contactar-nos, por chamada telef&oacute;nica ou em resposta a este e-mail.</span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\">Atentamente,</span></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:11px\"><span style=\"color:#000000\"><strong>Equipa Auto Moreira</strong></span></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📞 (+351) 231 472 555</span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">📧 <a href=\"mailto:automoreiraportugal@gmail.com\">automoreiraportugal@gmail.com</a></span></span></p>";
            body += "<p style=\"text-align:center\"><span style=\"font-size:12px\"><span style=\"font-family:Arial,Helvetica,sans-serif\">🏎&nbsp;<a href=\"https://auto-moreira-app.onrender.com/\">autoMoreira</a></span></span></p>";
            body += "<p style=\"text-align:center\">&nbsp;</p>";
            body += "<h3 style=\"text-align:center\"><span style=\"font-size:9px\">2024 @AutoMoreira | Todos os Direitos Reservados.</span></h3>";

            return (subject, body);
        }

        #endregion
    }
}
