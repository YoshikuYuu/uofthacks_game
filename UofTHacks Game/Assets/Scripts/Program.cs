using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Program : MonoBehaviour
{

    private class PlayerCodeBlock {
        // A block of code that can be executed, with head being the first line
        //and all other lines being nested as code blocks
        private string head;
        private List<PlayerCodeBlock> nested = new List<PlayerCodeBlock>();

        public PlayerCodeBlock(string head, string[] nested) {
            this.head = head;
            foreach (string line in nested) {
                this.nested.Add(new PlayerCodeBlock(line));
            }
        }
        public PlayerCodeBlock(string head) {
            this.head = head;
        }
        public string GetHead() {
            return head;
        }
        public List<PlayerCodeBlock> GetNested() {
            return nested;
        }
    }

    public interface Command {
        void Execute(List<PlayerCodeBlock> nested);
    }

    private class ForLoop : Command {
        private int start; 
        private int end;
        private int step;
        private string loopVar;
        public ForLoop(string function, string loopVar) {
            // Note that when executing loop, the list shall not be mutated until after turn end
            this.loopVar = loopVar;
            MatchCollection matches = Regex.Matches(input, @"\d+");
            if (matches.Count == 1) {
                this.start = int.Parse(matches[0].Value);
            } else if (matches.Count == 2) {
                this.start = int.Parse(matches[0].Value);
                this.end = int.Parse(matches[1].Value);
            } else if (matches.Count == 3) {
                this.start = int.Parse(matches[0].Value);
                this.end = int.Parse(matches[1].Value);
                this.step = int.Parse(matches[2].Value); 
            } else {
                throw Exception("Invalid for loop");
            }   
        }

        public ForLoop(int end) {
            this.start = 0;
            this.end = end;
            this.step = 1;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            for (int i = start; i < end; i += step) {
                foreach (PlayerCodeBlock block in nested) {
                    
                    ExecuteBlock(block);
                }
            }
        }
    }

    private class IfStatement : Command {
        private bool condition;

        private ElseStatement elseStatement;

        public IfStatement(bool condition) {
            this.condition = condition;
        }

        public IfStatement(bool condition, ElseStatement elseStatment) {
            this.condition = condition;
            this.elseStatement = elseStatement;
        }

        public void Execute(List<PlayerCodeBlock> nested) {
            if (condition) {
                foreach (PlayerCodeBlock block in nested) {
                    ExecuteBlock(block);
                }
            } else if (elseStatement != null) {
                elseStatement.Execute(nested);
            }
        }
    }

    private class ElseStatement : Command {
        public void Execute(List<PlayerCodeBlock> nested) {
            foreach (PlayerCodeBlock block in nested) {
                ExecuteBlock(block);
            }
        }
    }

    private class Attack : Command {
        private HydraHead target;
        public Attack(HydraHead target) {
            this.target = target;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            target.hp -= player.atk;
        }
    }

    private class IceBlast : Command {
        private HydraHead target;
        public IceBlast(HydraHead target) {
            this.target = target;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            if target.GetType() == "fire" {
                target.hp -= player.atk * 2;
            } else {
                target.hp += player.atk;
            }
        }
    }

    private class FireBall : Command {
        private HydraHead target;
        public FireBall(HydraHead target) {
            this.target = target;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            if target.GetType() == "ice" {
                target.hp -= player.atk * 2;
            } else {
                target.hp += player.atk;
            }
        }
    }

    private class Heal : Command {
        public Heal() {
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            target.hp += player.atk / 2;
        }
    }

    private class HydraHead {
        private int maxHp = 100;
        private int hp;
        private int atk = 10;
        private List<string> types = new List<string>() {"ice", "fire"};
        private string type;
        private List<string> actions = new List<string>() {"attack", "heal", "reflect"};
        private string action;
        
        public HydraHead() {
            this.hp = maxHp;

            Random random = new Random();
            this.type = this.types[random.Next(this.types.Count)];
            this.action = this.actions[random.Next(this.actions.Count)];
        }
        public void GetHP() {
            return this.hp;
        }
        public void GetType() {
            return this.type;
        }
        public void GetAction() {
            return this.action;
        }
    }

    private class Player {
        private int maxHp = 300;
        private int hp;
        private int atk = 10;
        private List<string> actions = new List<string> {"iceblast", "fireball", "attack", "heal"};
        
        public Player() {
            this.hp = maxHp;
        }
        public void GetHP() {
            return this.hp;
        }
    }

    private string[] playerCodeArrays;
    private List<HydraHead> hydra;
    private Player player;
    private PlayerCodeBlock[] playerCode;
    private List<string> iterables = new List<string> {@"hydra.get_heads()", @"range(\d)"};
    
    private Dictionary<string, Func<List<HydraHead>>> listFunctionDictionary = new Dictionary<string, Func<List<HydraHead>>>() {
        {"hydra.get_heads()", () => hydra},
        {"heads", () => hydra}
    };

    private Dictionary<string, Func<int>> intFunctionDictionary = new Dictionary<string, Func<int>>() {
        {"len", (list) => list.Count},
        {"get_hp()", (entity) => entity.GetHP()}
    };
    private Dictionary<string, Func<string>> stringFunctionDictionary = new Dictionary<string, Func<string>>() {
        {"get_type()", (HydraHead hydrahead) => hydrahead.GetType()},
        {"get_action()", (HydraHead hydrahead) => hydrahead.GetAction()}
    };
    private Dictionary<string, int> loopVars = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        hydra = new HydraHead[5];
        player = new Player();
        if () {
            ExecuteMain();
        }
    }

    void ExecuteMain()
    {
        playerCodeComponent = GetComponent<TypeName>();
        // Split code at newlines, trim each line, add appropriate indentaion
        // If player indents code for no reason, unindent it
        // Remove all empty lines

        // Example
        string head = "for i in range(5):";
        string[] nested = new string[] {"attack(hydra.get_heads[i])", "heal()"};
        playerCode = new PlayerCodeBlock[] {new PlayerCodeBlock(head, nested)};
        foreach (PlayerCodeBlock block in playerCode) {
            try {
                ExecuteBlock(block);
            }
            catch (Exception e) {
                Debug.Log("Error: " + e.Message);
            }
        }
    }

    void ExecuteBlock(PlayerCodeBlock block) {
        string head = block.GetHead();
        List<PlayerCodeBlock> nested = block.GetNested();
        Command control = _TranslateLine(head);
        control.Execute(nested)
    }

    private Command _TranslateLine(string line) {
        // Limitations: Only single digit ints
        if (_CheckMatch(@"^for ", line)) { // For loop
            return _TranslateFor(line);
        } else if (_CheckMatch(@"^if ", line)) { // If statement
            return _TranslateIf(line);
        } else if (_CheckMatch(@"^else:", line)) { // Else statement
            return _TranslateElse(line);
        } else if (_CheckMatch(@"^\w([\S+])?\S()$", line)) {
            return _TranslateCommand(line);
        } else {
            throw Exception("Invalid code.");
        }
        }
    }

    private ForLoop _TranslateFor(string line) {
        for_pattern = @"^for\s+[a-zA-Z_][a-zA-Z0-9_]*\s+in\s+\S+:$";
        if (_CheckMatch(for_pattern, line)) {
            string loopVar = Regex.Replace(line, "^for\s+|in\s+\S+:$", "").Trim();
            if (loopVars.ContainsKey(loopVar)) {
                throw Exception("Invalid loop variable");
            }
            if (loopVar == "heads") {
                throw Exception("Invalid loop variable");
            }
            string iterable = Regex.Replace(line, "^for\s+[a-zA-Z_][a-zA-Z0-9_]*\s+in\s+|:$", "").Trim();
            if (iterables.Contains(iterable)) {
                if (iterable == "hydra.get_heads()") {
                    return new ForLoop(hydra.Count);
                } else if (iterable == "range(\d)") {
                    loopVars.Add(loopVar, 0);
                    return new ForLoop(iterable, loopVar);
                } else {
                    throw Exception("Invalid iterable");
                }
            } else {
                throw Exception("Invalid iterable");
            }
        } else {
            throw Exception("Invalid for loop");
        }
    }
    
    private IfStatement _TranslateIf(string line) {
        // Only handles if statements with one operation
        // Only handles certain expressions
        if_pattern = @"^if(\s+\S+)+:$";
        if (_CheckMatch(if_pattern, line)) {
            string conditionStr = Regex.Replace(line, "^if\s+|:$", "").Trim();
            MatchCollection ops = Regex.Matches(conditionStr, @"==|!=|<|>|<=|>=");
            if (ops.Count > 1) {
                throw Exception("Invalid if statement");
            } else if (ops.Count == 0) {
                if (condition[0] == "False" || condition[0] == "0") {
                    return new IfStatement(false);
                } else if (condition[0] == "True" || Regex.IsMatch(condition[0], @"^\d+$")) {
                    return new IfStatement(true);
                } else {
                    throw Exception("Invalid condition");
                }
            } else {
                string op = ops[0].Value;
                string[] condition = conditionStr.Split(op);
                return new IfStatement(_EvaluateOperation(condition[0], op, condition[1]));
            }
        } else {
            throw Exception("Invalid if statement");
        }
    }

    private ElseStatement _TranslateElse(string line) {
        return new ElseStatement();
    }

    private Command _TranslateCommand(string line) {
        if (_CheckMatch("^(attack|fireball|iceblast)\(\w+\)$")) {
            string[] parts = Regex.Split(line, @"(|)");
            if (parts[0] == "attack") {
                if (_TranslateExpression(parts[1]) is HydraHead) {
                    return new Attack((HydraHead)_TranslateExpression(parts[1]));
                } else {
                    throw Exception("Invalid attack target");
                }
            } else if (parts[0] == "fireball") {
                if (_TranslateExpression(parts[1]) is HydraHead) {
                    return new FireBall((HydraHead)_TranslateExpression(parts[1]));
                } else {
                    throw Exception("Invalid fireball target");
                }
            } else if (parts[0] == "iceblast") {
                if (_TranslateExpression(parts[1]) is HydraHead) {
                    return new IceBlast((HydraHead)_TranslateExpression(parts[1]));
                } else {
                    throw Exception("Invalid iceblast target");
                }
            }
        } else if (_CheckMatch("^heal\(\)$")) {
            return new Heal();
        } else {
            throw Exception("Invalid command");
        }
    }

    private object _TranslateExpression(string line) {
        line = line.Trim();
        if (_CheckMatch("^\d+$", lines)) {
            return int.Parse(line);
        }
        else if (_CheckMatch("^\".*\"$", line)) {
            return Regex.Replace(line, "^\"|\"$", "");
        } else if (_CheckMatch("^[^\[\]\s]+\.", line)) {
            if (_CheckMatch("^hydra.get_heads()", line)) {
                if (_CheckMatch("^hydra.get_heads\(\)$", line)) {
                    return hydra;
                } else if (_CheckMatch("^hydra.get_heads\(\)\[.*\]", line)) {
                    string[] parts = Regex.Split(line, @"[|]");
                    int index = _TranslateIndex(parts[1]);
                    if (_CheckMatch("^hydra.get_heads\(\)\[.*\]$")) {
                        return hydra[index];
                    } else {
                        if (intFunctionDictionary.ContainsKey(parts[2])) {
                            return intFunctionDictionary[parts[2]](hydra[index]);
                        } else if (stringFunctionDictionary.ContainsKey(parts[2])) {
                            return stringFunctionDictionary[parts[2]](hydra[index]);
                        } else {
                            throw Exception("Invalid expression");
                        }
                    }
                }
            } else if (_CheckMatch("^Player.getHP", line)) {
                return player.getHP();
            } else {
                throw Exception("Invalid expression");
            }
        } else if (_CheckMatch("^heads\[.*\]", line)) {
            string[] parts = Regex.Split(line, "\[|\]");
            int index = _TranslateIndex(parts[1]);
            if (_CheckMatch("^[^\.\s]+\[.*\]$")) {
                return hydra[index];
            } else if (_CheckMatch("^[^\.\s]+\[.*\]\.\S+$")){
                string sub_exp = Regex.Replace(line, "^.*\.", "");
                if (intFunctionDictionary.ContainsKey(sub_exp)) {
                    return intFunctionDictionary[sub_exp](hydra[index]);
                } else if (stringFunctionDictionary.ContainsKey(sub_exp)) {
                    return stringFunctionDictionary[sub_exp](hydra[index]);
                } else {
                    throw Exception("Invalid expression");
                }
            } else {
                throw Exception("Invalid expression");
            }
        } else {
            throw Exception("Invalid expression");
        }
    }

    private int _TranslateIndex(string indexStr) {
        // Only handles square brackets with one index
        if loopVars.ContainsKey(indexStr) {
            int index = loopVars[indexStr];
        } else if (Regex.IsMatch(indexStr, "^\d+$")) {
            return int.Parse(indexStr);
        } else {
            throw Exception("Invalid index");
        }
    }

    private bool _CheckMatch(string pattern, string input)
    {
        // Create a Regex object with the provided pattern
        Regex regex = new Regex(pattern);

        // Check if the input matches the regex pattern
        return regex.IsMatch(input);
    }

    private bool _EvaluateOperation(string left, string op, string right) {
        left = _TranslateExpression(left);
        right = _TranslateExpression(right);
        if (op == "==") {
            return left == right;
        } else if (op == "!=") {
            return left != right;
        } else if (op == "<") {
            return left < right;
        } else if (op == ">") {
            return left > right;
        } else if (op == "<=") {
            return left <= right;
        } else if (op == ">=") {
            return left >= right;
        } else {
            throw Exception("Invalid operation");
        }
    }
}