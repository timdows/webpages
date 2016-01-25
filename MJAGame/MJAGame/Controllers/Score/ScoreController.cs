using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MJAGame.Data;

namespace MJAGame.Controllers.Score
{
    public class ScoreController : MJAGameController<MJAGameContext>
    {
        public JsonResult GetScores()
        {
            var users = _dataContext.Users
                .Where(a_item => !a_item.IsMarten)
                .ToList();

            var subjects = _dataContext.Subjects.ToList();

            var listScoreOverview = new List<ScoreOverview>();

            foreach (var user in users)
            {
                var scoreOverview = new ScoreOverview
                {
                    User = user
                };

                // Hack?
                try
                {
                    scoreOverview.TotalPoints = _dataContext.Scores
                        .Where(a_item => a_item.User.ID == user.ID)
                        .Sum(a_item => a_item.Points);
                }
                catch
                {
                    scoreOverview.TotalPoints = -1;
                }

                foreach (var subject in subjects)
                {
                    var scoreOverviewSubject = new ScoreOverviewSubject
                    {
                        Name = subject.Name
                    };

                    // Get all questions in this subject
                    var questionIDs = _dataContext.Questions
                        .Where(a_item => a_item.Subject.ID == subject.ID)
                        .Select(a_item => a_item.ID)
                        .ToList();

                    // See if the user has answered a question correct
                    var subjectPoints = _dataContext.Scores
                        .Where(a_item => a_item.User.ID == user.ID &&
                                         questionIDs.Contains(a_item.Question.ID))
                        .Select(a_item => a_item.Points)
                        .DefaultIfEmpty(0)
                        .Sum();

                    scoreOverviewSubject.Points = subjectPoints;

                    scoreOverview.Subjects.Add(scoreOverviewSubject);
                }

                listScoreOverview.Add(scoreOverview);
            }

            listScoreOverview = listScoreOverview
                .OrderByDescending(a_item => a_item.TotalPoints)
                .ToList();

            return Json(new {subjects, listScoreOverview});
        }
    }

    public class ScoreOverview
    {
        public ScoreOverview()
        {
            this.Subjects = new List<ScoreOverviewSubject>();
        }

        public Data.Models.User User { get; set; }
        public List<ScoreOverviewSubject> Subjects { get; set; }
        public long TotalPoints { get; set; }
    }

    public class ScoreOverviewSubject
    {
        public string Name { get; set; }
        public long Points { get; set; }
    }
}