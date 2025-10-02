# Dockerfile simples para Angular
FROM node:18-alpine

WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

RUN mkdir -p src/assets

EXPOSE 4200

CMD sh -c 'echo "window.ENV = { \"API_URL\": \"${API_URL:-http://localhost:8080}\" };" > src/assets/env.js && cat src/assets/env.js && npm start -- --host 0.0.0.0'