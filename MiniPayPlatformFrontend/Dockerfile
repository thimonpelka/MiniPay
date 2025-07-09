FROM node:18-alpine AS build

# Use Node.js to build the frontend application
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .

# Expose port 80 for the Nginx server
EXPOSE 4200

# Set the command to run Nginx in the foreground
CMD ["npx", "ng", "serve", "--host", "0.0.0.0"]
