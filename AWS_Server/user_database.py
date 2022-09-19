from pprint import pprint
import boto3
from decimal import Decimal
import json
from tracemalloc import start
from botocore.exceptions import ClientError
from boto3.dynamodb.conditions import Key


def put_user(username, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Userdata')
    response = table.put_item(
        Item={
            'username': username,
            'highestkillno': 0,
            'MVPcount': 0
        }
    )
    return response


def get_user(username, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')
    
    table = dynamodb.Table('Userdata')

    try:
        response = table.get_item(Key={'username':username}) 
    except Error as e:
        print(e.response['Error']['Message'])
    else:
        if 'Item' in response:
            return response['Item']
        else:
            return "N"


def update_highestkillno(username, highestkillno, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Userdata')

    response = table.update_item(
        Key={
            'username': username,
        },
        UpdateExpression="set highestkillno=:k",
        ExpressionAttributeValues={
            ':k': highestkillno
        },
        ReturnValues="UPDATED_NEW"
    )
    return response


def update_MVPcount(username, MVPcount, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Userdata')

    response = table.update_item(
        Key={
            'username': username,
        },
        UpdateExpression="set MVPcount=:m",
        ExpressionAttributeValues={
            ':m': MVPcount
        },
        ReturnValues="UPDATED_NEW"
    )
    return response


def delete_user(username, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Userdata')

    try:
        response = table.delete_item(
            Key={
                'username': username
            }
        )
    except ClientError as e:
        if e.response['Error']['Code'] == "ConditionalCheckFailedException":
            print(e.response['Error']['Message'])
        else:
            raise
    else:
        return response


def scan_MVPcount(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Userdata')

    #scan and get the first page of results
    response = table.scan()
    data = response['Items']

    #continue while there are more pages of results
    while 'LastEvaluatedKey' in response:
        response = table.scan(ExclusiveStartKey=response['LastEvaluatedKey'])
        data.extend(response['Items'])

    has_swapped = True
    while(has_swapped):
        has_swapped = False
        for i in range(len(data) - 1):
            if data[i]['MVPcount'] < data[i+1]['MVPcount']:
                data[i], data[i+1] = data[i+1], data[i]
                has_swapped = True
    return data


def scan_Highestkillno(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Userdata')

    #scan and get the first page of results
    response = table.scan();
    data = response['Items']

    #continue while there are more pages of results
    while 'LastEvaluatedKey' in response:
        response = table.scan(ExclusiveStartKey=response['LastEvaluatedKey'])
        data.extend(response['Items'])


    has_swapped = True
    while(has_swapped):
        has_swapped = False
        for i in range(len(data) - 1):
            if data[i]['highestkillno'] < data[i+1]['highestkillno']:
                data[i], data[i+1] = data[i+1], data[i]
                has_swapped = True
    return data