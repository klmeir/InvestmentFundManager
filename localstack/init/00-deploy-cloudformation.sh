#!/bin/bash
echo "🚀 Deploying DynamoDB stack via CloudFormation..."
awslocal cloudformation deploy \
  --template-file /opt/cloudformation/dynamodb-tables.yml \
  --stack-name investment-fund-db
