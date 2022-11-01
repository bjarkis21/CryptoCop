FROM mcr.microsoft.com/dotnet/sdk:6.0

LABEL maintainer="Bjarki Sigurdsson"

COPY . /app
WORKDIR /app/Cryptocop-Payments.Payment-service
ENV ASPNETCORE_ENVIRONMENT Production
RUN dotnet restore
ENTRYPOINT ["dotnet", "run"]