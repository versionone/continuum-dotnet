using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Flow.Artifacts;
using Newtonsoft.Json;

namespace ContinuumDotNet.Flow.Artifacts
{
    public class Artifact : IArtifact
    {
        [JsonProperty("artifact_id")]
        public string Id { get; set; }
        [JsonProperty("revision")]
        public int Revision { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("created_dt")]
        public DateTime CreationDateInUtc { get; set; }
    }
}
