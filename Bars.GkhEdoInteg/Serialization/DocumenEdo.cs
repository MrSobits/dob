namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    public sealed class DocumenEdo
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
    }
}
