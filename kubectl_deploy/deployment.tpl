apiVersion: apps/v1
kind: Deployment
metadata:
  name: chaps-dev
spec:
  replicas: 1
  selector:
    matchLabels:
      app: chaps-dev
  template:
    metadata:
      labels:
        app: chaps-dev
    spec:
      containers:
      - name: dotnet
        image: ${ECR_URL}:${IMAGE_TAG}
        ports:
        - containerPort: 5010
