using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace Collectors.Messaging
{
    public sealed class MessageService
    {
        private readonly IStringLocalizer<MessageService> _localizer = null!;

        public MessageService(IStringLocalizer<MessageService> localizer) =>
            _localizer = localizer;

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string getstring(string key)
        {
            LocalizedString localizedString = _localizer[key];
            return localizedString;
        }

        public string this[string key]
        {
            get =>
                _localizer[key] as LocalizedString;
        }
    }
}