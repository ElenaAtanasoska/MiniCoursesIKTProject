FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
RUN mkdir -p /app/wwwroot/uploads && chown $APP_UID:$APP_UID /app/wwwroot/uploads
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MiniCoursesIKTProject/MiniCoursesIKTProject.csproj", "MiniCoursesIKTProject/"]
COPY ["MiniCoursesDomain/MiniCoursesDomain.csproj", "MiniCoursesDomain/"]
COPY ["MiniCoursesRepository/MiniCoursesRepository.csproj", "MiniCoursesRepository/"]
RUN dotnet restore "MiniCoursesIKTProject/MiniCoursesIKTProject.csproj"
COPY . .
WORKDIR "/src/MiniCoursesIKTProject"
RUN dotnet build "MiniCoursesIKTProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MiniCoursesIKTProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniCoursesIKTProject.dll"]