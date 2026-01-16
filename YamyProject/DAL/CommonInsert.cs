using MySql.Data.MySqlClient;
using System;
using System.Data;

using System.Linq;



namespace YamyProject
{
    public static class CommonInsert
    {
        public static void InsertItemTransaction(DateTime date, string type, string reference, string itemId,
                                  string costPrice, string qtyIn, string salesPrice, string qtyOut, string qtyInc,
                                  string description,string warehouseId)
        {
            DBClass.ExecuteNonQuery(@"INSERT INTO tbl_item_transaction 
                    (date, type, reference, item_id, cost_price, qty_in,sales_price, qty_out,qty_inc, description, warehouse_id ) 
                    VALUES (@date, @type, @reference, @itemId, @costPrice, @qtyIn,@sales_price, @qtyOut,@qtyInc, @description, @warehouseId);",
                                    DBClass.CreateParameter("@date", date),
                                    DBClass.CreateParameter("@type", type),
                                    DBClass.CreateParameter("@reference", reference),
                                    DBClass.CreateParameter("@itemId", itemId),
                                    DBClass.CreateParameter("@costPrice", costPrice),
                                    DBClass.CreateParameter("@sales_price", salesPrice),
                                    DBClass.CreateParameter("@qtyIn", qtyIn),
                                    DBClass.CreateParameter("@qtyOut", qtyOut),
                                    DBClass.CreateParameter("@qtyInc", qtyInc),
                                    DBClass.CreateParameter("@description", description),
                                    DBClass.CreateParameter("warehouseId", warehouseId));

            UpdateOnHandItem(itemId);
            AddItemCardDetails(date, type, reference, itemId, costPrice, qtyIn, salesPrice, qtyOut, qtyInc, description, warehouseId);
        }
        private static void AddItemCardDetails(DateTime date, string type, string reference, string itemId,
                                  string costPrice, string qtyIn, string salesPrice, string qtyOut, string qtyInc,
                                  string description, string warehouseId)
        {
            string invoiceNo = "INV-" + reference, transNo = reference, transType = type;
            decimal qtyBalance = 0, debit = 0, credit = 0, price = decimal.Parse(costPrice), balance = 0, fifoQty = 0, fifoCost = 0;
            decimal _qtyIn = 0, _qtyOut = 0;
            if (!string.IsNullOrEmpty(qtyIn) && decimal.Parse(qtyIn) > 0)
            {
                debit = decimal.Parse(qtyIn) * decimal.Parse(costPrice);
                _qtyIn = decimal.Parse(qtyIn);
            }
            if (!string.IsNullOrEmpty(qtyOut) && decimal.Parse(qtyOut) >0)
            {
                credit = decimal.Parse(qtyOut) * decimal.Parse(costPrice);
                _qtyOut = decimal.Parse(qtyOut);
            }
            string query = "SELECT * FROM tbl_items WHERE id = @itemId";
            
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("itemId", itemId)))
            {
                object netResult = DBClass.ExecuteScalar(
                            "SELECT IFNULL(SUM(qty_in-qty_out),0) as QtyBalance FROM tbl_item_card_details WHERE itemId = @id",
                            DBClass.CreateParameter("id", itemId)
                        );
                decimal _QtyBalance = netResult != DBNull.Value ? Convert.ToDecimal(netResult) : 0;
                qtyBalance = _QtyBalance+(_qtyIn-_qtyOut);
                netResult = DBClass.ExecuteScalar(
                            "SELECT IFNULL(SUM(debit-credit),0) as Balance FROM tbl_item_card_details WHERE itemId = @id",
                            DBClass.CreateParameter("id", itemId)
                        );
                decimal _Balance = netResult != DBNull.Value ? Convert.ToDecimal(netResult) : 0;
                balance = _Balance+ (debit - credit);

                DBClass.ExecuteNonQuery(@"
                            INSERT INTO tbl_item_card_details (
                                itemId, date, wharehouse_id, inv_no, trans_no, trans_type, description,
                                price, qty_in, qty_out, qty_balance, debit, credit, balance, fifo_qty, fifo_cost
                            ) VALUES (
                                @itemId, @date, @wharehouse_id, @inv_no, @trans_no, @trans_type, @description,
                                @price, @qty_in, @qty_out, @qty_balance, @debit, @credit, @balance, @fifo_qty, @fifo_cost
                            );",
                                DBClass.CreateParameter("itemId", itemId),
                                DBClass.CreateParameter("date", date),
                                DBClass.CreateParameter("wharehouse_id", warehouseId),
                                DBClass.CreateParameter("inv_no", invoiceNo),
                                DBClass.CreateParameter("trans_no", transNo),
                                DBClass.CreateParameter("trans_type", transType),
                                DBClass.CreateParameter("description", description),
                                DBClass.CreateParameter("price", price),
                                DBClass.CreateParameter("qty_in", qtyIn),
                                DBClass.CreateParameter("qty_out", qtyOut),
                                DBClass.CreateParameter("qty_balance", qtyBalance),
                                DBClass.CreateParameter("debit", debit),
                                DBClass.CreateParameter("credit", credit),
                                DBClass.CreateParameter("balance", balance),
                                DBClass.CreateParameter("fifo_qty", fifoQty),
                                DBClass.CreateParameter("fifo_cost", fifoCost)
                            );
            }
        }
        public static void UpdateOnHandItem(string itemId)
        {
            DBClass.ExecuteNonQuery(@"UPDATE tbl_items SET on_hand = (SELECT SUM(qty_in - qty_out) FROM tbl_item_transaction WHERE item_id = @itemId) where id = @itemId",
                                    DBClass.CreateParameter("@itemId", itemId));
        }
        public static void DeleteItemTransaction(string type, string reference)
        {
            if (type == "Sales")
            {
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_transaction WHERE `type` = 'Sales Invoice' AND `REFERENCE` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Sales Invoice' AND `trans_no` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery(@"UPDATE tbl_items i
                                            INNER JOIN tbl_sales_details sd ON i.id = sd.item_id
                                            SET i.on_hand = i.on_hand + sd.qty
                                            WHERE sd.sales_id = @id;", DBClass.CreateParameter("id", reference));
            }
            else if (type == "Sales return")
            {
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_transaction WHERE `type` = 'Sales Return Invoice' AND `REFERENCE` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Sales Return Invoice' AND `trans_no` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery(@"UPDATE tbl_items i
                                            INNER JOIN tbl_sales_details sd ON i.id = sd.item_id
                                            SET i.on_hand = i.on_hand - sd.qty
                                            WHERE sd.sales_id = @id;", DBClass.CreateParameter("id", reference));
            }
            else if (type == "Purchase")
            {
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_transaction WHERE `type` = 'Purchase Invoice' AND `REFERENCE` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Purchase Invoice' AND `trans_no` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery(@"UPDATE tbl_items i
                                            INNER JOIN tbl_sales_details sd ON i.id = sd.item_id
                                            SET i.on_hand = i.on_hand - sd.qty
                                            WHERE sd.sales_id = @id;", DBClass.CreateParameter("id", reference));
            }
            else if (type == "Purchase return")
            {
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_transaction WHERE `type` = 'Purchase Return Invoice' AND `REFERENCE` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Purchase Return Invoice' AND `trans_no` = @id", DBClass.CreateParameter("id", reference));
                DBClass.ExecuteNonQuery(@"UPDATE tbl_items i
                                            INNER JOIN tbl_sales_details sd ON i.id = sd.item_id
                                            SET i.on_hand = i.on_hand + sd.qty
                                            WHERE sd.sales_id = @id;", DBClass.CreateParameter("id", reference));
            }
        }
        
        public static void InsertTransactionEntry(DateTime date, string accountId, string debit, string credit,
                               string transactionId,string humId, string type, string description, int createdBy, DateTime createdDate)
        {
            DBClass.ExecuteNonQuery(@"INSERT INTO tbl_transaction 
                    (date, account_id, debit, credit, transaction_id,hum_id,t_type,type, description, created_by, created_date, state) 
                    VALUES (@date, @accountId, @debit, @credit, @transactionId,@hum_id,@tType,@type, @description, @createdBy, @createdDate, 0);",
            DBClass.CreateParameter("@date", date),
            DBClass.CreateParameter("@accountId", accountId),
            DBClass.CreateParameter("@debit", debit),
            DBClass.CreateParameter("@credit", credit),
            DBClass.CreateParameter("@transactionId", transactionId),
            DBClass.CreateParameter("@type", type.Trim()),
            DBClass.CreateParameter("@tType", ""),
            DBClass.CreateParameter("@hum_id", humId ),
            DBClass.CreateParameter("@description", description),
            DBClass.CreateParameter("@createdBy", createdBy),
            DBClass.CreateParameter("@createdDate", createdDate));
        }
        public static void addTransactionEntry(DateTime date, string accountId, string debit, string credit,
                               string transactionId, string humId, string type, string voucher_name, string description, int createdBy, DateTime createdDate,string VoucherNo)
        {
            DBClass.ExecuteNonQuery(@"INSERT INTO tbl_transaction 
                    (date, account_id, debit, credit, transaction_id,hum_id,t_type,type, description, created_by, created_date, state,voucher_no) 
                    VALUES (@date, @accountId, @debit, @credit, @transactionId,@hum_id,@tType,@type, @description, @createdBy, @createdDate, 0,@voucher_no);",
            DBClass.CreateParameter("@date", date),
            DBClass.CreateParameter("@accountId", accountId),
            DBClass.CreateParameter("@debit", debit),
            DBClass.CreateParameter("@credit", credit),
            DBClass.CreateParameter("@transactionId", transactionId),
            DBClass.CreateParameter("@type", type),
            DBClass.CreateParameter("@tType", voucher_name),
            DBClass.CreateParameter("@hum_id", humId),
            DBClass.CreateParameter("@description", description),
            DBClass.CreateParameter("@createdBy", createdBy),
            DBClass.CreateParameter("@createdDate", createdDate),
            DBClass.CreateParameter("voucher_no", VoucherNo));
        }
        public static void UpdateTransactionEntry(int journalId, DateTime date, string accountId, string debit, string credit,
                                      string transactionId, string humId, string type, string description, int modifiedBy, DateTime modifiedDate)
        {
            DBClass.ExecuteNonQuery(@"UPDATE tbl_transaction 
                            SET date = @date, 
                                account_id = @accountId, 
                                debit = @debit, 
                                credit = @credit, 
                                transaction_id = @transactionId, 
                                hum_id =@hum_id,
                                type = @type,
                                description = @description, 
                                modified_by = @modifiedBy, 
                                modified_date = @modifiedDate
                            WHERE id = @journalId;",
                DBClass.CreateParameter("@date", date),
                DBClass.CreateParameter("@accountId", accountId),
                DBClass.CreateParameter("@debit", debit),
                DBClass.CreateParameter("@credit", credit),
                DBClass.CreateParameter("@transactionId", transactionId),
                DBClass.CreateParameter("@hum_id", humId),
                DBClass.CreateParameter("@type", type),
                DBClass.CreateParameter("@description", description),
                DBClass.CreateParameter("@modifiedBy", modifiedBy),
                DBClass.CreateParameter("@modifiedDate", modifiedDate),
                DBClass.CreateParameter("@journalId", journalId)
            );
        }
        public static void DeleteTransactionEntry(int id,string type)
        {
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_transaction Where t_type= @tType and transaction_id=@id;",
            DBClass.CreateParameter("@id", id),
            DBClass.CreateParameter("@tType", type));
        }
        public static void InsertCostCenterTransaction(DateTime date, string debit, string credit, string refId, string type, string description, string cost_center_id)
        {
            DBClass.ExecuteNonQuery(@"INSERT INTO tbl_cost_center_transaction (type,date,ref_id,debit,credit,description,cost_center_id) 
                                VALUES (@type,@date,@ref,@debit,@credit,@description,@cost_center_id);",
                DBClass.CreateParameter("@date", date),
                DBClass.CreateParameter("@type", type),
                DBClass.CreateParameter("@debit", debit),
                DBClass.CreateParameter("@credit", credit),
                DBClass.CreateParameter("@ref", refId),
                DBClass.CreateParameter("@description", description),
                DBClass.CreateParameter("@cost_center_id", cost_center_id));
        }
        public static void DeleteCostCenterTransactionEntry(string refId, string type)
        {
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_cost_center_transaction Where type= @type and ref_id=@id;",
            DBClass.CreateParameter("@id", refId),
            DBClass.CreateParameter("@type", type));
        }
        public static string GetFormName(string transactionType)
        {
            string type = transactionType.ToLower();

            if (type.Equals("sales invoice") || type.Equals("sales invoice cash"))
            {
                return "Sales";
            }
            else if (type.Equals("sales return invoice") || type.Equals("sales return invoice cash"))
            {
                return "Sales Return";
            }
            else if (type.Equals("purchase invoice") || type.Equals("purchase invoice cash"))
            {
                return "Purchase";
            }
            else if (type.Equals("purchase return invoice") || type.Equals("purchase return invoice cash"))
            {
                return "Purchase Return";
            }
            else if (type.Equals("vendor payment"))
            {
                return "Payment";
            }
            else if (type.Equals("customer receipt"))
            {
                return "Receipt";
            }
            else if (type.Equals("employee salary"))
            {
                return "Employee Salary";
            }
            else if (type.Equals("petty cash request"))
            {
                return "Petty Cash Request";
            }
            else if (type.Equals("petty cash submission"))
            {
                return "Petty Cash Submission";
            }
            else if (type.Equals("inventory opening stock"))
            {
                return "Inventory";
            }
            else if (type.Equals("prepaid expense"))
            {
                return "Prepaid Expense";
            }
            else if (type.Equals("fixed assets"))
            {
                return "Fixed Assets";
            }
            else if (type.Equals("pdc payable"))
            {
                return "PDC Payable";
            }
            else if (type.Equals("pdc receivable"))
            {
                return "PDC Receivable";
            }
            else if (type.Equals("loan request"))
            {
                return "Loan Request";
            }
            else if (type.Equals("customer advance payment"))
            {
                return "Customer Advance Payment";
            }
            else if (type.Equals("rms bill"))
            {
                return "RMS Bill";
            }
            else
            {
                return transactionType; // If no match
            }

            /*
             * Customer Opening Balance",
                    "Vendor Opening Balance",
                    "Sales Invoice",
                    "Purchase Invoice",
                    "Customer Receipt",
                    "Vendor Payment",
                    "Employee Salary",
                    "Petty Cash Request",
                    "Petty Cash Submission",
                    "Purchase Return Invoice",
                    "Employee Petty Cash Payment",
                    "Inventory Opening Stock",
                    "Prepaid Expense",
                    "Fixed Assets",
                    "PDC Payable",
                    "PDC Receivable",
                    "Employee Salary Payment",
                    "Loan Request",
                    "Vendor Advance Payment",
                    "Customer Advance Payment",
                    "RMS Bill
             */
        }
    }
}
