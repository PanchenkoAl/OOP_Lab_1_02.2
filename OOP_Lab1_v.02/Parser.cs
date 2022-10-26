using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Lab1_v._02
{
    internal class Parser
    {
        public List<string> startTracer = new List<string>();
        public List<string> endTracer = new List<string>();

        public bool cycle()
        {
            for(int i = 0; i < startTracer.Count; i ++)
            {
                string[] path = new string[startTracer.Count + 2];
                for(int o = 0; o < path.Length; o ++)
                {
                    path[o] = null;
                }
                path[0] = startTracer[i];
                string check = startTracer[i];
                for(int j = i; j <  startTracer.Count; j ++)
                {
                    if(startTracer[j] == check)
                    {
                        check = endTracer[j];
                        path[j + 1] = check;
                        int k = 0;
                        while (k < path.Length && path[k] != null)
                        {
                            int kk = k + 1;
                            while (kk < path.Length && path[kk] != null)
                            {
                                if (path[k] == path[kk])
                                    return false;
                                kk++;
                            }
                            k++;
                        }
                    }
                }
            }
            return true;
        }

        public string ParseExpression(string exp, Cell[,] table, int row, int col)
        {
            if(exp == null || exp.Length == 0)
            {
                MessageBox.Show("#ERROR_PE_EXPRESSION_IS_NULL");
                return "0";
            }
            exp = exp.Replace(" ", "");
            if (exp == null || exp.Length == 0)
            {
                MessageBox.Show("#ERROR_PE_EXPRESSION_IS_NULL");
                return "0";
            }
            if(!cycle())
            {
                MessageBox.Show("#ERROR_CYCLE");
                return "0";
            }
            if (exp[0] != '(' || exp[exp.Length - 1] != ')')
            {
                string temp = exp;
                exp = null;
                exp += "(";
                exp += temp;
                exp += ")";
            }
            
                

            exp = FindIncDec(exp);
            //MessageBox.Show(exp);
            exp = SetBrackets(exp, table);
            exp = exp.Replace(" ", "");
            //MessageBox.Show("SB " + exp);
            exp = EvaluateBrackets(exp, table, row, col);
            return exp;
        }

        public bool IsNumber(char c)
        {
            if ("1234567890".IndexOf(c) != -1)
                return true;
            else
                return false;
        }

        public Lexer InstanceOfLexer = new Lexer();

        public string FindIncDec(string exp)
        {
            for(int i = 0; i < exp.Length - 3; i ++)
            {
                if(exp[i] == 'i' && exp[i+1] == 'n' && exp[i + 2] == 'c' && exp[i + 3] == '(')
                {
                    //MessageBox.Show("inc " + i);
                    string t1 = null;
                    string t2 = null;
                    for (int k = 0; k < i; k++)
                        t1 += exp[k];
                    //MessageBox.Show("t1 " + t1);
                    int g = i + 4;
                    while (exp[g] != ')')
                        g++;
                    for (int k = g + 1; k < exp.Length; k++)
                        t2 += exp[k];
                    //MessageBox.Show("t2 " + t2);
                    string ss = null;
                    for (int k = i + 4; k < g; k++)
                        ss += exp[k];
                    //MessageBox.Show("ss " + ss);
                    string res = null;
                    res += t1;
                    res += "(";
                    res += ss;
                    res += "+1)";
                    res += t2;
                    exp = res;
                    return exp;
                }
            }
            for (int i = 0; i < exp.Length - 3; i ++)
            {
                if (exp[i] == 'd' && exp[i + 1] == 'e' && exp[i + 2] == 'c' && exp[i + 3] == '(')
                {
                    string t1 = null;
                    string t2 = null;
                    for (int k = 0; k < i; k++)
                        t1 += exp[k];
                    int g = i + 4;
                    while (exp[g] != ')')
                        g++;
                    for (int k = g + 1; k < exp.Length; k++)
                        t2 += exp[k];
                    string ss = null;
                    for (int k = i + 4; k < g; k++)
                        ss += exp[k];
                    string res = null;
                    res += t1;
                    res += "(";
                    res += ss;
                    res += "-1)";
                    res+= t2;
                    exp = res;
                    return exp;
                }
            }
            return exp;
        }

        public string SetBrackets(string exp, Cell[,] table)
        {
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '^')
                {
                    for (int k = i + 1; k < exp.Length; k++)
                    {
                        if (exp[k] == '(')
                        {
                            while (exp[k] != ')' && k < exp.Length)
                                k++;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == exp.Length - 1)
                        {
                            exp = exp.Insert(k, ")");
                            break;
                        }
                    }
                    for (int k = i - 1; k > 0; k--)
                    {
                        if (exp[k] == ')')
                        {
                            while (exp[k] != '(' && k > 0)
                                k--;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == 1)
                        {
                            exp = exp.Insert(k - 1, "(");
                            i++;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '*' || exp[i] == '/')
                {
                    //MessageBox.Show("*");
                    for (int k = i + 1; k < exp.Length; k++)
                    {
                        int cb = 0;
                        if (exp[k] == '(')
                        {
                            cb++;
                            while (exp[k] != ')' && k < exp.Length || cb != 0)
                            {
                                k++;
                                if (exp[k] == '(')
                                    cb++;
                                if (exp[k] == ')')
                                    cb--;
                            }
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || exp[k] == ')')
                        {
                            exp = exp.Insert(k, ")"); //MessageBox.Show(exp);
                            break;
                        }
                    }
                    for (int k = i - 1; k > 0; k--)
                    {
                        int cb = 0;
                        if (exp[k] == ')')
                        {
                            cb++;
                            while (exp[k] != '(' && k > 0 || cb != 0)
                            {
                                k--;
                                if (exp[k] == ')')
                                    cb++;
                                if (exp[k] == '(')
                                    cb--;
                            }
                        }
                        //MessageBox.Show(k.ToString() + " " + exp[k] + " " + InstanceOfLexer.IsDelim(exp[k]).ToString());
                        if (InstanceOfLexer.IsDelim(exp[k]) || exp[k] == '(')
                        {
                            //MessageBox.Show((k).ToString() + " " + exp);
                            exp = exp.Insert(k + 1, "(");
                            //MessageBox.Show((k).ToString() + " " + exp);
                            i++;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '+' || exp[i] == '-')
                {
                    //MessageBox.Show("+");
                    for (int k = i + 1; k < exp.Length; k++)
                    {
                        int cb = 0;
                        if (exp[k] == '(')
                        {
                            cb++;
                            while (exp[k] != ')' && k < exp.Length || cb != 0)
                            {
                                k++;
                                if (exp[k] == '(')
                                    cb++;
                                if (exp[k] == ')')
                                    cb--;
                            }
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || exp[k] == ')')
                        {
                            exp = exp.Insert(k, ")");
                            break;
                        }
                    }
                    for (int k = i - 1; k >= 0; k--)
                    {
                        int cb = 0;
                        if (exp[k] == ')')
                        {
                            cb++;
                            while (exp[k] != '(' && k > 0 || cb != 0)
                            {
                                k--;
                                if (exp[k] == ')')
                                    cb++;
                                if (exp[k] == '(')
                                    cb--;
                            }
                        }
                        //MessageBox.Show(k.ToString() + " " + exp + " " + InstanceOfLexer.IsDelim(exp[k]).ToString());
                        if (InstanceOfLexer.IsDelim(exp[k]) || exp[k] == '(')
                        {
                            //MessageBox.Show((k).ToString() + " " + exp);
                            exp = exp.Insert(k + 1, "(");
                           // MessageBox.Show((k).ToString() + " " + exp);
                            i++;
                            break;
                        }
                    }
                }
            }
            //MessageBox.Show(exp + " ret");
            
            return exp;
        }
        public string EvaluateBrackets(string exp, Cell[,] table, int row, int col)
        {
            
            if(exp == null)
            {
                MessageBox.Show("#ERROR_EB_EXPRESSION_IS_NULL");
                return "0";
            }
            int openBracketIdx = 0;
            int closeBracketIdx = 0;
            //MessageBox.Show("Evaluating brackets started");
            //MessageBox.Show(exp);
            for(int i = exp.Length - 1; i >= 0; i--)
            {
                if (exp[i] == '(')
                {
                    openBracketIdx = i;
                    i++;
                    while (exp[i] != ')')
                        i++;
                    closeBracketIdx = i;
                    //MessageBox.Show("Close Bracket Idx = " + closeBracketIdx);
                    string inBracketExpression = null;
                    for(int k = openBracketIdx + 1; k < closeBracketIdx; k++)
                        inBracketExpression += exp[k];
                    string rightExpression = null;
                    string leftExpression = null;
                    for (int k = 0; k < openBracketIdx; k++)
                        leftExpression += exp[k];
                    for(int k = closeBracketIdx + 1; k < exp.Length; k++)
                        rightExpression += exp[k];
                    string tempResult = null;
                    tempResult += EvaluateExpression(inBracketExpression, table, row, col);
                    //MessageBox.Show("Temp Result = " + tempResult);
                    string result = null;
                    result += leftExpression;
                    result += tempResult;
                    result += rightExpression;
                    //MessageBox.Show("End Result = " + result);
                    exp = result;
                    i = openBracketIdx;
                }
            }
            return exp;
        }

        public string EvaluateExpression(string exp, Cell[,] table, int row, int col)
        {
            int idm;
            if (exp == null)
            {
                MessageBox.Show("#ERROREE");
                return "0";
            }
            string op1 = null;
            string op2 = null;
            double res1 = 0, res2;
            char delim = '0';
            bool flag = false;

            for(int i = 0; i < exp.Length; i ++)
            {          
                int rb = exp.Length, lb = - 1;
                int delimIdx = FindDelim(exp);
                delim = exp[delimIdx];
                int cou = 0;
                for(int oo = 0; oo < exp.Length; oo ++)
                {
                    if (InstanceOfLexer.IsDelim(exp[oo]))
                    {
                        cou++;
                    }
                }
                if(cou == 0)
                {
                    flag = true;
                }
                for(int k = delimIdx + 1; k < exp.Length; k ++)
                {
                    if (InstanceOfLexer.IsDelim(exp[k]))
                    {
                        rb = k;
                    }
                }
                for (int k = delimIdx - 1; k > 0; k--)
                {
                    if (InstanceOfLexer.IsDelim(exp[k]))
                    {
                        lb = k;
                    }
                }
                for(int k = lb + 1; k < delimIdx; k ++)
                {
                    op1 += exp[k];
                }
                for (int k = delimIdx + 1; k < rb; k++)
                {
                    op2 += exp[k];
                }
                if(flag)
                {
                    op1 = exp;
                    res1 = FindCell(op1, table, row, col);
                    return res1.ToString();
                }
                res1 = FindCell(op1, table, row, col);
                res2 = FindCell(op2, table, row, col);
                res1 = CalculateResult(res1, res2, delim);
                return res1.ToString();
            }
            return res1.ToString();
        }

        public int FindDelim (string exp)
        {
            int delimIdx = 0;
            for(int i = 0; i < exp.Length; i ++)
            {
                if (exp[i] == '^')
                {
                    delimIdx = i;
                    return delimIdx;
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '*' || exp[i] == '/')
                {
                    delimIdx = i;
                    return delimIdx;
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '+' || exp[i] == '-')
                {
                    delimIdx = i;
                    return delimIdx;
                }
            }
            return delimIdx;
        }

        public double FindCell(string exp, Cell[,] table, int row, int col)
        {
            double oper1 = 0.0;
            string addressC = null;
            string addressR = null;
            Lexer lexer = new Lexer();
            if (exp == null)
                MessageBox.Show("exp is null");
            if (exp != null)
                if (IsNumber(exp[0]))
                {
                    oper1 = Double.Parse(exp);
                    return oper1;
                }
                else
                {
                    int it = 0;
                    while (it < exp.Length && InstanceOfLexer.IsLetter(exp[it]))
                    {
                        addressC += exp[it];
                        it++;
                    }
                    int itt = it;
                    while (itt < exp.Length && InstanceOfLexer.IsNumber(exp[itt]))
                    {
                        addressR += exp[itt];
                        itt++;
                    }
                    int c1 = 0, c2 = 0;
                    //MessageBox.Show("AddressC = " + addressC + " | " + addressC.Length.ToString());
                    if(addressC != null)
                    for (int k = 0; k < addressC.Length; k++)
                    {
                        if (addressC.Length != 1)
                            c1 += ((int)addressC[k] - 64) * (26 ^ (addressC.Length - k) - 1) - 1;
                        else
                            c1 += ((int)addressC[k] - 65);
                    }
                    if(addressR != null)
                    for (int k = 0; k < addressR.Length; k++)
                    {
                            if (addressC.Length != 1)
                                c2 += ((int)addressR[k] - 48) * (10 ^ (addressR.Length - k - 1));
                            else
                                c2 += ((int)addressR[k] - 48);
                    }
                    //MessageBox.Show("c1 = " + c1.ToString() + " c2 = " + c2.ToString());
                    //MessageBox.Show("table cv = " + table[c2, c1].cValue);
                    if(c1 < 100 && c2 < 100)
                    {
                        string st = null;
                        string et = null;
                        st += col;
                        st += "_";
                        st += row;
                        et += c1;
                        et += "_";
                        et += c2;
                        startTracer.Add(st);
                        endTracer.Add(et);
                        string buffer = ParseExpression(table[c2, c1].cValue, table, c2, c1);
                        oper1 = Double.Parse(buffer);
                        return oper1;
                    }
                    else
                    {
                        MessageBox.Show("#ERROR_FC_CELL_DOES_NOT_EXIST");
                        return 0.0;
                    }
                }
            else
                return 0.0;
        }

        public double CalculateResult(double r1, double r2, char del)
        {
            switch(del)
            {
                case '+':
                    return r1 + r2;
                case '-':
                    return r1 - r2;
                case '*':
                    return r1 * r2;
                case '/':
                    if(r2 != 0)
                        return r1 / r2;
                    else
                    {
                        return r1;
                        MessageBox.Show("#ERROR_DIVIDE_BY_ZERO");
                    }
                default:
                    MessageBox.Show("Wrong operation");
                    return 0.0;
            }
        }
    }
}
