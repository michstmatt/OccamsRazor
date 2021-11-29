FROM node:latest AS build
WORKDIR /app

COPY ./occams-razor-frontend/*.json ./

RUN npm install
ARG REACT_APP_API_URL=""

COPY ./occams-razor-frontend/src ./src

RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY OccamsRazor.Web/OccamsRazor.Web.csproj ./OccamsRazor.Web/OccamsRazor.Web.csproj

# Copy everything else and build
COPY ./OccamsRazor.Common ./OccamsRazor.Common
COPY ./OccamsRazor.Web ./OccamsRazor.Web

RUN ls

RUN dotnet restore ./OccamsRazor.Web
RUN dotnet publish -c Release -o out ./OccamsRazor.Web/OccamsRazor.Web.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build /app/dist/occams-razor-frontend web

ENV CONNECTION_STRING=""
ENV MARIADB_VERSION="10.2.1"

ENV ANSWERS_TABLE="PlayerAnswers"
ENV GAMEMETADATA_TABLE="GameMetadata"
ENV KEYS_TABLE="GameKeys"
ENV MC_ANSWERS_TABLE="MultipleChoiceAnswers"
ENV MC_QUESTIONS_TABLE="MultipleChoiceQuestions"
ENV QUESTIONS_TABLE="Questions"

EXPOSE 5000
ENTRYPOINT ["dotnet", "OccamsRazor.Web.dll"]
