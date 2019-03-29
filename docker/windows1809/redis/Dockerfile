FROM mcr.microsoft.com/windows/nanoserver:1809 AS base
WORKDIR /app
EXPOSE 6379

FROM mcr.microsoft.com/windows/servercore:1809 AS install
RUN @powershell -NoProfile -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"

RUN choco install -y redis-64 --version 3.0.503

FROM base AS final
WORKDIR /app
COPY --from=install C:/ProgramData/chocolatey/lib/redis-64 .

CMD redis-server.exe
