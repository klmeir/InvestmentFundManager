#!/bin/bash
echo "📦 Seeding initial funds data into DynamoDB..."

# Esperar unos segundos por si CloudFormation aún está creando las tablas
sleep 5

# Verificar que la tabla exista antes de insertar
TABLE_EXISTS=$(awslocal dynamodb list-tables | grep "Funds")

if [ -z "$TABLE_EXISTS" ]; then
  echo "⚠️ Table 'Funds' not found yet. Retrying in 5 seconds..."
  sleep 5
fi

echo "🚀 Inserting initial records into 'Funds' table..."

awslocal dynamodb put-item --table-name Funds --item '{
  "Id": {"S": "1"},
  "Name": {"S": "FPV_BTG_PACTUAL_RECAUDADORA"},
  "MinimumAmount": {"N": "75000"},
  "Category": {"S": "FPV"}
}'

awslocal dynamodb put-item --table-name Funds --item '{
  "Id": {"S": "2"},
  "Name": {"S": "FPV_BTG_PACTUAL_ECOPETROL"},
  "MinimumAmount": {"N": "125000"},
  "Category": {"S": "FPV"}
}'

awslocal dynamodb put-item --table-name Funds --item '{
  "Id": {"S": "3"},
  "Name": {"S": "DEUDAPRIVADA"},
  "MinimumAmount": {"N": "50000"},
  "Category": {"S": "FIC"}
}'

awslocal dynamodb put-item --table-name Funds --item '{
  "Id": {"S": "4"},
  "Name": {"S": "FDO-ACCIONES"},
  "MinimumAmount": {"N": "250000"},
  "Category": {"S": "FIC"}
}'

awslocal dynamodb put-item --table-name Funds --item '{
  "Id": {"S": "5"},
  "Name": {"S": "FPV_BTG_PACTUAL_DINAMICA"},
  "MinimumAmount": {"N": "100000"},
  "Category": {"S": "FPV"}
}'

echo "✅ All funds inserted successfully!"
