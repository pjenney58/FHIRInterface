using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataShapes.Model
{
    public enum ConnectionType
    {
        /// <summary>
        /// Disconnect after every completed transaction
        /// </summary>
        Stateless,

        /// <summary>
        /// Maintain a constant connection with the client
        /// </summary>
        Stateful
    }

    public enum DataProtocol
    {
        HL7v2,
        HL7v3,
        HL7_Fhirv4,
        HL7_Fhirv4b,
        HL7_Fhirv5,
        HL7_FhirvStu2,
        HL7_FhirvStu3,
        CDA,
        CCDA,
        DICOM,
        Unknown
    };

    public enum TransportPrototcol
    {
        Json,
        Xml,
        MMLP,
        PipeHat,
        Other
    };

    public enum NetworkProtocol
    {
        TCP,
        UDP,
        REST,
        HTTP,
        HTTPS
    };

    /// <summary>
    /// Collectors are modules that collect data from sources and passes it on for transformation into the Palisaid format.  There
    /// must be a specific collector for each target host and the host can use any protocols for data transfer.
    ///
    /// The CollectorConfig is a data record that describes a host.  Given the config, a collector will configure itself to do work.
    /// </summary>
    public class CollectorConfig : Entity
    {
        public string? TargetName { get; set; }
        public string? TargetUrl { get; set; }
        public Uri? TargetUri { get; set; }
        public string? TargetIp { get; set; }
        public string? TargetPort { get; set; }
        public string? ConnectionString { get; set; }

        public decimal TimerDurationMs { get; set; }
        public decimal TimerDurationSec { get; set; }
        public decimal TimerDurationMin { get; set; }
        public decimal TimerDurstionHr { get; set; }

        public Dictionary<string, string> HttpHeaders { get; set; }  = new();

        [NotMapped]
        public CancellationTokenSource? cancelTokenSrc { get; set; }

        //
        // The transport protocols may be different depending on the target system, for example, Epic delivers data in FHIR
        // but tackes v2.5 for input.
        //
        //  Inbound and outbound transmission is from Palisaid's perspective
        //
        //   {protocol}Out => Host reads from Palisaid
        //   {protocol}In  => Palisaid reads from host
        //
        // Example:
        //          DataProtocolId = DataProtocol.HL7_Fhirv4;
        //          TransportProtocolIn = TransportProtocol.Xml;
        //          NetworkProtocolIn = NetworkProtocol.HTTPS;
        //
        //          DataProtocolOut = DataProtocol.HL7v2;
        //          TransportProtocolIn = TransportProtocol.MMLP;
        //          NetworkProtocolIn = NetworkProtocol.TCP;
        //

        public DataProtocol DataProtocolOut { get; set; }
        public NetworkProtocol NetworkProtocolOut { get; set; }
        public TransportPrototcol TransportPrototcolOut { get; set; }

        public DataProtocol DataProtocolIn { get; set; }
        public NetworkProtocol NetworkProtocolIn { get; set; }
        public TransportPrototcol TransportPrototcolIn { get; set; }

        public string? username { get; set; }
        public string? password { get; set; }
        public string? apitoken { get; set; }

        public CollectorConfig()
        { }

		public CollectorConfig(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId)
        { }

        public async Task<string> GetData()
        { throw new NotImplementedException(nameof(GetData));  }

        public  async Task<string> SendData()
        { throw new NotImplementedException(nameof(SendData)); }

        public async Task<string> CancelRequest()
        { throw new NotImplementedException(nameof(CancelRequest)); }

        public async Task<string> RetryRequest()
        { throw new NotImplementedException(nameof(RetryRequest)); }

        public async Task<string> Connect()
        { throw new NotImplementedException(nameof(Connect)); }

        public async Task<string> Disconnect()
        { throw new NotImplementedException(nameof(Disconnect)); }

        //
        // A collector can manage multiple hosts just by changing the protocols and transport specifics.
        // Calling Reconfigure raises an event indicating that options have changed and itselfe needs to update
        //
        public async Task<string> Reset()
        { throw new NotImplementedException(nameof(Reset)); }

        public async Task<string> UpdateLog(string log)
        { throw new NotImplementedException(nameof(UpdateLog)); }

       
    }

    public enum LogRecordState
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Critical
    };

    public class CollectorLogRecord
    {
        [Key]
        public Guid EntityId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public LogRecordState State { get; set; }
        public string? TargetName { get; set; }
        public Guid CollectorId { get; set; }
        public Guid TenantId { get; set;  }
        public string? Message { get; set; }
    }
}

