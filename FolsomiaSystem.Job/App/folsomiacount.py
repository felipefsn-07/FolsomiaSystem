import cv2
import numpy as np
import argparse, logging
import sys
import os
from datetime import datetime


def log_exist (filename):
    filename = "folsomiacount.log"
    append_write = 'w'
    if os.path.exists(filename):
        append_write = 'a' # append if already exists
    else:
        append_write = 'w' # make a new file if not

    return append_write


def log_error(error, msg):
    now = datetime.now()
    with open("folsomiacount.log",log_exist("folsomiacount.log")) as f:
        f.write(str(now)+" ERROR "+str(msg['method'])+" "+str(error))

def log_debug(debug, msg):
    now = datetime.now()
    with open("folsomiacount.log",log_exist("folsomiacount.log")) as f:
        f.write(str(now)+" DEBUG "+str(msg['count_res'])+" "+str(debug))

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


def apply_mask (frame, l_w, u_w)  :
    hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    mask = cv2.inRange(hsv, l_w, u_w)
    res = cv2.bitwise_and(frame, frame, mask=mask)
    return res

def folsomia_count(img_url, low_hue, low_saturation, low_value, up_hue, up_saturation, up_value):
    try:
        frame = cv2.imread(img_url)
        l_w = np.array([low_hue, low_saturation, low_value])
        u_w = np.array([up_hue, up_saturation, up_value])
        res =  apply_mask (frame, l_w, u_w)      
        gauss = cv2.GaussianBlur(res, (3, 3), 2)
        canny = cv2.Canny(gauss, 50, 150)
        (contornos, _) = cv2.findContours(canny.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        cv2.drawContours(frame, contornos, -1, (0, 0, 255), 2)
        return len(contornos), frame
    except Exception as e:
        msg = {
            'status': 500,
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
        res, imgRes = folsomia_count(args.file_image, args.low_hue, args.low_saturation, args.low_value, args.up_hue, args.up_saturation, args.up_value)
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
    parser.add_argument('-d', help='For dark backgrounds', dest='dark', action="store_true", default=False) 
    parser.add_argument('-l',  help='For clear backgrounds', dest='light', action="store_true", default=False) 
    parser.add_argument('-ir',dest='file_image_res', action="store", help='Add a folsomia filename result of count ', type=str)  
    parser.add_argument('-lh', action="store", dest="low_hue", help="low hue (0,255)", default=0, type=int)
    parser.add_argument('-uh', action="store", dest="up_hue", help="up hue (0,255)",default=255, type=int)
    parser.add_argument('-ls', action="store", dest="low_saturation", help="low hue (0,255)", default=0, type=int)
    parser.add_argument('-us', action="store", dest="up_saturation", help="up hue (0,255)", default=68, type=int)
    parser.add_argument('-lv', action="store", dest="low_value", help="low value (0,255)",default=175, type=int)
    parser.add_argument('-uv', action="store", dest="up_value", help="up value (0,255)",default=255, type=int)

    parser.add_argument('--version', action='version', version='%(prog)s 1.0')


    args = parser.parse_args()
    
    if (args.dark):
        args.low_hue = 0
        args.up_hue = 255
        args.low_saturation = 0
        args.up_saturation = 60
        args.low_value = 145
        args.up_value = 255

    elif (args.light):
        args.low_hue = 0
        args.up_hue = 255
        args.low_saturation = 0
        args.up_saturation = 68
        args.low_value = 180
        args.up_value = 255 
    
    elif (args.dark and args.light):
        args.low_hue = 0
        args.up_hue = 255
        args.low_saturation = 0
        args.up_saturation = 70
        args.low_value = 160
        args.up_value = 255

    loglevel = logging.DEBUG


    main(args, loglevel)
