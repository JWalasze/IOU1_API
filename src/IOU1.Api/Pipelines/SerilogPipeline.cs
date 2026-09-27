using Elastic.Channels;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Serilog;

namespace IOU1.Api.Pipelines;

public static class SerilogPipeline
{
    public static void AddSerilogPipeline(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, services, cfg) => cfg
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt")
            .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], opts =>
            {
                opts.DataStream = new DataStreamName("logs", "iou1", "api");
                opts.BootstrapMethod = BootstrapMethod.Failure;
                opts.ConfigureChannel = channelOpts =>
                {
                    channelOpts.BufferOptions = new BufferOptions
                    {

                    };
                };

                opts.TextFormatting = new EcsTextFormatterConfiguration<LogEventEcsDocument>
                {
                    IncludeHost = false,
                    IncludeProcess = false,
                    IncludeUser = false,
                    IncludeActivityData = false
                };

            }));
    }
}
