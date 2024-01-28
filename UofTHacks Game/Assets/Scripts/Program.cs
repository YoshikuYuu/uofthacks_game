using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
using TMPro;
using System.Text.RegularExpressions;
public class Program : MonoBehaviour
{
    public class PlayerCodeBlock {
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

    public class ForLoop : Command {
        private int start; 
        private int end;
        private int step;
        private string loopVar;
        public ForLoop(string function, string loopVar) {
            // Note that when executing loop, the list shall not be mutated until after turn end
            this.loopVar = loopVar;
            MatchCollection matches = Regex.Matches(function, @"\d+");
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
                throw new Exception("Invalid for loop");
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

    public class IfStatement : Command {
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
                    Program.ExecuteBlock(block);
                }
            } else if (elseStatement != null) {
                elseStatement.Execute(nested);
            }
        }
    }

    public class ElseStatement : Command {
        public void Execute(List<PlayerCodeBlock> nested) {
            foreach (PlayerCodeBlock block in nested) {
                ExecuteBlock(block);
            }
        }
    }

    public class Attack : Command {
        private HydraHead target;
        public Attack(HydraHead target) {
            this.target = target;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            target.TakeDamage(player.GetAttack());

        }
    }

    public class IceBlast : Command {
        private HydraHead target;
        public IceBlast(HydraHead target) {
            this.target = target;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            if (target.GetHydraType() == "fire") {
                target.TakeDamage(player.GetAttack() * 2);
            } else {
                target.TakeDamage(-player.GetAttack());
            }
        }
    }

    public class FireBall : Command {
        private HydraHead target;
        public FireBall(HydraHead target) {
            this.target = target;
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            if (target.GetHydraType() == "ice") {
                target.TakeDamage(player.GetAttack() * 2);
            } else {
                target.TakeDamage(-player.GetAttack());
            }
        }
    }

    public class Heal : Command {
        public Heal() {
        }
        public void Execute(List<PlayerCodeBlock> nested) {
            player.Heal(player.GetAttack() / 2);
        }
    }

    public interface Entity {
        int GetHP();
    }
    public class HydraHead : Entity {
        private int maxHp = 100;
        private int hp;
        private int atk = 10;
        private List<string> types = new List<string>() {"ice", "fire"};
        private string type;
        private List<string> actions = new List<string>() {"attack", "heal", "reflect"};
        private string action;
        
        public HydraHead() {
            this.hp = maxHp;

            System.Random random = new System.Random();
            this.type = this.types[random.Next(this.types.Count)];
            this.action = this.actions[random.Next(this.actions.Count)];
        }
        public int GetHP() {
            return this.hp;
        }
        public string GetHydraType() {
            return this.type;
        }
        public string GetAction() {
            return this.action;
        }
        public void TakeDamage(int damage) {
            this.hp -= damage;
        }
    }

    public class Player : Entity {
        private int maxHp = 300;
        private int hp;
        private int atk = 10;
        private List<string> actions = new List<string> {"iceblast", "fireball", "attack", "heal"};
        
        public Player() {
            this.hp = maxHp;
        }
        public int GetHP() {
            return this.hp;
        }

        public int GetAttack() {
            return this.atk;
        }

        public void TakeDamage(int damage) {
            this.hp -= damage;
        }

        public void Heal(int amount) {
            this.hp += amount;
        }
    }

    private string[] playerCodeComponent;
    private static List<HydraHead> hydra;
    private static Player player;
    private PlayerCodeBlock[] playerCode;
    private static List<string> iterables = new List<string> {@"hydra.get_heads()", @"range(\d)"};
    
    private static Dictionary<string, Func<List<HydraHead>>> listFunctionDictionary;

    private static Dictionary<string, Func<List<object>, int>> intFunctionDictionary = new Dictionary<string, Func<List<object>, int>>() {
        {"len", (list) => list.Count},
        {"get_hp()", (entity) => ((Entity) entity[0]).GetHP()}
    };

    private static Dictionary<string, Func<HydraHead, string>> stringFunctionDictionary = new Dictionary<string, Func<HydraHead, string>>() {
        {"get_type()", (hydrahead) => hydrahead.GetHydraType()},
        {"get_action()", (hydrahead) => hydrahead.GetAction()}
    };
    private static Dictionary<string, int> loopVars = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        hydra = new List<HydraHead>(7) {
            new HydraHead(),
            new HydraHead(),
            new HydraHead(),
            new HydraHead(),
            new HydraHead(),
            new HydraHead(),
            new HydraHead()
        };

