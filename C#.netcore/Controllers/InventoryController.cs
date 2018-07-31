using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;


namespace dotnet2_1WebAPI
{
    [Route("api/[controller]")]
    
    public class InventoryController : BaseController
    {
        private AppDb _objdb;

        public InventoryController(AppDb DB)
        {
            _objdb = DB; 
        } 

        [HttpGet("GetInventory", Name = "GetInventory")]
        public dynamic GetInventory(int inventory_id)
        {
            dynamic objresponse = new { data = 0 , msg = "Invalid Token Data" };
            
            var mainQuery = (from main in _objdb.Inventory
                             select main);
       
            if (inventory_id != 0)
            {
                mainQuery = (from main in mainQuery
                            where main.inventory_id == inventory_id
                            select main);
            }
            
            var list = mainQuery.ToList();
            
            objresponse = new { data = list, msg = (list != null && list.Count > 0) ? "Success" : "No Record Found" };   
        
            return objresponse;
        }

        [HttpDelete("DeleteInventory/{inventory_id}", Name = "DeleteInventory")]
        public dynamic DeleteInventory(int inventory_id)
        {
            bool retBool = false;

            //validateDelete
            int result = _objdb.Inventory.Where(x => x.inventory_id == inventory_id).Count();
            if (result < 0)
            {
                retBool = false;
            }

            else
            {
                var objInventory = _objdb.Inventory.Find(inventory_id);
                if (objInventory != null)
                {
                    //string[] idname = new string[] { "AdminID" };
                    //string[] id = new string[] { AdminID.ToString() };
                    //string logmsg = _objdb.GetLogMessage("Info", "Delete", "tbl_admin", "", idname, id);

                    _objdb.Remove(objInventory);
                    _objdb.SaveChanges();
                    retBool = true;
                    //_objdb.AddEventLog("Info", "Delete Admin", logmsg, _tokenData.UserID, _tokenData.LoginType, _ipaddress);
                }
            }
            dynamic objresponse = new { data = retBool };
            return objresponse;
        }

        [HttpPost("AddInventory", Name = "AddInventory")]

        public dynamic AddInventory()//[FromBody] dynamic criteria)
        {
            var objstr = HttpContext.Request.Form["formDataObj"];
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(objstr);
            int inventory_id = obj.inventory_id != null ? obj.inventory_id : 0;
            var objoption = _objdb.Inventory.Find(inventory_id);
            if (objoption != null)
            {
                objoption.inventory_category = obj.inventory_category; 
                objoption.inventory_name = obj.inventory_name;
                objoption.inventory_description=obj.inventory_description;
                objoption.inventory_price=obj.inventory_price;
                objoption.inventory_brand=obj.inventory_brand;
            
                _objdb.Update(objoption);
                _objdb.SaveChanges();
            }
            else
            {
                var newobj = new Inventory();
                newobj.inventory_category = obj.inventory_category;
                newobj.inventory_name = obj.inventory_name;
                newobj.inventory_description=obj.inventory_description;
                newobj.inventory_price=obj.inventory_price;
                newobj.inventory_brand=obj.inventory_brand;

                 _objdb.Add(newobj);
                    _objdb.SaveChanges();
                    inventory_id = newobj.inventory_id;          
                            
            }          

            dynamic objresponse = new { data = inventory_id};
            return objresponse;
        }



    }

}
