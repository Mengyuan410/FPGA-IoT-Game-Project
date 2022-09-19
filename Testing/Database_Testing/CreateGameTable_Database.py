import boto3


def create_game_table(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')
        table = dynamodb.create_table(
            TableName='Gamedata',
            KeySchema=[
                {
                    'AttributeName': 'MVP',
                    'KeyType': 'HASH'  # Partition key
                },
                {
                    'AttributeName': 'gameID',
                    'KeyType': 'RANGE'  # Partition key
                }
            ],
            AttributeDefinitions=[
                {
                    'AttributeName': 'MVP',
                    'AttributeType': 'S'
                },
                {
                    'AttributeName': 'gameID',
                    'AttributeType': 'N'
                },
            ],
            ProvisionedThroughput={
                'ReadCapacityUnits': 10,
                'WriteCapacityUnits': 10
            }
        )
    return table


if __name__ == '__main__':
    game_table = create_game_table()
    print("Table status:", game_table.table_status)
