# MIT License

# Copyright (c) 2020 Joseph Auckley, Matthew O'Kelly, Aman Sinha, Hongrui Zheng

# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:

# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.

# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.



"""
Prototype of Camera class providing rendering of a Unity environment
Author: Hongrui Zheng, Xinlong Zheng, Xiaozhou Zhang
"""

import numpy as np
import zmq
from PIL import Image
import io

class RaceCarCamera(object):
    """
    Wrapper class for communication with Unity
    """
    def __init__(self, ego_idx, map_origin_x, map_origin_y, map_width, map_height, map_resolution, pose_port_num=12345, img_port_num=12346):
        self.map_origin_x = map_origin_x
        self.map_origin_y = map_origin_y
        self.map_width = map_width
        self.map_height = map_height
        self.map_resolution = map_resolution
        print(ego_idx)
        # zmq stuff
        self.context = zmq.Context()

        # publish socket gym-->zmq-->unity
        pose_tries = 0
        recon_max_tries = 100
        self.pose_pub_socket = self.context.socket(zmq.PUB)
        self.pose_port_num = pose_port_num + 2*ego_idx
        self.pose_pub_socket.bind('tcp://*:%s' % self.pose_port_num)
        


        # # loop for finding open port
        # # TODO: worry about these later
        # while pose_tries < recon_max_tries:
        #     try:
        #         self.pose_pub_socket.bind('tcp://*:%s' % pose_port_num + pose_tries)
        #         self.pose_port_num = pose_port_num + pose_tries
        #         break
        #     except:
        #         pose_tries += 1
        # self.pose_topic = 'pose'

        # image subscribe socket unity-->zmq-->gym
        # img_tries = 0
        self.img_sub_socket = self.context.socket(zmq.SUB)
        self.img_port_num = img_port_num + 2*ego_idx
        self.img_sub_socket.connect('tcp://localhost:%s' % self.img_port_num)

        # # loop for finding open port
        # # TODO: worry about these later
        # while img_tries < recon_max_tries:
        #     try:
        #         self.img_sub_socket.bind('tcp://*:%s' % img_port_num + img_tries)
        #         self.img_port_num = img_port_num + img_tries
        #         break
        #     except:
        #         img_tries += 1
        self.img_topicfilter = 'A'
        self.img_sub_socket.setsockopt_string(zmq.SUBSCRIBE, self.img_topicfilter)
        
        print('pose_port_num: '+str(self.pose_port_num))
        print('img_port_num: '+str(self.img_port_num))
        print('camera up')


    def send_pose(self, pose):
        """
        Sends the current pose of an agent to unity for render

        Args:
            pose (np.ndarray (3, )): pose to update unity with

        Returns:
            None
        """
        x = pose[0] - self.map_origin_x - self.map_width*self.map_resolution/2
        y = -(pose[1] - self.map_origin_y - self.map_height*self.map_resolution/2)
        th = -pose[2]
        # print(' '.join([str(x), str(y), str(th)]))
        self.pose_pub_socket.send_string(' '.join([str(x), str(y), str(th)]))

    def recv_img(self):
        """
        Receives the most up to date image from unity

        Args:
            None

        Returns:
            img (np.ndarray (n,m,3)): image rendering of the current agent state
        """
        topic = self.img_sub_socket.recv()
        img_bytes = self.img_sub_socket.recv()
        img = Image.open(io.BytesIO(img_bytes))
        return img