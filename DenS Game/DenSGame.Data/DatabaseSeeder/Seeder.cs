using System.Data.Entity;
using System.Linq;
using DenSGame.Data.Models;

namespace DenSGame.Data.DatabaseSeeder
{
    public class Seeder : CreateDatabaseIfNotExists<DenSGameContext>
    {
        protected override void Seed(DenSGameContext dataContext)
        {
            base.Seed(dataContext);

            dataContext.Users.Add(new User
            {
                Name = "root",
                IsRoot = true
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
                Name = "Other"
            });
            dataContext.SaveChanges();

            AddQuestion(dataContext, "Vacations", "What would Dennis & Sandra like to do on a holiday?");
            AddQuestion(dataContext, "Vacations", "Which country will Dennis & Sandra visit next?");
            AddQuestion(dataContext, "Vacations", "What was Dennis & Sandra's latest holiday destination?");
            AddQuestion(dataContext, "Vacations", "In which country Sandra would not want to de found dead?");

            AddQuestion(dataContext, "Dates (M/V)", "What brand of condom Dennis & Sandra use?");
            AddQuestion(dataContext, "Dates (M/V)", "What would Sandra bring to a first date?");
            AddQuestion(dataContext, "Dates (M/V)", "What part of the female body Dennis most likes?");
            AddQuestion(dataContext, "Dates (M/V)", "What part of the male body Sandra most likes?");

            AddQuestion(dataContext, "Living / work / education", "What lies below Dennis & Sandra's bed?");
            AddQuestion(dataContext, "Living / work / education", "What is the name of Dennis' first school?");
            AddQuestion(dataContext, "Living / work / education", "What is the current mileage of Dennis' car?");
            AddQuestion(dataContext, "Living / work / education", "What is the one thing you can wake Sandra up for at night?");
            AddQuestion(dataContext, "Living / work / education", "What is the bigest booboo Dennis made at work?");

            AddQuestion(dataContext, "Hobbies", "What was Dennis' hobby when he was 11?");
            AddQuestion(dataContext, "Hobbies", "What is Sandra's favorite song?");
            AddQuestion(dataContext, "Hobbies", "What is Dennis' favorite movie?");

            dataContext.SaveChanges();
        }

        private void AddQuestion(DenSGameContext dataContext, string subject, string question)
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