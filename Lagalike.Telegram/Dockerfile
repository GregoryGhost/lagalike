FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Lagalike.Telegram/Lagalike.Telegram.csproj", "Lagalike.Telegram/"]
RUN dotnet restore "Lagalike.Telegram/Lagalike.Telegram.csproj"
COPY . .
WORKDIR "/src/Lagalike.Telegram"
RUN dotnet build "Lagalike.Telegram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lagalike.Telegram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lagalike.Telegram.dll"]
