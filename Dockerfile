FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY DotNetBookstore/DotNetBookstore.csproj DotNetBookstore/
RUN dotnet restore DotNetBookstore/DotNetBookstore.csproj
COPY DotNetBookstore/ DotNetBookstore/
RUN dotnet publish DotNetBookstore/DotNetBookstore.csproj -c Release --no-restore -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000
USER $APP_UID
ENTRYPOINT ["dotnet", "DotNetBookstore.dll"]
