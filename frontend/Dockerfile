# Этап 1: Сборка
FROM node:18-alpine AS build
WORKDIR /app

# Копирование package.json и установка зависимостей
COPY package*.json ./
RUN npm install

# Копирование исходного кода
COPY . .

# Сборка приложения
RUN npm run build

# Этап 2: Сервер для статики
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Копирование собранного приложения
COPY --from=build /app/build .

# Копирование конфигурации nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]