/*
 MIT License - MailAlert.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Support.Model
{
    public class MailAlert
    {
        private IConfiguration? config;

        public MailAlert()
        {
            SetConfig();
        }

        private void SetConfig()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("supportsettings.json")
               .Build();
        }

        public string SendAlert(string message)
        {
            try
            {
                var ipAddress = string.Empty;
                var settings = config?.GetRequiredSection("Mail") ?? throw new NullReferenceException("config is null");

                var location = settings["OrganizationName"];

                using (var client = new SmtpClient(settings["SmtpServerName"]))
                {
                    IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in iPHost.AddressList)
                    {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.ToString();
                            break;
                        }
                    }

                    var entries = settings["SmtpTargetAddress"]?
                        .Split(new char[] { ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.IsBodyHtml = false;
                        mailMessage.From = new MailAddress(settings["SmtpUserName"],
                                                        $"SynMedConfig Server Alert from {Dns.GetHostName()}/{ipAddress} at {location}");

                        foreach (var entry in entries)
                        {
                            mailMessage.To.Add(new MailAddress(entry));
                        }

                        mailMessage.Subject = $"Alert from {Environment.UserName} on IP {ipAddress}: Domain/Machine {Environment.UserDomainName} at {location}";
                        mailMessage.Body = $"{DateTime.Now}: {message}";

                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.EnableSsl = true;
                        client.Host = settings["SmtpServerName"];
                        client.Port = 587;
                        client.Credentials = new NetworkCredential(settings["SmtpUserName"], settings["SmtpUserPassword"]);
                        client.Timeout = 20000;

                        // Send
                        client.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to send email alert: {ex.Message}");
                throw;
            }

            return "SUCCESS";
        }
    }
}