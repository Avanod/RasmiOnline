using System.Collections.Generic;

namespace RasmiOnline.Domain.Dto
{
    public class StatisticModel
    {
        public int UserCount { get; set; }
        public int OrderCount { get; set; }
        public int PayCount { get; set; }
        public int PayAmount { get; set; }
        public IDictionary<string, int> UserInDays { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> OrderInDays { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> PayCountInDays { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> PayAmountInDays { get; set; } = new Dictionary<string, int>();
    }
}