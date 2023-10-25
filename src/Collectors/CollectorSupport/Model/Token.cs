/*
 * Software License - Token.cs
 *
 *  Copyright (c) 2016 - 2022 by Sand Drift Software, LLC
 *
 * Permission is hereby granted, in consideration of a license fee or agreement,
   * to any person obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute,
 * and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 *The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Hl7Harmonizer.Adapters.Interface;

namespace Hl7Harmonizer.Transport.Model
{
    [Serializable]
    public class Token : IDisposable
    {
        private IBaseEventLogger eventLogger = new BaseEventLogger("Token");

        [XmlElement("accesstoken")]
        [JsonPropertyName("accesstoken")]
        public string? AccessToken { get; set; }

        [XmlElement("token_type")]
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [XmlElement("expires_in")]
        [JsonPropertyName("expires_in")]
        public double ExpiresInSeconds { get; set; }

        [XmlElement("refreshtoken")]
        [JsonPropertyName("refreshtoken")]
        public string? RefreshToken;

        [XmlElement("expiration")]
        [JsonPropertyName("expiration")]
        public DateTimeOffset Expiration { get; set; }

        private string? username;
        private string? password;

        public string BaseUri { get; set; } = "/";

        // TODO: Remove Hardcoded Azure Subscription references
        // TODO: Add Azure subscription references to management system
        private const string AzureSubscriptionKey = "Ocp-Apim-Subscription-Key";

        private const string AzureSubscriptionValue = "Ocp-Apim-Subscription-Key";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Token(string token, string refreshtoken, DateTimeOffset expiration, string baseurl)
        {
            TokenType = "bearer";
            AccessToken = token;
            RefreshToken = refreshtoken;
            Expiration = expiration.ToLocalTime();
            BaseUri = baseurl;
        }

        public Token()
        {
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                AccessToken = null;
                RefreshToken = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Token()
        {
            Dispose();
        }
    }
}