#!/usr/bin/env python
from __future__ import print_function
import sys
import math
import numpy as np

#ROS Imports
import rospy
from sensor_msgs.msg import Image, LaserScan
from ackermann_msgs.msg import AckermannDriveStamped, AckermannDrive
from nav_msgs.msg import Odometry

#PID CONTROL PARAMS
kp = 1.9#TODO 0.9 1.9
kd = 0.1#TODO 0.1
ki = 0#TODO
servo_offset = 0.0
prev_error = 0.0 
error = 0.0
integral = 0.0

#WALL FOLLOW PARAMS
ANGLE_RANGE = 270 # Hokuyo 10LX has 270 degrees scan
DESIRED_DISTANCE_RIGHT = 0.9 # meters
DESIRED_DISTANCE_LEFT = 0.55
VELOCITY = 2.00 # meters per second
CAR_LENGTH = 0.50 # Traxxas Rally is 20 inches or 0.5 meters

class WallFollow:
    """ Implement Wall Following on the car
    """
    def __init__(self):

        self.speed = 0
        #Topics & Subs, Pubs
        lidarscan_topic = '/scan'
        drive_topic = '/nav'
        odometry_topic = '/odom'
        
        self.odom_sub = rospy.Subscriber(odometry_topic, Odometry, self.odom_callback)
        self.lidar_sub = rospy.Subscriber(lidarscan_topic, LaserScan, self.lidar_callback)#TODO: Subscribe to LIDAR
        self.drive_pub = rospy.Publisher(drive_topic, AckermannDriveStamped, queue_size=10)#TODO: Publish to drive
    
    def getRange(self, data, angle):
        # data: single message from topic /scan
        # angle: between -45 to 225 degrees, where 0 degrees is directly to the right
        # Outputs length in meters to object with angle in lidar scan field of view
        #make sure to take care of nans etc.
        #TODO: implement
        ranges = data.ranges
        angle_min = data.angle_min
        angle_increment = data.angle_increment
        index = int(round((angle - angle_min)/angle_increment))
        if (~np.isnan(ranges[index])):
            return ranges[index]
        
        return 0

    def pid_control(self, error, velocity):
        global integral
        global prev_error
        global kp
        global ki
        global kd
        
        dt = 0.1
        integral = integral + error*dt
        angle = error*kp + kd*(error - prev_error)/dt + ki*integral
        prev_error = error


        if (abs(math.degrees(angle))<10):
            velocity = 1.5 #7.5
        elif (abs(math.degrees(angle))<20):
            velocity = 1 #5
        else:
            velocity = 0.5 #5
                

        #TODO: Use kp, ki & kd to implement a PID controller for 
        drive_msg = AckermannDriveStamped()
        drive_msg.header.stamp = rospy.Time.now()
        drive_msg.header.frame_id = "laser"
        drive_msg.drive.steering_angle = angle
        drive_msg.drive.speed = velocity
        self.drive_pub.publish(drive_msg)

    def followLeft(self, leftDist):
        #Follow left wall as per the algorithm 
        #TODO:implement
        desired_dist = 0.9 # mid pos
        error = -desired_dist + leftDist
        return error 

    def lidar_callback(self, data):
        """ 
        """
        angle_min = data.angle_min
        angle_left = angle_min+3.0/2*math.pi
        theta = 1/4*math.pi # set theta = 1/4*pi
        angle_theta = angle_left - theta 
        b = self.getRange(data, angle_left)
        a = self.getRange(data, angle_theta)
        
        alpha = math.atan2((a*math.cos(theta)-b), (a*math.sin(theta)))

        d_t = b*math.cos(alpha)
        l = 0.2*self.speed
        d_t_1 = d_t + l*math.sin(alpha) 

        error = self.followLeft(d_t_1) #TODO: replace with error returned by followLeft
        #send error to pid_control
        VELOCITY = 1.5
        self.pid_control(error, VELOCITY)
    
    def odom_callback(self, data):
        self.speed = data.twist.twist.linear.x

def main(args):
    rospy.init_node("WallFollow_node", anonymous=True)
    wf = WallFollow()
    rospy.sleep(0.1)
    rospy.spin()

if __name__=='__main__':
	main(sys.argv)
