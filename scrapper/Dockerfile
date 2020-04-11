#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN echo 'apt-get update & wget'

RUN apt-get update
RUN apt-get install -y wget

RUN echo 'install gnupg2'
RUN apt-get install -y gnupg2

RUN echo 'install image libs'
RUN apt-get install -y libc6-dev
RUN apt-get install -y libgdiplus
RUN apt-get install -y libx11-dev

RUN echo 'install google chrome'
RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN dpkg -i google-chrome-stable_current_amd64.deb; apt-get -fy install

RUN echo 'install chromedriver'
RUN apt-get install -yqq unzip
RUN wget -O /tmp/chromedriver.zip http://chromedriver.storage.googleapis.com/80.0.3987.16/chromedriver_linux64.zip

RUN ls -la /tmp/

RUN unzip /tmp/chromedriver.zip
RUN cp chromedriver /usr/bin/chromedriver
RUN chown root /usr/bin/chromedriver
RUN chmod +x /usr/bin/chromedriver
RUN chmod 755 /usr/bin/chromedriver

# set display port to avoid crash
ENV DISPLAY=:99

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN dotnet --version

# RUN dotnet dev-certs https
# RUN dotnet dev-certs https --trust

WORKDIR /src
COPY ["APIS/APIS.csproj", "APIS/"]
RUN dotnet restore "APIS/APIS.csproj"
COPY . .
WORKDIR "/src/APIS"
RUN dotnet build "APIS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "APIS.dll"]