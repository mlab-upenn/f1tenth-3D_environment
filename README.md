# f1tenth-3D_environment
This is the github repository for thedevelopment of the F1TENTH 3D Environment based on unity.

## Requirements
- Linux Ubuntu (tested on versions XX.XX and XX.XX)
- Python 3.XX.
- ....

## Installation
Use the provided `requirements.txt` in the root directory of this repo, in order to install all required modules.\
`pip3 install -r /path/to/requirements.txt`


The code is developed with Python 3.XX.

## Running the code
### ZeroMQ Bridge Demo
1. Start Unity project `Unity_ZeroMQ` and open the `ZeroMQ_Demo` scene and click the `Play` button
2. Navigate to `ZeroMQ_Bridge/Python_ZeroMQ`, then bring up the client and server
```
python server.py
python client.py
```
3. From the Unity you should see the box is moving in the direction given by the `server.py`, and a lot of images are saving into the `Python_ZeroMQ` folder

### Ros Bridge Demo
1. Under the `ROS_catkin_ws` build the relevant packages
```
catkin_make
source /devel/setup.bash
```
2. Bring up Websocket, F1tenth simulator, a simple controller. Go to the terminal that bring up F1tenth simulator, press `n` to get the vehicle run
```
roslaunch rosbridge_server rosbridge_websocket.launch
roslaunch f1tenth_simulator simulator.launch
rosrun wall_following xiaozhou_zhang_wallfollow.py 
```
3. Bring up the bridge from F1tenth to Unity and Unity to F1tenth
```
rosrun unity_bridge posePublisher.py
rosrun unity_bridge test_listener_ros.py 
```
4. Start Unity project `Unity_ROS_Bridge` and open the `ROS_Bridge_Demo` scene and click the `Play` button




## Folder Structure

All main scripts depend on the following subfolders:

1. Folder 1 contains the files for xxx...
2. Folder 2 contains the files for...


## Files
| File | Description |
|----|----|
main.py   | Is used to start the algorithm
test.py | Is used to create the results
