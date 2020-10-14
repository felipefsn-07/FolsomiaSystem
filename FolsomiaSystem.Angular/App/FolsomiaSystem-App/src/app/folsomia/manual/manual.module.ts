import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { ManualRoutingModule } from './manual-routing.module';
import { ManualComponent } from './manual.component';
import {SharedModule} from '../../theme/shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    ManualRoutingModule,
    SharedModule,
  ],
  
  declarations: [ManualComponent]
})
export class ManualModule { }
