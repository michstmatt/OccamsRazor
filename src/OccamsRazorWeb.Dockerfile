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

ENV CONNECTION_STRING=""
ENV ANSWER_TABLE="answers"
ENV GAMEMETADATA_TABLE="gamedata"
ENV QUESTIONS_TABLE="questions"
ENV KEYS_TABLE="keys"
ENV MC_QUESTIONS_TABLE="mcquestions"

EXPOSE 80
ENTRYPOINT ["dotnet", "OccamsRazor.Web.dll"]
