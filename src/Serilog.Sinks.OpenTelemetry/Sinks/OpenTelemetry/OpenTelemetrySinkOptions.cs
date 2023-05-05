﻿// Copyright © Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.OpenTelemetry;

/// <summary>
/// Initialization options for <see cref="OpenTelemetrySink"/>.
/// </summary>
public class OpenTelemetrySinkOptions
{
    internal const string DefaultEndpoint = "http://localhost:4317/v1/logs";
    internal const OtlpProtocol DefaultProtocol = OtlpProtocol.GrpcProtobuf;

    const IncludedData DefaultIncludedData = IncludedData.MessageTemplateTextAttribute |
                                             IncludedData.TraceIdField | IncludedData.SpanIdField;

    /// <summary>
    /// The full URL of the OTLP exporter endpoint.
    /// </summary>
    public string Endpoint { get; set; } = DefaultEndpoint;

    /// <summary>
    /// Custom HTTP message handler.
    /// </summary>
    public HttpMessageHandler? HttpMessageHandler { get; set; }

    /// <summary>
    /// The OTLP protocol to use.
    /// </summary>
    public OtlpProtocol Protocol { get; set; } = DefaultProtocol;

    /// <summary>
    /// A attributes of the resource attached to the logs generated by the sink. The values must be simple primitive
    /// values: integers, doubles, strings, or booleans. Other values will be silently ignored.
    /// </summary>
    public IDictionary<string, object> ResourceAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Which fields should be included in the log events generated by the sink. The default is to include <c>TraceId</c>
    /// and <c>SpanId</c> when <see cref="Activity.Current"/> is not null, and <c>message_template.text</c>.
    /// </summary>
    public IncludedData IncludedData { get; set; } = DefaultIncludedData;

    /// <summary>
    /// Headers to send with network requests.
    /// </summary>
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Supplies culture-specific formatting information, or null.
    /// </summary>
    public IFormatProvider? FormatProvider { get; set; }

    /// <summary>
    /// The minimum level for events passed through the sink. The default value is to not restrict events based on
    /// level. Ignored when <see cref="LevelSwitch"/> is specified.
    /// </summary>
    public LogEventLevel RestrictedToMinimumLevel { get; set; } = LevelAlias.Minimum;

    /// <summary>
    /// A switch allowing the pass-through minimum level
    /// to be changed at runtime.
    /// </summary>
    public LoggingLevelSwitch? LevelSwitch { get; set; }
}

/// <summary>
/// Options type for controlling batching behavior.
/// </summary>
public class BatchedOpenTelemetrySinkOptions : OpenTelemetrySinkOptions
{
    const int DefaultBatchSizeLimit = 1000, DefaultPeriodSeconds = 2, DefaultQueueLimit = 100000;

    /// <summary>
    /// Options that control the sending of asynchronous log batches. When <c>null</c> a batch size of 1 is used.
    /// </summary>
    public PeriodicBatchingSinkOptions BatchingOptions { get; } = new()
    {
        EagerlyEmitFirstEvent = true,
        BatchSizeLimit = DefaultBatchSizeLimit,
        Period = TimeSpan.FromSeconds(DefaultPeriodSeconds),
        QueueLimit = DefaultQueueLimit
    };
}
