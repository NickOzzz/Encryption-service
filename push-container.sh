REPO_NAME=$1

if [ $# -eq 0 ]
  then
    echo "PLEASE PROVIDE NAME OF THE REPOSITORY YOU WOULD LIKE TO PUSH TO"
    exit 0
fi

echo "login into your docker account"
docker login

echo "building encryptionservice"
docker build -t $REPO_NAME:main .

echo "pushing image to $REPO_NAME:main"
docker push $REPO_NAME:main
