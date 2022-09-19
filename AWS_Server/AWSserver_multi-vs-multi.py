import socket
import os
import re
import time
import datetime
import random 
import game_database
import user_database


def get_grouped_player_ids(players_information): 
    player_ids = "A/"
    team_A = []
    team_B = []
    for player in players_information:
        if player[2]!=0:
            player_team = player[2]
            if (player_team == "A"):
                team_A.append(player[1])
            else:
                team_B.append(player[1])
    for player_id in team_A:
        player_ids += player_id+"/"
    player_ids += ",B/"
    for player_id in team_B:
        player_ids += player_id+"/"

    return player_ids[:-1]
 
server_port= 12000   # server port 12000
server_ip = '0.0.0.0'
no_of_players = 4

gameID = random.randint(1, 999)

MVP_player_id = ""
players_information= [[0,0,0]*no_of_players]  #[[cadd,id,team],[]]

connection_socket= socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
connection_socket.bind('', server_port)

start_game_flag = 0
while (start_game_flag!=1):
    cmsg, cadd = connection_socket.recvfrom(2048)
    
    patternid = re.findall("ID[0-9a-f]+",cmsg)
    patternteam = re.findall("T[AB]",cmsg)

    correct_message = False
    if (len(patternid)!=0):
        id_or_team = patternid[0][2:] 
        correct_message = True
    elif (len(patternteam)!=0):
        id_or_team = patternteam[0][1:] 
        correct_message = True
    
    if correct_message == True: # make sure the data is id or team info
        for i in range(no_of_players):
            if players_information[i][0] != 0:
                if cadd == players_information[i][0]:
                    if players_information[i][1] == 0:
                        players_information[i][1] = id_or_team
            
                        user = user_database.get_user(id_or_team)
                        if (user == "N"):
                            response = user_database.put_user(id_or_team)
                            connection_socket.sendto("$N/None;".encode(),cadd)  #"None"
                            MVPcount =0
                            highestkillno = 0
                        else:
                            MVPcount = user['MVPcount']
                            highestkillno = user['highestkillno']
                            user_info_msg = "$R/"+ str(MVPcount)+"/"+str(highestkillno)+";"  #"I3/7"
                            connection_socket.sendto(user_info_msg.encode(),cadd)
                        
                    else:
                        players_information[i][2] = id_or_team
                        send_information = get_grouped_player_ids(players_information)
                        for j in range(no_of_players):
                            if (players_information[j][0]!= 0):
                                connection_socket.sendto(send_information.encode,players_information[i][0])
                    break
            else:
                players_information[i][0] = cadd
                players_information[i][1] = id_or_team
                break
       
    # check if all players are ready
    start_game_flag = 1
    for i in range(no_of_players):
        for j in range(4):
            if (players_information[i][j]==0):
                start_game_flag = 0
                break
            
    # broadcast all players informatin and start signal to everyone
    if start_game_flag ==1:
        for i in range(no_of_players):
            send_start_information = "start,"+ get_grouped_player_ids(players_information)
            connection_socket.sendto(send_start_information.encode(),players_information[i][0])

# get start time
startTime = datetime.datetime.now()
startTime = startTime.strftime('%Y-%m-%d-%H-%M-%S')

game_data_list = ["","","","","",""]
while (start_game_flag):
    # opponent_connection_socket = socket_dict[3-key]
    # opponent_connection_address = address_dict[3-key]
    # while (start_flag):
    cmsg, cadd = connection_socket.recvfrom(2048)
    msg = cmsg.decode()
    patternend = re.findall("END",msg)
    patternendinfor = re.findall("E[0-9]+",msg)

    # find the player number from cadd
    for i in range(players_information):
        if cadd == players_information[i][0]:
            player_num = i

    # normal game data
    if (len(patternend)==0 and len(patternendinfor)==0):
        game_data_list[player_num] = msg
        # concatenate all data
        merged_game_data = ";".join(game_data_list)
        # broadcast
        for i in range(players_information):
            connection_socket.sendto(merged_game_data, players_information[i][0])

    elif (len(patternend)!=0):
        
        request_msg = "REQ"
        start_game_flag = 0
        start_end_receive_flag = 1
        for player in players_information:
            connection_socket.sendto(request_msg.encode(), player[0])
            

    # When all players in a team died, one player send to server: END
    # Server send to all players:  REQ
    # All players send: E+killnum+isMVP  e.g. E30  (killnum:3 isMVP:no)

    # Server send:
    #   username/mvpcount;                     <- all users ranked by mvpcount
    #       e.g. 4253/4;3625/2!
    #   username/highestkillno;                <- all users ranked by highestkillno
    #       e.g. 4253/5;3625/1!
    #   gameID/startTime/endTime/MVPUserID/team/teamAUserIDs/teamBUserIDs;          <- all games ranked by startTime
    #       e.g. 231/2022-03-12-21-51-20/2022-03-12-21-53-45/5343/A/5343,2345/7754,3445;

    # (each row in ranking table seperated by ';'    ranking table separated by '!')



