using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using DynamicFilter.Tests.Benchmarks;
using Xunit.Abstractions;

namespace DynamicFilter.Tests;

public class BenchmarksRunner
{
    private readonly ITestOutputHelper mLogger;

    public BenchmarksRunner(ITestOutputHelper logger)
    {
        mLogger = logger;
    }

    [Fact]
    public void RunDynamicFilterBenchmarks()
    {
        RunBenchmark<DynamicFilterBenchmarks>();
    }

    private void RunBenchmark<TBenchmark>()
    {
        var summary = BenchmarkRunner.Run<TBenchmark>();

        var logger = new AccumulationLogger();

        MarkdownExporter.Console.ExportToLog(summary, logger);

        mLogger.WriteLine(logger.GetLog());
    }
}