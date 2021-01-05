using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    public class Question
    {
        public Question(string question, string answer)
        {
            QuestionString = question;
            AnswerString = answer;
        }

        public String QuestionString { get; set; }

        public String AnswerString { get; set; }
    }
}
