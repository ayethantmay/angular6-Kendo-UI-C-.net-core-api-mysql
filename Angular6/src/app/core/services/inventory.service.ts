import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  constructor(private apiservice : ApiService) { }

  getInventory() {
    return this.apiservice.fetch_get('/inventory/GetInventory');
  }

  deleteInventory(inventory_id) {
    return this.apiservice.delete('/inventory/DeleteInventory/' + inventory_id);
  }

  saveInventory(data){   
    return this.apiservice.saveLoanOption_Url_parameter('/inventory/AddInventory',data);
  }


}
