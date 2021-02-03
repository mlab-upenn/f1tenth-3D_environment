#
#   Hello World server in Python
#   Binds REP socket to tcp://*:5555
#   Expects b"Hello" from client, replies with b"World"
#

import time
import zmq

context = zmq.Context()
socket = context.socket(zmq.PUB)
socket.bind("tcp://*:12345")

i = 0
while True:
    pose = [i*0.001, i*0.001, i*0.1]
    time.sleep(0.01)
    socket.send_string(' '.join([str(elem) for elem in pose]))
    i = i+1
