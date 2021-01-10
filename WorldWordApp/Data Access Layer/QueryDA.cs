using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldWordApp.DB;
using WorldWordApp.Objects;

namespace WorldWordApp.Data_Access_Layer
{
    class QueryDA
    {
        public const int TotalQuestionsInGame = 24;
        public const int TotalNumOfQueriesInDB = 49;

        private DataBaseConnector dbConnector;
        private DataTable dt;
        private List<Question> questionsList;
        private List<Query> queriesList;
        private string db_name;

        //private Thread updateThread;
        private int avgNumOfQuestions;

        public QueryDA(DataBaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
            db_name = Properties.Settings.Default["NameOfDB"].ToString();
        }

        public List<Question> QuestionsGeneration(string[] categories)
        {
            avgNumOfQuestions = TotalQuestionsInGame / categories.Length;

            /*this.updateThread = new Thread(delegate ()
            {*/
            try
            {
                // retrieving questions queries from DB
                RetrievingQueries(categories);
                questionsList = new List<Question>();
                // creating question objects
                CreateQuestionsList();


            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignoring - do nothing
            }
            catch (Exception)
            {
                MessageBox.Show("Failed creating questions for the game", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*});
            this.updateThread.Start();*/


            return questionsList;
        }

        // for each category get the query and add to a list
        public void RetrievingQueries(string[] categories)
        {
            // retrievingQueries
            queriesList = new List<Query>();

            for (int i = 0; i <= categories.Length - 1; i++)
            {
                int category = Int32.Parse(categories[i]);
                string query = "SELECT * FROM " + db_name + ".queries where category=(@category) ORDER BY RAND() LIMIT " + avgNumOfQuestions.ToString();
                // getting the queries in DB from the i category
                dt = dbConnector.RetrieveQueryByCategory(query, category);

                /* number of rows returned: exactly avgNumOfQuestions OR less (not possible more than avgNumOfQuestions because of the LIMIT)
                check if there are queries from the i category in the DB */
                if (dt.Rows.Count != 0)
                {
                    // counts how many queries created from the i category
                    int j = 0;
                    // if the dt containes less than the avgNumOfQuestions unique queries, loop
                    while (j != avgNumOfQuestions)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Query q = new Query(Convert.ToInt32(dr["id"]), dr["query"].ToString(), dr["question"].ToString(),
                                Convert.ToInt32(dr["category"]), Convert.ToInt32(dr["type"]), Convert.ToInt32(dr["answer_pos"]));
                            queriesList.Add(q);
                            j++;
                            if (j == avgNumOfQuestions) break;
                        }
                    }

                }
                else
                {
                    MessageBox.Show("ERROR: DB does not contain queries for category", "DB query problem ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ask only the query that are not the same (happan when few category)
        public List<Question> CreateQuestionsList()
        {
            // all the same queries will be next to each other - so execute once
            queriesList.Sort();
            int sameQuery = 0;
            Query prevQuery = queriesList[0];
            int id = queriesList[0].Id;
            int index = 0;
            foreach (Query questionQuery in queriesList)
            {
                index++;
                int newId = questionQuery.Id;
                // check if it is the same query as previous OR the last one
                if (newId == id && index != queriesList.Count)
                {
                    sameQuery++;
                }
                // finish same queries
                else
                {
                    // the last query equals to prev
                    if (index == queriesList.Count)
                    {
                        sameQuery++;
                    }

                    sameQuestionsCreation(prevQuery, sameQuery);

                    sameQuery = 1;
                    prevQuery = questionQuery;
                    id = questionQuery.Id;
                }

            }
            return questionsList;
        }

        // ask the query and add random rows from the answer to question list.
        public void sameQuestionsCreation(Query prevQuery, int sameQuery)
        {
            string query = prevQuery.QueryString;

            // no need to change anything - just run.
            dt = dbConnector.CreateCommandForDB(query, true);

            // there are queries from the category in the DB
            if (dt.Rows.Count != 0)
            {
                IEnumerable<DataRow> questionsData = SelectRows(sameQuery);
                foreach (DataRow dr in questionsData)
                {
                    // building question from the info in the row selected
                    questionsList.Add(BuildQuestionObject(dr, prevQuery));
                }
            }
            else
            {
                string str = "no data exists in DB for the id=" + prevQuery.Id + " query";
                MessageBox.Show(str, "DB query problem ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        // building a question
        public Question BuildQuestionObject(DataRow dr, Query q)
        {
            int colOfInformation = 2 - q.AnswerColumn;
            string partOfQuestion = dr.ItemArray[colOfInformation].ToString();
            // the question to be presented to user
            string finalQuestion = q.QuestionString + " " + partOfQuestion;
            // if the query returns float - cutting the number to be shorter
            var answer = dr.ItemArray[q.AnswerColumn - 1];
            // decimal number - want to be shown only one digit after dot
            if (q.Type == 3)
            {

                answer = String.Format("{0:F1}", answer);
            }

            Question qu = new Question(finalQuestion, answer.ToString());
            return qu;
        }

        // select random rows to select real question.
        public IEnumerable<DataRow> SelectRows(int sameQuery)
        {
            var rand = new Random();
            //getting rows as the num of questions from the specific query
            var questionsData = dt.AsEnumerable().OrderBy(r => rand.Next()).Take(sameQuery);
            return questionsData;

        }

    }
}
