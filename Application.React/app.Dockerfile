# Building stage using Node.js image for building the React application
FROM node:18 AS build

# Setting working directory, good practice to encapsulate the app's environment
WORKDIR /app

# Copying package files to only install dependencies, leveraging caching
COPY package*.json ./

# Installing dependencies; consider running 'npm ci' for cleaner builds if using package-lock.json
RUN npm install

# Suggestion: Add a .dockerignore file to exclude unnecessary files from context

# Copying application code; ensure path is correct (relative to Docker build context)
COPY . ./

# Building the React app for production deployment
RUN npm run build 

# Consider adding ARG for build time variables if needed

# Switching to a lightweight Nginx image for serving the built app
FROM nginx:alpine AS runtime

# Copying the built output to the Nginx default serve directory
COPY --from=build /app/build /usr/share/nginx/html

# Exposing port 80 to match any external service configuration needs
EXPOSE 80

# Starting Nginx server in the foreground to ensure container remains running
CMD ["nginx", "-g", "daemon off;"]
