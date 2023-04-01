using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPMMaker.Models
{
    public class CostMembers
    {
        public int No { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Department { get; set; }
        public int Costcenter { get; set; }
        public int Stock { get; set; }
        public int SafetyStock { get; set; }
        public int YearPurchaseQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        public string Note { get; set; }
        public int VendorCode { get; set; }
        public int PurchaseMaterialCode { get; set; }
        public string Maker { get; set; }
        public string MaterialName { get; set; }
        public string Standard { get; set; }
        public int Quantity { get; set; }
        public int MonetaryFund { get; set; }
        public string UnitCost { get; set; }
        public string TotalCost { get; set; }
        public string Quotation { get; set; }
        public string RepairReport { get; set; }
        public string ComparativeVendor { get; set; }
        public string ComparativeUnitCost { get; set; }
        public string ComparativeQuotation { get; set; }
        public string Etc { get; set; }

    }
    public enum CostCategoryEnum
    {
        No,
        접수일,
        요청일,
        코스트센터,
        현재고,
        안전재고,
        연간예상구매량,
        기결재번호,
        구매사유,
        구분,
        업체코드,
        업체명,
        연락처,
        자재코드,
        Maker,
        품명,
        규격,
        요청수량,
        초기단가,
        TOTAL,
        공정명,
        적용설비,
        담당자,
        견적서,
        수리보고서,
        비교업체,
        비교단가,
        비교견적서,
        비고
    }
}
