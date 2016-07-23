using livemenu.Kernel.Rights;

namespace livemenu.Models.Rights
{
    public class RightsButtonsPanelModel
    {
        public bool IsView { get; set; }

        public long RightsHolderID { get; set; }
        public long RightSubjectID { get; set; }


        public RightValueType AllowedType { get; set; }
        public RightValueType DeniedType { get; set; }
        public RightValueType IsSyncType { get; set; }

        public bool IsEnabled { get; set; }
        public bool Allowed { get; set; }
        public bool Denied { get; set; }

        public bool IsSync { get; set; }
    }
}