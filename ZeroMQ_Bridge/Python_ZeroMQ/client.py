import time
import zmq
import base64
from PIL import Image
import io
import cv2
import numpy

context = zmq.Context()
socket = context.socket(zmq.SUB)
socket.connect("tcp://localhost:12346")
topicfilter = "A"
socket.setsockopt_string(zmq.SUBSCRIBE, topicfilter)

i = 0
fps = []
tic = time.perf_counter()
while True:
    topic = socket.recv()
    data = socket.recv()
    toc = time.perf_counter()
    i = i+1
    if (i % 100 == 0):
        print(i/(toc-tic))


    image = Image.open(io.BytesIO(data))
    cv2.imshow('image',cv2.cvtColor(numpy.array(image), cv2.COLOR_RGB2BGR))
    cv2.waitKey(1)
    # image.save(f"{i}.png")


