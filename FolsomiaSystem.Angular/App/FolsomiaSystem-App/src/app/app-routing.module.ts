import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './theme/layout/admin/admin.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: 'dashboard/default',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadChildren: () => import('./folsomia/dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'manual',
        loadChildren: () => import('./folsomia/manual/manual.module').then(m => m.ManualModule)
      },
      {
        path: 'about',
        loadChildren: () => import('./folsomia/about/about.module').then(m => m.AboutModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
