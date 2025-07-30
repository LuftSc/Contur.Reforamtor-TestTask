FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update && \
    apt-get install -y \
        openjdk-17-jre \
        wget \
        unzip && \
    wget https://www.saxonica.com/download/SaxonHE12-0J.zip -O /tmp/saxonhe.zip && \
    unzip /tmp/saxonhe.zip -d /opt/saxonhe && \
    rm /tmp/saxonhe.zip && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV SAXON_HOME=/opt/saxonhe
ENV PATH="${SAXON_HOME}:${PATH}"

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["XSLTReformator.csproj", "."]
RUN dotnet restore "XSLTReformator.csproj"
COPY . .
RUN dotnet build "XSLTReformator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XSLTReformator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY ["XSLT/", "./XSLT/"]
COPY ["XML/", "./XML/"]

RUN chown -R app:app /app

USER app
ENTRYPOINT ["dotnet", "XSLTReformator.dll"]