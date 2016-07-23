namespace livemenu.Areas.BWP.Models
{
    public interface IInternalWidgetModelBase
    {
        string Kind { get; set; }
        string Code { get; set; }
        string State { get; set; }
        string ID { get; set; }
    }
}