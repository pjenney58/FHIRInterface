using System;

namespace Collectors.Model
{
	public enum TransportType
	{
		http,
		https,
		http2,
		socket,
		securesocket,
		ftp,
		sftp
	}

	public enum TransportProtocol
	{
		plaintext,
		xml,
		json,
		file
	}

	public enum TargetAuthType
	{
		none,
		simple,
		apikey,
		token,
		twofactor
	}

	public class TargetConfiguration
	{
		public string? TargetId { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }

		public TransportType TransportType { get; set; }
		public TransportProtocol TransportProtocol { get; set; }

		public string? username { get; set; }
		public string? password { get; set; }
		public string? apikey { get; set; }
		public string? token { get; set; }

		DateTimeOffset createdate { get; set; }
		DateTimeOffset lastupdate { get; set; }
	}
}

