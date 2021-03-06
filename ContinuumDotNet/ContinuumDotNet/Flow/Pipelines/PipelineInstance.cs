﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Api;
using ContinuumDotNet.Api.Flow;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Flow.Pipelines;
using Newtonsoft.Json;
using RestSharp;

namespace ContinuumDotNet.Flow.Pipelines
{
    public class PipelineInstance : IPipelineInstance
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("actions")]
        public dynamic Actions { get; set; }
        public IContinuumConnection Connection { get; set; }

        public PipelineInstance(IContinuumConnection continuumConnection)
        {
            Connection = continuumConnection;
        }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string FlightCode { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }
    }
}
