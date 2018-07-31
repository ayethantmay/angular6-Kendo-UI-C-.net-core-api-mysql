import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InventoryComponent } from './inventory.component';
import { CommonModule } from '../../../../node_modules/@angular/common';

const routes: Routes=[

  {
    path:'',
    component: InventoryComponent
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule { }
