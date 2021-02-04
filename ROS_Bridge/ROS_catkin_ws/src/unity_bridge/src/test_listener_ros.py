#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
from sensor_msgs.msg import CompressedImage
import cv2
import numpy as np

def callback(data):
    np_arr = np.fromstring(data.data, np.uint8)
    image_np = cv2.imdecode(np_arr,1)

    print(len(image_np),len(image_np[0]))
    cv2.imshow('image',image_np)
    cv2.waitKey(1)
    #rospy.loginfo(rospy.get_caller_id() + "I heard %s", data.data)

def listener():

    # In ROS, nodes are uniquely named. If two nodes with the same
    # name are launched, the previous one is kicked off. The
    # anonymous=True flag means that rospy will choose a unique
    # name for our 'listener' node so that multiple listeners can
    # run simultaneously.
    rospy.init_node('listener', anonymous=True)

    rospy.Subscriber("ImgDeliver", CompressedImage, callback)

    # spin() simply keeps python from exiting until this node is stopped
    rospy.spin()

if __name__ == '__main__':
    listener()
