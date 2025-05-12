@echo off
dotnet pack CleanKit.Net.Outbox\CleanKit.Net.Outbox.csproj -c Release
dotnet pack CleanKit.Net.Outbox.Persistence\CleanKit.Net.Outbox.Persistence.csproj -c Release
dotnet pack CleanKit.Net.DependencyInjection\CleanKit.Net.DependencyInjection.csproj -c Release
dotnet pack CleanKit.Net.Domain\CleanKit.Net.Domain.csproj -c Release
dotnet pack CleanKit.Net.Idempotency\CleanKit.Net.Idempotency.csproj -c Release
dotnet pack CleanKit.Net.Idempotency.Persistence\CleanKit.Net.Idempotency.Persistence.csproj -c Release
dotnet pack CleanKit.Net.Persistence\CleanKit.Net.Persistence.csproj -c Release
dotnet pack CleanKit.Net.Presentation\CleanKit.Net.Presentation.csproj -c Release
dotnet pack CleanKit.Net.Utils\CleanKit.Net.Utils.csproj -c Release
dotnet pack CleanKit.Net.Application\CleanKit.Net.Application.csproj -c Release
color 9
echo COMPLETED!
pause