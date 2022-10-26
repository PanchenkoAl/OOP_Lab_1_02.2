using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Lab1_v._02
{
    internal class Lexer 
    {
        private string token;
        public enum Types { NONE, DELIMITER, VARIABLE, NUMBER };
        public enum Errors { NOERR, SYNTAX, UNBALPARENTS, NOEXP, DIVBYZERO };
        public Errors tokenError;
        public Types tokenType;

        public bool IsDelim(char c)
        {
            if ("+-/*%^=|&".IndexOf(c) != -1)
                return true;
            else
                return false;
        }

        public bool IsNumber(char c)
        {
            if ("1234567890".IndexOf(c) != -1)
                return true;
            else
                return false;
        }
        public bool IsLetter(char c)
        {
            if ("AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz".IndexOf(c) != -1)
                return true;
            else
                return false;
        }
    }
}
