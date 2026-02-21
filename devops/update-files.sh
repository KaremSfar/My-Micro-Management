#!/bin/bash

# Copy .env.exemple to .env
cp .env.exemple .env

# Update the .env file with the VITE_ build-time args (baked into the web image)
sed -i "s#VITE_AUTH_SERVICE_BASE_URL=.*#VITE_AUTH_SERVICE_BASE_URL=$AUTH_SERVICE_URL#g" .env
sed -i "s#VITE_MAIN_SERVICE_BASE_URL=.*#VITE_MAIN_SERVICE_BASE_URL=$MAIN_SERVICE_URL#g" .env
sed -i "s#VITE_ACTIVITY_SERVICE_BASE_URL=.*#VITE_ACTIVITY_SERVICE_BASE_URL=$ACTIVITY_SERVICE_URL#g" .env

echo "Environment file has been updated with build-time variables"