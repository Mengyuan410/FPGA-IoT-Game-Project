import socket
from threading import Thread
import os
from _thread import *
import re
import time
import datetime
import random 
import game_database
import user_database

def get_grouped_player_ids(team_A, team_B, ID_information): 
    player_ids = "A/"
    for player_key in team_A:
        player_ids += (ID_information[player_key]+"/")
    player_ids = player_ids[:-1]
    player_ids += ",B/"
    for player_key in team_B:
        player_ids += (ID_information[player_key]+ "/")
    player_ids = player_ids[:-1]
    return player_ids

def thread_initialize_receive(connection_socket, socket_dict, address_dict, ID_information, team_A, team_B, key, no_of_players, MVP_player_id, gameID):
    #INITIALIZING
    id_flag = 0
    team_flag = 0
    while ( not(id_flag and team_flag) ):
        cmsg, cadd = connection_socket.recvfrom(2048)
        address_dict[key] = cadd
        cmsg = cmsg.decode()
        patternid = re.findall("ID[0-9a-f]+",cmsg)
        patternteam = re.findall("T[AB]",cmsg)

        # Receive ID
        if (len(patternid)!=0):
            idinfor = patternid[0][2:] 
            ID_information[key] = idinfor
            user = user_database.get_user(idinfor)
            if (user == "N"):
                response = user_database.put_user(idinfor)
                connection_socket.sendto("None".encode(),cadd)  #"None"
            else:
                MVPcount = user['MVPcount']
                highestkillno = user['highestkillno']
                user_info_msg = str(MVPcount)+"/"+str(highestkillno)+","  #"I3/7"
                connection_socket.sendto(user_info_msg.encode(),cadd)
            
            player_ids = get_grouped_player_ids(team_A, team_B, ID_information)
            print("thread", key, "got id: ", player_ids)
            connection_socket.sendto(player_ids.encode(),cadd)
            id_flag = 1

        # Receive team selection
        if (len(patternteam)!=0): 
            team = patternteam[0][1:]
            if team == "A":
                team_A.append(key)
            elif team == "B":
                team_B.append(key)
            player_ids = get_grouped_player_ids(team_A, team_B, ID_information)
            print("thread", key, "got team selection: ", player_ids)
            # broadcast player_ids to all connected clients
        
            for i in address_dict.keys():
                socket_dict[i].sendto(str(player_ids).encode(),address_dict[i])
            team_flag = 1

    #WAITING
    start_flag = 0
    while (start_flag == 0):
        # print("Thread", key, "WAITING>>>>>> ", team_A, team_B)
        # Initialise completed - send signal to start game
        if (len(team_A)+len(team_B)==no_of_players):
            time.sleep(2)
            # StartA/1111/3333,B/2222/4444;2222/1111/3333/4444
            # before semicolon: team information
            # after semicolon: match userid to key(car number)
            start_info =  "start" + get_grouped_player_ids(team_A, team_B, ID_information)
    
            start_info += "$"

            for i in range(no_of_players):
                start_info += ID_information[i+1] + "/"
            start_info = start_info[:-1]
            print("Start info:", start_info)
            connection_socket.sendto(start_info.encode(), cadd)
            start_flag = 1

    print("Game Start")

    # get start time
    startTime = datetime.datetime.now()
    startTime = startTime.strftime('%Y-%m-%d-%H-%M-%S')


    opponent_connection_socket = socket_dict[3-key]
    opponent_connection_address = address_dict[3-key]
    while (start_flag):
        cmsg, cadd = connection_socket.recvfrom(2048)
        msg = cmsg.decode()
        patternend = re.findall("END",msg)
        patternendinfor = re.findall("E[0-9]+",msg)

        if (len(patternend)==0 and len(patternendinfor)==0):
            opponent_connection_socket.sendto(cmsg,opponent_connection_address)

        elif (len(patternendinfor)!=0):
            isMVP = patternendinfor[0][-1]
            print(patternendinfor[0])
            killno = int(patternendinfor[0][1:-1])
            start_flag = 0
            print("received end game data:", patternendinfor)

        elif (len(patternend)!=0):
            print("received end")
            request_msg = "REQ"
            connection_socket.sendto(request_msg.encode(), cadd)
            opponent_connection_socket.sendto(request_msg.encode(),opponent_connection_address)
    
    print(idinfor,"ended")
    
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

    # get end time
    endTime = datetime.datetime.now()
    endTime = endTime.strftime('%Y-%m-%d-%H-%M-%S')

    if (isMVP == "1"):
        response = user_database.update_MVPcount(idinfor,MVPcount+1)
        MVP_player_id[0] = idinfor
    
    if (killno > highestkillno):
        response = user_database.update_highestkillno(idinfor, killno)
    print("Updated user database")

    time.sleep(0.5)

    # the team that the MVP is in should be the winning team
    if MVP_player_id[0] in team_A:
        winningTeam = "A"
    else:
        winningTeam = "B"

    print(ID_information)

    team_A_members = ""
    for player_key in team_A: 
        team_A_members += str(ID_information[player_key]) + ","
    team_A_members = team_A_members[:-1]

    team_B_members = ""
    for player_key in team_B: 
        team_B_members += str(ID_information[player_key]) + ","
    team_B_members = team_B_members[:-1]

    response = game_database.put_game(gameID, MVP_player_id[0], startTime, endTime, winningTeam, team_A_members, team_B_members)
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

    # split into parts and send separately (TO BE DONE)
    connection_socket.sendto(all_data.encode(),cadd)

server_port__base= 11000   # server port 1: 12000; server port 2: 13000
server_ip = '0.0.0.0'
no_of_players = 2
ID_information= {}  
team_A =[]
team_B = []
threads = []
socket_dict = {}
address_dict = {}
key = 1
close_welcome_socket_flag = 0

gameID = random.randint(1, 999)

MVP_player_id = [""]

print("listening")

while (key <= no_of_players):
    connection_socket= socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    # welcome_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    connection_socket.bind(('', (server_port__base + key * 1000)))

    # print("waiting for player",key
    # print('Connected to :', cadd[0], ':', cadd[1])
    socket_dict[key] = connection_socket
    thread = Thread(target=thread_initialize_receive, args =(connection_socket, socket_dict, address_dict, ID_information, team_A, team_B, key, no_of_players, MVP_player_id, gameID,))
    threads.append(thread) 
    thread.start()
    print(thread)
    key += 1
print("All players connected")


