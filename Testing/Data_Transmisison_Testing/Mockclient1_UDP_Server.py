import socket
import time
import re
print("We're in tcp client...")
    
flag = 1
server_name = '3.238.145.62'
#server_name = '192.168.1.253'
server_port = 12000

client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

x = input("wait1")
str1 = "ID7777"
client_socket.sendto(str1.encode(),(server_name, server_port))

flat = 1
while (flag>0):
    cmsg,cadd = client_socket.recvfrom(2048)
    cmsg = cmsg.decode()
    print(cmsg)
    flag -= 1

x = input("wait2")
str2 = "TA"
client_socket.sendto(str2.encode(),(server_name, server_port))

start_flag = 0
while (start_flag==0):
    cmsg, cadd = client_socket.recvfrom(2048)
    if cmsg.decode()[:5] == "start":
        start_flag = 1
print(cmsg.decode())
cmsg0 = "2.34/45.78/1/0/12.45/23/7.3"

start_time = time.time()

for i in range (55):
    if (i%10 == 0):
        cmsg = cmsg0+"/$12.34/25.6"
    else:
        cmsg = cmsg0
    cmsg = cmsg+ ","
    client_socket.sendto(cmsg.encode(),(server_name, server_port))

    cmsg_2,cadd = client_socket.recvfrom(2048)
    cmsg_2 = cmsg_2.decode()
    print(cmsg_2)

    patterndied = re.findall("0/0/0/0/0/0/0",cmsg_2)
    if (len(patterndied) != 0):
        cmsg = "END"
        client_socket.sendto(cmsg.encode(),(server_name, server_port))
        print("end sent")
        break
end_time = time.time()
print("Time",end_time-start_time)    

request_flag = 0
while (request_flag == 0):
    cmsg_2,cadd = client_socket.recvfrom(2048)
    request_msg = cmsg_2.decode()
    patternreq = re.findall("REQ",request_msg)
    if (len(patternreq)!=0):
        request_flag = 1
        client_socket.sendto("E51".encode(),(server_name, server_port))

end_cmsg, cadd = client_socket.recvfrom(2048)
print(end_cmsg.decode())
end_cmsg, cadd = client_socket.recvfrom(2048)
print(end_cmsg.decode())
end_cmsg, cadd = client_socket.recvfrom(2048)
print(end_cmsg.decode())
    
client_socket.close()
