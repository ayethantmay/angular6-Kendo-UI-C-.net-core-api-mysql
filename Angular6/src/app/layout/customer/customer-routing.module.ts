import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerComponent } from './customer.component';
import { CommonModule } from '../../../../node_modules/@angular/common';

const routes: Routes=[

  {
    path:'',
    component: CustomerComponent
  }
];

@NgModule({
  imports: [CommonModule,RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
