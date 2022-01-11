import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from './dashboard-routing.module';
import {SharedModule} from '../../theme/shared/shared.module';
import { CanDeactivateGuard } from '../../theme/shared/models/sample-changes-guard';  



@NgModule({
  imports: [
    CommonModule,
    DashboardRoutingModule,
    SharedModule
    
  ],
  providers: [
    CanDeactivateGuard
  ]
})
export class DashboardModule { }
