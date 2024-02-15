using Quartz;

namespace Spriggan.Module.Worker;

public static class QuartzKeys
{
    public static readonly JobKey Example = new("example", "default");
}
