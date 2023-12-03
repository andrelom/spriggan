#
# SDK

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS sdk

WORKDIR /sdk

COPY ./src ./src

RUN dotnet publish ./src/Spriggan.Web -c Release -o ./release

#
# Main

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS main

WORKDIR /app

RUN chsh -s /usr/sbin/nologin root

RUN addgroup --system spriggan
RUN adduser --system spriggan-web

RUN mkdir ./tmp

RUN chown -R spriggan-web:spriggan ./tmp

COPY --from=sdk /sdk/release ./

USER spriggan-web

EXPOSE 80

ENTRYPOINT ["./Spriggan.Web", "--urls", "http://0.0.0.0:80"]
