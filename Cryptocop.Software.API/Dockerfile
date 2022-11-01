FROM mcr.microsoft.com/dotnet/sdk:6.0

LABEL maintainer="Bjarki Sigurdsson"

ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
ENV ASPNETCORE_ENVIRONMENT Production
ENV DOTNET_RUNNING_IN_CONTAINER true

COPY . /app
WORKDIR /app
RUN dotnet restore
WORKDIR /app/Cryptocop.Software.API
RUN dotnet dev-certs https
RUN dotnet dev-certs https -ep /usr/local/share/ca-certificates/aspnet/https.crt --format PEM
RUN update-ca-certificates
ENTRYPOINT ["dotnet", "run"]