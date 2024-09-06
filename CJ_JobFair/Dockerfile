#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CJ_JobFair/CJ_JobFair.csproj", "CJ_JobFair/"]
RUN dotnet restore "CJ_JobFair/CJ_JobFair.csproj"
COPY . .
WORKDIR "/src/CJ_JobFair"
RUN dotnet build "CJ_JobFair.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CJ_JobFair.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CJ_JobFair.dll"]

# Set permissions for the folder
RUN chmod -R 755 /app/CJ_JobFair/Areas/Admin/Adminroot/assets

#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#
#COPY CJ_JobFair/CJ_JobFair.csproj CJ_JobFair/
#RUN dotnet restore CJ_JobFair/CJ_JobFair.csproj
#
#COPY . .
#WORKDIR /src/CJ_JobFair
#RUN dotnet build -c Release -o /app
#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
#WORKDIR /app
#COPY --from=build /app .
#ENTRYPOINT ["dotnet", "CJ_JobFair.dll"]
#
