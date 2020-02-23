import cv2
import numpy as np
import os
def getImages(path):
    videoCap = cv2.VideoCapture(r'%s'%(path))
    frameNum = videoCap.get(cv2.CAP_PROP_FRAME_COUNT)
    i = 0
    hour = 0
    minute = 0
    second = 0
    mill = 0
    fps = videoCap.get(cv2.CAP_PROP_FPS)
    split = int(fps)
    folder = path.split('/')[-1].split('.')[0] + '_images'
    print(folder)
    if not os.path.exists('../../%s/'%(folder)):
        os.makedirs('../../%s/'%(folder))
        while i < frameNum:        
            videoCap.set(cv2.CAP_PROP_POS_FRAMES , i)
            ret , frame = videoCap.read()
            hour = i//fps//60//60
            minute = i//fps//60%60
            second = i//fps%60
            mill = int(i%fps/fps*1000)
            
            cv2.imwrite("../../%s/%02d_%02d_%02d_%03d.bmp"%(folder,hour,minute,second,mill),frame)
            #print(type(frame))
            #cv2.imshow("capture",frame)
            i+=split
            key = cv2.waitKey(1) & 0xFF
            if key == ord('q'):
                break
        videoCap.release()
        cv2.destroyAllWindows()

getImages('F:/PyCharmWorkspace/创新训练项目/src/source/movies/tv.mp4')
    
    