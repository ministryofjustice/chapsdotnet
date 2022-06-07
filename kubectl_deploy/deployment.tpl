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
        env:
          - name: DB_NAME
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: dbname
          - name: RDS_PORT
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: rdsport
          - name: RDS_HOSTNAME
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: rdshost
          - name: RDS_PASSWORD
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: rdspassword
          - name: RDS_USERNAME
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: rdsusername
          - name: Instance
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: instance
          - name: Domain
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: domain
          - name: TenantId
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: tenantid
          - name: ClientId
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: clientid
          - name: CallbackPath
            valueFrom:
              secretKeyRef:
                name: chapsdotnetsecret
                key: callbackpath
          
            
