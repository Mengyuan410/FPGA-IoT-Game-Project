# FPGA-IoT-Game-Project

:star:This repository is a part of the Information Processing course in 2nd Year Imperial College London EIE department. 

## Overview
In this project, an IoT-based car-racing game system that allows multiple users to engage in multiplayer races is developed. Players utilize an FPGA board (DE-10 Lite), local FPGA host, and local game host for their gameplay. They log in using the FPGA board's switches to enter their player ID, which is sent to an AWS server for authentication. 
Once logged in, players can select their team (Team A or Team B) with the server's assistance. A waiting room allows players to join and initialize their positions. During the game, players strategically place bombs to eliminate opponents and push cars onto bombs for additional damage. The AWS server synchronizes game data for real-time multiplayer functionality. At the end of the game, results are sent to the server for database updates and player rankings. This IoT system offers an immersive and competitive multiplayer racing experience.

## Technical Details
The FPGA board chosen for this project is DE-10 Lite. Unity is used as the platform to create the game. An AWS instance is set up to perform the role of a server. 
