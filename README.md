Encryption-service launch guide

BUILDING AND RUNNING

A. To build app's image and run it docker in container run command below:

1. sh start-container.sh

B. If you do not want to build the image on you machine then you can pull it from public repository and run container on your own: 

1. docker pull nicklavr/encryption-service:main
2. docker run --restart=always -d -p 8080:8080 --name encryptionservicecontainer nicklavr/ecnryption-service:main

LAUNCHING

Afterwards the app will be accessible on address http://localhost:8080

NOTE: To access swagger use http://localhost:8080/swagger/index.html