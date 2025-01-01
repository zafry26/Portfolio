﻿using DotNet.Testcontainers.Containers;
using BackendUnitTests.Constants;
using BackendUnitTests.Models;
using BackendUnitTests.Utils;
using Testcontainers.MsSql;

namespace BackendUnitTests.Services;

public class MsSqlContainerManager
{
    private IContainer? _container;

    public async Task<ContainerInfo> StartAsync()
    {
        TestLogUtils.WriteProgressMessage("Starting the MsSql container...");

        var containerName = "arrtodo_integration_test_mssql_" + Guid.NewGuid().ToString("D");

        var containerBuilder = new MsSqlBuilder()
            .WithName(containerName)
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithExposedPort(Defaults.MsSqlPort)
            .WithPassword(Defaults.DbPassword)
            .WithAutoRemove(true)
            .WithCleanUp(true);

        _container = containerBuilder.Build();

        using (var cancellationTokenSource = new CancellationTokenSource(Defaults.MsSqlStartTimeout))
        {
            var cancellationToken = cancellationTokenSource.Token;
            await _container.StartAsync(cancellationToken);
        }

        var containerHostPort = _container.GetMappedPublicPort(Defaults.MsSqlPort);

        TestLogUtils.WriteProgressMessage($"The MsSql container started successfully ({containerName})");

        return new ContainerInfo(containerHostPort, _container.Hostname);
    }

    public Task StopAsync(CancellationToken ct = default)
    {
        TestLogUtils.WriteProgressMessage($"The container is about to stop ({_container?.Name}).");

        if (_container != null)
        {
            _ = Task.Run(() => _container?.DisposeAsync(), ct);
        }

        return Task.CompletedTask;
    }
}