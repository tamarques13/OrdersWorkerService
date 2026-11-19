FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY OrdersWorkerService.csproj ./

RUN dotnet restore

COPY . ./

RUN dotnet publish OrdersWorkerService.csproj \
	--output /app/publish \
	--no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrdersWorkerService.dll"]
