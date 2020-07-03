
using System.Linq;


namespace MimeMagicSharp
{
    
    
    public class MimeTypeGuess
    {
        [Newtonsoft.Json.JsonProperty("Name")] 
        public string Name;
        
        [Newtonsoft.Json.JsonProperty("RuleSet")] 
        internal System.Collections.Generic.List<RuleSet> RuleSet;
        
        [Newtonsoft.Json.JsonProperty("Description")] 
        public string Description;
        
        [Newtonsoft.Json.JsonProperty("Extensions")] 
        internal System.Collections.Generic.List<string> Extensions;
        
        
        public MimeTypeGuess(string name, string description) 
        {
            Name = name;
            Description = description;
            
            RuleSet = new System.Collections.Generic.List<RuleSet>();
            Extensions = new System.Collections.Generic.List<string>();
        } // End Constructor 
        
        
        public MimeTypeGuess(string name)
            : this(name, "")
        { } // End Constructor 
        
        
        public MimeTypeGuess()
            : this("")
        { } // End Constructor 
        
        
        internal void AddNewRuleSet(Rule rule)
        {
            RuleSet.Add(new RuleSet(rule));
        }
        
        
        internal void AppendLastRuleSet(Rule rule)
        {
            RuleSet.Last().Rules.Add(rule);
        }
        
        
        internal bool CheckType(byte[] inputArray)
        {
            //  If one of the rule sets succeed, Mime is guessed
            return (RuleSet.Count > 0) &&
                   RuleSet.Select(x => x.CheckType(inputArray)).
                       Aggregate((a, b) => a || b);
        }
        
        
    }
    
    
}
