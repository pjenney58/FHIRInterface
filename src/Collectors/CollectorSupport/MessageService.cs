using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;

namespace CollectorSupport
{
    public sealed class MessageService
    {
        private readonly IStringLocalizer<MessageService> _localizer = null!;

        public MessageService(IStringLocalizer<MessageService> localizer) =>
            _localizer = localizer;

        public string? this[string key]
        {
            get =>
                _localizer[key] as LocalizedString;         
        }
    }
}

