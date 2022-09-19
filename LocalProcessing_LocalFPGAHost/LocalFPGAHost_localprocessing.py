from gettext import find
import string
import subprocess
import re
import time
import numpy as np
import math
import socket
from threading import Thread
import intel_jtag_uart

def fir_filter(current_reading, raw_list, coeffs, total_taps):
    filtered = 0
    for i in range(total_taps-1,0,-1):
        raw_list[i] = raw_list[i-1]

    raw_list[0] = current_reading
    filtered = sum(np.multiply(coeffs,raw_list))
    return filtered

def twos_complement(hexstr):
    value = int(hexstr,16)
    if value & (1 << 31):
        value -= 1 << 32
    return value
    
def speed(v):
    if abs(v)>730:
        level = "3"
    elif abs(v)>460:
        level = "2"
    else:
        level = "1"
    return level

def find_init_angles(initx,inity,initz):
    init_phi = math.atan2(inity,initz)*180/math.pi
    # process theta
    init_theta = math.atan2(-initx,math.sqrt(inity**2+initz**2))*180/math.pi
    if (initz<0 and init_theta>0):
        init_theta = 180-init_theta
    elif(initz<0 and init_theta<=0):
        init_theta = -180-init_theta
    return init_phi, init_theta

def find_phi_difference(init_phi, y_val, z_val):
    phi = math.atan2(y_val,z_val)*180/math.pi
    rotated_angle = init_phi - phi
    
    if rotated_angle > 180:
        rotated_angle -= 360
    elif rotated_angle < -180:
        rotated_angle += 360
    if abs(rotated_angle) >= 90:
        rotated_angle = 0
    return rotated_angle
    
def find_theta_difference(init_theta,x_val,y_val,z_val):
    theta = math.atan2(-x_val,math.sqrt(y_val**2+z_val**2))*180/math.pi
    if (z_val<0 and theta>0):
        theta = 180-theta
    elif(z_val<0 and theta<=0):
        theta = -180-theta
    rotated_angle = theta - init_theta
    
    if rotated_angle > 180:
        rotated_angle -= 360
    elif rotated_angle < -180:
        rotated_angle += 360
    if abs(rotated_angle) >= 90:
        rotated_angle = 0
    return rotated_angle

def receive_from_jtag(out,TAPSpt,TAPSz,phi_list,theta_list,z_list,sample_rate,zcount,init_phi,init_theta): 

    x_axis = 0
    y_axis = 0
    z_axis = 0

    z_axis_f = 0
    speedout = "0"
    rawbutton = 0
    button1 = "0"
    button0 = "0"
    eat = "0"
    direction = "0"

    coeffspt =  [1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000]
    coeffsz = [ 0.0684, 0.8643, 0.0684]

    patternrawx = re.findall("Rx[0-9a-f]*/", out) 
    patternrawy = re.findall("Ry[0-9a-f]*/", out) 
    patternrawz = re.findall("Rz[0-9a-f]*/", out)
    patternbutton = re.findall("B[0-9]*/", out)
    patterninitx = re.findall("Ix[0-9a-f]*/",out)
    patterninity = re.findall("Iy[0-9a-f]*/",out)
    patterninitz = re.findall("Iz[0-9a-f]*/",out)

    if (len(patterninitx)!=0):
        testx = patterninitx[0][2:-1]
        testy = patterninity[0][2:-1]
        testz = patterninitz[0][2:-1]
        initx = twos_complement(testx)
        inity = twos_complement(testy)
        initz = twos_complement(testz)
        init_phi, init_theta = find_init_angles(initx,inity,initz)

    if (len(patternrawx)!=0):
        test = patternrawx[-1][2:-1]
        x_axis=(twos_complement(test))
        
    if (len(patternrawy)!=0):
        test = patternrawy[-1][2:-1]
        y_axis=(twos_complement(test))

    if (len(patternrawz)!=0):
        test = patternrawz[-1][2:-1]
        z_axis=(twos_complement(test))
        z_axis_f = pow(fir_filter(z_axis, z_list,coeffsz,TAPSz),3)

    phi = find_phi_difference(init_phi, y_axis, z_axis)
    theta = find_theta_difference(init_theta,x_axis,y_axis,z_axis)

    phi_f = fir_filter(phi,phi_list,coeffspt,TAPSpt)
    theta_f = fir_filter(theta,theta_list,coeffspt,TAPSpt)

    if ((abs(z_axis_f) > 50000000 and zcount == 0) or (zcount<0.5*sample_rate and zcount>0)):
        eat = "1"
        zcount += 1

    else:
        if(abs(theta_f)>200 and abs(phi_f)>200):
            if abs(theta_f)> abs(phi_f):
                if (theta_f<0):
                    direction = "3"
                else:
                    direction = "4"
                speedout = speed(theta_f)
            else:
                if (phi_f<0):
                    direction="2"
                elif (phi_f>0):
                    direction="1"
                speedout = speed(phi_f)
        elif(abs(theta_f)>200 and abs(phi_f)<=200):
            if (theta_f<0):
                direction = "3"
            else:
                direction = "4"
            speedout = speed(theta_f)
        elif(abs(phi_f)>200 and abs(theta_f)<=200):
            if (phi_f<0):
                direction = "2"
            else:
                direction = "1"
            speedout = speed(phi_f)
        else:
            direction = "0"

    if (zcount>=(0.5*sample_rate-1)):
        zcount = 0
        
    if (len(patternbutton)!=0):
        test = patternbutton[0][1:-1]
        rawButton=(twos_complement(test))
        if(rawButton==1):
            button1="1"
        elif(rawButton==2):
            button0="1"
    output = direction+"/"+speedout+"/"+eat+"/"+button1 + "/" +button0+"," + "\n"
    return output,zcount,init_phi,init_theta,eat,button1,button0

