FROM node:18
WORKDIR /client
COPY /client/ ./
RUN npm install
EXPOSE 4200
CMD ["node_modules/.bin/ng", "serve", "--host", "0.0.0.0"]