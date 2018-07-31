import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryRoutingModule } from './inventory-routing.module';
import { InventoryComponent } from './inventory.component';
import { PageHeaderModule } from '../../shared';

import { GridModule, PDFModule } from '@progress/kendo-angular-grid';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  imports: [
    CommonModule,PageHeaderModule,
    InventoryRoutingModule,
    GridModule,
    DialogModule,DateInputsModule,FormsModule,ReactiveFormsModule,PDFModule
  ],
  declarations: [InventoryComponent]
})
export class InventoryModule { }