def perform_computation(client_socket, msg_send):
    ju = intel_jtag_uart.intel_jtag_uart()

    #Collect Username
    print("Please enter your 4-digit username. Press Button 0 to confirm.")
    ju.write(b'P\n')
    username_flag = 0
    while (username_flag==0):
        time.sleep(0.0001)
        usernameraw = ju.read().decode()
        
        patternusername = re.findall("Usernamex[0-9a-f]*",usernameraw)
        if (len(patternusername)!=0):
            username_flag = 1
            username = str(twos_complement(patternusername[0][9:])).zfill(4)
    print("Your username is ",username)
    send_data = "Initial/" + username + ","
    username_encoded = send_data.encode()
    client_socket.send(username_encoded)

    #Enter Game
    print("Start")
    transmission_rate = 1   
    sample_rate = 40
    TAPSpt = 11
    TAPSz = 3
    zcount = 0 # when a z peak is detected
    phi_list = np.zeros(TAPSpt)
    theta_list = np.zeros(TAPSpt)
    z_list = np.zeros(TAPSz)
    hostcount = 0

    flag_eat = 0
    flag_bomb = 0
    flag_confirmsetting = 0

    init_phi = 0
    init_theta = 0

    while (1):
        if (msg_send[0]!=0):
            msg_ju = msg_send[0]+"\n"
            #print(msg_ju)
            ju.write(bytes(msg_ju, 'utf-8'))
            msg_send[0] = 0
        else:
            time.sleep(0.0001)
            out = ju.read().decode()
            if out:
                output,zcount,init_phi,init_theta,eat,bomb,confirmsetting = receive_from_jtag(out,TAPSpt,TAPSz,phi_list,theta_list,z_list,sample_rate,zcount,init_phi,init_theta)
                hostcount += 1
                if (hostcount==transmission_rate):
                    send = False

                    if (eat != "1"):
                        flag_eat = 0
                    if (bomb != "1"):
                        flag_bomb = 0
                    if (confirmsetting !="1"):
                        flag_confirmsetting = 0
                        
                    if(eat=="1" and flag_eat == 0):
                        send = True
                        flag_eat = 1
                    elif (bomb=="1" and flag_bomb == 0):
                        flag_bomb = 1
                        send = True
                    elif (confirmsetting=="1" and flag_confirmsetting == 0):
                        flag_confirmsetting = 1
                        send = True
                    elif(eat != "1" and bomb != "1" and confirmsetting != "1"):
                        send = True
                        
                    if (send == True):
                        out_encoded = output.encode()
                        #print("o",output)
                        client_socket.send(out_encoded)
                    hostcount = 0

def receive_from_host(client_socket, msg_send):
    cmsg = client_socket.recv(2048)
    print("cmsg",cmsg)
    cmsg = cmsg.decode()
    # received format:
    # health/blood,health/blood,health/blood,
    cmsg = "," + cmsg  # add a comma at the beginning so that the following regex can perform as expected
    messages = re.findall("(?<=\,)(.*?)(?=\,)", cmsg)  # find all words between two comma
    message_win = re.findall("WIN",cmsg)
    message_die = re.findall("LOSE",cmsg)

    if len(message_win) != 0:
            msg_send[0] = "W"
    elif len(message_die) != 0:
            msg_send[0] = "L" 

    elif len(messages) != 0:
        msg_prep = messages[-1].split("/")
        msg = "," +msg_prep[0].zfill(2)+msg_prep[1].zfill(2)
        print("m",msg)
        if (msg != msg_send[0]):
            msg_send[0] = msg
    
def interact_with_host(cleint_socket,msg_send):
    while(1):
        receive_from_host(cleint_socket,msg_send)    
               
               

def main():
    print("Start connecting to host")
    server_name = '127.0.0.1'
    server_port = 8052
    # server_name = '192.168.1.77'
    # server_port = 12000
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client_socket.connect((server_name, server_port))
    print("Connected to host")
    
    global receive_flag
    receive_flag = 0
    msg_send = [0]

    thread_host = Thread(target=interact_with_host, args =(client_socket, msg_send,  ))
    thread_host.start()
    
    thread_fpga = Thread(target=perform_computation, args = (client_socket, msg_send, ))
    thread_fpga.start()

    #client_socket.close()
   
    
    
if __name__ == '__main__':
    main()
