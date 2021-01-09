using System;


namespace WorldWordApp.Objects
{
    class Query : IEquatable<Query>, IComparable<Query>
    {
        public Query(int id, string query, string question, int category, int type, int answerColumn)
        {
            Id = id;
            QueryString = query;
            QuestionString = question;
            Category = category;
            Type = type;
            AnswerColumn = answerColumn;
        }

        public int Id { get; set; }

        public String QueryString { get; set; }

        public String QuestionString { get; set; }

        public int Category { get; set; }

        public int Type { get; set; }
        // the column the answer exists in
        public int AnswerColumn { get; set; }

        public int CompareTo(Query compareQuery)
        {
            // A null value means that this object is greater.
            if (compareQuery == null)
                return 1;
            else
                return this.Id.CompareTo(compareQuery.Id);

        }
        // check if two Query objects are equal
        public bool Equals(Query other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id);
        }
    }
}
