#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlazorApp1/Server/BlazorApp1.Server.csproj", "BlazorApp1/Server/"]
COPY ["BlazorApp1/Client/BlazorApp1.Client.csproj", "BlazorApp1/Client/"]
COPY ["BlazorApp1/Shared/BlazorApp1.Data.csproj", "BlazorApp1/Shared/"]
COPY ["BlazorApp1.Data/BlazorApp1.Domain.csproj", "BlazorApp1.Data/"]
#RUN dotnet workload restore "BlazorApp1/Client/BlazorApp1.Client.csproj"
#RUN dotnet restore "BlazorApp1/Server/BlazorApp1.Server.csproj"
COPY . .
WORKDIR "/src/BlazorApp1/Server"
RUN apt-get update
RUN apt-get install -y python3
RUN dotnet build "BlazorApp1.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorApp1.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorApp1.Server.dll"]