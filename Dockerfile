FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./Encryption-service ./
RUN dotnet publish -c Release -o release

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/release ./

EXPOSE 8080
CMD ["dotnet", "Encryption-service.dll"]