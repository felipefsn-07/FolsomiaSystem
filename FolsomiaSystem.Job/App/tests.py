import folsomiacount as count
import math
import unittest
import argparse, logging
import numpy as np
import cv2
import glob

class FolsomiaCountTests(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        print('BasicTest.__init__')
        super(FolsomiaCountTests, self).__init__(*args, **kwargs)
        #test_folsomia_count_on_a_blue_image
        self.directory_test = "test_inputs_outputs/inputs/"
        self.directory_out = "test_inputs_outputs/automatic_outputs/"
        #RGB colors



    def test_media_images(self):
        
        listPositive=  glob.glob( self.directory_test+'*.png')
        tx_erros = []
        
        for image in listPositive:
            qtd = image.split("\\")[1].split(".")[0]            
            
            
            res_1, img_res_1 = count.folsomia_count(image)
            count.save_res_image (img_res_1, self.directory_out +str(qtd)+".png")
            per = abs (float(qtd)-float (res_1))/float (qtd)
            tx_erros.append (per)
            print (" res_manual "+str(qtd)+" res_auto "+str(res_1)+" "+ str(int (per*100))+"% ")            
            
        mape = np.sum (tx_erros)/len(tx_erros)*100
        print ("MAPE: "+str(int(mape))+"%")
        self.assertTrue(mape<=8)
        

if __name__ == '__main__':
    unittest.main()
