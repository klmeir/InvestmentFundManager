#!/bin/bash
ENV_FILE="/tmp/env_vars"

EMAIL_TOPIC_ARN=$(awslocal cloudformation describe-stacks \
  --stack-name investment-fund-db \
  --query "Stacks[0].Outputs[?OutputKey=='EmailTopicArn'].OutputValue" \
  --output text)

SMS_TOPIC_ARN=$(awslocal cloudformation describe-stacks \
  --stack-name investment-fund-db \
  --query "Stacks[0].Outputs[?OutputKey=='SmsTopicArn'].OutputValue" \
  --output text)

echo "EMAIL_TOPIC_ARN=$EMAIL_TOPIC_ARN" > $ENV_FILE
echo "SMS_TOPIC_ARN=$SMS_TOPIC_ARN" >> $ENV_FILE

echo "✅ Environment variables saved to $ENV_FILE"
