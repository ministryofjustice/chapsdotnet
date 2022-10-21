apiVersion: apps/v1
kind: Deployment
metadata:
  name: chaps-staging
spec:
  replicas: 1
  selector:
    matchLabels:
      app: chaps-staging
  template:
    metadata:
      labels:
        app: chaps-staging
    spec:
      containers:
      - name: dotnet
        image: ${ECR_URL}:${IMAGE_TAG}
        ports:
        - containerPort: 5010
        readinessProbe:
          httpGet:
            path: /Ping
            port: 5010
            httpHeaders:
              - name: X-Forwarded-Proto
                value: https
              - name: X-Forwarded-Ssl
                value: "on"
        livenessProbe:
          httpGet:
            path: /Ping
            port: 5010
            httpHeaders:
              - name: X-Forwarded-Proto
                value: https
              - name: X-Forwarded-Ssl
                value: "on"
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


