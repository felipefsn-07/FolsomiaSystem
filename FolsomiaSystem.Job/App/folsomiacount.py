import cv2
import numpy as np
import argparse, logging
import sys
import os
from datetime import datetime

file_log = "C:/Users/cs319260/Desktop/TCC/FolsomiaSystem/FolsomiaSystem.Job/App/Log/folsomiacount.log"
cascade_directory = "C:/Users/cs319260/Desktop/TCC/FolsomiaSystem/FolsomiaSystem.Job/App/classifier/cascade.xml"
def log_exist (filename):
    filename =file_log
    if os.path.exists(filename):
        append_write = 'a' # append if already exists
    else:
        append_write = 'w' # make a new file if not

    return append_write


def log_error(error, msg):
    now = datetime.now()
    with open(file_log,log_exist(file_log)) as f:
        f.write(str(now)+" ERROR "+str(msg['method'])+" "+str(error)+"\n")

def log_debug(debug, msg):
    now = datetime.now()
    with open(file_log,log_exist(file_log)) as f:
        f.write(str(now)+" DEBUG "+str(msg['count_res'])+" "+str(debug)+"\n")

def save_res_image(img, dirResImg):
    try:
        cv2.imwrite(dirResImg, img)
    except Exception as e:
        msg = {
            'status': 500,
            'method':"save_res_image", 
            'error':"error when trying to save the result image."   
            }
        print (msg)
        log_error (e, msg)
        sys.exit()


def apply_mask (frame):
    hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    l_w= np.array([15,15,60])
    u_w = np.array([255,255,255])
    mask = cv2.inRange(hsv, l_w, u_w)
    res = cv2.bitwise_and(frame, frame, mask=mask)
    return res

def draw_frame_folsomias (img, folsomias):
    if (len(folsomias)>0):
        for (x, y, w, h) in folsomias:
            center = (x + w//2, y + h//2)
            frame = cv2.ellipse(img, center, (10//2, 10//2), 0, 0, 360, (255,0,255), 2)    
    return img



def correction_gamma(img_original, gamma_max=200):
    ## [changing-contrast-brightness-gamma-correction]
    lookUpTable = np.empty((1,256), np.uint8)
    for i in range(256):
        lookUpTable[0,i] = np.clip(pow(i / 255.0, gamma_max/100) * 255.0, 0, 255)

    res = cv2.LUT(img_original, lookUpTable)

    return res


def recized_image (img, perc):
    scale_percent = perc # percent of original size
    width = int(img.shape[1] * scale_percent / 100)
    height = int(img.shape[0] * scale_percent / 100)

    if (width<2000 and height<2000):
        dim = (width, height)
        # resize image
        resized = cv2.resize(img, dim, interpolation = cv2.INTER_LINEAR)    
        return resized
    return img


def equalization_hist (img):
    img_to_yuv = cv2.cvtColor(img,cv2.COLOR_BGR2YUV)    
    img_to_yuv[:,:,0] = cv2.equalizeHist(img_to_yuv[:,:,0])
    hist_equalization_result = cv2.cvtColor(img_to_yuv, cv2.COLOR_YUV2BGR)
    return hist_equalization_result

def gauss_image (img):
    width = int(img.shape[1])
    height = int(img.shape[0])

    if (width < 1200 and height<1200):
        img = cv2.GaussianBlur(img,(1,1),2)
    else:
        img = cv2.GaussianBlur(img,(3,3),2)
    return img
def folsomia_count(img_url):
    try:
        folsomia_cascade = cv2.CascadeClassifier(cascade_directory)
        frame = cv2.imread(img_url)
        #frame = recized_image (frame, 240)
        gamma = correction_gamma (frame)
        gamma = correction_gamma (gamma)
        blur =gauss_image (gamma) 
        res = apply_mask (blur)
        res = equalization_hist (res)
        _,alpha = cv2.threshold(res,20,255,cv2.THRESH_BINARY)

        
        folsomias = folsomia_cascade.detectMultiScale(alpha)
        
        frame_final = draw_frame_folsomias(frame, folsomias)
        return len(folsomias), frame_final
    
    except Exception as e:
        msg = {
            'status': 500,
            'count_res':0,
            'method':"folsomia_count", 
            'error':"error when trying to count."   
            }
        print (msg)
        log_error (e, msg)
        sys.exit()

# Gather our code in a main() function
def main(args, loglevel):
    logging.basicConfig(format="%(levelname)s: %(message)s", level=loglevel)
    try:
        res, imgRes = folsomia_count(args.file_image)
        if args.file_image_res:
            save_res_image(imgRes, args.file_image_res)
            resFinal = {
                'status': 200,
                'count_res':res,
                'file_image_res':args.file_image_res
            }
        else:
            resFinal = {
                'status': 200,
                'count_res':res,
            }
        print (resFinal)
        log_debug ("", resFinal)
    except Exception as e:
        msg = {
                    'status': 500,
                    'count_res':0,
                    'method':"main", 
                    'error':"error when trying to count."   
            }

        print (msg)
        log_error (e, msg)
        sys.exit()

# the program.
if __name__ == '__main__':
    parser = argparse.ArgumentParser()

    #low_hue, low_saturation, low_value, up_hue, up_saturation, up_value

    parser.add_argument('file_image', help='Add a folsomia image filename result of count ', action="store", type=str)
    parser.add_argument('-ir',dest='file_image_res', action="store", help='Add a folsomia filename result of count ', type=str)  


    parser.add_argument('--version', action='version', version='%(prog)s 1.0')


    args = parser.parse_args()

    loglevel = logging.DEBUG


    main(args, loglevel)
