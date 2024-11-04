namespace HLS.Topup.StockManagement
{
    public enum StocksAirtimeStatus : byte 
    {
        Init = 0,
        Active = 1,
        Lock = 2
    }
    
    public enum BatchAirtimeStatus : byte 
    {
        Init = 0,
        Approval = 1,
        Reject = 2,
        Cancel = 3,
        Undefined = 99
    }
}