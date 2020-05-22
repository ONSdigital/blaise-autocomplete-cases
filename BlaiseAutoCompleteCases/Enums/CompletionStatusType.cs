
using System.ComponentModel;

namespace BlaiseAutoCompleteCases.Enums
{
    public enum CompletionStatusType
    {
        [Description("something wrong happened")]
        ActionModel,

        [Description("Case auto completed ?...")]
        CaseAutoCompleted,

        [Description("Case not auto completed...")]
        CaseNotAutoCompleted,

    }
}
