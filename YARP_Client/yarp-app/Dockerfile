FROM node:20 AS web-base
WORKDIR /yarp-app
COPY . .

FROM web-base AS web-dev
RUN npm install
RUN npm install -g @angular/cli

EXPOSE 4200
ENV NG_CLI_ANALYTICS=false
CMD ["npm", "run", "docker"]