        player = new Player();

        listFunctionDictionary = new Dictionary<string, Func<List<HydraHead>>>() 
        {
            {"hydra.get_heads()", () => hydra},
            {"heads", () => hydra}
        };

        
    }

    void ExecuteMain(string[] codeLines)
    {
        playerCodeComponent = codeLines;
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
            catch {
                Exception e = new Exception("Invalid code.");
                Debug.Log("Error: " + e.Message);
            }
        }
    }

    static private void ExecuteBlock(PlayerCodeBlock block) {
        string head = block.GetHead();
        List<PlayerCodeBlock> nested = block.GetNested();
        Command control = _TranslateLine(head);
        control.Execute(nested);
    }

    static private Command _TranslateLine(string line) {
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
            throw new Exception("Invalid code.");
        }
    }

    static private ForLoop _TranslateFor(string line) {
        string for_pattern = @"^for\s+[a-zA-Z_][a-zA-Z0-9_]*\s+in\s+\S+:$";
        if (_CheckMatch(for_pattern, line)) {
            string loopVar = Regex.Replace(line, @"^for\s+|in\s+\S+:$", "").Trim();
            if (loopVars.ContainsKey(loopVar)) {
                throw new Exception("Invalid loop variable");
            }
            if (loopVar == "heads") {
                throw new Exception("Invalid loop variable");
            }
            string iterable = Regex.Replace(line, @"^for\s+[a-zA-Z_][a-zA-Z0-9_]*\s+in\s+|:$", "").Trim();
            if (iterables.Contains(iterable)) {
                if (iterable == "hydra.get_heads()") {
                    return new ForLoop(hydra.Count);
                } else if (iterable == @"range(\d)") {
                    loopVars.Add(loopVar, 0);
                    return new ForLoop(iterable, loopVar);
                } else {
                    throw new Exception("Invalid iterable");
                }
            } else {
                throw new Exception("Invalid iterable");
            }
        } else {
            throw new Exception("Invalid for loop");
        }
    }
    
    static private IfStatement _TranslateIf(string line) {
        // Only handles if statements with one operation
        // Only handles certain expressions
        string if_pattern = @"^if(\s+\S+)+:$";
        if (_CheckMatch(if_pattern, line)) {
            string conditionStr = Regex.Replace(line, @"^if\s+|:$", "").Trim();
            MatchCollection ops = Regex.Matches(conditionStr, @"==|!=|<|>|<=|>=");
            if (ops.Count > 1) {
                 throw new Exception("Invalid if statement");
            } else if (ops.Count == 0) {
                if (line == "False" || line == "0") {
                    return new IfStatement(false);
                } else if (line == "True" || Regex.IsMatch(line, @"^\d+$")) {
                    return new IfStatement(true);
                } else {
                     throw new Exception("Invalid condition");
                }
            } else {
                string op = ops[0].Value;
                string[] condition = conditionStr.Split(op);
                return new IfStatement(_EvaluateOperation(condition[0], op, condition[1]));
            }
        } else {
             throw new Exception("Invalid if statement");
        }
    }

    static private ElseStatement _TranslateElse(string line) {
        return new ElseStatement();
    }

    static private Command _TranslateCommand(string line) {
        if (_CheckMatch(@"^(attack|fireball|iceblast)\(\w+\)$", line)) {
            string[] parts = Regex.Split(line, @"(|)");
            if (parts[0] == "attack") {
                if (_TranslateExpression(parts[1]).Item1 is HydraHead) {
                    return new Attack((HydraHead)_TranslateExpression(parts[1]).Item1);
                } else {
                    throw new Exception("Invalid attack target");
                }
            } else if (parts[0] == "fireball") {
                if (_TranslateExpression(parts[1]).Item1 is HydraHead) {
                    return new FireBall((HydraHead)_TranslateExpression(parts[1]).Item1);
                } else {
                    throw new Exception("Invalid fireball target");
                }
            } else if (parts[0] == "iceblast") {
                if (_TranslateExpression(parts[1]).Item1 is HydraHead) {
                    return new IceBlast((HydraHead)_TranslateExpression(parts[1]).Item1);
                } else {
                    throw new Exception("Invalid iceblast target");
                }
            } else {
                throw new Exception("Invalid command");
            }
        } else if (_CheckMatch(@"^heal\(\)$", line)) {
            return new Heal();
        } else {
            throw new Exception("Invalid command");
        }
    }

    static private (object, string) _TranslateExpression(string line) {
        line = line.Trim();
        if (_CheckMatch(@"^\d+$", line)) {
            return (int.Parse(line), "int");
        }
        else if (_CheckMatch("^\".*\"$", line)) {
            return (Regex.Replace(line, "^\"|\"$", ""), "string");
        } else if (_CheckMatch(@"^[^\[\]\s]+\.", line)) {
            if (_CheckMatch("^hydra.get_heads()", line)) {
                if (_CheckMatch(@"^hydra.get_heads\(\)$", line)) {
                    return (hydra, "list of hydra heads");
                } else if (_CheckMatch(@"^hydra.get_heads\(\)\[.*\]", line)) {
                    string[] parts = Regex.Split(line, @"[|]");
                    int index = _TranslateIndex(parts[1]);
                    if (_CheckMatch(@"^hydra.get_heads\(\)\[.*\]$", line)) {
                        return (hydra[index], "hydra head");
                    } else {
                        throw new Exception("Invalid expression");
                        // if (intFunctionDictionary.ContainsKey(parts[2])) {
                        //     return (intFunctionDictionary[parts[2]](hydra[index]), "int");
                        // } else if (stringFunctionDictionary.ContainsKey(parts[2])) {
                        //     return (stringFunctionDictionary[parts[2]](hydra[index]), "string");
                        // } else {
                        //     throw new Exception("Invalid expression");
                        // }
                    }
                }
            // } else if (_CheckMatch("^Player.getHP", line)) {
            //     return (player.getHP(), "int");
            } else {
                throw new Exception("Invalid expression");
            }
        } else if (_CheckMatch(@"^heads\[.*\]", line)) {
            string[] parts = Regex.Split(line, @"\[|\]");
            int index = _TranslateIndex(parts[1]);
            if (_CheckMatch(@"^[^\.\s]+\[.*\]$", line)) {
                return (hydra[index], "hydra head");
            // } else if (_CheckMatch(@"^[^\.\s]+\[.*\]\.\S+$")){
            //     string sub_exp = Regex.Replace(line, @"^.*\.", "");
            //     if (intFunctionDictionary.ContainsKey(sub_exp)) {
            //         return (intFunctionDictionary[sub_exp](hydra[index]), "int");
            //     } else if (stringFunctionDictionary.ContainsKey(sub_exp)) {
            //         return (stringFunctionDictionary[sub_exp](hydra[index]), "string");
            //     } else {
            //         throw new Exception("Invalid expression");
            //     }
            } else {
                throw new Exception("Invalid expression");
            }
        } else {
            throw new Exception("Invalid expression");
        }
        throw new Exception("Invalid expression");
    }

    static private int _TranslateIndex(string indexStr) {
        // Only handles square brackets with one index
        if (loopVars.ContainsKey(indexStr)) {
            return loopVars[indexStr];
        } else if (Regex.IsMatch(indexStr, @"^\d+$")) {
            return int.Parse(indexStr);
        } else {
            throw new Exception("Invalid index");
        }
    }

    static private bool _CheckMatch(string pattern, string input)
    {
        // Create a Regex object with the provided pattern
        Regex regex = new Regex(pattern);

        // Check if the input matches the regex pattern
        return regex.IsMatch(input);
    }

    static private bool _EvaluateOperation(object left, string op, object right) {
        if (left is int && right is int) {
            if (op == "==") {
                return (int)left == (int)right;
            } else if (op == "!=") {
                return (int)left != (int)right;
            } else if (op == "<") {
                return (int)left < (int)right;
            } else if (op == ">") {
                return (int)left > (int)right;
            } else if (op == "<=") {
                return (int)left <= (int)right;
            } else if (op == ">=") {
                return (int)left >= (int)right;
            } else {
                throw new Exception("Invalid operation");
            }
        } else if (left is string && right is string) {
            if (op == "==") {
                return (string)left == (string)right;
            } else if (op == "!=") {
                return (string)left != (string)right;
            } else {
                throw new Exception("Invalid operation");
            }
        } else {
            throw new Exception("Invalid operation");
        }
    }
}