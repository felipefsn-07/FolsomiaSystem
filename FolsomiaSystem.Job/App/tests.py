import folsomiacount as count
import math
import unittest
import argparse, logging
import numpy as np
import cv2

class FolsomiaCountTests(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        print('BasicTest.__init__')
        super(FolsomiaCountTests, self).__init__(*args, **kwargs)
        #test_folsomia_count_on_a_blue_image
        self.test_in_1 = "test_inputs_outputs/inputs/test_in_1.jpg"
        self.test_out_1  = "test_inputs_outputs/automatic_outputs/test_out_1.jpg"
        self.test_result_manual_1 = 189

        #test_folsomia_count_on_a_dark_image_with_brown_spot
        self.test_in_2 = "test_inputs_outputs/inputs/test_in_2.jpg"
        self.test_out_2  = "test_inputs_outputs/automatic_outputs/test_out_2.jpg"
        self.test_result_manual_2= 182

        #test_folsomia_count_count_on_other_colors
        self.test_in_3 = "test_inputs_outputs/inputs/outraCor.jpg"
        self.test_out_3 = "test_inputs_outputs/automatic_outputs/outraCorResult.jpg"
        self.test_result_manual_3= 25

        #test_folsomia_count_clear_image
        self.test_in_4 = "test_inputs_outputs/inputs/Screenshot_2.jpg"
        self.test_out_4 = "test_inputs_outputs/automatic_outputs/Screenshot_2Result.jpg"
        self.test_result_manual_4= 100

        #
        self.test_in_5 = "test_inputs_outputs/inputs/teste.png"
        self.test_out_5 = "test_inputs_outputs/automatic_outputs/testeResult.jpg"
        self.test_result_manual_5= 316

        #
        self.test_in_6 = "test_inputs_outputs/inputs/teste_1.jpg"
        self.test_out_6 = "test_inputs_outputs/automatic_outputs/teste_1Result.jpg"
        self.test_result_manual_6= 79

        #
        self.test_in_7 = "test_inputs_outputs/inputs/testeEspalhado.jpg"
        self.test_out_7 = "test_inputs_outputs/automatic_outputs/testeEspalhadoResult.jpg"
        self.test_result_manual_7 = 21

        #mask
        self.mask_out_dark = "test_inputs_outputs/automatic_outputs/mask_dark.jpg"
        self.mask_out_light = "test_inputs_outputs/automatic_outputs/mask_light.jpg"
        self.mask_out_light = "test_inputs_outputs/automatic_outputs/mask_dark_light.jpg"


        #RGB colors
        self.low_hue = 0
        self.low_saturation = 0
        self.low_value = 175
        self.up_hue = 255
        self.up_saturation = 68
        self.up_value = 255

    def test_folsomia_count_mask_dark (self):
        frame = cv2.imread(self.test_in_6)
        l_w = np.array([0, 0, 145])
        u_w = np.array([255,60,255])
        res =  count.apply_mask (frame, l_w, u_w)
        count.save_res_image (res, self.mask_out_dark)

    def test_folsomia_count_mask_light (self):
        frame = cv2.imread(self.test_in_4)
        l_w = np.array([0, 0, 175])
        u_w = np.array([255,60,255])
        res =  count.apply_mask (frame, l_w, u_w)
        count.save_res_image (res, self.mask_out_light)

    def test_folsomia_between_dark_and_light (self):
        frame = cv2.imread(self.test_in_7)
        l_w = np.array([0, 0, 160])
        u_w = np.array([255,70,255])
        res =  count.apply_mask (frame, l_w, u_w)
        #count.save_res_image (res, self.mask_out)


    def test_folsomia_count_on_a_blue_image(self):
        res_1, img_res_1 = count.folsomia_count(self.test_in_1, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        count.save_res_image (img_res_1, self.test_out_1)
        per_1 = math.ceil(abs(self.test_result_manual_1-res_1)/self.test_result_manual_1*100)
        print ("test_folsomia_count_on_a_blue_image - res:"+str(per_1))
        self.assertTrue(per_1<=8)
        self.assertFalse(per_1>8)
            
    def test_folsomia_count_on_a_dark_image_with_brown_spot(self):
        res_2, img_res_2 = count.folsomia_count(self.test_in_2, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        count.save_res_image (img_res_2, self.test_out_2)
        per_2 = math.ceil(abs(self.test_result_manual_2-res_2)/self.test_result_manual_2*100)
        print ("test_folsomia_count_on_a_dark_image_with_brown_spot - res:"+str(per_2))
        self.assertTrue(per_2<=8)
        self.assertFalse(per_2>8)

    def test_folsomia_count_count_on_other_colors(self):
        res_3, img_res_3 = count.folsomia_count(self.test_in_3, 0, 0, 145, 255,60,255)
        count.save_res_image (img_res_3, self.test_out_3)
        per_3 = math.ceil(abs(self.test_result_manual_3-res_3)/self.test_result_manual_3*100)
        print ("test_folsomia_count_count_on_other_colors - res:"+str(per_3))
        self.assertTrue(per_3<=8)
    
    def test_folsomia_count_clear_image (self):
        res_4, img_res_4 = count.folsomia_count(self.test_in_4, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        count.save_res_image (img_res_4, self.test_out_4)
        per_4 = math.ceil(abs(self.test_result_manual_4-res_4)/self.test_result_manual_4*100)
        print ("test_folsomia_count_count_on_other_colors - res:"+str(per_4))
        self.assertTrue(per_4<=8)
        self.assertFalse(per_4>8)

    def test_folsomia_count_on_a_brown_image (self):
        res_5, img_res_5 = count.folsomia_count(self.test_in_5, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        count.save_res_image (img_res_5, self.test_out_5)
        per_5 = math.ceil(abs(self.test_result_manual_5-res_5)/self.test_result_manual_5*100)
        print ("test_folsomia_count_on_a_brown_image - res:"+str(per_5))
        self.assertTrue(per_5<=8)
        self.assertFalse(per_5>8)
    
    def test_folsomia_count_on_a_brown_image (self):
        res_6, img_res_6 = count.folsomia_count(self.test_in_6, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        count.save_res_image (img_res_6, self.test_out_6)
        per_6 = math.ceil(abs(self.test_result_manual_6-res_6)/self.test_result_manual_6*100)
        print ("test_folsomia_count_on_a_brown_image - res:"+str(per_6))
        self.assertTrue(per_6<=8)
        self.assertFalse(per_6>8)
    
    def test_folsomia_count_spread (self):
        res_7, img_res_7 = count.folsomia_count(self.test_in_7, 0, 0, 145, 255,60,255)
        count.save_res_image (img_res_7, self.test_out_7)
        per_7 = math.ceil(abs(self.test_result_manual_7-res_7)/self.test_result_manual_7*100)
        print ("test_folsomia_count_spread - res:"+str(per_7))
        self.assertTrue(per_7<=8)
    
    def test_folsomia_count_all_image_default(self):
        res_1, img_res_1 = count.folsomia_count(self.test_in_1, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        res_2, img_res_2 = count.folsomia_count(self.test_in_2, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        res_3, img_res_3 = count.folsomia_count(self.test_in_3, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        res_4, img_res_4 = count.folsomia_count(self.test_in_4, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        res_5, img_res_5 = count.folsomia_count(self.test_in_5, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        res_6, img_res_6 = count.folsomia_count(self.test_in_6, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        res_7, img_res_7 = count.folsomia_count(self.test_in_7, self.low_hue, self.low_saturation, self.low_value, self.up_hue, self.up_saturation, self.up_value)
        per_1 = math.ceil(abs(self.test_result_manual_1-res_1)/self.test_result_manual_1*100)
        per_2 = math.ceil(abs(self.test_result_manual_2-res_2)/self.test_result_manual_2*100)
        per_3 = math.ceil(abs(self.test_result_manual_3-res_3)/self.test_result_manual_3*100)
        per_4 = math.ceil(abs(self.test_result_manual_4-res_4)/self.test_result_manual_4*100)
        per_5 = math.ceil(abs(self.test_result_manual_5-res_5)/self.test_result_manual_5*100)
        per_6 = math.ceil(abs(self.test_result_manual_6-res_6)/self.test_result_manual_6*100)
        per_7 = math.ceil(abs(self.test_result_manual_7-res_7)/self.test_result_manual_7*100)
        test_result_manual_all = self.test_result_manual_1+self.test_result_manual_2+self.test_result_manual_3+self.test_result_manual_4+self.test_result_manual_5+self.test_result_manual_6+self.test_result_manual_7
        res_total = res_1+res_2+res_3+res_4+res_5+res_6+res_7
        per_total = math.ceil(abs(test_result_manual_all - res_total)/(test_result_manual_all)*100)
        print ("test_folsomia_count_all_image - res:"+str(per_total))
        self.assertTrue (per_total<=8)
        

if __name__ == '__main__':
    unittest.main()