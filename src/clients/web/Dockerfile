# Use the official Node.js 20 image as the build environment
FROM node:20 AS build

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json into the working directory
COPY package.json ./
COPY package-lock.json ./

# Install dependencies
RUN npm ci

# Copy the rest of the application code
COPY . .

# Set build-time environment variables
ARG REACT_APP_AUTH_SERVICE_BASE_URL
ARG REACT_APP_MAIN_SERVICE_BASE_URL

# Pass them to the build process
ENV REACT_APP_AUTH_SERVICE_BASE_URL=$REACT_APP_AUTH_SERVICE_BASE_URL
ENV REACT_APP_MAIN_SERVICE_BASE_URL=$REACT_APP_MAIN_SERVICE_BASE_URL

# Build the React app for production
RUN npm run build

# Use nginx to serve static files
FROM nginx:alpine AS production

# Copy build files from the previous stage
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Entry point, using nginx
CMD ["nginx", "-g", "daemon off;"]