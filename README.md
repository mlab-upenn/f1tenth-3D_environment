# f1tenth-3D_environment
This is the github repository for thedevelopment of the F1TENTH 3D Environment based on unity.

## Requirements
- Linux Ubuntu (tested on versions XX.XX and XX.XX)
- Python 3.XX.
- ....

## Installation
1. Download the f1tenth_gym env and follow the intructions on [f1tenth_gym](https://github.com/f1tenth/f1tenth_gym) to install the environment
```bash
git clone https://github.com/f1tenth/f1tenth_gym.git
```
2. Download this repo
```bash
git clone https://github.com/mlab-upenn/f1tenth-3D_environment.git
```
3. Install Unity Editor through [unity hub](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html). The project is developed under Unity Version: 2019.3.0f1
4. Install the dependecies in the `requirements.txt` as `pip install -r requirements.txt`



## Running the code
### ZeroMQ Bridge with f1tenth_gym demo
1. Start Unity project `Unity_ZeroMQ` and open the `ZeroMQ_Demo` scene and click the `Play` button
2. Subsitute the files of in the [f1tenth_gym](https://github.com/f1tenth/f1tenth_gym) with the files in `ZeroMQ_Bridge/f1tenth_gym_sub/`
3. In the [f1tenth_gym](https://github.com/f1tenth/f1tenth_gym) workspace, run the following command to reinstall the updated env
```bash
cd gym
pip install -e .
```
4. In the [f1tenth_gym](https://github.com/f1tenth/f1tenth_gym) workspace, run the following command to bring up the car in the gym and in the Unity as well
```bash
cd examples
python3 waypoint_follow.py
```
5. One can then go back to this repo's workspace, run the following command to get a live stream demo of the images taken in Unity
```bash
cd ZeroMQ_Bridge/Python_ZeroMQ
python client.py
 
```

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
