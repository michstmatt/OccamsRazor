FROM node:latest AS build
WORKDIR /app

COPY ./OccamsRazor.Frontend/package*.json ./

RUN npm install
ARG REACT_APP_API_URL=""

COPY ./OccamsRazor.Frontend/public ./public
COPY ./OccamsRazor.Frontend/src ./src

RUN npm run build

FROM node:latest
WORKDIR /app
COPY --from=build /app/build ./build

COPY ./OccamsRazor.Frontend/prodServer.js ./prodServer.js

RUN npm install express
RUN ls

ENV PORT=3000
EXPOSE 3000

CMD ["node", "prodServer.js"]
