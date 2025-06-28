echo "building encryptionservice"
docker build -t encryptionservice .

echo "running encryptionservicecontainer"
docker run --restart=always -d -p 8080:8080 --name encryptionservicecontainer encryptionservice

echo "Encryption-service app is accessible on http://localhost:8080"