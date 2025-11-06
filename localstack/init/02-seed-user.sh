#!/bin/bash
echo "📦 Seeding initial Users data into DynamoDB..."

# Verificar que la tabla exista antes de insertar
TABLE_EXISTS=$(awslocal dynamodb list-tables | grep "Users")

if [ -z "$TABLE_EXISTS" ]; then
  echo "⚠️ Table 'Users' not found yet. Retrying in 5 seconds..."
  sleep 5
fi

echo "🚀 Inserting initial records into 'Users' table..."

awslocal dynamodb put-item \
    --table-name Users \
    --item '{
        "Id": {"S": "1"},
        "Name": {"S": "Default User"},
        "Email": {"S": "user@example.com"},
        "PhoneNumber": {"S": "+573000000000"},
        "PreferredChannel": {"S": "EMAIL"},
        "InitialBalance": {"N": "500000"},
        "Balance": {"N": "500000"},
        "LastUpdated": {"S": "2025-11-06T00:00:00Z"}
    }'

echo "✅ Default user seeded successfully."
