import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateGuard } from '../../theme/shared/models/sample-changes-guard';  

const routes: Routes = [
  {
    path: '',
    
    children: [
      {
        path: 'default',
        loadChildren: () => import('./dash-default/dash-default.module').then(module => module.DashDefaultModule)
      }
    ],
    canDeactivate: [CanDeactivateGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
