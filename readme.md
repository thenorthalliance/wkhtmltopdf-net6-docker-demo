# Minimal blazor solution for wkhtmlToPdf

> This solution runs **.net 6 blazor** app inside .net6 runtime docker image
> 
> Uses WkhtmlToPdf library as pdf renderer

---

> ``📝`` app can be started locally with regular VS build

---

## Prerequisites

* Create certificate for ssl

> dotnet dev-certs https --clean  
> dotnet dev-certs https -ep ./cert/cert.pfx -p password  
> dotnet dev-certs https --trust  

* update certificate info in docker-compose.yaml

> update variable ASPNETCORE_Kestrel__Certificates__Default__Password  
> update C:\git\wkhtmltopdf-net6-docker-demo\cert to wherever "./cert/cert.pfx" happens to be

## Build image

> docker-compose build

## Start image

> docker-compose up

---

> ``📝`` app will start on https://localhost:8001, http://localhost:8000  
> if you want to redirect to another ports edit "ports" in docker-compose.yaml
