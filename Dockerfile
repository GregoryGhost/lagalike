FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Core/Lagalike.Telegram/Lagalike.Telegram.csproj", "Core/Lagalike.Telegram/"]
COPY ["Core/Lagalike.Telegram.Shared/Lagalike.Telegram.Shared.csproj", "Core/Lagalike.Telegram.Shared/"]

COPY ["Packages/ThingsStore/ThingsStore.csproj", "Packages/ThingsStore/"]
COPY ["Packages/Lagalike.GraphML.Parser/Lagalike.GraphML.Parser.csproj", "Packages/Lagalike.GraphML.Parser/"]
COPY ["Packages/PatrickStar.MVU/PatrickStar.MVU.csproj", "Packages/PatrickStar.MVU/"]

COPY ["Demos/Lagalike.Demo.ThingsStoreSystem/Lagalike.Demo.ThingsStoreSystem.csproj", "Demos/Lagalike.Demo.ThingsStoreSystem/"]
COPY ["Demos/Lagalike.Demo.TestPatrickStar.MVU/Lagalike.Demo.TestPatrickStar.MVU.csproj", "Demos/Lagalike.Demo.TestPatrickStar.MVU/"]
COPY ["Demos/Lagalike.Demo.DialogSystem/Lagalike.Demo.DialogSystem.csproj", "Demos/Lagalike.Demo.DialogSystem/"]
COPY ["Demos/Lagalike.Demo.CockSizer.MVU/Lagalike.Demo.CockSizer.MVU/Lagalike.Demo.CockSizer.MVU.csproj", "Demos/Lagalike.Demo.CockSizer.MVU/"]

RUN dotnet restore "Core/Lagalike.Telegram/Lagalike.Telegram.csproj"
COPY . .
WORKDIR "/src/Core/Lagalike.Telegram"
RUN dotnet build "Lagalike.Telegram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lagalike.Telegram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lagalike.Telegram.dll"]
