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



## Folder Structure

All main scripts depend on the following subfolders:

1. Folder 1 contains the files for xxx...
2. Folder 2 contains the files for...


## Files
| File | Description |
|----|----|
main.py   | Is used to start the algorithm
test.py | Is used to create the results
