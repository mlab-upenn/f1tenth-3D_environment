#!/usr/bin/env python
from __future__ import print_function
import sys
import math
import numpy as np

#ROS Imports
import rospy
from geometry_msgs.msg import PoseStamped
from geometry_msgs.msg import Pose


class pose_publisher:
    def __init__(self):
        #Topics & Subscriptions,Publishers
        poseSub_topic = '/gt_pose'
        posePub_topic = '/chatter'

        self.pose_sub = rospy.Subscriber(poseSub_topic,PoseStamped,self.pose_callback)
        self.pose_pub = rospy.Publisher(posePub_topic, Pose, queue_size=10)

    def pose_callback(self, data):
        """ Process each LiDAR scan as per the Follow Gap algorithm & publish an AckermannDriveStamped Message
        """
        #Find the best point in the gap
        pub_msg = Pose()
        pub_msg = data.pose

        #print("velocity",velocity)
        #print("angle", angle)
        self.pose_pub.publish(pub_msg)
        #Publish Drive message

def main(args):
    rospy.init_node("posePublisher", anonymous=True)
    rfgs = pose_publisher()
    #rospy.sleep(0.1)
    rospy.spin()

if __name__ == '__main__':
    main(sys.argv)
