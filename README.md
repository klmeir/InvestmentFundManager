# 💸 Investment Fund Manager

Plataforma de suscripción a fondos desarrollada con **.NET 8 (Backend)** y **Angular 20 (Frontend)**.  
Todo el entorno puede levantarse fácilmente con **Docker Compose** para revisión.

---

## 🚀 Descripción general

La aplicación permite:

- Suscribirse a un nuevo fondo (aperturas).
- Ver el historial de últimas transacciones (aperturas y cancelaciones)
- Salirse de un fondo actual (cancelaciones)
- Persistir datos en **AWS DynamoDB** (mediante LocalStack en modo local).
- Enviar una notificación por email o sms dependiendo de la selección del usuario una vez suscrito a dicho fondo (mediante LocalStack en modo local)

---

## 🧩 Build & Run
# 🐳 Docker y Docker Compose
Desde la raíz del repositorio, ejecuta:
```plaintext
docker-compose build --no-cache
docker-compose up -d
```

Deberías ver algo similar:

| Aplicación             | URL o Servicio                |
| ---------------------- | ----------------------------- |
| InvestmentFund API     | [http://localhost:8000](http://localhost:8000/swagger/index.html)         |
| InvestmentFund Frontend| http://localhost:4200         |
| LocalStack (AWS Mock)  | http://localhost:4566         |

---

## 🧱 Arquitectura general

```plaintext
Docker Compose
│
├── api (.NET 8 Web API)
│   ├── /api/funds              → Listado de fondos
│   ├── /api/funds/subscribe    → Suscribir usuario a un fondo
│   ├── /api/funds/cancel       → Cancelar suscripción a un fondo
│   └── /api/funds/transactions → Consultar transacciones
│
├── frontend (Angular 20)
│   └── http://localhost:4200
│
└── localstack (AWS DynamoDB + SNS simulados)
```

## 🗂️ Estructura del proyecto
```plaintext
InvestmentFundManager/
│
├── src/
│   ├── InvestmentFundManager.Api/              # API .NET 8
│   │   ├── Controllers/
│   │   ├── Application/
│   │   ├── Domain/
│   │   ├── Infrastructure/
│   │   ├── appsettings.json
│   │   └── Dockerfile
│   │
│   └── InvestmentFundManager.sln
│
├── investment-fund-manager-spa/                # Frontend Angular 20
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   ├── services/
│   │   │   ├── models/
│   │   │   └── app.component.ts
│   │   └── environments/
│   │       ├── environment.ts
│   │       └── environment.development.ts
│   ├── angular.json
│   ├── package.json
│   ├── Dockerfile
│   └── nginx.conf
│
├── docker-compose.yml
├── README.md
```

## ⚙️ Configuración de entorno
yaml
```plaintext
environment:
  - AWS_ServiceURL=http://localstack:4566
  - AWS_Region=us-east-1
  - AWS_AccessKey=test
  - AWS_SecretKey=test
  - AWS_SnsEmailTopicArn=arn:aws:sns:us-east-1:000000000000:EmailTopic
  - AWS_SnsSmsTopicArn=arn:aws:sns:us-east-1:000000000000:SmsTopic
```
appsettings.json
```plaintext
{
  "AwsSettings": {
    "ServiceURL": "http://localstack:4566",
    "Region": "us-east-1",
    "AccessKey": "test",
    "SecretKey": "test",
    "SnsEmailTopicArn": "arn:aws:sns:us-east-1:000000000000:EmailTopic",
    "SnsSmsTopicArn": "arn:aws:sns:us-east-1:000000000000:SmsTopic",
    "DynamoFundsTransactionsTableName": "FundsTransactions",
    "DynamoUserTableName": "Users",
    "DynamoFundTableName": "Funds"
  },
  "AllowedHosts": "*"
}

