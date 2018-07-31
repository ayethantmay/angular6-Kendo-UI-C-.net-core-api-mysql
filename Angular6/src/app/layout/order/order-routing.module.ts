import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderComponent } from './order.component';
import { CommonModule } from '../../../../node_modules/@angular/common';

const routes: Routes=[

  {
    path:'',
    component: OrderComponent
  }
];

@NgModule({
  imports: [
    CommonModule,RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
