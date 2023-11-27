using Common.Models;

namespace Common
{
    public abstract class LabBase
    {
        public abstract Task<LabResult> Execute(LabInput input);
        public abstract int Code { get; }
    }
}
