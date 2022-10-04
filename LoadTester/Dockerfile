#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LoadTester/LoadTester.csproj", "LoadTester/"]
RUN dotnet restore "LoadTester/LoadTester.csproj"
COPY . .
WORKDIR "/src/LoadTester"
RUN dotnet build "LoadTester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoadTester.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoadTester.dll"]