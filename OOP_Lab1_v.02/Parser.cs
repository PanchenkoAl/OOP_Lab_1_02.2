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
        public Lexer InstanceOfLexer = new Lexer();

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
            if (exp == null || exp.Length == 0)
            {
                MessageBox.Show("#ERROR_PE_EXPRESSION_IS_NULL");
                return "#ERROR";
            }
            exp = exp.Replace(" ", "");
            for (int i = 0; i < exp.Length; i ++)
            {
                if ("+-/*^ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890()".IndexOf(exp[i]) != -1)
                {

                }
                else
                {
                    MessageBox.Show("#ERROR_SYNTAX");
                    return "#ERROR";
                }
            }
            if (exp == null || exp.Length == 0)
            {
                MessageBox.Show("#ERROR_PE_EXPRESSION_IS_NULL");
                return "#ERROR";
            }
            if(!cycle())
            {
                MessageBox.Show("#ERROR_CYCLE");
                return "#CYCLE";
            }
            if (exp[0] != '(' || exp[exp.Length - 1] != ')')
            {
                string temp = exp;
                exp = null;
                exp += "(";
                exp += temp;
                exp += ")";
            }


            //exp = FindMinuses(exp, table, row, col);
            exp = FindIncDec(exp);
            exp = SetBrackets(exp, table);
            exp = exp.Replace(" ", "");
            exp = EvaluateBrackets(exp, table, row, col);
            return exp;
        }

        public string FindMinuses(string exp, Cell[,] table, int row, int col)
        {
            for(int i = 1; i < exp.Length; i ++)
            {
                if (exp[i] == '-' && exp[i-1] == '(')
                {
                    int k = i + 1;
                    string subExp = null;
                    string left = null;
                    string right = null;
                    for (int g = 0; g < i; g++)
                        left += exp[g];
                    while (exp[k] != ')')
                    {
                        subExp += exp[k];
                        k++;
                    }
                    for (int g = k; g < exp.Length; g++)
                        right += exp[g];

                    string result = ParseExpression(subExp, table, row, col);
                    double res = Double.Parse(result);
                    res = -res;
                    int addC = 0, addR = 0;
                    for(int ii = 100; ii < 200; ii++)
                    {
                        for(int jj = 100; jj < 200; jj ++)
                        {
                            if(table[jj, ii].cValue == null)
                            {
                                addC = ii;
                                addR = jj;
                                table[jj, ii].cValue = subExp.Insert(0, "-");
                                table[jj, ii].cValue_s = res;
                            }
                        }
                    }
                    string ins = null;
                    int mod = addC / 26 - 1;
                    int ost = addC % 26;
                    ins += (char)(mod + 65);
                    if(ost != 0 && mod > 1)
                        ins += (char)(ost + 64);
                    ins += addR.ToString();
                    string end = null;
                    end += left += ins += right;
                }
            }
            return exp;
        }

        public string FindIncDec(string exp)
        {
            for(int i = 0; i < exp.Length - 3; i ++)
            {
                if(exp[i] == 'i' && exp[i+1] == 'n' && exp[i + 2] == 'c' && exp[i + 3] == '(')
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
                        if (InstanceOfLexer.IsDelim(exp[k]) || exp[k] == '(')
                        {
                            exp = exp.Insert(k + 1, "(");
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
                        if (InstanceOfLexer.IsDelim(exp[k]) || exp[k] == '(')
                        {
                            exp = exp.Insert(k + 1, "(");
                            i++;
                            break;
                        }
                    }
                }
            }
            
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
            for(int i = exp.Length - 1; i >= 0; i--)
            {
                if (exp[i] == '(')
                {
                    openBracketIdx = i;
                    i++;
                    while (exp[i] != ')')
                        i++;
                    closeBracketIdx = i;
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
                    tempResult += EvaluateExpression1(inBracketExpression, table, row, col);
                    string result = null;
                    result += leftExpression;
                    result += tempResult;
                    result += rightExpression;
                    exp = result;
                    i = openBracketIdx;
                }
            }
            return exp;
        }

        public string EvaluateExpression1 (string exp, Cell[,] table, int row, int col)
        {
            if (exp == null)
            {
                MessageBox.Show("#ERROREE");
                return "0";
            }
            string left = null, right = null;
            double left_d = 0, right_d = 0;
            for(int i = 0; i < exp.Length; i++)
            {
                if (InstanceOfLexer.IsDelim(exp[i]) && i != 0)
                {
                    for(int k = 0; k < i; k ++)
                        left += exp[k];
                    for (int k = i + 1; k < exp.Length; k++)
                        right += exp[k];
                    
                    if (InstanceOfLexer.IsLetter(left[0]))
                        left_d = FindCell(left, table, row, col);
                    else if (InstanceOfLexer.IsNumber(left[0]))
                        left_d = Double.Parse(left);
                    else if (left[0] == '-')
                    {
                        if (InstanceOfLexer.IsLetter(left[1]))
                        {
                            string temp = null;
                            for (int o = 1; o < left.Length; o++)
                                temp += left[o];
                            left = temp;
                            left_d = FindCell(left, table, row, col);
                            left_d = -left_d;
                        }
                        else if (InstanceOfLexer.IsNumber(left[1]))
                            left_d = Double.Parse(left);
                    }


                    if (InstanceOfLexer.IsLetter(right[0]))
                        right_d = FindCell(right, table, row, col);
                    else
                        if (InstanceOfLexer.IsNumber(right[0]))
                        right_d = Double.Parse(right);
                    double result = CalculateResult(left_d, right_d, exp[i]);
                    
                    return result.ToString();
                }
            }
            string op = exp;
            double op_d = 0;
            if (InstanceOfLexer.IsLetter(op[0]))
                op_d = FindCell(op, table, row, col);
            else if (InstanceOfLexer.IsNumber(op[0]))
                op_d = Double.Parse(op);
            else if (op[0] == '-')
            {
                if (InstanceOfLexer.IsLetter(op[1]))
                {
                    string temp = null;
                    for (int o = 1; o < op.Length; o++)
                        temp += op[o];
                    op = temp;
                    op_d = FindCell(op, table, row, col);
                    op_d = -op_d;
                }
                else if (InstanceOfLexer.IsNumber(op[1]))
                    op_d = Double.Parse(op);
            }
            return op_d.ToString();
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
                if (InstanceOfLexer.IsNumber(exp[0]))
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
                        try
                        {
                            oper1 = Double.Parse(buffer);
                            table[c2, c1].cValue_s = oper1;
                            return oper1;
                        }
                        catch (System.FormatException)
                        {
                            return 0.0;
                        }
                        

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
