namespace PrettyPrintNet.InternalTypes
{
    internal class TimeFormat
    {
        public readonly string GroupSeparator;
        public readonly string LastGroupSeparator;
        public readonly string UnitValueSeparator;

        public TimeFormat(string groupSeparator, string lastGroupSeparator, string unitValueSeparator)
        {
            GroupSeparator = groupSeparator;
            LastGroupSeparator = lastGroupSeparator;
            UnitValueSeparator = unitValueSeparator;
        }
    }
}