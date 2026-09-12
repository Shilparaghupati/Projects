FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["CanteenWeb.csproj", "./"]

RUN dotnet restore "CanteenWeb.csproj"

COPY . .

RUN dotnet publish "CanteenWeb.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "CanteenWeb.dll"]