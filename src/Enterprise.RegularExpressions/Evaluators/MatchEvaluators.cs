using System.Text.RegularExpressions;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.RegularExpressions.Evaluators;

public static class MatchEvaluators
{
    public  static readonly MatchEvaluator ReplaceWithAsterisk = m => new string (Asterisk, m.Length);
}
