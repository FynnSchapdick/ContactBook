FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Source/ContactBook.Api/ContactBook.Api.csproj", "Source/ContactBook.Api/"]
RUN dotnet restore "Source/ContactBook.Api/ContactBook.Api.csproj"
COPY . .
WORKDIR "/src/Source/ContactBook.Api"
RUN dotnet build "ContactBook.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactBook.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactBook.Api.dll"]
