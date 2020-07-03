
using System.Linq;


namespace MimeMagicSharp
{
    
    
    //  Rule set representation
    class RuleSet
    {
        
        [Newtonsoft.Json.JsonProperty("Rule")]  
        public System.Collections.Generic.List<Rule> Rules;
        
        
        public RuleSet(Rule rule)
        {
            Rules = new System.Collections.Generic.List<Rule>();
            if(rule != null)
                this.Rules.Add(rule);
        } // End Constructor 
        
        
        public RuleSet()
            :this(null)
        { } // End Constructor 
        
        
        //  Check file header with given rule set
        public bool CheckType(byte[] inputArray)
        {
            int levelPointer = 0;
            System.Collections.Generic.List<Rule> levelRules;

            //  Rule set can contain several rules with sertain hierarchy
            //  Level 0. indent 0
            //  Level 1. indent 1, indent 2, indent 3
            //  Level 2. indent 4
            //  Level 3. indent 5
            //  Result = 0 && (1 || 2 || 3) && 4 && 5 ...
            do
            {
                //  Get list of rules for the specified level
                levelRules = Rules.Where(x => x.Level == levelPointer).ToList();

                //  If level rules are not empty
                if (levelRules.Count > 0)
                {
                    bool indentRuleResult = levelRules.Select(x => x.CheckRule(inputArray)).Aggregate((a, b) => a || b);

                    //  If false => no need to check any further
                    if (!indentRuleResult) return false;
                }

                levelPointer++;
            }
            while (levelRules.Count > 0);

            return true;
        }
    }
}
