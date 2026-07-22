FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["../GenAIFundamentals/GenAILab03-AIIntegrationDemo/GenAILab03-AIIntegrationDemo.csproj", "./"]
RUN dotnet restore "GenAILab03-AIIntegrationDemo.csproj"
COPY ../GenAIFundamentals/GenAILab03-AIIntegrationDemo/. ./
RUN dotnet publish "GenAILab03-AIIntegrationDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "GenAILab03-AIIntegrationDemo.dll"]
