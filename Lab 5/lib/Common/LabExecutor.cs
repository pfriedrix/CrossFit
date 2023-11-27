using Common.Models;

namespace Common
{
    public static class LabExecutor
    {
        public static List<LabBase> Labs { get; private set; } = new();

        public static async Task<LabResult> Execute(int labCode, LabInput input)
            => await Labs.FirstOrDefault(lab => lab.Code == labCode)?.Execute(input)! ??
               throw new Exception($"Lab with {labCode} code does not exist.");
    }
}
