import { Component, OnInit, ViewChild, Input, ElementRef } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {CroppedEvent} from '../../../theme/shared/components/image-editor/image-editor.component';
import Stepper from 'bs-stepper';
import {FolsomiaSetupService} from '../../../theme/shared/services/folsomia-setup-service/folsomia-setup.service';
import { Observable } from 'rxjs';
import { FolsomiaSetup } from 'src/app/theme/shared/models/folsomia-setup';

@Component({
  selector: 'app-dash-default',
  templateUrl: './dash-default.component.html',
  styleUrls: ['./dash-default.component.scss']
})


export class DashDefaultComponent implements OnInit {

  public radioButtons: string;
  public checkBox: any;
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  private stepper: Stepper;
 public ini:boolean;
 public iniForm: FormGroup;
 folsomiaSetup: FolsomiaSetup;  

  next() {
    this.stepper.next();
  }

  validation(){
    this.iniForm = new FormGroup({
      tituloTeste: new FormControl('', Validators.required),
      substancia: new FormControl('', Validators.required),
      description: new FormControl(''),


    })
  }

  startTest(){

    this.ini= false;
  }

  previous() {
    this.stepper.previous();
  }
  constructor(private _formBuilder: FormBuilder, private folsomiaSetupService:FolsomiaSetupService) {
    this.radioButtons = '1';
    this.checkBox = {
      left: true,
      center: false,
      right: false
    };

    this.ini = true;
    this.getFolsomiaSetup();


  }


  public close() {

  }

  public getEditedFile(file: File) {

  }

  ngOnInit() {
    this.validation();
    this.stepper = new Stepper(document.querySelector('#stepper1'), {
      linear: false,
      animation: true
    })
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }

  


  getFolsomiaSetup (){

    this.folsomiaSetupService.getSetup().subscribe((data: any) => this.folsomiaSetup = data);

  }

}
