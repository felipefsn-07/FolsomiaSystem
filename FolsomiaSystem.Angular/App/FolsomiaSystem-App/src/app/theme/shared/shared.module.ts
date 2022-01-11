import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlertModule, BreadcrumbModule, CardModule, ModalModule } from './components';
import { DataFilterPipe } from './components/data-table/data-filter.pipe';
import { PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarConfigInterface, PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { ClickOutsideModule } from 'ng-click-outside';
import {ImageEditorModule} from './components/image-editor/image-editor.module';
import {FolsomiaSetupService } from './services/folsomia-setup-service/folsomia-setup.service';
import { HttpClientModule, HttpClient } from '@angular/common/http'; 
import { ToastrModule } from 'ngx-toastr';
import 'hammerjs';
import 'mousetrap';
import {GalleryModule} from '@ks89/angular-modal-gallery';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { ApexChartComponent } from './components/chart/apex-chart/apex-chart.component';
import {ApexChartService} from './components/chart/apex-chart/apex-chart.service';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

@NgModule({
  imports: [
    CommonModule,
    PerfectScrollbarModule,
    FormsModule,
    ReactiveFormsModule,
    AlertModule,
    CardModule,
    BreadcrumbModule,
    ModalModule,
    ToastrModule.forRoot(),
    GalleryModule.forRoot(),
    ClickOutsideModule,
    HttpClientModule
  ],
  exports: [
    CommonModule,
    PerfectScrollbarModule,
    FormsModule,
    ReactiveFormsModule,
    AlertModule,
    CardModule,
    BreadcrumbModule,
    ModalModule,
    GalleryModule,				  
    DataFilterPipe,
    ClickOutsideModule,
    SpinnerComponent,
    ApexChartComponent,
	ImageEditorModule,
    ToastrModule
  ],
  declarations: [
    DataFilterPipe,
    SpinnerComponent,
    ApexChartComponent,
	  SpinnerComponent
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    ApexChartService,
	HttpClientModule, FolsomiaSetupService
  ]
})
export class SharedModule { }
