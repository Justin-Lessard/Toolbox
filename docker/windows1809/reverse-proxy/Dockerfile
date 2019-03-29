# escape=`
FROM mcr.microsoft.com/windows/servercore:1809 AS install
ARG NGINX_VERSION="1.14.2"
EXPOSE 80 443

RUN @powershell -NoProfile -ExecutionPolicy Bypass -Command "Write-Host \"Downloading Nginx version: $env:NGINX_VERSION\"; `
    Invoke-WebRequest -OutFile nginx.zip -UseBasicParsing \"http://nginx.org/download/nginx-$($env:NGINX_VERSION).zip\"; `
    Expand-Archive nginx.zip -DestinationPath C:\ ; `
	Rename-Item \"C:\nginx-$($env:NGINX_VERSION)\" C:\nginx;"

COPY ["nginx.conf", "/nginx/conf/"]

WORKDIR "C:\nginx"
CMD "nginx.exe"
