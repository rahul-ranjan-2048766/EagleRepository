@echo off

cd K8s

kubectl apply -f .

start "" http://localhost:4000/swagger/index.html

start "" http://localhost:4000/actuator/health

start "" http://localhost:4000/metrics

start "" http://localhost:6464/swagger/index.html

start "" http://localhost:6464/actuator/health

start "" http://localhost:6464/metrics

start "" http://localhost:4646

start "" http://localhost:9411/

start "" http://localhost:9200/

start "" http://localhost:5601/

start "" http://localhost:16686/
