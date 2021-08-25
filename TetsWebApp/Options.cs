using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TetsWebApp
{
    public class Options
    {
        public bool EnableMetrics { get; set; } = true;
        public bool UseDefaultMetrics { get; set; } = false;
        public bool UseDebuggingMetrics { get; set; } = false;
        public TimeSpan RecycleEvery { get; set; } = TimeSpan.FromDays(1);
        public int? MinThreadPoolSize { get; set; } = null;
    }
}
