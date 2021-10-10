FROM node:latest AS build
WORKDIR /app

COPY ./OccamsRazor.Frontend/package*.json ./

RUN npm install
ARG REACT_APP_API_URL=""

COPY ./OccamsRazor.Frontend/public ./public
COPY ./OccamsRazor.Frontend/src ./src

RUN npm run build

FROM nginx:1.21.3-alpine

COPY --from=build /app/build /usr/share/nginx/html/
COPY ./nginx.conf /etc/nginx/conf.d/default.conf