from pprint import pprint
import boto3
import random
import datetime
import time
from boto3.dynamodb.conditions import Key
from decimal import Decimal
from botocore.exceptions import ClientError

def put_game(gameID, MVPUserName, startTime, endTime, winningTeam, teamAPlayers, teamBPlayers, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Gamedata')
    response = table.put_item(
        Item={
            'gameID': gameID,
            'MVP': MVPUserName,
            'startTime': startTime,
            'endTime': endTime,
            'winningTeam': winningTeam,
            'teamA': teamAPlayers,
            'teamB': teamBPlayers
        }
    )
    return response


def scan_games(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Gamedata')

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
            if data[i]['startTime'] > data[i+1]['startTime']:
                # Swap
                data[i], data[i+1] = data[i+1], data[i]
                has_swapped = True
    return data


def delete_game(game_id, MVP_id, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.Table('Gamedata')

    try:
        response = table.delete_item(
            Key={
                'gameID': game_id,
                'MVP': MVP_id
            }
        )
    except ClientError as e:
        if e.response['Error']['Code'] == "ConditionalCheckFailedException":
            print(e.response['Error']['Message'])
        else:
            raise
    else:
        return response

