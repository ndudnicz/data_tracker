#### Run
```
docker build --no-cache -t api . && docker run -v ./db:/db -p 5000:5000 api
```
