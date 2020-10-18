import { Component, OnInit, ViewChild, Input, ElementRef } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import Stepper from 'bs-stepper';
import {FolsomiaSetupService} from '../../../theme/shared/services/folsomia-setup-service/folsomia-setup.service';
import { FolsomiaSetup } from 'src/app/theme/shared/models/folsomia-setup';
import { FolsomiaCount } from 'src/app/theme/shared/models/folsomia-count';
import { FolsomiaResult } from 'src/app/theme/shared/components/image-editor/image-editor.component';

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
 public ini:boolean;
 public iniForm: FormGroup;
  public folsomia: FolsomiaCount;
  private folsomiaResults:Array<FolsomiaCount>;
  //Criar contante 
 folsomiaSetup: FolsomiaSetup;  


  saveCountFolsomia(){

    this.folsomiaResults.push(this.folsomia);
    this.folsomia = new FolsomiaCount();

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


  constructor(private _formBuilder: FormBuilder, private folsomiaSetupService:FolsomiaSetupService) {
    this.radioButtons = '1';
        this.folsomia = new FolsomiaCount();
    this.folsomia.totalCountFolsomia = 0;

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


  ngOnInit() {

    this.validation();

    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }

  folsomiaResult(event: FolsomiaResult) {
    this.folsomia = event.folsomiaCount;
  }


  getFolsomiaSetup (){

    this.folsomiaSetupService.getSetup().subscribe((data: any) => this.folsomiaSetup = data);

  }

}
