import numpy as np
import cv2 as cv
import matplotlib.pyplot as plt
import json
import time
import os
import os.path
import base64

from os import path
from flask import request, Flask

#Fields
imageFormat = '.jpg'
resourcesDirectoryPath = 'Resources'
images = []
threshould = 0.5

#Methods

# Initiate ORB detector
orb = cv.ORB_create()
# create BFMatcher object
bf = cv.BFMatcher(cv.NORM_HAMMING, crossCheck=True)

# (descriptors, roomName)
def LoadImages():
    subfolders = [folder.name for folder in os.scandir(resourcesDirectoryPath) if folder.is_dir()]
    images = []
    for folder in subfolders:
        localPath = os.path.join(resourcesDirectoryPath, folder)
        imgs = os.listdir(localPath)
        for img in imgs:
            localPathImg = os.path.join(localPath, img)
            imgArray = cv.imread(localPathImg, cv.IMREAD_GRAYSCALE)
            images.append((GetDescriptors(imgArray), folder))
    return images

def WriteImage(roomName, image):
    if path.exists(resourcesDirectoryPath) == False:
        os.mkdir(resourcesDirectoryPath)
    pathToRoomName = path.join(resourcesDirectoryPath, roomName)
    if path.exists(pathToRoomName) == False:
        os.mkdir(pathToRoomName)
    imageNamePath = int(len(os.listdir(pathToRoomName))) + 1
    imageNamePath = str(imageNamePath) + imageFormat
    imageNamePath = path.join(pathToRoomName, imageNamePath)
    array = np.fromstring(image, np.uint8)
    img = cv.imdecode(array, cv.IMREAD_COLOR)
    cv.imwrite(imageNamePath, img)

def GetKeypointsAndDescriptors(img):
    return orb.detectAndCompute(img, None)

def GetDescriptors(img):
    keypointsCamera, descriptorsCamera = GetKeypointsAndDescriptors(img)
    return descriptorsCamera

def FilterMatches(matches, matchesThreshold):
    distances = [match.distance for match in matches]
    if(len(distances) != 0):
        sumDistances = sum(distances)
        lenDistances = len(distances)
        threshold_distance = (sumDistances / lenDistances) * matchesThreshold
    matches = [match for match in matches if match.distance < threshold_distance]
    return matches

def GetMatches(des1, des):
    try:
        matches = bf.match(des1 ,des)
    except:
        matches = []
        print("Something was wrong.")
    return FilterMatches(matches, threshould)

def FindRoom(descriptorCamera):
    match = [0,0,0,0,0]
    roomName = ""
    for img in images:
        localMatch = GetMatches(img[0], descriptorCamera)
        if(len(localMatch) > len(match)):
            match = localMatch
            roomName = img[1]
    return roomName

# Flask
app = Flask(__name__)

@app.route('/admin', methods = ['GET'])
def GetRooms():
    if request.method == 'GET':
        subfolders = [folder.name for folder in os.scandir(resourcesDirectoryPath) if folder.is_dir()]
        localJson = json.dumps(subfolders)
        return localJson

@app.route('/user', methods = ['GET'])
def PrepareRooms():
    global images
    if request.method == 'GET':
        images = LoadImages()
        return "Success"

@app.route('/send', methods = ['PUT'])
def Receive():
    if request.method == 'PUT':
        data = request.get_data()
        imageData = json.loads(data.decode("utf-8"))
        WriteImage(imageData.get('roomName'), base64.b64decode(imageData.get('image')))
        return "Success"

@app.route('/detection', methods = ['PUT'])
def Detection():
    if request.method == 'PUT':
        data = request.get_data()

        array = np.fromstring(data, np.uint8)
        img = cv.imdecode(array, cv.COLOR_BGR2GRAY)

        value = FindRoom(GetDescriptors(img))
        return value

app.run(host = '0.0.0.0', port = '5000')