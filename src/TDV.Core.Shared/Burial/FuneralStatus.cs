using System;
using System.Collections.Generic;
using System.Text;

namespace TDV.Burial
{
    public enum FuneralStatus
    {
        NeedFix, /*Düzeltme gerekenler*/
        New, /*Yeni açılmış kayıtları veya düzeltilmiş kayıtları*/
        Approved, /*Bilgileri onaylanmış kayıtları*/
        Appointed, /*İş emri verilen kayıtları*/
        Completed, /*Teslim edilmiş cenazeleri*/
        Canceled /*İptal edilen kayıtları Tanımlar. Tam amacı anlaşılmadı*/
    }
}
