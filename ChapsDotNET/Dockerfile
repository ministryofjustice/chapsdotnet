#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChapsDotNET/ChapsDotNET.csproj", "ChapsDotNET/"]
RUN dotnet restore "ChapsDotNET/ChapsDotNET.csproj"
COPY . .
WORKDIR "/src/ChapsDotNET"
RUN dotnet build "ChapsDotNET.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChapsDotNET.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChapsDotNET.dll"]