using Orleans.TestingHost;
using Xunit;

namespace Agenci.UnitTests;

public sealed class ClusterFixture : IDisposable
{
    public TestCluster Cluster { get; } = new TestClusterBuilder()
        .AddSiloBuilderConfigurator<TestSiloConfiguration>()
        .Build();
    
    public ClusterFixture()
    {
        Cluster.Deploy();
    }
    
    public void Dispose()
    {
        Cluster.StopAllSilos();
    }
}

file sealed class TestSiloConfiguration : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder
            .AddMemoryGrainStorage("orchestratorStore")
            .AddMemoryGrainStorage("parkingStore")
            .AddMemoryGrainStorage("driverStore");
    }
}

[CollectionDefinition(Name)]
public sealed class ClusterCollection : ICollectionFixture<ClusterFixture>
{
    public const string Name = nameof(ClusterCollection);
}
