FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl apt-transport-https gnupg \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && . /etc/os-release \
    && curl -sSL https://packages.microsoft.com/config/ubuntu/${VERSION_ID}/mssql-server-2022.list \
       -o /etc/apt/sources.list.d/mssql-server-2022.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends mssql-server-fts \
    && rm -rf /var/lib/apt/lists/*

USER mssql
