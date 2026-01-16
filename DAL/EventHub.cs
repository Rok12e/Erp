using System;

namespace YamyProject
{
    public static class EventHub
    {
        public static event EventHandler lvl1Account;
        public static event EventHandler lvl2Account;
        public static event EventHandler lvl3Account;
        public static event EventHandler lvl4Account;
        public static event EventHandler Journal;
        public static event EventHandler User;
        public static event EventHandler Roles;
        public static event EventHandler Customer;
        public static event EventHandler CustomerCategory;
        public static event EventHandler ReceiptVoucher;
        public static event EventHandler PaymentVoucher;
        public static event EventHandler PettyCashVoucher;
        public static event EventHandler SalesInv;
        public static event EventHandler SalesOrder;
        public static event EventHandler SalesQuotation;
        public static event EventHandler SalesProforma;
        public static event EventHandler SalesReturn;
        public static event EventHandler Vendor;
        public static event EventHandler SubContract;
        public static event EventHandler VendorCategory;
        public static event EventHandler SubContractCategory;
        public static event EventHandler PurchaseInv;
        public static event EventHandler PurchaseOrder;
        public static event EventHandler PurchaseReturn;
        public static event EventHandler DamageInv;
        public static event EventHandler Item;
        public static event EventHandler ItemCategory;
        public static event EventHandler FixedAssetsCategory;
        public static event EventHandler ItemUnit;
        public static event EventHandler wareHouse;
        public static event EventHandler Employee;
        public static event EventHandler EmployeeDept;
        public static event EventHandler EmployeePosition;
        public static event EventHandler Bank;
        public static event EventHandler PDCPayable;
        public static event EventHandler PDCReceivable;
        public static event EventHandler Check;
        public static event EventHandler BankCard;
        public static event EventHandler PrepaidExpense;
        public static event EventHandler FixedAsset;
        public static event EventHandler CreditNote;
        public static event EventHandler DebitNote;
        public static event EventHandler MainCostCenter;
        public static event EventHandler TaxSchedule;
        public static event EventHandler Project;
        public static event EventHandler ProjectPlanning;
        public static event EventHandler ProjectTendering;
        private static void RaiseEvent(EventHandler handler)
        {
            handler?.Invoke(null, EventArgs.Empty);
        }
        public static void Refreshlvl1Account() => RaiseEvent(lvl1Account);
        public static void Refreshlvl2Account() => RaiseEvent(lvl2Account);
        public static void Refreshlvl3Account() => RaiseEvent(lvl3Account);
        public static void Refreshlvl4Account() => RaiseEvent(lvl4Account);

        public static void RefreshJournal() => RaiseEvent(Journal);
        public static void RefreshUser() => RaiseEvent(User);
        public static void RefreshRoles() => RaiseEvent(Roles);
        public static void RefreshCustomer() => RaiseEvent(Customer);
        public static void RefreshCustomerCategory() => RaiseEvent(CustomerCategory);
        public static void RefreshSales() => RaiseEvent(SalesInv);
        public static void RefreshSalesOrder() => RaiseEvent(SalesOrder);
        public static void RefreshQuotation() => RaiseEvent(SalesQuotation);
        public static void RefreshSalesProforma() => RaiseEvent(SalesProforma); 
        public static void RefreshSalesReturn() => RaiseEvent(SalesReturn);
        public static void RefreshVendor() => RaiseEvent(Vendor);
        public static void RefreshSubContract() => RaiseEvent(SubContract);
        public static void RefreshVendorCategory() => RaiseEvent(VendorCategory);
        public static void RefreshSubContractCategory() => RaiseEvent(SubContractCategory);
        public static void RefreshPurchase() => RaiseEvent(PurchaseInv);
        public static void RefreshPurchaseOrder() => RaiseEvent(PurchaseOrder);
        public static void RefreshPurchaseReturn() => RaiseEvent(PurchaseReturn);
        public static void RefreshDamage() => RaiseEvent(DamageInv);
        public static void RefreshItem() => RaiseEvent(Item);
        public static void RefreshWarehouse() => RaiseEvent(wareHouse);
        public static void RefreshItemUnit() => RaiseEvent(ItemUnit);
        public static void RefreshItemCategory() => RaiseEvent(ItemCategory);
        public static void RefreshFixedAsset() => RaiseEvent(FixedAsset);
        public static void RefreshFixedAssetsCategory() => RaiseEvent(FixedAssetsCategory);
        public static void RefreshReceiptVoucher() => RaiseEvent(ReceiptVoucher);
        public static void RefreshPaymentVoucher() => RaiseEvent(PaymentVoucher);
        public static void RefreshPettyCashVoucher() => RaiseEvent(PettyCashVoucher);
        public static void RefreshEmployee() => RaiseEvent(Employee);
        public static void RefreshEmployeeDept() => RaiseEvent(EmployeeDept);
        public static void RefreshEmployeePosition() => RaiseEvent(EmployeePosition);
        public static void RefreshBank() => RaiseEvent(Bank);
        public static void RefreshCheck() => RaiseEvent(Check);
        public static void RefreshPDCPayable() => RaiseEvent(PDCPayable);
        public static void RefreshPDCReceivable() => RaiseEvent(PDCReceivable);
        public static void RefreshBankCard() => RaiseEvent(BankCard);
        public static void RefreshPrepaidExpense() => RaiseEvent(PrepaidExpense);
        public static void RefreshCreditNote() => RaiseEvent(CreditNote);
        public static void RefreshDebitNote() => RaiseEvent(DebitNote);
        public static void RefreshMainCostCenter() => RaiseEvent(MainCostCenter);
        public static void RefreshTaxSchedule() => RaiseEvent(TaxSchedule);
        public static void RefreshProject() => RaiseEvent(Project);
        public static void RefreshProjectPlanning() => RaiseEvent(ProjectPlanning);
        public static void RefreshProjectTendering() => RaiseEvent(ProjectTendering);
    }
}
