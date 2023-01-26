using ShippingPortGOF.Composite;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.Models
{
    public class Mooring : IComponent, IMooring
    {
        public int ID { get; set; }

        public int? IdPier { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }

        public int HourRate { get; set; }

        public int MaxLength { get; set; }

        public int MaxWidth { get; set; }

        public int MaxDepth { get; set; }

        public int Volume
        {
            get
            {
                return MaxLength * MaxWidth * MaxDepth;
            }
        }

        public Mooring(int id, string label, string type,
            int hourRate, int maxLength, int maxWidth, int maxDepth)
        {
            ID = id;
            IdPier = null;
            Label = label;
            Type = type;
            HourRate = hourRate;
            MaxLength = maxLength;
            MaxWidth = maxWidth;
            MaxDepth = maxDepth;
        }

        public string Accept(IMooringVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public int GetId()
        {
            return ID;
        }

        public string GetName()
        {
            return Label;
        }

        public bool IsComposite()
        {
            return false;
        }
    }
}
