import {Component,  EventEmitter, Input, Output, ViewChild, ViewEncapsulation} from '@angular/core';
import { FolsomiaCountService } from '../../services/folsomia-count-service/folsomia-count.service';
import {FolsomiaCount, FolsomiaCountInput} from '../../models/folsomia-count';
import Cropper from 'cropperjs';
import ViewMode = Cropper.ViewMode;
import { ElementRef } from '@angular/core';
import { BackgroundImage } from '../../enum/BackgroundImage';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'image-editor',
  templateUrl: './image-editor.component.html',
  styleUrls: ['./image-editor.component.scss', ],
  encapsulation: ViewEncapsulation.None
})
export class ImageEditorComponent  {

  @ViewChild('imageEditorContent', {static: false}) content;

  @ViewChild('myInput') myInputVariable: ElementRef;

  public cropper: Cropper;
  public cropperResult:Cropper;
  public outputImage: string;
  public outputResult: string;
  public folsomiaResult:FolsomiaCount;
  public backgroundImage:BackgroundImage;
  public hideLoading:boolean;

  prevZoom = 1;

  @Output() folsomiaResultFinal = new EventEmitter<FolsomiaResult>();

  @Input() modalTitle = '';
  @Input() aspectRatio = 1;
  @Input() autoCropArea = 1;
  @Input() autoCrop = false;
  @Input() mask = true;
  @Input() guides = true;
  @Input() centerIndicator = true;
  @Input() viewMode: ViewMode = 1;
  @Input() modalSize: size;
  @Input() modalCentered = true;
  @Input() scalable = true;
  @Input() zoomable = true;
  @Input() cropBoxMovable = true;
  @Input() cropBoxResizable = true;
  @Input() darkTheme = true;
  @Input() roundCropper = false;
  @Input() canvasHeight = 300;

  public showDragDrop = true;
  public showResult = false;

  public edit:boolean;
  public result:boolean;
  public resultCount:boolean;

  @Input() resizeToWidth: number;
  @Input() resizeToHeight: number;
  @Input() imageSmoothingEnabled = true;
  @Input() imageSmoothingQuality: ImageSmoothingQuality = 'high';
  url: string;
  lastUpdate = Date.now();

  format = 'png';
  quality = 100;

  isFormatDefined = false;

  imageCropped = new EventEmitter<CroppedEvent>();


  constructor(private folsomiaCountService:FolsomiaCountService) {
    this.hideLoading = false;
    this.backgroudDefault();
    this.result = false;
    this.resultCount = false;
    this.edit= true;
    this.folsomiaResult = new FolsomiaCount();
  }

  @Input() set imageQuality(value: number) {
    if (value > 0 && value <= 100) {
      this.quality = value;
    }
  }

  @Input() set imageFormat(type: imageFormat) {
    if ((/^(gif|jpe?g|tiff|png|webp|bmp)$/i).test(type)) {
      this.format = type;
      this.isFormatDefined = true;
    }
  }

  @Input() set imageUrl(url: string) {
    if (url) {
      this.url = url;
      if (this.lastUpdate !== Date.now()) {
        this.lastUpdate = Date.now();
      }
    }
  }

