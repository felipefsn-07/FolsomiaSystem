import {NgModule} from '@angular/core';
import {ImageEditorComponent} from './image-editor.component';
import {NgbModalModule} from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [ImageEditorComponent],
  imports: [NgbModalModule],
  exports: [ImageEditorComponent],
})
export class ImageEditorModule {
}
