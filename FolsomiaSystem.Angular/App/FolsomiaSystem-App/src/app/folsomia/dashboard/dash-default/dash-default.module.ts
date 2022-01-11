import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashDefaultRoutingModule } from './dash-default-routing.module';
import { DashDefaultComponent } from './dash-default.component';
import {SharedModule} from '../../../theme/shared/shared.module';
import {NgbButtonsModule, NgbDropdownModule, NgbTooltipModule, NgbCollapseModule,NgbTabsetModule } from '@ng-bootstrap/ng-bootstrap';
import { FileSaverModule } from 'ngx-filesaver';


@NgModule({
  declarations: [DashDefaultComponent],
  imports: [
    CommonModule,
    DashDefaultRoutingModule,
    SharedModule,
    NgbButtonsModule,
    NgbDropdownModule,
    NgbTooltipModule ,
    NgbTabsetModule,
    NgbCollapseModule,
    FileSaverModule
  ]
})
export class DashDefaultModule { }
