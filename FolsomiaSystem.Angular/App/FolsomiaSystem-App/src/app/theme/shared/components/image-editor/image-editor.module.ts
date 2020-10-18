import {NgModule} from '@angular/core';
import {ImageEditorComponent} from './image-editor.component';
import {NgbModalModule} from '@ng-bootstrap/ng-bootstrap';
import {CardModule} from '../card_footer/card.module';


@NgModule({
  declarations: [ImageEditorComponent],
  imports: [
    NgbModalModule, 
    CardModule    
  ],
  exports: [ImageEditorComponent],
})
export class ImageEditorModule {
}