# receive information from players about who is the MVP, and their kill number
count = 0
while (start_end_receive_flag):
    cmsg, cadd = connection_socket.recvfrom(2048)
    patternendinfor = re.findall("E[0-9]+",msg)
    
    if (len(patternendinfor)!=0):
        count += 1
        isMVP = patternendinfor[0][-1]
        killno = int(patternendinfor[0][1:-1])
        for player in players_information:
            if cadd == player[0]:
                if (isMVP == "1"):
                    response = user_database.update_MVPcount(player[1],MVPcount+1)
                    MVP_player_id = player[1]
                if (killno > highestkillno):
                    response = user_database.update_highestkillno(player[1], killno)
                print("Updated user database")

    if count == no_of_players:
        start_end_receive_flag = 0

time.sleep(0.5)

#UPDATE DATABASE
# after game process: update game database & send ranking
# the team that the MVP is in should be the winning team
endTime = datetime.datetime.now()
endTime = endTime.strftime('%Y-%m-%d-%H-%M-%S')

for player in players_information:
    if MVP_player_id == player[1]:
        winningTeam = player[2]

team_A_members = ""
team_B_members = ""

for i in range(len(players_information)):
    if players_information[i][2] == "A":
        team_A_members += players_information[i][1] + ","
    elif players_information[i][2] == "B":
        team_B_members += players_information[i][1] + ","
        
team_A_members = team_A_members[:-1]
team_B_members = team_B_members[:-1]

response = game_database.put_game(gameID, MVP_player_id, startTime, endTime, winningTeam, team_A_members, team_B_members)
print("Updated game database")

# MVP count ranking
ranking_MVPcount_data = user_database.scan_MVPcount()
ranking_MVP = ""
for i in range(len(ranking_MVPcount_data)):  # username/mvpcount;
    if i < 7:
        ranking_MVP += str(ranking_MVPcount_data[i]["username"]) + "/" + str(ranking_MVPcount_data[i]["MVPcount"]) + ";"
ranking_MVP = ranking_MVP[:-1] + "!"

# highest kill ranking
ranking_Highestkillno_data = user_database.scan_Highestkillno()
ranking_kill = ""
for i in range(len(ranking_Highestkillno_data)):  # username/highestkillno;
    if i < 7:
        ranking_kill += str(ranking_Highestkillno_data[i]["username"]) + "/" + str(ranking_Highestkillno_data[i]["highestkillno"]) + ";"
ranking_kill = ranking_kill[:-1] + "!"

# game history
games_data = game_database.scan_games()
game_history = ""
for i in range(len(games_data)):   # gameID/startTime/endTime/MVPUserID/team/teamAUserIDs/teamBUserIDs; 
    if i < 7:
        game_history += str(games_data[i]["gameID"])+"/"+str(games_data[i]["startTime"])+"/"+str(games_data[i]["endTime"])+"/"+str(games_data[i]["MVP"])+"/"+str(games_data[i]["winningTeam"])+"/"+str(games_data[i]["teamA"])+"/"+str(games_data[i]["teamB"])+";"
game_history = game_history[:-1] + "!"

all_data = ranking_MVP + ranking_kill + game_history

# broadcast
for i in range(players_information):
    connection_socket.sendto(all_data.encode(), players_information[i][0])


