using System;
using System.Collections;
using System.Web;
using Newtonsoft.Json;
using SunTech.WS.ClassFolder;

namespace SunTech.WS.HxServcieFolder
{
	
	public class Hx_24Pay : System.Web.IHttpHandler
	{
		
		private Cls_24Pay Cls24Pay = new Cls_24Pay();
		
		public void ProcessRequest(HttpContext context)
		{
			string Method_Type = System.Convert.ToString(context.Request.ServerVariables["REQUEST_METHOD"].ToString());
			string SERVER_PORT = System.Convert.ToString(context.Request.ServerVariables["SERVER_PORT"].ToString());
         
			//超商條碼交易服務
			Cls_vbMessage ClsvbMessage = default(Cls_vbMessage);
			Jso_GetData JsoGetData = default(Jso_GetData);
			Jso_GetObject JsoGetObject = default(Jso_GetObject);
            string TransType = Mod_Str.Convert_ToString(context.Request["TransType"]);
			string JsonStr = "";
			try
			{
				switch (TransType)
				{
					case "ImportAdd_24Pay":
						//超商代收大批上傳(合約編號,商店代碼,上傳IP位置,檔案內容(陣列),檔案路徑,檔名)
						string Contract_NBR_1 = Mod_Str.Convert_ToString(context.Request["Contract_NBR"]);
						string PayNo =Mod_Str.Convert_ToString(context.Request["PayNo"]);
						string IP =Mod_Str.Convert_ToString(context.Request["IP"]);
						string UpdateText_1 =Mod_Str.Convert_ToString(context.Request["UpdateText"]);
						string TransData_1 =Mod_Str.Convert_ToString(context.Request["TransData"]);
						Jso_SetData JsoSetData_1 = new Jso_SetData();
						JsoSetData_1 = JsonConvert.DeserializeObject<Jso_SetData>(TransData_1);
						ClsvbMessage = Cls24Pay.ImportAdd_24Pay(Contract_NBR_1, PayNo, IP, JsoSetData_1.GetStringArr, UpdateText_1);
						JsoGetObject = new Jso_GetObject();
						JsoGetObject.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsonStr = JsonConvert.SerializeObject(JsoGetObject);
						break;
						
					case "Return_ATM":
						//虛擬帳號電文傳入
						string PD_Layout =Mod_Str.Convert_ToString(context.Request["PD_Layout"]);
						string AgencyBank =Mod_Str.Convert_ToString(context.Request["AgencyBank"]);
						ClsvbMessage = Cls24Pay.Return_ATM(AgencyBank, PD_Layout);
						JsoGetObject = new Jso_GetObject();
						JsoGetObject.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsonStr = JsonConvert.SerializeObject(JsoGetObject);
						break;
						
					case "Import_ATM":
						//中信虛擬帳號 勾稽
						string UpdateText_2 =Mod_Str.Convert_ToString(context.Request["UpdateText"]);
						string ScUser_1 =Mod_Str.Convert_ToString(context.Request["ScUser"]);
						string TransData_2 =Mod_Str.Convert_ToString(context.Request["TransData"]);
						Jso_SetData JsoSetData_2 = new Jso_SetData();
						JsoSetData_2 = JsonConvert.DeserializeObject<Jso_SetData>(TransData_2);
						ClsvbMessage = Cls24Pay.Upd_ATM(JsoSetData_2.GetStringArr, UpdateText_2, ScUser_1);
						JsoGetObject = new Jso_GetObject();
						JsoGetObject.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsonStr = JsonConvert.SerializeObject(JsoGetObject);
						break;
						
					case "Upd_24Pay":
						// 超商代收匯入更新(檔案內容(陣列), 檔名, 上傳人員)
						string UpdateText =Mod_Str.Convert_ToString(context.Request["UpdateText"]);
						string ScUser =Mod_Str.Convert_ToString(context.Request["ScUser"]);
						string PayType =Mod_Str.Convert_ToString(context.Request["PayType"]);
						string TransData_3 =Mod_Str.Convert_ToString(context.Request["TransData"]);
						Jso_SetData JsoSetData = new Jso_SetData();
						JsoSetData = JsonConvert.DeserializeObject<Jso_SetData>(TransData_3);
						switch (PayType)
						{
							case "1": //超商條碼
								ClsvbMessage = Cls24Pay.Upd_TSBCON(JsoSetData.GetStringArr, UpdateText, ScUser);
								break;
							case "2": //郵局條碼
								ClsvbMessage = Cls24Pay.Upd_TSBPOST(JsoSetData.GetStringArr, UpdateText, ScUser);
								break;
							case "3": //虛擬帳號
								ClsvbMessage = Cls24Pay.Upd_TSAC(JsoSetData.GetStringArr, UpdateText, ScUser);
								break;
							default:
								ClsvbMessage = new Cls_vbMessage();
								ClsvbMessage.ErrorMessage = "PayType錯誤";
								break;
						}
						JsoGetObject = new Jso_GetObject();
						JsoGetObject.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsonStr = JsonConvert.SerializeObject(JsoGetObject);
						break;
						
					case "Add_24Pay":
						//新增超商代收交易(交易編號,商店代碼,商店自訂編號,交易金額,備註一,備註二,交易內容,繳款人姓名,繳款人Email,繳款人電話,商店列帳日期,用戶之編號,繳費期限,產品名稱,產品單價,產品數量,繳款人IP位置,字碼(big5 or utf8))
						string TransData_4 =Mod_Str.Convert_ToString(context.Request["TransData"]);
						Jso_GetPayment Jso = new Jso_GetPayment();
						Jso = JsonConvert.DeserializeObject<Jso_GetPayment>(TransData_4);
						ClsvbMessage = Cls24Pay.Add_24Pay(Jso);
						JsoGetData = new Jso_GetData();
						JsoGetData.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsoGetData.GetDateSet = ClsvbMessage.GetDateSet;
						JsoGetData.GetString = ClsvbMessage.GetString;
						JsonStr = JsonConvert.SerializeObject(JsoGetData);
						break;
						
					case "API_24Pay":
						//超商代收API接口(商店代碼,交易金額,商店自訂編號,交易內容,繳款人姓名,繳款人電話,繳款人Email,繳款人IP位置,用戶編號,列帳日期[yyyyMMdd],繳款期限[yyyyMMdd],產品名稱,產品單價,產品數量,備註一,備註二)
						string TransData =Mod_Str.Convert_ToString(context.Request["TransData"]);
						Jso_GetPayment Js = new Jso_GetPayment();
						Js = JsonConvert.DeserializeObject<Jso_GetPayment>(TransData);
						ClsvbMessage = Cls24Pay.API_24Pay(Js.PayNo, Js.Price, Js.OrderNo, Js.OrderInfo, Js.CustName, Js.Tel, Js.E_mail, Js.IP, Js.UserNo, Js.BillDate, Js.DueDate, Js.ProductName, Js.ProductPrice, Js.ProductQuantity, Js.Note1, Js.Note2);
						JsoGetData = new Jso_GetData();
						JsoGetData.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsoGetData.GetDateSet = ClsvbMessage.GetDateSet;
						JsoGetData.GetString = ClsvbMessage.GetString;
						JsonStr = JsonConvert.SerializeObject(JsoGetData);
						break;
						
					case "Query_Import24PayPDF":
						//搜尋超商代收大批上傳資料(合約編號,檔案類別[24Pay_txt、24Pay_pdf、24Pay_Import、空白],開始搜尋日期[yyyyMMdd],結束搜尋日期[yyyyMMdd])
						string Contract_NBR =Mod_Str.Convert_ToString(context.Request["Contract_NBR"]);
						string FileType =Mod_Str.Convert_ToString(context.Request["FileType"]);
						string StartDate =Mod_Str.Convert_ToString(context.Request["StartDate"]);
						string EndDate =Mod_Str.Convert_ToString(context.Request["EndDate"]);
						ClsvbMessage = Cls24Pay.Query_Import24PayPDF(Contract_NBR, FileType, StartDate, EndDate);
						JsoGetData = new Jso_GetData();
						JsoGetData.ErrorMessage = ClsvbMessage.ErrorMessage;
						JsoGetData.GetDateSet = ClsvbMessage.GetDateSet;
						JsoGetData.GetString = ClsvbMessage.GetString;
						JsonStr = JsonConvert.SerializeObject(JsoGetData);
						break;
						
					default:
						throw (new Exception("無此交易類別" + TransType));
				}
			}
			catch (Exception ex)
			{
				JsonStr = ex.Message;
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(JsonStr);
		}
		
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		
	}
}
