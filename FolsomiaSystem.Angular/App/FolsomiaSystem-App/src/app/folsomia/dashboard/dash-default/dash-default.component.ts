import { Component, ElementRef, OnInit, Injectable, HostListener} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {FolsomiaSetupService} from '../../../theme/shared/services/folsomia-setup-service/folsomia-setup.service';
import { FolsomiaSetup } from 'src/app/theme/shared/models/folsomia-setup';
import { FolsomiaCount } from 'src/app/theme/shared/models/folsomia-count';
import { FolsomiaResult } from 'src/app/theme/shared/components/image-editor/image-editor.component';
import { NotificationService } from '../../../theme/shared/services/notification-service/notification.service';
import {UiModalComponent} from '../../../theme/shared/components/modal/ui-modal/ui-modal.component';
import { animate, AUTO_STYLE, state, style, transition, trigger } from '@angular/animations';
import { ViewChild } from '@angular/core';
import { saveAs } from 'file-saver';
import { FileSaverService } from 'ngx-filesaver';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Http } from '@angular/http/src/http';
import { ResponseContentType } from '@angular/http/src/enums';
import { Observable } from 'rxjs/Observable';


@Component({
  selector: 'app-dash-default',
  templateUrl: './dash-default.component.html',
  styleUrls: ['./dash-default.component.scss'],
  animations: [
    trigger('collapsedCard', [
      state('collapsed, void',
        style({
          overflow: 'hidden',
          height: '0px',
        })
      ),
      state('expanded',
        style({
          overflow: 'hidden',
          height: AUTO_STYLE,
        })
      ),
      transition('collapsed <=> expanded', [
        animate('400ms ease-in-out')
      ])
    ]),
    trigger('cardRemove', [
      state('open', style({
        opacity: 1
      })),
      state('closed', style({
        opacity: 0,
        display: 'none'
      })),
      transition('open <=> closed', animate('400ms')),
    ])
  ]
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
  public folsomiaResults:Array<FolsomiaCount>=[];
  public media:number;
  public maxValue:boolean;
  public cleanTest:boolean;
  public animation: string;
  public fullIcon: string;
  public isAnimating: boolean;
  public folsomiaEdit:FolsomiaCount;
  public folsomiaDelete:FolsomiaCount;

  public collapsedCard: string;
  public collapsedIcon: string;
  public loadCard: boolean;
  public cardRemove: string;


  @HostListener("window:beforeunload")  
 selloutcanDeactivate(): Observable<boolean> | boolean {  
     return (  
         false 
     );  
 } 

  @ViewChild('modal') modal: UiModalComponent;
  @ViewChild('modalDelete') modalDelete: UiModalComponent;

  @ViewChild('uploader') uploader:ElementRef;

  //Criar contante 
 folsomiaSetup: FolsomiaSetup;  


  calcMedia(){
    var totalTests = 0;
    var totalValue = 0;

    this.folsomiaResults.forEach((folsomia, index) => {
      totalTests++;
      totalValue += folsomia.totalCountFolsomia;

      }
    );

    if (totalTests>0){
      this.media = totalValue/totalTests;
    }else{
      this.media = 0;
    }
  }

  saveTest(){

    var array ={"iniForm":this.iniForm.value,"folsomiaResults": this.folsomiaResults};
    var theJSON = JSON.stringify(array);
    var data = new Blob([theJSON], { type: 'json/plain;charset=utf-8'},)
    this._FileSaverService.save(data, this.iniForm.value.tituloTeste+".folsys");

  }

  openTest(event){
    let file:any;
    let fileEx:any;
  
    file = event.target.files[0];
    this.readFile(file).then(  //success fn
      (x) => { 
        try{ 
          //do something
          fileEx = JSON.parse(x.toString());
          this.folsomiaEdit = new FolsomiaCount();
          this.folsomiaDelete = new FolsomiaCount();
          this.folsomiaResults = [];
          this.folsomiaResults = fileEx.folsomiaResults;
          this.iniForm.setValue(fileEx.iniForm);
          this.calcMedia();
          this.ini = false;
          this.uploader.nativeElement.value = "";
        }catch (error) {
          this.notifyService.showError("Esse arquivo não é permitido!", "");
          console.error('Here is the error message', error);
        }
      });



  }

  readFile(file) {
    return new Promise((resolve, reject) => {
        const fileReader = new FileReader();
        fileReader.onload = () => {
            resolve(fileReader.result);
        };
        fileReader.onerror = () => {
            reject(fileReader.error);
        };
        fileReader.readAsText(file);
    });
}


  addNewTest(){

  if (this.folsomiaResults.length == this.folsomiaSetup.maxConcentration){
      this.warningMaxConcentr();

    }else{
      this.cleanTest = true; 
      this.folsomiaEdit = new FolsomiaCount();
      this.modal.show();
    }
    

  }


  saveEdit (){
    var index = this.folsomiaResults.indexOf(this.folsomiaEdit);
    this.folsomiaResults[index] = this.folsomia;
      this.folsomiaEdit = new FolsomiaCount();
      this.cleanTest = false;
      this.calcMedia();
      this.modal.hide();
  }

  saveCountFolsomia(){

    if (this.folsomiaEdit.fileResult.fileAsBase64 != ""){
      this.saveEdit ();
    }else{
      
      if (this.folsomiaResults.length< this.folsomiaSetup.maxConcentration){
        if (this.folsomia.totalCountFolsomia>0 && this.folsomia.fileResult.fileAsBase64!=""){
          this.cleanTest = false;
          
          this.folsomiaResults.push(this.folsomia);
          this.calcMedia();
          this.modal.hide();
          this.folsomia = new FolsomiaCount();
        }else{

          this.notifyService.showError("Insira uma imagem para finalizar!", "");
        }
      }else{
        this.warningMaxConcentr();
      } 
    }
  }

  validation(){
    this.iniForm = new FormGroup({
      tituloTeste: new FormControl('', Validators.required),
      substancia: new FormControl('', Validators.required),
      description: new FormControl(''),
    })
  }

  startTest(){

    if (this.iniForm.valid){
      this.ini= false;
    }else{

      this.errorValidationForm();
    }
  }

  warningMaxConcentr(){
    this.notifyService.showWarning("O máximo de concetrações em um teste foi atingido!", "");
  }

  errorValidationForm (){

    this.notifyService.showError("Os campos obrigatórios devem ser preenchidos!", "");
  }


  constructor(private _formBuilder: FormBuilder, private folsomiaSetupService:FolsomiaSetupService, private notifyService : NotificationService, private _FileSaverService: FileSaverService) {
    this.media = 0;
    this.radioButtons = '1';
    this.folsomia = new FolsomiaCount();
    this.folsomiaEdit = new FolsomiaCount();

    this.folsomia.totalCountFolsomia = 0;
    this.maxValue = false;
    this.folsomiaDelete =new FolsomiaCount();

    this.checkBox = {
      left: true,
      center: false,
      right: false
    };

    this.ini = true;
    this.getFolsomiaSetup();


  }
  buscarDadosNoBanco(): void {
    throw new Error('Method not implemented.');
  }


  close() {
    this.cleanTest = false;
    this.modal.hide();
  }


  ngOnInit() {

    this.validation();
    this.cleanTest = false;

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

  cardRemoveAction(item:FolsomiaCount) {
      this.modalDelete.show();
      this.folsomiaDelete = item;


  }


  confirmDelete (){
      this.cardRemove = this.cardRemove === 'closed' ? 'open' : 'closed';
      var index = this.folsomiaResults.indexOf(this.folsomiaDelete);
      this.folsomiaResults.splice(index, 1); 
      this.cleanTest = false;
      this.calcMedia();
      if (this.maxValue == true){
        this.maxValue = false;
      }
      this.modalDelete.hide();
  }
  
  cardEditAction(folsomiaEdit:FolsomiaCount) {
    this.folsomiaEdit = folsomiaEdit;
    this.modal.show();
    

  }


}
