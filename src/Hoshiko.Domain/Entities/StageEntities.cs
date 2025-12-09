namespace Hoshiko.Domain.Entities
{
    public class Stage
    {
        public int Id {get;set;}
        public string Name {get;set;} = null!;

        public ICollection<Quiz> Quizzes {get;set;} = new List<Quiz>();
        public ICollection<Learn> Learns {get;set;} = new List<Learn>();
    }


    public class Quiz
    {
        public int Id {get;set;}
        public string Question {get;set;} = null!;
        public string OptionA {get;set;} = null!;
        public string OptionB {get;set;} = null!;
        public string OptionC {get;set;} = null!;
        public string OptionD {get;set;} = null!;
        public string CorrectOption {get;set;} = null!;

        public int StageId {get;set;}
        public Stage Stage {get;set;} = null!;
    }


    public class Learn
    {
        public int Id {get;set;}
        public string Title {get;set;} = null!;
        public string Content {get;set;} = null!;

        public int StageId {get;set;}
        public Stage Stage {get;set;} = null!;
    }
}