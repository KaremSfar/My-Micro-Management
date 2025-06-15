#!/bin/bash

# Copy .env.exemple to .env
cp .env.exemple .env

# Update the .env file with GitHub secrets
sed -i "s#Auth_Database__DatabaseType=.*#Auth_Database__DatabaseType=$AUTH_DB_TYPE#g" .env
sed -i "s#Auth_Database__ConnectionString=.*#Auth_Database__ConnectionString=$AUTH_DB_CONNECTION#g" .env
sed -i "s#Jwt__JwtAccessKey=.*#Jwt__JwtAccessKey=$JWT_ACCESS_KEY#g" .env
sed -i "s#Jwt__JwtRefreshKey=.*#Jwt__JwtRefreshKey=$JWT_REFRESH_KEY#g" .env
sed -i "s#Main_Database__DatabaseType=.*#Main_Database__DatabaseType=$MAIN_DB_TYPE#g" .env
sed -i "s#Main_Database__ConnectionString=.*#Main_Database__ConnectionString=$MAIN_DB_CONNECTION#g" .env
sed -i "s#VITE_AUTH_SERVICE_BASE_URL=.*#VITE_AUTH_SERVICE_BASE_URL=$AUTH_SERVICE_URL#g" .env
sed -i "s#VITE_MAIN_SERVICE_BASE_URL=.*#VITE_MAIN_SERVICE_BASE_URL=$MAIN_SERVICE_URL#g" .env
sed -i "s#googleclient_id=.*#googleclient_id=$GOOGLE_CLIENT_ID#g" .env
sed -i "s#googleclient_secret=.*#googleclient_secret=$GOOGLE_CLIENT_SECRET#g" .env
sed -i "s#POSTGRES_USER=.*#POSTGRES_USER=$POSTGRES_USER#g" .env
sed -i "s#POSTGRES_PASSWORD=.*#POSTGRES_PASSWORD=$POSTGRES_PASSWORD#g" .env
sed -i "s#OTEL__JAEGER_URL=.*#OTEL__JAEGER_URL=$OTEL_JAEGER_URL#g" .env

echo "Environment file has been updated with secrets"