import { Component, OnInit} from '@angular/core';
import { DialogRef, DialogCloseResult, DialogService } from '../../../../node_modules/@progress/kendo-angular-dialog';
import { Validators,FormControl, FormBuilder, FormGroup } from '../../../../node_modules/@angular/forms';

import {Inventory} from '../../core/models/inventory.model';
import {InventoryService} from '../../core/services/inventory.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss']
})
export class InventoryComponent implements OnInit {

  inventory_data
  show: boolean = false;
  data : Inventory;

  public inventory_id;
  public inventory_category : FormControl;
  public inventory_name : FormControl;
  public inventory_description : FormControl;
  public inventory_price : FormControl;
  public inventory_brand : FormControl;


  public InventoryForm : FormGroup;
  public unit;
  public flag;
  public result;

  constructor(private inventory_service: InventoryService, 
    private dialogService: DialogService,
    public formBuilder: FormBuilder) { }

  ngOnInit() {

    this.inventory_service.getInventory()
    .subscribe( x => {this.inventory_data = x.data;
    });

    this.inventory_id =0;
    this.inventory_category = new FormControl('', Validators.compose([Validators.required]));
    this.inventory_name = new FormControl('', Validators.compose([Validators.required]));
    this.inventory_description = new FormControl('', Validators.compose([Validators.required]));
    this.inventory_price = new FormControl;
    this.inventory_brand = new FormControl('', Validators.compose([Validators.required]));
    this.buildForm();
  }


  buildForm() : void {

    this.InventoryForm = this.formBuilder.group({
    inventory_id : this.inventory_id,
    inventory_category : this.inventory_category,
    inventory_name : this.inventory_name,
    inventory_description : this.inventory_description,
    inventory_price : this.inventory_price,
    inventory_brand : this.inventory_brand
    });

  }


  // Insert
  save(){
    const test = this.InventoryForm.value; 
    this.inventory_service.saveInventory(test).subscribe(x =>{
     this.ngOnInit();
      this.show = false;
    });
    ;
  }


  // Update
  edit(dataItem){
    this.show = true;
    this.InventoryForm=new FormGroup({
      'inventory_id' : new FormControl(dataItem.inventory_id),
      'inventory_category' : new FormControl(dataItem.inventory_category),
      'inventory_name' : new FormControl(dataItem.inventory_name),
      'inventory_description' : new FormControl(dataItem.inventory_description),
      'inventory_price' : new FormControl(dataItem.inventory_price),
      'inventory_brand' : new FormControl(dataItem.inventory_brand)
    })
  }


  // Delete
  public deleteConfirm(dataItem) {
    
    const dialog: DialogRef = this.dialogService.open({
      title: 'Please confirm',
      content: ' Are you sure you want to delete ? ',
      actions: [
          { text: 'No' },
          { text: 'Yes', primary: true }
      ]      
  });

  dialog.result.subscribe((result) => {
      if (result instanceof DialogCloseResult) {
          console.log('close');
      } else {
          if (result.text == 'Yes') {
            this.delete(dataItem.inventory_id);
          }
      }
  });
}

delete(id) {
  this.inventory_service.deleteInventory(id)
  .subscribe( x => {
    this.ngOnInit()
  });
}


// Show Form
show_form(){
  this.show = true;
}

// Close Form
close_form(){
  this.show = false;
}
}

