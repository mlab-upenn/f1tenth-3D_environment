# f1tenth-3D_environment
This is the github repository for thedevelopment of the F1TENTH 3D Environment based on unity.

## Installation
1. Download this repo
```bash
$ git clone https://github.com/mlab-upenn/f1tenth-3D_environment.git
```
2. Install Unity Editor through [unity hub](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html). The project is developed under Unity Version: 2019.3.0f1
3. Install the dependecies in the `requirements.txt` as `pip install -r requirements.txt`
4. Install the gym environment
```bash
$ cd f1tenth_gym-camera_dev_billy
$ pip3 install --user -e gym/
```

## Running the code
### ZeroMQ Bridge with f1tenth_gym demo
1. Start Unity project `Unity_ZeroMQ` and open the `ZeroMQ_Demo` scene through unity hub which you installed before, and click the `Play` button.
2. In the root folder, type the following command which would start two cars racing.
```bash
cd f1tenth_gym-camera_dev_billy/examples
python3 waypoint_follow_multi.py
```
3. In the unity `Play` mode, one can click the button `Switch Method` for one car racing
4. In the root folder, type the following command which would start one car racing.
```bash
cd f1tenth_gym-camera_dev_billy/examples
python3 waypoint_follow_single.py
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




## ZeroMQ_Bridge Structure

The `ZeroMQ_Bridge` has the structure as
```C
- ZeroMQ_Bridge
    /* demo zmq code for the gym */
    - Python_ZeroMQ
        /* demo zmq client code receiving img from the unity side. used for reference in the gym environment */
        - client.py 
        /* demo zmq server code sending pos info to the unity side. used for reference in the gym environment */
        - server.py
    /* Unity files */
    - Unity_ZeroMQ
        - Assets // used Unity assets //
            - ARCADE - FREE Racing Car // car model assets
            - Concrete textures pack // concrete wall material
            - Free // wall model assets
            - Maps // maps used in the gym side
            - Materials // other materials
            - Plugins // ZeroMQ plugins
            - Prefabs // ground and wall prefabs
            - Scenes // saved ZeroMQ_Demo scene
            - Scripts // used scripts
                - CameraC.cs // mount the camera onto the car and follow its translation and orientation
                - MapReader.cs // read the map from ../Maps and generate the walls on the floor
                - Publisher.cs // publish the camera img to the gym side
                - Subscriber.cs // subscribe the location and orientation info of the car from the gym side
                - SwitchMethod // a button for switching between one car racing and two cars racing
        - Other derivatives
```

## ZeroMQ_Demo Unity Scene Hierarchy
The scene has the following hierarchy
```C
- MapReader // an empty object loading MapReader.cs
- Plane // the floor object
- Player1 // car#1
    - Main Camera // cam mounted on car#1
    - ZmqConnector // empty object loading Publisher.cs and Subscriber.cs for car#1
    - Free_Racing_Car_Blue // car model object for car#1
- Player2 // car#2
    - Main Camera2 // cam mounted on car#2
    - ZmqConnector2 // empty object loading Publisher.cs and Subscriber.cs for car#2
    - Free_Racing_Car_Red // car model object for car#1
- Canvas // canvas for the switching button
    - Button // switch methond
- EventSystem // system object for switching function
```

## ZeroMQ_Unity Code params
1. MapReader.cs
```C
Map // the same map file from the gym with the config below
Wall Object // object for generating the wall
Material // wall material
Resolution // the same map resolution of the gym side
```
![](../f1tenth-3D_environment/document/map_config.png)

2. CameraC.cs
```C
Target // the mounted car's transform
Offset Position // the translation between cam's frame and the car's frame
```
For the physical camera stats, one can refer to [Unity Physical Cameras](https://docs.unity3d.com/Manual/PhysicalCameras.html) and the Inspector info listed on the camera object. The FPS is 30.

3. Publisher.cs
```C
PortNum // zmq port num for publishing img to the gym side. hardcoded as 12346 now on the gym side
Image Camera // the camera object
Resolution Width/Height // camera resolution
Quality level // published img quality
```
It refers to [this repo](https://github.com/valkjsaaa/Unity-ZeroMQ-Example).

4. Subscriber.cs
```C
Player // the car object
PortNum // zmq port num for subscribing car's pose fromt he gym side. hardcoded as 12345 now on the gym side
```
It refers to [this repo](https://github.com/valkjsaaa/Unity-ZeroMQ-Example).

5. SwitchMethod.cs
```C
Second Car // the car#2 object
```

## Video Demo link
[Demo](https://drive.google.com/file/d/17mPikjnLHj_oWPZpzE_cKq1EdGVkt0eA/view?usp=sharing)
