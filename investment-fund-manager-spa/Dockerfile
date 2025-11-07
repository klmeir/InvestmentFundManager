# Etapa 1: Build Angular
FROM node:20 AS build
WORKDIR /app

# Copiar dependencias
COPY package*.json ./
RUN npm install

# Copiar código fuente
COPY . .

# Variable pasada desde docker-compose
ARG API_URL
# Reemplazar el valor del API_URL en environment.ts
RUN echo "Usando API_URL=${API_URL}" && \
    sed -i "s|API_URL_PLACEHOLDER|${API_URL}|g" src/environments/environment.ts

# Compilar la app
RUN npm run build --omit=dev

# Etapa 2: Servir con Nginx
FROM nginx:alpine
COPY --from=build /app/dist/InvestmentFundManager/browser /usr/share/nginx/html

# Configurar proxy para el backend
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
