### IFMS-CTS Pension Backend

[![DotNet CI](https://github.com/abusalam/ctsapi/actions/workflows/dotnet-workflow.yml/badge.svg)](https://github.com/abusalam/ctsapi/actions/workflows/dotnet-workflow.yml)

## Setup the code repository

#### Prerequisites:
- Install [Docker Engine](https://docs.docker.com/engine/install/ubuntu/) and [Docker Compose (standalone)](https://docs.docker.com/compose/install/standalone/)  on Ubuntu
- On Windows OS using [WSL2](https://learn.microsoft.com/en-us/windows/wsl/install) is preferred.

Run the following commands to clone the whole repository

```sh
git clone https://github.com/abusalam/IFMS-CTS.git ifms-cts
cd ifms-cts
git submodule update --init --recursive
cp .env.docker .env
```

### Build Docker Images

Run the following command to build custom docker images to run this project

```sh
docker-compose build --build-arg HOST_UID=$UID
```

### You can start only required containers

To start dotnet container use the following command this will start only required stack for dotnet development

```sh
docker-compose up -d dotnet
```

#### Or start all containers using the following command

```sh
docker-compose up -d
```

Make host entry: add the following line to your `%WINDIR%\System32\drivers\etc\hosts` file

```
127.0.0.1	docker.test api.docker.test uat.docker.test rabbitmq.docker.test mailhog.docker.test
```

Open Dotnet Swagger UI http://api.docker.test/swagger/index.html