  @Input() set imageBase64(base64: string) {
    if (base64 && (/^data:image\/([a-zA-Z]*);base64,([^\"]*)$/).test(base64)) {
      this.imageUrl = base64;
      if (!this.isFormatDefined) {
        this.format = ((base64.split(',')[0]).split(';')[0]).split(':')[1].split('/')[1];
      }
    }
  }

  @Input() set imageChanedEvent(event: any) {
    if (event) {
      const file = event.target.files[0];
      if (file && (/\.(gif|jpe?g|tiff|png|webp|bmp)$/i).test(file.name)) {
        if (!this.isFormatDefined) {
          this.format = event.target.files[0].type.split('/')[1];
        }
        const reader = new FileReader();
        reader.onload = (ev: any) => {
          this.imageUrl = ev.target.result;
        };
        reader.readAsDataURL(event.target.files[0]);
      }
    }
  }

  @Input() set imageFile(file: File) {
    if (file && (/\.(gif|jpe?g|tiff|png|webp|bmp)$/i).test(file.name)) {
      if (!this.isFormatDefined) {
        this.format = file.type.split('/')[1];
      }
      const reader = new FileReader();
      reader.onload = (ev: any) => {
        this.imageUrl = ev.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  

  onImageLoad(image) {

    image.addEventListener('ready', () => {
      if (this.roundCropper) {
        (document.getElementsByClassName('cropper-view-box')[0] as HTMLElement).style.borderRadius = '50%';
        (document.getElementsByClassName('cropper-face')[0] as HTMLElement).style.borderRadius = '50%';
      }
    });


    this.cropper = new Cropper(image, {
      aspectRatio: this.aspectRatio,
      autoCropArea: this.autoCropArea,
      autoCrop: this.autoCrop,
      modal: this.mask, // black mask
      guides: this.guides, // grid
      center: this.centerIndicator, // center indicator
      viewMode: this.viewMode,
      scalable: this.scalable,
      zoomable: this.zoomable,
      cropBoxMovable: this.cropBoxMovable,
      cropBoxResizable: this.cropBoxResizable,
      background: false
    });
  }

  concentrationChanged(value:number){
      this.folsomiaResult.concetration = value;

  }


  onImageLoadResult(image) {

    this.cropperResult = new Cropper(image, {
      autoCropArea: this.autoCropArea,
      autoCrop: false,
      modal: this.mask, // black mask
      guides: this.guides, // grid
      center: this.centerIndicator, // center indicator
      viewMode: this.viewMode,
      scalable: this.scalable,
      zoomable: this.zoomable,
      cropBoxMovable: false,
      cropBoxResizable: false,
      dragMode:"move",
      background: false
    });
  }

  rotateRight() {
    this.cropper.rotate(45);
  }

  rotateLeft() {
    this.cropper.rotate(-45);
  }

  crop() {

    this.roundCropper = false;
    (document.getElementsByClassName('cropper-view-box')[0] as HTMLElement).style.borderRadius = '0px';
    (document.getElementsByClassName('cropper-face')[0] as HTMLElement).style.borderRadius = '0px';
    
    this.cropper.setDragMode('crop');
  }

  move() {
    this.cropper.setDragMode('move');
  }

  cropRound(){
    this.roundCropper = true;
    if (this.roundCropper) {
      (document.getElementsByClassName('cropper-view-box')[0] as HTMLElement).style.borderRadius = '50%';
      (document.getElementsByClassName('cropper-face')[0] as HTMLElement).style.borderRadius = '50%';
    }
    this.cropper.setDragMode('crop');
  }

  zoom(event) {
    const value = Number(event.target.value);
    this.cropper.zoom(value - this.prevZoom);
    this.prevZoom = value;
  }



  zoomIn() {
    this.cropper.zoom(0.1);
  }

  zoomOut() {
    this.cropper.zoom(-0.1);
  }

  flipH() {
    this.cropper.scaleX(-this.cropper.getImageData().scaleX);
  }

  flipV() {
    this.cropper.scaleY(-this.cropper.getImageData().scaleY);
  }

  reset() {
    this.cropper.reset();
  }

  trash(){
    this.myInputVariable.nativeElement.value = "";
    this.showDragDrop = true;
    this.resultCount = false;
    this.edit= true;
    this.result = false;
    this.url = null;
    this.base64 = null;
    this.outputImage = null;
    this.cropper.destroy();
    this.hideLoading = false;
    this.roundCropper = false;
    this.cropper.destroy();

  }


  backgroudDark(){
    this.backgroundImage = BackgroundImage.Dark;
  }

  backgroudDefault(){
    this.backgroundImage = BackgroundImage.Default;
  }

  backgroudLight(){
    this.backgroundImage = BackgroundImage.Light;
  }

  trashResult(){

    this.trash();
    
    this.folsomiaResult = new FolsomiaCount();
    this.cropperResult.destroy();
    this.cropper.destroy();
    

  }

  
  export() {
    let cropedImage;
    this.backgroudDefault();
    this.showResult = true;
    this.showDragDrop = false;
    this.result = true;
    this.edit= false;

    cropedImage = this.cropper.getCroppedCanvas();
    if (this.roundCropper){

      // Round
      var roundedCanvas = this.getRoundedCanvas(cropedImage);

      this.outputImage = roundedCanvas.toDataURL('image/' + this.format, this.quality);
      cropedImage.toBlob(blob => {
        this.imageCropped.emit({
          base64: this.outputImage,
          file: new File([blob], Date.now() + '.' + this.format, {type: 'image/' + this.format})
        });
      }, 'image/' + this.format, this.quality / 100);
    }else{

      this.outputImage = cropedImage.toDataURL('image/' + this.format, this.quality);
      cropedImage.toBlob(blob => {
        this.imageCropped.emit({
          base64: this.outputImage,
          file: new File([blob], Date.now() + '.' + this.format, {type: 'image/' + this.format})
        });
      }, 'image/' + this.format, this.quality / 100);
    }

  }


  read(files) {
    return new Promise((resolve, reject) => {
      if (!files || files.length === 0) {
        resolve();
        return;
      }
      const file = files[0];
      if (/^image\/\w+$/.test(file.type)) {
        if (URL) {
          resolve({
            loaded: true,
            name: file.name,
            type: file.type,
            url: URL.createObjectURL(file),
          });
        } else {
          reject(new Error('Your browser is not supported.'));
        }
      } else {
        reject(new Error('Please choose an image file.'));
      }
    });
  }


  imageChangedEvent: any;
  base64: any;

  fileChangeEvent(event: any) {
    this.imageChanedEvent = event;
    this.showDragDrop = false;
  }


  getRoundedCanvas(sourceCanvas) {
    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    var width = sourceCanvas.width;
    var height = sourceCanvas.height;

    canvas.width = width;
    canvas.height = height;
    context.imageSmoothingEnabled = true;
    context.drawImage(sourceCanvas, 0, 0, width, height);
    context.globalCompositeOperation = 'destination-in';
    context.beginPath();
    context.arc(width / 2, height / 2, Math.min(width, height) / 2, 0, 2 * Math.PI, true);
    context.fill();
    return canvas;
  }

  change({ target }) {
    this.read(target.files).then((base64) => {
      target.value = '';
      this.showDragDrop = false;
    
      this.update(base64);
    }).catch((e) => {
      target.value = '';
      this.alert(e);
    });
  }

  dragover(e: { preventDefault: () => void; }) {
    e.preventDefault();
  }
  drop(e: { preventDefault: () => void; dataTransfer: { files: any; }; }) {
    e.preventDefault();
    this.read(e.dataTransfer.files)
      .then((base64) => {
        this.base64 = base64;
        this.showDragDrop = false;
      })
      .catch(this.alert);

  }
  alert(e) {
    window.alert(e && e.message ? e.message : e);
  }
  update(base64) {
    Object.assign(this.base64, base64);
  }

  countFolsomia(){

      this.result = false;
      this.resultCount = true;
      var folsomia= new FolsomiaCountInput();
      this.folsomiaResult = new FolsomiaCount();
      folsomia.backgroundImage =this.backgroundImage;
      folsomia.fileAsBase64 = this.outputImage;

      this.count(folsomia);

  }

  count (folsomiaCount: FolsomiaCountInput){


      this.outputResult = "";
       this.folsomiaCountService.countFolsomia(folsomiaCount).subscribe(
        data => (this.folsomiaResult = data,
        this.folsomiaResultFinal.emit({
        
          folsomiaCount: data
        }), this.hideLoading = true),
        error => alert("Erro ao tentar realizar a contagem. Tente novamente!"),
        () => console.log("acesso a webapi post ok...")
     );



  
     }

}

export interface CroppedEvent {
  base64?: string;
  file?: File;
}

export interface FolsomiaResult {
  
  folsomiaCount?:FolsomiaCount;

}

export type imageFormat = 'gif' | 'jpeg' | 'tiff' | 'png' | 'webp' | 'bmp' | 'JPG';

export type size = 'sm' | 'lg';
