FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.19 AS base
WORKDIR /app
EXPOSE 5058

ENV ASPNETCORE_URLS=http://+:5058

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
ARG configuration=Release
WORKDIR /src
COPY ["TherapistService.csproj", "./"]
RUN dotnet restore "TherapistService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TherapistService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TherapistService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TherapistService.dll"]
