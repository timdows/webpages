using System.Data.Entity;
using System.Linq;
using MJAGame.Data.Models;

namespace MJAGame.Data.DatabaseSeeder
{
    public class Seeder : CreateDatabaseIfNotExists<MJAGameContext>
    {
        protected override void Seed(MJAGameContext dataContext)
        {
            base.Seed(dataContext);

            dataContext.Users.Add(new User
            {
                Name = "root",
                IsMarten = true
            });
            dataContext.SaveChanges();

            dataContext.Subjects.Add(new Subject
            {
                Name = "Vacations"
            });
            dataContext.Subjects.Add(new Subject
            {
                Name = "Dates (M/V)"
            });
            dataContext.Subjects.Add(new Subject
            {
                Name = "Living / work / education"
            });
            dataContext.Subjects.Add(new Subject
            {
                Name = "Hobbies"
            });
            dataContext.Subjects.Add(new Subject
            {
                Name = "Challenges"
            });
            dataContext.Subjects.Add(new Subject
            {
                Name = "Other"
            });
            dataContext.SaveChanges();

            AddQuestion(dataContext, "Vacations", "What would Marten like to do on a holiday?");
            AddQuestion(dataContext, "Vacations", "Which country will Marten visit next?");
            AddQuestion(dataContext, "Vacations", "What was Martens latest holiday destination?");
            AddQuestion(dataContext, "Vacations", "In which country Marten would not want to de found dead?");
            AddQuestion(dataContext, "Vacations", "What was the name of Martens latest vacation flower?");

            AddQuestion(dataContext, "Dates (M/V)", "What brand of condom Marten uses?");
            AddQuestion(dataContext, "Dates (M/V)", "What would Marten bring to a first date?");
            AddQuestion(dataContext, "Dates (M/V)", "What part of the female body Marten most likes?");
            AddQuestion(dataContext, "Dates (M/V)", "What part of the male body Marten most likes?");

            AddQuestion(dataContext, "Challenges", "Can Marten do 10 pushups in 20 seconds?");
            AddQuestion(dataContext, "Challenges", "Can Marten do a handstand?");

            AddQuestion(dataContext, "Living / work / education", "What lies below Martens bed?");
            AddQuestion(dataContext, "Living / work / education", "What is the name of Martens first school?");
            AddQuestion(dataContext, "Living / work / education", "What is the current mileage of Martens car?");
            AddQuestion(dataContext, "Living / work / education", "What is the exact adres of Martens appartment?");
            AddQuestion(dataContext, "Living / work / education", "What is the one thing you can wake Marten up for at night?");
            AddQuestion(dataContext, "Living / work / education", "What is the bigest booboo Marten made at work?");

            AddQuestion(dataContext, "Hobbies", "What is the brand and model of Martens DSLR camera?");
            AddQuestion(dataContext, "Hobbies", "What is the latest drawing Marten made?");
            AddQuestion(dataContext, "Hobbies", "What was Martens hobby when he was 11?");
            AddQuestion(dataContext, "Hobbies", "What is Martens favorite song?");
            AddQuestion(dataContext, "Hobbies", "What is Martens favorite movie?");

            dataContext.SaveChanges();
        }

        private void AddQuestion(MJAGameContext dataContext, string subject, string question)
        {
            dataContext.Questions.Add(new Question
            {
                Subject = dataContext.Subjects.Single(a_item => a_item.Name == subject),
                SubmittedUser = dataContext.Users.Single(a_item => a_item.Name == "root"),
                QuestionString = question
            });
        }
    }
}